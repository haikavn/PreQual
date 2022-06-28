using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelGeneralInfoUpdateModel
    {
        public string Name { get; set; }
        [Required]
        public long BuyerId { get; set; }
        public long CampaignId { get; set; }
        public BuyerChannelStatuses Status { get; set; }
        public string TimeZone { get; set; }
        public short MaxDuplicateDays { get; set; }
        public short ResponseFormat { get; set; }
        public short DataFormat { get; set; }
        public string RedirectUrl { get; set; }
        public short Timeout { get; set; }
        public string NotificationEmail { get; set; }
        public string PostingUrl { get; set; }
        public string PostingHeaders { get; set; }
        public short PauseAfterTimeout { get; set; }
        public short PauseFor { get; set; }
        public long PrevManagerId { get; set; }
        public long NewManagerId { get; set; }
        public short AffiliatePriceOption { get; set; }
        public decimal AffiliatePrice { get; set; }
        public BuyerPriceOptions BuyerPriceOption { get; set; }
        public decimal BuyerPrice { get; set; }
        public bool CapReachedNotification { get; set; }
        public bool IsPauseChannel { get; set; }
        public bool TimeoutNotification { get; set; }

        public short TypeId { get; set; }
    }
}