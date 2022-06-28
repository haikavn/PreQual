// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PaymentMethodModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class PaymentMethodModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class PaymentMethodModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public PaymentMethodModel()
        {
            this.ListCountry = new List<SelectListItem>();
            this.ListAffiliates = new List<SelectListItem>();
            this.ListPaymentMethod = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>The payment method identifier.</value>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        /// <value>The country identifier.</value>
        public long CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country id2.
        /// </summary>
        /// <value>The country id2.</value>
        public long CountryId2 { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        public short PaymentMethod { get; set; }

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

        /// <summary>
        /// Gets or sets the list payment method.
        /// </summary>
        /// <value>The list payment method.</value>
        public IList<SelectListItem> ListPaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the list country.
        /// </summary>
        /// <value>The list country.</value>
        public IList<SelectListItem> ListCountry { get; set; }

        /// <summary>
        /// Gets or sets the list affiliates.
        /// </summary>
        /// <value>The list affiliates.</value>
        public IList<SelectListItem> ListAffiliates { get; set; }

        #endregion Properties
    }
}