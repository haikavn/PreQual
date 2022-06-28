using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Payment
{
    public class PaymentPastDays
    {
        public Billing.Payment Payment { get; set; }
        public double PastDaysCount { get; set; }
        public User User { get; set; }
    }
}
