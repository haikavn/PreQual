using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignIntegrationModel
    {
        [StringLength(50, ErrorMessage = "Maximum length should be 50 symbols")]
        public string CampaignField { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50 symbols")]
        public string SystemField { get; set; }
        public short DataType { get; set; }
        [StringLength(300, ErrorMessage = "Maximum length should be 300 symbols")]
        public string Description { get; set; }
        [StringLength(100, ErrorMessage = "Maximum length should be 100 symbols")]
        public string PossibleValue { get; set; }
        public bool IsRequired { get; set; }
        public bool IsHash { get; set; }
        public bool IsFilterable { get; set; }
        [StringLength(100, ErrorMessage = "Maximum length should be 100 symbols")]
        public string SectionName { get; set; }
    }
}