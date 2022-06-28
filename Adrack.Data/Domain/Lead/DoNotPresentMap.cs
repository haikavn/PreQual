// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DoNotPresentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class DoNotPresentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.DoNotPresent}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Filter}" />
    public partial class DoNotPresentMap : AppEntityTypeConfiguration<DoNotPresent>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public DoNotPresentMap() // elite group
        {
            this.ToTable("DoNotPresent");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}