// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BlackListService.cs" company="Adrack.com">
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
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class BlackListService.
    /// Implements the <see cref="Adrack.Service.Lead.IBlackListService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBlackListService" />
    public partial class BlackListService : IBlackListService
    {
        #region Constants

        /// <summary>
        /// Cache Blacklist By Id Key
        /// </summary>
        private const string CACHE_BLACKLIST_BY_ID_KEY = "App.Cache.BlackList.By.Id-{0}";

        /// <summary>
        /// Cache Blacklist All Key
        /// </summary>
        private const string CACHE_BLACKLIST_ALL_KEY = "App.Cache.BlackList.All";

        /// <summary>
        /// Cache Blacklist All Count
        /// </summary>
        private const string CACHE_BLACKLIST_COUNT = "App.Cache.BlackList.Count-{0}-{1}";


        /// <summary>
        /// Cache Blacklist Values
        /// </summary>
        private const string CACHE_BLACKLIST_VALUES = "App.Cache.BlackList.Values-{0}-{1}-{2}";

        /// <summary>
        /// Cache Blacklist Values
        /// </summary>
        private const string CACHE_BLACKLIST_VALUES_PAIR = "App.Cache.BlackList.ValuesPair-{0}-{1}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BLACKLIST_PATTERN_KEY = "App.Cache.BlackList.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BlackListType> _blackListTypeRepository;

        /// <summary>
        /// The black list value repository
        /// </summary>
        private readonly IRepository<BlackListValue> _blackListValueRepository;

        /// <summary>
        /// The custom black list value repository
        /// </summary>
        private readonly IRepository<CustomBlackListValue> _customBlackListValueRepository;

        private readonly IRepository<CampaignField> _campaignTemplateRepository;

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

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="blackListTypeRepository">The black list type repository.</param>
        /// <param name="blackListValueRepository">The black list value repository.</param>
        /// <param name="customBlackListValueRepository">The custom black list value repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public BlackListService(IRepository<BlackListType> blackListTypeRepository, 
                                IRepository<BlackListValue> blackListValueRepository, 
                                IRepository<CustomBlackListValue> customBlackListValueRepository,
                                IRepository<CampaignField> campaignTemplateRepository,
                                ICacheManager cacheManager, 
                                IAppEventPublisher appEventPublisher, 
                                IDataProvider dataProvider)
        {
            this._blackListTypeRepository = blackListTypeRepository;
            this._blackListValueRepository = blackListValueRepository;
            this._customBlackListValueRepository = customBlackListValueRepository;
            this._campaignTemplateRepository = campaignTemplateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual BlackListType GetBlackListTypeById(long Id)
        {
            if (Id == 0)
                return null;

            return _blackListTypeRepository.GetById(Id);
        }

        /// <summary>
        /// Gets the type of the black list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>BlackListType.</returns>
        public virtual BlackListType GetBlackListType(string name)
        {
                var query = from x in _blackListTypeRepository.Table
                            where x.Name.ToLower() == name
                            select x;

                return query.FirstOrDefault();         
        }

        /// <summary>
        /// Gets the black list value by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BlackListValue.</returns>
        public virtual BlackListValue GetBlackListValueById(long Id)
        {
            if (Id == 0)
                return null;

            return _blackListValueRepository.GetById(Id);
        }

        /// <summary>
        /// Gets the custom black list value by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>CustomBlackListValue.</returns>
        public virtual CustomBlackListValue GetCustomBlackListValueById(long Id)
        {
            if (Id == 0)
                return null;

            return _customBlackListValueRepository.GetById(Id);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BlackListType> GetAllBlackListTypes()
        {
            string key = CACHE_BLACKLIST_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _blackListTypeRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all black list types by parent identifier.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;BlackListType&gt;.</returns>
        public virtual IList<BlackListType> GetAllBlackListTypesByParentId(short type, long parentid)
        {
                var query = from x in _blackListTypeRepository.Table
                            where x.BlackType == type && x.ParentId == parentid
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
        }

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        public virtual IList<BlackListValue> GetAllBlackListValues(long Id)
        {
            return (from x in _blackListValueRepository.Table
                    where x.BlackListTypeId == Id
                    select x).ToList();
        }

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        public virtual IList<BlackListValue> GetAllBlackListValues(string value)
        {
            return (from x in _blackListValueRepository.Table
                    where x.Value.Contains(value)
                    select x).ToList();
        }

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="blackListTypeId">The black list type identifier.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        public virtual IList<BlackListValue> GetAllBlackListValues(string value, long blackListTypeId)
        {
            return (from x in _blackListValueRepository.Table
                    where x.Value.Contains(value) && x.BlackListTypeId == blackListTypeId
                    select x).ToList();
        }

        /// <summary>
        /// Checks the black list value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>BlackListValue.</returns>
        public virtual bool CheckBlackListValue(string name, string value)
        {
            string key = string.Format(CACHE_BLACKLIST_VALUES_PAIR, name, value);
            return _cacheManager.Get(key, () =>
            {
                var nameParam = _dataProvider.GetParameter();
                nameParam.ParameterName = "name";
                nameParam.Value = name;
                nameParam.DbType = DbType.String;

                var valueParam = _dataProvider.GetParameter();
                valueParam.ParameterName = "value";
                valueParam.Value = value;
                valueParam.DbType = DbType.String;

                var result = _blackListValueRepository.GetDbClientContext().SqlQuery<BlackListValue>("EXECUTE [dbo].[CheckBlackListValue] @name, @value", nameParam, valueParam).FirstOrDefault();
                return result != null;

            });
            
            
        }

        /// <summary>
        /// Gets the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        /// <returns>IList&lt;CustomBlackListValue&gt;.</returns>
        public virtual IList<CustomBlackListValue> GetCustomBlackListValues(long channelId, short channelType)
        {
            return (from x in _customBlackListValueRepository.Table
                    where x.ChannelId == channelId && x.ChannelType == channelType
                    select x).ToList();
        }

        

        /// <summary>
        /// Gets the custom black list values count.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>        
        /// <returns>Number of values</returns>
        /// 
        public virtual int GetCustomBlackListItemsCount(long channelId, short channelType)
        {
            string key = string.Format(CACHE_BLACKLIST_COUNT, channelId, channelType);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _customBlackListValueRepository.Table
                    where x.ChannelId == channelId && x.ChannelType == channelType
                    select x).Count();
            });


        }

        /// <summary>
        /// Gets the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        /// <param name="field">The field.</param>
        /// <param name="fieldId"></param>
        /// <returns>IList&lt;CustomBlackListValue&gt;.</returns>
        public virtual IList<CustomBlackListValue> GetCustomBlackListValues(long channelId, short channelType, long fieldId)
        {
            string key = string.Format(CACHE_BLACKLIST_VALUES, channelId, channelType, fieldId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _customBlackListValueRepository.Table
                    where x.ChannelId == channelId && x.ChannelType == channelType && x.TemplateFieldId == fieldId
                    select x).ToList();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">blacklisttype</exception>
        public virtual long InsertBlackListType(BlackListType blacklisttype)
        {
            if (blacklisttype == null)
                throw new ArgumentNullException("blacklisttype");

            _blackListTypeRepository.Insert(blacklisttype);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(blacklisttype);

            return blacklisttype.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        /// <exception cref="ArgumentNullException">blacklisttype</exception>
        public virtual void UpdateBlackListType(BlackListType blacklisttype)
        {
            if (blacklisttype == null)
                throw new ArgumentNullException("blacklisttype");

            _blackListTypeRepository.Update(blacklisttype);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(blacklisttype);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        /// <exception cref="ArgumentNullException">blacklisttype</exception>
        public virtual void DeleteBlackListType(BlackListType blacklisttype)
        {
            if (blacklisttype == null)
                throw new ArgumentNullException("blacklisttype");

            _blackListTypeRepository.Delete(blacklisttype);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(blacklisttype);
        }

        /// <summary>
        /// Deletes the black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual void DeleteBlackListValue(BlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _blackListValueRepository.Delete(blacklistvalue);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(blacklistvalue);
        }

        /// <summary>
        /// Deletes the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual void DeleteCustomBlackListValue(CustomBlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _customBlackListValueRepository.Delete(blacklistvalue);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(blacklistvalue);
        }

        /// <summary>
        /// Deletes the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        public virtual void DeleteCustomBlackListValues(long channelId, short channelType)
        {
            List<CustomBlackListValue> values = (List<CustomBlackListValue>)this.GetCustomBlackListValues(channelId, channelType);
            foreach (CustomBlackListValue v in values)
            {
                this.DeleteCustomBlackListValue(v);
            }
        }

        /// <summary>
        /// Inserts the black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual long InsertBlackListValue(BlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _blackListValueRepository.Insert(blacklistvalue);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(blacklistvalue);

            return blacklistvalue.Id;
        }

        /// <summary>
        /// Inserts the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual void InsertCustomBlackListValue(CustomBlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _customBlackListValueRepository.Insert(blacklistvalue);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(blacklistvalue);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual void UpdateBlackListValue(BlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _blackListValueRepository.Update(blacklistvalue);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(blacklistvalue);
        }

        /// <summary>
        /// Updates the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <exception cref="ArgumentNullException">blacklistvalue</exception>
        public virtual void UpdateCustomBlackListValue(CustomBlackListValue blacklistvalue)
        {
            if (blacklistvalue == null)
                throw new ArgumentNullException("blacklistvalue");

            _customBlackListValueRepository.Update(blacklistvalue);

            _cacheManager.RemoveByPattern(CACHE_BLACKLIST_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(blacklistvalue);
        }


        public bool CheckCustomBlackListValues(long channelId, short channelType, long fieldId, string value)
        {
            var blackList = GetCustomBlackListValues(channelId, channelType, fieldId).FirstOrDefault();
            var template = _campaignTemplateRepository.GetById(fieldId);
            if (blackList != null && !string.IsNullOrEmpty(blackList.Value))
            {
                var valueStrings = blackList.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in valueStrings)
                {
                    if ((template.TemplateField == "Email" && 
                         s.StartsWith("*@") && s.Trim().Equals(value.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries)[1])) || 
                        s.Trim() == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion Methods
    }
}