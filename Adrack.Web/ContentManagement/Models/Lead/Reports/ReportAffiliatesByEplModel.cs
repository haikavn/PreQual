// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ReportAffiliatesByEplModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class ReportAffiliatesByEplModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportAffiliatesByEplModel : BaseAppModel
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
            /// Gets or sets the affiliate identifier.
            /// </summary>
            /// <value>The affiliate identifier.</value>
            public long AffiliateId { get; set; }

            /// <summary>
            /// Gets or sets the name of the affiliate.
            /// </summary>
            /// <value>The name of the affiliate.</value>
            public string AffiliateName { get; set; }

            /// <summary>
            /// Gets or sets the affiliate channel identifier.
            /// </summary>
            /// <value>The affiliate channel identifier.</value>
            public long AffiliateChannelId { get; set; }

            /// <summary>
            /// Gets or sets the name of the affiliate channel.
            /// </summary>
            /// <value>The name of the affiliate channel.</value>
            public string AffiliateChannelName { get; set; }

            /// <summary>
            /// Gets or sets the total.
            /// </summary>
            /// <value>The total.</value>
            public float Total { get; set; }

            /// <summary>
            /// Gets or sets the sold.
            /// </summary>
            /// <value>The sold.</value>
            public int Sold { get; set; }

            /// <summary>
            /// Gets or sets the redirects.
            /// </summary>
            /// <value>The redirects.</value>
            public int Redirects { get; set; }

            /// <summary>
            /// Gets or sets the profit.
            /// </summary>
            /// <value>The profit.</value>
            public decimal Profit { get; set; }

            /// <summary>
            /// Gets or sets the accept rate.
            /// </summary>
            /// <value>The accept rate.</value>
            public decimal AcceptRate { get; set; }

            /// <summary>
            /// Gets or sets the epl.
            /// </summary>
            /// <value>The epl.</value>
            public decimal EPL { get; set; }

            /// <summary>
            /// Gets or sets the redirect rate.
            /// </summary>
            /// <value>The redirect rate.</value>
            public decimal RedirectedRate { get; set; }

            /// <summary>
            /// Gets or sets the epa.
            /// </summary>
            /// <value>The epa.</value>
            public decimal EPA { get; set; }

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
        public ReportAffiliatesByEplModel()
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