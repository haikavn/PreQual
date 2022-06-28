// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LocalizedStringExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Service.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Localized String Extensions
    /// </summary>
    public static class LocalizedStringExtensions
    {
        #region Methods

        /// <summary>
        /// Get Localized
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> key) where T : BaseEntity, ILocalizedEntity
        {
            var appContext = AppEngineContext.Current.Resolve<IAppContext>();

            return GetLocalized(entity, key, appContext.AppLanguage.Id);
        }

        /// <summary>
        /// Get Localized
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="returnDefaultValue">Return Default Value</param>
        /// <param name="ensureTwoPublishedLanguages">Ensure Two Published Languages</param>
        /// <returns>String Item</returns>
        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> key, long languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) where T : BaseEntity, ILocalizedEntity
        {
            return GetLocalized<T, string>(entity, key, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// Get Localized
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="returnDefaultValue">Return Default Value</param>
        /// <param name="ensureTwoPublishedLanguages">Ensure Two Published Languages</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentNullException">entity</exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static TPropType GetLocalized<T, TPropType>(this T entity, Expression<Func<T, TPropType>> key, long languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var member = key.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", key));
            }

            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", key));
            }

            TPropType result = default(TPropType);
            string resultStr = string.Empty;

            // Load localized value
            string localizedPropertyKeyGroup = typeof(T).Name;
            string localizedPropertyKey = propertyInfo.Name;

            if (languageId > 0)
            {
                // Ensure that we have at least two published languages
                bool loadLocalizedValue = true;

                if (ensureTwoPublishedLanguages)
                {
                    var languageService = AppEngineContext.Current.Resolve<ILanguageService>();
                    var totalPublishedLanguages = languageService.GetAllLanguages().Count;

                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                // Localized value
                if (loadLocalizedValue)
                {
                    var localizedPropertyService = AppEngineContext.Current.Resolve<ILocalizedPropertyService>();

                    resultStr = localizedPropertyService.GetLocalizedPropertyValue(languageId, entity.Id, localizedPropertyKeyGroup, localizedPropertyKey);

                    if (!String.IsNullOrEmpty(resultStr))
                        result = CommonHelper.To<TPropType>(resultStr);
                }
            }

            // Set default value if required
            if (String.IsNullOrEmpty(resultStr) && returnDefaultValue)
            {
                var localizer = key.Compile();

                result = localizer(entity);
            }

            return result;
        }

        /// <summary>
        /// Get Localized Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="returnDefaultValue">Return Default Value</param>
        /// <param name="ensureTwoPublishedLanguages">Ensure Two Published Languages</param>
        /// <returns>String Item</returns>
        public static string GetLocalizedSetting<T>(this T entity, Expression<Func<T, string>> key, long languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true) where T : ISetting, new()
        {
            var settingService = AppEngineContext.Current.Resolve<ISettingService>();

            string settingKey = entity.GetSettingKey(key);

            var setting = settingService.GetSetting(settingKey, loadSharedValueIfNotFound: false);

            if (setting == null)
                return null;

            return setting.GetLocalized(x => x.Value, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// Save Localized Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="value">Value</param>
        public static void SaveLocalizedSetting<T>(this T entity, Expression<Func<T, string>> key, long languageId, string value) where T : ISetting, new()
        {
            var settingService = AppEngineContext.Current.Resolve<ISettingService>();

            var localizedPropertyService = AppEngineContext.Current.Resolve<ILocalizedPropertyService>();

            string keyResult = entity.GetSettingKey(key);

            var setting = settingService.GetSetting(keyResult, loadSharedValueIfNotFound: false);

            if (setting == null)
                return;

            localizedPropertyService.SaveLocalizedPropertyValue(setting, languageId, x => x.Value, value);
        }

        /// <summary>
        /// Get Localized Enum
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="enumValue">Enumeration Value Type</param>
        /// <param name="localizedStringService">Localized Property Service</param>
        /// <param name="appContext">Application Context</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentNullException">appContext</exception>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizedStringService localizedStringService, IAppContext appContext) where T : struct
        {
            if (appContext == null)
                throw new ArgumentNullException("appContext");

            return GetLocalizedEnum(enumValue, localizedStringService, appContext.AppLanguage.Id);
        }

        /// <summary>
        /// Get Localized Enum
        /// </summary>
        /// <typeparam name="T">Enumeration Type</typeparam>
        /// <param name="enumValue">Enumeration Value Type</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>String Value</returns>
        /// <exception cref="ArgumentNullException">localizedStringService</exception>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizedStringService localizedStringService, long languageId) where T : struct
        {
            if (localizedStringService == null)
                throw new ArgumentNullException("localizedStringService");

            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            string key = string.Format("Enums.{0}.{1}", typeof(T).ToString(), enumValue.ToString()); /* Convert.ToInt32(enumValue) */

            string result = localizedStringService.GetLocalizedString(languageId, key, "", true);

            if (String.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        /// <summary>
        /// Get Localized Permission Name
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="appContext">Application Context</param>
        /// <returns>String Value</returns>
        /// <exception cref="ArgumentNullException">appContext</exception>
        public static string GetLocalizedPermissionName(this Permission permission, ILocalizedStringService localizedStringService, IAppContext appContext)
        {
            if (appContext == null)
                throw new ArgumentNullException("appContext");

            return GetLocalizedPermissionName(permission, localizedStringService, appContext.AppLanguage.Id);
        }

        /// <summary>
        /// Get Localized Permission Name
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>String Value</returns>
        /// <exception cref="ArgumentNullException">permission
        /// or
        /// localizedStringService</exception>
        public static string GetLocalizedPermissionName(this Permission permission, ILocalizedStringService localizedStringService, long languageId)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            if (localizedStringService == null)
                throw new ArgumentNullException("localizedStringService");

            string key = string.Format("Permission.{0}", permission.Key);

            string result = localizedStringService.GetLocalizedString(languageId, key, "", true);

            if (String.IsNullOrEmpty(result))
                result = permission.Name;

            return result;
        }

        /// <summary>
        /// Save Localized Permission Name
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="languageService">Language Service</param>
        /// <exception cref="ArgumentNullException">permission
        /// or
        /// localizedStringService
        /// or
        /// languageService</exception>
        public static void SaveLocalizedPermissionName(this Permission permission, ILocalizedStringService localizedStringService, ILanguageService languageService)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            if (localizedStringService == null)
                throw new ArgumentNullException("localizedStringService");

            if (languageService == null)
                throw new ArgumentNullException("languageService");

            string key = string.Format("Permission.{0}", permission.Key);

            string value = permission.Name;

            foreach (var language in languageService.GetAllLanguages())
            {
                var localizedString = localizedStringService.GetLocalizedStringByKey(language.Id, key);

                if (localizedString == null)
                {
                    localizedString = new LocalizedString
                    {
                        LanguageId = language.Id,
                        Key = key,
                        Value = value
                    };

                    localizedStringService.InsertLocalizedString(localizedString);
                }
                else
                {
                    localizedString.Value = value;
                    localizedStringService.UpdateLocalizedString(localizedString);
                }
            }
        }

        /// <summary>
        /// Delete Localized Permission Name
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="languageService">Language Service</param>
        /// <exception cref="ArgumentNullException">permission
        /// or
        /// localizedStringService
        /// or
        /// languageService</exception>
        public static void DeleteLocalizedPermissionName(this Permission permission, ILocalizedStringService localizedStringService, ILanguageService languageService)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            if (localizedStringService == null)
                throw new ArgumentNullException("localizedStringService");

            if (languageService == null)
                throw new ArgumentNullException("languageService");

            string key = string.Format("Permission.{0}", permission.Key);

            foreach (var language in languageService.GetAllLanguages())
            {
                var localizedString = localizedStringService.GetLocalizedStringByKey(language.Id, key);

                if (localizedString != null)
                    localizedStringService.DeleteLocalizedString(localizedString);
            }
        }

        #endregion Methods
    }
}