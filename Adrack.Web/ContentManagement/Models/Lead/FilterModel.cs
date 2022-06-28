// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class FilterModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class FilterModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public FilterModel()
        {
            this.FilterConditions = new List<FilterCondition>();
            this.ListCampaign = new List<SelectListItem>();
            this.ListVertical = new List<SelectListItem>();
            this.CampaignTemplate = new List<Core.Domain.Lead.CampaignField>();
            CampaignId = 0;
            VerticalId = 0;
            IsCampaignReadOnly = false;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the filter identifier.
        /// </summary>
        /// <value>The filter identifier.</value>
        public long FilterId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the vertical identifier.
        /// </summary>
        /// <value>The vertical identifier.</value>
        public long VerticalId { get; set; }

        /// <summary>
        /// Gets or sets the list campaign.
        /// </summary>
        /// <value>The list campaign.</value>
        public IList<SelectListItem> ListCampaign { get; set; }

        /// <summary>
        /// Gets or sets the list vertical.
        /// </summary>
        /// <value>The list vertical.</value>
        public IList<SelectListItem> ListVertical { get; set; }

        /// <summary>
        /// Gets or sets the campaign template.
        /// </summary>
        /// <value>The campaign template.</value>
        public List<CampaignField> CampaignTemplate { get; set; }

        /// <summary>
        /// Gets or sets the filter conditions.
        /// </summary>
        /// <value>The filter conditions.</value>
        public List<FilterCondition> FilterConditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is campaign read only.
        /// </summary>
        /// <value><c>true</c> if this instance is campaign read only; otherwise, <c>false</c>.</value>
        public bool IsCampaignReadOnly { get; set; }

        #endregion Properties
    }
}