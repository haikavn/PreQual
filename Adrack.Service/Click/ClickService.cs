// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Click
{
    /// <summary>
    /// Represents a click Service
    /// Implements the <see cref="Adrack.Service.Click.IClickService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Directory.ICountryService" />
    public partial class ClickService : IClickService
    {
        #region Constants

        private const string CACHE_CLICK_CHANNEL_BY_ACCESS_KEY = "App.Cache.Click.Channel.By.AccessKey-{0}";

        private const string CACHE_CLICK_CONTENT = "App.Cache.Click.Content-{0}-{1}-{2}";

        private const string CACHE_CLICK_POSTBACKURLS = "App.Cache.Click.PostBackUrls-{0}";

        private const string CACHE_CLICK_GetClickChannelByAffiliateChannelId = "App.Cache.Click.GetClickChannelByAffiliateChannelId-{0}"; 

        private const string CACHE_CLICK_PATTERN_KEY = "App.Cache.Click.";

        #endregion Constants

        #region Fields

        private readonly IRepository<ClickChannel> _clickChannelRepository;


        private readonly IRepository<ClickMain> _clickMainRepository;

        private readonly IRepository<ClickContent> _clickContentRepository;

        private readonly IRepository<ClickPostBackUrl> _clickPostBackUrlRepository;

        private readonly IRepository<ClickPostbackUrlLog> _clickPostBackUrlLogRepository;

        private readonly IDataProvider _dataProvider;


        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Country Service
        /// </summary>
        /// <param name="countryRepository">Country Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public ClickService(IRepository<ClickChannel> clickChannelRepository, 
            IRepository<ClickMain> clickMainRepository, 
            IRepository<ClickContent> clickContentRepository,
            IRepository<ClickPostBackUrl> clickPostBackUrlRepository,
            IRepository<ClickPostbackUrlLog> clickPostBackUrlLogRepository,
            ICacheManager cacheManager, 
            IAppEventPublisher appEventPublisher,
            IDataProvider dataProvider)
        {
            this._clickChannelRepository = clickChannelRepository;
            this._clickMainRepository = clickMainRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._clickContentRepository = clickContentRepository;
            this._clickPostBackUrlRepository = clickPostBackUrlRepository;
            this._clickPostBackUrlLogRepository = clickPostBackUrlLogRepository;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        public virtual ClickChannel GetClickChannelByAccessKey(string accessKey)
        {
            if (string.IsNullOrEmpty(accessKey))
                return null;

            string key = string.Format(CACHE_CLICK_CHANNEL_BY_ACCESS_KEY, accessKey);

            return _cacheManager.Get(key, () => {
                var query = from x in _clickChannelRepository.Table
                            where x.AccessKey == accessKey
                            select x;

                return query.FirstOrDefault();
            });
        }

        public virtual ClickChannel GetClickChannelByAffiliateChannelId(long affiliateChannelId, bool fromCache = true)
        {
            if (!fromCache)
            {
                var query = from x in _clickChannelRepository.Table
                            where x.AffiliateChannelId == affiliateChannelId
                            select x;

                return query.FirstOrDefault();
            }

            string key = string.Format(CACHE_CLICK_GetClickChannelByAffiliateChannelId, affiliateChannelId);

            return _cacheManager.Get(key, () => {
                var query = from x in _clickChannelRepository.Table
                            where x.AffiliateChannelId == affiliateChannelId
                            select x;

                return query.FirstOrDefault();
            });
        }

        public void InsertClickChannel(ClickChannel clickChannel)
        {
            if (clickChannel == null)
                throw new ArgumentNullException("clickChannel");

            _clickChannelRepository.Insert(clickChannel);

            _cacheManager.RemoveByPattern(CACHE_CLICK_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(clickChannel);
        }

        public void UpdateClickChannel(ClickChannel clickChannel)
        {
            if (clickChannel == null)
                throw new ArgumentNullException("clickChannel");

            _clickChannelRepository.Update(clickChannel);

            _cacheManager.RemoveByPattern(CACHE_CLICK_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(clickChannel);
        }

        public virtual void InsertClickMain(ClickMain clickMain)
        {
            if (clickMain == null)
                throw new ArgumentNullException("clickMain");

            _clickMainRepository.Insert(clickMain);

            _cacheManager.RemoveByPattern(CACHE_CLICK_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(clickMain);
        }

        public virtual void InsertClickContent(ClickContent clickContent)
        {
            if (clickContent == null)
                throw new ArgumentNullException("clickContent");

            _clickContentRepository.Insert(clickContent);

            _cacheManager.RemoveByPattern(CACHE_CLICK_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(clickContent);
        }

        public virtual void InsertClickPostbackUrlLog(ClickPostbackUrlLog clickPostbackUrlLog)
        {
            if (clickPostbackUrlLog == null)
                throw new ArgumentNullException("clickPostbackUrlLog");

            _clickPostBackUrlLogRepository.Insert(clickPostbackUrlLog);

            _cacheManager.RemoveByPattern(CACHE_CLICK_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(clickPostbackUrlLog);
        }

        public ClickContent GetClickContent(long clickChannelId, string paramName, string paramValue)
        {
            string key = string.Format(CACHE_CLICK_CONTENT, clickChannelId, paramName, paramValue);

            return _cacheManager.Get(key, () => {
                var query = from x in _clickContentRepository.Table
                            where x.ClickChannelId == clickChannelId && x.ParamName == paramValue && paramValue == paramValue
                            select x;

                return query.FirstOrDefault();
            });
        }

        public List<ClickPostBackUrl> GetClickPostBackUrls(long clickChannelId)
        {
            string key = string.Format(CACHE_CLICK_POSTBACKURLS, clickChannelId);

            return _cacheManager.Get(key, () => {
                var query = from x in _clickPostBackUrlRepository.Table
                            where x.ClickChannelId == clickChannelId
                            select x;

                return query.ToList();
            });
        }

        public virtual ReportClickCount GetClicksCount(long affiliateChannelId)
        {
            var affiliateChannelParam = _dataProvider.GetParameter();
            affiliateChannelParam.ParameterName = "affiliateChannelId";
            affiliateChannelParam.Value = affiliateChannelId;
            affiliateChannelParam.DbType = DbType.Int64;

            return _clickMainRepository.GetDbClientContext().SqlQuery<ReportClickCount>("EXECUTE [dbo].[GetClicksCount] @affiliateChannelId", affiliateChannelParam).FirstOrDefault();
        }


        #endregion Methods
    }
}