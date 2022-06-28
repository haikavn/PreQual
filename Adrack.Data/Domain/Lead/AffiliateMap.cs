// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Affiliate}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Affiliate}" />
    public partial class AffiliateMap : AppEntityTypeConfiguration<Affiliate>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateMap()
        {
            this.ToTable("Affiliate");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}