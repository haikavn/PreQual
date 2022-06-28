using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Infrastructure.Core.Services
{
    public class UsersExtensionService : IUsersExtensionService
    {
        private readonly IAffiliateService _affiliateService;
        private readonly ICampaignService _campaignService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IUserService _userService;
        private readonly IAddonService _addonService;


        public UsersExtensionService(ICampaignService campaignService,
                                     IAffiliateService affiliateService,
                                     IAffiliateChannelService affiliateChannelService,
                                     IBuyerService buyerService,
                                     IBuyerChannelService buyerChannelService,
                                     IUserService userService,
                                     IAddonService addonService)
        {
            _campaignService = campaignService;
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _userService = userService;
            _addonService = addonService;
        }
        #region methods


        public List<IdNameModel> GetCampaignOwnerships(long id)
        {
            var campaigns = new List<IdNameModel>();
            var campaignIds = _userService.GetUserEntityIds(id, "Campaign", Guid.Empty).ToList();
            foreach (var campaignId in campaignIds)
            {
                var campaign = _campaignService.GetCampaignById(campaignId);
                if (campaign != null)
                {
                    campaigns.Add(new IdNameModel
                    {
                        Id = campaign.Id,
                        Name = campaign.Name
                    });
                }
            }

            return campaigns;
        }

        public List<IdNameModel> GetAffiliateOwnerships(long id)
        {
            var affiliates = new List<IdNameModel>();
            var affiliateIds = _userService.GetUserEntityIds(id, "Affiliate", Guid.Empty).ToList();
            foreach (var affiliateId in affiliateIds)
            {
                var affiliate = _affiliateService.GetAffiliateById(affiliateId, false);
                if (affiliate != null)
                {
                    affiliates.Add(new IdNameModel
                    {
                        Id = affiliate.Id,
                        Name = affiliate.Name
                    });
                }
            }

            return affiliates;
        }

        public List<IdNameModel> GetAffiliateChannelOwnerships(long id)
        {
            var affiliateChannels = new List<IdNameModel>();
            var affiliateChannelIds = _userService.GetUserEntityIds(id, "AffiliateChannel", Guid.Empty).ToList();

            foreach (var affiliateChannelId in affiliateChannelIds)
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId, false);
                if (affiliateChannel != null)
                {
                    affiliateChannels.Add(new IdNameModel
                    {
                        Id = affiliateChannel.Id,
                        Name = affiliateChannel.Name
                    });
                }
            }

            return affiliateChannels;
        }

        public List<IdNameModel> GetBuyerOwnerships(long id)
        {
            var buyers = new List<IdNameModel>();
            var buyerChannelIds = _userService.GetUserEntityIds(id, "Buyer", Guid.Empty).ToList();

            foreach (var buyerChannelId in buyerChannelIds)
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId, false);
                if (buyerChannel != null)
                {
                    buyers.Add(new IdNameModel
                    {
                        Id = buyerChannel.Id,
                        Name = buyerChannel.Name
                    });
                }
            }

            return buyers;
        }

        public List<IdNameModel> GetBuyerChannelOwnerships(long id)
        {
            var buyerChannels = new List<IdNameModel>();
            var buyerChannelIds = _userService.GetUserEntityIds(id, "Buyer", Guid.Empty).ToList();

            foreach (var buyerChannelId in buyerChannelIds)
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId, false);
                if (buyerChannel != null)
                {
                    buyerChannels.Add(new IdNameModel
                    {
                        Id = buyerChannel.Id,
                        Name = buyerChannel.Name
                    });
                }
            }

            return buyerChannels;
        }

        public string GetFullName(User user)
        {
            return user.GetFullName();
        }


        public virtual List<UserAddonsModel> GetAddons(long id)
        {
            var result = new List<UserAddonsModel>();
            var addons = _addonService.GetAddonsByUserId(id).ToList();

            foreach (var addon in addons)
            {
                var addonItem = _addonService.GetAddonById(addon.AddonId);
                if (addonItem != null)
                {
                    result.Add(new UserAddonsModel
                    {
                        Id = addon.AddonId,
                        Name = addonItem.Name,
                        Key = addonItem.Key,
                        AddData = addonItem.AddData,
                        Date = addon.Date,
                        Amount = addon.Amount.HasValue ? addon.Amount.Value : 0,
                        Status = addon.Status.HasValue ? addon.Status.Value : (short)0
                    });
                }
            }

            return result;
        }

        #endregion


    }
}