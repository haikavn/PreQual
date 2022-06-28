// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FormTemplateService.cs" company="Adrack.com">
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
    /// Class FormTemplateService.
    /// Implements the <see cref="Adrack.Service.Lead.IFormTemplateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IFormTemplateService" />
    public partial class FormTemplateService : IFormTemplateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_FORMTEMPLATE_BY_ID_KEY = "App.Cache.FormTemplate.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_FORMTEMPLATE_ALL_KEY = "App.Cache.FormTemplate.All";

        private const string CACHE_FORMTEMPLATE_ALL_GetAllFormTemplatesByAffiliateChannelId = "App.Cache.FormTemplate.All.GetAllFormTemplatesByAffiliateChannelId-{0}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_FORMTEMPLATE_PATTERN_KEY = "App.Cache.FormTemplate.";


        private const string CACHE_FORMTEMPLATEITEM_BY_ID_KEY = "App.Cache.FormTemplateItem.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_FORMTEMPLATE_ITEM_ALL_KEY = "App.Cache.FormTemplateItem.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_FORMTEMPLATE_ITEM_PATTERN_KEY = "App.Cache.FormTemplateItem.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<FormTemplate> _formTemplateRepository;
        private readonly IRepository<FormTemplateItem> _formTemplateItemRepository;

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
        /// <param name="formTemplateRepository">The form template repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public FormTemplateService(IRepository<FormTemplate> formTemplateRepository,IRepository<FormTemplateItem> formTemplateItemRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._formTemplateRepository = formTemplateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._formTemplateItemRepository = formTemplateItemRepository;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual FormTemplate GetFormTemplateById(long Id)
        {
            if (Id == 0)
                return null;

            return _formTemplateRepository.Table.Where(x=>x.Id==Id).FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<FormTemplate> GetAllFormTemplates()
        {
            string key = CACHE_FORMTEMPLATE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _formTemplateRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        public virtual IList<FormTemplate> GetAllFormTemplates(long affiliateChannelId)
        {
            string key = string.Format(CACHE_FORMTEMPLATE_ALL_GetAllFormTemplatesByAffiliateChannelId, affiliateChannelId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _formTemplateRepository.Table
                            where x.AffiliateChannelId == affiliateChannelId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }


        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">formTemplate</exception>
        public virtual long InsertFormTemplate(FormTemplate formTemplate)
        {
            if (formTemplate == null)
                throw new ArgumentNullException("formTemplate");

            _formTemplateRepository.Insert(formTemplate);

            _cacheManager.RemoveByPattern(CACHE_FORMTEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(formTemplate);

            return formTemplate.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        /// <exception cref="ArgumentNullException">formTemplate</exception>
        public virtual void UpdateFormTemplate(FormTemplate formTemplate)
        {
            if (formTemplate == null)
                throw new ArgumentNullException("formTemplate");

            _formTemplateRepository.SetCanTrackChanges(true);

            _formTemplateRepository.Update(formTemplate);

            _appEventPublisher.EntityUpdated(formTemplate);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        /// <exception cref="ArgumentNullException">formTemplate</exception>
        public virtual void DeleteFormTemplate(FormTemplate formTemplate)
        {
            if (formTemplate == null)
                throw new ArgumentNullException("formTemplate");

            _formTemplateRepository.Delete(formTemplate);

            _cacheManager.RemoveByPattern(CACHE_FORMTEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(formTemplate);
        }

        public FormTemplateItem GetFormTemplateItemById(long Id)
        {
            if (Id == 0)
                throw new ArgumentNullException("id");
            return _formTemplateItemRepository.Table.Where(x => x.Id == Id).FirstOrDefault();
        }

        public IList<FormTemplateItem> GetFormTemplateItemsByTemplateId(long Id)
        {
            if (Id == 0)
                throw new ArgumentNullException("id");
                return _formTemplateItemRepository.Table.Where(x => x.FormTemplateId == Id).OrderByDescending(x=>x.Id).ToList();
        }


        public long InsertFormTemplateItem(FormTemplateItem formTemplateItem)
        {
            if (formTemplateItem == null)
                throw new ArgumentNullException("formTemplate");



            _formTemplateItemRepository.Insert(formTemplateItem);
            _cacheManager.RemoveByPattern(CACHE_FORMTEMPLATE_ITEM_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();

            _appEventPublisher.EntityInserted(formTemplateItem);

            return formTemplateItem.Id;
        }

        public void UpdateFormTemplateItem(FormTemplateItem formTemplateItem)
        {
            if (formTemplateItem == null)
                throw new ArgumentNullException("formTemplate");
            _formTemplateRepository.SetCanTrackChanges(true);
            _formTemplateItemRepository.Update(formTemplateItem);
           // _cacheManager.RemoveByPattern(CACHE_FORMTEMPLATE_ITEM_PATTERN_KEY);
            _appEventPublisher.EntityUpdated(formTemplateItem);
        }

        public void DeleteFormTemplateItem(FormTemplateItem formTemplateItem)
        {
            if (formTemplateItem == null)
                throw new ArgumentNullException("formTemplateItem");

            _formTemplateItemRepository.Delete(formTemplateItem);
            _cacheManager.RemoveByPattern(CACHE_FORMTEMPLATE_ITEM_PATTERN_KEY);
            _appEventPublisher.EntityDeleted(formTemplateItem);
        }

        public void DeleteFormTemplateItems(FormTemplate formTemplate)
        {
            var items = GetFormTemplateItemsByTemplateId(formTemplate.Id);
            foreach (var item in items)
            {
                DeleteFormTemplateItem(item);
            }
        }

        #endregion Methods
    }
}