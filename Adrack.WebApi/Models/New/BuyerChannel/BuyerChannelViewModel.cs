using Adrack.Core;
using System;

namespace Adrack.WebApi.Models.New.BuyerChannel
{
    public class BuyerChannelViewModel
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public long BuyerId { get; set; }
        public string Name { get; set; }
        public BuyerChannelStatuses Status { get; set; }
        public string XmlTemplate { get; set; }
        public string AcceptedField { get; set; }
        public string AcceptedValue { get; set; }
        public short AcceptedFrom { get; set; }
        public string ErrorField { get; set; }
        public string ErrorValue { get; set; }
        public short ErrorFrom { get; set; }
        public string RejectedField { get; set; }
        public string RejectedValue { get; set; }
        public short RejectedFrom { get; set; }
        public string TestField { get; set; }
        public string TestValue { get; set; }
        public short TestFrom { get; set; }
        public string RedirectField { get; set; }
        public string MessageField { get; set; }
        public string PriceField { get; set; }
        public string Delimeter { get; set; }
        public string PriceRejectField { get; set; }
        public string PriceRejectValue { get; set; }
        public string PostingUrl { get; set; }
        public short DeliveryMethod { get; set; }
        public short Timeout { get; set; }
        public short AfterTimeout { get; set; }
        public string NotificationEmail { get; set; }
        public decimal AffiliatePrice { get; set; }
        public decimal BuyerPrice { get; set; }
        public bool CapReachedNotification { get; set; }
        public bool TimeoutNotification { get; set; }
        public int OrderNum { get; set; }
        public bool IsFixed { get; set; }
        public string AllowedAffiliateChannels { get; set; }
        public short DataFormat { get; set; }
        public string PostingHeaders { get; set; }
        public short BuyerPriceOption { get; set; }
        public short AffiliatePriceOption { get; set; }
        public short TypeId { get; set; }
        public string ZipCodeTargeting { get; set; }
        public string StateTargeting { get; set; }
        public short MinAgeTargeting { get; set; }
        public short MaxAgeTargeting { get; set; }
        public bool EnableZipCodeTargeting { get; set; }
        public bool EnableStateTargeting { get; set; }
        public bool EnableAgeTargeting { get; set; }
        public short ZipCodeCondition { get; set; }
        public short StateCondition { get; set; }
        public bool? Deleted { get; set; }
        public string Holidays { get; set; }
        public string RedirectUrl { get; set; }
        public short? MaxDuplicateDays { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneStr { get; set; }
        public double? LeadAcceptRate { get; set; }
        public bool? SubIdWhiteListEnabled { get; set; }
        public string AccountIdField { get; set; }
        public bool? EnableCustomPriceReject { get; set; }
        public string PriceRejectWinResponse { get; set; }
        public bool? FieldAppendEnabled { get; set; }
        public string WinResponseUrl { get; set; }
        public string WinResponsePostMethod { get; set; }
        public string LeadIdField { get; set; }
        public string ChildChannels { get; set; }
        public short? ResponseFormat { get; set; }
        public string ChannelMappingUniqueId { get; set; }
        public string StatusStr { get; set; }
        public DateTime? StatusExpireDate { get; set; }
        public bool? StatusAutoChange { get; set; }
        public short? StatusChangeMinutes { get; set; }
        public int? DailyCap { get; set; }
        public string Note { get; set; }
    }
}