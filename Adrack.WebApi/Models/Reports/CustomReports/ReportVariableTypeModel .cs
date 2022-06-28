using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class ReportVariableTypeModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SQLQuery { get; set; }
        public string Comment { get; set; }
    }
}