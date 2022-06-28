using System.ComponentModel.DataAnnotations;
using Adrack.Core;
using Adrack.Web.Framework;

namespace Adrack.WebApi.Models.New.Membership
{
    public class RegisterModel
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public bool FirstNameEnabled { get; set; }
        public bool FirstNameRequired { get; set; }

        [AppLocalizedStringDisplayName("Profile.Field.FirstName")]
        public string FirstName { get; set; }
        public bool MiddleNameEnabled { get; set; }
        public bool MiddleNameRequired { get; set; }

        [AppLocalizedStringDisplayName("Profile.Field.MiddleName")]
        public string MiddleName { get; set; }
        public bool LastNameEnabled { get; set; }
        public bool LastNameRequired { get; set; }

        [AppLocalizedStringDisplayName("Profile.Field.LastName")]
        public string LastName { get; set; }
        public bool SummaryEnabled { get; set; }
        public bool SummaryRequired { get; set; }

        [AppLocalizedStringDisplayName("Profile.Field.Summary")]
        public string Summary { get; set; }
        public bool CountryRequired { get; set; }
        public bool CountryEnabled { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.Country")]
        public long CountryId { get; set; }
        public bool StateProvinceEnabled { get; set; }
        public bool StateProvinceRequired { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.StateProvince")]
        public long StateProvinceId { get; set; }
        public bool AddressLine1Enabled { get; set; }
        public bool AddressLine1Required { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.AddressLine1")]
        public string AddressLine1 { get; set; }
        public bool AddressLine2Enabled { get; set; }
        public bool AddressLine2Required { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.AddressLine2")]
        public string AddressLine2 { get; set; }
        public bool CityEnabled { get; set; }
        public bool CityRequired { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.City")]
        public string City { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }
        public bool ZipPostalCodeRequired { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.ZipPostalCode")]
        public string ZipPostalCode { get; set; }
        public bool TelephoneEnabled { get; set; }
        public bool TelephoneRequired { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.Telephone")]
        public string Phone { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.Telephone")]
        public string CompanyPhone { get; set; }

        [AppLocalizedStringDisplayName("Address.Field.CompanyEmail")]
        public string CompanyEmail { get; set; }

        public bool UsernameEnabled { get; set; }

        [AppLocalizedStringDisplayName("Membership.Field.Username")]
        public string Username { get; set; }
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [AppLocalizedStringDisplayName("Membership.Field.Email")]
        public string Email { get; set; }
        public bool IsMaskEmail { get; set; }
        public string ContactEmail { get; set; }
        public UserTypes UserType { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public bool EmailSubscriptionEnabled { get; set; }

        [AppLocalizedStringDisplayName("Membership.Field.EmailSubscription")]
        public bool EmailSubscription { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string Name { get; set; }
        public long UserRoleId { get; set; }
        public long ParentId { get; set; }
        public string Comment { get; set; }
        public string CellPhone { get; set; }
        public Core.Domain.Membership.User LoggedInUser { get; set; }
        public string Website { get; set; }
        public bool ValidateFromCode { get; set; }
        public string ValidationEmail { get; set; }
        public string TimeZone { get; set; }
        public long? ManagerId { get; set; }
        public string WhiteIp { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsChangePassOnLogin { get; set; }
        public decimal Credit { get; set; }
        public short AlwaysSoldOption { get; set; }
        public short MaxDuplicateDays { get; set; }
        public int DailyCap { get; set; }
        public string Description { get; set; }
        public string DoNotPresentResultField { get; set; }
        public string DoNotPresentResultValue { get; set; }
        public short? DoNotPresentStatus { get; set; }
        public string DoNotPresentPostMethod { get; set; }
        public string DoNotPresentRequest { get; set; }
        public string DoNotPresentUrl { get; set; }
        public bool? CanSendLeadId { get; set; }
        public int? AccountId { get; set; }
    }
}