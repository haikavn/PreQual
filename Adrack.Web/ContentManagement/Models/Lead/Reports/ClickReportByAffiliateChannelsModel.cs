// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ClickReportByAffiliateChannelsModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class ClickReportByAffiliateChannelsModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ClickReportByAffiliateChannelsModel : BaseAppModel
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
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the impressions.
            /// </summary>
            /// <value>The impressions.</value>
            public int Impressions { get; set; }

            /// <summary>
            /// Gets or sets the clicks.
            /// </summary>
            /// <value>The clicks.</value>
            public int Clicks { get; set; }

            /// <summary>
            /// Gets or sets the conversions.
            /// </summary>
            /// <value>The conversions.</value>
            public int Conversions { get; set; }

            /// <summary>
            /// Gets or sets the cost.
            /// </summary>
            /// <value>The cost.</value>
            public decimal Cost { get; set; }

            /// <summary>
            /// Gets or sets the CPC.
            /// </summary>
            /// <value>The CPC.</value>
            public decimal CPC { get; set; }

            /// <summary>
            /// Gets or sets the revenue.
            /// </summary>
            /// <value>The revenue.</value>
            public decimal Revenue { get; set; }

            /// <summary>
            /// Gets or sets the profit.
            /// </summary>
            /// <value>The profit.</value>
            public decimal Profit { get; set; }

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
        public ClickReportByAffiliateChannelsModel()
        {
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        /// <value>The name of the buyer channel.</value>
        public string BuyerChannelName { get; set; }

        /// <summary>
        /// Gets or sets the total leads.
        /// </summary>
        /// <value>The total leads.</value>
        public int TotalLeads { get; set; }

        /// <summary>
        /// Gets or sets the sold leads.
        /// </summary>
        /// <value>The sold leads.</value>
        public int SoldLeads { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal Cost { get; set; }

        #endregion Properties
    }
}