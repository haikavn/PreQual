// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BuyerReportByBuyerChannelModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class BuyerReportByBuyerChannelModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class BuyerReportByBuyerChannelModel : BaseAppModel
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

            public int RejectedLeads { get; set; }

            /// <summary>
            /// Gets or sets the loaned leads.
            /// </summary>
            /// <value>The loaned leads.</value>
            public int LoanedLeads { get; set; }

            /// <summary>
            /// Gets or sets the cost.
            /// </summary>
            /// <value>The cost.</value>
            public decimal Cost { get; set; }

            /// <summary>
            /// Gets or sets the cap.
            /// </summary>
            /// <value>The cap.</value>
            public int Cap { get; set; }

            /// <summary>
            /// Gets or sets the accept rate.
            /// </summary>
            /// <value>The accept rate.</value>
            public decimal AcceptRate { get; set; }

            /// <summary>
            /// Gets or sets the redirected.
            /// </summary>
            /// <value>The redirected.</value>
            public int Redirected { get; set; }

            /// <summary>
            /// Gets or sets the redirected rate.
            /// </summary>
            /// <value>The redirected rate.</value>
            public decimal RedirectedRate { get; set; }

            /// <summary>
            /// Gets or sets the profit.
            /// </summary>
            /// <value>The profit.</value>
            public decimal Profit { get; set; }

            /// <summary>
            /// Gets or sets the average price.
            /// </summary>
            /// <value>The sum of affiliate price.</value>
            public decimal AveragePrice { get; set; }

            /// <summary>
            /// Gets or sets the rank.
            /// </summary>
            /// <value>The rank.</value>
            public int Rank { get; set; }

            public bool CapHit { get; set; }

            public string LastSoldDate { get; set; }

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
        public BuyerReportByBuyerChannelModel()
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