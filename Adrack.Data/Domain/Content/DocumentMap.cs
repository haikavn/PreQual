// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DocumentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Common
{
    /// <summary>
    /// Class DocumentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.Document}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.Document}" />
    public partial class DocumentMap : AppEntityTypeConfiguration<Document>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public DocumentMap()
        {
            this.ToTable("Document");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}