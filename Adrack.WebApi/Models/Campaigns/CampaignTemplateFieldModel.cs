using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignTemplateFieldModel : BaseIdentifiedItem
    {
        public string Name { get; internal set; }
        public short Validator { get; internal set; }
        public string Parent { get; internal set; }
    }
}