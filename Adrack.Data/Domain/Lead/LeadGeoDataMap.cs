// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadGeoDataMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadGeoDataMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadGeoData}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadGeoData}" />
    public partial class LeadGeoDataMap : AppEntityTypeConfiguration<LeadGeoData>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadGeoDataMap()
        {
            this.ToTable("LeadGeoData");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}