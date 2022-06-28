// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="HistoryMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class HistoryMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.History}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.History}" />
    public partial class HistoryMap : AppEntityTypeConfiguration<History>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public HistoryMap()
        {
            this.ToTable("History");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}