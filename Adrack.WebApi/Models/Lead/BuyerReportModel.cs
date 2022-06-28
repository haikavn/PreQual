using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerReportModel
    {
        public string BaseUrl { get; internal set; }
        public IList<ISelectedItem> ListCampaigns { get; internal set; }
    }
}