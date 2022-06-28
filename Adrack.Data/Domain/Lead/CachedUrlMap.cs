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
    /// Class BuyerMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CachedUrl}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CachedUrl}" />
    public partial class CachedUrlMap : AppEntityTypeConfiguration<CachedUrl>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public CachedUrlMap()
        {
            this.ToTable("CachedUrl");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}