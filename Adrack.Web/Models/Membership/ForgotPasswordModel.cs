// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ForgotPasswordModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using Adrack.Web.Validators.Membership;
using FluentValidation.Attributes;
using System.Web.Mvc;

namespace Adrack.Web.Models.Membership
{
    /// <summary>
    ///     Represents a Forgot Password Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    [Validator(typeof(ForgotPasswordValidator))]
    public class ForgotPasswordModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        ///     Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        [AppLocalizedStringDisplayName("Membership.ForgotPassword.Email")]
        [AllowHtml]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        #endregion Properties
    }
}