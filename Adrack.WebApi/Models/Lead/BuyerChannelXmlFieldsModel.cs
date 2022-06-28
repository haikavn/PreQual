using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerChannelXmlFieldsModel : IBaseInModel
    {
        public string Xml { get; internal set; }
        public string AffiliateChannelField { get; internal set; }
        public string AffiliateChannelValue { get; internal set; }
        public string AffiliatePasswordField { get; internal set; }
        public string AffiliatePasswordValue { get; internal set; }
    }
}