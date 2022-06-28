// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 25-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 25-03-2021
// ***********************************************************************
// <copyright file="StaticPageCategoryRelationMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class BuyerChannelScheduleMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPageCategoryRelation}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.StaticPageCategoryRelation}" />
    public partial class StaticPageCategoryRelationMap : AppEntityTypeConfiguration<StaticPageCategoryRelation>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public StaticPageCategoryRelationMap() // elite group
        {
            this.ToTable("StaticPageCategoryRelation");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}