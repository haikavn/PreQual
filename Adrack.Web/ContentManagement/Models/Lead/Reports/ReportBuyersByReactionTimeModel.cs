// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ReportBuyersByReactionTimeModel.cs" company="Adrack.com">
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
    /// Class ReportBuyersByReactionTimeModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportBuyersByReactionTimeModel : BaseAppModel
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
            /// Gets or sets the created.
            /// </summary>
            /// <value>The created.</value>
            public string Created { get; set; }

            /// <summary>
            /// Gets or sets the lead views.
            /// </summary>
            /// <value>The lead views.</value>
            public int LeadViews { get; set; }

            /// <summary>
            /// Gets or sets the minimum elapsed.
            /// </summary>
            /// <value>The minimum elapsed.</value>
            public int MinElapsed { get; set; }

            /// <summary>
            /// Gets or sets the average elapsed.
            /// </summary>
            /// <value>The average elapsed.</value>
            public int AvgElapsed { get; set; }

            /// <summary>
            /// Gets or sets the maximum elapsed.
            /// </summary>
            /// <value>The maximum elapsed.</value>
            public int MaxElapsed { get; set; }

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
        public ReportBuyersByReactionTimeModel()
        {
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the lead views.
        /// </summary>
        /// <value>The lead views.</value>
        public int LeadViews { get; set; }

        /// <summary>
        /// Gets or sets the minimum elapsed.
        /// </summary>
        /// <value>The minimum elapsed.</value>
        public int MinElapsed { get; set; }

        /// <summary>
        /// Gets or sets the average elapsed.
        /// </summary>
        /// <value>The average elapsed.</value>
        public int AvgElapsed { get; set; }

        /// <summary>
        /// Gets or sets the maximum elapsed.
        /// </summary>
        /// <value>The maximum elapsed.</value>
        public int MaxElapsed { get; set; }

        #endregion Properties
    }
}