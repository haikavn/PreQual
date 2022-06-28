// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
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
using Adrack.Web.ContentManagement.Controllers;
using Autofac;
using Autofac.Core;

namespace Adrack.Web.ContentManagement.Infrastructure
{
    /// <summary>
    /// Represents a Application Dependency
    /// Implements the <see cref="Adrack.Core.Infrastructure.ApplicationDependency.IAppDependency" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.ApplicationDependency.IAppDependency" />
    public class AppDependency : IAppDependency
    {
        #region Methods

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="containerBuilder">Container Builder</param>
        /// <param name="typeFinder">Type Finder</param>
        /// <param name="appConfiguration">Application Configuration</param>
        public virtual void Register(ContainerBuilder containerBuilder, ITypeFinder typeFinder, AppConfiguration appConfiguration)
        {
            #region Home

            containerBuilder.RegisterType<HomeController>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static"));

            #endregion Home

        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Application Dependency Order
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return 2; }
        }

        #endregion Properties
    }
}