// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateInvoiceAdjustmentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateInvoiceAdjustmentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliateInvoiceAdjustment}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliateInvoiceAdjustment}" />
    public partial class AffiliateInvoiceAdjustmentMap : AppEntityTypeConfiguration<AffiliateInvoiceAdjustment>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateInvoiceAdjustmentMap()
        {
            this.ToTable("AffiliateInvoiceAdjustment");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}