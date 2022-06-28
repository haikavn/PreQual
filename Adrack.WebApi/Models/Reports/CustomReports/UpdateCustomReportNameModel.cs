using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class UpdateCustomReportNameModel
    {
        public int ReportId{ get; set; }
        [Required]
        public string Name{ get; set; }
    }
}