using Adrack.Core.Domain.Lead.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Adrack.PlanManagement
{
    
    public enum AdrackPlanApplicationPeriod
    {
        Day,
        Week,
        Month,
        Year
    }

    public enum AdrackPlanVerificationStatus
    {
        Success,
        UsersLimitReached,
        PingLimitsReached,
        LeadCountReached,
        CampaignLimitReached,
        AffiliateChannelLimitReached,
        BuyerChannelLimitReached,
    }

    public class AdrackManagementPlan
    {
        public long Id;
        //public long UserId; /// optional
        
        public List<AdrackPlanPeriodPrice> AvailablePricePlans=new List<AdrackPlanPeriodPrice>();
        public AdrackPlanPeriodPrice PricePlan; //Selected price plan
        public String Name;
        public int PingLimits;
        public int UserLimits;
        public int LeadCount;
        public AdrackPlanApplicationPeriod Period; //for over all period
        public int PeriodLength; // 0 for for overall period

        public int AffiliateChannelLimit;
        public int BuyerChannelLimit;
        public int CampaignLimit;

        public long UserId;
        public DateTime CreationDate;
        public bool IsActive;

        //Should be used only 1 from these types of discounts
        public decimal? IndependentDiscountInPercent { get; set; } //Independent discount from subscribtion's discount
        public decimal? IndependentDiscountInPrice { get; set; } //Independent discount from subscribtion's discount 

        public AdrackManagementPlan()
        {
            

        }

        public DateTime GetPeriodStart()
        {
            return DateTime.Now.AddDays(-PeriodLength);
        }

        public DateTime GetPeriodEnd()
        {
            return DateTime.UtcNow;
        }

        public void InitDefault()
        {
            AvailablePricePlans.Add(new AdrackPlanPeriodPrice { PaymentPeriod = EnumPaymentPeriod.Month, Price = 0 });
            AvailablePricePlans.Add(new AdrackPlanPeriodPrice { PaymentPeriod = EnumPaymentPeriod.Year, Price = 0 });
            UserLimits = 10;
            LeadCount = 1000;
            PeriodLength = 0;
            AffiliateChannelLimit = 100;
            BuyerChannelLimit = 100;
            CampaignLimit = 100;
            CreationDate = DateTime.Now;
            IsActive = true;
        }

        /*
        public AdrackPlanVerificationStatus GetCheckStatus(TotalRemainingEntity data, TotalRemaining pings)
        {
            if (this.UserLimits > data.TotalUsers) return AdrackPlanVerificationStatus.UsersLimitReached;
            if (this.AffiliateChannelLimit > data.TotalAffiliateChannels) return AdrackPlanVerificationStatus.AffiliateChannelLimitReached;
            if (this.CampaignLimit > data.TotalCampaigns) return AdrackPlanVerificationStatus.CampaignLimitReached;
            if (this.BuyerChannelLimit > data.TotalBuyerChannels) return AdrackPlanVerificationStatus.BuyerChannelLimitReached;
            if (this.PingLimits > pings.TotalPings) return AdrackPlanVerificationStatus.PingLimitsReached;
            if (this.LeadCount > pings.TotalLeads) return AdrackPlanVerificationStatus.LeadCountReached;
            return AdrackPlanVerificationStatus.Success;
        }
        */
    }
}