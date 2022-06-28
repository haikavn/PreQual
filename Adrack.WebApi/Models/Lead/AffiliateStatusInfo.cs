using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateStatusInfo
    {
        public string[] StatusArray { get; internal set; }
        public int TotalStatuses { get; internal set; }
    }
}