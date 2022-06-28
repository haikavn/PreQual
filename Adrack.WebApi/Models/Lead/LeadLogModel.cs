using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadLogModel
    {
        public string BuyerName { get; set; }
        public long BuyerId { get; set; }
        public string BuyerChannelName { get; set; }
        public long BuyerChannelId { get; set; }
        public string PostedData { get; set; }
        public DateTime PostedDate { get; set; }
        public string Request { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Response { get; set; }
        public double? ResponseTime { get; set; } 
        public decimal? MinPrice { get; set; }
        public short Status { get; set; }
        public string BuyerIconPath { get; set; }
    }
}