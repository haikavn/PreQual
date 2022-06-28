using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignListModel : BaseIdentifiedItem
    {
        #region Properties
        public string CampaignName { get; set; }
        public long CampaignId { get; set; }
        public string Vertical { get; set; }
        public long VerticalId { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit{ get; set; }
        public ActivityStatuses Status{ get; set; }
        #endregion Properties


    }
}