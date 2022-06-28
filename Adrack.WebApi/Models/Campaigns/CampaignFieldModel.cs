using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignFieldModel : BaseIdentifiedItem
    {
        #region Properties
        public long CampaignId { get; set; }
        [Required]
        public string TemplateField { get; set; }
        public string DatabaseField { get; set; }
        public short Validator { get; set; }
        public string SectionName { get; set; }
        public string Description { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public bool Required { get; set; }
        public string PossibleValue { get; set; }
        public bool? IsHash { get; set; }
        public string ValidatorValue { get; set; }
        public bool? IsFilterable { get; set; }


        #endregion Properties

        public static explicit operator CampaignField(CampaignFieldModel campaignFieldModel)
        {
            return new CampaignField
            {
                Id = campaignFieldModel.Id,
                BlackListTypeId = null,
                CampaignId = campaignFieldModel.CampaignId,
                ColumnNumber = 0,
                DatabaseField = campaignFieldModel.DatabaseField,
                Description = campaignFieldModel.Description,
                FieldFilterSettings = "",
                FieldType = null,
                IsFilterable = campaignFieldModel.IsFilterable,
                IsFormField = false,
                IsHash = campaignFieldModel.IsHash,
                IsHidden = null,
                Label = "",
                MaxLength = campaignFieldModel.MaxLength,
                MinLength = campaignFieldModel.MinLength,
                OptionValues = "",
                PageNumber = 0,
                PossibleValue = campaignFieldModel.PossibleValue,
                Required = campaignFieldModel.Required,
                SectionName = campaignFieldModel.SectionName,
                TemplateField = campaignFieldModel.TemplateField,
                Validator = campaignFieldModel.Validator,
                ValidatorSettings = campaignFieldModel.ValidatorValue
            };
        }

        public static explicit operator CampaignFieldModel(CampaignField campaignField)
        {
            return new CampaignFieldModel
            {
                Id = campaignField.Id,
                CampaignId = campaignField.CampaignId,
                TemplateField = campaignField.TemplateField,
                DatabaseField = campaignField.DatabaseField,
                Validator = campaignField.Validator,
                SectionName = campaignField.SectionName,
                Description = campaignField.Description,
                MinLength = campaignField.MinLength,
                MaxLength = campaignField.MaxLength,
                Required = campaignField.Required,
                PossibleValue = campaignField.PossibleValue,
                IsHash = campaignField.IsHash,
                ValidatorValue = campaignField.ValidatorSettings,
                IsFilterable = campaignField.IsFilterable
            };
        }
    }
}