// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SettingService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Data;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Adrack.Service.Configuration
{
    /// <summary>
    /// Represents a Setting Service
    /// Implements the <see cref="Adrack.Service.Configuration.ISettingService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Configuration.ISettingService" />
    public partial class SettingService : ISettingService
    {
        #region Nested Class

        /// <summary>
        /// Represents a Cache Setting
        /// </summary>
        [Serializable]
        public class CacheSetting
        {
            #region Properties

            /// <summary>
            /// Gets or Sets the Setting Identifier
            /// </summary>
            /// <value>The identifier.</value>
            public long Id { get; set; }

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

            /// <summary>
            /// Gets or Sets the Description
            /// </summary>
            /// <value>The description.</value>
            public string Description { get; set; }

            #endregion Properties
        }

        #endregion Nested Class

        #region Constants

        /// <summary>
        /// Cache Setting All Key
        /// </summary>
        private const string CACHE_SETTING_ALL_KEY = "App.Cache.Setting.All";
        private const string CACHE_SETTING_GetSetting = "App.Cache.Setting.GetSetting-{0}";

        /// <summary>
        /// Cache Setting Pattern Key
        /// </summary>

        private const string CACHE_SETTING_PATTERN_KEY = "App.Cache.Setting.";
        private const string CACHE_SETTING_PATTERN_KEY_SYSTEM = "System.";
        private const string CACHE_SETTING_PATTERN_KEY_USER = "UserSetting.";
        private const string CACHE_SETTING_PATTERN_KEY_SETTINGS = "Setting.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Setting
        /// </summary>
        private readonly IRepository<Setting> _settingRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get All Settings Cached
        /// </summary>
        /// <returns>Dictionary Collection Item</returns>
        protected virtual IDictionary<string, IList<CacheSetting>> GetAllSettingsCached()
        {
            string key = string.Format(CACHE_SETTING_ALL_KEY);

            return _cacheManager.Get(key, () =>
            {
                List<Setting> settings = new List<Setting>();

                try
                {
                    var query = from s in _settingRepository.Table
                                orderby s.Key
                                select s;

                    settings = query.ToList();
                }
                catch(Exception ex)
                {
                }

                var dictionary = new Dictionary<string, IList<CacheSetting>>();

                foreach (var settingValue in settings)
                {
                    var resourceName = settingValue.Key.ToLowerInvariant();

                    var cacheSetting = new CacheSetting()
                    {
                        Id = settingValue.Id,
                        Key = settingValue.Key,
                        Value = settingValue.Value,
                        Description = settingValue.Description
                    };

                    if (!dictionary.ContainsKey(resourceName))
                    {
                        dictionary.Add(resourceName, new List<CacheSetting>()
                        {
                            cacheSetting
                        });
                    }
                    else
                    {
                        dictionary[resourceName].Add(cacheSetting);
                    }
                }

                return dictionary;
            });
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Setting Service
        /// </summary>
        /// <param name="settingRepository">Setting Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">Application Context for Data Access</param>
        /// <param name="dataProvider">Application Data Provider Service</param>
        public SettingService(IRepository<Setting> settingRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._settingRepository = settingRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Setting By Id
        /// </summary>
        /// <param name="id">Setting Identifier</param>
        /// <returns>Setting Item</returns>
        public virtual Setting GetSettingById(long id)
        {
            if (id == 0)
                return null;

            return _settingRepository.GetById(id);
        }



        /// <summary>
        /// Get Setting
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="loadSharedValueIfNotFound">Load Shared Value If Not Found</param>
        /// <returns>Setting Item</returns>
        public virtual Setting GetSetting(string key, bool loadSharedValueIfNotFound = false)
        {
            string key2 = string.Format(CACHE_SETTING_GetSetting, key);

            return _cacheManager.Get(key2, () =>
            {
                return (from x in _settingRepository.Table
                        where x.Key == key
                        select x).FirstOrDefault();

            });


            /*if (String.IsNullOrEmpty(key))
                return null;

            var allSettingsCached = GetAllSettingsCached();

            if (allSettingsCached.Count == 0)
            {
                this.ClearCache();
                allSettingsCached = GetAllSettingsCached();
            }

            key = key.Trim().ToLowerInvariant();

            if (allSettingsCached.ContainsKey(key))
            {
                var settingByKey = allSettingsCached[key];

                var settingCached = settingByKey.FirstOrDefault();

                if (settingCached == null && loadSharedValueIfNotFound)

                    settingCached = settingByKey.FirstOrDefault();

                var Setting = new Setting()
                {
                    Id = settingCached.Id,
                    Key = settingCached.Key,
                    Value = settingCached.Value,
                    Description = settingCached.Description
                };
                return Setting;
            }

            return null;*/
        }


        /// <summary>
        /// Get Setting
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="loadSharedValueIfNotFound">Load Shared Value If Not Found</param>
        /// <returns>Setting Item</returns>
        public virtual CacheSetting GetSettingCache(string key, bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            var allSettingsCached = GetAllSettingsCached();

            key = key.Trim().ToLowerInvariant();

            if (allSettingsCached.ContainsKey(key))
            {
                var settingByKey = allSettingsCached[key];

                var settingCached = settingByKey.FirstOrDefault();

                if (settingCached == null && loadSharedValueIfNotFound)

                    settingCached = settingByKey.FirstOrDefault();

                
                return settingCached;
            }

            return null;
        }
        /// <summary>
        /// Get All Settings
        /// </summary>
        /// <returns>Setting Collection Item</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            string key = CACHE_SETTING_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _settingRepository.Table
                            orderby x.Key
                            select x;

                var setting = query.ToList();

                return setting;
            });
        }

        /// <summary>
        /// Get Setting By Key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="loadSharedValueIfNotFound">Load Shared Value If Not Found</param>
        /// <returns>Type Item</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T), bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();

            key = key.Trim().ToLowerInvariant();

            if (settings.ContainsKey(key))
            {
                var settingByKey = settings[key];

                var setting = settingByKey.FirstOrDefault();

                if (setting == null && loadSharedValueIfNotFound)
                    setting = settingByKey.FirstOrDefault();

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the UTC date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="user">The user.</param>
        /// <param name="tzSettings">The tz settings.</param>
        /// <returns>DateTime.</returns>
        public virtual DateTime GetUTCDate(DateTime dt, User user = null, Setting tzSettings = null, double customTimeZoneHour = 0)
        {
            string timeZone = (user != null ? user.TimeZone : null);

            if (customTimeZoneHour == 0)
            {
                Setting setting = tzSettings;

                if (setting == null)
                    setting = GetSetting("TimeZone");

                if (setting != null && string.IsNullOrEmpty(timeZone))
                {
                    timeZone = setting.Value;
                }
                else
                    if (string.IsNullOrEmpty(timeZone))
                    //return DateTime.UtcNow;
                    return dt;
            }
            else
                timeZone = customTimeZoneHour.ToString();

            double dc = 0;
            if (double.TryParse(timeZone, out dc))
            {
                return dt.AddHours(-dc);
            }
            else
            {
                try
                {
                    TimeZoneInfo s = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    TimeZoneInfo d = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime now = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Unspecified);

                    return TimeZoneInfo.ConvertTime(now, s, d);
                }
                catch
                {
                    return dt;
                    //return DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Gets the time zone date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="user">The user.</param>
        /// <param name="tzSettings">The tz settings.</param>
        /// <returns>DateTime.</returns>
        public virtual DateTime GetTimeZoneDate(DateTime dt, User user = null, Setting tzSettings = null, double customTimeZoneHour = 0)
        {
            string timeZone = (user != null ? user.TimeZone : null);

            if (customTimeZoneHour == 0)
            {
                Setting setting = tzSettings;

                if (setting == null)
                {
                    //this.ClearCache();
                    setting = GetSetting("TimeZoneStr");
                    if (setting == null)
                    {
                        setting = GetSetting("TimeZone");
                    }
                }

                if (setting != null && string.IsNullOrEmpty(timeZone))
                {
                    timeZone = setting.Value;
                }
                else
                    if (string.IsNullOrEmpty(timeZone))
                    return dt;
            }
            else
                timeZone = customTimeZoneHour.ToString();

            double dc = 0;
            if (double.TryParse(timeZone, out dc))
            {
                return dt.AddHours(dc);
            }
            else
            {
                try
                {
                    TimeZoneInfo s = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    TimeZoneInfo d = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime now = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Utc);

                    return TimeZoneInfo.ConvertTime(now, s);
                }
                catch
                {
                    return dt;
                }
            }
        }

        /// <summary>
        /// Load Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Type Item</returns>
        public virtual T LoadSetting<T>() where T : ISetting, new()
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var property in typeof(T).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + property.Name;

                string setting = GetSettingByKey<string>(key, loadSharedValueIfNotFound: true);

                if (setting == null)
                    continue;

                if (!CommonHelper.GetAppCustomTypeConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!CommonHelper.GetAppCustomTypeConverter(property.PropertyType).IsValid(setting))
                    continue;

                object value = CommonHelper.GetAppCustomTypeConverter(property.PropertyType).ConvertFromInvariantString(setting);

                property.SetValue(settings, value, null);
            }

            return settings;
        }

        /// <summary>
        /// Insert Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">Clear Cache</param>
        /// <exception cref="ArgumentNullException">setting</exception>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            _settingRepository.Insert(setting);

            if (clearCache)
            {
                ClearCache();
            }
            //if (setting.Key!="TimeZone")
              //  _cacheManager.ClearRemoteServersCache();

            _appEventPublisher.EntityInserted(setting);
        }

        /// <summary>
        /// Set Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">Clear Cache</param>
        /// <exception cref="ArgumentNullException">key</exception>
        public virtual void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            key = key.Trim().ToLowerInvariant();

            string valueString = CommonHelper.GetAppCustomTypeConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();

            var cacheSetting = allSettings.ContainsKey(key) ? allSettings[key].FirstOrDefault() : null;

            if (cacheSetting != null)
            {
                var setting = GetSettingById(cacheSetting.Id);

                setting.Value = valueString;

                UpdateSetting(setting, clearCache);
            }
            else
            {
                var setting = new Setting()
                {
                    Key = key,
                    Value = valueString
                };

                InsertSetting(setting, clearCache);
            }
        }

        /// <summary>
        /// Save Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void SaveSetting<T>(T entity) where T : ISetting, new()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                if (!CommonHelper.GetAppCustomTypeConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + property.Name;

                dynamic value = property.GetValue(entity, null);

                if (value != null)
                    SetSetting(key, value, false);
                else
                    SetSetting(key, "", false);
            }

            ClearCache();
        }

        /// <summary>
        /// Save Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="clearCache">Clear Cache</param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void SaveSetting<T, TPropType>(T entity, Expression<Func<T, TPropType>> key, bool clearCache = true) where T : ISetting, new()
        {
            var memberExpression = key.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", key));
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", key));
            }

            string keyResult = entity.GetSettingKey(key);

            dynamic value = propertyInfo.GetValue(entity, null);

            if (value != null)
                SetSetting(keyResult, value, clearCache);
            else
                SetSetting(keyResult, "", clearCache);
        }

        /// <summary>
        /// Update Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">Clear Cache</param>
        /// <exception cref="ArgumentNullException">setting</exception>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            Setting realSetting = GetSettingById(setting.Id);

            if (realSetting != null)
            {
                realSetting.Value = setting.Value;
                realSetting.Description = setting.Description;
                _settingRepository.Update(realSetting);
            }
            else
                _settingRepository.Update(setting);

            ClearCache();

            /*if (clearCache)
            {
                ClearCache();
            }
            else
            {
                var settingCache=GetSettingCache(setting.Key);
                if (settingCache!=null)
                    settingCache.Value = setting.Value;
            }
            if (setting.Key!="TimeZone")
                _cacheManager.ClearRemoteServersCache();*/

            _appEventPublisher.EntityUpdated(setting);
        }

        /// <summary>
        /// Setting Exists
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>Bool Item</returns>
        public virtual bool SettingExists<T, TPropType>(T entity, Expression<Func<T, TPropType>> key) where T : ISetting, new()
        {
            string keyResult = entity.GetSettingKey(key);

            var setting = GetSettingByKey<string>(keyResult);

            return setting != null;
        }

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <exception cref="ArgumentNullException">setting</exception>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            _settingRepository.Delete(setting);

            ClearCache();
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(setting);
        }

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual void DeleteSetting<T>() where T : ISetting, new()
        {
            var settingList = new List<Setting>();

            var allSettings = GetAllSettings();

            foreach (var property in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + property.Name;

                settingList.AddRange(allSettings.Where(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            foreach (var setting in settingList)
                DeleteSetting(setting);
        }

        /// <summary>
        /// Delete Setting
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        public virtual void DeleteSetting<T, TPropType>(T entity, Expression<Func<T, TPropType>> key) where T : ISetting, new()
        {
            string keyResult = entity.GetSettingKey(key);

            keyResult = keyResult.Trim().ToLowerInvariant();

            var allSettingsCached = GetAllSettingsCached();

            var settingForCaching = allSettingsCached.ContainsKey(keyResult) ? allSettingsCached[keyResult].FirstOrDefault() : null;

            if (settingForCaching != null)
            {
                var setting = GetSettingById(settingForCaching.Id);

                DeleteSetting(setting);
            }
        }

        /// <summary>
        /// Clear Cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(CACHE_SETTING_PATTERN_KEY);
        }

        /// <summary>
        /// Execute custom query
        /// </summary>
        public virtual void ExecCustomQuery(string QueryString)
        {
            var sqlParam = _dataProvider.GetParameter();
            sqlParam.ParameterName = "affiliateChannels";
            sqlParam.Value = QueryString;
            sqlParam.DbType = DbType.String;

            _settingRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[ExecCustomQuery] @QueryString", sqlParam);
        }

        /// <summary>
        /// AddShortFormData
        /// </summary>
        public virtual void AddShortFormData(int amount, string firstname, string zip, string email, string url, string country, string IP)
        {
            var sqlParamAmount = _dataProvider.GetParameter();
            sqlParamAmount.ParameterName = "Amount";
            sqlParamAmount.Value = amount;
            sqlParamAmount.DbType = DbType.Int32;

            var sqlParamFirstname = _dataProvider.GetParameter();
            sqlParamFirstname.ParameterName = "FirstName";
            sqlParamFirstname.Value = firstname;
            sqlParamFirstname.DbType = DbType.String;

            var sqlParamZip = _dataProvider.GetParameter();
            sqlParamZip.ParameterName = "Zip";
            sqlParamZip.Value = zip;
            sqlParamZip.DbType = DbType.String;

            var sqlParamEmail = _dataProvider.GetParameter();
            sqlParamEmail.ParameterName = "Email";
            sqlParamEmail.Value = email;
            sqlParamEmail.DbType = DbType.String;

            var sqlParamUrl = _dataProvider.GetParameter();
            sqlParamUrl.ParameterName = "ReferringUrl";
            sqlParamUrl.Value = url;
            sqlParamUrl.DbType = DbType.String;

            var sqlParamCountry = _dataProvider.GetParameter();
            sqlParamCountry.ParameterName = "Country";
            sqlParamCountry.Value = url;
            sqlParamCountry.DbType = DbType.String;

            var sqlParamIP = _dataProvider.GetParameter();
            sqlParamIP.ParameterName = "IP";
            sqlParamIP.Value = IP;
            sqlParamIP.DbType = DbType.String;

            var sqlParamDateTime = _dataProvider.GetParameter();
            sqlParamDateTime.ParameterName = "DateTime";
            sqlParamDateTime.Value = DateTime.UtcNow;
            sqlParamDateTime.DbType = DbType.DateTime;

            var retVal = _settingRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[AddShortFormData] @FirstName, @Email, @Zip, @Amount, @DateTime, @ReferringUrl, @Country, @IP",
                sqlParamFirstname,
                sqlParamEmail,
                sqlParamZip,
                sqlParamAmount,
                sqlParamDateTime,
                sqlParamUrl,
                sqlParamCountry,
                sqlParamIP
                ).FirstOrDefault();
        }

        public virtual void RemoveEntity(Entity entity)
        {
            var passwordParam = _dataProvider.GetParameter();
            passwordParam.ParameterName = "password";
            passwordParam.Value = entity.Passport;
            passwordParam.DbType = DbType.String;

            var entityTypeParam = _dataProvider.GetParameter();
            entityTypeParam.ParameterName = "entityType";
            entityTypeParam.Value = Convert.ToInt32(entity.EntityType);
            entityTypeParam.DbType = DbType.Int32;

            var entityIdsParam = _dataProvider.GetParameter();
            entityIdsParam.ParameterName = "entityIds";
            entityIdsParam.Value = entity.EntityIds;
            entityIdsParam.DbType = DbType.String;

            var retVal  = _settingRepository.GetDbClientContext().SqlQuery<object>("EXECUTE [dbo].[CleanUpDBWithEntities] @password, @entityType, @entityIds", passwordParam, entityTypeParam, entityIdsParam).ToList();
        }

        #endregion Methods
    }
}