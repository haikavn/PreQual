// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SecuritySetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Security Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class SecuritySetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Encryption Key
        /// </summary>
        /// <value>The encryption key.</value>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or Sets the Secure Sockets Layer Required For All Pages
        /// </summary>
        /// <value><c>true</c> if [SSL required for all pages]; otherwise, <c>false</c>.</value>
        public bool SslRequiredForAllPages { get; set; }

        /// <summary>
        /// Gets or Sets the Content Management XSRF Protection
        /// </summary>
        /// <value><c>true</c> if [content management XSRF protection]; otherwise, <c>false</c>.</value>
        public bool ContentManagementXsrfProtection { get; set; }

        /// <summary>
        /// Gets or Sets the Member XSRF Protection
        /// </summary>
        /// <value><c>true</c> if [member XSRF protection]; otherwise, <c>false</c>.</value>
        public bool MemberXsrfProtection { get; set; }

        /// <summary>
        /// Gets or Sets the Public XSRF Protection
        /// </summary>
        /// <value><c>true</c> if [public XSRF protection]; otherwise, <c>false</c>.</value>
        public bool PublicXsrfProtection { get; set; }

        /// <summary>
        /// Gets or Sets the Honeypot
        /// </summary>
        /// <value><c>true</c> if [honeypot enabled]; otherwise, <c>false</c>.</value>
        public bool HoneypotEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Honeypot Input Name
        /// </summary>
        /// <value>The name of the honeypot input.</value>
        public string HoneypotInputName { get; set; }

        #endregion Properties
    }
}