// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LanguageService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Language Service
    /// Implements the <see cref="Adrack.Service.Localization.ILanguageService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Localization.ILanguageService" />
    public partial class LanguageService : ILanguageService
    {
        #region Constants

        /// <summary>
        /// Cache Language By Id Key
        /// </summary>
        private const string CACHE_LANGUAGE_BY_ID_KEY = "App.Cache.Language.By.Id-{0}";

        /// <summary>
        /// Cache Language All Key
        /// </summary>
        private const string CACHE_LANGUAGE_ALL_KEY = "App.Cache.Language.All";

        /// <summary>
        /// Cache Language Pattern Key
        /// </summary>
        private const string CACHE_LANGUAGE_PATTERN_KEY = "App.Cache.Language.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Localization Setting
        /// </summary>
        private readonly LocalizationSetting _localizationSetting;

        /// <summary>
        /// Language
        /// </summary>
        private readonly IRepository<Language> _languageRepository;

        /// <summary>
        /// Setting Service
        /// </summary>
        private readonly ISettingService _settingService;

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
        /// Language Service
        /// </summary>
        /// <param name="localizationSetting">Localization Setting</param>
        /// <param name="languageRepository">Language Repository</param>
        /// <param name="settingService">Setting Service</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public LanguageService(LocalizationSetting localizationSetting, IRepository<Language> languageRepository, ISettingService settingService, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._localizationSetting = localizationSetting;
            this._languageRepository = languageRepository;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Language By Id
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Language Item</returns>
        public virtual Language GetLanguageById(long languageId)
        {
            if (languageId == 0)
                return null;

            string key = string.Format(CACHE_LANGUAGE_BY_ID_KEY, languageId);

            return _cacheManager.Get(key, () => { return _languageRepository.GetById(languageId); });
        }

        /// <summary>
        /// Get All Languages
        /// </summary>
        /// <returns>Language Collection Item</returns>
        public virtual IList<Language> GetAllLanguages()
        {
            string key = CACHE_LANGUAGE_ALL_KEY;

            var languages = _cacheManager.Get(key, () =>
            {
                var query = from x in _languageRepository.Table
                            orderby x.DisplayOrder, x.Id
                            select x;

                return query.ToList();
            });

            return languages;
        }

        /// <summary>
        /// Insert Language
        /// </summary>
        /// <param name="language">Language</param>
        /// <exception cref="ArgumentNullException">language</exception>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            _languageRepository.Insert(language);

            _cacheManager.RemoveByPattern(CACHE_LANGUAGE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(language);
        }

        /// <summary>
        /// Update Language
        /// </summary>
        /// <param name="language">Language</param>
        /// <exception cref="ArgumentNullException">language</exception>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            _languageRepository.Update(language);

            _cacheManager.RemoveByPattern(CACHE_LANGUAGE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(language);
        }

        /// <summary>
        /// Delete Language
        /// </summary>
        /// <param name="language">Language</param>
        /// <exception cref="ArgumentNullException">language</exception>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            if (_localizationSetting.ContentManagementLanguageId == language.Id)
            {
                foreach (var languageValue in GetAllLanguages())
                {
                    if (languageValue.Id != language.Id)
                    {
                        _localizationSetting.ContentManagementLanguageId = languageValue.Id;
                        _settingService.SaveSetting(_localizationSetting);

                        break;
                    }
                }
            }

            _languageRepository.Delete(language);

            _cacheManager.RemoveByPattern(CACHE_LANGUAGE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(language);
        }

        #endregion Methods
    }
}