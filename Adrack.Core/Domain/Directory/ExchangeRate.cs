// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ExchangeRate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a Exchange Rate
    /// </summary>
    public partial class ExchangeRate
    {
        #region Constructor

        /// <summary>
        /// Exchange Rate
        /// </summary>
        public ExchangeRate()
        {
            Code = string.Empty;
            Rate = 1.0m;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String Item</returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.Code, this.Rate);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Code
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets the Rate
        /// </summary>
        /// <value>The rate.</value>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or Sets the Updated On
        /// </summary>
        /// <value>The updated on.</value>
        public DateTime? UpdatedOn { get; set; }

        #endregion Properties
    }
}