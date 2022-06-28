// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppEngineContext.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Application Engine Context (Provides access to the singleton instance of the App Engine)
    /// </summary>
    public class AppEngineContext
    {
        #region Methods

        /// <summary>
        /// Initializes a static instance of the application factory.
        /// </summary>
        /// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
        /// <returns>IAppEngine.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IAppEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IAppEngine>.Instance == null || forceRecreate)
            {
                Singleton<IAppEngine>.Instance = new AppEngine();

                var appConfiguration = ConfigurationManager.GetSection("AppConfiguration") as AppConfiguration;

                Singleton<IAppEngine>.Instance.Initialize(appConfiguration);
            }

            return Singleton<IAppEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="appEngine">Application Engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IAppEngine appEngine)
        {
            Singleton<IAppEngine>.Instance = appEngine;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets the singleton App Engine used to access application services.
        /// </summary>
        /// <value>The current.</value>
        public static IAppEngine Current
        {
            get
            {
                if (Singleton<IAppEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IAppEngine>.Instance;
            }
        }

        #endregion Properties
    }
}