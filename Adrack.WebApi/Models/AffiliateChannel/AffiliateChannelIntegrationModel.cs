using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelIntegrationModel
    {
        public string TemplateField { get; set; }
        public long CampaignFieldId { get; set; }

        public long? CampaignTemplateId { get; set; }

        public string DefaultValue { get; set; }
        public string SectionName { get; set; }
        public long AffiliateChannelId { get; set; }

        public List<string> DataFormatValues { get; set; }

        public string GetDataFormat()
        {
            if (DataFormatValues == null) return "";
            return String.Join(";", DataFormatValues.ToArray());
        }
    }
}