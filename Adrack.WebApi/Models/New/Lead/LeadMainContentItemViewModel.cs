using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class LeadMainContentItemViewModel
    {
        public long Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long CampaignId { get; set; }
        public string CampaignName { get; set; }
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public short Status { get; set; }
        public long AffiliateChannelId { get; set; }
        public string AffiliateChannelName { get; set; }
        public short CampaignType { get; set; }
        public long LeadNumber { get; set; }
        public short Warning { get; set; }
        public string AffiliateSubId { get; set; }
        public double? ProcessingTime { get; set; }
        public long DuplicateLeadId { get; set; }
        public string ReceivedData { get; set; }
        public short? ErrorType { get; set; }
        public long LeadId { get; set; }
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
        public string MinPriceStr { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        public long? BuyerId { get; set; }
        public string BuyerName { get; set; }
        public long? BuyerChannelId { get; set; }
        public string BuyerChannelName { get; set; }
        public bool? Clicked { get; set; }
        public string ClickIp { get; set; }
        public decimal? AffiliatePrice { get; set; }
        public decimal? BuyerPrice { get; set; }
        public int? RiskScore { get; set; }
    }
}