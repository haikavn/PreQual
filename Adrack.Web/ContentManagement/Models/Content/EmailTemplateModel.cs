// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="EmailTemplateModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;

namespace Adrack.Web.ContentManagement.Models.Content
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class EmailTemplateModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Company Settings
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the SMTP account identifier.
        /// </summary>
        /// <value>The SMTP account identifier.</value>
        public long SmtpAccountId { get; set; }

        /// <summary>
        /// Gets or sets the attachment identifier.
        /// </summary>
        /// <value>The attachment identifier.</value>
        public long AttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        /// <value>The BCC.</value>
        public string Bcc { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EmailTemplateModel"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        #endregion Properties
    }
}