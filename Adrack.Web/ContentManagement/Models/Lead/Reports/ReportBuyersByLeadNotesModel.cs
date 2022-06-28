// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ReportBuyersByLeadNotesModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class ReportBuyersByLeadNotesModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ReportBuyersByLeadNotesModel : BaseAppModel
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
            /// Gets or sets the quantity1.
            /// </summary>
            /// <value>The quantity1.</value>
            public int Quantity1 { get; set; }

            /// <summary>
            /// Gets or sets the quantity2.
            /// </summary>
            /// <value>The quantity2.</value>
            public int Quantity2 { get; set; }

            /// <summary>
            /// Gets or sets the quantity3.
            /// </summary>
            /// <value>The quantity3.</value>
            public int Quantity3 { get; set; }

            /// <summary>
            /// Gets or sets the quantity4.
            /// </summary>
            /// <value>The quantity4.</value>
            public int Quantity4 { get; set; }

            /// <summary>
            /// Gets or sets the quantity5.
            /// </summary>
            /// <value>The quantity5.</value>
            public int Quantity5 { get; set; }

            /// <summary>
            /// Gets or sets the quantity6.
            /// </summary>
            /// <value>The quantity6.</value>
            public int Quantity6 { get; set; }

            /// <summary>
            /// Gets or sets the quantity7.
            /// </summary>
            /// <value>The quantity7.</value>
            public int Quantity7 { get; set; }

            /// <summary>
            /// Gets or sets the quantity8.
            /// </summary>
            /// <value>The quantity8.</value>
            public int Quantity8 { get; set; }

            /// <summary>
            /// Gets or sets the quantity9.
            /// </summary>
            /// <value>The quantity9.</value>
            public int Quantity9 { get; set; }

            /// <summary>
            /// Gets or sets the quantity10.
            /// </summary>
            /// <value>The quantity10.</value>
            public int Quantity10 { get; set; }

            /// <summary>
            /// Gets or sets the quantity11.
            /// </summary>
            /// <value>The quantity11.</value>
            public int Quantity11 { get; set; }

            /// <summary>
            /// Gets or sets the quantity12.
            /// </summary>
            /// <value>The quantity12.</value>
            public int Quantity12 { get; set; }

            /// <summary>
            /// Gets or sets the quantity13.
            /// </summary>
            /// <value>The quantity13.</value>
            public int Quantity13 { get; set; }

            /// <summary>
            /// Gets or sets the quantity14.
            /// </summary>
            /// <value>The quantity14.</value>
            public int Quantity14 { get; set; }

            /// <summary>
            /// Gets or sets the quantity15.
            /// </summary>
            /// <value>The quantity15.</value>
            public int Quantity15 { get; set; }

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
        public ReportBuyersByLeadNotesModel()
        {
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the quantity1.
        /// </summary>
        /// <value>The quantity1.</value>
        public int Quantity1 { get; set; }

        /// <summary>
        /// Gets or sets the quantity2.
        /// </summary>
        /// <value>The quantity2.</value>
        public int Quantity2 { get; set; }

        /// <summary>
        /// Gets or sets the quantity3.
        /// </summary>
        /// <value>The quantity3.</value>
        public int Quantity3 { get; set; }

        /// <summary>
        /// Gets or sets the quantity4.
        /// </summary>
        /// <value>The quantity4.</value>
        public int Quantity4 { get; set; }

        /// <summary>
        /// Gets or sets the quantity5.
        /// </summary>
        /// <value>The quantity5.</value>
        public int Quantity5 { get; set; }

        /// <summary>
        /// Gets or sets the quantity6.
        /// </summary>
        /// <value>The quantity6.</value>
        public int Quantity6 { get; set; }

        /// <summary>
        /// Gets or sets the quantity7.
        /// </summary>
        /// <value>The quantity7.</value>
        public int Quantity7 { get; set; }

        /// <summary>
        /// Gets or sets the quantity8.
        /// </summary>
        /// <value>The quantity8.</value>
        public int Quantity8 { get; set; }

        /// <summary>
        /// Gets or sets the quantity9.
        /// </summary>
        /// <value>The quantity9.</value>
        public int Quantity9 { get; set; }

        /// <summary>
        /// Gets or sets the quantity10.
        /// </summary>
        /// <value>The quantity10.</value>
        public int Quantity10 { get; set; }

        /// <summary>
        /// Gets or sets the quantity11.
        /// </summary>
        /// <value>The quantity11.</value>
        public int Quantity11 { get; set; }

        /// <summary>
        /// Gets or sets the quantity12.
        /// </summary>
        /// <value>The quantity12.</value>
        public int Quantity12 { get; set; }

        /// <summary>
        /// Gets or sets the quantity13.
        /// </summary>
        /// <value>The quantity13.</value>
        public int Quantity13 { get; set; }

        /// <summary>
        /// Gets or sets the quantity14.
        /// </summary>
        /// <value>The quantity14.</value>
        public int Quantity14 { get; set; }

        /// <summary>
        /// Gets or sets the quantity15.
        /// </summary>
        /// <value>The quantity15.</value>
        public int Quantity15 { get; set; }

        #endregion Properties
    }
}