using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    [Serializable]
    public class ReportRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}