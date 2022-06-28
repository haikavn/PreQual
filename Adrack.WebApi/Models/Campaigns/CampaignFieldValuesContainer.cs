using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignFieldValuesContainer : BaseModel
    {
        public List<SelectItem> ListPingTreeCycle { get; internal set; }
        public List<SelectItem> ListStatus { get; internal set; }
        public List<SelectItem> ListVisibility { get; internal set; }
        public List<SelectItem> ListSystemField { get; internal set; }
        public List<SelectItem> ListDataType { get; internal set; }
        public List<SelectItem> ListCampaignType { get; internal set; }
        public List<SelectItem> ListVertical { get; internal set; }
    }
}