// ***********************************************************************
// Assembly         : Adrack.WebApi
// Author           : Adrack Team
// Created          : 11-12-2019
//
// Last Modified By : Sergey
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
using Adrack.WebApi.Controllers;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Core;
using System.Linq;
using System.Web.Mvc;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Core.Services;
using Adrack.WebApi.Infrastructure.Core.WrapperData;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Content;

namespace Adrack.WebApi.Infrastructure
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
        public virtual void Register(ContainerBuilder containerBuilder, ITypeFinder typeFinder,
            AppConfiguration appConfiguration)
        {
            #region Home

            containerBuilder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());
            containerBuilder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            containerBuilder.RegisterType<FileUploadService>().As<IFileUploadService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SupportPageService>().As<ISupportPageService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UsersExtensionService>().As<IUsersExtensionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<RolePermissionService>().As<IRolePermissionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SharedDataWrapper>().As<ISharedDataWrapper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SearchService>().As<ISearchService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LeadDemoModeService>().As<ILeadDemoModeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AddonService>().As<IAddonService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PlanService>().As<IPlanService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<StaticPagesService>().As<IStaticPagesService>().InstancePerLifetimeScope();

            #endregion Home

        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Application Dependency Order
        /// </summary>
        /// <value>The order.</value>
        public int Order => 3;

        #endregion Properties
    }
}