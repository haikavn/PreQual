// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LocalizedStringService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Audit;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Localized String Service
    /// Implements the <see cref="Adrack.Service.Localization.ILocalizedStringService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Localization.ILocalizedStringService" />
    public partial class LocalizedStringService : ILocalizedStringService
    {
        #region Constants

        /// <summary>
        /// Cache Localized String By Id Key
        /// </summary>
        private const string CACHE_LOCALIZEDSTRING_BY_ID_KEY = "App.Cache.LocalizedString.By.Id-{0}";

        /// <summary>
        /// Cache Localized String By Language Id Key
        /// </summary>
        private const string CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY = "App.Cache.LocalizedString.By.Language.Id-{0}";

        /// <summary>
        /// Cache Localized String By Language Id, Key Key
        /// </summary>
        private const string CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY_KEY = "App.Cache.LocalizedString.By.Language.Id-{0}.Key-{1}";

        /// <summary>
        /// Cache Localized String All Key
        /// </summary>
        private const string CACHE_LOCALIZEDSTRING_ALL_KEY = "App.Cache.LocalizedString.All";

        /// <summary>
        /// Cache Localized String Pattern Key
        /// </summary>
        private const string CACHE_LOCALIZEDSTRING_PATTERN_KEY = "App.Cache.LocalizedString.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Localization Setting
        /// </summary>
        private readonly LocalizationSetting _localizationSetting;

        /// <summary>
        /// Localized String
        /// </summary>
        private readonly IRepository<LocalizedString> _localizedStringRepository;

        /// <summary>
        /// Language Service
        /// </summary>
        private readonly ILanguageService _languageService;

        /// <summary>
        /// Log Service
        /// </summary>
        private readonly ILogService _logService;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Data Provider
        /// </summary>
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
        /// Localized String Service
        /// </summary>
        /// <param name="localizationSetting">Localization Setting</param>
        /// <param name="localizedStringRepository">Localized String Repository</param>
        /// <param name="languageService">Language Service</param>
        /// <param name="logService">Log Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="dataProvider">Data Provider</param>
        /// <param name="dbContext">Db Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public LocalizedStringService(LocalizationSetting localizationSetting, IRepository<LocalizedString> localizedStringRepository, ILanguageService languageService, ILogService logService, IAppContext appContext, IDataProvider dataProvider, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._localizationSetting = localizationSetting;
            this._localizedStringRepository = localizedStringRepository;
            this._languageService = languageService;
            this._logService = logService;
            this._appContext = appContext;
            this._dataProvider = dataProvider;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Localized String By Id
        /// </summary>
        /// <param name="localizedStringId">Localized String Identifier</param>
        /// <returns>Localized String Item</returns>
        public virtual LocalizedString GetLocalizedStringById(long localizedStringId)
        {
            if (localizedStringId == 0)
                return null;

            string key = string.Format(CACHE_LOCALIZEDSTRING_BY_ID_KEY, localizedStringId);

            return _cacheManager.Get(key, () => { return _localizedStringRepository.GetById(localizedStringId); });
        }

        /// <summary>
        /// Get Localized String By Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Localized String Item</returns>
        public virtual LocalizedString GetLocalizedStringByKey(string key)
        {
            if (_appContext.AppLanguage != null)
                return GetLocalizedStringByKey(_appContext.AppLanguage.Id, key);

            return null;
        }

        /// <summary>
        /// Get Localized String By Key
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <returns>Localized String Item</returns>
        public virtual LocalizedString GetLocalizedStringByKey(long languageId, string key)
        {
            string keyValue = string.Format(CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY_KEY, languageId, key);

            return _cacheManager.Get(keyValue, () =>
            {
                var query = from x in _localizedStringRepository.Table
                            orderby x.Key
                            where x.LanguageId == languageId && x.Key == key
                            select x;

                var localizedStrings = query.FirstOrDefault();

                if (localizedStrings == null)
                    _logService.Warning(string.Format("Localized string ({0}) not found. Language Id = {1}", key, languageId));

                return localizedStrings;
            });
        }

        /// <summary>
        /// Get All Localized Strings
        /// </summary>
        /// <returns>Localized String Collection Item</returns>
        public virtual IList<LocalizedString> GetAllLocalizedStrings()
        {
            string key = CACHE_LOCALIZEDSTRING_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _localizedStringRepository.Table
                            orderby x.LanguageId, x.Id
                            select x;

                var localizedStrings = query.ToList();

                return localizedStrings;
            });
        }

        /// <summary>
        /// Get All Localized Strings
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Localized String Collection Item</returns>
        public virtual IList<LocalizedString> GetAllLocalizedStrings(long languageId)
        {
            string key = string.Format(CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY, languageId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _localizedStringRepository.Table
                            orderby x.Key
                            where x.LanguageId == languageId
                            select x;

                var localizedStrings = query.ToList();

                return localizedStrings;
            });
        }

        /// <summary>
        /// Get All Localized String Values
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Localized String Collection Item</returns>
        public virtual Dictionary<string, KeyValuePair<long, string>> GetAllLocalizedStringValues(long languageId)
        {
            string keyValue = string.Format(CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY, languageId);

            return _cacheManager.Get(keyValue, () =>
            {
                var query = from x in _localizedStringRepository.TableNoTracking
                            orderby x.Key
                            where x.LanguageId == languageId
                            select x;

                var localizedStrings = query.ToList();

                var dictionary = new Dictionary<string, KeyValuePair<long, string>>();

                foreach (var locale in localizedStrings)
                {
                    var key = locale.Key.ToLowerInvariant();

                    if (!dictionary.ContainsKey(key))
                        dictionary.Add(key, new KeyValuePair<long, string>(locale.Id, locale.Value));
                }

                return dictionary;
            });
        }

        /// <summary>
        /// Get Localized String
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        public virtual string GetLocalizedString(string key)
        {
            if (_appContext.AppLanguage != null)
                return GetLocalizedString(_appContext.AppLanguage.Id, key);

            return "";
        }

        /// <summary>
        /// Get Localized String
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="returnEmptyIfNotFound">Return Empty If Not Found</param>
        /// <returns>String Item</returns>
        public virtual string GetLocalizedString(long languageId, string key, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = string.Empty;

            if (key == null)
                key = string.Empty;

            key = key.Trim().ToLowerInvariant();

            if (_localizationSetting.LoadAllLocalizedStringsOnStartup)
            {
                var keys = GetAllLocalizedStringValues(languageId);

                if (keys.ContainsKey(key))
                {
                    result = keys[key].Value;
                }
            }
            else
            {
                string keyValue = string.Format(CACHE_LOCALIZEDSTRING_BY_LANGUAGE_ID_KEY_KEY, languageId, key);

                string localizedStrings = _cacheManager.Get(keyValue, () =>
                {
                    var query = from x in _localizedStringRepository.TableNoTracking
                                where x.LanguageId == languageId && x.Key == key
                                select x.Value;

                    return query.FirstOrDefault();
                });

                if (localizedStrings != null)
                    result = localizedStrings;
            }

            if (String.IsNullOrEmpty(result))
            {
                _logService.Warning(string.Format("Localized string ({0}) not found. Language Id = {1}", key, languageId));

                if (!String.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = key;
                }
            }

            return result;
        }

        /// <summary>
        /// Insert Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        /// <exception cref="ArgumentNullException">localizedString</exception>
        public virtual void InsertLocalizedString(LocalizedString localizedString)
        {
            if (localizedString == null)
                throw new ArgumentNullException("localizedString");

            _localizedStringRepository.Insert(localizedString);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDSTRING_PATTERN_KEY);

            _appEventPublisher.EntityInserted(localizedString);
        }

        /// <summary>
        /// Update Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        /// <exception cref="ArgumentNullException">localizedString</exception>
        public virtual void UpdateLocalizedString(LocalizedString localizedString)
        {
            if (localizedString == null)
                throw new ArgumentNullException("localizedString");

            _localizedStringRepository.Update(localizedString);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDSTRING_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(localizedString);
        }

        /// <summary>
        /// Delete Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        /// <exception cref="ArgumentNullException">localizedString</exception>
        public virtual void DeleteLocalizedString(LocalizedString localizedString)
        {
            if (localizedString == null)
                throw new ArgumentNullException("localizedString");

            _localizedStringRepository.Delete(localizedString);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDSTRING_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(localizedString);
        }

        #endregion Methods
    }
}