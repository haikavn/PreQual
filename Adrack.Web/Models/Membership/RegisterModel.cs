// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RegisterModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using Adrack.Web.Validators.Membership;
using FluentValidation.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.Web.Models.Membership
{
    /// <summary>
    ///     Represents a Register Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    [Validator(typeof(RegisterValidator))]
    public class RegisterModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        ///     Register Model
        /// </summary>
        public RegisterModel()
        {
            ListCountry = new List<SelectListItem>();
            ListStateProvince = new List<SelectListItem>();
            ListUserType = new List<SelectListItem>();
            ListUserRole = new List<SelectListItem>();
            ListDepartment = new List<SelectListItem>();
            TimeZones = new List<SelectListItem>();

            ListStateProvince.Add(new SelectListItem
            {
                Text = "",
                Value = "0",
                Selected = false
            });
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets the profile identifier.
        /// </summary>
        /// <value>The profile identifier.</value>
        public long ProfileId { get; set; }

        /// <summary>
        ///     Gets or Sets the First Name Enabled
        /// </summary>
        /// <value><c>true</c> if [first name enabled]; otherwise, <c>false</c>.</value>
        public bool FirstNameEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the First Name Required
        /// </summary>
        /// <value><c>true</c> if [first name required]; otherwise, <c>false</c>.</value>
        public bool FirstNameRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the First Name
        /// </summary>
        /// <value>The first name.</value>
        [AppLocalizedStringDisplayName("Profile.Field.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or Sets the Middle Name Enabled
        /// </summary>
        /// <value><c>true</c> if [middle name enabled]; otherwise, <c>false</c>.</value>
        public bool MiddleNameEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Middle Name Required
        /// </summary>
        /// <value><c>true</c> if [middle name required]; otherwise, <c>false</c>.</value>
        public bool MiddleNameRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Middle Name
        /// </summary>
        /// <value>The name of the middle.</value>
        [AppLocalizedStringDisplayName("Profile.Field.MiddleName")]
        [AllowHtml]
        public string MiddleName { get; set; }

        /// <summary>
        ///     Gets or Sets the Last Name Enabled
        /// </summary>
        /// <value><c>true</c> if [last name enabled]; otherwise, <c>false</c>.</value>
        public bool LastNameEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Last Name Required
        /// </summary>
        /// <value><c>true</c> if [last name required]; otherwise, <c>false</c>.</value>
        public bool LastNameRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Last Name
        /// </summary>
        /// <value>The last name.</value>
        [AppLocalizedStringDisplayName("Profile.Field.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        /// <summary>
        ///     Gets or Sets the Summary Enabled
        /// </summary>
        /// <value><c>true</c> if [summary enabled]; otherwise, <c>false</c>.</value>
        public bool SummaryEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Summary Required
        /// </summary>
        /// <value><c>true</c> if [summary required]; otherwise, <c>false</c>.</value>
        public bool SummaryRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Summary
        /// </summary>
        /// <value>The summary.</value>
        [AppLocalizedStringDisplayName("Profile.Field.Summary")]
        [AllowHtml]
        public string Summary { get; set; }

        /// <summary>
        ///     Gets or Sets the Country Enabled
        /// </summary>
        /// <value><c>true</c> if [country enabled]; otherwise, <c>false</c>.</value>
        public bool CountryEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Country Required
        /// </summary>
        /// <value><c>true</c> if [country required]; otherwise, <c>false</c>.</value>
        public bool CountryRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The country identifier.</value>
        [AppLocalizedStringDisplayName("Address.Field.Country")]
        [AllowHtml]
        public long CountryId { get; set; }

        /// <summary>
        ///     Gets or Sets the Country
        /// </summary>
        /// <value>The list country.</value>
        public IList<SelectListItem> ListCountry { get; set; }

        /// <summary>
        ///     Gets or Sets the State Province Enabled
        /// </summary>
        /// <value><c>true</c> if [state province enabled]; otherwise, <c>false</c>.</value>
        public bool StateProvinceEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the State Province Required
        /// </summary>
        /// <value><c>true</c> if [state province required]; otherwise, <c>false</c>.</value>
        public bool StateProvinceRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the State Province Identifier
        /// </summary>
        /// <value>The state province identifier.</value>
        [AppLocalizedStringDisplayName("Address.Field.StateProvince")]
        [AllowHtml]
        public long StateProvinceId { get; set; }

        /// <summary>
        ///     Gets or Sets the State Province
        /// </summary>
        /// <value>The list state province.</value>
        public IList<SelectListItem> ListStateProvince { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 1 Enabled
        /// </summary>
        /// <value><c>true</c> if [address line1 enabled]; otherwise, <c>false</c>.</value>
        public bool AddressLine1Enabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 1 Required
        /// </summary>
        /// <value><c>true</c> if [address line1 required]; otherwise, <c>false</c>.</value>
        public bool AddressLine1Required { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 1
        /// </summary>
        /// <value>The address line1.</value>
        [AppLocalizedStringDisplayName("Address.Field.AddressLine1")]
        [AllowHtml]
        public string AddressLine1 { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 2 Enabled
        /// </summary>
        /// <value><c>true</c> if [address line2 enabled]; otherwise, <c>false</c>.</value>
        public bool AddressLine2Enabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 2 Required
        /// </summary>
        /// <value><c>true</c> if [address line2 required]; otherwise, <c>false</c>.</value>
        public bool AddressLine2Required { get; set; }

        /// <summary>
        ///     Gets or Sets the Address Line 2
        /// </summary>
        /// <value>The address line2.</value>
        [AppLocalizedStringDisplayName("Address.Field.AddressLine2")]
        [AllowHtml]
        public string AddressLine2 { get; set; }

        /// <summary>
        ///     Gets or Sets the City Enabled
        /// </summary>
        /// <value><c>true</c> if [city enabled]; otherwise, <c>false</c>.</value>
        public bool CityEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the City Required
        /// </summary>
        /// <value><c>true</c> if [city required]; otherwise, <c>false</c>.</value>
        public bool CityRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the City
        /// </summary>
        /// <value>The city.</value>
        [AppLocalizedStringDisplayName("Address.Field.City")]
        [AllowHtml]
        public string City { get; set; }

        /// <summary>
        ///     Gets or Sets the Zip Postal Code Enabled
        /// </summary>
        /// <value><c>true</c> if [zip postal code enabled]; otherwise, <c>false</c>.</value>
        public bool ZipPostalCodeEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Zip Postal Code Required
        /// </summary>
        /// <value><c>true</c> if [zip postal code required]; otherwise, <c>false</c>.</value>
        public bool ZipPostalCodeRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Zip Postal Code
        /// </summary>
        /// <value>The zip postal code.</value>
        [AppLocalizedStringDisplayName("Address.Field.ZipPostalCode")]
        [AllowHtml]
        public string ZipPostalCode { get; set; }

        /// <summary>
        ///     Gets or Sets the Telephone Enabled
        /// </summary>
        /// <value><c>true</c> if [telephone enabled]; otherwise, <c>false</c>.</value>
        public bool TelephoneEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Telephone Required
        /// </summary>
        /// <value><c>true</c> if [telephone required]; otherwise, <c>false</c>.</value>
        public bool TelephoneRequired { get; set; }

        /// <summary>
        ///     Gets or Sets the Telephone
        /// </summary>
        /// <value>The phone.</value>
        [AppLocalizedStringDisplayName("Address.Field.Telephone")]
        [AllowHtml]
        public string Phone { get; set; }

        /// <summary>
        ///     Gets or sets the company phone.
        /// </summary>
        /// <value>The company phone.</value>
        [AppLocalizedStringDisplayName("Address.Field.Telephone")]
        [AllowHtml]
        public string CompanyPhone { get; set; }

        /// <summary>
        ///     Gets or Sets the Telephone
        /// </summary>
        /// <value>The company email.</value>
        [AppLocalizedStringDisplayName("Address.Field.CompanyEmail")]
        [AllowHtml]
        public string CompanyEmail { get; set; }

        /// <summary>
        ///     Gets or Sets the Username Enabled
        /// </summary>
        /// <value><c>true</c> if [username enabled]; otherwise, <c>false</c>.</value>
        public bool UsernameEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Username
        /// </summary>
        /// <value>The username.</value>
        [AppLocalizedStringDisplayName("Membership.Field.Username")]
        [AllowHtml]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or Sets the Check Username Availability Enabled
        /// </summary>
        /// <value><c>true</c> if [check username availability enabled]; otherwise, <c>false</c>.</value>
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        [AppLocalizedStringDisplayName("Membership.Field.Email")]
        [AllowHtml]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or Sets the contact Email
        /// </summary>
        /// <value>The contact email.</value>
        [AllowHtml]
        public string ContactEmail { get; set; }

        /// <summary>
        ///     Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        [AllowHtml]
        public string Password { get; set; }

        /// <summary>
        ///     Gets or Sets the Confirm Password
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ConfirmPassword")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        /// <summary>
        ///     Gets or Sets the Email Subscription Enabled
        /// </summary>
        /// <value><c>true</c> if [email subscription enabled]; otherwise, <c>false</c>.</value>
        public bool EmailSubscriptionEnabled { get; set; }

        /// <summary>
        ///     Gets or Sets the Email Subscription
        /// </summary>
        /// <value><c>true</c> if [email subscription]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("Membership.Field.EmailSubscription")]
        public bool EmailSubscription { get; set; }

        /// <summary>
        ///     Gets or Sets the Display Captcha
        /// </summary>
        /// <value><c>true</c> if [display captcha]; otherwise, <c>false</c>.</value>
        public bool DisplayCaptcha { get; set; }

        /// <summary>
        ///     Gets or sets the security question.
        /// </summary>
        /// <value>The security question.</value>
        public string SecurityQuestion { get; set; }

        /// <summary>
        ///     Gets or sets the security answer.
        /// </summary>
        /// <value>The security answer.</value>
        public string SecurityAnswer { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the user type identifier.
        /// </summary>
        /// <value>The user type identifier.</value>
        public UserTypes UserType { get; set; }

        /// <summary>
        ///     Gets or sets the type of the list user.
        /// </summary>
        /// <value>The type of the list user.</value>
        public IList<SelectListItem> ListUserType { get; set; }

        /// <summary>
        ///     Gets or sets the list user role.
        /// </summary>
        /// <value>The list user role.</value>
        public IList<SelectListItem> ListUserRole { get; set; }

        /// <summary>
        ///     Gets or sets the list department.
        /// </summary>
        /// <value>The list department.</value>
        public IList<SelectListItem> ListDepartment { get; set; }

        /// <summary>
        ///     Gets or sets the user role identifier.
        /// </summary>
        /// <value>The user role identifier.</value>
        public long UserRoleId { get; set; }

        /// <summary>
        ///     Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the department identifier.
        /// </summary>
        /// <value>The department identifier.</value>
        public long DepartmentId { get; set; }

        /// <summary>
        ///     Gets or sets the cell phone.
        /// </summary>
        /// <value>The cell phone.</value>
        public string CellPhone { get; set; }

        /// <summary>
        ///     Gets or sets the logged in user.
        /// </summary>
        /// <value>The logged in user.</value>
        public User LoggedInUser { get; set; }

        /// <summary>
        ///     Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public string Website { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [validate from code].
        /// </summary>
        /// <value><c>true</c> if [validate from code]; otherwise, <c>false</c>.</value>
        public bool ValidateFromCode { get; set; }

        /// <summary>
        ///     Gets or sets the validation code.
        /// </summary>
        /// <value>The validation code.</value>
        public string ValidationCode { get; set; }

        /// <summary>
        ///     Gets or sets the validation email.
        /// </summary>
        /// <value>The validation email.</value>
        public string ValidationEmail { get; set; }

        /// <summary>
        ///     Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        ///     Gets or sets the time zones.
        /// </summary>
        /// <value>The time zones.</value>
        public List<SelectListItem> TimeZones { get; set; }

        #endregion Properties
    }
}