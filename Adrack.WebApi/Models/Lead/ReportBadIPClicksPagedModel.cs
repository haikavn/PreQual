using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class ReportBadIPClicksPagedModel : BasePagedModel
    {
        public string TimeZoneNowStr { get; internal set; }
        public List<ReportBadIPClicksModel> ReportBadIPClicks { get; internal set; }
    }
}