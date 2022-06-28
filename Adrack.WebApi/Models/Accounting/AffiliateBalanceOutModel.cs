using System.Collections.Generic;
using Adrack.Core.Domain.Accounting;

namespace Adrack.WebApi.Models.Accounting
{
    public class AffiliateBalanceRow
    {
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal SoldSum { get; set; }
        public decimal InvoiceSum { get; set; }
        public decimal PaymentSum { get; set; }
        public decimal Balance { get; set; }
        public decimal FinalBalance { get; set; }
        public decimal Credit { get; set; }

    }
    public class AffiliateBalanceOutModel
    {
        public AffiliateBalanceOutModel()
        {
            AffiliateBalances = new List<AffiliateBalanceRow>();
            TotalInitialBalance = 0;
            TotalSoldSum = 0;
            TotalInvoiceSum = 0;
            TotalPaymentSum = 0;
            TotalBalance = 0;
            TotalFinalBalance = 0;
        }

        public List<AffiliateBalanceRow> AffiliateBalances { get; set; }
        public decimal TotalInitialBalance { get; set; }
        public decimal TotalSoldSum { get; set; }
        public decimal TotalInvoiceSum { get; set; }
        public decimal TotalPaymentSum { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalFinalBalance { get; set; }
    }
}