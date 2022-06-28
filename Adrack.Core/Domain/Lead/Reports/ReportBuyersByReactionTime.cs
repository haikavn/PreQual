// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportBuyersByReactionTime.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportBuyersByReactionTime.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportBuyersByReactionTime : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the lead views.
        /// </summary>
        /// <value>The lead views.</value>
        public int LeadViews { get; set; }

        /// <summary>
        /// Gets or sets the minimum elapsed.
        /// </summary>
        /// <value>The minimum elapsed.</value>
        public int MinElapsed { get; set; }

        /// <summary>
        /// Gets or sets the average elapsed.
        /// </summary>
        /// <value>The average elapsed.</value>
        public int AvgElapsed { get; set; }

        /// <summary>
        /// Gets or sets the maximum elapsed.
        /// </summary>
        /// <value>The maximum elapsed.</value>
        public int MaxElapsed { get; set; }

        #endregion Properties
    }
}