// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SmtpAccount.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Smtp Account
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class SmtpAccount : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Email Queue
        /// </summary>
        private ICollection<EmailQueue> _smtpQueues;

        /// <summary>
        /// Email Template
        /// </summary>
        private ICollection<EmailTemplate> _smtpTemplates;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets the Host
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or Sets the Port
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets the Enable Ssl
        /// </summary>
        /// <value><c>true</c> if [enable SSL]; otherwise, <c>false</c>.</value>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or Sets the Use Default Credentials
        /// </summary>
        /// <value><c>true</c> if [use default credentials]; otherwise, <c>false</c>.</value>
        public bool UseDefaultCredentials { get; set; }

        #endregion Properties

        #region Custom Properties

        /// <summary>
        /// Gets the Friendly Name
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.DisplayName))
                {
                    return this.Email + " (" + this.DisplayName + ") ";
                }

                return this.Email;
            }
        }

        #endregion Custom Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Smtp Queue
        /// </summary>
        /// <value>The SMTP queues.</value>
        public virtual ICollection<EmailQueue> SmtpQueues
        {
            get { return _smtpQueues ?? (_smtpQueues = new List<EmailQueue>()); }
            protected set { _smtpQueues = value; }
        }

        /// <summary>
        /// Gets or Sets the Smtp Template
        /// </summary>
        /// <value>The SMTP templates.</value>
        public virtual ICollection<EmailTemplate> SmtpTemplates
        {
            get { return _smtpTemplates ?? (_smtpTemplates = new List<EmailTemplate>()); }
            protected set { _smtpTemplates = value; }
        }

        #endregion Navigation Properties
    }
}