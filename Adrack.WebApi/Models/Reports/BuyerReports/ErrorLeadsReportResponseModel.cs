using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class ErrorLeadsReportResponseModel
    {
        public long LeadId { get; set; }
        public short? Status { get; set; }
        public DateTime Date { get; set; }
        public string AffiliateChannelName { get; set; }
        public string BuyerChannelName { get; set; }

        public string AffiliateName { get; set; }

        public string BuyerName { get; set; }
        public string CampaignName { get; set; }
        public string State { get; set; }
        public string Response { get; set; }
        public short? ErrorType { get; set; }
        public decimal MinPrice { get; set; }
    }
}