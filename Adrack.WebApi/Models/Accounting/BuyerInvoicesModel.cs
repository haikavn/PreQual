//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Arman Zakaryan
// Description:	BuyerInvoice Model
// Created: 03-02-2021
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Adrack.Core.Domain.Accounting;

namespace Adrack.WebApi.Models.Accounting
{
    public class BuyerInvoicesModel
    {
        public BuyerInvoicesModel()
        {
            buyerInvoices = new List<BuyerInvoiceModel>();
        }
        public List<BuyerInvoiceModel> buyerInvoices { get; set; }

        public double approvedInvoices { get; set; }
        public double payments { get; set; }
        public double outstanding { get; set; }

    }
}