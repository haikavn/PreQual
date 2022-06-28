using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateStatusUpdateModel
    {
        public long AffiliateId { get; set; }
        public EntityStatus Status { get; set; }
    }
}