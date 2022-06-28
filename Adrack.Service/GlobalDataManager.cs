// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="GlobalDataManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Lead;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service
{
    /// <summary>
    /// Class GlobalDataManager.
    /// </summary>
    public class GlobalDataManager
    {
        /// <summary>
        /// The memory manager
        /// </summary>
        protected static PerRequestCacheManager _memoryManager;

        /// <summary>
        /// The current lead count
        /// </summary>
        public static long CurrentLeadCount = 0;

        /// <summary>
        /// The waiting leads
        /// </summary>
        public static long WaitingLeads = 0;

        /// <summary>
        /// The maximum processing leads
        /// </summary>
        public static int MaxProcessingLeads = 0;

        /// <summary>
        /// The processing delay
        /// </summary>
        public static int ProcessingDelay = 0;

        /// <summary>
        /// The lock fill report
        /// </summary>
        public static bool LockFillReport = false;

        /// <summary>
        /// Gets or sets the memory manager.
        /// </summary>
        /// <value>The memory manager.</value>
        public static PerRequestCacheManager MemoryManager
        {
            get
            {
                return _memoryManager;
            }
            set
            {
                _memoryManager = value;
            }
        }

        /// <summary>
        /// Reloads the data.
        /// </summary>
        public static void ReloadData()
        {
            var campaignService = AppEngineContext.Current.Resolve<ICampaignService>();
            var campaignTemplateService = AppEngineContext.Current.Resolve<ICampaignTemplateService>();

            var affiliateService = AppEngineContext.Current.Resolve<IAffiliateService>();
            var affiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
            var affiliateChannelTemplateService = AppEngineContext.Current.Resolve<IAffiliateChannelTemplateService>();

            var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();
            var buyerChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>();
            var buyerChannelTemplateService = AppEngineContext.Current.Resolve<IBuyerChannelTemplateService>();

            List<Campaign> campaigns = (List<Campaign>)campaignService.GetAllCampaigns(0);

            _memoryManager.Set("Global.Data.Campaigns", campaigns.ToDictionary(x => x.Id), 1080);

            foreach (Campaign campaign in campaigns)
            {
                List<CampaignField> campaignTemplates = (List<CampaignField>)campaignTemplateService.GetCampaignTemplatesByCampaignId(campaign.Id);
                _memoryManager.Set("Global.Data.CampaignFields." + campaign.Id.ToString(), campaignTemplates.ToDictionary(x => x.Id), 1080);
                _memoryManager.Set("Global.Data.CampaignFieldsList." + campaign.Id.ToString(), campaignTemplates, 1080);
            }

            List<Affiliate> affiliates = (List<Affiliate>)affiliateService.GetAllAffiliates(0);

            _memoryManager.Set("Global.Data.Affiliates", affiliates.ToDictionary(x => x.Id), 1080);
            _memoryManager.Set("Global.Data.AffiliateChannels", affiliateChannelService.GetAllAffiliateChannels(0).ToDictionary(x => x.ChannelKey), 1080);

            foreach (Affiliate affiliate in affiliates)
            {
                List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliate.Id, 0);
                _memoryManager.Set("Global.Data.AffiliateChannels." + affiliate.Id.ToString(), affiliateChannels.ToDictionary(x => x.Id), 1080);

                foreach (AffiliateChannel affiliateChannel in affiliateChannels)
                {
                    List<AffiliateChannelTemplate> affiliateChannelTemplates = (List<AffiliateChannelTemplate>)affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannel.Id);
                    _memoryManager.Set("Global.Data.AffiliateChannelFields." + affiliateChannel.Id.ToString(), affiliateChannelTemplates.ToDictionary(x => x.Id), 1080);
                    _memoryManager.Set("Global.Data.AffiliateChannelFieldsList." + affiliateChannel.Id.ToString(), affiliateChannelTemplates, 1080);
                }
            }

            List<Buyer> buyers = (List<Buyer>)buyerService.GetAllBuyers(0);

            _memoryManager.Set("Global.Data.Buyers", buyers.ToDictionary(x => x.Id), 1080);
            _memoryManager.Set("Global.Data.BuyerChannels", buyerChannelService.GetAllBuyerChannels(0).ToDictionary(x => x.Id), 1080);

            foreach (Buyer buyer in buyers)
            {
                List<BuyerChannel> buyerChannels = (List<BuyerChannel>)buyerChannelService.GetAllBuyerChannelsByBuyerId(buyer.Id, 0);
                _memoryManager.Set("Global.Data.BuyerChannels." + buyer.Id.ToString(), buyerChannels.ToDictionary(x => x.Id), 1080);

                foreach (BuyerChannel buyerChannel in buyerChannels)
                {
                    List<BuyerChannelTemplate> buyerChannelTemplates = (List<BuyerChannelTemplate>)buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id);
                    _memoryManager.Set("Global.Data.BuyerChannelFields." + buyerChannel.Id.ToString(), buyerChannelTemplates.ToDictionary(x => x.Id), 1080);
                }
            }
        }

        /// <summary>
        /// Initializes the memory manager.
        /// </summary>
        public static void InitMemoryManager()
        {
            _memoryManager = (PerRequestCacheManager)AppEngineContext.Current.Resolve<ICacheManager>();

            ReloadData();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalDataManager" /> class.
        /// </summary>
        public GlobalDataManager()
        {
        }
    }
}