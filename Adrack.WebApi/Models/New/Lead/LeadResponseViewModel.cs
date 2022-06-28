using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class LeadResponseViewModel
    {
        public long Id { get; set; }
        public long BuyerId { get; set; }
        public long BuyerChannelId { get; set; }
        public string Response { get; set; }
        public double? ResponseTime { get; set; }
        public short Status { get; set; }
        public long LeadId { get; set; }
        public long AffiliateChannelId { get; set; }
        public long AffiliateId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerChanelName { get; set; }
        public decimal? MinPrice { get; set; }
        public string Posted { get; set; }
        public DateTime Created { get; set; }
        public DateTime ResponseCreated { get; set; }
    }
}