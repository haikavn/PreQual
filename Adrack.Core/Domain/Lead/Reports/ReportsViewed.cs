// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 01-27-2021
//
// Last Modified By : Grigori
// Last Modified On : 01-27-2021
// ***********************************************************************
// <copyright file="ReportsViewed.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportsViewed.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportsViewed : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        public string ReportName { get; set; }


        /// <summary>
        /// Gets or sets the Date of the view.
        /// </summary>
        /// <value>The Date of the view.</value>
        public DateTime ViewDate { get; set; }


        /// <summary>
        /// Gets or sets the Custom Report Type Id.
        /// </summary>
        /// <value>The Custom Report Type Id.</value>
        public int? CustomReportTypeId { get; set; }

        #endregion Properties
    }
}