// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 06-02-2022
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 06-02-2022
// ***********************************************************************
// <copyright file="CustomInvoice.cs" company="Adrack.com">
//     Copyright © 2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class CustomInvoiceRow.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class CustomInvoiceRow : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>CustomInvoiceRow</returns>
        public object Clone()
        {
            var cInvoiceRow = new CustomInvoiceRow()
            {
            };

            return cInvoiceRow;
        }

        #endregion Methods

        #region Properties


        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The long.</value>
        public long CustomInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public int qty { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public double unitPrice { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public double amount { get; set; }

        #endregion Properties
    }
}