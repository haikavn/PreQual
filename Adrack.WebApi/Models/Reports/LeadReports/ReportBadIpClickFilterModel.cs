using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.LeadReports
{
    public class ReportBadIpClickFilterModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long LeadId { get; set; }
        public List<long> AffiliateIds { get; set; }
        public string LeadIp { get; set; }
        public string ClickIp { get; set; }

        public bool IsCsv { get; set; }
    }
}