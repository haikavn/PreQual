// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FilterMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class FilterMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Filter}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Filter}" />
    public partial class FilterMap : AppEntityTypeConfiguration<Filter>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public FilterMap() // elite group
        {
            this.ToTable("Filter");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}