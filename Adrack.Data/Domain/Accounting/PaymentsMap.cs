// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 04-11-2020
//
// Last Modified By : Arman Zakaryna
// Last Modified On : 04-11-2020
// ***********************************************************************
// <copyright file="PaymentsMap.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Billing;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class PaymentsMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.Payments}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Accounting.Payments}" />
    public partial class PaymentsMap : AppEntityTypeConfiguration<Payment>
    {
        #region Constructor

        /// <summary>
        /// Payments Map
        /// </summary>
        public PaymentsMap()
        {
            this.ToTable("Payments");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}