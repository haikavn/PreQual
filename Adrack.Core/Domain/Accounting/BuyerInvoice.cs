// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 08-04-2021
// ***********************************************************************
// <copyright file="BuyerInvoice.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class BuyerInvoice.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerInvoice : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var bInvoice = new BuyerInvoice()
            {
            };

            return bInvoice;
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
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

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