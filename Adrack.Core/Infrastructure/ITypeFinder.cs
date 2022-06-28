// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ITypeFinder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Type Finder
    /// </summary>
    public interface ITypeFinder
    {
        #region Methods

        /// <summary>
        /// Get Assemblies
        /// </summary>
        /// <returns>Assembly Collection</returns>
        IList<Assembly> GetAssemblies();

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <param name="assignTypeFrom">Assign Type From</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <param name="assignTypeFrom">Assign Type From</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        #endregion Methods
    }
}