// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CampaignService.cs" company="Adrack.com">
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
using System.Linq;
using System.Xml;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class CampaignService.
    /// Implements the <see cref="Adrack.Service.Lead.ICampaignService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ICampaignService" />
    public partial class CampaignService : ICampaignService
    {
        #region Constants

        /// <summary>
        /// Cache Campaign By Id Key
        /// </summary>
        private const string CACHE_CAMPAIGN_BY_ID_KEY = "App.Cache.Campaign.By.Id-{0}";

        /// <summary>
        /// Cache Campaign XML By Id
        /// </summary>
        private const string CACHE_CAMPAIGN_BY_ID_KEY_XML = "App.Cache.Campaign.By.Id.Xml-{0}";

        /// <summary>
        /// Cache Campaign All Key
        /// </summary>
        private const string CACHE_CAMPAIGN_ALL_KEY = "App.Cache.Campaign.All";

        /// <summary>
        /// Cache Campaign All Key
        /// </summary>
        private const string CACHE_CAMPAIGN_BY_NAME = "App.Cache.Campaign.By.Name-{0}-{1}";

        private const string CACHE_CAMPAIGN_GetAllCampaigns = "App.Cache.Campaign.GetAllCampaigns-{0}";
        private const string CACHE_CAMPAIGN_GetAllCampaignsByStatus = "App.Cache.Campaign.GetAllCampaignsByStatus-{0}-{1}";

        private const string CACHE_CAMPAIGN_GetCampaignListWithVerticals = "App.Cache.Campaign.GetCampaignListWithVertical-{0}";

        private const string CACHE_CAMPAIGN_GetAllCampaigns2 = "App.Cache.Campaign.GetAllCampaigns-{0}-{1}";

        private const string CACHE_CAMPAIGN_GetTemplateCampaigns = "App.Cache.Campaign.GetTemplateCampaigns";

        private const string CACHE_CAMPAIGN_GetCampaignsByVerticalId = "App.Cache.Campaign.GetCampaignsByVerticalId-{0}-{1}";

        private const string CACHE_CAMPAIGN_GetPingTrees = "App.Cache.Campaign.GetPingTrees-{0}";

        private const string CACHE_CAMPAIGN_GetPingTreeItems = "App.Cache.Campaign.GetPingTreeItems-{0}";

        private const string CACHE_PINGTREE_BY_ID_KEY = "App.Cache.Campaign.PingTree.By.Id-{0}";

        private const string CACHE_PINGTREEITEM_BY_ID_KEY = "App.Cache.Campaign.PingTreeItem.By.Id-{0}";

        /// <summary>
        /// Cache Campaign Pattern Key
        /// </summary>
        private const string CACHE_CAMPAIGN_PATTERN_KEY = "App.Cache.Campaign.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Vertical
        /// </summary>
        private readonly IRepository<Vertical> _verticalRepository;

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Campaign> _campaignRepository;

        private readonly IRepository<PingTree> _pingTreeRepository;

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
        /// <param name="campaignRepository">The campaign repository.</param>
        /// <param name="verticalRepository">The vertical repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CampaignService(IRepository<Campaign> campaignRepository,
            IRepository<Vertical> verticalRepository,
            IRepository<PingTree> pingTreeRepository,
            IRepository<PingTreeItem> pingTreeItemRepository,
            ICacheManager cacheManager,
            IDataProvider dataProvider,
            IAppEventPublisher appEventPublisher,
            IAppContext appContext)
        {
            this._campaignRepository = campaignRepository;
            this._verticalRepository = verticalRepository;
            this._pingTreeRepository = pingTreeRepository;
            this._pingTreeItemRepository = pingTreeItemRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._appContext = appContext;
        }

        #endregion Constructor

        #region Methods

        public IQueryable<Campaign> FilterAllowed(IQueryable<Campaign> query, User user = null)
        {
            //return query;

            if (user == null)
                user = _appContext.AppUser;

            var affiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
            var buyerChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>();
            var userService = AppEngineContext.Current.Resolve<IUserService>();

            List<long> allowedCampaignIds = (List<long>)userService.GetUserEntityIds(user.Id, "campaign", Guid.Empty);

            if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                //var campaignIds = affiliateChannelService.GetAllAffiliateChannels().Select(x => x.CampaignId).ToList();
                //query = query.Where(x => campaignIds.Contains(x.Id) && allowedCampaignIds.Contains(x.Id));
                query = query.Where(x => allowedCampaignIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.BuyerUserTypeId)
            {
                //var campaignIds = buyerChannelService.GetAllBuyerChannels().Select(x => x.CampaignId).ToList();
                //query = query.Where(x => campaignIds.Contains(x.Id) && allowedCampaignIds.Contains(x.Id));
                query = query.Where(x => allowedCampaignIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.NetowrkUserTypeId)
            {
                //var campaignIds1 = affiliateChannelService.GetAllAffiliateChannels().Select(x => x.CampaignId).ToList();
                //var campaignIds2 = buyerChannelService.GetAllBuyerChannels().Select(x => (long?)x.CampaignId).ToList();
                //campaignIds1.AddRange(campaignIds2);
                //query = query.Where(x => campaignIds1.Contains(x.Id) && allowedCampaignIds.Contains(x.Id));
                query = query.Where(x => allowedCampaignIds.Contains(x.Id));
            }

            return query;
        }

        public List<CampaignList> GetCampaignListWithVerticals(ActivityStatuses status)
        {
            var key = string.Format(CACHE_CAMPAIGN_GetCampaignListWithVerticals, status);
            return _cacheManager.Get(key, () =>
            {
                return (from c in _campaignRepository.Table
                        join v in _verticalRepository.Table on c.VerticalId equals v.Id
                        where (status == ActivityStatuses.All || c.Status == status) && ((c.Deleted.HasValue && !c.Deleted.Value) || !c.Deleted.HasValue)
                        select new CampaignList()
                        {
                            CampaignName = c.Name,
                            CampaignId = c.Id,
                            Vertical = v.Name,
                            VerticalId = c.VerticalId,
                            Revenue = 0,
                            Cost = 0,
                            Profit = 0,
                            Status = c.Status,
                        }).ToList();
            });

        }
        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Campaign GetCampaignById(long campaignId, bool cached = false)
        {
            if (campaignId == 0)
                return null;

            if (!cached)
                return _campaignRepository.GetById(campaignId);

            string key = string.Format(CACHE_CAMPAIGN_BY_ID_KEY, campaignId);

            return _cacheManager.Get(key, () => { return _campaignRepository.GetById(campaignId); });
        }



        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual XmlDocument GetCampaignByIdXml(long campaignId)
        {
            if (campaignId == 0)
                return null;

            string key = string.Format(CACHE_CAMPAIGN_BY_ID_KEY_XML, campaignId);
            Campaign campaign = GetCampaignById(campaignId);
            if (campaign != null)
                return _cacheManager.Get(key, () =>
                {
                    if (string.IsNullOrEmpty(campaign.DataTemplate))
                    {
                        return null;
                    }
                    var xml = new XmlDocument(); 
                    xml.LoadXml(campaign.DataTemplate);
                    return xml;
                });
            return null;
        }

        /// <summary>
        /// Gets the name of the campaign by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Campaign.</returns>
        public virtual Campaign GetCampaignByName(string name, long exceptId)
        {
            string key = String.Format(CACHE_CAMPAIGN_BY_NAME, name, exceptId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _campaignRepository.Table
                        where x.Name == name && x.Id != exceptId && (!x.IsDeleted.HasValue || !x.IsDeleted.Value)
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Campaign> GetAllCampaigns(short deleted = 0)
        {
            string key = string.Format(CACHE_CAMPAIGN_GetAllCampaigns, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _campaignRepository.Table
                            join y in _verticalRepository.Table on x.VerticalId equals y.Id
                            where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Campaign>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="status">The status.</param>
        /// <returns>Campaign Collection Item</returns>
        public virtual IList<Campaign> GetAllCampaignsByStatus(short deleted = 0, short status = -1)
        {
            string key = string.Format(CACHE_CAMPAIGN_GetAllCampaignsByStatus, deleted, status);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _campaignRepository.Table
                            join y in _verticalRepository.Table on x.VerticalId equals y.Id
                            where x.IsTemplate == false && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                                                        && (status == -1 || (status == 0 && x.Status == 0) || (status == 1 && x.Status == ActivityStatuses.Active))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Campaign>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all campaigns.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        public virtual IList<Campaign> GetAllCampaigns(short type, short deleted = 0)
        {
            string key = string.Format(CACHE_CAMPAIGN_GetAllCampaigns, type, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _campaignRepository.Table
                            where x.IsTemplate == false && x.CampaignType == (CampaignTypes)type && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Campaign>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets the template campaigns.
        /// </summary>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        public virtual IList<Campaign> GetTemplateCampaigns()
        {
            string key = CACHE_CAMPAIGN_GetTemplateCampaigns;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _campaignRepository.Table
                            where x.IsTemplate == true
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets the campaigns by vertical identifier.
        /// </summary>
        /// <param name="verticalId">The vertical identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        public virtual IList<Campaign> GetCampaignsByVerticalId(long verticalId, short deleted = 0)
        {
            string key = string.Format(CACHE_CAMPAIGN_GetCampaignsByVerticalId, verticalId, deleted);

            return _cacheManager.Get(key, () =>
            {

                var query = from x in _campaignRepository.Table
                            where x.VerticalId == verticalId && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets the campaign templates by vertical identifier.
        /// </summary>
        /// <param name="verticalId">The vertical identifier.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        public virtual IList<Campaign> GetCampaignTemplatesByVerticalId(long verticalId)
        {
            var query = from x in _campaignRepository.Table
                        where x.VerticalId == verticalId && x.IsTemplate == true
                        orderby x.Id descending
                        select x;

            var profiles = query.ToList();

            return profiles;
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">campaign</exception>
        public virtual long InsertCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.SetCanTrackChanges(true);

            _campaignRepository.Insert(campaign);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(campaign);

            return campaign.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <exception cref="ArgumentNullException">campaign</exception>
        public virtual void UpdateCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.SetCanTrackChanges(true);
            _campaignRepository.Update(campaign);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(campaign);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <exception cref="ArgumentNullException">campaign</exception>
        public virtual void DeleteCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.SetCanTrackChanges(true);

            _campaignRepository.Delete(campaign);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(campaign);
        }

        public virtual long InsertPingTree(PingTree pingTree)
        {
            if (pingTree == null)
                throw new ArgumentNullException("pingTree");

            _pingTreeRepository.SetCanTrackChanges(true);

            _pingTreeRepository.Insert(pingTree);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(pingTree);

            return pingTree.Id;
        }

        public virtual void UpdatePingTree(PingTree pingTree)
        {
            if (pingTree == null)
                throw new ArgumentNullException("pingTree");

            _pingTreeRepository.SetCanTrackChanges(true);
            _pingTreeRepository.Update(pingTree);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(pingTree);
        }

        public virtual void DeletePingTree(PingTree pingTree)
        {
            if (pingTree == null)
                throw new ArgumentNullException("pingTree");

            _pingTreeRepository.SetCanTrackChanges(true);

            _pingTreeRepository.Delete(pingTree);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(pingTree);
        }

        public virtual long InsertPingTreeItem(PingTreeItem pingTreeItem)
        {
            if (pingTreeItem == null)
                throw new ArgumentNullException("pingTreeItem");

            _pingTreeItemRepository.SetCanTrackChanges(true);

            _pingTreeItemRepository.Insert(pingTreeItem);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(pingTreeItem);

            return pingTreeItem.Id;
        }

        public virtual void UpdatePingTreeItem(PingTreeItem pingTreeItem)
        {
            if (pingTreeItem == null)
                throw new ArgumentNullException("pingTreeItem");

            _pingTreeItemRepository.SetCanTrackChanges(true);
            _pingTreeItemRepository.Update(pingTreeItem);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(pingTreeItem);
        }

        public virtual void DeletePingTreeItem(PingTreeItem pingTreeItem)
        {
            if (pingTreeItem == null)
                throw new ArgumentNullException("pingTreeItem");

            DeletePingTreeItems(pingTreeItem.Id);

            _pingTreeItemRepository.SetCanTrackChanges(true);
            _pingTreeItemRepository.Delete(pingTreeItem);

            _cacheManager.RemoveByPattern(CACHE_CAMPAIGN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(pingTreeItem);
        }

        public virtual IList<PingTree> GetPingTrees(long campaignId)
        {
            string key = string.Format(CACHE_CAMPAIGN_GetPingTrees, campaignId);

            return _cacheManager.Get(key, () =>
            {

                var query = from x in _pingTreeRepository.Table
                            where x.CampaignId == campaignId
                            orderby x.Id descending
                            select x;

                return query.ToList();
            });
        }

        public virtual IList<PingTreeItem> GetPingTreeItems(long pingTreeId, bool cached = false)
        {
            if (!cached)
            {
                return (from x in _pingTreeItemRepository.Table
                        where x.PingTreeId == pingTreeId
                        orderby x.Id descending
                        select x).ToList();
            }

            string key = string.Format(CACHE_CAMPAIGN_GetPingTreeItems, pingTreeId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _pingTreeItemRepository.Table
                            where x.PingTreeId == pingTreeId
                            orderby x.Id descending
                            select x;

                return query.ToList();
            });
        }


        public virtual PingTree GetPingTreeById(long Id, bool cached = false)
        {
            if (Id == 0)
                return null;

            if (!cached)
                return _pingTreeRepository.GetById(Id);

            string key = string.Format(CACHE_PINGTREE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _pingTreeRepository.GetById(Id); });
        }

        public virtual PingTreeItem GetPingTreeItemById(long Id, bool cached = false)
        {
            if (Id == 0)
                return null;

            if (!cached)
                return _pingTreeItemRepository.GetById(Id);

            string key = string.Format(CACHE_PINGTREEITEM_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _pingTreeItemRepository.GetById(Id); });
        }

        public void DeletePingTreeItems(long pingTreeId)
        {
            var items = (from x in _pingTreeItemRepository.Table
                         where x.PingTreeId == pingTreeId
                         orderby x.Id descending
                         select x).ToList();

            foreach (var item in items)
            {
                _pingTreeItemRepository.Delete(item);
            }
        }

        /// <summary>
        /// Deletes the campaign templates.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        public virtual void DeleteCampaignTemplates(long campaignId)
        {
            var UserIdParam = _dataProvider.GetParameter();
            UserIdParam.ParameterName = "campaignid";
            UserIdParam.Value = campaignId;
            UserIdParam.DbType = DbType.Int64;

            _campaignRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[DeleteCampaignTemplates] @campaignid", UserIdParam).FirstOrDefault();
        }

        #endregion Methods

        public string GetConnectionString()
        {
            return _campaignRepository.GetConnectionString();
        }

        public List<string> GetAllKeys()
        {
            return _campaignRepository.GetAllKeys();
        }
    }
}