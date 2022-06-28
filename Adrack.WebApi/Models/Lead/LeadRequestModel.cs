using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadRequestModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public short Status { get; set; }
        public List<long> FilterAffiliateIds { get; set; }
        public List<long> FilterAffiliateChannelIds { get; set; }
        public List<long> FilterBuyerIds { get; set; }
        public List<long> FilterBuyerChannelIds { get; set; }
        public List<long> FilterCampaignIds { get; set; }

        public string SubId { get; set; }

        public int? LeadId { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }
    }
}