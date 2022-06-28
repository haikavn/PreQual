using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Filter = Adrack.Core.Domain.Lead.Filter;
using Adrack.Core;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerChannelModel
    {
        public List<SelectItem> ListResponseFormats { get; set; }
        public List<SelectItem> ListTypeId { get; set; }
        public List<SelectItem> ListWinResponsePostMethod { get; set; }
        public List<SelectItem> ListDataFormat { get; set; }
        public List<SelectItem> ListStatus { get; set; }
        public List<SelectItem> ListDeliveryMethod { get; set; }
        public List<SelectItem> ListFromFieldType { get; set; }
        public List<SelectItem> ListBuyerPriceOption { get; set; }
        public List<SelectItem> ListAffiliatePriceOption { get; set; }
        public List<SelectItem> ListCampaign { get; set; }
        public List<SelectItem> ListBuyer { get; set; }
        public List<SelectItem> ListBuyerChannels { get; set; }
        public List<SelectItem> ListChildChannels { get; set; }
        public List<SelectListItem> TimeZones { get; set; }
        public long CampaignId { get; set; }
        public List<Filter> Filters { get; set; }
        public List<CampaignField> CampaignTemplate { get; set; }
        public long BuyerChannelId { get; set; }
        public List<BuyerChannelFilterCondition> FilterConditions { get; set; }
        public string ChildChannels { get; set; }
        public long BuyerId { get; set; }
        public List<ScheduleItem> ScheduleItems { get; set; }
        public CampaignTypes CampaignType { get; set; }
        public string SelectedTimeZone { get; set; }
        public short TypeId { get; set; }
        public short Timeout { get; set; }
        public short AfterTimeout { get; set; }
        public decimal BuyerPrice { get; internal set; }
        public string Name { get; internal set; }
        public string XmlTemplate { get; internal set; }
        public short Status { get; internal set; }
        public string AcceptedField { get; internal set; }
        public string AcceptedValue { get; internal set; }
        public short AcceptedFromField { get; internal set; }
        public string ErrorField { get; internal set; }
        public string ErrorValue { get; set; }
        public short ErrorFromField { get; internal set; }
        public string RejectedField { get; internal set; }
        public string RejectedValue { get; internal set; }
        public short RejectedFromField { get; internal set; }
        public string TestField { get; internal set; }
        public string TestValue { get; internal set; }
        public short TestFromField { get; internal set; }
        public string RedirectField { get; internal set; }
        public string MessageField { get; internal set; }
        public string PriceField { get; internal set; }
        public string AccountIdField { get; internal set; }
        public string Delimeter { get; internal set; }
        public string PriceRejectField { get; internal set; }
        public string PriceRejectValue { get; internal set; }
        public string PostingUrl { get; internal set; }
        public string NotificationEmail { get; internal set; }
        public short DeliveryMethod { get; internal set; }
        public decimal AffiliatePrice { get; internal set; }
        public bool TimeoutNotification { get; internal set; }
        public bool CapReachedNotification { get; internal set; }
        public short DataFormat { get; internal set; }
        public string PostingHeaders { get; internal set; }
        public short AffiliatePriceOption { get; internal set; }
        public short BuyerPriceOption { get; internal set; }
        public short? MaxDuplicateDays { get; internal set; }
        public string RedirectUrl { get; internal set; }
        public bool SubIdWhiteListEnabled { get; internal set; }
        public bool EnableCustomPriceReject { get; internal set; }
        public string PriceRejectWinResponse { get; internal set; }
        public bool FieldAppendEnabled { get; internal set; }
        public string WinResponseUrl { get; internal set; }
        public List<SelectItem> Holidays { get; internal set; }
        public List<SelectItem> ListCampaignField { get; internal set; }
        public string AllowedAffiliateChannels { get; internal set; }
        public List<SelectItem> ListAffiliateChannels { get; internal set; }
        public short ResponseFormat { get; internal set; }
        public string ChannelMappingUniqueId { get; internal set; }
        public string WinResponsePostMethod { get; internal set; }
        public string LeadIdField { get; internal set; }
        public List<TreeItem> ListBuyerChannelTemplates { get; internal set; }
    }
}