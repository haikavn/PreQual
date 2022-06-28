// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppEngine.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.ApplicationDependency;
using Adrack.Core.Infrastructure.Configuration;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Application Engine
    /// Implements the <see cref="Adrack.Core.Infrastructure.IAppEngine" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.IAppEngine" />
    public class AppEngine : IAppEngine
    {
        #region Fields

        /// <summary>
        /// Container Manager
        /// </summary>
        private ContainerManager _containerManager;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Run Application Startup Tasks
        /// </summary>
        private void RunApplicationStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();

            var startUpTaskTypes = typeFinder.FindClassesOfType<IAppStartupTask>();

            var startUpTasks = new List<IAppStartupTask>();

            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IAppStartupTask)Activator.CreateInstance(startUpTaskType));

            // Sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();

            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        /// <summary>
        /// Application Dependencies
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        protected virtual void AppDependencies(AppConfiguration appConfiguration)
        {
            var containerBuilder = new ContainerBuilder();
            var container = containerBuilder.Build();
            this._containerManager = new ContainerManager(container);

            var typeFinder = new WebAppTypeFinder();
            containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(appConfiguration).As<AppConfiguration>().SingleInstance();
            containerBuilder.RegisterInstance(this).As<IAppEngine>().SingleInstance();
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            containerBuilder.Update(container);

            containerBuilder = new ContainerBuilder();

            var drTypes = typeFinder.FindClassesOfType<IAppDependency>();
            var drInstances = new List<IAppDependency>();

            foreach (var drType in drTypes)
                drInstances.Add((IAppDependency)Activator.CreateInstance(drType));

            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();

            foreach (var applicationDependency in drInstances)
                applicationDependency.Register(containerBuilder, typeFinder, appConfiguration);

            containerBuilder.Update(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="appConfiguration">Application Configuration</param>
        public void Initialize(AppConfiguration appConfiguration)
        {
            // Application Dependencies
            AppDependencies(appConfiguration);

            // Application Startup Tasks
            if (appConfiguration.ApplicationStartupTasksEnabled)
            {
                RunApplicationStartupTasks();
            }
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T Collection</returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// ResolveAll
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T Array</returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Container Manager
        /// </summary>
        /// <value>The container manager.</value>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion Properties
    }
}