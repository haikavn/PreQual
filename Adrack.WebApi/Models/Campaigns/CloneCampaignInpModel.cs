using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CloneCampaignInpModel 
    {
        #region fields

        public long CurrentCampaignId { get; set; }
        public string NewCampaignName { get; set; }

        #endregion
    }
}