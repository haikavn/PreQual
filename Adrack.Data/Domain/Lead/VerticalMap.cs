// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerticalMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class VerticalMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Vertical}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Vertical}" />
    public partial class VerticalMap : AppEntityTypeConfiguration<Vertical>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public VerticalMap() // elite group
        {
            this.ToTable("Vertical");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}