using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyerReportStatementFilterModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long BuyerId { get; set; }
    }
}