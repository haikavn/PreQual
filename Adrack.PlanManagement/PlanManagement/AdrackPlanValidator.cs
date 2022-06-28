using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.PlanManagement
{
    public enum AdrackPlanValidatorErrors
    {
        None,
        PingsReached,
        AffilaiteChannelReached,
        CampaingsReached,
        BuyerChannelsReached,
        UsersLimitReached,
          
    }
    public class AdrackPlanValidator
    {
        public AdrackManagementPlan CurrentPlan; //current user plan
        public AdrackPlanValidatorErrors GetAdrackPlanValidationResult(AdrackManagementPlan plan,AdrackPlanReporter reporter)
        {
            if (!plan.IsActive) return AdrackPlanValidatorErrors.None;
            if (!reporter.IsFilled) reporter.FillReporting(plan);

            if (plan.PingLimits < reporter.Pings)
                return AdrackPlanValidatorErrors.PingsReached;

            if (plan.AffiliateChannelLimit < reporter.AffiliateChannels)
                return AdrackPlanValidatorErrors.AffilaiteChannelReached;

            if (plan.CampaignLimit < reporter.Campaingns)
                return AdrackPlanValidatorErrors.CampaingsReached;

            if (plan.BuyerChannelLimit < reporter.BuyerChannels)
                return AdrackPlanValidatorErrors.BuyerChannelsReached;

            if (plan.UserLimits < reporter.Users)
                return AdrackPlanValidatorErrors.UsersLimitReached;

            return AdrackPlanValidatorErrors.None;
        }
    }
}