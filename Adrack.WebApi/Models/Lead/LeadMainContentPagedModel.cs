using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadMainContentPagedModel : BasePagedModel
    {
        public List<LeadMainContentModel> LeadMainContents { get; internal set; }
        public string TimeZoneNowStr { get; internal set; }
    }
}