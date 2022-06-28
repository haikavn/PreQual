// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 01-06-2022
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 01-06-2022
// ***********************************************************************
// <copyright file="AffiliateInvoiceMap.cs" company="Adrack.com">
//     Copyright © 2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class CustomInvoiceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.CustomInvoice}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.CustomInvoice}" />
    public partial class CustomInvoiceMap : AppEntityTypeConfiguration<CustomInvoice>
    {
        #region Constructor

        /// <summary>
        /// CustomInvoice Map
        /// </summary>
        public CustomInvoiceMap()
        {
            this.ToTable("CustomInvoice");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}