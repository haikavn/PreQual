// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AppDomainTypeFinder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Application Domain Type Finder
    /// Implements the <see cref="Adrack.Core.Infrastructure.ITypeFinder" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.ITypeFinder" />
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region Fields

        /// <summary>
        /// Assembly Skip Loading Pattern
        /// </summary>
        private string assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        /// <summary>
        /// Ignore Reflection Errors
        /// </summary>
        private bool ignoreReflectionErrors = true;

        /// <summary>
        /// Load App Domain Assemblies
        /// </summary>
        private bool loadAppDomainAssemblies = true;

        /// <summary>
        /// Assembly Restrict To Loading Pattern
        /// </summary>
        private string assemblyRestrictToLoadingPattern = ".*";

        /// <summary>
        /// Assembly Names
        /// </summary>
        private IList<string> assemblyNames = new List<string>();

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list
        /// </summary>
        /// <param name="addedAssemblyNames">Added Assembly Names</param>
        /// <param name="assemblies">Assemblies</param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Matches(assembly.FullName))
                {
                    if (!addedAssemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);

                        addedAssemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Adds specificly configured assemblies
        /// </summary>
        /// <param name="addedAssemblyNames">Added Assembly Names</param>
        /// <param name="assemblies">Assemblies</param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (string assemblyName in AssemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);

                if (!addedAssemblyNames.Contains(assembly.FullName))
                {
                    assemblies.Add(assembly);

                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        /// <summary>
        /// Check if a dll is one of the shipped dlls that we know don't need to be investigated
        /// </summary>
        /// <param name="assemblyFullName">Assembly Full Name</param>
        /// <returns>True if the assembly should be loaded into App</returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern) && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        /// Check if a dll is one of the shipped dlls that we know don't need to be investigated
        /// </summary>
        /// <param name="assemblyFullName">Assembly Full Name</param>
        /// <param name="pattern">Pattern</param>
        /// <returns>True if the pattern matches the assembly name</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Makes sure matching assemblies in the supplied folder are loaded in the App Domain
        /// </summary>
        /// <param name="directoryPath">Directory Path</param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();

            foreach (Assembly x in GetAssemblies())
            {
                loadedAssemblyNames.Add(x.FullName);
            }

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);

                    if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        App.Load(an);
                    }
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Does Type Implement Open Generic
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="openGeneric">Open Generic</param>
        /// <returns>Boolean</returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();

                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());

                    return isMatch;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <param name="assignTypeFrom">Assign Type From</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        /// <summary>
        /// Find Classes Of Type
        /// </summary>
        /// <param name="assignTypeFrom">Assign Type From</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">Only Concrete Classes</param>
        /// <returns>Type Collection</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;

                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        if (!ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }

                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            {
                                if (!t.IsInterface)
                                {
                                    if (onlyConcreteClasses)
                                    {
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            result.Add(t);
                                        }
                                    }
                                    else
                                    {
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var exceptionMessage = string.Empty;

                foreach (var e in ex.LoaderExceptions)
                    exceptionMessage += e.Message + Environment.NewLine;

                var errorMessage = new Exception(exceptionMessage, ex);

                Debug.WriteLine(errorMessage.Message, errorMessage);

                throw errorMessage;
            }
            return result;
        }

        /// <summary>
        /// Get Assemblies
        /// </summary>
        /// <returns>Assembly Collection</returns>
        public virtual IList<Assembly> GetAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);

            AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// The app domain to look for types in
        /// </summary>
        /// <value>The application.</value>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>
        /// Gets or Sets wether App should iterate assemblies in the app domain when loading App types. Loading patterns are applied when loading these assemblies
        /// </summary>
        /// <value><c>true</c> if [load application domain assemblies]; otherwise, <c>false</c>.</value>
        public bool LoadAppDomainAssemblies
        {
            get { return loadAppDomainAssemblies; }
            set { loadAppDomainAssemblies = value; }
        }

        /// <summary>
        /// Gets or Sets thessemblies loaded a startup in addition to those loaded in the AppDomain
        /// </summary>
        /// <value>The assembly names.</value>
        public IList<string> AssemblyNames
        {
            get { return assemblyNames; }
            set { assemblyNames = value; }
        }

        /// <summary>
        /// Gets or Sets the pattern for dlls that we know don't need to be investigated
        /// </summary>
        /// <value>The assembly skip loading pattern.</value>
        public string AssemblySkipLoadingPattern
        {
            get { return assemblySkipLoadingPattern; }
            set { assemblySkipLoadingPattern = value; }
        }

        /// <summary>
        /// Gets or Sets the pattern for dll that will be investigated. For ease of use this defaults to match all but to increase performance you might want to configure a pattern that includes assemblies and your own
        /// </summary>
        /// <value>The assembly restrict to loading pattern.</value>
        public string AssemblyRestrictToLoadingPattern
        {
            get { return assemblyRestrictToLoadingPattern; }
            set { assemblyRestrictToLoadingPattern = value; }
        }

        #endregion Properties
    }
}