using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyersReportByHourFilterModel
    {
        public DateTime ReportDate1 { get; set; }
        public DateTime ReportDate2 { get; set; }
        public DateTime ReportDate3 { get; set; }
        public List<long> BuyerChannelIds { get; set; }
        public List<long> CampaignIds { get; set; }
        public bool IsCsv { get; set; }
    }
}