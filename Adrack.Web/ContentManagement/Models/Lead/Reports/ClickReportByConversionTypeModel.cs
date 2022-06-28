// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ClickReportByConversionTypeModel.cs" company="Adrack.com">
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
    /// Class ClickReportByConversionTypeModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class ClickReportByConversionTypeModel : BaseAppModel
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
            /// Gets or sets the conversion date.
            /// </summary>
            /// <value>The conversion date.</value>
            public string ConversionDate { get; set; }

            /// <summary>
            /// Gets or sets the type of the conversion.
            /// </summary>
            /// <value>The type of the conversion.</value>
            public short ConversionType { get; set; }

            /// <summary>
            /// Gets or sets the name of the conversion type.
            /// </summary>
            /// <value>The name of the conversion type.</value>
            public string ConversionTypeName { get; set; }

            /// <summary>
            /// Gets or sets the quantity.
            /// </summary>
            /// <value>The quantity.</value>
            public int Quantity { get; set; }

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
        public ClickReportByConversionTypeModel()
        {
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the conversion date.
        /// </summary>
        /// <value>The conversion date.</value>
        public DateTime ConversionDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the conversion.
        /// </summary>
        /// <value>The type of the conversion.</value>
        public short ConversionType { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

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

        #endregion Properties
    }
}