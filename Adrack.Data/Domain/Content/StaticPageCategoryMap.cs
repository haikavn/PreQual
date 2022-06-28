// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 22-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 22-03-2021
// ***********************************************************************
// <copyright file="StaticPageCategoryMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class BuyerChannelScheduleMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPageCategory}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPageCategory}" />
    public partial class StaticPageCategoryMap : AppEntityTypeConfiguration<StaticPageCategory>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public StaticPageCategoryMap() // elite group
        {
            this.ToTable("StaticPageCategory");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}