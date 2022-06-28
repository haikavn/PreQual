// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ReportBuyersComparisonModel.cs" company="Adrack.com">
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
    /// Class ReportBuyersComparisonModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportBuyersComparisonModel : BaseAppModel
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
            /// Gets or sets the posted.
            /// </summary>
            /// <value>The posted.</value>
            public int Posted { get; set; }

            /// <summary>
            /// Gets or sets the sold.
            /// </summary>
            /// <value>The sold.</value>
            public int Sold { get; set; }

            /// <summary>
            /// Gets or sets the rejected.
            /// </summary>
            /// <value>The rejected.</value>
            public int Rejected { get; set; }

            /// <summary>
            /// Gets or sets the redirected.
            /// </summary>
            /// <value>The redirected.</value>
            public int Redirected { get; set; }

            public decimal Revenue { get; set; }

            /// <summary>
            /// Gets or sets the date1 buyers.
            /// </summary>
            /// <value>The date1 buyers.</value>
            public TreeItem Date1Buyers { get; set; }

            /// <summary>
            /// Gets or sets the date2 buyers.
            /// </summary>
            /// <value>The date2 buyers.</value>
            public TreeItem Date2Buyers { get; set; }

            /// <summary>
            /// Gets or sets the date3 buyers.
            /// </summary>
            /// <value>The date3 buyers.</value>
            public TreeItem Date3Buyers { get; set; }

            /// <summary>
            /// Gets or sets the date4 buyers.
            /// </summary>
            /// <value>The date4 buyers.</value>
            public List<TreeItem> Date4Buyers { get; set; }

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
                Date1Buyers = null;
                Date2Buyers = null;
                Date3Buyers = null;
                Posted = Sold = Rejected = Redirected = 0;
                title = "";
                folder = false;
                expanded = false;
                folder = false;
                expanded = false;
            }
        }



        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public ReportBuyersComparisonModel()
        {
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        /// <value>The name of the campaign.</value>
        public string CampaignName { get; set; }

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
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the sold.
        /// </summary>
        /// <value>The sold.</value>
        public int Sold { get; set; }

        /// <summary>
        /// Gets or sets the rejected.
        /// </summary>
        /// <value>The rejected.</value>
        public int Rejected { get; set; }

        /// <summary>
        /// Gets or sets the posted.
        /// </summary>
        /// <value>The posted.</value>
        public int Posted { get; set; }

        #endregion Properties
    }
}