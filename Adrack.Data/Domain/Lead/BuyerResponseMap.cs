// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerResponseMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerResponseMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerResponse}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerResponse}" />
    public partial class BuyerResponseMap : AppEntityTypeConfiguration<BuyerResponse>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerResponseMap()
        {
            this.ToTable("BuyerResponse");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}