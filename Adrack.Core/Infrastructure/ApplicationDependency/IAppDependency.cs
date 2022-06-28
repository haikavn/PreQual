// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IAppDependency.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using Autofac;

namespace Adrack.Core.Infrastructure.ApplicationDependency
{
    /// <summary>
    /// Represents a Application Dependency
    /// </summary>
    public interface IAppDependency
    {
        #region Methods

        /// <summary>
        /// Register Application Dependency
        /// </summary>
        /// <param name="containerBuilder">Container Builder</param>
        /// <param name="typeFinder">Type Finder</param>
        /// <param name="appConfiguration">Application Configuration</param>
        void Register(ContainerBuilder containerBuilder, ITypeFinder typeFinder, AppConfiguration appConfiguration);

        #endregion Methods

        #region Properties

        /// <summary>
        /// Application Dependency Order
        /// </summary>
        /// <value>The order.</value>
        int Order { get; }

        #endregion Properties
    }
}