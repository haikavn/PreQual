// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="WebAppTypeFinder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Web Application Type Finder
    /// Implements the <see cref="Adrack.Core.Infrastructure.AppDomainTypeFinder" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.AppDomainTypeFinder" />
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        /// <summary>
        /// Ensure Bin Folder Assemblies Loaded
        /// </summary>
        private bool _ensureBinFolderAssembliesLoaded = true;

        /// <summary>
        /// Bin Folder Assemblies Loaded
        /// </summary>
        private bool _binFolderAssembliesLoaded;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get Bin Directory
        /// </summary>
        /// <returns>String</returns>
        public virtual string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                return HttpRuntime.BinDirectory;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Get Assemblies
        /// </summary>
        /// <returns>List Connection</returns>
        public override IList<Assembly> GetAssemblies()
        {
            if (this.EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded)
            {
                _binFolderAssembliesLoaded = true;

                string binPath = GetBinDirectory();

                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Ensure Bin Folder Assemblies Loaded
        /// </summary>
        /// <value><c>true</c> if [ensure bin folder assemblies loaded]; otherwise, <c>false</c>.</value>
        public bool EnsureBinFolderAssembliesLoaded
        {
            get { return _ensureBinFolderAssembliesLoaded; }
            set { _ensureBinFolderAssembliesLoaded = value; }
        }

        #endregion Properties
    }
}