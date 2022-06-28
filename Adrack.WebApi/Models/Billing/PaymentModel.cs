using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Billing
{
    public class PaymentModel : IBaseInModel
    {
        public bool Annually { get; set; }
        
        public short Plan { get; set; }
        
        public int PingsLimit { get; set; }

        public int LeadsLimit { get; set; }

        public string PaymentMethodNonce { get; set; }
        public string CouponCode{ get; set; }
    }
}