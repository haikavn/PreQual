using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyersReportByPricesFilterModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<long> BuyerChannelIds { get; set; }
        public List<long> CampaignIds { get; set; }
        public int Price1 { get; set; }
        public int Price2 { get; set; }
        public bool IsCsv { get; set; }
    }
}