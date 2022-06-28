using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateAdvancedModel
    {
        public Affiliate Affiliate { get; set; }
        public string Manager { get; set; }
    }
}