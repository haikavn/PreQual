// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerPaymentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerPaymentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerPayment}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerPayment}" />
    public partial class BuyerPaymentMap : AppEntityTypeConfiguration<BuyerPayment>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerPaymentMap()
        {
            this.ToTable("BuyerPayment");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}