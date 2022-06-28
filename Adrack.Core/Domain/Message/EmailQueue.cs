// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailQueue.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Queue
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class EmailQueue : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Email Account Identifier
        /// </summary>
        /// <value>The SMTP account identifier.</value>
        public long SmtpAccountId { get; set; }

        /// <summary>
        /// Gets or Sets the Attachment Identifier
        /// </summary>
        /// <value>The attachment identifier.</value>
        public long AttachmentId { get; set; }

        /// <summary>
        /// Gets or Sets the Sender
        /// </summary>
        /// <value>The sender.</value>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or Sets the Sender Name
        /// </summary>
        /// <value>The name of the sender.</value>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or Sets the Recipient
        /// </summary>
        /// <value>The recipient.</value>
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or Sets the Recipient Name
        /// </summary>
        /// <value>The name of the recipient.</value>
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or Sets the Reply To
        /// </summary>
        /// <value>The reply to.</value>
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or Sets the Reply To Name
        /// </summary>
        /// <value>The name of the reply to.</value>
        public string ReplyToName { get; set; }

        /// <summary>
        /// Gets or Sets the Cc
        /// </summary>
        /// <value>The cc.</value>
        public string Cc { get; set; }

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
        /// Gets or Sets the Attachment Name
        /// </summary>
        /// <value>The name of the attachment.</value>
        public string AttachmentName { get; set; }

        /// <summary>
        /// Gets or Sets the Attachment Path
        /// </summary>
        /// <value>The attachment path.</value>
        public string AttachmentPath { get; set; }

        /// <summary>
        /// Gets or Sets the Priority
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or Sets the Delivery Attempts
        /// </summary>
        /// <value>The delivery attempts.</value>
        public int DeliveryAttempts { get; set; }

        /// <summary>
        /// Gets or Sets the Sent On
        /// </summary>
        /// <value>The sent on.</value>
        public DateTime? SentOn { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Smtp Account
        /// </summary>
        /// <value>The SMTP account.</value>
        public virtual SmtpAccount SmtpAccount { get; set; }

        #endregion Navigation Properties
    }
}