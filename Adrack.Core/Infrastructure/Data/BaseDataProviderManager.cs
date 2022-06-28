// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BaseDataProviderManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Infrastructure.Data
{
    /// <summary>
    /// Represents a Base Data Provider Manager
    /// </summary>
    public abstract class BaseDataProviderManager
    {
        #region Constructor

        /// <summary>
        /// Base Data Provider Manager
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <exception cref="ArgumentNullException">setting</exception>
        protected BaseDataProviderManager(DataSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            this.Setting = setting;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Data Provider
        /// </summary>
        /// <returns>Data Provider</returns>
        public abstract IDataProvider LoadDataProvider();

        #endregion Methods

        #region Properties

        /// <summary>
        /// Data Setting
        /// </summary>
        /// <value>The setting.</value>
        protected DataSetting Setting { get; private set; }

        #endregion Properties
    }
}