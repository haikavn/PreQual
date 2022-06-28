//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Arman Zakaryan
// Description:	AffiliateInvoiceModel
//------------------------------------------------------------------------------

using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.Accounting
{
    /// <summary>
    /// Represents a Activation Model
    /// </summary>
    public partial class AffiliateInvoiceModel : BaseAppModel
    {
        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Utilities
        #endregion

        #region Constructor
        #endregion

        #region Methods
        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>

        public long Id { get; set; }

        public string Number { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public string DateCreated { get; set; }

        public long AffiliateId { get; set; }

        public double Sum { get; set; }

        public double Refunded { get; set; }

        public double Adjustment { get; set; }

        public short? Status { get; set; }

        public long UserID { get; set; }
        /// <summary>
        /// Company Data
        /// </summary>
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyBank { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyLogoPath { get; set; }

        public Affiliate affiliate { get; set; }
        public string AffiliateCountryName { get; set; }

        

        public List<AffiliateInvoice> invoices { get; set; }

        public List<AffiliateInvoiceDetails> AffiliateInvoiceDetailsList { get; set; }

        public IList<RefundedLeads> AffiliateRefundedLeadsList { get; set; }

        public IList<AffiliateInvoiceAdjustment> AffiliateInvoiceAdjustmentsList { get; set; }

        public decimal RefundedTotal { get; set; }

        public decimal AdjustmentTotal { get; set; }
        public decimal Total { get; set; }

        #endregion
    }
}