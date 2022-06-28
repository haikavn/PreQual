// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="NoteTitleMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class NoteTitleMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.NoteTitle}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.NoteTitle}" />
    public partial class NoteTitleMap : AppEntityTypeConfiguration<NoteTitle>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public NoteTitleMap()
        {
            this.ToTable("NoteTitle");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}