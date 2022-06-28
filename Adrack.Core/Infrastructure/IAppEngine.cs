// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IAppEngine.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.ApplicationDependency;
using Adrack.Core.Infrastructure.Configuration;
using System;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Application Engine (Classes implementing this interface can serve for the various services composing the application engine. Edit functionality, modules and implementations access most application functionality through this interface)
    /// </summary>
    public interface IAppEngine
    {
        #region Methods

        /// <summary>
        /// Initialize components in the App environment
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        void Initialize(AppConfiguration appConfiguration);

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T Class Collection</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Type</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve All
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T Class Collection</returns>
        T[] ResolveAll<T>();

        #endregion Methods

        #region Properties

        /// <summary>
        /// Container Manager
        /// </summary>
        /// <value>The container manager.</value>
        ContainerManager ContainerManager { get; }

        #endregion Properties
    }
}