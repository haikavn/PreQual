// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateInvoice.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class AffiliateInvoice.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class AffiliateInvoice : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aInvoice = new AffiliateInvoice()
            {
            };

            return aInvoice;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public long Number { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        /// <value>The date from.</value>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        /// <value>The date to.</value>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the sum.
        /// </summary>
        /// <value>The sum.</value>
        public double Sum { get; set; }

        /// <summary>
        /// Gets or sets the refunded.
        /// </summary>
        /// <value>The refunded.</value>
        public double Refunded { get; set; }

        /// <summary>
        /// Gets or sets the adjustment.
        /// </summary>
        /// <value>The adjustment.</value>
        public double Adjustment { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short? Status { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the paid.
        /// </summary>
        /// <value>The paid.</value>
        public double Paid { get; set; }

        #endregion Properties
    }
}