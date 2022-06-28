using Adrack.WebApi.Models.Interfaces;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.Accounting
{
    public struct row
    {
        public long customInvoiceId { get; set; }
        public string description { get; set; }
        public int qty { get; set; }
        public double unitPrice { get; set; }
        public double amount { get; set; }
    }
    public class CustomInvoiceInModel : IBaseInModel
    {
        public long Id { get; set; }

        public string dateOfIssue { get; set; }
        public string dateOfDue { get; set; }
        public string billingPeriod { get; set; }
        public string website { get; set; }
        public string address { get; set; }
        public string contactInformation { get; set; }
        public string billingFullName { get; set; }
        public string billingAddress { get; set; }
        public string billingContactInformation { get; set; }
        public List<row> rows;
        public double total { get; set; }

        public CustomInvoiceInModel()
        {
            rows = new List<row>();
        }

    }
}