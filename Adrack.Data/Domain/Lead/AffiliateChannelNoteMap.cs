// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateNoteMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateNoteMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelNote}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelNote}" />
    public partial class AffiliateChannelNoteMap : AppEntityTypeConfiguration<AffiliateChannelNote>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateChannelNoteMap() // elite group
        {
            this.ToTable("AffiliateChannelNote");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}