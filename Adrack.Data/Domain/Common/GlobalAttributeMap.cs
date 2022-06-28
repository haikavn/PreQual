// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="GlobalAttributeMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;

namespace Adrack.Data.Domain.Common
{
    /// <summary>
    /// Represents a Global Attribute Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.GlobalAttribute}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.GlobalAttribute}" />
    public partial class GlobalAttributeMap : AppEntityTypeConfiguration<GlobalAttribute>
    {
        #region Constructor

        /// <summary>
        /// Global Attribute Map
        /// </summary>
        public GlobalAttributeMap()
        {
            this.ToTable("GlobalAttribute");

            this.HasKey(x => x.Id);

            this.Property(x => x.KeyGroup).IsRequired().HasMaxLength(250);
            this.Property(x => x.Key).IsRequired().HasMaxLength(250);
            this.Property(x => x.Value).IsRequired().HasMaxLength(1000);
        }

        #endregion Constructor
    }
}