using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.NetworkReports
{
    public class ReportBuyersByTrafficEstimatorModel
    {
        public string Title { get; set; }

        public long BuyerId { get; set; }

        public string BuyerName { get; set; }

        public long BuyerChannelId { get; set; }

        public string BuyerChannelName { get; set; }

        public int Quantity { get; set; }

        public int UQuantity { get; set; }

        public DateTime Created { get; set; }

        public ReportBuyersByTrafficEstimatorModel()
        {
        }
    }
}