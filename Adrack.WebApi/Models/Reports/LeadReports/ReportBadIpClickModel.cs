using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.LeadReports
{
    public class ReportBadIpClickModel
    {
        public long LeadId { get; set; }
        public DateTime Date { get; set; }
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public string LeadIp { get; set; }
        public string ClickIp { get; set; }
    }
}