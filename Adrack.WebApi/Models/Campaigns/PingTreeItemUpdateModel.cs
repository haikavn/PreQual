using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class PingTreeItemUpdateModel
    {
        public long PingTreeItemId { get; set; }
        public int OrderNum { get; set; }

        public int GroupNum { get; set; }

        public int Percent { get; set; }
        
        public EntityStatus? Status { get; set; }

        public bool IsLocked { get; set; }
    }
}