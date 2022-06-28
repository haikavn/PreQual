using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadsErrorReportPagedModel : BasePagedModel
    {
        public string TimeZoneNowStr { get; internal set; }
        public List<LeadsErrorReportModel> LeadsErrorReports { get; internal set; }
    }
}