// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ContainerManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.Core.Infrastructure.ApplicationDependency
{
    /// <summary>
    /// Represents a Container Manager
    /// </summary>
    public class ContainerManager
    {
        #region Fields

        /// <summary>
        /// Container
        /// </summary>
        private readonly IContainer _container;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Container Manager
        /// </summary>
        /// <param name="container">Container</param>
        public ContainerManager(IContainer container)
        {
            _container = container;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="key">Key</param>
        /// <param name="scope">Scope</param>
        /// <returns>T Collection</returns>
        public T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = Scope();
            }

            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }

            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope</param>
        /// <returns>Object</returns>
        public object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve All
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="key">Key</param>
        /// <param name="scope">Scop</param>
        /// <returns>T Collection</returns>
        public T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }

            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve Unregistered
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="scope">Scope</param>
        /// <returns>T Collection</returns>
        public T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// Resolve Unregistered
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope</param>
        /// <returns>T Collection</returns>
        /// <exception cref="Adrack.Core.AppException">
        /// Unkown application dependency
        /// or
        /// No contructor was found that had all the dependencies satisfied.
        /// </exception>
        public object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();

                    var parameterInstances = new List<object>();

                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);

                        if (service == null) throw new AppException("Unkown application dependency");

                        parameterInstances.Add(service);
                    }

                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (AppException)
                {
                }
            }

            throw new AppException("No contructor was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Try Resolve
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <param name="scope">Scope</param>
        /// <param name="instance">Instance</param>
        /// <returns>Boolean</returns>
        public bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// Is Registered
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <param name="scope">Scope</param>
        /// <returns>Boolean</returns>
        public bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve Optional
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <param name="scope">Scope</param>
        /// <returns>Object</returns>
        public object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }

            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Scope
        /// </summary>
        /// <returns>Lifetime Scope</returns>
        public ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Container
        /// </summary>
        /// <value>The container.</value>
        public IContainer Container
        {
            get { return _container; }
        }

        #endregion Properties
    }
}