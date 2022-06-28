using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.PlanManagement;
using Adrack.Service.Lead;
namespace Adrack.Service.Helpers
{
    public class AccessInitializer
    {
        
        public static void InitDefault(AdrackUserFeatureAccessMap map)
        {
            map.AddFeaturesFromService(new AffiliateChannelService(null, null, null, null, null, null));
            map.AddFeaturesFromService(new BuyerChannelService(null, null, null, null, null, null, null, null, null, null, null));
            map.AddFeaturesFromService(new CampaignService(null, null, null, null, null, null, null, null));
            map.AddFeaturesFromService(new AffiliateChannelTemplateService(null, null, null));
            map.AddFeaturesFromService(new BuyerChannelTemplateService(null, null, null));
            map.AddFeaturesFromService(new BlackListService(null, null, null, null, null, null, null));
        }
    }
}
