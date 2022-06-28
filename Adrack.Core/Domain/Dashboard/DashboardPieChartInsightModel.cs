using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.Core.Domain.Dashboard
{
    public class DashboardPieChartInsightModel
    {
        public long Counts { get; set; }
        public string State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}