// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FilterConditionMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class FilterConditionMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.FilterCondition}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.FilterCondition}" />
    public partial class FilterConditionMap : AppEntityTypeConfiguration<FilterCondition>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public FilterConditionMap() // elite group
        {
            this.ToTable("FilterCondition");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}