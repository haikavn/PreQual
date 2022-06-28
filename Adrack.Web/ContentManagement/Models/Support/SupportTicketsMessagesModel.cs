// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SupportTicketsMessagesModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Support
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class SupportTicketsMessagesModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// Affiliates Invoices
        /// </summary>
        /// <value>The support tickets messages.</value>
        public List<SupportTicketsMessages> supportTicketsMessages { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>The ticket identifier.</value>
        public long TicketID { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>The author identifier.</value>
        public long AuthorID { get; set; }

        /// <summary>
        /// Gets or sets the is new.
        /// </summary>
        /// <value>The is new.</value>
        public int IsNew { get; set; }

        /// <summary>
        /// Gets or sets the ticket subject.
        /// </summary>
        /// <value>The ticket subject.</value>
        public string TicketSubject { get; set; }

        /// <summary>
        /// Gets or sets the ticket status.
        /// </summary>
        /// <value>The ticket status.</value>
        public int TicketStatus { get; set; }

        #endregion Properties
    }
}