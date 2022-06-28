// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateResponseMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateResponseMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateResponse}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateResponse}" />
    public partial class AffiliateResponseMap : AppEntityTypeConfiguration<AffiliateResponse>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateResponseMap()
        {
            this.ToTable("AffiliateResponse");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}