// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Helpers;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml;
using Adrack.PlanManagement;
using System.Reflection;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class BuyerChannelService.
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerChannelService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerChannelService" />
    public partial class BuyerChannelService : IBuyerChannelService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_BY_ID_KEY = "App.Cache.BC.BuyerChannel.By.Id-{0}";

        private const string CACHE_BUYER_CHANNEL_BY_ID_XML = "App.Cache.BC.BuyerChannel.By.Id.XML-{0}";

        /// <summary>
        /// The cache buyer channel template by campaign identifier key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_BY_CAMPAIGN_ID_KEY = "App.Cache.BC.BuyerChannel.By.CampaignId-{0}-{1}-{2}-{3}";

        /// <summary>
        /// The cache buyer channel check allowed affiliate channel
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_CheckAllowedAffiliateChannel = "App.Cache.BC.BuyerChannel.CheckAllowedAffiliateChannel-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_GetAllBuyerChannels = "App.Cache.BC.BuyerChannel.GetAllBuyerChannels-{0}";

        private const string CACHE_BUYER_CHANNEL_GetAllBuyerChannels2 = "App.Cache.BC.BuyerChannel.GetAllBuyerChannels2-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_GetAllBuyerChannelsByOrder = "App.Cache.BC.BuyerChannel.GetAllBuyerChannelsByOrder-{0}";

        private const string CACHE_BUYER_CHANNEL_GetBuyerChannelByName = "App.Cache.BC.BuyerChannel.GetBuyerChannelByName-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_GetBuyerChannelByContainingName = "App.Cache.BC.BuyerChannel.GetBuyerChannelByContainingName-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_GetBuyerChannelByUniqueMappingId = "App.Cache.BC.BuyerChannel.GetBuyerChannelByUniqueMappingId-{0}";

        private const string CACHE_BUYER_CHANNEL_GetBuyerChannelByBuyerIdAndUniqueMappingId = "App.Cache.BC.BuyerChannel.GetBuyerChannelByBuyerIdAndUniqueMappingId-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidays = "App.Cache.BC.BuyerChannel.GetBuyerChannelHolidays-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelScheduleDay = "App.Cache.BC.BuyerChannel.GetBuyerChannelScheduleDay-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelScheduleTimePeriod = "App.Cache.BC.BuyerChannel.GetBuyerChannelScheduleTimePeriod-{0}";

        private const string CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidays2 = "App.Cache.BC.BuyerChannel.GetBuyerChannelHolidays2-{0}-{1}";

        private const string CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidayById = "App.Cache.BC.BuyerChannel.GetBuyerChannelHolidayById-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_ALL_KEY = "App.Cache.BC.BuyerChannel.All";

        /// <summary>
        /// The cache buyer channel by campaign template all key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_BY_CAMPAIGN_ALL_KEY = "App.Cache.BC.BuyerChannel.ByCampaign.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_CHANNEL_PATTERN_KEY = "App.Cache.BC.BuyerChannel.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<BuyerChannel> _buyerChannelRepository;

        private readonly IRepository<BuyerChannelNote> _buyerChannelNoteRepository;

        private readonly IRepository<BuyerChannelScheduleDay> _buyerChannelScheduleDayRepository;
        
        private readonly IRepository<BuyerChannelScheduleTimePeriod> _buyerChannelScheduleTimePeriodRepository;

        private readonly IRepository<BuyerChannelHoliday> _buyerChannelHolidayRepository;

        private readonly IRepository<AttachedChannel> _attachedChannelRepository;

        private readonly IRepository<PingTreeItem> _pingTreeItemRepository;



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

        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="buyerChannelRepository">The buyer channel repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public BuyerChannelService(IRepository<BuyerChannel> buyerChannelRepository, 
            ICacheManager cacheManager, 
            IDataProvider dataProvider, 
            IAppEventPublisher appEventPublisher,
            IRepository<BuyerChannelNote> _buyerChannelNoteRepository,
            IRepository<BuyerChannelScheduleDay> buyerChannelScheduleDayRepository,
            IRepository<BuyerChannelScheduleTimePeriod> buyerChannelScheduleTimePeriodRepository,
            IRepository<BuyerChannelHoliday> buyerChannelHolidayRepository,
            IRepository<AttachedChannel> attachedChannelRepository,
            IRepository<PingTreeItem> pingTreeItemRepository,
            IAppContext appContext)
        {
            this._buyerChannelRepository = buyerChannelRepository;
            this._cacheManager = cacheManager;
            this._dataProvider = dataProvider;
            this._appEventPublisher = appEventPublisher;
            this._buyerChannelNoteRepository = _buyerChannelNoteRepository;
            this._buyerChannelScheduleDayRepository = buyerChannelScheduleDayRepository;
            this._buyerChannelScheduleTimePeriodRepository = buyerChannelScheduleTimePeriodRepository;
            this._attachedChannelRepository = attachedChannelRepository;
            this._appContext = appContext;
            this._buyerChannelHolidayRepository = buyerChannelHolidayRepository;
            this._pingTreeItemRepository = pingTreeItemRepository;
        }

        #endregion Constructor

        #region Methods


        IQueryable<BuyerChannel> FilterAllowed(IQueryable<BuyerChannel> query, User user = null)
        {
            //return query;

            if (user == null)
                user = _appContext.AppUser;

            var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();
            var userService = AppEngineContext.Current.Resolve<IUserService>();

            List<long> allowedBuyerChannelIds = (List<long>)userService.GetUserEntityIds(user.Id, "buyerchannel", Guid.Empty);

            if (user.UserType == SharedData.BuyerUserTypeId)
            {
                query = query.Where(x => allowedBuyerChannelIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.NetowrkUserTypeId)
            {
                //var buyerIds = buyerService.GetAllBuyers().Select(x => x.Id);
                //query = query.Where(x => buyerIds.Contains(x.BuyerId) && allowedBuyerChannelIds.Contains(x.Id));
                query = query.Where(x => x.ManagerId == user.Id || allowedBuyerChannelIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                query = query.Where(x => x.Id == -1);
            }

            return query;
        }

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual BuyerChannel GetBuyerChannelById(long Id, bool cached = false)
        {

            if (Id == 0)
                return null;

            if (!cached)
                return _buyerChannelRepository.GetById(Id);

            string key = string.Format(CACHE_BUYER_CHANNEL_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _buyerChannelRepository.GetById(Id); });
        }

        public virtual XmlDocument GetBuyerChannelXML(BuyerChannel channel)
        {
            
            string key = string.Format(CACHE_BUYER_CHANNEL_BY_ID_XML, channel.Id);

            return _cacheManager.Get(key, () => 
            {
                var xml = new XmlDocument(); xml.LoadXml(channel.XmlTemplate);
                return xml;
            });
        }

        /// <summary>
        /// Gets buyer channel by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>BuyerChannel.</returns>
        public virtual BuyerChannel GetBuyerChannelByName(string name, long exceptId, bool fromCache = true)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_GetBuyerChannelByName, name, exceptId);

            if (!fromCache)
            {
                return (from x in _buyerChannelRepository.Table
                        where x.Name == name && x.Id != exceptId && (!x.Deleted.HasValue || !x.Deleted.Value)
                        select x).FirstOrDefault();
            }

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelRepository.Table
                        where x.Name == name && x.Id != exceptId
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets buyer channel by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>BuyerChannel.</returns>
        public virtual List<BuyerChannel> GetBuyerChannelByContainingName(string name, long exceptId)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_GetBuyerChannelByContainingName, name, exceptId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelRepository.Table
                        where x.Name.ToLower().Contains(name.ToLower()) && x.Id != exceptId
                        select x).ToList();
            });
        }

        public BuyerChannel GetBuyerChannelByUniqueMappingId(string uniqueId)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_GetBuyerChannelByUniqueMappingId, uniqueId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelRepository.Table
                        where x.ChannelMappingUniqueId == uniqueId
                        select x).FirstOrDefault();
            });
        }

        public BuyerChannel GetBuyerChannelByBuyerIdAndUniqueMappingId(long buyerId, string uniqueId)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_GetBuyerChannelByBuyerIdAndUniqueMappingId, buyerId, uniqueId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelRepository.Table
                        where x.BuyerId == buyerId && x.ChannelMappingUniqueId == uniqueId
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerChannel> GetAllBuyerChannels(short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_BUYER_CHANNEL_GetAllBuyerChannels, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<BuyerChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Get buyer channels
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerChannel> GetBuyerChannels(long[] ids,  short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _buyerChannelRepository.Table
                            where ids.Contains(x.Id) &&
                            (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<BuyerChannel>)FilterAllowed(query);

                return query.ToList();
        }

        public virtual IList<long> GetAllBuyerChannelIds(short deleted = 0)
        {
            var query = from x in _buyerChannelRepository.Table
                        where 
                        (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                        orderby x.Id descending
                        select x.Id;


            return query.ToList();
        }

        /// <summary>
        /// Gets all buyer channels.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        public virtual IList<BuyerChannel> GetAllBuyerChannels(long buyerId, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_BUYER_CHANNEL_GetAllBuyerChannels2, buyerId, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelRepository.Table
                            where x.BuyerId == buyerId && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<BuyerChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all buyer channels by order.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        public virtual IList<BuyerChannel> GetAllBuyerChannelsByOrder(short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_BUYER_CHANNEL_GetAllBuyerChannelsByOrder, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.OrderNum
                            select x;

                query = (IOrderedQueryable<BuyerChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all buyer channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        public virtual IList<BuyerChannel> GetAllBuyerChannelsByCampaignId(long campaignId, short deleted = 0)
        {
            //string key = CACHE_BUYER_CHANNEL_BY_CAMPAIGN_TEMPLATE_ALL_KEY;

            //return _cacheManager.Get(key, () =>
            // {
            var query = from x in _buyerChannelRepository.Table
                        where x.CampaignId == campaignId && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                        orderby x.OrderNum
                        select x;

            query = (IOrderedQueryable<BuyerChannel>)FilterAllowed(query);

            return query.ToList();
            //});
        }

        /// <summary>
        /// Gets all buyer channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="state">The state.</param>
        /// <param name="age">The age.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        public virtual BuyerChannel[] GetAllBuyerChannelsByCampaignId(long campaignId, string zipCode, string state, short age, long? pingTreeId = null)
        {
            string key = string.Format(CACHE_BUYER_CHANNEL_BY_CAMPAIGN_ID_KEY, campaignId, zipCode, state, age);
            const BuyerChannelStatuses ACTIVE_STATUS = BuyerChannelStatuses.Active;
            const BuyerChannelStatuses PAUSED_STATUS = BuyerChannelStatuses.Paused;
            return _cacheManager.Get(key, () =>
            {
                List<long> ids = null;
                if (pingTreeId.HasValue)
                {
                    ids = (from x in _pingTreeItemRepository.Table
                     where x.PingTreeId == pingTreeId.Value
                     select x.BuyerChannelId).ToList();
                }

                var query = from x in _buyerChannelRepository.Table
                            where (x.Status == ACTIVE_STATUS || x.Status == PAUSED_STATUS) && 
                            ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue) && 
                            x.CampaignId == campaignId &&
                            (ids == null || (ids != null && ids.Contains(x.Id)))
                            /*&&
                            ((!x.EnableZipCodeTargeting || (x.EnableZipCodeTargeting &&

                                (x.ZipCodeCondition == 1 && x.ZipCodeTargeting.Contains(zipCode)) ||
                                (x.ZipCodeCondition == 2 && !x.ZipCodeTargeting.Contains(zipCode)) ||
                                (x.ZipCodeCondition == 3 && x.ZipCodeTargeting.StartsWith(zipCode)) ||
                                (x.ZipCodeCondition == 4 && x.ZipCodeTargeting.EndsWith(zipCode)) ||
                                (x.ZipCodeCondition == 5 && x.ZipCodeTargeting == zipCode) ||
                                (x.ZipCodeCondition == 6 && x.ZipCodeTargeting != zipCode)
                            )) &&
                            ((!x.EnableStateTargeting || (x.EnableStateTargeting &&

                                (x.StateCondition == 1 && x.StateTargeting.Contains(state)) ||
                                (x.StateCondition == 2 && !x.StateTargeting.Contains(state)) ||
                                (x.StateCondition == 3 && x.StateTargeting.StartsWith(state)) ||
                                (x.StateCondition == 4 && x.StateTargeting.EndsWith(state)) ||
                                (x.StateCondition == 5 && x.StateTargeting == state) ||
                                (x.StateCondition == 6 && x.StateTargeting != state)
                            ))) &&
                            ((!x.EnableAgeTargeting || (x.EnableAgeTargeting && age >= x.MinAgeTargeting && age <= x.MaxAgeTargeting)))
                            )*/
                            orderby x.OrderNum
                            select x;


                var profiles = query.Where(x => !x.Deleted.HasValue || x.Deleted.HasValue && !x.Deleted.Value).ToArray<BuyerChannel>();

                return profiles;
            });
        }

        /// <summary>
        /// Checks the allowed affiliate channel.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool CheckAllowedAffiliateChannel(long affiliateChannelId, long buyerChannelId)
        {

            string key = string.Format(CACHE_BUYER_CHANNEL_CheckAllowedAffiliateChannel, affiliateChannelId, buyerChannelId);

            return _cacheManager.Get(key, () =>
            {
                if ((from x in _attachedChannelRepository.Table
                     where x.BuyerChannelId == buyerChannelId && x.AffiliateChannelId == affiliateChannelId
                     select x).ToList().Count > 0) return true;

                return false;
            });
        }

        /// <summary>
        /// Clones the specified buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int64.</returns>
        public virtual long Clone(long buyerChannelId, string name)
        {
            var buyerChannelIdParam = _dataProvider.GetParameter();
            buyerChannelIdParam.ParameterName = "BuyerChannelId";
            buyerChannelIdParam.Value = buyerChannelId;
            buyerChannelIdParam.DbType = DbType.Int64;

            var nameParam = _dataProvider.GetParameter();
            nameParam.ParameterName = "name";
            nameParam.Value = name;
            nameParam.DbType = DbType.String;

            return _buyerChannelRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[CloneBuyerChannel] @buyerChannelId, @name", buyerChannelIdParam, nameParam).FirstOrDefault();
        }

        /// <summary>
        /// Updates the allowed.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="add">if set to <c>true</c> [add].</param>
        public virtual void UpdateAllowed(long affiliateChannelId, long buyerChannelId, bool add)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            BuyerChannel bc = (BuyerChannel)this.GetBuyerChannelById(buyerChannelId);

            if (!add)
                bc.AllowedAffiliateChannels = bc.AllowedAffiliateChannels.Replace(":" + affiliateChannelId.ToString() + ";", "").Replace(":" + affiliateChannelId.ToString(), "");
            else
                bc.AllowedAffiliateChannels += ":" + affiliateChannelId.ToString() + ";";

            UpdateBuyerChannel(bc);
        }

        /// <summary>
        /// Gets all buyer channels by buyer identifier.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        public virtual IList<BuyerChannel> GetAllBuyerChannelsByBuyerId(long buyerId, short deleted = 0)
        {
            var query = from x in _buyerChannelRepository.Table
                        where x.BuyerId == buyerId && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                        orderby x.OrderNum
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

        /// <summary>
        /// Gets the allowed affiliate channels.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        public virtual IList<AffiliateChannel> GetAttachedAffiliateChannels(long Id)
        {
            BuyerChannel bc = this.GetBuyerChannelById(Id);

            var affiliateChannelIds = (from x in _attachedChannelRepository.Table
                        where x.BuyerChannelId == Id
                        select x.AffiliateChannelId).ToList();


            //string[] achannels = bc.AllowedAffiliateChannels.Split(new char[1] { ';' });

            var affiliateChannelsService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();

            List<AffiliateChannel> list = new List<AffiliateChannel>();

            foreach (var affiliateChannelId in affiliateChannelIds)
            {
                list.Add(affiliateChannelsService.GetAffiliateChannelById(affiliateChannelId));
            }

            return list;
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerChannel</exception>
        public virtual long InsertBuyerChannel(BuyerChannel buyerChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannel == null)
                throw new ArgumentNullException("buyerChannel");

            _buyerChannelRepository.SetCanTrackChanges(false);

            _buyerChannelRepository.Insert(buyerChannel);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannel);

            return buyerChannel.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <exception cref="ArgumentNullException">buyerChannel</exception>
        public virtual void UpdateBuyerChannel(BuyerChannel buyerChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannel == null)
                throw new ArgumentNullException("buyerChannel");

            _buyerChannelRepository.SetCanTrackChanges(true);
            _buyerChannelRepository.Update(buyerChannel);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyerChannel);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <exception cref="ArgumentNullException">buyerChannel</exception>
        public virtual void DeleteBuyerChannel(BuyerChannel buyerChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);

            _buyerChannelRepository.SetCanTrackChanges(true);

            if (buyerChannel == null)
                throw new ArgumentNullException("buyerChannel");

            _buyerChannelRepository.Delete(buyerChannel);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(buyerChannel);
        }

        /// <summary>
        /// Check Sum buyer channel's LeadAcceptRate By GroupNum.
        /// </summary>
        /// <param name="groupNum">The buyer channel's groupNum</param>
        /// <param name="buyerChannelId">The buyer channel Id</param>
        /// <param name="leadAcceptRate">The buyer channel's leadAcceptRate</param>
        public virtual bool CheckSumLeadAcceptRateByGroupNum(int groupNum, long buyerChannelId, float leadAcceptRate = 0)
        {
            if (leadAcceptRate > 100)
                return false;

            var sumLeadAcceptRate = (from x in _buyerChannelRepository.Table
                where x.Id != buyerChannelId && x.GroupNum == groupNum && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)
                orderby x.OrderNum
                select x.LeadAcceptRate).Sum();

            if(sumLeadAcceptRate != null && (sumLeadAcceptRate + leadAcceptRate) > 100)
                return false;
            else
                return true;
        }


        public IList<BuyerChannel> GetAllBuyerChannelsByMultipleBuyerId(List<long> buyerId, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _buyerChannelRepository.Table
                        where buyerId.Contains(x.BuyerId) && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                        orderby x.OrderNum
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

        public List<BuyerChannelNote> GetAllBuyerChannelNotesByBuyerChannelId(long buyerChannelId)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _buyerChannelNoteRepository.Table
                        where x.BuyerChannelId == buyerChannelId
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

       
        public virtual void DeleteBuyerChannelNote(BuyerChannelNote buyerChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelNote == null)
                throw new ArgumentNullException("buyerChanneleNote");

            _buyerChannelNoteRepository.Delete(buyerChannelNote);

            _appEventPublisher.EntityDeleted(buyerChannelNote);
        }
        
        public virtual long InsertBuyerChannelNote(BuyerChannelNote buyerChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelNote == null)
                throw new ArgumentNullException("buyerChanneleNote");

            _buyerChannelNoteRepository.Insert(buyerChannelNote);

            _appEventPublisher.EntityInserted(buyerChannelNote);
            _cacheManager.ClearRemoteServersCache();
            return buyerChannelNote.Id;
        }
        
        public virtual void UpdateBuyerChannelNote(BuyerChannelNote buyerChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelNote == null)
                throw new ArgumentNullException("buyerChannelNote");

            _buyerChannelNoteRepository.Update(buyerChannelNote);

            _appEventPublisher.EntityUpdated(buyerChannelNote);
        }
        
        public virtual BuyerChannelNote GetBuyerChannelNoteById(long Id)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (Id == 0)
                return null;

            return _buyerChannelNoteRepository.GetById(Id);
        }

        public IList<BuyerChannelScheduleDay> GetBuyerChannelScheduleDays(long buyerChannelId, short dayValue = 0, bool fromCache = true)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (!fromCache)
            {
                return (from x in _buyerChannelScheduleDayRepository.Table
                        where x.BuyerChannelId == buyerChannelId && (x.DayValue == dayValue || dayValue == 0)
                        select x).ToList();
            }

            string key = string.Format(CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelScheduleDay, buyerChannelId, dayValue);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelScheduleDayRepository.Table
                            where x.BuyerChannelId == buyerChannelId && (x.DayValue == dayValue || dayValue == 0)
                            select x;

                return query.ToList();
            });
        }

        public IList<BuyerChannelScheduleTimePeriod> GetBuyerChannelScheduleTimePeriod(long scheduleDayId, bool fromCache = true)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (!fromCache)
            {
                return (from x in _buyerChannelScheduleTimePeriodRepository.Table
                        where x.ScheduleDayId == scheduleDayId
                        select x).ToList();
            }

            string key = string.Format(CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelScheduleTimePeriod, scheduleDayId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerChannelScheduleTimePeriodRepository.Table
                            where x.ScheduleDayId == scheduleDayId
                            select x;

                return query.ToList();
            });
        }

        public virtual long InsertBuyerChannelScheduleDay(BuyerChannelScheduleDay buyerChannelScheduleDay)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelScheduleDay == null)
                throw new ArgumentNullException("buyerChannelScheduleDay");

            _buyerChannelScheduleDayRepository.Insert(buyerChannelScheduleDay);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelScheduleDay);

            return buyerChannelScheduleDay.Id;
        }


        public void UpdateBuyerChannelScheduleDay(BuyerChannelScheduleDay buyerChannelScheduleDay)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelScheduleDay == null)
                throw new ArgumentNullException("buyerChannelScheduleDay");


            _buyerChannelScheduleDayRepository.Update(buyerChannelScheduleDay);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelScheduleDay);
        }

        public BuyerChannelScheduleDay GetBuyerChannelScheduleDay(long id)
        {
            return _buyerChannelScheduleDayRepository.GetById(id);
        }

        public virtual long InsertBuyerChannelScheduleTimePeriod(BuyerChannelScheduleTimePeriod buyerChannelScheduleTimePeriod)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelScheduleTimePeriod == null)
                throw new ArgumentNullException("buyerChannelScheduleTimePeriod");

            _buyerChannelScheduleTimePeriodRepository.Insert(buyerChannelScheduleTimePeriod);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelScheduleTimePeriod);

            return buyerChannelScheduleTimePeriod.Id;
        }

        public BuyerChannelScheduleTimePeriod InsertBuyerChannelScheduleTimePeriod(long scheduleDayId, int fromTime = 0, int toTime = 0, int quantity = 0, int postedWait = 0, int soldWait = 0, int hourMax = 0, decimal price = 0, short leadStatus = -1)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            BuyerChannelScheduleTimePeriod obj = new BuyerChannelScheduleTimePeriod();

            obj.ScheduleDayId = scheduleDayId;
            obj.FromTime = fromTime;
            obj.ToTime = toTime;
            obj.Quantity = quantity;
            obj.PostedWait = postedWait;
            obj.SoldWait = soldWait;
            obj.HourMax = hourMax;
            obj.Price = price;
            obj.LeadStatus = leadStatus;

            _buyerChannelScheduleTimePeriodRepository.Insert(obj);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(obj);
   
            return obj;
        }

        public BuyerChannelScheduleTimePeriod UpdateBuyerChannelScheduleTimePeriod(long id, int fromTime = 0, int toTime = 0, int quantity = 0, int postedWait = 0, int soldWait = 0, int hourMax = 0, decimal price = 0, short leadStatus = -1)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            BuyerChannelScheduleTimePeriod obj = _buyerChannelScheduleTimePeriodRepository.GetById(id);
            if (obj == null)
                throw new ArgumentNullException("buyerChannelScheduleTimePeriod");

            obj.FromTime = fromTime;
            obj.ToTime = toTime;
            obj.Quantity = quantity;
            obj.PostedWait = postedWait;
            obj.SoldWait = soldWait;
            obj.HourMax = hourMax;
            obj.Price = price;
            obj.LeadStatus = leadStatus;
            _buyerChannelScheduleTimePeriodRepository.Update(obj);
            
            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(obj);

            return obj;
        }

        public void DeleteBuyerChannelScheduleTimePeriod(long id)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            BuyerChannelScheduleTimePeriod obj = _buyerChannelScheduleTimePeriodRepository.GetById(id);
            if (obj == null)
                throw new ArgumentNullException("buyerChannelScheduleTimePeriod");

            _buyerChannelScheduleTimePeriodRepository.Delete(obj);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(obj);
        }

        public virtual IList<BuyerChannelHoliday> GetBuyerChannelHolidays(long buyerChannelId, int year = 0, bool fromCache = true)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (!fromCache)
            {
                return (from x in _buyerChannelHolidayRepository.Table
                        where x.BuyerChannelId == buyerChannelId && (year == 0 || x.HolidayDate.Year == year)
                        select x).ToList();
            }

            string key = string.Format(CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidays, buyerChannelId, year);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelHolidayRepository.Table
                        where x.BuyerChannelId == buyerChannelId && (year == 0 || x.HolidayDate.Year == year)
                        select x).ToList();
            });
        }

        public virtual BuyerChannelHoliday GetBuyerChannelHoliday(long buyerChannelId, DateTime date)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidays2, buyerChannelId, date);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelHolidayRepository.Table
                        where x.BuyerChannelId == buyerChannelId && x.HolidayDate == date
                        select x).FirstOrDefault();
            });
        }

        public virtual BuyerChannelHoliday GetBuyerChannelHolidayById(long Id)
        {

            return (from x in _buyerChannelHolidayRepository.Table
                    where x.Id == Id
                    select x).FirstOrDefault();

            string key = string.Format(CACHE_BUYER_CHANNEL_HOLIDAY_GetBuyerChannelHolidayById, Id);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerChannelHolidayRepository.Table
                        where x.Id == Id
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Insert holiday
        /// </summary>
        /// <param name="buyerChannelHoliday">The buyer channel.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerChannel</exception>
        public virtual long InsertBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelHoliday == null)
                throw new ArgumentNullException("buyerChannelHoliday");

            _buyerChannelHolidayRepository.Insert(buyerChannelHoliday);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerChannelHoliday);

            return buyerChannelHoliday.Id;
        }

        /// <summary>
        /// Update holiday
        /// </summary>
        /// <param name="buyerChannelHoliday">The buyer channel holiday.</param>
        /// <exception cref="ArgumentNullException">buyerChannelHoliday</exception>
        public virtual void UpdateBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelHoliday == null)
                throw new ArgumentNullException("buyerChannel");

            _buyerChannelHolidayRepository.Update(buyerChannelHoliday);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyerChannelHoliday);
        }

        /// <summary>
        /// Delete holiday
        /// </summary>
        /// <param name="buyerChannelHoliday">The buyer channel holiday.</param>
        /// <exception cref="ArgumentNullException">buyerChannelHoliday</exception>
        public virtual void DeleteBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (buyerChannelHoliday == null)
                throw new ArgumentNullException("buyerChannelHoliday");

            _buyerChannelHolidayRepository.Delete(buyerChannelHoliday);

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(buyerChannelHoliday);
        }


        public void ResetBuyerChannelSchedule(long id)
        {
            var days = this.GetBuyerChannelScheduleDays(id, fromCache: false);

            foreach(var day in days)
            {
                var timePeriods = this.GetBuyerChannelScheduleTimePeriod(day.Id, false);
                foreach(var timePeriod in timePeriods)
                {
                    _buyerChannelScheduleTimePeriodRepository.Delete(timePeriod);
                }

                _buyerChannelScheduleDayRepository.Delete(day);
            }

            _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
        }
        public void ResetBuyerChannelHolidays(long id, int year)
        {
            var holidays = this.GetBuyerChannelHolidays(id, year, false);

            foreach(var h in holidays)
            {
                _buyerChannelHolidayRepository.Delete(h);
            }

            if (holidays.Count > 0)
            {
                _cacheManager.RemoveByPattern(CACHE_BUYER_CHANNEL_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
            }
        }

        #endregion Methods
    }
}