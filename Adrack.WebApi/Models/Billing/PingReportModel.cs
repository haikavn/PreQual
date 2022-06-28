using System;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Billing
{
    public class PingReportModel : IBaseInModel
    {
        public DateTime startDate { get; set; }
        
        public DateTime endDate { get; set; }

        public long buyerId { get; set; }

        public long affiliateId { get; set; }

        public long campaignId { get; set; }


    }
}