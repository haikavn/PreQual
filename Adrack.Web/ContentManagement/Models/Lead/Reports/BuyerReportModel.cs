// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BuyerReportModel.cs" company="Adrack.com">
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
    /// Class BuyerReportModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class BuyerReportModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public BuyerReportModel()
        {
            this.ListBuyers = new List<SelectListItem>();
            this.ListBuyerChannels = new List<SelectListItem>();
            this.ListCampaigns = new List<SelectListItem>();
            this.ListStates = new List<SelectListItem>();
            this.ListCountry = new List<SelectListItem>();
            this.ListNoteTitles = new List<SelectListItem>();
            this.ListAffiliateChannels = new List<SelectListItem>();
            this.ListAffiliates = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the list buyers.
        /// </summary>
        /// <value>The list buyers.</value>
        public IList<SelectListItem> ListBuyers { get; set; }

        /// <summary>
        /// Gets or sets the list buyer channels.
        /// </summary>
        /// <value>The list buyer channels.</value>
        public IList<SelectListItem> ListBuyerChannels { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate channels.
        /// </summary>
        /// <value>The list affiliate channels.</value>
        public IList<SelectListItem> ListAffiliateChannels { get; set; }

        /// <summary>
        /// Gets or sets the list campaigns.
        /// </summary>
        /// <value>The list campaigns.</value>
        public IList<SelectListItem> ListCampaigns { get; set; }

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
        /// Gets or sets the list note titles.
        /// </summary>
        /// <value>The list note titles.</value>
        public IList<SelectListItem> ListNoteTitles { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the always sold option.
        /// </summary>
        /// <value>The always sold option.</value>
        public short AlwaysSoldOption { get; set; }

        public IList<SelectListItem> ListAffiliates { get; set; }

        #endregion Properties
    }
}