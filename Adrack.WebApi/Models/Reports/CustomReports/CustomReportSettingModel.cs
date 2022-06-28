using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class CustomReportSettingModel
    {
        public long SettingId { get; set; }
        public long ReportTypeId { get; set; }
        public ReportEntityType ReportEntityType1 { get; set; }
        public List<long> ReportEntityIds1 { get; set; }

        public ReportEntityType? ReportEntityType2 { get; set; }
        public List<long> ReportEntityIds2 { get; set; }

        public ReportPeriodType? ReportPeriodType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool? IsAscending { get; set; }

        public int? OrderVariableId { get; set; }
    }
}