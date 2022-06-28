using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.NetworkReports
{
    public class ReportBuyersComparisonFilterModel
    {
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public DateTime Date3 { get; set; }


        public short ShowBy { get; set; }

        public List<long> Ids { get; set; }

        public List<long> CampaignIds { get; set; }

        public bool IsCsv { get; set; }
    }
}