// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ReportBuyersByStatesModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class ReportSendingTimeModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportSendingTimeModel : BaseAppModel
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

            public long BuyerId { get; set; }

            public double BeforeSoldMin { get; set; }

            public double BeforeSoldAvg { get; set; }

            public double BeforeSoldMax { get; set; }

            public int BeforeSoldQuantity { get; set; }

            public double BeforeRejectMin { get; set; }

            public double BeforeRejectAvg { get; set; }

            public double BeforeRejectMax { get; set; }

            public int BeforeRejectQuantity { get; set; }

            public int BeforePostedQuantity { get; set; }


            public double AfterSoldMin { get; set; }

            public double AfterSoldAvg { get; set; }

            public double AfterSoldMax { get; set; }

            public int AfterSoldQuantity { get; set; }

            public double AfterRejectMin { get; set; }

            public double AfterRejectAvg { get; set; }

            public double AfterRejectMax { get; set; }

            public int AfterRejectQuantity { get; set; }

            public int AfterPostedQuantity { get; set; }

            public int TotalPaused { get; set; }


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
        public ReportSendingTimeModel()
        {
        }

        #endregion Constructor

        #region Properties

        public string Name { get; set; }

        public float BeforeSoldMin { get; set; }

        public float BeforeSoldAvg { get; set; }

        public float BeforeSoldMax { get; set; }

        public int BeforeSoldQuantity { get; set; }

        public float BeforeRejectMin { get; set; }

        public float BeforeRejectAvg { get; set; }

        public float BeforeRejectMax { get; set; }

        public int BeforeRejectQuantity { get; set; }


        public float AfterSoldMin { get; set; }

        public float AfterSoldAvg { get; set; }

        public float AfterSoldMax { get; set; }

        public int AfterSoldQuantity { get; set; }

        public float AfterRejectMin { get; set; }

        public float AfterRejectAvg { get; set; }

        public float AfterRejectMax { get; set; }

        public int AfterRejectQuantity { get; set; }


        #endregion Properties
    }
}