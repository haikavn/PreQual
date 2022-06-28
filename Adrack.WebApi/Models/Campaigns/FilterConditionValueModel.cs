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
    public class FilterConditionValueModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public FilterConditionValueModel()
        {
        }

        #endregion Constructor

        #region Properties

        public string Value1 { get; set; }

        public string Value2 { get; set; }


        #endregion Properties
    }
}