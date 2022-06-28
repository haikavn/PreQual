// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="CampaignModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class CampaignModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class CampaignModel : BaseAppModel
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
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public long Id { get; set; }

            /// <summary>
            /// Gets or sets the database field.
            /// </summary>
            /// <value>The database field.</value>
            public string DatabaseField { get; set; }

            /// <summary>
            /// Gets or sets the validator.
            /// </summary>
            /// <value>The validator.</value>
            public short Validator { get; set; }

            public string ValidatorValue { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is required.
            /// </summary>
            /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
            public bool Required { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is hash.
            /// </summary>
            /// <value><c>true</c> if this instance is hash; otherwise, <c>false</c>.</value>
            public bool IsHash { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is hidden.
            /// </summary>
            /// <value><c>true</c> if this instance is hidden; otherwise, <c>false</c>.</value>
            public bool IsHidden { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>The description.</value>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the possible value.
            /// </summary>
            /// <value>The possible value.</value>
            public string PossibleValue { get; set; }

            /// <summary>
            /// Gets or sets the black list type identifier.
            /// </summary>
            /// <value>The black list type identifier.</value>
            public long BlackListTypeId { get; set; }

            /// <summary>
            /// Gets or sets the minimum length.
            /// </summary>
            /// <value>The minimum length.</value>
            public int MinLength { get; set; }

            /// <summary>
            /// Gets or sets the maximum length.
            /// </summary>
            /// <value>The maximum length.</value>
            public int MaxLength { get; set; }

            /// <summary>
            /// The data format HTML
            /// </summary>
            public string DataFormatHtml = "";

            /// <summary>
            /// Gets or sets a value indicating whether this instance is filterable.
            /// </summary>
            /// <value><c>true</c> if this instance is filterable; otherwise, <c>false</c>.</value>
            public bool IsFilterable { get; set; }

            /// <summary>
            /// Gets or sets the label.
            /// </summary>
            /// <value>The label.</value>
            public string Label { get; set; }

            /// <summary>
            /// Gets or sets the column number.
            /// </summary>
            /// <value>The column number.</value>
            public int ColumnNumber { get; set; }

            /// <summary>
            /// Gets or sets the page number.
            /// </summary>
            /// <value>The page number.</value>
            public int PageNumber { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is form field.
            /// </summary>
            /// <value><c>true</c> if this instance is form field; otherwise, <c>false</c>.</value>
            public bool IsFormField { get; set; }

            /// <summary>
            /// Gets or sets the option values.
            /// </summary>
            /// <value>The option values.</value>
            public string OptionValues { get; set; }

            /// <summary>
            /// Gets or sets the type of the field.
            /// </summary>
            /// <value>The type of the field.</value>
            public short FieldType { get; set; }

            public string FieldFilterSettings { get; set; }

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
        public CampaignModel()
        {
            this.ListCampaignType = new List<SelectListItem>();
            this.ListStatus = new List<SelectListItem>();
            this.ListVisibility = new List<SelectListItem>();
            this.ListSystemField = new List<SelectListItem>();
            this.ListDataType = new List<SelectListItem>();
            this.ListBlackListType = new List<SelectListItem>();
            this.ListVertical = new List<SelectListItem>();
            this.ListPingTreeCycle = new List<SelectListItem>();
            CampaignId = 0;
            PrioritizedEnabled = true;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ActivityStatuses Status { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        /// <value>The visibility.</value>
        public short Visibility { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the finish.
        /// </summary>
        /// <value>The finish.</value>
        public DateTime Finish { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public CampaignTypes CampaignType { get; set; }

        /// <summary>
        /// Gets or sets the price format.
        /// </summary>
        /// <value>The price format.</value>
        public short PriceFormat { get; set; }

        /// <summary>
        /// Gets or sets the XML template.
        /// </summary>
        /// <value>The XML template.</value>
        public string XmlTemplate { get; set; }

        /// <summary>
        /// Gets or sets the vertical identifier.
        /// </summary>
        /// <value>The vertical identifier.</value>
        public long VerticalId { get; set; }

        /// <summary>
        /// Gets or sets the type of the list campaign.
        /// </summary>
        /// <value>The type of the list campaign.</value>
        public IList<SelectListItem> ListCampaignType { get; set; }

        /// <summary>
        /// Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectListItem> ListStatus { get; set; }

        /// <summary>
        /// Gets or sets the list visibility.
        /// </summary>
        /// <value>The list visibility.</value>
        public IList<SelectListItem> ListVisibility { get; set; }

        /// <summary>
        /// Gets or sets the list system field.
        /// </summary>
        /// <value>The list system field.</value>
        public IList<SelectListItem> ListSystemField { get; set; }

        /// <summary>
        /// Gets or sets the type of the list data.
        /// </summary>
        /// <value>The type of the list data.</value>
        public IList<SelectListItem> ListDataType { get; set; }

        /// <summary>
        /// Gets or sets the type of the list black list.
        /// </summary>
        /// <value>The type of the list black list.</value>
        public IList<SelectListItem> ListBlackListType { get; set; }

        /// <summary>
        /// Gets or sets the list vertical.
        /// </summary>
        /// <value>The list vertical.</value>
        public IList<SelectListItem> ListVertical { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable is template].
        /// </summary>
        /// <value><c>true</c> if [enable is template]; otherwise, <c>false</c>.</value>
        public bool EnableIsTemplate { get; set; }

        /// <summary>
        /// Gets or sets the network target revenue.
        /// </summary>
        /// <value>The network target revenue.</value>
        public decimal NetworkTargetRevenue { get; set; }

        /// <summary>
        /// Gets or sets the network minimum revenue.
        /// </summary>
        /// <value>The network minimum revenue.</value>
        public decimal NetworkMinimumRevenue { get; set; }

        /// <summary>
        /// Gets or sets the HTML form identifier.
        /// </summary>
        /// <value>The HTML form identifier.</value>
        public string HtmlFormId { get; set; }

        public short? PingTreeCycle { get; set; }

        public IList<SelectListItem> ListPingTreeCycle { get; set; }

        public bool PrioritizedEnabled { get; set; }

        #endregion Properties
    }
}