// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelTemplateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelTemplate}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelTemplate}" />
    public partial class BuyerChannelTemplateMap : AppEntityTypeConfiguration<BuyerChannelTemplate>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelTemplateMap() // elite group
        {
            this.ToTable("BuyerChannelTemplate");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}