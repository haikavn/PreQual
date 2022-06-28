// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateChannelTemplateService.cs" company="Adrack.com">
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
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class AffiliateChannelTemplateService.
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateChannelTemplateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateChannelTemplateService" />
    public partial class AffiliateChannelTemplateService : IAffiliateChannelTemplateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_TEMPLATE_BY_ID_KEY = "App.Cache.AffiliateChannelTemplate.By.Id-{0}";

        /// <summary>
        /// The cache affiliate channel template template by channel identifier key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_TEMPLATE_BY_CHANNEL_ID_KEY = "App.Cache.AffiliateChannelTemplate.By.ChannelId-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_ALL_KEY = "App.Cache.AffiliateChannelTemplate.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY = "App.Cache.AffiliateChannelTemplate.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<AffiliateChannelTemplate> _affiliateChannelTemplateRepository;

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
        /// Profile Service
        /// </summary>
        /// <param name="affiliateChannelTemplateRepository">The affiliate channel template repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public AffiliateChannelTemplateService(IRepository<AffiliateChannelTemplate> affiliateChannelTemplateRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._affiliateChannelTemplateRepository = affiliateChannelTemplateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual AffiliateChannelTemplate GetAffiliateChannelTemplateById(long Id)
        {
            if (Id == 0)
                return null;

            string key = string.Format(CACHE_AFFILIATE_CHANNEL_TEMPLATE_TEMPLATE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _affiliateChannelTemplateRepository.GetById(Id); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateChannelTemplate> GetAllAffiliateChannelTemplates()
        {
            string key = CACHE_AFFILIATE_CHANNEL_TEMPLATE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelTemplateRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all affiliate channel templates by affiliate channel identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;AffiliateChannelTemplate&gt;.</returns>
        public virtual IList<AffiliateChannelTemplate> GetAllAffiliateChannelTemplatesByAffiliateChannelId(long Id)
        {
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_TEMPLATE_TEMPLATE_BY_CHANNEL_ID_KEY, Id);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelTemplateRepository.Table
                            where x.AffiliateChannelId == Id
                            orderby x.Id
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the affiliate channel template.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>AffiliateChannelTemplate.</returns>
        public virtual AffiliateChannelTemplate GetAffiliateChannelTemplate(long channelId, long campaignTemplateId)
        {
                return (from x in _affiliateChannelTemplateRepository.Table
                        where x.AffiliateChannelId == channelId && x.CampaignTemplateId == campaignTemplateId
                        orderby x.Id
                        select x).FirstOrDefault();            
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateChannelTemplate</exception>
        public virtual long InsertAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate)
        {
            if (affiliateChannelTemplate == null)
                throw new ArgumentNullException("affiliateChannelTemplate");

            _affiliateChannelTemplateRepository.Insert(affiliateChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(affiliateChannelTemplate);
            _cacheManager.ClearRemoteServersCache();
            return affiliateChannelTemplate.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        /// <exception cref="ArgumentNullException">affiliateChannelTemplate</exception>
        public virtual void UpdateAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate)
        {
            if (affiliateChannelTemplate == null)
                throw new ArgumentNullException("affiliateChannelTemplate");

            _affiliateChannelTemplateRepository.Update(affiliateChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(affiliateChannelTemplate);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        /// <exception cref="ArgumentNullException">affiliateChannelTemplate</exception>
        public virtual void DeleteAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate)
        {
            if (affiliateChannelTemplate == null)
                throw new ArgumentNullException("affiliateChannelTemplate");

            _affiliateChannelTemplateRepository.Delete(affiliateChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliateChannelTemplate);
        }

        /// <summary>
        /// Deletes the affiliate channel templates by affiliate channel identifier.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        public virtual void DeleteAffiliateChannelTemplatesByAffiliateChannelId(long affiliateChannelId)
        {
            var list = (from x in _affiliateChannelTemplateRepository.Table
                        where x.AffiliateChannelId == affiliateChannelId
                        select x).ToList();

            foreach (var item in list)
            {
                _affiliateChannelTemplateRepository.Delete(item);
            }
        }

        #endregion Methods
    }
}