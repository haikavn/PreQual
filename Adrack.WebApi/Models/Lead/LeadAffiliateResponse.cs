using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadAffiliateResponse
    {
        public decimal MinPrice { get; set; }
        public DateTime ProcessStarted { get; set; }
        public DateTime ResponseSent { get; set; }
        public decimal ProcessingTime { get; set; }
        public string Response { get; set; }
    }
}