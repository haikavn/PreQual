// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DataSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Adrack.Core.Infrastructure.Data
{
    /// <summary>
    /// Represents a Data Setting
    /// </summary>
    public partial class DataSetting
    {
        #region Constructor

        /// <summary>
        /// Data Setting
        /// </summary>
        public DataSetting()
        {
            RawDataSetting = new Dictionary<string, string>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Check Data Connection String
        /// </summary>
        /// <returns>Boolean</returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Data Provider
        /// </summary>
        /// <value>The data provider.</value>
        public string DataProvider { get; set; }

        /// <summary>
        /// Gets or Sets the Data Connection String
        /// </summary>
        /// <value>The data connection string.</value>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// Gets or Sets the Raw Data Setting
        /// </summary>
        /// <value>The raw data setting.</value>
        public IDictionary<string, string> RawDataSetting { get; private set; }

        #endregion Properties
    }
}