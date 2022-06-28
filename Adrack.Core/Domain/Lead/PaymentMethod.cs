// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PaymentMethod.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Affiliate
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class PaymentMethod : BaseEntity
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
            var affiliate = new Affiliate()
            {
            };

            return affiliate;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>The type of the payment.</value>
        public short PaymentType { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name on account.</value>
        public string NameOnAccount { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>The account number.</value>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the swift routing number.
        /// </summary>
        /// <value>The swift routing number.</value>
        public string SwiftRoutingNumber { get; set; }

        /// <summary>
        /// Gets or sets the bank address.
        /// </summary>
        /// <value>The bank address.</value>
        public string BankAddress { get; set; }

        /// <summary>
        /// Gets or sets the bank phone.
        /// </summary>
        /// <value>The bank phone.</value>
        public string BankPhone { get; set; }

        /// <summary>
        /// Gets or sets the account owner address.
        /// </summary>
        /// <value>The account owner address.</value>
        public string AccountOwnerAddress { get; set; }

        /// <summary>
        /// Gets or sets the account owner phone.
        /// </summary>
        /// <value>The account owner phone.</value>
        public string AccountOwnerPhone { get; set; }

        /// <summary>
        /// Gets or sets the special instructions.
        /// </summary>
        /// <value>The special instructions.</value>
        public string SpecialInstructions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value><c>true</c> if this instance is primary; otherwise, <c>false</c>.</value>
        public bool IsPrimary { get; set; }

        #endregion Properties
    }
}