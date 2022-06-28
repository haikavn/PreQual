using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class PingTreeItemModel
    {
        public long Id { get; set; }
        public long BuyerChannelId { get; set; }

        public int OrderNum { get; set; }

        public int GroupNum { get; set; }

        public int Percent { get; set; }

        public bool IsLocked { get; set; }

        public EntityStatus Status { get; set; }

        public static explicit operator Core.Domain.Lead.PingTreeItem(PingTreeItemModel pingTreeItemModel)
        {
            return new Core.Domain.Lead.PingTreeItem
            {
                Id = pingTreeItemModel.Id,
                BuyerChannelId = pingTreeItemModel.BuyerChannelId,
                OrderNum = pingTreeItemModel.OrderNum,
                IsLocked = pingTreeItemModel.IsLocked,
                GroupNum = pingTreeItemModel.GroupNum,
                Percent = pingTreeItemModel.Percent,
                Status = pingTreeItemModel.Status,
                PingTreeId = 0
            };
        }
    }
}