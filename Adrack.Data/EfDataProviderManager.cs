// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EfDataProviderManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure.Data;
using System;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Entity Framework Data Provider Manager
    /// Implements the <see cref="Adrack.Core.Infrastructure.Data.BaseDataProviderManager" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Data.BaseDataProviderManager" />
    public partial class EfDataProviderManager : BaseDataProviderManager
    {
        #region Constructor

        /// <summary>
        /// Entity Framework Data Provider Manager
        /// </summary>
        /// <param name="dataSetting">Data Setting</param>
        public EfDataProviderManager(DataSetting dataSetting)
            : base(dataSetting)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Load Data Provider
        /// </summary>
        /// <returns>Data Provider</returns>
        /// <exception cref="AppException">
        /// Data Setting doesn't contain a dataProviderName
        /// or
        /// </exception>
        public override IDataProvider LoadDataProvider()
        {
            var dataProviderName = Setting.DataProvider;

            if (String.IsNullOrWhiteSpace(dataProviderName))
                throw new AppException("Data Setting doesn't contain a dataProviderName");

            switch (dataProviderName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();

                default:
                    throw new AppException(string.Format("Not supported data provider name: {0}", dataProviderName));
            }
        }

        #endregion Methods
    }
}