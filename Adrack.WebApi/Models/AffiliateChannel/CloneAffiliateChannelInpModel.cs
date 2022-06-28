using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CloneAffiliateChannelInpModel
    {
        #region fields

        public long CurrentAffiliateChannelId { get; set; }
        public string NewAffiliateChannelName { get; set; }

        #endregion
    }
}