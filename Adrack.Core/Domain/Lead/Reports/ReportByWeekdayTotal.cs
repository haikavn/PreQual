// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 22-02-2021
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 22-02-2021
// ***********************************************************************
// <copyright file="ReportByWeekdayTotal.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportByWeekdayTotal.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportByWeekdayTotal : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the Weekday.
        /// </summary>
        /// <value>Weekday.</value>
        public int Weekday { get; set; }

        /// <summary>
        /// Gets or sets the Leads.
        /// </summary>
        /// <value>Leads.</value>
        public int Leads { get; set; }

        #endregion Properties
    }
}