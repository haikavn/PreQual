using Adrack.Core;
using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class LeadContentDuplicateViewModel
    {
        public long Id { get; set; }
        public long LeadId { get; set; }
        public DateTime Created { get; set; }
        public string Ip { get; set; }
        public decimal? MinPrice { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string BankPhone { get; set; }
        public string Email { get; set; }
        public string PayFrequency { get; set; }
        public string DirectDeposit { get; set; }
        public string AccountType { get; set; }
        public string IncomeType { get; set; }
        public decimal? NetMonthlyIncome { get; set; }
        public short? EmpTime { get; set; }
        public short? AddressMonth { get; set; }
        public DateTime? Dob { get; set; }
        public short? Age { get; set; }
        public decimal? RequestedAmount { get; set; }
        public string Ssn { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        public long? AffiliateId { get; set; }
        public CampaignTypes? CampaignType { get; set; }
        public long? OriginalLeadId { get; set; }
        public string AffiliateName { get; set; }

    }
}