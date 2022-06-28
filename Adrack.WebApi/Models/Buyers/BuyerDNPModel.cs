using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerDNPModel
    {
        public short? DoNotPresentStatus { get; set; }
        public string DoNotPresentUrl { get; set; }
        public string DoNotPresentResultField { get; set; }
        public string DoNotPresentResultValue { get; set; }
        public string DoNotPresentRequest { get; set; }
        public string DoNotPresentPostMethod { get; set; }
    }
}