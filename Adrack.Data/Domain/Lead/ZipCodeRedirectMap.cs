// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ZipCodeRedirectMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class ZipCodeRedirectMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.ZipCodeRedirect}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.ZipCodeRedirect}" />
    public partial class ZipCodeRedirectMap : AppEntityTypeConfiguration<ZipCodeRedirect>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public ZipCodeRedirectMap()
        {
            this.ToTable("ZipCodeRedirect");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}