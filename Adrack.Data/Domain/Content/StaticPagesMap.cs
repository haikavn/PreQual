// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 17-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 17-03-2021
// ***********************************************************************
// <copyright file="StaticPagesMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class BuyerChannelScheduleMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPages}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPages}" />
    public partial class StaticPagesMap : AppEntityTypeConfiguration<StaticPages>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public StaticPagesMap() // elite group
        {
            this.ToTable("StaticPages");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}