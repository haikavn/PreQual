using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateResponseModel
    {
        public string AffiliateName { get; internal set; }
        public string AffiliateChannelName { get; internal set; }
        public long Id { get; internal set; }
        public DateTime Created { get; internal set; }
        public long AffiliateId { get; internal set; }
        public long AffiliateChannelId { get; internal set; }
        public string Response { get; internal set; }
        public decimal MinPrice { get; internal set; }
    }
}