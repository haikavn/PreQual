// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportBuyersByStates.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportSendingTime.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportSendingTime : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        public long Id { get; set; }

        public string Name { get; set; }

        public string TimeoutType { get; set; }

        public string StatusType { get; set; }

        public double MinSec { get; set; }

        public double AvgSec { get; set; }

        public double MaxSec { get; set; }

        public int Quantity { get; set; }

        #endregion Properties
    }
}