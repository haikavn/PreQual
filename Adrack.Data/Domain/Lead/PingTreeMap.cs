// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class PingTreeMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PingTree}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PingTree}" />
    public partial class PingTreeMap : AppEntityTypeConfiguration<PingTree>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PingTreeMap()
        {
            this.ToTable("PingTree");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}