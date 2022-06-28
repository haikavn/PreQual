using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelFilterModel
    {
        public long CampaignFieldId { get; set; }

        public string CampaignFieldName { get; set; }
        public short Condition { get; set; }
        public List<FilterConditionValueModel> Values { get; set; }

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

        public static explicit operator BuyerChannelFilterCondition(BuyerChannelFilterModel buyerChannelFilter)
        {
            return new BuyerChannelFilterCondition
            {
                Id = 0,
                BuyerChannelId = 0,
                Value = buyerChannelFilter.GetValue(),
                Value2 = null,
                Condition = buyerChannelFilter.Condition,
                ConditionOperator = 0,
                CampaignTemplateId = buyerChannelFilter.CampaignFieldId,
                ParentId = null
            };
        }
    }
}