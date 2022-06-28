// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplateMatchingMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelTemplateMatchingMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelTemplateMatching}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelTemplateMatching}" />
    public partial class BuyerChannelTemplateMatchingMap : AppEntityTypeConfiguration<BuyerChannelTemplateMatching>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelTemplateMatchingMap() // elite group
        {
            this.ToTable("BuyerChannelTemplateMatching");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}