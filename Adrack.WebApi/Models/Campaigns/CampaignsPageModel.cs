using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignsPageModel : BasePagedModel
    {
        #region constrcutors

        public CampaignsPageModel(bool initialized = false)
        {
            if (initialized)
            {
                this.SetInstanceValues();
            }
        }

        #endregion

        #region fields

        public List<CampaignModel> Campaigns { get; set; }

        #endregion
    }
}