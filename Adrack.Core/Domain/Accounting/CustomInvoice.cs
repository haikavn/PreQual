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
    public partial class CustomInvoice : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>CustomInvoice</returns>
        public object Clone()
        {
            var cInvoice = new CustomInvoice()
            {
            };

            return cInvoice;
        }

        #endregion Methods

        #region Properties


        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string dateOfIssue { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string dateOfDue { get; set; }
        
        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string billingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string website { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string address { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string contactInformation { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string billingFullName { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string billingAddress { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public string billingContactInformation { get; set; }

        /// <summary>
        /// Gets or sets the string.
        /// </summary>
        /// <value>The string.</value>
        public double total { get; set; }
        
        #endregion Properties
    }
}