using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports
{
    public class ReportSendingInModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<int> CampaignIds { get; set; }

        public List<int> BuyerIds { get; set; }

        public List<int> BuyerChannelIds { get; set; }

    }
}