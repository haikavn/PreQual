// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ForgotPasswordConfirmationModel.cs" company="Adrack.com">
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
    ///     Represents a Forgot Password Confirmation Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    [Validator(typeof(ForgotPasswordConfirmationValidator))]
    public class ForgotPasswordConfirmationModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        ///     Gets or Sets the New Password
        /// </summary>
        /// <value>The new password.</value>
        [AllowHtml]
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.NewPassword")]
        public string NewPassword { get; set; }

        /// <summary>
        ///     Gets or Sets the Confirm New Password
        /// </summary>
        /// <value>The confirm new password.</value>
        [AllowHtml]
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        /// <summary>
        ///     Gets or Sets the Disable Password Changing
        /// </summary>
        /// <value><c>true</c> if [disable password changing]; otherwise, <c>false</c>.</value>
        public bool DisablePasswordChanging { get; set; }

        /// <summary>
        ///     Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        #endregion Properties
    }
}