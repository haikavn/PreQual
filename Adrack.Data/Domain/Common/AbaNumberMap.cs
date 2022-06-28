// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 05-05-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-05-2020
// ***********************************************************************
// <copyright file="AbaNumberMap.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;

namespace Adrack.Data.Domain.Common
{
    /// <summary>
    /// Represents a AbaNumber Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    public partial class AbaNumberMap : AppEntityTypeConfiguration<AbaNumber>
    {
        #region Constructor

        /// <summary>
        /// Navigation Map
        /// </summary>
        public AbaNumberMap()
        {
            this.ToTable("AbaNumber");

            this.HasKey(x => x.abanumber);
        }

        #endregion Constructor
    }
}