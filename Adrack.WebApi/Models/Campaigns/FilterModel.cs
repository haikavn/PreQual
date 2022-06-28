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
using System.Linq;
using Adrack.WebApi.Models.BaseModels;
using Newtonsoft.Json;

namespace Adrack.WebApi.Models.Campaigns
{
    public class FilterModel : BaseIdentifiedItem
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public FilterModel()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the filter identifier.
        /// </summary>
        /// <value>The filter identifier.</value>
        public long Id { get; set; }

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

        public List<FilterConditionModel> Conditions { get; set; } = new List<FilterConditionModel>();


        #endregion Properties

        public static explicit operator Core.Domain.Lead.Filter(FilterModel filterModel)
        {
            var conditions = new List<FilterCondition>();
            /*if (filterModel.Conditions != null && filterModel.Conditions.Any())
            {
                foreach (var condition in filterModel.Conditions)
                {
                    conditions.Add((FilterCondition)condition);
                }
            }*/

            return new Core.Domain.Lead.Filter
            {
                CampaignId = filterModel.CampaignId,
                Name = filterModel.Name,
                VerticalId = null
            };
        }
    }
}