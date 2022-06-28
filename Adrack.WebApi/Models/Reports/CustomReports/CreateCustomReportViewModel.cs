using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class CreateCustomReportViewModel
    {
        public CustomReportSettingModel SettingModel { get; set; }
        public ReportTypeModel ReportTypeModel { get; set; }
    }
}