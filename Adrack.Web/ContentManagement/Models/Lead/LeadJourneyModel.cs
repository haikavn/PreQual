// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="VerticalModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class LeadJourneyModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class LeadJourneyModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public LeadJourneyModel()
        {
        }

        #endregion Constructor

        #region Properties


        public long Id { get; set; }

        public string Name { get; set; }

        public string Action { get; set; }

        public DateTime DateTime { get; set; }

        public string Data { get; set; }

        #endregion Properties
    }
}