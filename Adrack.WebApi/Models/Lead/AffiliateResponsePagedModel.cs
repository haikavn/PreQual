using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateResponsePagedModel : BasePagedModel
    {
        public List<AffiliateResponseModel> AffiliateResponses { get; internal set; }
        public string TimeZoneNowStr { get; internal set; }
    }
}