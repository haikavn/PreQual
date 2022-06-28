// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CustomBlackListValueMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class CustomBlackListValueMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CustomBlackListValue}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CustomBlackListValue}" />
    public partial class CustomBlackListValueMap : AppEntityTypeConfiguration<CustomBlackListValue>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public CustomBlackListValueMap() // elite group
        {
            this.ToTable("CustomBlackListValue");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}