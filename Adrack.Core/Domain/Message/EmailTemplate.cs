// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailTemplate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Template
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    public partial class EmailTemplate : BaseEntity, ILocalizedEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Smtp Account Identifier
        /// </summary>
        /// <value>The SMTP account identifier.</value>
        public long SmtpAccountId { get; set; }

        /// <summary>
        /// Gets or Sets the Attachment Identifier
        /// </summary>
        /// <value>The attachment identifier.</value>
        public long AttachmentId { get; set; }

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Bcc
        /// </summary>
        /// <value>The BCC.</value>
        public string Bcc { get; set; }

        /// <summary>
        /// Gets or Sets the Subject
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or Sets the Body
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or Sets the Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        public string SendgridId { get; set; }

        #endregion Properties
    }
}