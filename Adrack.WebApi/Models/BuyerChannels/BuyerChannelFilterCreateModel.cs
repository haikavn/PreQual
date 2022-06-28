using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelFilterCreateModel
    {
        public List<FilterConditionValueModel> Values { get; set; }

        public List<BuyerChannelSubFilterModel> Children { get; set; }

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

        public List<FilterConditionValueModel> GetValues(string value)
        {
            List<FilterConditionValueModel> filterConditionValueModels = new List<FilterConditionValueModel>();

            if (string.IsNullOrEmpty(value))
                return filterConditionValueModels;

            string[] vals = value.Split(new char[1] { ',' });

            foreach (string v in vals)
            {
                FilterConditionValueModel filterConditionValueModel = new FilterConditionValueModel();
                string[] rangeVals = v.Split(new char[1] { '-' });
                if (rangeVals.Length > 0)
                {
                    filterConditionValueModel.Value1 = rangeVals[0];
                    if (rangeVals.Length > 1)
                        filterConditionValueModel.Value2 = rangeVals[1];
                    filterConditionValueModels.Add(filterConditionValueModel);
                }
            }

            return filterConditionValueModels;
        }

        public static explicit operator BuyerChannelFilterCondition(BuyerChannelFilterCreateModel affiliateChannelFilter)
        {
            return new BuyerChannelFilterCondition
            {
                Id = 0,
                BuyerChannelId = 0,
                Value = affiliateChannelFilter.GetValue(),
                Condition = affiliateChannelFilter.Condition,
                ConditionOperator = 0,
                CampaignTemplateId = affiliateChannelFilter.CampaignFieldId,
                ParentId = affiliateChannelFilter.ParentId
            };
        }
    }
}