// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerInvoiceMapAdjustment.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerInvoiceAdjustmentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerInvoiceAdjustment}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerInvoiceAdjustment}" />
    public partial class BuyerInvoiceAdjustmentMap : AppEntityTypeConfiguration<BuyerInvoiceAdjustment>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerInvoiceAdjustmentMap()
        {
            this.ToTable("BuyerInvoiceAdjustment");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}