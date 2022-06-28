// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerPaymentsView.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class BuyerPaymentView.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class BuyerPaymentView : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aPayment = new BuyerPaymentView()
            {
            };

            return aPayment;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>The payment date.</value>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        public short? PaymentMethod { get; set; }

        #endregion Properties
    }
}