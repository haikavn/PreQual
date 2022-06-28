using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Interfaces;
using Adrack.WebApi.Models.New.BuyerChannel;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelIntegrationModel
    {
        public string Name { get; set; }
        public string SectionName { get; set; }
        public long CampaignFieldId { get; set; }
        public string DefaultValue { get; set; }
        public List<BuyerChannelFieldMatchingModel> BuyerChannelFieldMatches { get; set; } = new List<BuyerChannelFieldMatchingModel>();
        public List<string> DataFormatValues { get; set; }

        public string GetDataFormat()
        {
            if (DataFormatValues == null) return "";
            return String.Join(";", DataFormatValues.ToArray());
        }

        public static explicit operator BuyerChannelTemplate(BuyerChannelIntegrationModel buyerChannelIntegrationModel)
        {
            return new BuyerChannelTemplate
            {
                Id = 0,
                CampaignTemplateId = buyerChannelIntegrationModel.CampaignFieldId,
                TemplateField = buyerChannelIntegrationModel.Name,
                SectionName = buyerChannelIntegrationModel.SectionName,
                DefaultValue = buyerChannelIntegrationModel.DefaultValue,
                DataFormat = buyerChannelIntegrationModel.GetDataFormat()
            };
        }
    }
}