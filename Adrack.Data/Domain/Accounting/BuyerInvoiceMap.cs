// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerInvoiceMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerInvoiceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerInvoice}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.BuyerInvoice}" />
    public partial class BuyerInvoiceMap : AppEntityTypeConfiguration<BuyerInvoice>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerInvoiceMap()
        {
            this.ToTable("BuyerInvoice");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}