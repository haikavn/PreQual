// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateInvoiceMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateInvoiceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliateInvoice}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliateInvoice}" />
    public partial class AffiliateInvoiceMap : AppEntityTypeConfiguration<AffiliateInvoice>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateInvoiceMap()
        {
            this.ToTable("AffiliateInvoice");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}