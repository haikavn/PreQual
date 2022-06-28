using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelFieldModel : BaseIdentifiedItem
    {
        #region Properties
        public long CampaignTemplateId { get; set; }
        public string TemplateField { get; set; }
        public string SectionName { get; set; }
        public long AffiliateChannelId { get; set; }
        public string DefaultValue { get; set; }

        #endregion Properties


    }
}