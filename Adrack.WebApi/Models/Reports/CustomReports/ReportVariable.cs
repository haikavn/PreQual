using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class ReportVariableModel
    {
        public long Id { get; set; }
        public bool isAttached { get; set; }
        public long ReportTypeId { get; set; }
        public long ReportVariableTypeId { get; set; }
    }
}