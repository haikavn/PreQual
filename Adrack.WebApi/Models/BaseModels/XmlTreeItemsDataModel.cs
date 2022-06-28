using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    public class XmlTreeItemsDataModel
    {
        public XmlTreeItemsDataModel()
        {
            TreeItems = new List<TreeItem>();
            MinimumRevenue = 0M;
            TargetRevenue = 0M;
            Xml = string.Empty;
        }
        public List<TreeItem> TreeItems { get; internal set; }
        public string Xml { get; set; }
        public CampaignTypes CampaignType { get; internal set; }
        public decimal MinimumRevenue { get; internal set; }
        public decimal TargetRevenue { get; internal set; }
    }
}