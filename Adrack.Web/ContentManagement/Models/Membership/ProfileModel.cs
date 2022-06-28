// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ProfileModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Web.ContentManagement.Validators.Membership;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Membership
{
    /// <summary>
    /// Represents a Profile Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    [Validator(typeof(ProfileValidator))]
    public partial class ProfileModel : BaseAppEntityModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileModel"/> class.
        /// </summary>
        public ProfileModel()
        {
            TimeZones = new List<SelectListItem>();
            MenuTypes = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        #region Personal Information

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        /// <value>The user type identifier.</value>
        public UserTypes UserType { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or Sets the First Name
        /// </summary>
        /// <value>The first name.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets the Middle Name
        /// </summary>
        /// <value>The name of the middle.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name
        /// </summary>
        /// <value>The last name.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets the Alternate Name
        /// </summary>
        /// <value>The name of the alternate.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string AlternateName { get; set; }

        /// <summary>
        /// Gets or Sets the Gender
        /// </summary>
        /// <value>The gender.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or Sets the MenuType
        /// </summary>
        /// <value>The type of the menu.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("MenuType")]
        public short? MenuType { get; set; }

        /// <summary>
        /// Gets or Sets the Date Of Birth
        /// </summary>
        /// <value>The date of birth.</value>
        [AllowHtml]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or Sets the Summary
        /// </summary>
        /// <value>The summary.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or Sets Phone
        /// </summary>
        /// <value>The phone.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or Sets Cell Phone
        /// </summary>
        /// <value>The cell phone.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string CellPhone { get; set; }

        #endregion Personal Information

        #region Change Password

        /// <summary>
        /// Gets or Sets the Old Password
        /// </summary>
        /// <value>The old password.</value>
        [AllowHtml]
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.OldPassword")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or Sets the New Password
        /// </summary>
        /// <value>The new password.</value>
        [AllowHtml]
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.NewPassword")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or Sets the Confirm New Password
        /// </summary>
        /// <value>The confirm new password.</value>
        [AllowHtml]
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        #endregion Change Password

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        public string ProfileImagePath { get; set; }

        /// <summary>
        /// Gets or sets the time zones.
        /// </summary>
        /// <value>The time zones.</value>
        public List<SelectListItem> TimeZones { get; set; }


        /// <summary>
        /// Gets or sets the menu types.
        /// </summary>
        /// <value>The menu types.</value>
        public List<SelectListItem> MenuTypes { get; set; }

        #endregion Properties
    }
}