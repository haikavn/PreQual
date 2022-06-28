// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportBadIPClicks.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportBadIPClicks.
    /// </summary>
    public partial class ReportBadIPClicks
    {
        #region Properties

        /// <summary>
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the lead ip.
        /// </summary>
        /// <value>The lead ip.</value>
        public string LeadIp { get; set; }

        /// <summary>
        /// Gets or sets the click ip.
        /// </summary>
        /// <value>The click ip.</value>
        public string ClickIp { get; set; }

        #endregion Properties
    }
}