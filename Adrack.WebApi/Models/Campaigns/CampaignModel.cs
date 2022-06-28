using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignModel : BaseIdentifiedItem
    {
        #region Properties
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public CampaignTypes CampaignType { get; set; }
        public string XmlTemplate { get; set; }
        public long VerticalId { get; set; }
        [MaxLength(50)]
        public string CampaignKey { get; set; }
        public decimal NetworkTargetRevenue { get; set; }
        public decimal NetworkMinimumRevenue { get; set; }
        public short? PingTreeCycle { get; set; }
        public bool? PrioritizedEnabled { get; set; }

        public ActivityStatuses Status { get; set; }
        public List<CampaignFieldModel> CampaignFields { get; set; }

        #endregion Properties

        public static explicit operator Campaign(CampaignModel campaignModel)
        {
            var fields = new List<CampaignField>();
            if (campaignModel.CampaignFields != null && campaignModel.CampaignFields.Any())
            {
                foreach (var field in campaignModel.CampaignFields)
                {
                    fields.Add((CampaignField)field);
                }
            }
            
            return new Campaign
            {
                Id = campaignModel.Id,
                CampaignFields = fields,
                CampaignKey = campaignModel.CampaignKey,
                CampaignType = campaignModel.CampaignType,
                IsDeleted = false,
                Description = campaignModel.Description,
                Finish = DateTime.UtcNow,
                HtmlFormId = null,
                Name = campaignModel.Name,
                NetworkMinimumRevenue = campaignModel.NetworkMinimumRevenue,
                NetworkTargetRevenue = campaignModel.NetworkTargetRevenue,
                PingTreeCycle = campaignModel.PingTreeCycle,
                PrioritizedEnabled = campaignModel.PrioritizedEnabled,
                Start = DateTime.UtcNow,
                VerticalId = campaignModel.VerticalId,
                Visibility = 0,
                DataTemplate = campaignModel.XmlTemplate
            };
        }
    }
}