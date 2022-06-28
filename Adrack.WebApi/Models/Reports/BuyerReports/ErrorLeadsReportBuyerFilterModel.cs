using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class ErrorLeadsReportBuyerFilterModel
    {
        public long LeadId { get; set; }
        public short StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<long> AffiliateChannelIds { get; set; }
        public List<long> BuyerChannelIds { get; set; }
        public List<long> CampaignIds { get; set; }
        public List<string> States { get; set; }
        public string Description { get; set; }

        public short ErrorTypeId { get; set; }

        public bool IsCsv { get; set; }


        public int Page { get; set; }

        public int Count { get; set; }
    }
}