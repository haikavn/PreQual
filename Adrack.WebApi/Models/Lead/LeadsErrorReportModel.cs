using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadsErrorReportModel
    {
        public short ErrorType { get; internal set; }
        public string ErrorTypeString { get; internal set; }
        public string Response { get; internal set; }
        public string State { get; internal set; }
        public string BuyerChannelName { get; internal set; }
        public string AffiliateChannelName { get; internal set; }
        public long LeadId { get; internal set; }
        public DateTime CreatedTimeZoneDate { get; internal set; }
        public bool HasPermission { get; internal set; }
    }
}