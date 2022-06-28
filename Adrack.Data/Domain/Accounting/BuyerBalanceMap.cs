// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerBalanceMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerBalanceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerBalance}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerBalance}" />
    public partial class BuyerBalanceMap : AppEntityTypeConfiguration<BuyerBalance>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerBalanceMap()
        {
            this.ToTable("BuyerBalance");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}