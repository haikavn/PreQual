
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core.Domain.Membership;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface IUsersExtensionService
    {
         string GetFullName(User user);
        List<IdNameModel> GetCampaignOwnerships(long id);
        List<IdNameModel> GetAffiliateOwnerships(long id);
        List<IdNameModel> GetAffiliateChannelOwnerships(long id);
        List<IdNameModel> GetBuyerOwnerships(long id);
        List<IdNameModel> GetBuyerChannelOwnerships(long id);
        List<UserAddonsModel> GetAddons(long id);
    }
}
