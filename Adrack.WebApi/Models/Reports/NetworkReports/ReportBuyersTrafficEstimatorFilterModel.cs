using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.NetworkReports
{
    public class ReportBuyersTrafficEstimatorFieldFilterModel
    {
        public string FieldName { get; set; }

        public string Value { get; set; }

        public short ValueType { get; set; }

        public short Condition { get; set; }
    }

    public class ReportBuyersTrafficEstimatorFilterModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<long> BuyerChannelIds { get; set; }

        public List<long> CampaignIds { get; set; }

        public List<ReportBuyersTrafficEstimatorFieldFilterModel> Fields { get; set; }

        public bool IsCsv { get; set; }
    }
}