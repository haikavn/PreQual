// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateChannelService.cs" company="Adrack.com">
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
using System.Linq;
using System.Reflection;
using Adrack.PlanManagement;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class AffiliateChannelService.
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateChannelService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateChannelService" />
    public partial class AffiliateChannelService : IAffiliateChannelService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_BY_ID_KEY = "App.Cache.AffiliateChannel.By.Id-{0}";

        /// <summary>
        /// The cache affiliate channel template by key key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_BY_KEY_KEY = "App.Cache.AffiliateChannel.By.Key-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_ALL_KEY = "App.Cache.AffiliateChannel.All";

        /// <summary>
        /// The cache affiliate channel by campaign template all key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_BY_CAMPAIGN_TEMPLATE_ALL_KEY = "App.Cache.AffiliateChannel.AffiliateChannelByCampaign.All";

        private const string CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannels = "App.Cache.AffiliateChannel.GetAllAffiliateChannels-{0}";

        private const string CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannelsByCampaignId = "App.Cache.AffiliateChannel.GetAllAffiliateChannelsByCampaignId-{0}-{1}";

        private const string CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannelsByAffiliateId = "App.Cache.AffiliateChannel.GetAllAffiliateChannelsByAffiliateId-{0}-{1}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY = "App.Cache.AffiliateChannel.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<AffiliateChannelNote> _affiliateChannelNoteRepository;
        private readonly IRepository<AffiliateChannel> _affiliateChannelRepository;
        private readonly IRepository<AttachedChannel> _attachedChannelRepository;


        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="affiliateChannelRepository">The affiliate channel repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public AffiliateChannelService(IRepository<AffiliateChannel> affiliateChannelRepository, 
            IRepository<AffiliateChannelNote> affiliateChannelNoteRepository,
            IRepository<AttachedChannel> attachedChannelRepository,
            ICacheManager cacheManager,
            IAppEventPublisher appEventPublisher,
            IAppContext appContext)
        {
            this._affiliateChannelNoteRepository = affiliateChannelNoteRepository;
            this._affiliateChannelRepository = affiliateChannelRepository;
            this._attachedChannelRepository = attachedChannelRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._appContext = appContext;
        }

        #endregion Constructor

        #region Methods
        IQueryable<AffiliateChannel> FilterAllowed(IQueryable<AffiliateChannel> query, User user = null)
        {
            //return query;

            if (user == null)
                user = _appContext.AppUser;

            var affiliateService = AppEngineContext.Current.Resolve<IAffiliateService>();
            var userService = AppEngineContext.Current.Resolve<IUserService>();

            List<long> allowedAffiliateChannelIds = (List<long>)userService.GetUserEntityIds(user.Id, "affiliatechannel", Guid.Empty);

            if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                query = query.Where(x => allowedAffiliateChannelIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.NetowrkUserTypeId)
            {
                //var affiliateIds = affiliateService.GetAllAffiliates().Select(x => x.Id);
                //query = query.Where(x => affiliateIds.Contains(x.AffiliateId) && allowedAffiliateChannelIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.BuyerUserTypeId)
            {
                //var buyerChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>();
                //var campaignIds = buyerChannelService.GetAllBuyerChannels().Select(x => x.CampaignId).ToList();
                //query = query.Where(x =>(x.CampaignId == null ||  campaignIds.Contains(x.CampaignId.Value)) && allowedAffiliateChannelIds.Contains(x.Id));
                query = query.Where(x => x.Id == -1);
            }

            return query;
        }

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual AffiliateChannel GetAffiliateChannelById(long Id, bool cached = false)
        {

            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name,this);

            

            if (Id == 0)
                return null;

            if (!cached)
                return _affiliateChannelRepository.GetById(Id);

            string key = string.Format(CACHE_AFFILIATE_CHANNEL_TEMPLATE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _affiliateChannelRepository.GetById(Id); });
        }

        /// <summary>
        /// Gets the affiliate channel by key.
        /// </summary>
        /// <param name="affiliateChannelKey">The affiliate channel key.</param>
        /// <returns>AffiliateChannel.</returns>
        public virtual AffiliateChannel GetAffiliateChannelByKey(string affiliateChannelKey)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_TEMPLATE_BY_KEY_KEY, affiliateChannelKey);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _affiliateChannelRepository.Table
                        where x.ChannelKey == affiliateChannelKey
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets the name of the affiliate channel by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>AffiliateChannel.</returns>
        public virtual AffiliateChannel GetAffiliateChannelByName(string name, long exceptId)
        {
            
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name,this);
            return (from x in _affiliateChannelRepository.Table
                    where x.Name == name && x.Id != exceptId
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateChannel> GetAllAffiliateChannels(short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannels, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<AffiliateChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<long> GetAllAffiliateChannelIds(short deleted = 0)
        {
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannels, deleted);

           
                var query = (from x in _affiliateChannelRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x.Id);

               
            return query.Distinct().ToList();
        }

        /// <summary>
        /// Gets all affiliate channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        public virtual IList<AffiliateChannel> GetAllAffiliateChannelsByCampaignId(long campaignId, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannelsByCampaignId, campaignId, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelRepository.Table
                            where x.CampaignId == campaignId && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<AffiliateChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        public virtual IList<AffiliateChannel> GetAllAffiliateChannelsByMultipleCampaignId(List<long> campaignId, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _affiliateChannelRepository.Table
                        where campaignId.Contains(x.CampaignId.HasValue ? x.CampaignId.Value : 0)
                        && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) 
                            || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                        orderby x.Id
                        select x;
            return query.ToList();
        }

        public virtual IList<AffiliateChannel> GetAllAffiliateChannelsByMultipleAffiliateIds(List<long> affiliateIds, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _affiliateChannelRepository.Table
                        where affiliateIds.Contains(x.AffiliateId)
                        && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue))
                            || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                        orderby x.Id
                        select x;
            return query.ToList();
        }

        /// <summary>
        /// Gets all affiliate channels by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        public virtual IList<AffiliateChannel> GetAllAffiliateChannelsByAffiliateId(long affiliateId, short deleted = 0)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_GetAllAffiliateChannelsByAffiliateId, affiliateId, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelRepository.Table
                            where x.AffiliateId == affiliateId && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<AffiliateChannel>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateChannel</exception>
        public virtual long InsertAffiliateChannel(AffiliateChannel affiliateChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (affiliateChannel == null)
                throw new ArgumentNullException("affiliateChannel");

            _affiliateChannelRepository.SetCanTrackChanges(true);

            _affiliateChannelRepository.Insert(affiliateChannel);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(affiliateChannel);
            _cacheManager.ClearRemoteServersCache();
            return affiliateChannel.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <exception cref="ArgumentNullException">affiliateChannel</exception>
        public virtual void UpdateAffiliateChannel(AffiliateChannel affiliateChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (affiliateChannel == null)
                throw new ArgumentNullException("affiliateChannel");

            _affiliateChannelRepository.SetCanTrackChanges(true);
            _affiliateChannelRepository.Update(affiliateChannel);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(affiliateChannel);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <exception cref="ArgumentNullException">affiliateChannel</exception>
        public virtual void DeleteAffiliateChannel(AffiliateChannel affiliateChannel)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);

            _affiliateChannelRepository.SetCanTrackChanges(true);

            if (affiliateChannel == null)
                throw new ArgumentNullException("affiliateChannel");

            _affiliateChannelRepository.Delete(affiliateChannel);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_CHANNEL_TEMPLATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliateChannel);
        }
        public virtual void DeleteAffiliateNote(AffiliateChannelNote affiliateChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (affiliateChannelNote == null)
                throw new ArgumentNullException("affiliateNote");

            _affiliateChannelNoteRepository.Delete(affiliateChannelNote);

            _appEventPublisher.EntityDeleted(affiliateChannelNote);
        }
        public virtual long InsertAffiliateChannelNote(AffiliateChannelNote affiliateChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (affiliateChannelNote == null)
                throw new ArgumentNullException("affiliateNote");

            _affiliateChannelNoteRepository.Insert(affiliateChannelNote);

            _appEventPublisher.EntityInserted(affiliateChannelNote);
            _cacheManager.ClearRemoteServersCache();
            return affiliateChannelNote.Id;
        }
        public virtual void UpdateAffiliateChannelNote(AffiliateChannelNote affiliateChannelNote)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (affiliateChannelNote == null)
                throw new ArgumentNullException("affiliateChannelNote");

            _affiliateChannelNoteRepository.Update(affiliateChannelNote);

            _appEventPublisher.EntityUpdated(affiliateChannelNote);
        }
        public virtual AffiliateChannelNote GetAffiliateChannelNoteById(long Id)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            if (Id == 0)
                return null;

            return _affiliateChannelNoteRepository.GetById(Id);
        }
        public virtual IList<AffiliateChannelNote> GetAllAffiliateChannelNotesByAffiliateChannelId(long affiliateChannelId)
        {
            ServiceFeatureControl.ValidateProcedure(MethodBase.GetCurrentMethod().ReflectedType.Name, this);
            var query = from x in _affiliateChannelNoteRepository.Table
                        where x.AffiliateChannelId == affiliateChannelId
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

        public virtual IList<BuyerChannel> GetAttachedBuyerChannels(long Id)
        {
            AffiliateChannel ac = this.GetAffiliateChannelById(Id);

            var buyerChannelIds = (from x in _attachedChannelRepository.Table
                                       where x.AffiliateChannelId == Id
                                       select x.BuyerChannelId).ToList();

            var buyerChannelsService = AppEngineContext.Current.Resolve<IBuyerChannelService>();

            List<BuyerChannel> list = new List<BuyerChannel>();

            foreach (var buyerChannelId in buyerChannelIds)
            {
                list.Add(buyerChannelsService.GetBuyerChannelById(buyerChannelId));
            }

            return list;
        }

        public virtual void AttachBuyerChannel(long affiliateChannelId, long buyerChannelId)
        {
            AttachedChannel attachedChannel = new AttachedChannel()
            {
                AffiliateChannelId = affiliateChannelId,
                BuyerChannelId = buyerChannelId
            };

            _attachedChannelRepository.Insert(attachedChannel);
            _appEventPublisher.EntityInserted(attachedChannel);
        }

        public virtual void DettachBuyerChannel(long affiliateChannelId, long buyerChannelId)
        {
            AttachedChannel attachedChannel = (from x in _attachedChannelRepository.Table
                                                                     where x.AffiliateChannelId == affiliateChannelId && x.BuyerChannelId == buyerChannelId
                                                                     select x).FirstOrDefault();

            if (attachedChannel != null)
            {
                _attachedChannelRepository.Delete(attachedChannel);
                _appEventPublisher.EntityDeleted(attachedChannel);
            }
        }

        #endregion Methods
    }
}