using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class UpdateCustomReportCommentModel
    {
        public int ReportId{ get; set; }
        public string Comment{ get; set; }
    }
}