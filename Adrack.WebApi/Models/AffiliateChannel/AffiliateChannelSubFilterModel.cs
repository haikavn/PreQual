using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelSubFilterModel
    {
        public long? Id { get; set; }
        public List<FilterConditionValueModel> Values { get; set; }
        public short Condition { get; set; }

        public long CampaignFieldId { get; set; }

        public string CampaignFieldName { get; set; }

        public long ParentId { get; set; }

        public string GetValue()
        {
            if (Values == null) return "";
            string returnValue = "";
            foreach (var value in Values)
            {
                returnValue += value.Value1;
                if (!string.IsNullOrEmpty(value.Value2))
                    returnValue += $"-{value.Value2}";
                returnValue += ",";
            }

            if (Values.Count > 0)
                returnValue = returnValue.Remove(returnValue.Length - 1);

            return returnValue;
        }

        public static explicit operator AffiliateChannelFilterCondition(AffiliateChannelSubFilterModel affiliateChannelFilter)
        {
            return new AffiliateChannelFilterCondition
            {
                Id = 0,
                AffiliateChannelId = 0,
                Value = affiliateChannelFilter.GetValue(),
                Condition = affiliateChannelFilter.Condition,
                ConditionOperator = 0,
                CampaignTemplateId = affiliateChannelFilter.CampaignFieldId,
                ParentId = affiliateChannelFilter.ParentId
            };
        }
    }
}