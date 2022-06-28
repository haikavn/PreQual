// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ChangePasswordModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using Adrack.Web.Validators.Membership;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.Web.Models.Membership
{
    /// <summary>
    ///     Represents a Forgot Password Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    [Validator(typeof(ChangePasswordValidator))]
    public class ChangePasswordModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the old password.
        /// </summary>
        /// <value>The old password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        [AllowHtml]
        public string OldPassword { get; set; }

        /// <summary>
        ///     Gets or sets the password.
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

        #endregion Properties
    }
}