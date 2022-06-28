// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppDependency.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.ApplicationDependency;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Web.Controllers;
using Autofac;
using Autofac.Core;

namespace Adrack.Web
{
    /// <summary>
    ///     Represents a Application Dependency
    ///     Implements the <see cref="IAppDependency" />
    /// </summary>
    /// <seealso cref="IAppDependency" />
    public class AppDependency : IAppDependency
    {
        #region Methods

        /// <summary>
        ///     Register
        /// </summary>
        /// <param name="containerBuilder">Container Builder</param>
        /// <param name="typeFinder">Type Finder</param>
        /// <param name="appConfiguration">Application Configuration</param>
        public virtual void Register(ContainerBuilder containerBuilder, ITypeFinder typeFinder,
            AppConfiguration appConfiguration)
        {
            #region Common

            containerBuilder.RegisterType<CommonController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static"));

            #endregion Common

            #region Directory

            containerBuilder.RegisterType<DirectoryController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static"));

            #endregion Directory
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Application Dependency Order
        /// </summary>
        /// <value>The order.</value>
        public int Order => 2;

        #endregion Properties
    }
}