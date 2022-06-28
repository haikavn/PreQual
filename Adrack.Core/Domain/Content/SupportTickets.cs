// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 16-02-2021
// ***********************************************************************
// <copyright file="SupportTickets.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Class SupportTickets.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class SupportTickets : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aPayment = new SupportTickets()
            {
            };

            return aPayment;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public long ManagerID { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the due date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket time.
        /// </summary>
        /// <value>The date time.</value>
        public byte? TicketType { get; set; }


        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public string TicketFilePath { get; set; }

        
        #endregion Properties
    }

    /// <summary>
    /// Class SupportTicketsView.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class SupportTicketsView : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aPayment = new SupportTicketsView()
            {
            };

            return aPayment;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public long ManagerID { get; set; }

        /// <summary>
        /// Gets or sets the managername.
        /// </summary>
        /// <value>The managername.</value>
        public string Managername { get; set; }

        /// <summary>
        /// Gets or sets the CompanyName.
        /// </summary>
        /// <value>The CompanyName.</value>
        public string CompanyName { get; set; }


        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Creates new count.
        /// </summary>
        /// <value>The new count.</value>
        public int NewCount { get; set; }

        /// <summary>
        /// Creates total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the due date time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket time.
        /// </summary>
        /// <value>The date time.</value>
        public byte? TicketType { get; set; }


        #endregion Properties
    }
}