// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppEntityTypeConfiguration.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Application Entity Type Configuration
    /// Implements the <see cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{T}" />
    public abstract class AppEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        #region Constructor

        /// <summary>
        /// Application Entity Type Configuration
        /// </summary>
        protected AppEntityTypeConfiguration()
        {
            PostInitialize();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Post Initialize
        /// </summary>
        protected virtual void PostInitialize()
        {
        }

        #endregion Methods
    }
}