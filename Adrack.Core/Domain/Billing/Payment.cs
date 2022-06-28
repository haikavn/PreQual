// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 04-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-11-2020
// ***********************************************************************
// <copyright file="Payment.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Billing
{
    /// <summary>
    /// Class BuyerPayment.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class Payment : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Payment Item</returns>
        public object Clone()
        {
            var aPayment = new Payment()
            {
            };

            return aPayment;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the Payment identifier.
        /// </summary>
        /// <value>The Payment identifier.</value>
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public short PaymentMethod { get; set; }

        public long UserId { get; set; }

        public double Price { get; set; }

        public bool Annually { get; set; }

        public short Plan { get; set; }

        public int PingsLimit { get; set; }

        public int InvoiceNumber { get; set; }

        public short Status { get; set; }

        public string PayPalSubscriptionId { get; set; }

        public string TransactionId { get; set; }
        
        public int? LeadsLimit { get; set; }

        public string PayPalCustomerId { get; set; }

        public string CreditCardLastFour { get; set; }
        public string CreditCardMaskedNumber { get; set; }

        public int? CreditCardType { get; set; }

        public string CreditCardCardholderName { get; set; }

        public string CreditCardExpirationDate { get; set; }


        #endregion Properties
    }
}