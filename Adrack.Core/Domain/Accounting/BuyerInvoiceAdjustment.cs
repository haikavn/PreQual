// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerInvoiceAdjustment.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class BuyerInvoiceAdjustment.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerInvoiceAdjustment : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var bInvoice = new BuyerInvoiceAdjustment()
            {
            };

            return bInvoice;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the buyer invoice identifier.
        /// </summary>
        /// <value>The buyer invoice identifier.</value>
        public long BuyerInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the qty.
        /// </summary>
        /// <value>The qty.</value>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the sum.
        /// </summary>
        /// <value>The sum.</value>
        public double Sum { get; set; }

        #endregion Properties
    }
}