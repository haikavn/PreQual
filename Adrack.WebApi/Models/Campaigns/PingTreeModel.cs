using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class PingTreeModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }

        public long CampaignId { get; set; }

        public List<PingTreeItemModel> Items { get; set; }

        public static explicit operator Core.Domain.Lead.PingTree(PingTreeModel pingTreeModel)
        {
            return new Core.Domain.Lead.PingTree
            {
                Id = pingTreeModel.Id,
                CampaignId = pingTreeModel.CampaignId,
                Name = pingTreeModel.Name,
                Quantity = pingTreeModel.Quantity
            };
        }
    }
}