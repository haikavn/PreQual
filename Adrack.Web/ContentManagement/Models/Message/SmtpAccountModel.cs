// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SmtpAccountModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.ContentManagement.Validators.Message;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using FluentValidation.Attributes;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Message
{
    /// <summary>
    /// Represents a Smtp Account Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    [Validator(typeof(SmtpAccountValidator))]
    public partial class SmtpAccountModel : BaseAppEntityModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.Email")]
        [AllowHtml]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        /// <value>The display name.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.DisplayName")]
        [AllowHtml]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets the Host
        /// </summary>
        /// <value>The host.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.Host")]
        [AllowHtml]
        public string Host { get; set; }

        /// <summary>
        /// Gets or Sets the Port
        /// </summary>
        /// <value>The port.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.Port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        /// <value>The username.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.Username")]
        [AllowHtml]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.Password")]
        [AllowHtml]
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets the Enable Ssl
        /// </summary>
        /// <value><c>true</c> if [enable SSL]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.EnableSsl")]
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or Sets the Use Default Credentials
        /// </summary>
        /// <value><c>true</c> if [use default credentials]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.UseDefaultCredentials")]
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        /// <value>The send email to.</value>
        [AppLocalizedStringDisplayName("Message.SmtpAccount.Field.SendEmailTo")]
        [AllowHtml]
        public string SendEmailTo { get; set; }

        #endregion Properties
    }
}