//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Arman Zakaryan
// Description:	AffiliateInvoiceModel
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Adrack.Core.Domain.Accounting;

namespace Adrack.WebApi.Models.Accounting
{
    public class AffiliateInvoicesModel
    {
        public IList<AffiliateInvoice> affiliateInvoices { get; set; }

        public double approvedInvoices { get; set; }
        public double payments { get; set; }
        public double outstanding { get; set; }

    }
}