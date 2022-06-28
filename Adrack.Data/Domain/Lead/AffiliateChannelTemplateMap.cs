// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateChannelTemplateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateChannelTemplateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelTemplate}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelTemplate}" />
    public partial class AffiliateChannelTemplateMap : AppEntityTypeConfiguration<AffiliateChannelTemplate>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateChannelTemplateMap() // elite group
        {
            this.ToTable("AffiliateChannelTemplate");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}