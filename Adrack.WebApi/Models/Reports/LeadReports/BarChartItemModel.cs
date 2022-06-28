using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.LeadReports
{
    public class BarChartItemModel
    {
            public string Name { get; set; }
            public decimal Value { get; set; }
            public string ChartCategory { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
    }
}