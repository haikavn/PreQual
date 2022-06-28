using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.CustomReports
{
    public class UserReportModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public  bool IsOwner{ get; set; }

        public string Owner { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}