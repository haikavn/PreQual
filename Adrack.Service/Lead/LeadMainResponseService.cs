// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadMainResponseService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
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
    /// Implements the <see cref="Adrack.Service.Lead.ILeadMainResponseService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadMainResponseService" />
    public partial class LeadMainResponseService : ILeadMainResponseService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_RESPONSE_BY_ID_KEY = "App.Cache.LeadMainResponse.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_RESPONSE_ALL_KEY = "App.Cache.LeadMainResponse.All";

        /// <summary>
        /// Cache Lead Key by Buyer Id
        /// </summary>
        private const string CACHE_LEAD_MAIN_RESPONSE_BY_ID = "App.Cache.LeadMainResponse.{0}.{1}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_RESPONSE_PATTERN_KEY = "App.Cache.LeadMainResponse.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<LeadMainResponse> _leadMainRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadMainRepository">The lead main repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        public LeadMainResponseService(IRepository<LeadMainResponse> leadMainRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._leadMainRepository = leadMainRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead main response by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadMainResponse.</returns>
        public virtual LeadMainResponse GetLeadMainResponseById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_MAIN_RESPONSE_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _leadMainRepository.GetById(leadId); });
        }

        // <summary>
        /// <summary>
        /// Gets the lead main response by lead identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;LeadResponse&gt;.</returns>
        public virtual IList<LeadResponse> GetLeadMainResponseByLeadId(long Id)
        {
            if (Id == 0)
                return null;

            string key = string.Format(CACHE_LEAD_MAIN_RESPONSE_BY_ID_KEY, Id);

            var LeadIdParam = _dataProvider.GetParameter();
            LeadIdParam.ParameterName = "leadId";
            LeadIdParam.Value = Id;
            LeadIdParam.DbType = DbType.Int64;

            var lrl = _leadMainRepository.GetDbClientContext().SqlQuery<LeadResponse>("EXECUTE [dbo].[GetResponsesByLeadId] @leadId", LeadIdParam).ToList();

            return lrl;
        }

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<LeadMainResponse> GetAllLeadResponses()
        {
            string key = CACHE_LEAD_MAIN_RESPONSE_ALL_KEY;

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
        /// Gets the leads count by period.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="status">The status.</param>
        /// <returns>System.Int32.</returns>
        public virtual int GetLeadsCountByPeriod(long buyerChannelId, DateTime fromDate, DateTime toDate, short status)
        {
            if (status == 0) status = -1;
            var fromParam = _dataProvider.GetParameter();
            fromParam.ParameterName = "from";
            fromParam.Value = fromDate;
            fromParam.DbType = DbType.DateTime;

            var toParam = _dataProvider.GetParameter();
            toParam.ParameterName = "to";
            toParam.Value = toDate;
            toParam.DbType = DbType.DateTime;

            var buyerChannelParam = _dataProvider.GetParameter();
            buyerChannelParam.ParameterName = "BuyerChannelId";
            buyerChannelParam.Value = buyerChannelId;
            buyerChannelParam.DbType = DbType.Int64;

            var statusParam = _dataProvider.GetParameter();
            statusParam.ParameterName = "status";
            statusParam.Value = status;
            buyerChannelParam.DbType = DbType.Int16;

            return _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetLeadsCountByPeriod] @buyerchannelid, @from, @to, @status", buyerChannelParam, fromParam, toParam, statusParam).FirstOrDefault();
        }

        /// <summary>
        /// Gets the dublicate lead by buyer.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="created">The created.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="fromBuyer">if set to <c>true</c> [from buyer].</param>
        /// <returns>LeadMainResponse.</returns>
        public virtual bool GetDublicateLeadByBuyer(string ssn, DateTime created, long id, bool fromBuyer, long leadId = 0)
        {
            var ssnParam = _dataProvider.GetParameter();
            ssnParam.ParameterName = "ssn";
            ssnParam.Value = (ssn == null ? "" : ssn);
            ssnParam.DbType = DbType.String;

            var createdParam = _dataProvider.GetParameter();
            createdParam.ParameterName = "created";
            createdParam.Value = created;
            createdParam.DbType = DbType.DateTime;

            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "buyerid";
            buyerIdParam.Value = id;
            buyerIdParam.DbType = DbType.Int64;

            var fromBuyerParam = _dataProvider.GetParameter();
            fromBuyerParam.ParameterName = "fromBuyer";
            fromBuyerParam.Value = fromBuyer;
            fromBuyerParam.DbType = DbType.Boolean;

            /*var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            long res = 1;
            if (_dbContext.SqlQuery<int>("EXECUTE [dbo].[GetDublicateLeadByBuyer] @ssn, @created, @buyerid, @fromBuyer, @leadId", ssnParam, createdParam, buyerIdParam, fromBuyerParam, leadIdParam).FirstOrDefault() == res)
                return true;
            else
                return false;
            */

            long res = 1;
            if (_leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[GetDublicateLeadByBuyer] @ssn, @created, @buyerid, @fromBuyer", ssnParam, createdParam, buyerIdParam, fromBuyerParam).FirstOrDefault() == res)
                return true;
            else
                return false;
        }
       

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual long InsertLeadMainResponse(LeadMainResponse leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Insert(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_RESPONSE_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadMain);

            return leadMain.Id;
        }

        public virtual long InsertLeadMainResponseList(IEnumerable<LeadMainResponse> leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Insert(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_RESPONSE_PATTERN_KEY);            
            return 0;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void UpdateLeadMainResponse(LeadMainResponse leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Update(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_RESPONSE_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadMain);
        }

        /// <summary>
        /// Delete LeadMainResponse
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void DeleteLeadMainResponse(LeadMainResponse leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Delete(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_RESPONSE_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(leadMain);
        }

        /// <summary>
        /// GetLeadMainResponsesByLeadIdBuyerId
        /// </summary>
        /// <param name="LeadId">The lead identifier.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual LeadMainResponse GetLeadMainResponsesByLeadIdBuyerId(long LeadId, long BuyerId)
        {        
            string key = String.Format(CACHE_LEAD_MAIN_RESPONSE_BY_ID,LeadId,BuyerId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadMainRepository.Table
                            where x.LeadId == LeadId && x.BuyerId == BuyerId
                            select x;

                var profiles = query.FirstOrDefault();

                return profiles;
            });
        }

        public LeadMainResponse GetLeadMainResponsesByLeadIdBuyerChannelId(long LeadId, long BuyerChannelId)
        {
            var buyerChannelIdParam = _dataProvider.GetParameter();
            buyerChannelIdParam.ParameterName = "buyerChannelId";
            buyerChannelIdParam.Value = BuyerChannelId;
            buyerChannelIdParam.DbType = DbType.Int64;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "LeadId";
            leadIdParam.Value = LeadId;
            leadIdParam.DbType = DbType.Int64;

            return _leadMainRepository.GetDbClientContext().SqlQuery<LeadMainResponse>("EXECUTE [dbo].[CheckForPosted] @leadId, @buyerChannelId", leadIdParam, buyerChannelIdParam).FirstOrDefault();
        }

        #endregion Methods
    }
}