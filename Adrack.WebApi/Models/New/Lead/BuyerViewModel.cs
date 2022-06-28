using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class BuyerViewModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public long? StateProvinceId { get; set; }
        public long? ManagerId { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public short Status { get; set; }
        public string BillFrequency { get; set; }
        public int? FrequencyValue { get; set; }
        public DateTime? LastPostedSold { get; set; }
        public DateTime? LastPosted { get; set; }
        public short TypeId { get; set; }
        public short MaxDuplicateDays { get; set; }
        public int DailyCap { get; set; }
        public string Description { get; set; }
        public long? ExternalId { get; set; }
        public bool? Deleted { get; set; }
        public bool? IsBiWeekly { get; set; }
        public bool? CoolOffEnabled { get; set; }
        public DateTime? CoolOffStart { get; set; }
        public DateTime? CoolOffEnd { get; set; }
        public short? DoNotPresentStatus { get; set; }
        public string DoNotPresentUrl { get; set; }
        public string DoNotPresentResultField { get; set; }
        public string DoNotPresentResultValue { get; set; }
        public string DoNotPresentRequest { get; set; }
        public string DoNotPresentPostMethod { get; set; }
        public bool? CanSendLeadId { get; set; }
        public int? AccountId { get; set; }

    }
}