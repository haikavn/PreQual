using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadJourneyModel
    {
        public string Name { get; set; }
        public string ChannelName { get; set; }
        public short Type { get; set; }
        public LeadActionType Action { get; set; }
        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}