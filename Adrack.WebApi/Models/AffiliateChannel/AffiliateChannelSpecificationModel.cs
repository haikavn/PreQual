using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelSpecificationModel
    {
        public AffiliateChannelExtendedModel AffiliateChannel { get; internal set; }
        public Core.Domain.Lead.Campaign Campaign { get; internal set; }
        public string PostingUrl { get; internal set; }
        public string ChannelId { get; internal set; }
        public string Password { get; internal set; }
        public List<AffiliateCampaignTemplateModel> CampaignTemplateList { get; internal set; }
        public string XmlTemplate { get; internal set; }
        public long UserTypeId { get; internal set; }
        public string HashCode { get; internal set; }
    }
}