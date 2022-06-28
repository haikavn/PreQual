// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CampaignTemplateService.cs" company="Adrack.com">
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
    /// Class CampaignTemplateService.
    /// Implements the <see cref="Adrack.Service.Lead.ICampaignTemplateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ICampaignTemplateService" />
    public partial class CampaignTemplateService : ICampaignTemplateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_CAMPAIGN_TEMPLATE_BY_ID_KEY = "App.Cache.CampaignTemplate.By.Id-{0}";

        /// <summary>
        /// The cache campaign template by campaign identifier key
        /// </summary>
        private const string CACHE_CAMPAIGN_TEMPLATE_BY_CAMPAIGN_ID_KEY = "App.Cache.CampaignTemplate.By.CampaignId-{0}-{1}-{2}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_CAMPAIGN_TEMPLATE_ALL_KEY = "App.Cache.CampaignTemplate.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_CAMPAIGN_TEMPLATE_PATTERN_KEY = "App.Cache.CampaignTemplate.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<CampaignField> _campaignTemplateRepository;

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
        /// <param name="campaignTemplateRepository">The campaign template repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CampaignTemplateService(IRepository<CampaignField> campaignTemplateRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._campaignTemplateRepository = campaignTemplateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="campaignTemplate">The campaign template.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">campaignTemplate</exception>
        public virtual long InsertCampaignTemplate(CampaignField campaignTemplate)
        {
            if (campaignTemplate == null)
                throw new ArgumentNullException("campaignTemplate");

            _campaignTemplateRepository.SetCanTrackChanges(true);

            _campaignTemplateRepository.Insert(campaignTemplate);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(campaignTemplate);

            return campaignTemplate.Id;
        }

        /// <summary>
        /// Updates the campaign template.
        /// </summary>
        /// <param name="campaignTemplate">The campaign template.</param>
        /// <exception cref="ArgumentNullException">campaignTemplate</exception>
        public virtual void UpdateCampaignTemplate(CampaignField campaignTemplate)
        {
            if (campaignTemplate == null)
                throw new ArgumentNullException("campaignTemplate");

            _campaignTemplateRepository.SetCanTrackChanges(true);

            _campaignTemplateRepository.Update(campaignTemplate);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(campaignTemplate);
        }

        /// <summary>
        /// Determines whether [is campaign template hidden] [the specified campaign identifier].
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns><c>true</c> if [is campaign template hidden] [the specified campaign identifier]; otherwise, <c>false</c>.</returns>
        public virtual bool IsCampaignTemplateHidden(long campaignId, string fieldName)
        {
            CampaignField ct = (from x in _campaignTemplateRepository.Table
                                   where x.TemplateField == fieldName && x.CampaignId == campaignId
                                   select x).FirstOrDefault();

            if (ct == null) return false;
            if ((bool)ct.IsHidden) return true;

            return IsCampaignTemplateHidden(campaignId, ct.SectionName);
        }

        /// <summary>
        /// Campaigns the template allowed names.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public virtual List<string> CampaignTemplateAllowedNames(long campaignId)
        {
            List<string> ct = (from x in _campaignTemplateRepository.Table
                               where x.CampaignId == campaignId && x.IsHidden == false
                               select x.TemplateField).ToList();

            return ct;
        }

        /// <summary>
        /// Gets the campaign template by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>CampaignTemplate.</returns>
        public virtual CampaignField GetCampaignTemplateById(long Id, bool cached = false)
        {
            if (Id == 0)
                return null;

            if (!cached)
                return _campaignTemplateRepository.GetById(Id);

            string key = string.Format(CACHE_CAMPAIGN_TEMPLATE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _campaignTemplateRepository.GetById(Id); });
        }

        /// <summary>
        /// Gets the name of the campaign template by section and.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>CampaignTemplate.</returns>
        public virtual CampaignField GetCampaignTemplateBySectionAndName(string sectionName, string fieldName, long ID)
        {
            return (from x in _campaignTemplateRepository.Table
                    where (sectionName.Length == 0 || x.SectionName == sectionName) && x.TemplateField == fieldName && x.CampaignId==ID
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Gets the campaign templates by section.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>IList&lt;CampaignTemplate&gt;.</returns>
        public virtual IList<CampaignField> GetCampaignTemplatesBySection(long campaignId, string sectionName)
        {
            return (from x in _campaignTemplateRepository.Table
                    where x.SectionName == sectionName && x.CampaignId == campaignId
                    select x).ToList();
        }

        /// <summary>
        /// Gets the campaign templates by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="order">The order.</param>
        /// <param name="isfilterable">The isfilterable.</param>
        /// <returns>IList&lt;CampaignTemplate&gt;.</returns>
        public virtual IList<CampaignField> GetCampaignTemplatesByCampaignId(long campaignId, string order = "asc", short isfilterable = 0)
        {
            string key = string.Format(CACHE_CAMPAIGN_TEMPLATE_BY_CAMPAIGN_ID_KEY, campaignId, order, isfilterable);

            return _cacheManager.Get(key, () =>
            {
                if (order == "desc")
                {
                    var query = from x in _campaignTemplateRepository.Table
                                where x.CampaignId == campaignId && (isfilterable == 0 || (isfilterable == 1 && (bool)x.IsFilterable) || (isfilterable == 2 && !(bool)x.IsFilterable))
                                orderby x.Id descending
                                select x;

                    var profiles = query.ToList();

                    return profiles;
                }
                else
                {
                    var query = from x in _campaignTemplateRepository.Table
                                where x.CampaignId == campaignId && (isfilterable == 0 || (isfilterable == 1 && (bool)x.IsFilterable) || (isfilterable == 2 && !(bool)x.IsFilterable))
                                orderby x.Id
                                select x;

                    var profiles = query.ToList();

                    return profiles;
                }
            });
        }

        /// <summary>
        /// Gets the campaign template by validator.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>CampaignTemplate.</returns>
        public CampaignField GetCampaignTemplateByValidator(long campaignId, short validator)
        {
            return (from x in _campaignTemplateRepository.Table
                    where x.CampaignId == campaignId && x.Validator == validator
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="campaignTemplate">The campaign template.</param>
        /// <exception cref="ArgumentNullException">campaignTemplate</exception>
        public virtual void DeleteCampaignTemplate(CampaignField campaignTemplate)
        {
            if (campaignTemplate == null)
                return;

            _campaignTemplateRepository.Delete(campaignTemplate);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(campaignTemplate);
        }

        #endregion Methods
    }
}