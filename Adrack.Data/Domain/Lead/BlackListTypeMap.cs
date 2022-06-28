// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BlackListTypeMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BlackListTypeMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BlackListType}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BlackListType}" />
    public partial class BlackListTypeMap : AppEntityTypeConfiguration<BlackListType>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BlackListTypeMap() // elite group
        {
            this.ToTable("BlackListType");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}