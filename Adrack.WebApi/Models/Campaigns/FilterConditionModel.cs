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
using Adrack.WebApi.Models.BaseModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.WebApi.Models.Campaigns
{
    public class FilterConditionModel : BaseIdentifiedItem
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public FilterConditionModel()
        {
        }

        #endregion Constructor

        #region Properties

        public long Id { get; set; }

        public List<FilterConditionValueModel> Values { get; set; } = new List<FilterConditionValueModel>();

        public List<FilterSubConditionModel> Children { get; set; } = new List<FilterSubConditionModel>();




        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public short Condition { get; set; }

        /// <summary>
        /// Gets or sets the campaign template identifier.
        /// </summary>
        /// <value>The campaign template identifier.</value>
        public long CampaignFieldId { get; set; }

        public string CampaignFieldName { get; set; }

        /// <summary>
        /// Gets or sets the parentid.
        /// </summary>
        /// <value>The parent id.</value>
        public long ParentId { get; set; }

        public long FilterId { get; set; }

        public string GetValue()
        {
            if (Values == null) return "";
            string returnValue = "";
            foreach(var value in Values)
            {
                returnValue += value.Value1;
                if (!string.IsNullOrEmpty(value.Value2))
                    returnValue += $"-{value.Value2}";
                returnValue += ",";
            }

            if (Values.Count > 0)
                returnValue = returnValue.Remove(returnValue.Length - 1);

            return returnValue;
        }

        public static explicit operator Core.Domain.Lead.FilterCondition(FilterConditionModel filterConditionModel)
        {
            return new Core.Domain.Lead.FilterCondition
            {
                CampaignTemplateId = filterConditionModel.CampaignFieldId,
                Condition = filterConditionModel.Condition,
                ParentId = filterConditionModel.ParentId,
                Value = filterConditionModel.GetValue(),
                Value2 = filterConditionModel.GetValue(),
                FilterId = filterConditionModel.FilterId
            };
        }

        #endregion Properties
    }
}