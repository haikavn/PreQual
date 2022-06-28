using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerChannelInfoModel
    {
        public BuyerChannelModel BuyerChannel { get; internal set; }
        public string Template { get; internal set; }
        public string Schedule { get; internal set; }
        public string XmlTemplate { get; internal set; }
        public string Conditions { get; internal set; }
        public string Prices { get; internal set; }
        public string Allowed { get; internal set; }
        public string Holidays { get; internal set; }
        public string RedirectUrl { get; internal set; }
    }
}