
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.PlanManagement
{
    public class AdrackPlanReporter
    {
        public bool IsFilled = false;
        public DateTime StartDate;
        public DateTime EndDate;
        public int Pings;
        public int Users;        
        public int AffiliateChannels;
        public int BuyerChannels;
        public int Campaingns;
        public int LeadCount;

        public void FillReporting(AdrackManagementPlan plan)
        {
            IsFilled = true;
            StartDate = DateTime.Now;
            switch (plan.Period)
            {
                case AdrackPlanApplicationPeriod.Day: StartDate=StartDate.AddDays(-plan.PeriodLength); break;
                case AdrackPlanApplicationPeriod.Month: StartDate = StartDate.AddMonths(-plan.PeriodLength); break;
                case AdrackPlanApplicationPeriod.Week: StartDate = StartDate.AddDays(-plan.PeriodLength*7); break;
                case AdrackPlanApplicationPeriod.Year: StartDate = StartDate.AddYears(-plan.PeriodLength); break;
            }

            EndDate = DateTime.Now;
            Pings = 10;
            Users = 10;
            AffiliateChannels = 10;
            BuyerChannels = 10;
            Campaingns = 10;
            LeadCount = 10;
        }
    }
}