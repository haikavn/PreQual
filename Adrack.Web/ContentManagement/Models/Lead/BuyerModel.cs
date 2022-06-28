// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class BuyerModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class BuyerModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public BuyerModel()
        {
            this.ListCountry = new List<SelectListItem>();
            this.ListStateProvince = new List<SelectListItem>();
            this.ListUser = new List<SelectListItem>();
            this.ListStatus = new List<SelectListItem>();
            this.ListUserRole = new List<SelectListItem>();
            this.ListAlwaysSoldOption = new List<SelectListItem>();
            this.ListDoNotPresentStatus = new List<SelectListItem>();
            this.ListDoNotPresentPostMethod = new List<SelectListItem>();
            this.ListAffiliates = new List<SelectListItem>();
            this.ListAffiliateChannels = new List<SelectListItem>();
            CoolOffEnabled = false;
            CoolOffStart = DateTime.UtcNow;
            CoolOffEnd = DateTime.UtcNow;

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
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        /// <value>The country identifier.</value>
        public long CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state province identifier.
        /// </summary>
        /// <value>The state province identifier.</value>
        public long StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public long ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the list country.
        /// </summary>
        /// <value>The list country.</value>
        public IList<SelectListItem> ListCountry { get; set; }

        /// <summary>
        /// Gets or sets the list state province.
        /// </summary>
        /// <value>The list state province.</value>
        public IList<SelectListItem> ListStateProvince { get; set; }

        /// <summary>
        /// Gets or sets the list user.
        /// </summary>
        /// <value>The list user.</value>
        public IList<SelectListItem> ListUser { get; set; }

        /// <summary>
        /// Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectListItem> ListStatus { get; set; }

        /// <summary>
        /// Gets or sets the list user role.
        /// </summary>
        /// <value>The list user role.</value>
        public IList<SelectListItem> ListUserRole { get; set; }

        /// <summary>
        /// Gets or sets the list always sold option.
        /// </summary>
        /// <value>The list always sold option.</value>
        public IList<SelectListItem> ListAlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the list of do not present status.
        /// </summary>
        /// <value>The list of do not present status.</value>
        public IList<SelectListItem> ListDoNotPresentStatus { get; set; }

        public IList<SelectListItem> ListDoNotPresentPostMethod{ get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [AppLocalizedStringDisplayName("Profile.Field.AddressLine1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [AppLocalizedStringDisplayName("Profile.Field.AddressLine2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the zip postal code.
        /// </summary>
        /// <value>The zip postal code.</value>
        [AppLocalizedStringDisplayName("Profile.Field.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the contact email.
        /// </summary>
        /// <value>The contact email.</value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the company phone.
        /// </summary>
        /// <value>The company phone.</value>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the bill frequency.
        /// </summary>
        /// <value>The bill frequency.</value>
        public string BillFrequency { get; set; }

        /// <summary>
        /// Gets or sets the frequency value.
        /// </summary>
        /// <value>The frequency value.</value>
        public int FrequencyValue { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [AppLocalizedStringDisplayName("Profile.Field.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        [AppLocalizedStringDisplayName("Profile.Field.MiddleName")]
        [AllowHtml]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [AppLocalizedStringDisplayName("Profile.Field.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the company email.
        /// </summary>
        /// <value>The company email.</value>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Gets or sets the cell phone.
        /// </summary>
        /// <value>The cell phone.</value>
        [AppLocalizedStringDisplayName("Profile.Field.CellPhone")]
        [AllowHtml]
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ConfirmPassword")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the user role identifier.
        /// </summary>
        /// <value>The user role identifier.</value>
        public long UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        /// <value>The user type identifier.</value>
        public UserTypes UserType { get; set; }

        /// <summary>
        /// Gets or sets the always sold option.
        /// </summary>
        /// <value>The always sold option.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AlwaysSoldOption")]
        public short AlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the maximum duplicate days.
        /// </summary>
        /// <value>The maximum duplicate days.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.MaxDuplicateDays")]
        public short MaxDuplicateDays { get; set; }

        /// <summary>
        /// Gets or sets the daily cap.
        /// </summary>
        /// <value>The daily cap.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.DailyCap")]
        public int DailyCap { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the external id.
        /// </summary>
        /// <value>The description.</value>
        public long? ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cool off enabled].
        /// </summary>
        /// <value><c>true</c> if [cool off enabled]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("Buyer.Field.CoolOffEnabled")]
        public bool CoolOffEnabled { get; set; }

        /// <summary>
        /// Gets or sets the cool off start.
        /// </summary>
        /// <value>The cool off start.</value>
        [AppLocalizedStringDisplayName("Buyer.Field.CoolOffStart")]
        public DateTime CoolOffStart { get; set; }

        /// <summary>
        /// Gets or sets the cool off end.
        /// </summary>
        /// <value>The cool off end.</value>
        [AppLocalizedStringDisplayName("Buyer.Field.CoolOffEnd")]
        public DateTime CoolOffEnd { get; set; }


        /// <summary>
        /// Gets or sets do not present status.
        /// </summary>
        public short? DoNotPresentStatus { get; set; }

        /// <summary>
        /// Gets or sets do not present url.
        /// </summary>
        public string DoNotPresentUrl { get; set; }

        /// <summary>
        /// Gets or sets do not present response field.
        /// </summary>
        public string DoNotPresentResultField { get; set; }

        /// <summary>
        /// Gets or sets do not present response success value.
        /// </summary>
        public string DoNotPresentResultValue { get; set; }

        public string DoNotPresentRequest { get; set; }

        public string DoNotPresentPostMethod { get; set; }

        public bool CanSendLeadId { get; set; }

        public int AccountId { get; set; }

        public IList<SelectListItem> ListAffiliates { get; set; }

        public IList<SelectListItem> ListAffiliateChannels { get; set; }

        #endregion Properties
    }
}