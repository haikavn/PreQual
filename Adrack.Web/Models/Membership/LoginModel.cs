// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LoginModel.cs" company="Adrack.com">
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
    ///     Represents a Login Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    [Validator(typeof(LoginValidator))]
    public class LoginModel : BaseAppModel
    {
        #region Properties

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
        ///     Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        [AppLocalizedStringDisplayName("Membership.Field.Email")]
        [AllowHtml]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        [AllowHtml]
        public string Password { get; set; }

        /// <summary>
        ///     Gets or Sets the Remember Me
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("Membership.Field.RememberMe")]
        public bool RememberMe { get; set; }

        /// <summary>
        ///     Gets or Sets the Display Captcha
        /// </summary>
        /// <value><c>true</c> if [display captcha]; otherwise, <c>false</c>.</value>
        public bool DisplayCaptcha { get; set; }

        #endregion Properties
    }
}