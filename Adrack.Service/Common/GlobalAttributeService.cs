// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="GlobalAttributeService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Global Attribute Service
    /// Implements the <see cref="Adrack.Service.Common.IGlobalAttributeService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Common.IGlobalAttributeService" />
    public partial class GlobalAttributeService : IGlobalAttributeService
    {
        #region Constants

        /// <summary>
        /// Cache Global Attribute By Id Key
        /// </summary>
        private const string CACHE_GLOBALATTRIBUTE_BY_ID_KEY = "App.Cache.GlobalAttribute.By.Id-{0}";

        /// <summary>
        /// Cache Global Attribute By Entity Id, Key Group Key
        /// </summary>
        private const string CACHE_GLOBALATTRIBUTE_BY_ENTITY_ID_KEY_GROUP_KEY = "App.Cache.GlobalAttribute.By.Entity.Id-{0}.Key.Group-{1}";

        /// <summary>
        /// Cache Global Attribute All Key
        /// </summary>
        private const string CACHE_GLOBALATTRIBUTE_ALL_KEY = "App.Cache.GlobalAttribute.All";

        /// <summary>
        /// Cache Global Attribute Pattern Key
        /// </summary>
        private const string CACHE_GLOBALATTRIBUTE_PATTERN_KEY = "App.Cache.GlobalAttribute.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Global Attribute
        /// </summary>
        private readonly IRepository<GlobalAttribute> _globalAttributeRepository;

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
        /// Global Attribute Service
        /// </summary>
        /// <param name="globalAttributeRepository">Global Attribute Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public GlobalAttributeService(IRepository<GlobalAttribute> globalAttributeRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._globalAttributeRepository = globalAttributeRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Global Attribute By Id
        /// </summary>
        /// <param name="globalAttributeId">Global Attribute Identifier</param>
        /// <returns>Global Attribute Item</returns>
        public virtual GlobalAttribute GetGlobalAttributeById(long globalAttributeId)
        {
            if (globalAttributeId == 0)
                return null;

            string key = string.Format(CACHE_GLOBALATTRIBUTE_BY_ID_KEY, globalAttributeId);

            return _cacheManager.Get(key, () => { return _globalAttributeRepository.GetById(globalAttributeId); });
        }

        /// <summary>
        /// Get Global Attribute By Entity Id And Key Group
        /// </summary>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="keyGroup">Key Group</param>
        /// <returns>Global Attribute Collection Item</returns>
        public virtual IList<GlobalAttribute> GetGlobalAttributeByEntityIdAndKeyGroup(long entityId, string keyGroup)
        {
            string key = string.Format(CACHE_GLOBALATTRIBUTE_BY_ENTITY_ID_KEY_GROUP_KEY, entityId, keyGroup);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _globalAttributeRepository.Table
                            where x.EntityId == entityId && x.KeyGroup == keyGroup
                            select x;

                var globalAttributes = query.ToList();

                return globalAttributes;
            });
        }




        /// <summary>
        /// Get Global Attribute By Key And Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Global Attribute Collection Item</returns>
        public virtual GlobalAttribute GetGlobalAttributeByKeyAndValue(string pKey, string pValue)
        {
            var query = from x in _globalAttributeRepository.Table
                where x.Key == pKey && x.Value == pValue
                        select x;

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Save Global Attribute Only Key And Value
        /// </summary>
        /// <typeparam name="pKey">Key</typeparam>
        /// <param name="pValue">Value</param>
        /// <param name="pExtraData">ExtraData</param>
        /// <exception cref="ArgumentNullException">key or value</exception>
        public virtual void SaveGlobalAttributeOnlyKeyAndValue(string pKey, string pValue, string pExtraData = "")
        {
            if (string.IsNullOrWhiteSpace(pKey))
                throw new ArgumentNullException("key");

            if (string.IsNullOrWhiteSpace(pValue))
                throw new ArgumentNullException("value");

            GlobalAttribute globalAttribute = new GlobalAttribute()
            {
                EntityId = 0,
                KeyGroup = "User",
                Key = pKey,
                Value = pValue,
                ExtraData = pExtraData
            };

            InsertGlobalAttribute(globalAttribute);
        }

        /// <summary>
        /// Delete Global Attribute By Key And Value
        /// </summary>
        /// <typeparam name="pKey">Key</typeparam>
        /// <param name="pValue">Value</param>
        /// <exception cref="ArgumentNullException">key or value</exception>
        public virtual void DeleteGlobalAttributeByKeyAndValue(string pKey, string pValue)
        {
            if (string.IsNullOrWhiteSpace(pKey))
                throw new ArgumentNullException("key");

            if (string.IsNullOrWhiteSpace(pValue))
                throw new ArgumentNullException("value");

            var attribute = GetGlobalAttributeByKeyAndValue(pKey, pValue);
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            DeleteGlobalAttribute(attribute);
        }





        /// <summary>
        /// Get All Global Attributes
        /// </summary>
        /// <returns>Global Attribute Collection Item</returns>
        public virtual IList<GlobalAttribute> GetAllGlobalAttributes()
        {
            string key = CACHE_GLOBALATTRIBUTE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _globalAttributeRepository.Table
                            orderby x.Id
                            select x;

                var globalAttributes = query.ToList();

                return globalAttributes;
            });
        }

        /// <summary>
        /// Insert Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        /// <exception cref="ArgumentNullException">globalAttribute</exception>
        public virtual void InsertGlobalAttribute(GlobalAttribute globalAttribute)
        {
            if (globalAttribute == null)
                throw new ArgumentNullException("globalAttribute");

            _globalAttributeRepository.Insert(globalAttribute);

            _cacheManager.RemoveByPattern(CACHE_GLOBALATTRIBUTE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(globalAttribute);
        }

        /// <summary>
        /// Save Global Attribute
        /// </summary>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="baseEntity">Base Entity</param>
        /// <param name="key">Key</param>
        /// <param name="propTypeValue">Property Type Value</param>
        /// <exception cref="ArgumentNullException">baseEntity
        /// or
        /// key</exception>
        public virtual void SaveGlobalAttribute<TPropType>(BaseEntity baseEntity, string key, TPropType propTypeValue)
        {
            if (baseEntity == null)
                throw new ArgumentNullException("baseEntity");

            if (key == null)
                throw new ArgumentNullException("key");

            string keyGroup = baseEntity.GetUnproxiedEntityType().Name;

            var globalAttributeList = GetGlobalAttributeByEntityIdAndKeyGroup(baseEntity.Id, keyGroup);

            var globalAttribute = globalAttributeList.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            string valueStr = CommonHelper.To<string>(propTypeValue);

            if (globalAttribute != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    var attribute = GetGlobalAttributeById(globalAttribute.Id);

                    DeleteGlobalAttribute(attribute);
                }
                else
                {
                    var attribute = GetGlobalAttributeById(globalAttribute.Id);

                    attribute.Value = valueStr;
                    UpdateGlobalAttribute(attribute);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    globalAttribute = new GlobalAttribute()
                    {
                        EntityId = baseEntity.Id,
                        KeyGroup = keyGroup,
                        Key = key,
                        Value = valueStr,
                    };

                    InsertGlobalAttribute(globalAttribute);
                }
            }
        }

        /// <summary>
        /// Update Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        /// <exception cref="ArgumentNullException">globalAttribute</exception>
        public virtual void UpdateGlobalAttribute(GlobalAttribute globalAttribute)
        {
            if (globalAttribute == null)
                throw new ArgumentNullException("globalAttribute");

            _globalAttributeRepository.Update(globalAttribute);

            _cacheManager.RemoveByPattern(CACHE_GLOBALATTRIBUTE_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(globalAttribute);
        }

        /// <summary>
        /// Delete Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        /// <exception cref="ArgumentNullException">globalAttribute</exception>
        public virtual void DeleteGlobalAttribute(GlobalAttribute globalAttribute)
        {
            if (globalAttribute == null)
                throw new ArgumentNullException("globalAttribute");

            _globalAttributeRepository.Delete(globalAttribute);

            _cacheManager.RemoveByPattern(CACHE_GLOBALATTRIBUTE_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(globalAttribute);
        }

        #endregion Methods
    }
}