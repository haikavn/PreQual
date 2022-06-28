using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateFilterModel
    {
        public string Name { get; set; }
        public EntityFilterByStatus ActivityStatus { get; set; }
    }
}