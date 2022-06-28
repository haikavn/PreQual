// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PostedDataMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class PostedDataMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PostedData}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PostedData}" />
    public partial class PostedDataMap : AppEntityTypeConfiguration<PostedData>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PostedDataMap()
        {
            this.ToTable("PostedData");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}