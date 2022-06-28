using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class AddCampaignModel : BaseIdentifiedItem
    {
        #region Properties

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public ActivityStatuses Status { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
        public CampaignTypes CampaignType { get; set; }
        public string XmlTemplate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long VerticalId { get; set; }
        public string CampaignKey { get; set; }
        public decimal NetworkTargetRevenue { get; set; }
        public decimal NetworkMinimumRevenue { get; set; }
        public bool? Deleted { get; set; }
        public short? PingTreeCycle { get; set; }
        public bool? PrioritizedEnabled { get; set; }

        public bool IsTemplate { get; set; }

        public List<CampaignFieldModel> CampaignFields { get; set; }

        public List<FilterModel> FilterSets { get; set; }



        #endregion Properties

        public static explicit operator Campaign(AddCampaignModel campaignModel)
        {
            var fields = new List<CampaignField>();
            if (campaignModel.CampaignFields != null && campaignModel.CampaignFields.Any())
            {
                foreach (var field in campaignModel.CampaignFields)
                {
                    fields.Add((CampaignField)field);
                }
            }

            var filterSets = new List<Filter>();
            /*if (campaignModel.FilterSets != null && campaignModel.FilterSets.Any())
            {
                foreach (var filterSetModel in campaignModel.FilterSets)
                {
                    Filter filterSet = (Filter)filterSetModel;
                    filterSets.Add(filterSet);

                    filterSet.FilterConditions.Clear();

                    var conditions = new List<FilterCondition>();
                    if (filterSetModel.Conditions != null && filterSetModel.Conditions.Any())
                    {
                        foreach (var conditionModel in filterSetModel.Conditions)
                        {
                            var filterSetCondition = (FilterCondition)conditionModel;
                            if (!string.IsNullOrEmpty(conditionModel.CampaignFieldName))
                                filterSetCondition.CampaignTemplate = fields.Where(x => x.TemplateField == conditionModel.CampaignFieldName).FirstOrDefault();

                            conditions.Add(filterSetCondition);
                        }
                    }
                    filterSet.FilterConditions = conditions;
                }
            }*/

            return new Campaign
            {
                Id = campaignModel.Id,
                CampaignFields = fields,
                CampaignKey = campaignModel.CampaignKey,
                CampaignType = campaignModel.CampaignType,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Description = campaignModel.Description,
                Finish = DateTime.UtcNow,
                HtmlFormId = null,
                IsTemplate = campaignModel.IsTemplate,
                Name = campaignModel.Name,
                NetworkMinimumRevenue = campaignModel.NetworkMinimumRevenue,
                NetworkTargetRevenue = campaignModel.NetworkTargetRevenue,
                PingTreeCycle = campaignModel.PingTreeCycle,
                PrioritizedEnabled = campaignModel.PrioritizedEnabled,
                Start = DateTime.UtcNow,
                Status = campaignModel.Status,
                VerticalId = campaignModel.VerticalId,
                Visibility = 0,
                DataTemplate = campaignModel.XmlTemplate
            };
        }
    }
}