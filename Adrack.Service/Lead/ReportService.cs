// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ReportService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Cache;
using Adrack.Core.Domain.CustomReports;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Data.Domain;
using System.Data.Entity;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Adrack.Core.Domain.Dashboard;
using Adrack.Core;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class ReportService.
    /// Implements the <see cref="Adrack.Service.Lead.IReportService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IReportService" />
    public partial class ReportService : IReportService
    {
        #region Constants

        private const string CACHE_REPORTS_VIEWED_GetReportsViewedByUserId = "App.Cache.ReportsViewed.GetReportsViewedByUserId-{0}";

        private const string CACHE_REPORTS_VIEWED_PATTERN_KEY = "App.Cache.ReportsViewed.";

        private readonly Core.IAppContext _appContext;
        #endregion Constants


        #region Fields

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        private readonly IRepository<Core.Domain.CustomReports.ReportType> _reportTypesRepository;
        private readonly IRepository<ReportVariable> _reportTypeVariableRepository;
        private readonly IRepository<ReportVariableType> _reportVariableTypesRepository;

        private readonly IRepository<ReportFilterSetting> _reportSettingsRepository;
        private readonly IRepository<ReportsViewed> _reportsViewedRepository;

        private readonly IRepository<UserReport> _userReportsRepository;

        private readonly ICampaignService _campaignService;
        private readonly IBuyerService _buyerService;
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerChannelService _buyerChannelService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public ReportService(ICacheManager cacheManager, IAppEventPublisher appEventPublisher,
            IRepository<ReportVariableType> _reportVariableTypesRepository
            , IRepository<ReportVariable> reportTypeVariableRepository
            , IRepository<Core.Domain.CustomReports.ReportType> reportTypesRepository
            , IRepository<ReportFilterSetting> reportSettingsRepository
            , IRepository<ReportsViewed> reportsViewedRepository
            , IRepository<UserReport> userReportsRepository
            , IDataProvider dataProvider
            , Core.IAppContext appContext
            , ICampaignService campaignService
            , IBuyerService buyerService
            , IAffiliateService affiliateService
            , IAffiliateChannelService affiliateChannelService
            , IBuyerChannelService buyerChannelService)
        {
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._reportTypesRepository = reportTypesRepository;
            this._reportTypeVariableRepository = reportTypeVariableRepository;
            this._reportVariableTypesRepository = _reportVariableTypesRepository;
            this._reportSettingsRepository = reportSettingsRepository;
            this._reportsViewedRepository = reportsViewedRepository;
            this._userReportsRepository = userReportsRepository;
            this._appContext = appContext;

            this._campaignService = campaignService;
            this._buyerService = buyerService;
            this._affiliateService = affiliateService;
            this._affiliateChannelService = affiliateChannelService;
            this._buyerChannelService = buyerChannelService;
    }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Buyers the report by buyer channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;BuyerReportByBuyerChannel&gt;.</returns>
        public virtual IList<BuyerReportByBuyerChannel> BuyerReportByBuyerChannels(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<BuyerReportByBuyerChannel>("EXECUTE [dbo].[BuyerReportByBuyerChannels] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Buyers the report by hour.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;BuyerReportByHour&gt;.</returns>
        public virtual IList<BuyerReportByHour> BuyerReportByHour(DateTime date, string buyerChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "date";
            startParam.Value = date;
            startParam.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<BuyerReportByHour>("EXECUTE [dbo].[BuyerReportByHour] @date,@buyerChannels,@campaigns", startParam, buyerChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by campaigns.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByCampaigns&gt;.</returns>
        public virtual IList<ReportBuyersByCampaigns> ReportBuyersByCampaigns(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByCampaigns>("EXECUTE [dbo].[ReportBuyersByCampaigns] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by affiliate channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByAffiliateChannels&gt;.</returns>
        public virtual IList<ReportBuyersByAffiliateChannels> ReportBuyersByAffiliateChannels(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByAffiliateChannels>("EXECUTE [dbo].[ReportBuyersByAffiliateChannels] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="states">The states.</param>
        /// <returns>IList&lt;ReportBuyersByStates&gt;.</returns>
        public virtual IList<ReportBuyersByStates> ReportBuyersByStates(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns, string states)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            var statesParam = _dataProvider.GetParameter();
            statesParam.ParameterName = "states";
            statesParam.Value = states;
            statesParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByStates>("EXECUTE [dbo].[ReportBuyersByStates] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns,@states", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam, statesParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by dates.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByDates&gt;.</returns>
        public virtual IList<ReportBuyersByDates> ReportBuyersByDates(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByDates>("EXECUTE [dbo].[ReportBuyersByDates] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by lead notes.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByLeadNotes&gt;.</returns>
        public virtual IList<ReportBuyersByLeadNotes> ReportBuyersByLeadNotes(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannels";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByLeadNotes>("EXECUTE [dbo].[ReportBuyersByLeadNotes] @start,@end,@buyers,@buyerChannels,@affiliateChannels,@campaigns", startParam, endParam, buyersParam, buyerChannelsParam, affiliateChannelsParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by reaction time.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerid">The buyerid.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByReactionTime&gt;.</returns>
        public virtual IList<ReportBuyersByReactionTime> ReportBuyersByReactionTime(DateTime start, DateTime end, long buyerid, string campaigns)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyerid";
            buyersParam.Value = buyerid;
            buyersParam.DbType = DbType.Int64;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByReactionTime>("EXECUTE [dbo].[ReportBuyersByReactionTime] @start,@end,@buyerid,@campaigns", startParam, endParam, buyersParam, campaignsParam).ToList();
        }

        /// <summary>
        /// Reports the buyers comparison.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <param name="date3">The date3.</param>
        /// <returns>IList&lt;ReportBuyersComparison&gt;.</returns>
        public virtual IList<ReportBuyersComparison> ReportBuyersComparison(long buyerId, long buyerChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3)
        {
            var date1Param = _dataProvider.GetParameter();
            date1Param.ParameterName = "date1";
            date1Param.Value = date1;
            date1Param.DbType = DbType.DateTime;

            var date2Param = _dataProvider.GetParameter();
            date2Param.ParameterName = "date2";
            date2Param.Value = date2;
            date2Param.DbType = DbType.DateTime;

            var date3Param = _dataProvider.GetParameter();
            date3Param.ParameterName = "date3";
            date3Param.Value = date3;
            date3Param.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "buyerId";
            buyersParam.Value = buyerId;
            buyersParam.DbType = DbType.Int64;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "BuyerChannelId";
            buyerChannelsParam.Value = buyerChannelId;
            buyerChannelsParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignId";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersComparison>("EXECUTE [dbo].[ReportBuyersComparison] @buyerid,@buyerchannelid,@campaignId,@date1,@date2,@date3", buyersParam, buyerChannelsParam, campaignParam, date1Param, date2Param, date3Param).ToList();
        }

        public virtual IList<ReportBuyersComparison> ReportAffiliatesComparison(long affiliateId, long affiliateChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3)
        {
            var date1Param = _dataProvider.GetParameter();
            date1Param.ParameterName = "date1";
            date1Param.Value = date1;
            date1Param.DbType = DbType.DateTime;

            var date2Param = _dataProvider.GetParameter();
            date2Param.ParameterName = "date2";
            date2Param.Value = date2;
            date2Param.DbType = DbType.DateTime;

            var date3Param = _dataProvider.GetParameter();
            date3Param.ParameterName = "date3";
            date3Param.Value = date3;
            date3Param.DbType = DbType.DateTime;

            var affiliatesParam = _dataProvider.GetParameter();
            affiliatesParam.ParameterName = "affiliateid";
            affiliatesParam.Value = affiliateId;
            affiliatesParam.DbType = DbType.Int64;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliatechannelid";
            affiliateChannelsParam.Value = affiliateChannelId;
            affiliateChannelsParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignid";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersComparison>("EXECUTE [dbo].[ReportAffiliatesComparison] @affiliateid,@affiliatechannelid,@campaignId,@date1,@date2,@date3", affiliatesParam, affiliateChannelsParam, campaignParam, date1Param, date2Param, date3Param).ToList();
        }


        /// <summary>
        /// Reports the buyer channel comparison.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <param name="date3">The date3.</param>
        /// <returns>IList&lt;ReportBuyersComparison&gt;.</returns>
        public virtual IList<ReportBuyersComparison> ReportBuyersComparisonBuyerChannels(long buyerChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3)
        {
            var date1Param = _dataProvider.GetParameter();
            date1Param.ParameterName = "date1";
            date1Param.Value = date1;
            date1Param.DbType = DbType.DateTime;

            var date2Param = _dataProvider.GetParameter();
            date2Param.ParameterName = "date2";
            date2Param.Value = date2;
            date2Param.DbType = DbType.DateTime;

            var date3Param = _dataProvider.GetParameter();
            date3Param.ParameterName = "date3";
            date3Param.Value = date3;
            date3Param.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "BuyerChannelId";
            buyerChannelsParam.Value = buyerChannelId;
            buyerChannelsParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignId";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersComparison>("EXECUTE [dbo].[ReportBuyersComparisonBuyerChannels] @buyerchannelid,@campaignId,@date1,@date2,@date3", buyerChannelsParam, campaignParam, date1Param, date2Param, date3Param).ToList();
        }


        /// <summary>
        /// Reports the buyers by prices.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="fromPrice">From price.</param>
        /// <param name="toPrice">To price.</param>
        /// <returns>IList&lt;ReportBuyersByPrices&gt;.</returns>
        public virtual IList<ReportBuyersByPrices> ReportBuyersByPrices(DateTime start, DateTime end, string buyers, string buyerChannels, string campaigns, decimal fromPrice, decimal toPrice)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "Start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "End";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "Buyers";
            buyersParam.Value = buyers;
            buyersParam.DbType = DbType.String;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "BuyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var campaignsParam = _dataProvider.GetParameter();
            campaignsParam.ParameterName = "Campaigns";
            campaignsParam.Value = campaigns;
            campaignsParam.DbType = DbType.String;

            var fromPriceParam = _dataProvider.GetParameter();
            fromPriceParam.ParameterName = "FromPrice";
            fromPriceParam.Value = fromPrice;
            fromPriceParam.DbType = DbType.Currency;

            var toPriceParam = _dataProvider.GetParameter();
            toPriceParam.ParameterName = "ToPrice";
            toPriceParam.Value = toPrice;
            toPriceParam.DbType = DbType.Currency;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByPrices>("EXECUTE [dbo].[ReportBuyersByPrices] @start,@end,@buyers,@buyerChannels,@campaigns,@fromprice,@toprice", startParam, endParam, buyersParam, buyerChannelsParam, campaignsParam, fromPriceParam, toPriceParam).ToList();
        }

        /// <summary>
        /// Reports the buyers by trffic estimate.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="values">The values.</param>
        /// <param name="valueTypes">The value types.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="excludes">The excludes.</param>
        /// <returns>IList&lt;ReportBuyersByTrafficEstimate&gt;.</returns>
        public virtual IList<ReportBuyersByTrafficEstimate> ReportBuyersByTrfficEstimate(DateTime start, DateTime end, string buyerChannels, string campaigns, List<string> fields, List<string> values, List<short> valueTypes, List<short> conditions, List<bool> excludes)
        {
            string sqlQuery = "SELECT  Buyer.Id AS BuyerId, Buyer.Name AS BuyerName, BuyerChannel.Id AS BuyerChannelId, BuyerChannel.Name AS BuyerChannelName, cast(lr.Created as Date) as Created, count(lr.leadid) as Quantity, count(distinct lr.leadid) as UQuantity FROM LeadMainResponse lr INNER JOIN Buyer ON Buyer.Id = lr.BuyerId INNER JOIN BuyerChannel ON BuyerChannel.Id = lr.BuyerChannelId INNER JOIN LeadContent lc on lc.LeadId = lr.LeadId";

            //if (fields.Count > 0)
            {
                sqlQuery += " and (lr.Status <= 3";
            }

            for (int i = 0; i < fields.Count; i++)
            {
                string[] valuesStr = values[i].Split(new char[1] { ',' });

                switch (valueTypes[i])
                {
                    case 0:
                    case 1:
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 11:
                    case 12:
                        if (valuesStr[0].Length > 0)
                        {
                            switch (conditions[i])
                            {
                                case 1: sqlQuery += " and (lc." + fields[i] + (excludes[i] ? " !" : "") + "='" + valuesStr[0] + "')"; break;
                                case 2: sqlQuery += " and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '" + valuesStr[0] + "%')"; break;
                                case 3: sqlQuery += " and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '%" + valuesStr[0] + "')"; break;
                                case 4: sqlQuery += " and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '%" + valuesStr[0] + "%')"; break;
                            }
                        }
                        break;

                    case 2:
                    case 4:
                        if (valuesStr.Length > 1)
                        {
                            int val1 = 0;
                            int val2 = 0;

                            int.TryParse(valuesStr[0], out val1);
                            int.TryParse(valuesStr[1], out val2);
                            if (val1 > 0 || val2 > 0)
                            {
                                sqlQuery += " and ";
                                sqlQuery += "(lc." + fields[i] + " between " + val1.ToString() + " and " + val2.ToString() + ")";
                            }
                        }
                        break;

                    case 16:
                        if (valuesStr.Length > 1 && valuesStr[0].Length > 0 && valuesStr[0].Length > 0)
                        {
                            decimal val1 = 0;
                            decimal val2 = 0;

                            decimal.TryParse(valuesStr[0], out val1);
                            decimal.TryParse(valuesStr[1], out val2);
                            if (val1 > 0 || val2 > 0)
                            {
                                sqlQuery += " and ";
                                sqlQuery += "(lc." + fields[i] + " between " + val1.ToString() + " and " + val2.ToString() + ")";
                            }
                        }
                        break;

                    default:
                        if (valuesStr[0].Length > 0)
                        {
                            switch (conditions[i])
                            {
                                case 1: sqlQuery += " and (lc." + fields[i] + (excludes[i] ? " !" : "") + "='" + valuesStr[0] + "')"; break;
                                case 2: sqlQuery += "and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '" + valuesStr[0] + "%')"; break;
                                case 3: sqlQuery += "and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '%" + valuesStr[0] + "')"; break;
                                case 4: sqlQuery += "and (lc." + fields[i] + (excludes[i] ? " not " : "") + " like '%" + valuesStr[0] + "%')"; break;
                            }
                        }
                        break;
                }
            }

            //if (fields.Count > 0)
            {
                sqlQuery += ") ";
            }

            sqlQuery += " where lr.id > 0 ";

            if (buyerChannels.Length > 0)
            {
                sqlQuery += " and lr.BuyerChannelId IN (" + buyerChannels + ")";
            }

            if (campaigns.Length > 0)
            {
                sqlQuery += " and lr.CampaignId IN (" + campaigns + ")";
            }

            sqlQuery += " AND lr.Created BETWEEN '" + start.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "' AND '" + end.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
            sqlQuery += " GROUP BY Buyer.Id, Buyer.Name, BuyerChannel.Id, BuyerChannel.Name, cast(lr.created as Date) ORDER BY Buyer.name";

            var sqlQueryParam = _dataProvider.GetParameter();
            sqlQueryParam.ParameterName = "sqlQuery";
            sqlQueryParam.Value = sqlQuery;
            sqlQueryParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByTrafficEstimate>("EXECUTE [dbo].[ReportBuyersTrafficEstimator] @sqlQuery", sqlQueryParam).ToList();
        }

        /// <summary>
        /// Reports the buyers win rate report.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <returns>IList&lt;ReportBuyersByStates&gt;.</returns>
        public virtual IList<ReportBuyersWinRateReport> ReportBuyersWinRateReport(DateTime start, DateTime end, string buyerChannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersWinRateReport>("EXECUTE [dbo].[ReportBuyersWinRateReport] @start,@end,@buyerChannels", startParam, endParam, buyerChannelsParam).ToList();
        }

        public virtual IList<ReportBuyersConversionAnalysys> ReportConversionAnalysys(DateTime start, DateTime end, string buyerChannels, string type)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var typeParam = _dataProvider.GetParameter();
            typeParam.ParameterName = "type";
            typeParam.Value = type;
            typeParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersConversionAnalysys>("EXECUTE [dbo].[ReportConversionAnalysys] @start,@end,@buyerChannels,@type", startParam, endParam, buyerChannelsParam, typeParam).ToList();
        }

        public virtual IList<ReportSendingTime> ReportSendingTime(DateTime start, DateTime end, string campaignIds)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var campaignIdsParam = _dataProvider.GetParameter();
            campaignIdsParam.ParameterName = "campaigns";
            campaignIdsParam.Value = campaignIds;
            campaignIdsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportSendingTime>("EXECUTE [dbo].[ReportSendingTime] @start,@end,@campaigns", startParam, endParam, campaignIdsParam).ToList();
        }


        public virtual IList<ReportSendingTime> ReportSendingTimeByFilter(DateTime start, DateTime end, string campaignIds, string buyerIds, string buyerChannelIds)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var campaignIdsParam = _dataProvider.GetParameter();
            campaignIdsParam.ParameterName = "campaigns";
            campaignIdsParam.Value = campaignIds;
            campaignIdsParam.DbType = DbType.String;

            var buyerIdsParam = _dataProvider.GetParameter();
            buyerIdsParam.ParameterName = "buyers";
            buyerIdsParam.Value = buyerIds;
            buyerIdsParam.DbType = DbType.String;

            var buyerChannelIdsParam = _dataProvider.GetParameter();
            buyerChannelIdsParam.ParameterName = "buyerChannels";
            buyerChannelIdsParam.Value = buyerChannelIds;
            buyerChannelIdsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportSendingTime>("EXECUTE [dbo].[ReportSendingTimeByFilter] @start,@end,@campaigns,@buyers,@buyerChannels", startParam, endParam, campaignIdsParam, buyerIdsParam, buyerChannelIdsParam).ToList();
        }

        /// <summary>
        /// Get price points.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="isNot">Search in provided buyer channels or not.</param>/// 
        /// <returns>IList&lt;decimal&gt;.</returns>
        public IList<decimal> GetPricePoints(DateTime start, DateTime end, string buyerChannels, bool isNot)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannels";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var isNotParam = _dataProvider.GetParameter();
            isNotParam.ParameterName = "isnot";
            isNotParam.Value = isNot;
            isNotParam.DbType = DbType.Boolean;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<decimal>("EXECUTE [dbo].[GetPricePoints] @start,@end,@buyerChannels,@isnot", startParam, endParam, buyerChannelsParam, isNotParam).ToList();
        }



        /// <summary>
        /// Reports the affiliates by campaigns.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="affiliates">The affiliates.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByCampaigns&gt;.</returns>
        public virtual IList<ReportAffiliatesByCampaigns> ReportAffiliatesByCampaigns(DateTime start, DateTime end, string affiliates, string achannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "affiliates";
            buyersParam.Value = affiliates;
            buyersParam.DbType = DbType.String;

            var chanelsParam = _dataProvider.GetParameter();
            chanelsParam.ParameterName = "affiliateChannels";
            chanelsParam.Value = achannels;
            chanelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportAffiliatesByCampaigns>("EXECUTE [dbo].[ReportAffiliatesByCampaigns] @start,@end,@affiliates,@affiliateChannels", startParam, endParam, buyersParam, chanelsParam).ToList();
        }

        /// <summary>
        /// Reports the affiliates by affiliate channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="affiliates">The affiliates.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByAffiliateChannels&gt;.</returns>
        public virtual IList<ReportAffiliatesByAffiliateChannels> ReportAffiliatesByAffiliateChannels(DateTime start, DateTime end, string affiliates, string achannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "affiliates";
            buyersParam.Value = affiliates;
            buyersParam.DbType = DbType.String;

            var chanelsParam = _dataProvider.GetParameter();
            chanelsParam.ParameterName = "affiliateChannels";
            chanelsParam.Value = achannels;
            chanelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportAffiliatesByAffiliateChannels>("EXECUTE [dbo].[ReportAffiliatesByAffiliateChannels] @start,@end,@affiliates,@affiliateChannels", startParam, endParam, buyersParam, chanelsParam).ToList();
        }

        public virtual IList<ReportClickMain> ReportClickMain(DateTime start, DateTime end, string achannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var chanelsParam = _dataProvider.GetParameter();
            chanelsParam.ParameterName = "affiliateChannels";
            chanelsParam.Value = achannels;
            chanelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportClickMain>("EXECUTE [dbo].[ReportClickMain] @start,@end,@affiliateChannels", startParam, endParam, chanelsParam).ToList();
        }



        /// <summary>
        /// Reports the affiliates by states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="affiliates">The affiliates.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByStates&gt;.</returns>
        public virtual IList<ReportAffiliatesByStates> ReportAffiliatesByStates(DateTime start, DateTime end, string affiliates, string achannels, string states)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "affiliates";
            buyersParam.Value = affiliates;
            buyersParam.DbType = DbType.String;

            var chanelsParam = _dataProvider.GetParameter();
            chanelsParam.ParameterName = "affiliateChannels";
            chanelsParam.Value = achannels;
            chanelsParam.DbType = DbType.String;

            var statesParam = _dataProvider.GetParameter();
            statesParam.ParameterName = "states";
            statesParam.Value = states;
            statesParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportAffiliatesByStates>("EXECUTE [dbo].[ReportAffiliatesByStates] @start,@end,@affiliates,@affiliateChannels,@states", startParam, endParam, buyersParam, chanelsParam, statesParam).ToList();
        }

        /// <summary>
        /// Reports the affiliates by epl.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="affiliates">The affiliates.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByEpl&gt;.</returns>
        public virtual IList<ReportAffiliatesByEpl> ReportAffiliatesByEpl(DateTime start, DateTime end, string affiliates, string achannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyersParam = _dataProvider.GetParameter();
            buyersParam.ParameterName = "affiliates";
            buyersParam.Value = affiliates;
            buyersParam.DbType = DbType.String;

            var chanelsParam = _dataProvider.GetParameter();
            chanelsParam.ParameterName = "affiliateChannels";
            chanelsParam.Value = achannels;
            chanelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportAffiliatesByEpl>("EXECUTE [dbo].[ReportAffiliatesEpl] @start,@end,@affiliates,@affiliateChannels", startParam, endParam, buyersParam, chanelsParam).ToList();
        }

        /// <summary>
        /// Reports the by days.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByDays&gt;.</returns>
        public virtual IList<ReportByDays> ReportByDays(DateTime start, DateTime end, long campaignId, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignid";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByDays>("EXECUTE [dbo].[ReportByDays] @start, @end, @campaignid, @parentid", startParam, endParam, campaignParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the by minutes.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByMinutes&gt;.</returns>
        public virtual IList<ReportByMinutes> ReportByMinutes(DateTime start, DateTime end, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByMinutes>("EXECUTE [dbo].[ReportByMinutes] @start,@end,@parentid", startParam, endParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the by date.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByMinutes&gt;.</returns>
        public virtual IList<ReportByMinutes> ReportByDate(string activity, DateTime start, DateTime end, int delta, long campaignId, long parentid)
        {
            var activityParam = _dataProvider.GetParameter();
            activityParam.ParameterName = "activity";
            activityParam.Value = activity;
            activityParam.DbType = DbType.String;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var deltaParam = _dataProvider.GetParameter();
            deltaParam.ParameterName = "delta";
            deltaParam.Value = delta;
            deltaParam.DbType = DbType.Int32;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignId";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByMinutes>("EXECUTE [dbo].[ReportByDate] @activity, @start, @end, @delta, @campaignId, @parentid", activityParam, startParam, endParam, deltaParam, campaignParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the totals by date.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotalsByDate&gt;.</returns>
        public virtual IList<ReportTotalsByDate> ReportTotalsByDate(DateTime start, DateTime end, long campaignId, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignId";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportTotalsByDate>("EXECUTE [dbo].[ReportTotalsByDate] @start, @end, @campaignId, @parentid", startParam, endParam, campaignParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the by hour.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByHour&gt;.</returns>
        public virtual IList<ReportByHour> ReportByHour(DateTime start, DateTime end, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByHour>("EXECUTE [dbo].[ReportByHour] @start,@end,@parentid", startParam, endParam, parentParam).ToList();
        }

        public virtual LeadsReportTotal LeadDashboardTotals()
        {
            return _reportTypesRepository.GetDbClientContext().SqlQuery<LeadsReportTotal>("EXECUTE [dbo].[DashboardTotals]").FirstOrDefault();
        }

        public virtual IList<LeadsReportByHour> LeadDashboardReportByHour(DateTime start, DateTime end, List<long> campaignIds)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "campaignIds";
            parentParam.Value = string.Join(",", campaignIds);
            parentParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<LeadsReportByHour>("EXECUTE [dbo].[DashboardLeadMainReportByHour] @start,@end,@campaignIds", startParam, endParam, parentParam).ToList();
        }

        public virtual IList<LeadsReportByDay> LeadDashboardReportByDays(DateTime start, DateTime end, List<long> campaignIds)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "campaignIds";
            parentParam.Value = string.Join(",", campaignIds);
            parentParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<LeadsReportByDay>("EXECUTE [dbo].[DashboardLeadMainReportByDays] @start,@end,@campaignIds", startParam, endParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the by year.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <param name="campaignid">The campaignid.</param>
        /// <returns>IList&lt;ReportByYear&gt;.</returns>
        public virtual IList<ReportByYear> ReportByYear(DateTime start, DateTime end, long parentid, long campaignid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignid";
            campaignParam.Value = campaignid;
            campaignParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByYear>("EXECUTE [dbo].[ReportByYear] @start,@end,@parentid,@campaignid", startParam, endParam, parentParam, campaignParam).ToList();
        }

        /// <summary>
        /// Reports the by sub ids.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="subid">The subid.</param>
        /// <returns>IList&lt;ReportBySubId&gt;.</returns>
        public virtual IList<ReportBySubId> ReportBySubIds(DateTime start, DateTime end, string subid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var subIdParam = _dataProvider.GetParameter();
            subIdParam.ParameterName = "subid";
            subIdParam.Value = subid;
            subIdParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBySubId>("EXECUTE [dbo].[ReportBySubIds] @start,@end,@subid", startParam, endParam, subIdParam).ToList();
        }

        /// <summary>
        /// Reports the totals.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotals&gt;.</returns>
        public virtual IList<ReportTotals> ReportTotals(DateTime start, DateTime end, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportTotals>("EXECUTE [dbo].[ReportTotals] @start,@end,@parentid", startParam, endParam, parentParam).ToList();
        }

        public virtual IList<ReportTotals> ReportTotalsByCampaign(DateTime start, DateTime end, long campaignId, long parentId)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var campaignParam = _dataProvider.GetParameter();
            campaignParam.ParameterName = "campaignId";
            campaignParam.Value = campaignId;
            campaignParam.DbType = DbType.Int64;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentId;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportTotals>("EXECUTE [dbo].[ReportTotalsByCampaign] @start,@end,@campaignId,@parentid", startParam, endParam, campaignParam, parentParam).ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStateRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[RecievedReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardBarChartInsightModel> ReportTopAffiliatesRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardBarChartInsightModel>("EXECUTE [dbo].[TOPAffiliatesByCampaigns] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardBarChartInsightModel> ReportTopAffiliateChannelsRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardBarChartInsightModel>("EXECUTE [dbo].[TOPAffiliateChannelsByCampaigns] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }
        public List<DashboardBarChartInsightModel> ReportTopBuyersRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardBarChartInsightModel>("EXECUTE [dbo].[TOPBuyersByCampaigns] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardBarChartInsightModel> ReportTopBuyerChannelsRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardBarChartInsightModel>("EXECUTE [dbo].[TOPBuyerChannelsByCampaigns] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStatePosted(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[PostedReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStateRevenue(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[RevenueReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStateSold(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[SoldReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStateCost(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[CostReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }

        public List<DashboardPieChartInsightModel> ReportTopStateProfit(DateTime? start, DateTime? end, List<long> campaignIds, long count)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = (object)start ?? DBNull.Value; ;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = (object)end ?? DBNull.Value; ;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "CampaingnId";
            parentParam.Value = (object)string.Join(",", campaignIds) ?? DBNull.Value;
            parentParam.DbType = DbType.String;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = count;
            countParam.DbType = DbType.Int32;

            var result = _reportTypesRepository.GetDbClientContext().SqlQuery<DashboardPieChartInsightModel>("EXECUTE [dbo].[ProfitReportByTopStates] @StartDate,@EndDate,@Count,@CampaingnId", startParam, endParam, countParam, parentParam).ToList();
            return result.ToList();
        }
        /// <summary>
        /// Reports the totals buyer.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotalsBuyer&gt;.</returns>
        public virtual IList<ReportTotalsBuyer> ReportTotalsBuyer(DateTime start, DateTime end, long parentid)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var parentParam = _dataProvider.GetParameter();
            parentParam.ParameterName = "parentid";
            parentParam.Value = parentid;
            parentParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportTotalsBuyer>("EXECUTE [dbo].[ReportTotalsBuyer] @start,@end,@parentid", startParam, endParam, parentParam).ToList();
        }

        /// <summary>
        /// Reports the by statuses.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>IList&lt;ReportByStatuses&gt;.</returns>
        public virtual IList<ReportByStatuses> ReportByStatuses(DateTime start, DateTime end)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var obj = _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByStatuses>("EXECUTE [dbo].[ReportByStatuses] @StartDate, @EndDate", startParam, endParam).ToList();
            return obj;
        }

        /// <summary>
        /// Reports the by top states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="Count">The count.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="CampaingnId">The campaingn identifier.</param>
        /// <returns>IList&lt;ReportTopStates&gt;.</returns>
        public virtual IList<ReportTopStates> ReportByTopStates(DateTime start, DateTime end, int Count, long BuyerId, long AffiliateId, long CampaingnId)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "StartDate";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "EndDate";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "Count";
            countParam.Value = Count;
            countParam.DbType = DbType.Int32;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = BuyerId;
            buyerParam.DbType = DbType.Int64;

            var affiliateIdParam = _dataProvider.GetParameter();
            affiliateIdParam.ParameterName = "AffiliateId";
            affiliateIdParam.Value = AffiliateId;
            affiliateIdParam.DbType = DbType.Int64;

            var campaingnIdParam = _dataProvider.GetParameter();
            campaingnIdParam.ParameterName = "CampaingnId";
            campaingnIdParam.Value = CampaingnId;
            campaingnIdParam.DbType = DbType.Int64;

            var obj = _reportTypesRepository.GetDbClientContext().SqlQuery<ReportTopStates>("EXECUTE [dbo].[ReportByTopStates] @StartDate, @EndDate, @Count, @BuyerId, @AffiliateId, @CampaingnId", startParam, endParam, countParam, buyerParam, affiliateIdParam, campaingnIdParam).ToList();
            return obj;
        }

        /// <summary>
        /// Sales report.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>IList&lt;dynamic&gt;.</returns>
        public virtual IList<ChartData> SalesReport(DateTime start, DateTime end, string type)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var typeParam = _dataProvider.GetParameter();
            typeParam.ParameterName = "type";
            typeParam.Value = type;
            typeParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ChartData>("EXECUTE [dbo].[SalesReport] @start,@end,@type", startParam, endParam, typeParam).ToList();
        }

        public virtual IList<GlobalReport> GetGlobalReport(DateTime start, DateTime end, string buyerChannels, string affiliateChannels)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "startDate";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "endDate";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerChannelsParam = _dataProvider.GetParameter();
            buyerChannelsParam.ParameterName = "buyerChannelIds";
            buyerChannelsParam.Value = buyerChannels;
            buyerChannelsParam.DbType = DbType.String;

            var affiliateChannelsParam = _dataProvider.GetParameter();
            affiliateChannelsParam.ParameterName = "affiliateChannelIds";
            affiliateChannelsParam.Value = affiliateChannels;
            affiliateChannelsParam.DbType = DbType.String;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<GlobalReport>("EXECUTE [dbo].[GetGlobalReport] @startDate,@endDate,@buyerChannelIds,@affiliateChannelIds", startParam, endParam, buyerChannelsParam, affiliateChannelsParam).ToList();
        }

        public List<Core.Domain.CustomReports.ReportType> GetAllCustomReports()
        {
            return (from rt in _reportTypesRepository.Table
                    select rt).ToList();
        }

        public List<UserReport> GetUserAllReports(long userId)
        {
            return (from ur in _userReportsRepository.Table
                    where ur.UserId == userId
                    select ur).ToList();
        }

        public List<UserReport> GetUserAllReports(long reportId, bool isOwner)
        {
            return (from ur in _userReportsRepository.Table
                    where ur.ReportId == reportId && ur.IsOwner == isOwner
                    select ur).ToList();
        }

        public long AttachUserReport(UserReport userReport)
        {
            var report = _userReportsRepository.Table.Where(x => x.UserId == userReport.UserId && x.ReportId == userReport.ReportId).FirstOrDefault();
            if (report == null)
            {
                _userReportsRepository.Insert(userReport);
                _appEventPublisher.EntityInserted(userReport);
                return userReport.Id;
            }
            else return 0;
        }

        public void DetachUserReport(UserReport userReport)
        {
            var report = _userReportsRepository.Table.Where(x => x.UserId == userReport.UserId && x.ReportId == userReport.ReportId).FirstOrDefault();
            if (report != null)
            {
                _userReportsRepository.Delete(report);
                _appEventPublisher.EntityDeleted(report);
            }
        }



        public List<ReportVariableType> GetCustomReportVariables(long reportTypeId)
        {
            var variables = (from rtp in _reportTypeVariableRepository.Table.OrderBy(x => x.VariableOrder)
                             join rvt in _reportVariableTypesRepository.Table on rtp.ReportVariableTypeId equals rvt.Id
                             where rtp.ReportTypeId == reportTypeId
                             select new { rtp, rvt }).ToList().OrderBy(x => x.rtp.VariableOrder);

            return variables.Select(x => x.rvt).ToList();
        }

        public Core.Domain.CustomReports.ReportType GetCustomReportById(long customreportId)
        {
            return (from rtp in _reportTypesRepository.Table
                    where rtp.Id == customreportId
                    select rtp).FirstOrDefault();
        }

        public List<ReportVariable> GetReportVariables(long reportTypeId)
        {
            return (from rtp in _reportTypeVariableRepository.Table
                    where rtp.ReportTypeId == reportTypeId
                    select rtp).ToList();
        }

        public virtual IList<string> ExecuteCustomReportQuery(string queryStrings)
        {
            return _reportTypesRepository.GetDbClientContext().SqlQuery<string>(queryStrings + " FOR JSON PATH").ToList();
        }

        public virtual IList<T> ExecuteCustomReportQuery<T>(string queryStrings)
        {
            return _reportTypesRepository.GetDbClientContext().SqlQuery<T>(queryStrings).ToList();
        }

        public List<ReportFilterSetting> GetAllReportSettings()
        {
            return (from rs in _reportSettingsRepository.Table
                    select rs).ToList();
        }
        public List<ReportFilterSetting> GetAllReportSettingByReportType(long reportTypeId)
        {
            return (from rs in _reportSettingsRepository.Table
                    where rs.ReportTypeId == reportTypeId
                    select rs).ToList();
        }

        public ReportFilterSetting GetReportSettingById(long settingId)
        {
            return (from rs in _reportSettingsRepository.Table
                    where rs.Id == settingId
                    select rs).FirstOrDefault();
        }

        public ReportVariableType AddReportVariableType(ReportVariableType reportVariable)
        {
            if (reportVariable == null)
                throw new ArgumentNullException("reportSetting");

            _reportVariableTypesRepository.Insert(reportVariable);

            _appEventPublisher.EntityInserted(reportVariable);
            return reportVariable;
        }
        public ReportVariableType GetReportVariableType(long id)
        {
            var reportVariableType = _reportVariableTypesRepository.GetById(id);
            return reportVariableType;
        }

        public List<ReportVariableType> GetReportVariableTypes()
        {
            return _reportVariableTypesRepository.Table.ToList();
        }


        public void UpdateReportVariableType(ReportVariableType reportVariableType)
        {
            if (reportVariableType != null)
            {
                _reportVariableTypesRepository.Update(reportVariableType);

                _appEventPublisher.EntityInserted(reportVariableType);
            }
        }

        public void DeleteReportVariableType(long id)
        {
            var reportVariable = _reportTypeVariableRepository.GetById(id);
            if (reportVariable != null)
            {
                _reportTypeVariableRepository.Delete(reportVariable);
                _appEventPublisher.EntityDeleted(reportVariable);
            }
        }

        public void DeleteCustomReport(long reportId)
        {
            var report = _reportTypesRepository.GetById(reportId);
            if (report != null)
            {
                foreach (var reportVariable in _reportTypeVariableRepository.Table.Where(x => x.ReportTypeId == reportId).ToList())
                {
                    _reportTypeVariableRepository.Delete(reportVariable);
                }

                foreach (var userReport in _userReportsRepository.Table.Where(x => x.ReportId == reportId).ToList())
                {
                    _userReportsRepository.Delete(userReport);
                }

                foreach (var reportSettings in _reportSettingsRepository.Table.Where(x => x.ReportTypeId == reportId).ToList())
                {
                    _reportSettingsRepository.Delete(reportSettings);
                }

                _reportTypesRepository.Delete(report);
                _appEventPublisher.EntityDeleted(report);
            }
        }

        public void UpdateReportSetting(ReportFilterSetting reportSetting)
        {
            if (reportSetting == null)
                throw new ArgumentNullException("reportSetting");

            _reportSettingsRepository.Update(reportSetting);

            _appEventPublisher.EntityUpdated(reportSetting);
        }
        public long AddReportSetting(ReportFilterSetting reportSetting)
        {
            if (reportSetting == null)
                throw new ArgumentNullException("reportSetting");

            _reportSettingsRepository.Insert(reportSetting);

            _appEventPublisher.EntityInserted(reportSetting);
            return reportSetting.Id;
        }
        public void DeleteReportSettingById(long settingId)
        {
            var reportSetting = _reportSettingsRepository.GetById(settingId);
            if (reportSetting != null)
            {
                _reportSettingsRepository.Delete(reportSetting);
                _appEventPublisher.EntityDeleted(reportSetting);
            }
        }


        public void UpdateReportType(Core.Domain.CustomReports.ReportType reportType)
        {
            if (reportType == null)
                throw new ArgumentNullException("reportType");

            _reportTypesRepository.Update(reportType);

            _appEventPublisher.EntityUpdated(reportType);
        }

        public ReportVariable AttachReportVariableType(ReportVariable reportVariable)
        {
            if (reportVariable == null)
            {
                throw new ArgumentNullException("reportVariable");
            }
            var existing = _reportTypeVariableRepository.Table.Select(x => x).Where(x => x.ReportTypeId == reportVariable.ReportTypeId).OrderBy(x => x.VariableOrder).ToList();
            if (existing.Where(x => x.ReportVariableTypeId == reportVariable.ReportVariableTypeId).FirstOrDefault() != null)
            {
                throw new Exception("This variable already attached to given report type");
            }
            else
            {
                //var reportVariableType = _reportVariableTypesRepository.Table.Where(x => x.Id == reportVariable.ReportVariableTypeId).FirstOrDefault();
                //if (reportVariableType == null) { throw new Exception("This report variable type already exists"); }
                var lastOrderId = (existing != null && existing.Any()) ? existing.Last().VariableOrder : 0;
                reportVariable.VariableOrder = lastOrderId + 1;
                _reportTypeVariableRepository.Insert(reportVariable);
                _appEventPublisher.EntityInserted(reportVariable);
            }
            return reportVariable;
        }
        public void DetachReportVariableType(ReportVariable reportVariable)
        {
            var existing = _reportTypeVariableRepository.Table.FirstOrDefault(x => x.Id == reportVariable.Id);
            if (existing == null)
            {
                throw new Exception("This variable already attached to given report type");
            }
            _reportTypeVariableRepository.Delete(existing);
            _appEventPublisher.EntityDeleted(existing);
        }

        public void ClearReportVariables(int reportId)
        {
            var reportVariables = _reportTypeVariableRepository.Table.Where(x => x.ReportTypeId == reportId).ToList();

            foreach (var reportVariable in reportVariables)
            {
                _reportTypeVariableRepository.Delete(reportVariable);
                _appEventPublisher.EntityDeleted(reportVariable);
            }
        }

        public Core.Domain.CustomReports.ReportType AddReportType(Core.Domain.CustomReports.ReportType reportType)
        {
            if (reportType == null)
                throw new ArgumentNullException("reportType");

            _reportTypesRepository.Insert(reportType);

            _appEventPublisher.EntityInserted(reportType);
            return reportType;
        }


        public ReportFilterSetting GetReportSettingByReportId(long reportId)
        {
            return _reportSettingsRepository.Table.Where(x => x.ReportTypeId == reportId).FirstOrDefault();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public virtual void ClearCache()
        {
            _reportTypesRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[ClearCache]").FirstOrDefault();
        }

        /// <summary>
        /// Fills the main report.
        /// </summary>
        public virtual void FillMainReport()
        {

            _reportTypesRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[FillMainReport]").FirstOrDefault();
        }


        /// <summary>
        /// Get Reports Viewed
        /// </summary>
        /// <returns>ReportsViewed Collection All Item</returns>
        public virtual IList<ReportsViewed> GetReportsViewed()
        {
            string key = string.Format(CACHE_REPORTS_VIEWED_GetReportsViewedByUserId, string.Empty);

            return _cacheManager.Get(key, () =>
            {
                var query = (from x in _reportsViewedRepository.Table
                             orderby x.ViewDate descending
                             select x);

                return query.ToList();
            });
        }


        /// <summary>
        /// Get Reports Viewed By UserId
        /// </summary>
        /// <returns>ReportsViewed Collection Item By UserId</returns>
        public virtual IList<ReportsViewed> GetReportsViewedByUserId(long userId)
        {
            string key = string.Format(CACHE_REPORTS_VIEWED_GetReportsViewedByUserId, userId);

            return _cacheManager.Get(key, () =>
            {
                var query = (from x in _reportsViewedRepository.Table
                             where x.UserId == userId
                             orderby x.ViewDate descending
                             select x).Take(5);

                return query.ToList();
            });
        }


        /// <summary>
        /// Insert ReportsViewed
        /// </summary>
        /// <param name="reportName">The name of the report.</param>
        /// <param name="customReportTypeId">The name of Custom Report TypeId.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">ReportsViewed</exception>
        public long InsertReportsViewed(string reportName, int? customReportTypeId = null)
        {
            var userViewedReport = _reportsViewedRepository.Table.FirstOrDefault(x => x.UserId == _appContext.AppUser.Id &&
            ((customReportTypeId.HasValue && x.CustomReportTypeId == customReportTypeId) || (!customReportTypeId.HasValue && x.ReportName == reportName)));
            if (userViewedReport == null)
            {
                var obj = new ReportsViewed()
                {
                    UserId = _appContext.AppUser.Id,
                    ReportName = reportName,
                    ViewDate = DateTime.UtcNow,
                    CustomReportTypeId = customReportTypeId
                };

                _reportsViewedRepository.Insert(obj);

                _cacheManager.RemoveByPattern(CACHE_REPORTS_VIEWED_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityInserted(obj);

                return obj.Id;
            }
            else
            {
                userViewedReport.ViewDate = DateTime.UtcNow;
                _reportsViewedRepository.Update(userViewedReport);

                _cacheManager.RemoveByPattern(CACHE_REPORTS_VIEWED_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityUpdated(userViewedReport);
            }
            return 0;
        }

        /// <summary>
        /// Reports the by hours total.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public virtual IList<ReportByHourTotal> ReportByHourTotals(DateTime start, DateTime end)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByHourTotal>("EXECUTE [dbo].[ReportByHourTotals] @start, @end", startParam, endParam).ToList();
        }

        /// <summary>
        /// Reports the by weekday total.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public virtual IList<ReportByWeekdayTotal> ReportByWeekDayTotals(DateTime start, DateTime end)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportByWeekdayTotal>("EXECUTE [dbo].[ReportByWeekDayTotals] @start, @end", startParam, endParam).ToList();
        }

        /// <summary>
        /// Reports the by weekday total.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerId">buyerId.</param>
        /// <returns></returns>
        public virtual IList<ReportBuyersByPricesAndStates> ReportBuyersByPricesAndStates(DateTime start, DateTime end, long buyerId = 0)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "end";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "buyer";
            buyerParam.Value = buyerId;
            buyerParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<ReportBuyersByPricesAndStates>("EXECUTE [dbo].[ReportBuyersByPricesAndStates] @start, @end, @buyer", startParam, endParam, buyerParam).ToList();
        }

        public TotalRemaining GetRemainingsByPeriod(DateTime start, DateTime end, long buyerId = 0, long buyerChannelId = 0)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "startDate";
            startParam.Value = start;
            startParam.DbType = DbType.DateTime;

            var endParam = _dataProvider.GetParameter();
            endParam.ParameterName = "endDate";
            endParam.Value = end;
            endParam.DbType = DbType.DateTime;

            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "buyerId";
            buyerIdParam.Value = buyerId;
            buyerIdParam.DbType = DbType.Int64;

            var buyerChannelIdParam = _dataProvider.GetParameter();
            buyerChannelIdParam.ParameterName = "buyerChannelId";
            buyerChannelIdParam.Value = buyerChannelId;
            buyerChannelIdParam.DbType = DbType.Int64;

            return _reportTypesRepository.GetDbClientContext().SqlQuery<TotalRemaining>("EXECUTE [dbo].[GetRemainingsByPeriod] @startDate, @endDate, @buyerId, @buyerChannelId", startParam, endParam, buyerIdParam, buyerChannelIdParam).FirstOrDefault();

        }

        public TotalRemainingEntity GetRemainingEntities()
        {
            return _reportTypesRepository.GetDbClientContext().SqlQuery<TotalRemainingEntity>("EXECUTE [dbo].[GetRemainingEntities]").FirstOrDefault();

        }

        public void ValidateFilters(
            List<long> FilterCampaignIds = null,
            List<long> FilterAffiliateIds = null,
            List<long> FilterAffiliateChannelIds = null,
            List<long> FilterBuyerIds = null,
            List<long> FilterBuyerChannelIds = null,
            bool fillBuyers = true
            )
        {
            /*if (FilterCampaignIds == null)
                FilterCampaignIds = new List<long>();

            if (FilterAffiliateIds == null)
                FilterAffiliateIds = new List<long>();

            if (FilterAffiliateChannelIds == null)
                FilterAffiliateChannelIds = new List<long>();

            if (FilterBuyerIds == null)
                FilterBuyerIds = new List<long>();

            if (FilterBuyerChannelIds == null)
                FilterBuyerChannelIds = new List<long>();*/

            //if (_appContext.AppUser.UserType != UserTypes.Super)
            {
                if (FilterCampaignIds != null)
                {
                    var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id).ToList();

                    FilterCampaignIds.RemoveAll(x => !campaignIds.Contains(x));
                    if (FilterCampaignIds.Count == 0)
                    {
                        if (_appContext.AppUser.UserType != UserTypes.Super)
                            FilterCampaignIds.Add(-1);
                        else
                            FilterCampaignIds.AddRange(campaignIds);
                    }
                }

                if (FilterAffiliateIds != null)
                {
                    var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id).ToList();

                    FilterAffiliateIds.RemoveAll(x => !affiliateIds.Contains(x));
                    if (FilterAffiliateIds.Count == 0)
                    {
                        if (_appContext.AppUser.UserType == UserTypes.Super)
                            FilterAffiliateIds.AddRange(affiliateIds);
                        else
                        if (_appContext.AppUser.UserType != UserTypes.Buyer)
                            FilterAffiliateIds.Add(-1); 
                    }
                }

                if (FilterAffiliateChannelIds != null)
                {
                    var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id).ToList();

                    FilterAffiliateChannelIds.RemoveAll(x => !affiliateChannelIds.Contains(x));
                    if (FilterAffiliateChannelIds.Count == 0)
                    {
                        if (_appContext.AppUser.UserType == UserTypes.Super)
                            FilterAffiliateChannelIds.AddRange(affiliateChannelIds);
                        else
                        if (_appContext.AppUser.UserType != UserTypes.Buyer)
                            FilterAffiliateChannelIds.Add(-1);
                    }
                }

                if (FilterBuyerIds != null)
                {
                    var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id).ToList();

                    FilterBuyerIds.RemoveAll(x => !buyerIds.Contains(x));
                    if (FilterBuyerIds.Count == 0)
                    {
                        if (_appContext.AppUser.UserType == UserTypes.Super)
                        {
                            if (fillBuyers)
                                FilterBuyerIds.AddRange(buyerIds);
                        }
                        else
                        if (_appContext.AppUser.UserType != UserTypes.Affiliate)
                            FilterBuyerIds.Add(-1);
                    }
                }

                if (FilterBuyerChannelIds != null)
                {
                    var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id).ToList();

                    FilterBuyerChannelIds.RemoveAll(x => !buyerChannelIds.Contains(x));
                    if (FilterBuyerChannelIds.Count == 0)
                    {
                        if (_appContext.AppUser.UserType == UserTypes.Super)
                        {
                            if (fillBuyers)
                                FilterBuyerChannelIds.AddRange(buyerChannelIds);
                        }
                        else
                        if (_appContext.AppUser.UserType != UserTypes.Affiliate)
                            FilterBuyerChannelIds.Add(-1);
                    }
                }
            }
        }

        public long ValidateFilter(long? campaignId = null, long? affiliateId = null, long? affiliateChannelId = null, long? buyerId = null, long? buyerChannelId = null)
        {
            if (_appContext.AppUser.UserType != UserTypes.Super)
            {
                if (campaignId != null)
                {
                    var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id).ToList();

                    if (!campaignIds.Contains(campaignId.Value))
                        return -1;
                }

                if (affiliateId != null)
                {
                    var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id).ToList();

                    if (!affiliateIds.Contains(affiliateId.Value))
                        return -1;
                }

                if (affiliateChannelId != null)
                {
                    var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id).ToList();

                    if (!affiliateChannelIds.Contains(affiliateChannelId.Value))
                        return -1;
                }

                if (buyerId != null)
                {
                    var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id).ToList();

                    if (!buyerIds.Contains(buyerId.Value))
                        return -1;
                }

                if (buyerChannelId != null)
                {
                    var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id).ToList();

                    if (!buyerChannelIds.Contains(buyerChannelId.Value))
                        return -1;
                }
            }

            if (campaignId.HasValue) return campaignId.Value;
            if (affiliateId.HasValue) return affiliateId.Value;
            if (affiliateChannelId.HasValue) return affiliateChannelId.Value;
            if (buyerId.HasValue) return buyerId.Value;
            if (buyerChannelId.HasValue) return buyerChannelId.Value;

            return -1;
        }

        #endregion Methods
    }

    public class sqlResult
    {
        public long BuyerId { get; set; }
        public string BuyerName { get; set; }
    }
}