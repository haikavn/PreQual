using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class ReportCloneModel
    {
        public CustomReportSettingModel SettingModel { get; set; }
        public long ReportId { get; set; }

        public string Name { get; set; }
    }
}