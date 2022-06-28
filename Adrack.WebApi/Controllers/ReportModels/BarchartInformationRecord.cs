using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    public class BarchartInformationRecord
    {
        public string label;
        public DateTime dateStart;
        public DateTime dateEnd;

        public string color { get; set; }
        public string type { get; set; }

        public List<BarchartItem> values { get; set; } = new List<BarchartItem>();
    }

    public class BarchartItem
    {
        public string name { get; set; }
        public decimal value { get; set; }
    }
}