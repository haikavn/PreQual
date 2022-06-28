// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BlackListValueMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BlackListValueMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BlackListValue}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BlackListValue}" />
    public partial class BlackListValueMap : AppEntityTypeConfiguration<BlackListValue>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BlackListValueMap() // elite group
        {
            this.ToTable("BlackListValue");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}