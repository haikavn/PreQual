// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RefundedLeadsMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class RefundedLeadsMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.RefundedLeads}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.RefundedLeads}" />
    public partial class RefundedLeadsMap : AppEntityTypeConfiguration<RefundedLeads>
    {
        #region Constructor

        /// <summary>
        /// RefundedLeadsMap
        /// </summary>
        public RefundedLeadsMap()
        {
            this.ToTable("RefundedLeads");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}