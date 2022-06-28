using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface ICampaignsService
    {
        IList<Campaign> GetCampaignsByVerticalId(long verticalId, short deleted = 0, bool? isTemplate = null);
        IList<CampaignModel> GetTemplateCampaigns();
        Campaign GetCampaignById(long campaignId, bool cached = false);
        bool DeleteCampaignById(long id, out string message, long appUserId);

        CampaignModelTreeItem GetItemsFromXml(string xml);
        CampaignTemplateModel GetCampaignTemplateItem(long id, bool isClone = false);
        CampaignsPageModel GetAllCampaigns(short deleted = -1, bool isPaging = false);
        void PrepareContainer(CampaignModel campaign, long? verticalId = null);
        void PostCampaignItem(CampaignExtendedModel campaignExtendedModel, bool isTemplate, long appUserId);
    }
}
