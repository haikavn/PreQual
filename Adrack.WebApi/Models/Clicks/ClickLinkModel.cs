using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Clicks
{
    public class ClickLinkModel
    {
        public decimal ClickPrice { get; set; }

        public string RedirectUrl { get; set; }
    }
}