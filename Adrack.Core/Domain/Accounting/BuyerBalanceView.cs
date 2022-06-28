// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerBalanceView.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class BuyerBalanceView.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerBalanceView : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var buyerBalanceView = new BuyerBalanceView()
            {
            };

            return buyerBalanceView;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the sold sum.
        /// </summary>
        /// <value>The sold sum.</value>
        public decimal SoldSum { get; set; }

        /// <summary>
        /// Gets or sets the payment sum.
        /// </summary>
        /// <value>The payment sum.</value>
        public decimal PaymentSum { get; set; }

        /// <summary>
        /// Gets or sets the invoiced sum.
        /// </summary>
        /// <value>The invoiced sum.</value>
        public decimal InvoicedSum { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

        #endregion Properties
    }
}