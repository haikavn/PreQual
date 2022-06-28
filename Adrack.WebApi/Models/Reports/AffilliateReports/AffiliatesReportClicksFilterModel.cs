using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.AffilliateReports
{
    public class AffiliatesReportClicksFilterModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<long> AffiliateIds { get; set; }
        public List<long> AffiliateChannelIds { get; set; }
        public bool IsCsv { get; set; }
    }
}