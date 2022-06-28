// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportBuyersByPricesModel.cs" company="Adrack.com">
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
    /// Class ReportBuyersByPricesModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportBuyersByPricesModel : BaseAppModel
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
            /// Gets or sets the buyer channel identifier.
            /// </summary>
            /// <value>The buyer identifier.</value>
            public long BuyerChannelId { get; set; }

            /// <summary>
            /// Gets or sets the name of the buyer channel.
            /// </summary>
            /// <value>The name of the buyer.</value>
            public string BuyerChannelName { get; set; }

            /// <summary>
            /// Gets or sets the sold quantity.
            /// </summary>
            /// <value>The sold quantity.</value>
            public int SoldQuantity { get; set; }

            /// <summary>
            /// Gets or sets the quantity.
            /// </summary>
            /// <value>The quantity.</value>
            public int Quantity { get; set; }

            /// <summary>
            /// Gets or sets the u quantity.
            /// </summary>
            /// <value>The u quantity.</value>
            public int UQuantity { get; set; }

            /// <summary>
            /// Gets or sets the created.
            /// </summary>
            /// <value>The created.</value>
            public DateTime Created { get; set; }

            /// <summary>
            /// Gets or sets the created string.
            /// </summary>
            /// <value>The created string.</value>
            public string CreatedStr { get; set; }

            /// <summary>
            /// Gets or sets the buyer price.
            /// </summary>
            /// <value>The buyer price.</value>
            public decimal BuyerPrice { get; set; }

            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>The status.</value>
            public short Status { get; set; }

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
        public ReportBuyersByPricesModel()
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
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the u quantity.
        /// </summary>
        /// <value>The u quantity.</value>
        public int UQuantity { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal BuyerPrice { get; set; }

        #endregion Properties
    }
}