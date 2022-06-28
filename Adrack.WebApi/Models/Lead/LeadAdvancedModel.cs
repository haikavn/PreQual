using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadAdvancedModel
    {
        public long Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AffiliateName { get; set; }
        public string AffiliateChannelName { get; set; }
        public string BuyerName { get; set; }
        public string BuyerChannelName { get; set; }
        public string CampaignName { get; set; }
        public double? ProcessingTime { get; set; }
        public short Status { get; set; }
        public string Url { get; set; }
        public decimal? AffiliateProfit { get; set; }
        public decimal? SoldAmount { get; set; }
        public decimal? Profit { get; set; }

        public short DuplicateIndicator { get; set; }
                    
        public bool? IsRedirected { get; set; }
    }
}