// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ISettingService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Data;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Adrack.Service.Configuration
{
    /// <summary>
    /// Represents a Setting Service
    /// </summary>
    public partial interface ISettingService
    {
        #region Methods

        /// <summary>
        /// Get Setting By Id
        /// </summary>
        /// <param name="id">Setting Identifier</param>
        /// <returns>Setting Item</returns>
        Setting GetSettingById(long id);

        /// <summary>
        /// Get Setting
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="loadSharedValueIfNotFound">Load Shared Value If Not Found</param>
        /// <returns>Setting Item</returns>
        Setting GetSetting(string key, bool loadSharedValueIfNotFound = false);

        /// <summary>
        /// Get All Settings
        /// </summary>
        /// <returns>Setting Collection Item</returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// Gets the time zone date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="user">The user.</param>
        /// <param name="tzSettings">The tz settings.</param>
        /// <returns>DateTime.</returns>
        DateTime GetTimeZoneDate(DateTime dt, User user = null, Setting tzSettings = null, double customTimeZoneHour = 0);

        /// <summary>
        /// Gets the UTC date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="user">The user.</param>
        /// <param name="tzSettings">The tz settings.</param>
        /// <returns>DateTime.</returns>
        DateTime GetUTCDate(DateTime dt, User user = null, Setting tzSettings = null, double customTimeZoneHour = 0);

        /// <summary>
        /// Get Setting By Key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="loadSharedValueIfNotFound">Load Shared Value If Not Found</param>
        /// <returns>Type Item</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T), bool loadSharedValueIfNotFound = false);

        /// <summary>
        /// Load Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Type Item</returns>
        T LoadSetting<T>() where T : ISetting, new();

        /// <summary>
        /// Insert Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">Clear Cache</param>
        void InsertSetting(Setting setting, bool clearCache = true);

        /// <summary>
        /// Set Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">Clear Cache</param>
        void SetSetting<T>(string key, T value, bool clearCache = true);

        /// <summary>
        /// Save Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        void SaveSetting<T>(T entity) where T : ISetting, new();

        /// <summary>
        /// Save Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="clearCache">Clear Cache</param>
        void SaveSetting<T, TPropType>(T entity, Expression<Func<T, TPropType>> key, bool clearCache = true) where T : ISetting, new();

        /// <summary>
        /// Update Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">Clear Cache</param>
        void UpdateSetting(Setting setting, bool clearCache = true);

        /// <summary>
        /// Setting Exists
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>Bool Item</returns>
        bool SettingExists<T, TPropType>(T entity, Expression<Func<T, TPropType>> key) where T : ISetting, new();

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        void DeleteSetting(Setting setting);

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void DeleteSetting<T>() where T : ISetting, new();

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        void DeleteSetting<T, TPropType>(T entity, Expression<Func<T, TPropType>> key) where T : ISetting, new();

        /// <summary>
        /// Clear Cache
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Execute custom query
        /// </summary>
        void ExecCustomQuery(string QueryString);

        /// <summary>
        /// AddShortFormData
        /// </summary>
        void AddShortFormData(int amount, string firstname, string zip, string email, string url = "", string country = "", string IP = "");

        void RemoveEntity(Entity entity);
        

        #endregion Methods
    }
}