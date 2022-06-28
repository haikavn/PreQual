using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class CustomReportModel:ReportModelBase
    {
        public long ReportTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<long> BuyerIds { get; set; } = new List<long>();
        public List<long> AffiliateIds { get; set; } = new List<long>();
        public List<long> BuyerChannelIds { get; set; } = new List<long>();
        public List<long> AffiliateChannelIds { get; set; } = new List<long>();
        public List<long> CampaignIds { get; set; } = new List<long>();
    }
}