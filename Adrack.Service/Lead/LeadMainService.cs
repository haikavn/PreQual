// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadMainService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.ILeadMainService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadMainService" />
    public partial class LeadMainService : ILeadMainService
    {
        #region Constants

        /// <summary>
        /// Cache Lader By Id Key
        /// </summary>
        private const string CACHE_LEAD_BY_ID_KEY = "App.Cache.LeadMain.By.Id-{0}";

        private const string CACHE_LEADCONTENT_BY_ID_KEY = "App.Cache.LeadMainContent.By.Id-{0}";

        /// <summary>
        /// The cache lead geo data by lead identifier
        /// </summary>
        private const string CACHE_LEAD_GEO_DATA_BY_LEAD_ID = "App.Cache.LeadMain.GeoData.LeadId-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_ALL_KEY = "App.Cache.LeadMain.All";

        /// <summary>
        /// The cache dublicate lead key
        /// </summary>
        private const string CACHE_DUBLICATE_LEAD_KEY = "App.Cache.LeadMain.Dublicate.All";

        /// <summary>
        /// The cache lead error key
        /// </summary>
        private const string CACHE_LEAD_ERROR_KEY = "App.Cache.LeadMain.Errors.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_PATTERN_KEY = "App.Cache.LeadMain.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// LeadMain
        /// </summary>
        private readonly IRepository<LeadMain> _leadMainRepository;

        /// <summary>
        /// The lead geo data repository
        /// </summary>
        private readonly IRepository<LeadGeoData> _leadGeoDataRepository;

        /// <summary>
        /// LeadContent
        /// </summary>
        private readonly IRepository<LeadContent> _leadContentRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// IDataProvider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadMainRepository">The lead main repository.</param>
        /// <param name="leadContentRepository">The lead content repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="leadGeoDataRepository">The lead geo data repository.</param>
        public LeadMainService(
                                IRepository<LeadMain> leadMainRepository,
                                IRepository<LeadContent> leadContentRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher,
                                IDataProvider dataProvider,
                                IRepository<LeadGeoData> leadGeoDataRepository
                                )
        {
            this._leadMainRepository = leadMainRepository;
            this._leadContentRepository = leadContentRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._leadGeoDataRepository = leadGeoDataRepository;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead main by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadMain.</returns>
        public virtual LeadMain GetLeadMainById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _leadMainRepository.GetById(leadId); });
        }

        

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<LeadMain> GetAllLeads()
        {
            string key = CACHE_LEAD_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadMainRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Get Leads
        /// </summary>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>Lead Collection</returns>
        public virtual IList<LeadMain> GetLeads(int StartAt, int Count)
        {
                var query = from x in _leadMainRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.Take(Count).Skip(StartAt).ToList();

                return profiles;            
        }

        /// <summary>
        /// Get LeadsAll
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterEmail">The filter email.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterAffiliateChannelSubId">The filter affiliate channel sub identifier.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="FilterCampaign">The filter campaign.</param>
        /// <param name="Status">The status.</param>
        /// <param name="IP">The ip.</param>
        /// <param name="State">The state.</param>
        /// <param name="FilterFirstName">First name of the filter.</param>
        /// <param name="FilterLastName">Last name of the filter.</param>
        /// <param name="FilterBPrice">The filter b price.</param>
        /// <param name="FilterZipCode">The filter zip code.</param>
        /// <param name="Notes">The notes.</param>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>Lead Collection</returns>
        public virtual IList<LeadMainContent> GetLeadsAll(
                                            DateTime dateFrom,
                                            DateTime dateTo,
                                            long leadId,
                                            string FilterEmail,
                                            long FilterAffiliate,
                                            long FilterAffiliateChannel,
                                            string FilterAffiliateChannelSubId,
                                            long FilterBuyer,
                                            long FilterBuyerChannel,
                                            long FilterCampaign,
                                            short Status,
                                            string IP,
                                            string State,
                                            string FilterFirstName,
                                            string FilterLastName,
                                            decimal FilterBPrice,
                                            string FilterZipCode,
                                            string Notes,
                                            int StartAt,
                                            int Count)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = StartAt;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = Count;
            countParam.DbType = DbType.Int32;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "Status";
            statusParam.Value = Status;
            statusParam.DbType = DbType.Int16;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "State";
            stateParam.Value = State;
            stateParam.DbType = DbType.String;

            var ipParam = _dataProvider.GetParameter();
            ipParam.ParameterName = "IP";
            ipParam.Value = IP;
            ipParam.DbType = DbType.String;

            var emailParam = _dataProvider.GetParameter();
            emailParam.ParameterName = "Email";
            emailParam.Value = FilterEmail;
            emailParam.DbType = DbType.String;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliate;
            affiliateParam.DbType = DbType.Int64;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannel;
            affiliateChannelParam.DbType = DbType.Int64;

            var affiliateChannelSubIdParam = _dataProvider.GetParameter();
            affiliateChannelSubIdParam.ParameterName = "AffiliateChannelSubId";
            affiliateChannelSubIdParam.Value = FilterAffiliateChannelSubId;
            affiliateChannelSubIdParam.DbType = DbType.String;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyer;
            buyerParam.DbType = DbType.Int64;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannel;
            buyerChannelParam.DbType = DbType.Int64;

            var filterCampaignParam = _dataProvider.GetParameter();
            filterCampaignParam.ParameterName = "CampaignId";
            filterCampaignParam.Value = FilterCampaign;
            filterCampaignParam.DbType = DbType.Int64;

            var firstnameParam = _dataProvider.GetParameter();
            firstnameParam.ParameterName = "FirstName";
            firstnameParam.Value = FilterFirstName;
            firstnameParam.DbType = DbType.String;

            var lastnameParam = _dataProvider.GetParameter();
            lastnameParam.ParameterName = "LastName";
            lastnameParam.Value = FilterLastName;
            lastnameParam.DbType = DbType.String;

            var bpriceParam = _dataProvider.GetParameter();
            bpriceParam.ParameterName = "BPrice";
            bpriceParam.Value = FilterBPrice;
            bpriceParam.DbType = DbType.Decimal;

            var zipcodeParam = _dataProvider.GetParameter();
            zipcodeParam.ParameterName = "ZipCode";
            zipcodeParam.Value = FilterZipCode;
            zipcodeParam.DbType = DbType.String;

            var notesParam = _dataProvider.GetParameter();
            notesParam.ParameterName = "Notes";
            notesParam.Value = Notes;
            notesParam.DbType = DbType.String;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<LeadMainContent>("EXECUTE [dbo].[GetLeads] @dateFrom, @dateTo, @Email, @AffiliateId, @AffiliateChannelId, @AffiliateChannelSubId, @BuyerId, @BuyerChannelId, @CampaignId, @Status, @IP, @FirstName, @LastName, @BPrice, @ZipCode, @State, @start, @count, @leadId, @Notes",
                        dateFromParam, dateToParam, emailParam, affiliateParam, affiliateChannelParam, affiliateChannelSubIdParam, buyerParam, buyerChannelParam, filterCampaignParam, statusParam, ipParam, firstnameParam, lastnameParam, bpriceParam, zipcodeParam, stateParam, startParam, countParam, leadIdParam, notesParam).ToList();
            return result;
        }

        public virtual IList<LeadMainContent> GetLeadsAll(
                                            DateTime dateFrom,
                                            DateTime dateTo,
                                            long leadId,
                                            string FilterEmail,
                                            string FilterAffiliates,
                                            string FilterAffiliateChannels,
                                            string FilterAffiliateChannelSubId,
                                            string FilterBuyers,
                                            string FilterBuyerChannels,
                                            string FilterCampaigns,
                                            short Status,
                                            string IP,
                                            string State,
                                            string FilterFirstName,
                                            string FilterLastName,
                                            decimal FilterBPrice,
                                            string FilterZipCode,
                                            string Notes,
                                            int StartAt,
                                            int Count)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = StartAt;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = Count;
            countParam.DbType = DbType.Int32;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "Status";
            statusParam.Value = Status;
            statusParam.DbType = DbType.Int16;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "State";
            stateParam.Value = State;
            stateParam.DbType = DbType.String;

            var ipParam = _dataProvider.GetParameter();
            ipParam.ParameterName = "IP";
            ipParam.Value = IP;
            ipParam.DbType = DbType.String;

            var emailParam = _dataProvider.GetParameter();
            emailParam.ParameterName = "Email";
            emailParam.Value = FilterEmail;
            emailParam.DbType = DbType.String;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliates;
            affiliateParam.DbType = DbType.String;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannels;
            affiliateChannelParam.DbType = DbType.String;

            var affiliateChannelSubIdParam = _dataProvider.GetParameter();
            affiliateChannelSubIdParam.ParameterName = "AffiliateChannelSubId";
            affiliateChannelSubIdParam.Value = FilterAffiliateChannelSubId;
            affiliateChannelSubIdParam.DbType = DbType.String;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyers;
            buyerParam.DbType = DbType.String;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannels;
            buyerChannelParam.DbType = DbType.String;

            var filterCampaignParam = _dataProvider.GetParameter();
            filterCampaignParam.ParameterName = "CampaignId";
            filterCampaignParam.Value = FilterCampaigns;
            filterCampaignParam.DbType = DbType.String;

            var firstnameParam = _dataProvider.GetParameter();
            firstnameParam.ParameterName = "FirstName";
            firstnameParam.Value = FilterFirstName;
            firstnameParam.DbType = DbType.String;

            var lastnameParam = _dataProvider.GetParameter();
            lastnameParam.ParameterName = "LastName";
            lastnameParam.Value = FilterLastName;
            lastnameParam.DbType = DbType.String;

            var bpriceParam = _dataProvider.GetParameter();
            bpriceParam.ParameterName = "BPrice";
            bpriceParam.Value = FilterBPrice;
            bpriceParam.DbType = DbType.Decimal;

            var zipcodeParam = _dataProvider.GetParameter();
            zipcodeParam.ParameterName = "ZipCode";
            zipcodeParam.Value = FilterZipCode;
            zipcodeParam.DbType = DbType.String;

            var notesParam = _dataProvider.GetParameter();
            notesParam.ParameterName = "Notes";
            notesParam.Value = Notes;
            notesParam.DbType = DbType.String;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<LeadMainContent>("EXECUTE [dbo].[GetLeads2] @dateFrom, @dateTo, @Email, @AffiliateId, @AffiliateChannelId, @AffiliateChannelSubId, @BuyerId, @BuyerChannelId, @CampaignId, @Status, @IP, @FirstName, @LastName, @BPrice, @ZipCode, @State, @start, @count, @leadId, @Notes",
                        dateFromParam, dateToParam, emailParam, affiliateParam, affiliateChannelParam, affiliateChannelSubIdParam, buyerParam, buyerChannelParam, filterCampaignParam, statusParam, ipParam, firstnameParam, lastnameParam, bpriceParam, zipcodeParam, stateParam, startParam, countParam, leadIdParam, notesParam).ToList();
            return result;
        }

        /// <summary>
        /// Get Leads Count
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="FilterEmail">The filter email.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="FilterCampaign">The filter campaign.</param>
        /// <param name="Status">The status.</param>
        /// <param name="IP">The ip.</param>
        /// <param name="State">The state.</param>
        /// <param name="Notes">The notes.</param>
        /// <returns>long</returns>
        public virtual int GetLeadsCount(DateTime dateFrom,
                                            DateTime dateTo,
                                            string FilterEmail,
                                            long FilterAffiliate,
                                            long FilterAffiliateChannel,
                                            long FilterBuyer,
                                            long FilterBuyerChannel,
                                            long FilterCampaign,
                                            short Status,
                                            string IP,
                                            string State,
                                            string Notes)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "Status";
            statusParam.Value = Status;
            statusParam.DbType = DbType.Int16;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "State";
            stateParam.Value = State;
            stateParam.DbType = DbType.String;

            var ipParam = _dataProvider.GetParameter();
            ipParam.ParameterName = "IP";
            ipParam.Value = IP;
            ipParam.DbType = DbType.String;

            var emailParam = _dataProvider.GetParameter();
            emailParam.ParameterName = "Email";
            emailParam.Value = FilterEmail;
            emailParam.DbType = DbType.String;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliate;
            affiliateParam.DbType = DbType.Int64;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannel;
            affiliateChannelParam.DbType = DbType.Int64;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyer;
            buyerParam.DbType = DbType.Int64;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannel;
            buyerChannelParam.DbType = DbType.Int64;

            var filterCampaignParam = _dataProvider.GetParameter();
            filterCampaignParam.ParameterName = "CampaignId";
            filterCampaignParam.Value = FilterCampaign;
            filterCampaignParam.DbType = DbType.Int64;

            var notesParam = _dataProvider.GetParameter();
            notesParam.ParameterName = "Notes";
            notesParam.Value = Notes;
            notesParam.DbType = DbType.String;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetLeadsCount] @dateFrom, @dateTo, @Email, @AffiliateId, @AffiliateChannelId, @BuyerId, @BuyerChannelId, @CampaignId, @Status, @IP, @State, @Notes",
                        dateFromParam, dateToParam, emailParam, affiliateParam, affiliateChannelParam, buyerParam, buyerChannelParam, filterCampaignParam, statusParam, ipParam, stateParam, notesParam).FirstOrDefault();

            return result;

            /*
            var query = from x in _leadMainRepository.Table
                        where x.Created >= DateFrom && x.Created <= DateTo
                        select x.Id;
            return query.ToList().Count;
            */
        }

        public virtual int GetLeadsCount(DateTime dateFrom,
                                            DateTime dateTo,
                                            string FilterEmail,
                                            string FilterAffiliate,
                                            string FilterAffiliateChannel,
                                            string FilterAffiliateChannelSubId,
                                            string FilterBuyer,
                                            string FilterBuyerChannel,
                                            string FilterCampaign,
                                            short Status,
                                            string IP,
                                            string State,
                                            string FilterFirstName,
                                            string FilterLastName,
                                            decimal FilterBPrice,
                                            string FilterZipCode,
                                            string Notes)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "Status";
            statusParam.Value = Status;
            statusParam.DbType = DbType.Int16;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "State";
            stateParam.Value = State;
            stateParam.DbType = DbType.String;

            var ipParam = _dataProvider.GetParameter();
            ipParam.ParameterName = "IP";
            ipParam.Value = IP;
            ipParam.DbType = DbType.String;

            var emailParam = _dataProvider.GetParameter();
            emailParam.ParameterName = "Email";
            emailParam.Value = FilterEmail;
            emailParam.DbType = DbType.String;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliate;
            affiliateParam.DbType = DbType.String;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannel;
            affiliateChannelParam.DbType = DbType.String;

            var affiliateChannelSubIdParam = _dataProvider.GetParameter();
            affiliateChannelSubIdParam.ParameterName = "AffiliateChannelSubId";
            affiliateChannelSubIdParam.Value = FilterAffiliateChannelSubId;
            affiliateChannelSubIdParam.DbType = DbType.String;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyer;
            buyerParam.DbType = DbType.String;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannel;
            buyerChannelParam.DbType = DbType.String;

            var filterCampaignParam = _dataProvider.GetParameter();
            filterCampaignParam.ParameterName = "CampaignId";
            filterCampaignParam.Value = FilterCampaign;
            filterCampaignParam.DbType = DbType.String;


            var firstnameParam = _dataProvider.GetParameter();
            firstnameParam.ParameterName = "FirstName";
            firstnameParam.Value = FilterFirstName;
            firstnameParam.DbType = DbType.String;

            var lastnameParam = _dataProvider.GetParameter();
            lastnameParam.ParameterName = "LastName";
            lastnameParam.Value = FilterLastName;
            lastnameParam.DbType = DbType.String;

            var bpriceParam = _dataProvider.GetParameter();
            bpriceParam.ParameterName = "BPrice";
            bpriceParam.Value = FilterBPrice;
            bpriceParam.DbType = DbType.Decimal;

            var zipcodeParam = _dataProvider.GetParameter();
            zipcodeParam.ParameterName = "ZipCode";
            zipcodeParam.Value = FilterZipCode;
            zipcodeParam.DbType = DbType.String;

            var notesParam = _dataProvider.GetParameter();
            notesParam.ParameterName = "Notes";
            notesParam.Value = Notes;
            notesParam.DbType = DbType.String;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetLeadsCount2] @dateFrom, @dateTo, @Email, @AffiliateId, @AffiliateChannelId, @AffiliateChannelSubId, @BuyerId, @BuyerChannelId, @CampaignId, @Status, @IP, @FirstName, @LastName, @BPrice, @ZipCode, @State, @Notes",
                        dateFromParam, dateToParam, emailParam, affiliateParam, affiliateChannelParam, affiliateChannelSubIdParam, buyerParam, buyerChannelParam, filterCampaignParam, statusParam, ipParam, firstnameParam, lastnameParam, bpriceParam, zipcodeParam, stateParam, notesParam).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Get LeadsAll
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>Lead Collection</returns>
        public virtual int GetLeadsCountByDay(long buyerChannelId, DateTime date)
        {
            var dateParam = _dataProvider.GetParameter();
            dateParam.ParameterName = "datetime";
            dateParam.Value = date;
            dateParam.DbType = DbType.DateTime;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = buyerChannelId;
            buyerChannelParam.DbType = DbType.Int64;

            return _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetLeadsCountByDay] @buyerchannelid, @datetime", buyerChannelParam, dateParam).FirstOrDefault();
        }

        /// <summary>
        /// Gets the processing leads.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetProcessingLeads()
        {
            return _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetProcessingLeads]").FirstOrDefault();
        }

        /// <summary>
        /// Get LeadsAll
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>Lead Collection</returns>
        public virtual LeadMainContent GetLeadsAllById(long leadId)
        {
            string key = string.Format(CACHE_LEADCONTENT_BY_ID_KEY, leadId);

            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "leadId";
            IdParam.Value = leadId;
            IdParam.DbType = DbType.Int64;

            return _cacheManager.Get(key, () =>
            {
                var result = _leadMainRepository.GetDbClientContext().SqlQuery<LeadMainContent>("EXECUTE [dbo].[GetLeadById] @leadId", IdParam).ToList();

                return result.FirstOrDefault();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual long InsertLeadMain(LeadMain leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Insert(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadMain);

            return leadMain.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void UpdateLeadMain(LeadMain leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Update(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadMain);
        }

        /// <summary>
        /// Delete LeadMain
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void DeleteLeadMain(LeadMain leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Delete(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(leadMain);
        }

        /// <summary>
        /// Get Leads GeoData by ID
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadGeoData</returns>
        public virtual LeadGeoData GetLeadGeoData(long leadId)
        {
            string key = string.Format(CACHE_LEAD_GEO_DATA_BY_LEAD_ID, leadId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadGeoDataRepository.Table
                            where x.LeadId == leadId
                            select x;

                return query.FirstOrDefault();
            });
        }

        /// <summary>
        /// GetBadIPClicksReport
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="LeadIP">The lead ip.</param>
        /// <param name="ClickIP">The click ip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>LeadGeoData</returns>

        public virtual List<ReportBadIPClicks> GetBadIPClicksReport(
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            string FilterAffiliates,
                            string LeadIP,
                            string ClickIP,
                            int page,
                            int pageSize)
        {            

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var leadIpParam = _dataProvider.GetParameter();
            leadIpParam.ParameterName = "LeadIP";
            leadIpParam.Value = LeadIP;
            leadIpParam.DbType = DbType.String;

            var clickIpParam = _dataProvider.GetParameter();
            clickIpParam.ParameterName = "ClickIP";
            clickIpParam.Value = ClickIP;
            clickIpParam.DbType = DbType.String;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateIds";
            affiliateParam.Value = FilterAffiliates;
            affiliateParam.DbType = DbType.String; ;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = page;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = pageSize;
            countParam.DbType = DbType.Int32;

            
            var result = _leadMainRepository.GetDbClientContext().SqlQuery<ReportBadIPClicks>("EXECUTE [dbo].[GetBadIPClicksReport]  @dateFrom, @dateTo, @AffiliateIds, @LeadIP, @ClickIP, @start, @count, @leadId",
                    dateFromParam, dateToParam, affiliateParam, leadIpParam, clickIpParam, startParam, countParam, leadIdParam);

            return result.ToList();            
        }

        /// <summary>
        /// GetErrorLeadsReport
        /// </summary>
        /// <param name="ErrorType">Type of the error.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="ReportType">Type of the report.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>LeadGeoData</returns>
        public virtual List<ReportErrorLeads> GetErrorLeadsReport(
                            short ErrorType,
                            short validator,
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            short status,
                            long FilterAffiliate,
                            long FilterAffiliateChannel,
                            long FilterBuyer,
                            long FilterBuyerChannel,
                            long FilterCampaign,
                            string FilterState,
                            decimal FilterMinPrice,
                            short ReportType,
                            int page,
                            int pageSize)
        {
            var errorTypeParam = _dataProvider.GetParameter();
            errorTypeParam.ParameterName = "errorType";
            errorTypeParam.Value = ErrorType;
            errorTypeParam.DbType = DbType.Int16;

            var validatorParam = _dataProvider.GetParameter();
            validatorParam.ParameterName = "validator";
            validatorParam.Value = validator;
            validatorParam.DbType = DbType.Int16;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "status";
            statusParam.Value = status;
            statusParam.DbType = DbType.Int16;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliate;
            affiliateParam.DbType = DbType.Int64;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannel;
            affiliateChannelParam.DbType = DbType.Int64;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyer;
            buyerParam.DbType = DbType.Int64;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannel;
            buyerChannelParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "CampaignId";
            campaignParam.Value = FilterCampaign;
            campaignParam.DbType = DbType.Int64;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "state";
            stateParam.Value = FilterState;
            stateParam.DbType = DbType.String;

            var minPriceParam = _dataProvider.GetParameter();
            minPriceParam.ParameterName = "minPrice";
            minPriceParam.Value = FilterMinPrice;
            minPriceParam.DbType = DbType.Double;

            var reportTypeParam = _dataProvider.GetParameter();
            reportTypeParam.ParameterName = "reportType";
            reportTypeParam.Value = ReportType;
            reportTypeParam.DbType = DbType.Int16;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = page;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = pageSize;
            countParam.DbType = DbType.Int32;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<ReportErrorLeads>("EXECUTE [dbo].[GetErrorLeadsReport]  @errorType, @validator, @dateFrom, @dateTo, @AffiliateId, @AffiliateChannelId, @BuyerId, @BuyerChannelId, @CampaignId, @state, @minPrice, @reportType, @leadId, @status, @start, @count",
                errorTypeParam, validatorParam, dateFromParam, dateToParam, affiliateParam, affiliateChannelParam, buyerParam, buyerChannelParam, campaignParam, stateParam, minPriceParam, reportTypeParam, leadIdParam, statusParam, startParam, countParam);

            return result.ToList();
        }

        public virtual List<ReportErrorLeads> GetErrorLeadsReport(
                            short ErrorType,
                            short validator,
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            short status,
                            List<long> FilterAffiliate,
                            List<long> FilterAffiliateChannel,
                            List<long> FilterBuyer,
                            List<long> FilterBuyerChannel,
                            List<long> FilterCampaign,
                            List<string> FilterState,
                            decimal FilterMinPrice,
                            short ReportType,
                            int page,
                            int pageSize)
        {
            var errorTypeParam = _dataProvider.GetParameter();
            errorTypeParam.ParameterName = "errorType";
            errorTypeParam.Value = ErrorType;
            errorTypeParam.DbType = DbType.Int16;

            var validatorParam = _dataProvider.GetParameter();
            validatorParam.ParameterName = "validator";
            validatorParam.Value = validator;
            validatorParam.DbType = DbType.Int16;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "status";
            statusParam.Value = status;
            statusParam.DbType = DbType.Int16;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = string.Join(",", FilterAffiliate);
            affiliateParam.DbType = DbType.String;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = string.Join(",", FilterAffiliateChannel);
            affiliateChannelParam.DbType = DbType.String;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = string.Join(",", FilterBuyer);
            buyerParam.DbType = DbType.String;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = string.Join(",", FilterBuyerChannel);
            buyerChannelParam.DbType = DbType.String;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "CampaignId";
            campaignParam.Value = string.Join(",", FilterCampaign);
            campaignParam.DbType = DbType.String;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "state";
            stateParam.Value = string.Join(",", FilterState);
            stateParam.DbType = DbType.String;

            var minPriceParam = _dataProvider.GetParameter();
            minPriceParam.ParameterName = "minPrice";
            minPriceParam.Value = FilterMinPrice;
            minPriceParam.DbType = DbType.Double;

            var reportTypeParam = _dataProvider.GetParameter();
            reportTypeParam.ParameterName = "reportType";
            reportTypeParam.Value = ReportType;
            reportTypeParam.DbType = DbType.Int16;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = page;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = pageSize;
            countParam.DbType = DbType.Int32;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<ReportErrorLeads>("EXECUTE [dbo].[GetErrorLeadsReport]  @errorType, @validator, @dateFrom, @dateTo, @AffiliateId, @AffiliateChannelId, @BuyerId, @BuyerChannelId, @CampaignId, @state, @minPrice, @reportType, @leadId, @status, @start, @count",
                errorTypeParam, validatorParam, dateFromParam, dateToParam, affiliateParam, affiliateChannelParam, buyerParam, buyerChannelParam, campaignParam, stateParam, minPriceParam, reportTypeParam, leadIdParam, statusParam, startParam, countParam);

            return result.ToList();
        }

        public virtual int GetErrorLeadsReportCount(
                            short ErrorType,
                            short validator,
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            short status,
                            long FilterAffiliate,
                            long FilterAffiliateChannel,
                            long FilterBuyer,
                            long FilterBuyerChannel,
                            long FilterCampaign,
                            string FilterState,
                            decimal FilterMinPrice,
                            short ReportType)
        {
            var errorTypeParam = _dataProvider.GetParameter();
            errorTypeParam.ParameterName = "errorType";
            errorTypeParam.Value = ErrorType;
            errorTypeParam.DbType = DbType.Int16;

            var validatorParam = _dataProvider.GetParameter();
            validatorParam.ParameterName = "validator";
            validatorParam.Value = validator;
            validatorParam.DbType = DbType.Int16;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "status";
            statusParam.Value = status;
            statusParam.DbType = DbType.Int16;

            var affiliateParam = _dataProvider.GetParameter();
            affiliateParam.ParameterName = "AffiliateId";
            affiliateParam.Value = FilterAffiliate;
            affiliateParam.DbType = DbType.Int64;

            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "AffiliateChannelId";
            affiliateChannelParam.Value = FilterAffiliateChannel;
            affiliateChannelParam.DbType = DbType.Int64;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = FilterBuyer;
            buyerParam.DbType = DbType.Int64;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = FilterBuyerChannel;
            buyerChannelParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "CampaignId";
            campaignParam.Value = FilterCampaign;
            campaignParam.DbType = DbType.Int64;

            var stateParam = _dataProvider.GetParameter();
            stateParam.ParameterName = "state";
            stateParam.Value = FilterState;
            stateParam.DbType = DbType.String;

            var minPriceParam = _dataProvider.GetParameter();
            minPriceParam.ParameterName = "minPrice";
            minPriceParam.Value = FilterMinPrice;
            minPriceParam.DbType = DbType.Double;

            var reportTypeParam = _dataProvider.GetParameter();
            reportTypeParam.ParameterName = "reportType";
            reportTypeParam.Value = ReportType;
            reportTypeParam.DbType = DbType.Int16;

            var result = _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetErrorLeadsReportCount]  @errorType, @validator, @dateFrom, @dateTo, @AffiliateId, @AffiliateChannelId, @BuyerId, @BuyerChannelId, @CampaignId, @state, @minPrice, @reportType, @leadId, @status",
                errorTypeParam, validatorParam, dateFromParam, dateToParam, affiliateParam, affiliateChannelParam, buyerParam, buyerChannelParam, campaignParam, stateParam, minPriceParam, reportTypeParam, leadIdParam, statusParam);

            return result.FirstOrDefault();
        }


        /// <summary>
        /// Gets the lead count by buyer.
        /// </summary>
        /// <param name="created">The created.</param>
        /// <param name="buyerid">The buyerid.</param>
        /// <returns>System.Int32.</returns>
        public virtual int GetLeadCountByBuyer(DateTime created, long buyerid)
        {
            var createdParam = _dataProvider.GetParameter();
            createdParam.ParameterName = "created";
            createdParam.Value = created;
            createdParam.DbType = DbType.DateTime;

            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "buyerid";
            buyerIdParam.Value = buyerid;
            buyerIdParam.DbType = DbType.Int64;

            return _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetLeadCountByBuyer] @created, @buyerid", createdParam, buyerIdParam).FirstOrDefault();
        }

        /// <summary>
        /// Clears the sensitive data.
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[ClearSensitiveData]").FirstOrDefault();
        }


        // <summary>
        /// <summary>
        /// Gets the next and previous leads Ids.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadIds</returns>
        public virtual long[] GetNextPrevLeadId(long leadId)
        {
            if (leadId == 0)
                return null;

            var nextLeadId = (from x in _leadMainRepository.Table
                        where x.Id > leadId
                        select x.Id).Take(1).FirstOrDefault();

            var prevLeadId = (from x in _leadMainRepository.Table
                      where x.Id < leadId
                      orderby x.Id descending
                      select x.Id).Take(1).FirstOrDefault();

            long[] nextPrev = { nextLeadId, prevLeadId };

            return nextPrev;
        }

        #endregion Methods
    }
}