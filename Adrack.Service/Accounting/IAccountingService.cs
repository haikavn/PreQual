// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAccountingService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Accounting
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IAccountingService
    {
        #region Methods

        /// <summary>
        /// GetAllAffiliateInvoices
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerInvoice> GetAllBuyerInvoices(DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Get Invoices of Buyers
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="status">The status.</param>
        /// <returns>BuyerInvoice Collection Item</returns>
        IList<BuyerInvoice> GetBuyerInvoices(long BuyerId, DateTime dateFrom, DateTime dateTo, int status);

        /// <summary>
        /// GetBuyerInvoiceById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerInvoice</returns>
        BuyerInvoice GetBuyerInvoiceById(long Id);

        /// <summary>
        /// GetBuyerInvoiceDetails
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerInvoiceDetails</returns>
        List<BuyerInvoiceDetails> GetBuyerInvoiceDetails(long Id);

        /// <summary>
        /// GetAffiliateInvoiceDetails
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>AffiliateInvoiceDetails</returns>
        List<AffiliateInvoiceDetails> GetAffiliateInvoiceDetails(long Id);

        /// <summary>
        /// GetAffiliateInvoiceById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>AffiliateInvoices</returns>
        AffiliateInvoice GetAffiliateInvoiceById(long Id);

        /// <summary>
        /// Disable Buyer Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        int DisableBuyerInvoice(long Id);

        /// <summary>
        /// Disable Affiliate Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        int DisableAffiliateInvoice(long Id);

        /// <summary>
        /// Approve Buyer Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        int ApproveBuyerInvoice(long Id);

        /// <summary>
        /// Approve Affiliate Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        int ApproveAffiliateInvoice(long Id);

        /// <summary>
        /// AffiliateInvoiceChangeStatus
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns>true/false</returns>
        int AffiliateInvoiceChangeStatus(long Id, short Status);

        /// <summary>
        /// BuyerInvoiceChangeStatus
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns>true/false</returns>
        int BuyerInvoiceChangeStatus(long Id, short Status);

        /// <summary>
        /// AddBuyerInvoicePayment
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Amount">The amount.</param>
        /// <returns>true/false</returns>
        int AddBuyerInvoicePayment(long Id, double Amount);

        /// <summary>
        /// AddAffiliateInvoicePayment
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Amount">The amount.</param>
        /// <returns>true/false</returns>
        int AddAffiliateInvoicePayment(long Id, double Amount);

        /// <summary>
        /// GetBuyerDistrib
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>double</returns>
        double GetBuyerDistrib(long BuyerId);

        /// <summary>
        /// GetAllAffiliateInvoices
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="StatusFilter">The status filter.</param>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateInvoice> GetAllAffiliateInvoices(long AffiliateId, DateTime dateFrom, DateTime dateTo, int StatusFilter);

        /// <summary>
        /// GetAffiliateInvoices
        /// </summary>
        /// <param name="AffilaiteId">The affilaite identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="status">The status.</param>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateInvoice> GetAffiliateInvoices(long AffilaiteId, DateTime dateFrom, DateTime dateTo, int status);

        /// <summary>
        /// GetAllAffiliatePayments
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliatePaymentView> GetAllAffiliatePayments();

        /// <summary>
        /// Insert Affiliate Payment
        /// </summary>
        /// <param name="affiliatePayment">The affiliate payment.</param>
        /// <returns>Profile Collection Item</returns>
        long InsertAffiliatePayment(AffiliatePayment affiliatePayment);

        /// <summary>
        /// Delete Affiliate Payment
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        int DeleteAffiliatePayment(long id);

        /// <summary>
        /// GetAllBuyerPayments
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerPaymentView> GetAllBuyerPayments(short paymentMethod, string keyword);

        /// <summary>
        /// GetAllBuyerPaymentsByBuyer
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerPaymentView> GetAllBuyerPaymentsByBuyer(long buyerId, DateTime DateFrom, DateTime DateTo);

        /// <summary>
        /// Insert Buyer Payment
        /// </summary>
        /// <param name="buyerPayment">The buyer payment.</param>
        /// <returns>Profile Collection Item</returns>
        long InsertBuyerPayment(BuyerPayment buyerPayment);

        /// <summary>
        /// Delete Buyer Payment
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        int DeleteBuyerPayment(long id);

        /// <summary>
        /// EditBuyerPaymen
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        /// <exception cref="ArgumentNullException">buyerPayment</exception>
        int EditBuyerPayment(BuyerPayment buyerPaymentModel);


        /// <summary>
        /// GetAllRefundedLeads
        /// </summary>
        /// <returns>IList BuyerPaymentView</returns>
        IList<RefundedLeads> GetAllRefundedLeads(int status, DateTime fromDate, DateTime toDate, string keyword);

        /// <summary>
        /// Gets the buyer refunded leads by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;RefundedLeads&gt;.</returns>
        IList<RefundedLeads> GetBuyerRefundedLeadsById(long Id);

        /// <summary>
        /// Gets the affiliate refunded leads by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;RefundedLeads&gt;.</returns>
        IList<RefundedLeads> GetAffiliateRefundedLeadsById(long Id);

        /// <summary>
        /// Insert Refunded Lead
        /// </summary>
        /// <param name="refLeads">The reference leads.</param>
        /// <returns>long</returns>
        long InsertRefundedLeads(RefundedLeads refLeads);

        /// <summary>
        /// Delete Refunded Lead
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        int DeleteRefundedLead(long Id);

        /// <summary>
        /// Change Refunded Lead Status
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Note">The note.</param>
        /// <returns>true/false</returns>
        int ChangeRefundedStatus(long Id, byte Status, string Note);

        /// <summary>
        /// Invoice Generation
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns>Generated Invoice Id</returns>
        long GenerateBuyerInvoices(long BuyerId, DateTime? dateFrom, DateTime dateTo, long UserId);

        /// <summary>
        /// Generates the affiliate invoices.
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns>System.Int64.</returns>
        long GenerateAffiliateInvoices(long BuyerId, DateTime? dateFrom, DateTime dateTo, long UserId);

        /// <summary>
        /// Get Buyer Credit By Id
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>decimal (money)</returns>
        decimal GetBuyerCredit(long BuyerId);

        /// <summary>
        /// Get Buyer Balance By Id
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>BuyerBalance</returns>
        BuyerBalance GetBuyerBalanceById(long BuyerId);

        /// <summary>
        /// InsertBuyerBalance
        /// </summary>
        /// <param name="buyerBalance">The buyer balance.</param>
        /// <returns>long</returns>
        long InsertBuyerBalance(BuyerBalance buyerBalance);

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="soldSum">The sold sum.</param>
        /// <returns>long</returns>
        long UpdateBuyerBalance(long buyerId, decimal soldSum);

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerBalance">The buyer balance.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>long</returns>
        long UpdateBuyerBalance(BuyerBalance buyerBalance, string columnName = "");

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>long</returns>
        bool CheckCredit(long buyerId);

        /// <summary>
        /// Get Balance of Buyers
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>BuyerBalanceView</returns>
        IList<BuyerBalanceView> GetBuyersBalance(long buyerId, DateTime DateFrom, DateTime DateTo);

        /// <summary>
        /// Get Balance of Affiliates
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>BuyerBalanceView</returns>
        IList<BuyerBalanceView> GetAffiliatesBalance(long affiliateId, DateTime DateFrom, DateTime DateTo);

        /// <summary>
        /// AddBuyerInvoiceAdjustment
        /// </summary>
        /// <param name="BinvoiceId">The binvoice identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Price">The price.</param>
        /// <param name="Qty">The qty.</param>
        /// <returns>true/false</returns>
        long AddBuyerInvoiceAdjustment(long BinvoiceId, string Name, double Price, int Qty);

        /// <summary>
        /// AddAffiliateInvoiceAdjustment
        /// </summary>
        /// <param name="AinvoiceId">The ainvoice identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Price">The price.</param>
        /// <param name="Qty">The qty.</param>
        /// <returns>true/false</returns>
        long AddAffiliateInvoiceAdjustment(long AinvoiceId, string Name, double Price, int Qty);

        /// <summary>
        /// DeleteBuyerInvoiceAdjustment
        /// </summary>
        /// <param name="AdjustmentId">The adjustment identifier.</param>
        /// <returns>true/false</returns>
        long DeleteBuyerInvoiceAdjustment(long AdjustmentId);

        /// <summary>
        /// DeleteAffiliateInvoiceAdjustment
        /// </summary>
        /// <param name="AdjustmentId">The adjustment identifier.</param>
        /// <returns>true/false</returns>
        long DeleteAffiliateInvoiceAdjustment(long AdjustmentId);

        /// <summary>
        /// Gets the buyer adjustments.
        /// </summary>
        /// <param name="InvoiceId">The invoice identifier.</param>
        /// <returns>IList&lt;BuyerInvoiceAdjustment&gt;.</returns>
        IList<BuyerInvoiceAdjustment> GetBuyerAdjustments(long InvoiceId);

        /// <summary>
        /// Gets the affiliate adjustments.
        /// </summary>
        /// <param name="InvoiceId">The invoice identifier.</param>
        /// <returns>IList&lt;AffiliateInvoiceAdjustment&gt;.</returns>
        IList<AffiliateInvoiceAdjustment> GetAffiliateAdjustments(long InvoiceId);

        /// <summary>
        /// Creates the buyer invoice.
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>System.Int64.</returns>
        long CreateBuyerInvoice(long BuyerId, DateTime date);

        /// <summary>
        /// Creates the affiliate invoice.
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>System.Int64.</returns>
        long CreateAffiliateInvoice(long AffiliateId, DateTime date);

        /// <summary>
        /// Creates the custom invoice.
        /// </summary>
        /// <param name="customInvoice">The custom invoice.</param>
        /// <returns>System.Int64.</returns>
        long CreateCustomInvoice(CustomInvoice customInvoice);

        /// <summary>
        /// Creates the custom invoice.
        /// </summary>
        /// <param name="customInvoice">The custom invoice.</param>
        /// <returns>System.Int64.</returns>
        long CreateCustomInvoiceRow(CustomInvoiceRow customInvoiceRow);

        /// <summary>
        /// Get custom all invoices.
        /// </summary>
        /// <returns>List<CustomInvoice>.</returns>
        List<CustomInvoice> GetCustomInvoices();

        /// <summary>
        /// Get custom all invoice rows.
        /// </summary>
        /// /// <param name="customInvoiceId">The customInvoiceRow identifier.</param>
        /// <returns>List<CustomInvoiceRows>.</returns>
        List<CustomInvoiceRow> GetCustomInvoiceRows(long customInvoiceId);

        #endregion Methods
    }
}