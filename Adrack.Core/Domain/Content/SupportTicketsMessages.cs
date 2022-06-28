// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SupportTicketsMessages.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Class SupportTicketsMessages.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class SupportTicketsMessages : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aPayment = new SupportTicketsMessages()
            {
            };

            return aPayment;
        }

        #endregion Methods

        #region Properties

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
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        public bool IsNew { get; set; }

        #endregion Properties
    }
}