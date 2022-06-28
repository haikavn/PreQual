using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelResponseSettingModel
    {
        public string RedirectField { get; set; }
        public string MessageField { get; set; }
        public string PriceField { get; set; }
        public string AccountIdField { get; set; }
        public string SoldFieldName { get; set; }
        public string SoldValue { get; set; }
        public short SoldFrom { get; set; }
        public string ErrorFieldName { get; set; }
        public string ErrorValue { get; set; }
        public short ErrorFrom { get; set; }
        public string RejectedFieldName { get; set; }
        public string RejectedValue { get; set; }
        public short RejectedFrom { get; set; }
        public string TestFieldName { get; set; }
        public string TestValue { get; set; }
        public short TestFrom { get; set; }
        public string PriceRejectFieldName { get; set; }
        public string PriceRejectValue { get; set; }
        public string Delimeter { get; set; }
        public string WinResponseUrl { get; set; }
        public string WinResponsePostMethod { get; set; }
        public string WinResponsePriceReject { get; set; }
        public string LeadId { get; set; }

        public short TypeId { get; set; }

        public bool AlwaysBuyerPrice { get; set; }
    }
}