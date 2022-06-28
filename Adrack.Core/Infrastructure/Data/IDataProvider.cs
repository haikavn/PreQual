// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IDataProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data.Common;

namespace Adrack.Core.Infrastructure.Data
{
    /// <summary>
    /// Represents a Data Provider
    /// </summary>
    public interface IDataProvider
    {
        #region Methods

        /// <summary>
        /// Initialize Connection Factory
        /// </summary>
        void InitializeConnectionFactory();

        /// <summary>
        /// Initialize Database
        /// </summary>
        void InitializeDatabase();

        /// <summary>
        /// Set Database Initializer
        /// </summary>
        void SetDatabaseInitializer();

        /// <summary>
        /// Gets a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        DbParameter GetParameter();

        #endregion Methods
    }
}