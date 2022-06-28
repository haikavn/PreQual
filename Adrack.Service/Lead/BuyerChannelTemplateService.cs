// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplateService.cs" company="Adrack.com">
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
    /// Class BuyerChannelTemplateService.
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerChannelTemplateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerChannelTemplateService" />
    public partial class BuyerChannelTemplateService : IBuyerChannelTemplateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_TEMPLATE_BY_ID_KEY = "App.Cache.BC.BuyerChannelTemplate.By.Id-{0}";

        /// <summary>
        /// The cache buyer channel template by buyer channel identifier key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_BY_BUYER_CHANNEL_ID_KEY = "App.Cache.BC.BuyerChannelTemplate.By.BuyerChannelId-{0}";

        /// <summary>
        /// The cache buyer channel template by identifier field section
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_BY_ID_FIELD_SECTION = "App.Cache.BC.BuyerChannelTemplate.By.Id.Field.Section-{0}-{1}-{2}";

        /// <summary>
        /// The cache buyer channel template by campaign template
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_BY_CAMPAIGN_TEMPLATE = "App.Cache.BC.BuyerChannelTemplate.By.Campaign.Template-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_ALL_KEY = "App.Cache.BC.BuyerChannelTemplate.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_TEMPLATE_PATTERN_KEY = "App.Cache.BC.BuyerChannelTemplate.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BuyerChannelTemplate> _buyerChannelTemplateRepository;

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
        /// <param name="buyerChannelTemplateRepository">The buyer channel template repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public BuyerChannelTemplateService(IRepository<BuyerChannelTemplate> buyerChannelTemplateRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._buyerChannelTemplateRepository = buyerChannelTemplateRepository;
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
        public virtual BuyerChannelTemplate GetBuyerChannelTemplateById(long Id)
        {
            if (Id == 0)
                return null;

            return _buyerChannelTemplateRepository.GetById(Id);
        }

        /// <summary>
        /// Gets the buyer channel template.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="field">The field.</param>
        /// <param name="section">The section.</param>
        /// <returns>BuyerChannelTemplate.</returns>
        public virtual BuyerChannelTemplate GetBuyerChannelTemplate(long buyerChannelId, string field, string section)
        {
            return (from x in _buyerChannelTemplateRepository.Table
                    where x.BuyerChannelId == buyerChannelId && x.TemplateField == field && x.SectionName == section
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Gets the buyer channel template.
        /// </summary>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>BuyerChannelTemplate.</returns>
        public BuyerChannelTemplate GetBuyerChannelTemplate(long campaignTemplateId)
        {
            return (from x in _buyerChannelTemplateRepository.Table
                    where x.CampaignTemplateId == campaignTemplateId
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerChannelTemplate> GetAllBuyerChannelTemplates()
        {
            string key = CACHE_BUYER_CHANNEL_TEMPLATE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelTemplateRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all buyer channel templates by buyer channel identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BuyerChannelTemplate&gt;.</returns>
        public virtual IList<BuyerChannelTemplate> GetAllBuyerChannelTemplatesByBuyerChannelId(long Id)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_TEMPLATE_BY_BUYER_CHANNEL_ID_KEY, Id);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelTemplateRepository.Table
                            where x.BuyerChannelId == Id
                            orderby x.Id
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerChannelTemplate</exception>
        public virtual long InsertBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate)
        {
            if (buyerChannelTemplate == null)
                throw new ArgumentNullException("buyerChannelTemplate");

            _buyerChannelTemplateRepository.Insert(buyerChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelTemplate);

            return buyerChannelTemplate.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        /// <exception cref="ArgumentNullException">buyerChannelTemplate</exception>
        public virtual void UpdateBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate)
        {
            if (buyerChannelTemplate == null)
                throw new ArgumentNullException("buyerChannelTemplate");

            _buyerChannelTemplateRepository.Update(buyerChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyerChannelTemplate);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        /// <exception cref="ArgumentNullException">buyerChannelTemplate</exception>
        public virtual void DeleteBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate)
        {
            if (buyerChannelTemplate == null)
                throw new ArgumentNullException("buyerChannelTemplate");

            _buyerChannelTemplateRepository.Delete(buyerChannelTemplate);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(buyerChannelTemplate);
        }

        /// <summary>
        /// Deletes the buyer channel templates by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        public virtual void DeleteBuyerChannelTemplatesByBuyerChannelId(long buyerChannelId)
        {
            var list = (from x in _buyerChannelTemplateRepository.Table
                        where x.BuyerChannelId == buyerChannelId
                        select x).ToList();

            foreach (var item in list)
            {
                _buyerChannelTemplateRepository.Delete(item);
            }
        }

        #endregion Methods
    }
}