using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Settings
{
    public class PostingDetailsModel
    {
        [Required(ErrorMessage = "Application URL is required")]
        public string ApplicationUrl { get; set; }

        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$")]
        [Required(ErrorMessage = "Posting URL is required")]
        public string PostingUrl { get; set; }

        [Required(ErrorMessage = "Affiliate Channel Xml is required")]
        public string AffiliateChannelXmlField { get; set; }

        public bool IsDuplicatedMonitor { get; set; }
        public bool IsAllowedAffiliateRedirect { get; set; }

        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$")]
        [Required(ErrorMessage = "Affiliate Redirection Url is required")]
        public string AffiliateRedirectUrl { get; set; }

        [Required(ErrorMessage = "White IP is required")]
        [RegularExpression(@"\b(?:\d{1,3}\.){3}\d{1,3}\b")]
        public string WhiteIp { get; set; }

        public bool MinProcessingMode { get; set; }
        public bool SystemOnHold { get; set; }
        public bool DebugMode { get; set; }
    }
}