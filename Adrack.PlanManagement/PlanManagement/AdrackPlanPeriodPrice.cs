
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.PlanManagement
{
    public enum EnumPaymentPeriod
    {
        Month=1,
        HalfYear=2,
        Year=3,
        TwoYears=4

    }
    public class AdrackPlanPeriodPrice
    {
        public EnumPaymentPeriod PaymentPeriod;
        public int Price; //Un USD

        //Should be used only 1 from these types of discounts
        public decimal? DiscountInPercent { get; set; }
        public decimal? DiscountInPrice { get; set; }
    }
}