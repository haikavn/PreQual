using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadDuplicateMonitorModel
    {
        public long LeadId { get; set; }
        public DateTime? Created { get; set; }
        public long? AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public decimal? RequestedAmount { get; set; }
        public decimal? NetMonthlyIncome { get; set; }
        public string PayFrequency { get; set; }
        public string DirectDeposit { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string Ip { get; set; }
    }
}