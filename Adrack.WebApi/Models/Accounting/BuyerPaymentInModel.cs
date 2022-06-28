using System;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Accounting
{
    public class BuyerPaymentInModel : IBaseInModel
    {
        public long Id { get; set; }
        public long BuyerId { get; set; }

        public DateTime PaymentDate { get; set; }

        public double Amount { get; set; }

        public short PaymentMethod { get; set; }

        public string Note { get; set; }
    }
}