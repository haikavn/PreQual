//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Arman Zakaryan
// Description:	BuyerInvoice Model
//------------------------------------------------------------------------------

using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.Accounting
{
    /// <summary>
    /// Represents a BuyerInvoiceModel Model
    /// </summary>
    public partial class BuyerInvoiceModel : BaseAppModel
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

        public long Id { get; set; }

        public string Number { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public string DateCreated { get; set; }

        public long BuyerId { get; set; }

        public double Sum { get; set; }

        public double Refunded { get; set; }

        public double Adjustment { get; set; }

        public double Distribute { get; set; }

        public short? Status { get; set; }

        public long UserID { get; set; }
        /// <summary>
        /// Company Data
        /// </summary>
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyBank  { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyLogoPath { get; set; }
        public string BuyerCountryName { get; set; }

        /// <summary>
        /// Buyer Information
        /// </summary>
        public Buyer buyer { get; set; }  
        public List<BuyerInvoice> invoices { get; set; }
        public List<BuyerInvoiceDetails> BuyerInvoiceDetailsList { get; set; }
        public IList<RefundedLeads> BuyerRefundedLeadsList { get; set; }
        public IList<BuyerInvoiceAdjustment> BuyerInvoiceAdjustmentsList { get; set; }
        public decimal RefundedTotal { get; set; }
        public decimal AdjustmentTotal { get; set; }
        public decimal Total { get; set; }

        #endregion
    }
}