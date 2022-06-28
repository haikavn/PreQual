using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class ReportBadIPClicksModel
    {
        public string AffiliateName { get; internal set; }
        public long AffiliateId { get; internal set; }
        public bool HasPermission { get; internal set; }
        public DateTime LeadId { get; internal set; }
        public string LeadIp { get; internal set; }
        public string ClickIp { get; internal set; }
    }
}