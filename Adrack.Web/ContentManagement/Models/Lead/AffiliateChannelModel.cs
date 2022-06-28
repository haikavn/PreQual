// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateChannelModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class AffiliateChannelModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class AffiliateChannelModel : BaseAppModel
    {
        /// <summary>
        /// Class TreeItem.
        /// </summary>
        public class TreeItem
        {
            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            /// <value>The title.</value>
            public string title { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is folder.
            /// </summary>
            /// <value><c>true</c> if folder; otherwise, <c>false</c>.</value>
            public bool folder { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is expanded.
            /// </summary>
            /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
            public bool expanded { get; set; }

            /// <summary>
            /// Gets or sets the template field.
            /// </summary>
            /// <value>The template field.</value>
            public string TemplateField { get; set; }

            /// <summary>
            /// Gets or sets the campaign template identifier.
            /// </summary>
            /// <value>The campaign template identifier.</value>
            public long CampaignTemplateId { get; set; }

            /// <summary>
            /// Gets or sets the default value.
            /// </summary>
            /// <value>The default value.</value>
            public string DefaultValue { get; set; }

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
            /// Gets or sets the children.
            /// </summary>
            /// <value>The children.</value>
            public List<TreeItem> children { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="TreeItem"/> class.
            /// </summary>
            public TreeItem()
            {
                children = new List<TreeItem>();
                folder = false;
                expanded = false;
            }
        }



        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateChannelModel()
        {
            this.ListCampaign = new List<SelectListItem>();
            this.ListAffiliate = new List<SelectListItem>();
            this.ListStatus = new List<SelectListItem>();
            this.ListCampaignField = new List<SelectListItem>();
            this.ListMinPriceOption = new List<SelectListItem>();
            this.ListDataFormat = new List<SelectListItem>();
            this.ListAffiliatePriceMethod = new List<SelectListItem>();

            MinPriceOptionValue = 20;
            MinPriceOption = 0;
            MinRevenue = 1;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
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
        public IList<SelectListItem> ListCampaign { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate.
        /// </summary>
        /// <value>The list affiliate.</value>
        public IList<SelectListItem> ListAffiliate { get; set; }

        /// <summary>
        /// Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectListItem> ListStatus { get; set; }

        /// <summary>
        /// Gets or sets the list campaign field.
        /// </summary>
        /// <value>The list campaign field.</value>
        public IList<SelectListItem> ListCampaignField { get; set; }

        /// <summary>
        /// Gets or sets the list data format.
        /// </summary>
        /// <value>The list data format.</value>
        public IList<SelectListItem> ListDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the list minimum price option.
        /// </summary>
        /// <value>The list minimum price option.</value>
        public IList<SelectListItem> ListMinPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public List<Adrack.Core.Domain.Lead.Filter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the campaign template.
        /// </summary>
        /// <value>The campaign template.</value>
        public List<CampaignField> CampaignTemplate { get; set; }

        /// <summary>
        /// Gets or sets the filter conditions.
        /// </summary>
        /// <value>The filter conditions.</value>
        public List<Adrack.Core.Domain.Lead.AffiliateChannelFilterCondition> FilterConditions { get; set; }

        /// <summary>
        /// Gets or sets the custom black lists.
        /// </summary>
        /// <value>The custom black lists.</value>
        public List<Adrack.Core.Domain.Lead.CustomBlackListValue> CustomBlackLists { get; set; }

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

        public IList<SelectListItem> ListAffiliatePriceMethod { get; set; }

        public short Timeout { get; set; }

        public string Note { get; set; }

        #endregion Properties
    }
}