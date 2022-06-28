using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.LeadReports
{
    public class DashboardLeadsReportFilterModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<long> CampaignIds { get; set; }
    }
}