// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AffiliateReportModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead.Reports
{
    /// <summary>
    /// Class AffiliateReportModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class AffiliateReportModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateReportModel()
        {
            this.ListAffiliates = new List<SelectListItem>();
            this.ListAffiliateChannels = new List<SelectListItem>();
            this.ListCountry = new List<SelectListItem>();
            this.ListStates = new List<SelectListItem>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the list affiliates.
        /// </summary>
        /// <value>The list affiliates.</value>
        public IList<SelectListItem> ListAffiliates { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate channels.
        /// </summary>
        /// <value>The list affiliate channels.</value>
        public IList<SelectListItem> ListAffiliateChannels { get; set; }

        /// <summary>
        /// Gets or sets the list countries.
        /// </summary>
        /// <value>The list states.</value>
        public IList<SelectListItem> ListCountry { get; set; }

        /// <summary>
        /// Gets or sets the list states.
        /// </summary>
        /// <value>The list states.</value>
        public IList<SelectListItem> ListStates { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        #endregion Properties
    }
}