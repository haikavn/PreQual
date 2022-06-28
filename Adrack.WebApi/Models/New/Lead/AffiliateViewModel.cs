using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class AffiliateViewModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public long? StateProvinceId { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ManagerId { get; set; }
        public short Status { get; set; }
        public string BillFrequency { get; set; }
        public int? FrequencyValue { get; set; }
        public int? BillWithin { get; set; }
        public string RegistrationIp { get; set; }
        public string Website { get; set; }
        public bool? Deleted { get; set; }
        public bool? IsBiWeekly { get; set; }
        public string WhiteIp { get; set; }
        public short? DefaultAffiliatePriceMethod { get; set; }
        public decimal? DefaultAffiliatePrice { get; set; }
    }
}