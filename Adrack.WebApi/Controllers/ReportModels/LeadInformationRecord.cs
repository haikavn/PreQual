using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    [Serializable]
    public class LeadInformationRecord
    {
        public string Label;
        public DateTime DateStart;
        public DateTime DateEnd;
        public double Recieved;
        public double Posted;
        public double Pings;

        public double Sold;
        public double Revenue;
        public double Cost;
        public double Profit;
    }
}