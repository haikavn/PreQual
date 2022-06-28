// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PaymentMethodMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class PaymentMethodMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PaymentMethod}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PaymentMethod}" />
    public partial class PaymentMethodMap : AppEntityTypeConfiguration<PaymentMethod>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PaymentMethodMap()
        {
            this.ToTable("PaymentMethod");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}