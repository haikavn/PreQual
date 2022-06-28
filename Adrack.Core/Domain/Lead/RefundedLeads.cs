// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RefundedLeads.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Lead
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class RefundedLeads : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var refundedLeads = new RefundedLeads()
            {
            };

            return refundedLeads;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets a price.
        /// </summary>
        /// <value>a price.</value>
        public decimal APrice { get; set; }

        /// <summary>
        /// Gets or sets the b price.
        /// </summary>
        /// <value>The b price.</value>
        public decimal BPrice { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets a invoice identifier.
        /// </summary>
        /// <value>a invoice identifier.</value>
        public long? AInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the b invoice identifier.
        /// </summary>
        /// <value>The b invoice identifier.</value>
        public long? BInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the review note.
        /// </summary>
        /// <value>The review note.</value>
        public string ReviewNote { get; set; }

        /// <summary>
        /// Gets or sets the approved.
        /// </summary>
        /// <value>The approved.</value>
        public byte Approved { get; set; }

        #endregion Properties
    }
}