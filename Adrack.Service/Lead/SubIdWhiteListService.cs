// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterService.cs" company="Adrack.com">
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
    /// Class DoNotPresentService.
    /// Implements the <see cref="Adrack.Service.Lead.ISubIdWhiteListService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ISubIdWhiteListService" />
    public partial class SubIdWhiteListService : ISubIdWhiteListService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_SUBIDWHITELIST_BY_ID_KEY = "App.Cache.SubIdWhiteList.By.Id-{0}";

        private const string CACHE_SUBIDWHITELIST_BY_BUYERCHANNELID_KEY = "App.Cache.SubIdWhiteList.By.BuyerChannelId-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_SUBIDWHITELIST_ALL_KEY = "App.Cache.SubIdWhiteList.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_SUBIDWHITELIST_PATTERN_KEY = "App.Cache.SubIdWhiteList.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<SubIdWhiteList> _subIdWhiteListRepository;

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

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="doNotPresentRepository">The do not present repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public SubIdWhiteListService(IRepository<SubIdWhiteList> subIdWhiteListRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._subIdWhiteListRepository = subIdWhiteListRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Insert SubIdWhiteList
        /// </summary>
        /// <param name="subIdWhiteList">The SubIdWhiteList.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">SubIdWhiteList</exception>
        public virtual long InsertSubIdWhiteList(SubIdWhiteList subIdWhiteList)
        {
            if (subIdWhiteList == null)
                throw new ArgumentNullException("subIdWhiteList");

            _subIdWhiteListRepository.Insert(subIdWhiteList);

            _cacheManager.RemoveByPattern(CACHE_SUBIDWHITELIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(subIdWhiteList);

            return subIdWhiteList.Id;
        }

        public virtual int CheckSubIdWhiteList(string subId, long buyerChannelId)
        {
            var subIdParam = _dataProvider.GetParameter();
            subIdParam.ParameterName = "subId";
            subIdParam.Value = subId;
            subIdParam.DbType = DbType.String;

            var buyerChannelIdParam = _dataProvider.GetParameter();
            buyerChannelIdParam.ParameterName = "BuyerChannelId";
            buyerChannelIdParam.Value = buyerChannelId;
            buyerChannelIdParam.DbType = DbType.Int64;

            return _subIdWhiteListRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[CheckSubIdWhiteList] @subid,@buyerchannelid", subIdParam, buyerChannelIdParam).FirstOrDefault();
        }

        public virtual IList<SubIdWhiteList> GetAllSubIdWhiteList()
        {
            string key = CACHE_SUBIDWHITELIST_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _subIdWhiteListRepository.Table
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        public virtual IList<SubIdWhiteList> GetAllSubIdWhiteList(long buyerChannelId)
        {
            string key = string.Format(CACHE_SUBIDWHITELIST_BY_BUYERCHANNELID_KEY, buyerChannelId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _subIdWhiteListRepository.Table
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        public virtual void DeleteSubIdWhiteList(SubIdWhiteList subIdWhiteList)
        {
            if (subIdWhiteList == null)
                throw new ArgumentNullException("subIdWhiteList");

            _subIdWhiteListRepository.Delete(subIdWhiteList);

            _cacheManager.RemoveByPattern(CACHE_SUBIDWHITELIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(subIdWhiteList);
        }

        public virtual void DeleteAllSubIdWhiteList(long buyerChannelId)
        {
            List<SubIdWhiteList> list = (List<SubIdWhiteList>)GetAllSubIdWhiteList(buyerChannelId);
            foreach(SubIdWhiteList item in list)
            {
                DeleteSubIdWhiteList(item);
            }
        }

        #endregion Methods
    }
}