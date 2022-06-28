// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportAffiliatesByAffiliateChannels.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportAffiliatesByAffiliateChannels.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportClickCount
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        public int Hits { get; set; }

        public int Clicks { get; set; }

        #endregion Properties
    }
}