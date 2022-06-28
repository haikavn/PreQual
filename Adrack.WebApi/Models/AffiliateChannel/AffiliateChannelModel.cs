using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelModel : IBaseInModel
    {
        #region constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateChannelModel()
        {
            this.ListCampaign = new List<SelectItem>();
            this.ListAffiliate = new List<SelectItem>();
            this.ListStatus = new List<SelectItem>();
            this.ListCampaignField = new List<SelectItem>();
            this.ListMinPriceOption = new List<SelectItem>();
            this.ListDataFormat = new List<SelectItem>();
            this.ListAffiliatePriceMethod = new List<SelectItem>();

            MinPriceOptionValue = 20;
            MinPriceOption = 0;
            MinRevenue = 1;
        }

        #endregion constructor

        #region properties

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the XML template.
        /// </summary>
        /// <value>The XML template.</value>
        public string XmlTemplate { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the list campaign.
        /// </summary>
        /// <value>The list campaign.</value>
        public IList<SelectItem> ListCampaign { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate.
        /// </summary>
        /// <value>The list affiliate.</value>
        public IList<SelectItem> ListAffiliate { get; set; }

        /// <summary>
        /// Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectItem> ListStatus { get; set; }

        /// <summary>
        /// Gets or sets the list campaign field.
        /// </summary>
        /// <value>The list campaign field.</value>
        public IList<SelectItem> ListCampaignField { get; set; }

        /// <summary>
        /// Gets or sets the list data format.
        /// </summary>
        /// <value>The list data format.</value>
        public IList<SelectItem> ListDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the list minimum price option.
        /// </summary>
        /// <value>The list minimum price option.</value>
        public IList<SelectItem> ListMinPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public List<Filter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the campaign template.
        /// </summary>
        /// <value>The campaign template.</value>
        public List<CampaignField> CampaignTemplate { get; set; }

        /// <summary>
        /// Gets or sets the filter conditions.
        /// </summary>
        /// <value>The filter conditions.</value>
        public List<AffiliateChannelFilterCondition> FilterConditions { get; set; }

        /// <summary>
        /// Gets or sets the custom black lists.
        /// </summary>
        /// <value>The custom black lists.</value>
        public List<CustomBlackListValue> CustomBlackLists { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price methods.
        /// </summary>
        /// <value>The affiliate price methods list.</value>
        public IList<SelectItem> ListAffiliatePriceMethod { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the sample response.
        /// </summary>
        /// <value>The sample response.</value>
        public string SampleResponse { get; set; }

        /// <summary>
        /// Gets or sets the minimum price option.
        /// </summary>
        /// <value>The minimum price option.</value>
        public short MinPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the minimum price option value.
        /// </summary>
        /// <value>The minimum price option value.</value>
        public int MinPriceOptionValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum revenue.
        /// </summary>
        /// <value>The minimum revenue.</value>
        public decimal MinRevenue { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel key.
        /// </summary>
        /// <value>The affiliate channel key.</value>
        [MaxLength(50)]
        public string AffiliateChannelKey { get; set; }

        /// <summary>
        /// Gets or sets the allowed buyer channels.
        /// </summary>
        /// <value>The allowed buyer channels.</value>
        public string AllowedBuyerChannels { get; set; }

        /// <summary>
        /// Gets or sets the data format.
        /// </summary>
        /// <value>The data format.</value>
        public short DataFormat { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public CampaignTypes CampaignType { get; set; }

        public short AffiliatePriceMethod { get; set; }

        public decimal AffiliatePrice { get; set; }

        public short Timeout { get; set; }

        #endregion Properties
    }
}