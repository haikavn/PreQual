// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BuyerInvoiceModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Accounting
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class BuyerInvoiceModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        /// <value>The date from.</value>
        public string DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        /// <value>The date to.</value>
        public string DateTo { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public string DateCreated { get; set; }

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
        /// Company Data
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>The company address.</value>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the company bank.
        /// </summary>
        /// <value>The company bank.</value>
        public string CompanyBank { get; set; }

        /// <summary>
        /// Gets or sets the company email.
        /// </summary>
        /// <value>The company email.</value>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Gets or sets the company logo path.
        /// </summary>
        /// <value>The company logo path.</value>
        public string CompanyLogoPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer country.
        /// </summary>
        /// <value>The name of the buyer country.</value>
        public string BuyerCountryName { get; set; }

        /// <summary>
        /// Buyer Information
        /// </summary>
        /// <value>The buyer.</value>
        public Buyer buyer { get; set; }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>The invoices.</value>
        public List<BuyerInvoice> invoices { get; set; }

        /// <summary>
        /// Gets or sets the buyer invoice details list.
        /// </summary>
        /// <value>The buyer invoice details list.</value>
        public List<BuyerInvoiceDetails> BuyerInvoiceDetailsList { get; set; }

        /// <summary>
        /// Gets or sets the buyer refunded leads list.
        /// </summary>
        /// <value>The buyer refunded leads list.</value>
        public IList<RefundedLeads> BuyerRefundedLeadsList { get; set; }

        /// <summary>
        /// Gets or sets the buyer invoice adjustments list.
        /// </summary>
        /// <value>The buyer invoice adjustments list.</value>
        public IList<BuyerInvoiceAdjustment> BuyerInvoiceAdjustmentsList { get; set; }

        /// <summary>
        /// Gets or sets the refunded total.
        /// </summary>
        /// <value>The refunded total.</value>
        public decimal RefundedTotal { get; set; }

        /// <summary>
        /// Gets or sets the adjustment total.
        /// </summary>
        /// <value>The adjustment total.</value>
        public decimal AdjustmentTotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal Total { get; set; }

        #endregion Properties
    }
}