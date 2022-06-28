using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class ShareCustomReportModel
    {
        public List<long> UserIds { get; set; } = new List<long>();
        public long ReportId { get; set; }
    }
}