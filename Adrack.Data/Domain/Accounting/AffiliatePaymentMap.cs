// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliatePaymentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliatePaymentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliatePayment}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.AffiliatePayment}" />
    public partial class AffiliatePaymentMap : AppEntityTypeConfiguration<AffiliatePayment>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliatePaymentMap()
        {
            this.ToTable("AffiliatePayment");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}