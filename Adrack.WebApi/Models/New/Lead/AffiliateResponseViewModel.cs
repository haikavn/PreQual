using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class AffiliateResponseViewModel
    {
        public long Id { get; set; }
        public long? LeadId { get; set; }
        public long AffiliateId { get; set; }
        public long AffiliateChannelId { get; set; }
        public DateTime Created { get; set; }
        public string Response { get; set; }
        public decimal MinPrice { get; set; }
        public DateTime ProcessStartedAt { get; set; }
        public string Message { get; set; }
        public short? Status { get; set; }
        public short? ErrorType { get; set; }
        public short? Validator { get; set; }
        public string ReceivedData { get; set; }
        public string State { get; set; }
    }
}