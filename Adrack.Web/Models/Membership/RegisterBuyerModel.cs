// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RegisterBuyerModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.WebPages.Html;

namespace Adrack.Web.Models.Lead
{
    /// <summary>
    ///     Class RegisterBuyerModel.
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    public class RegisterBuyerModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        ///     Register Model
        /// </summary>
        public RegisterBuyerModel()
        {
            ListCountry = new List<SelectListItem>();
            ListStateProvince = new List<SelectListItem>();
            ListUser = new List<SelectListItem>();
            ListStatus = new List<SelectListItem>();

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
        ///     Gets or sets the country identifier.
        /// </summary>
        /// <value>The country identifier.</value>
        public long CountryId { get; set; }

        /// <summary>
        ///     Gets or sets the state province identifier.
        /// </summary>
        /// <value>The state province identifier.</value>
        public long StateProvinceId { get; set; }

        /// <summary>
        ///     Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public long ManagerId { get; set; }

        /// <summary>
        ///     Gets or sets the list country.
        /// </summary>
        /// <value>The list country.</value>
        public IList<SelectListItem> ListCountry { get; set; }

        /// <summary>
        ///     Gets or sets the list state province.
        /// </summary>
        /// <value>The list state province.</value>
        public IList<SelectListItem> ListStateProvince { get; set; }

        /// <summary>
        ///     Gets or sets the list user.
        /// </summary>
        /// <value>The list user.</value>
        public IList<SelectListItem> ListUser { get; set; }

        /// <summary>
        ///     Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectListItem> ListStatus { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        public string AddressLine1 { get; set; }

        /// <summary>
        ///     Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        public string AddressLine2 { get; set; }

        /// <summary>
        ///     Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        ///     Gets or sets the zip postal code.
        /// </summary>
        /// <value>The zip postal code.</value>
        public string ZipPostalCode { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public long Phone { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

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

        #endregion Properties
    }
}