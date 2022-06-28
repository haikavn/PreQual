// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AffiliateInvoiceModel.cs" company="Adrack.com">
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
    public partial class AffiliateInvoiceModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
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
        /// Gets or sets the affiliate.
        /// </summary>
        /// <value>The affiliate.</value>
        public Affiliate affiliate { get; set; }

        /// <summary>
        /// Gets or sets the name of the affiliate country.
        /// </summary>
        /// <value>The name of the affiliate country.</value>
        public string AffiliateCountryName { get; set; }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>The invoices.</value>
        public List<AffiliateInvoice> invoices { get; set; }

        /// <summary>
        /// Gets or sets the affiliate invoice details list.
        /// </summary>
        /// <value>The affiliate invoice details list.</value>
        public List<AffiliateInvoiceDetails> AffiliateInvoiceDetailsList { get; set; }

        /// <summary>
        /// Gets or sets the affiliate refunded leads list.
        /// </summary>
        /// <value>The affiliate refunded leads list.</value>
        public IList<RefundedLeads> AffiliateRefundedLeadsList { get; set; }

        /// <summary>
        /// Gets or sets the affiliate invoice adjustments list.
        /// </summary>
        /// <value>The affiliate invoice adjustments list.</value>
        public IList<AffiliateInvoiceAdjustment> AffiliateInvoiceAdjustmentsList { get; set; }

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