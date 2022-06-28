// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AffiliateModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class AffiliateModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class AffiliateModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateModel()
        {
            this.ListCountry = new List<SelectListItem>();
            this.ListStateProvince = new List<SelectListItem>();
            this.ListUser = new List<SelectListItem>();
            this.ListStatus = new List<SelectListItem>();
            this.Notes = new List<AffiliateNote>();

            this.ListUserType = new List<SelectListItem>();
            this.ListUserRole = new List<SelectListItem>();
            this.ListDepartment = new List<SelectListItem>();
            this.ListDefaultAffiliatePriceMethod = new List<SelectListItem>();

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
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [AppLocalizedStringDisplayName("Profile.Field.AddressLine1")]
        [AllowHtml]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [AppLocalizedStringDisplayName("Profile.Field.AddressLine2")]
        [AllowHtml]
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
        [AllowHtml]
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the company email.
        /// </summary>
        /// <value>The company email.</value>
        public string CompanyEmail { get; set; }

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
        /// Gets or sets the name of the contact email.
        /// </summary>
        /// <value>The contact email.</value>
        public string ContactEmail { get; set; }

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
        /// Gets or sets the bill within.
        /// </summary>
        /// <value>The bill within.</value>
        public int BillWithin { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public List<AffiliateNote> Notes { get; set; }

        /// <summary>
        /// Gets or Sets the Confirm Password
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ConfirmPassword")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

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
        /// Gets or sets the user type identifier.
        /// </summary>
        /// <value>The user type identifier.</value>
        public UserTypes UserType { get; set; }

        /// <summary>
        /// Gets or sets the type of the list user.
        /// </summary>
        /// <value>The type of the list user.</value>
        public IList<SelectListItem> ListUserType { get; set; }

        /// <summary>
        /// Gets or sets the list user role.
        /// </summary>
        /// <value>The list user role.</value>
        public IList<SelectListItem> ListUserRole { get; set; }

        /// <summary>
        /// Gets or sets the list department.
        /// </summary>
        /// <value>The list department.</value>
        public IList<SelectListItem> ListDepartment { get; set; }

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
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        /// <value>The department identifier.</value>
        public long DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the cell phone.
        /// </summary>
        /// <value>The cell phone.</value>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        /// <value>The logged in user.</value>
        public User LoggedInUser { get; set; }

        /// <summary>
        /// Gets or sets the white ip.
        /// </summary>
        /// <value>The white ip.</value>
        public string WhiteIp { get; set; }

        public short DefaultAffiliatePriceMethod { get; set; }

        public decimal DefaultAffiliatePrice { get; set; }

        public IList<SelectListItem> ListDefaultAffiliatePriceMethod { get; set; }

        #endregion Properties
    }
}