// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LocalizedPropertyService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Localized Property Service
    /// Implements the <see cref="Adrack.Service.Localization.ILocalizedPropertyService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Localization.ILocalizedPropertyService" />
    public partial class LocalizedPropertyService : ILocalizedPropertyService
    {
        #region Nested Class

        /// <summary>
        /// Represents a Cache Localized Property
        /// </summary>
        [Serializable]
        public class CacheLocalizedProperty
        {
            #region Properties

            /// <summary>
            /// Gets or Sets the Localized Property Identifier
            /// </summary>
            /// <value>The identifier.</value>
            public long Id { get; set; }

            /// <summary>
            /// Gets or Sets the Entity Identifier
            /// </summary>
            /// <value>The entity identifier.</value>
            public long EntityId { get; set; }

            /// <summary>
            /// Gets or Sets the Language Identifier
            /// </summary>
            /// <value>The language identifier.</value>
            public long LanguageId { get; set; }

            /// <summary>
            /// Gets or Sets the Key Group
            /// </summary>
            /// <value>The key group.</value>
            public string KeyGroup { get; set; }

            /// <summary>
            /// Gets or Sets the Key
            /// </summary>
            /// <value>The key.</value>
            public string Key { get; set; }

            /// <summary>
            /// Gets or Sets the Value
            /// </summary>
            /// <value>The value.</value>
            public string Value { get; set; }

            #endregion Properties
        }

        #endregion Nested Class

        #region Constants

        /// <summary>
        /// Cache Localized Property By Id Key
        /// </summary>
        private const string CACHE_LOCALIZEDPROPERTY_BY_ID_KEY = "App.Cache.LocalizedProperty.By.Id-{0}";

        /// <summary>
        /// Cache Localized Property By Entity Id, Language Id, Key Group, Key, Key
        /// </summary>
        private const string CACHE_LOCALIZEDPROPERTY_BY_ENTITY_ID_LANGUAGE_ID_KEY_GROUP_KEY_KEY = "App.Cache.LocalizedProperty.By.Entity.Id-{0}.Language.Id-{1}.Key.Group-{2}.Key-{3}";

        /// <summary>
        /// Cache Localized Property All Key
        /// </summary>
        private const string CACHE_LOCALIZEDPROPERTY_ALL_KEY = "App.Cache.LocalizedProperty.All";

        /// <summary>
        /// Cache Localized Property Pattern Key
        /// </summary>
        private const string CACHE_LOCALIZEDPROPERTY_PATTERN_KEY = "App.Cache.LocalizedProperty.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Localization Setting
        /// </summary>
        private readonly LocalizationSetting _localizationSetting;

        /// <summary>
        /// Localized Property
        /// </summary>
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get Localized Properties
        /// </summary>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="keyGroup">Key Group</param>
        /// <returns>Localized Property Collection Item</returns>
        protected virtual IList<LocalizedProperty> GetLocalizedProperties(long entityId, string keyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(keyGroup))
                return new List<LocalizedProperty>();

            var query = from x in _localizedPropertyRepository.Table
                        orderby x.Id
                        where x.EntityId == entityId && x.KeyGroup == keyGroup
                        select x;

            var localizedProperties = query.ToList();

            return localizedProperties;
        }

        /// <summary>
        /// Get All Cache Localized Properties
        /// </summary>
        /// <returns>Cache Localized Property Collection Item</returns>
        protected virtual IList<CacheLocalizedProperty> GetAllCacheLocalizedProperties()
        {
            string key = string.Format(CACHE_LOCALIZEDPROPERTY_ALL_KEY);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _localizedPropertyRepository.Table
                            select x;

                var localizedProperties = query.ToList();

                var _cacheLocalizedProperty = new List<CacheLocalizedProperty>();

                foreach (var x in localizedProperties)
                {
                    var cacheLocalizedProperty = new CacheLocalizedProperty()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        LanguageId = x.LanguageId,
                        KeyGroup = x.KeyGroup,
                        Key = x.Key,
                        Value = x.Value
                    };

                    _cacheLocalizedProperty.Add(cacheLocalizedProperty);
                }

                return _cacheLocalizedProperty;
            });
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Localized Property Service
        /// </summary>
        /// <param name="localizationSetting">Localization Setting</param>
        /// <param name="localizedPropertyRepository">Localized Property Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        public LocalizedPropertyService(LocalizationSetting localizationSetting, IRepository<LocalizedProperty> localizedPropertyRepository, ICacheManager cacheManager)
        {
            this._localizationSetting = localizationSetting;
            this._localizedPropertyRepository = localizedPropertyRepository;
            this._cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Localized Property By Id
        /// </summary>
        /// <param name="localizedPropertyId">Localized Property Identifier</param>
        /// <returns>Localized Property Item</returns>
        public virtual LocalizedProperty GetLocalizedPropertyById(long localizedPropertyId)
        {
            if (localizedPropertyId == 0)
                return null;

            string key = string.Format(CACHE_LOCALIZEDPROPERTY_BY_ID_KEY, localizedPropertyId);

            return _cacheManager.Get(key, () => { return _localizedPropertyRepository.GetById(localizedPropertyId); });
        }

        /// <summary>
        /// Get Localized Value
        /// </summary>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="keyGroup">Key Group</param>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        public virtual string GetLocalizedPropertyValue(long entityId, long languageId, string keyGroup, string key)
        {
            if (_localizationSetting.LoadAllLocalizedPropertiesOnStartup)
            {
                string keyValue = string.Format(CACHE_LOCALIZEDPROPERTY_BY_ENTITY_ID_LANGUAGE_ID_KEY_GROUP_KEY_KEY, entityId, languageId, keyGroup, key);

                return _cacheManager.Get(keyValue, () =>
                {
                    var query = from x in GetAllCacheLocalizedProperties()
                                where x.EntityId == entityId &&
                                      x.LanguageId == languageId &&
                                      x.KeyGroup == keyGroup &&
                                      x.Key == key
                                select x.Value;

                    var localizedProperties = query.FirstOrDefault();

                    if (localizedProperties == null)
                        localizedProperties = "";

                    return localizedProperties;
                });
            }
            else
            {
                string keyValue = string.Format(CACHE_LOCALIZEDPROPERTY_BY_ENTITY_ID_LANGUAGE_ID_KEY_GROUP_KEY_KEY, entityId, languageId, keyGroup, key);

                return _cacheManager.Get(keyValue, () =>
                {
                    var query = from x in _localizedPropertyRepository.Table
                                where x.EntityId == entityId &&
                                      x.LanguageId == languageId &&
                                      x.KeyGroup == keyGroup &&
                                      x.Key == key
                                select x.Value;

                    var localizedProperties = query.FirstOrDefault();

                    if (localizedProperties == null)
                        localizedProperties = "";

                    return localizedProperties;
                });
            }
        }

        /// <summary>
        /// Get All Localized Properties
        /// </summary>
        /// <returns>Localized Property Collection Item</returns>
        public virtual IList<LocalizedProperty> GetAllLocalizedProperties()
        {
            string key = CACHE_LOCALIZEDPROPERTY_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _localizedPropertyRepository.Table
                            orderby x.EntityId, x.Id
                            select x;

                var localizedProperties = query.ToList();

                return localizedProperties;
            });
        }

        /// <summary>
        /// Insert Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        /// <exception cref="ArgumentNullException">localizedProperty</exception>
        public virtual void InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            _localizedPropertyRepository.Insert(localizedProperty);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        /// <summary>
        /// Update Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        /// <exception cref="ArgumentNullException">localizedProperty</exception>
        public virtual void UpdateLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            _localizedPropertyRepository.Update(localizedProperty);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        /// <summary>
        /// Save Localized Property Value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public virtual void SaveLocalizedPropertyValue<T>(T entity, long languageId, Expression<Func<T, string>> key, string value) where T : BaseEntity, ILocalizedEntity
        {
            SaveLocalizedPropertyValue<T, string>(entity, languageId, key, value);
        }

        /// <summary>
        /// Save Localized Property Value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Type Property</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <exception cref="ArgumentNullException">entity
        /// or
        /// languageId - Language ID should not be 0
        /// or
        /// or</exception>
        public virtual void SaveLocalizedPropertyValue<T, TPropType>(T entity, long languageId, Expression<Func<T, TPropType>> key, TPropType value) where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (languageId == 0)
                throw new ArgumentNullException("languageId", "Language ID should not be 0");

            var memberExpression = key.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentNullException(string.Format("Expression '{0}' refers to a method, not a property.", key));
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentNullException(string.Format("Expression '{0}' refers to a method, not a property.", key));
            }

            string keyGroupValue = typeof(T).Name;
            string keyValue = propertyInfo.Name;

            var getLocalizedProperties = GetLocalizedProperties(entity.Id, keyGroupValue);
            var getLocalizedProperty = getLocalizedProperties.FirstOrDefault(x => x.LanguageId == languageId && x.Key.Equals(keyValue, StringComparison.InvariantCultureIgnoreCase));

            string localizedValueString = CommonHelper.To<string>(keyValue);

            if (getLocalizedProperty != null)
            {
                if (string.IsNullOrWhiteSpace(localizedValueString))
                {
                    DeleteLocalizedProperty(getLocalizedProperty);
                }
                else
                {
                    getLocalizedProperty.Value = localizedValueString;

                    UpdateLocalizedProperty(getLocalizedProperty);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localizedValueString))
                {
                    getLocalizedProperty = new LocalizedProperty()
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        KeyGroup = keyGroupValue,
                        Key = keyValue,
                        Value = localizedValueString
                    };

                    InsertLocalizedProperty(getLocalizedProperty);
                }
            }
        }

        /// <summary>
        /// Delete Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        /// <exception cref="ArgumentNullException">localizedProperty</exception>
        public virtual void DeleteLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            _localizedPropertyRepository.Delete(localizedProperty);

            _cacheManager.RemoveByPattern(CACHE_LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        #endregion Methods
    }
}