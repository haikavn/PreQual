// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 22-02-2021
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 22-02-2021
// ***********************************************************************
// <copyright file="ReportByHourTotal.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportByHourTotal.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportByHourTotal : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the Hour.
        /// </summary>
        /// <value>Hour.</value>
        public int Hour { get; set; }

        /// <summary>
        /// Gets or sets the Leads.
        /// </summary>
        /// <value>Leads.</value>
        public int Leads { get; set; }

        #endregion Properties
    }
}