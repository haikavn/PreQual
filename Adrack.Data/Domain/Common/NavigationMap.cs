// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="NavigationMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;

namespace Adrack.Data.Domain.Common
{
    /// <summary>
    /// Represents a Navigation Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    public partial class NavigationMap : AppEntityTypeConfiguration<Navigation>
    {
        #region Constructor

        /// <summary>
        /// Navigation Map
        /// </summary>
        public NavigationMap()
        {
            this.ToTable("Navigation");

            this.HasKey(x => x.Id);

            this.Property(x => x.ParentId).IsRequired();
            this.Property(x => x.Layout).IsRequired().HasMaxLength(100);
            this.Property(x => x.Key).IsRequired().HasMaxLength(250);
            this.Property(x => x.Controller).IsRequired().HasMaxLength(100);
            this.Property(x => x.Action).IsRequired().HasMaxLength(100);
            this.Property(x => x.Permission).IsRequired().HasMaxLength(100);
        }

        #endregion Constructor
    }
}