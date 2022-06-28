// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CurrencySetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a Currency Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class CurrencySetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Display Currency Label
        /// </summary>
        /// <value>The display currency label.</value>
        public string DisplayCurrencyLabel { get; set; }

        /// <summary>
        /// Gets or Sets the Primary Exchange Rate Currency Identifier
        /// </summary>
        /// <value>The primary exchange rate currency identifier.</value>
        public int PrimaryExchangeRateCurrencyId { get; set; }

        /// <summary>
        /// Gets or Sets the Active Exchange Rate Provider Key
        /// </summary>
        /// <value>The active exchange rate provider key.</value>
        public string ActiveExchangeRateProviderKey { get; set; }

        /// <summary>
        /// Gets or Sets the Auto Update Enabled
        /// </summary>
        /// <value><c>true</c> if [automatic update enabled]; otherwise, <c>false</c>.</value>
        public bool AutoUpdateEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Last Update Time
        /// </summary>
        /// <value>The last update time.</value>
        public long LastUpdateTime { get; set; }

        #endregion Properties
    }
}