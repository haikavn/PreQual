// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageBundleOrderer.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Web.Optimization;

namespace Adrack.Web.Framework.UI
{
    /// <summary>
    /// Represents a Page Bundle Orderer
    /// Implements the <see cref="System.Web.Optimization.IBundleOrderer" />
    /// </summary>
    /// <seealso cref="System.Web.Optimization.IBundleOrderer" />
    public partial class PageBundleOrderer : IBundleOrderer
    {
        #region Methods

        /// <summary>
        /// Order Files
        /// </summary>
        /// <param name="bundleContext">Bundle Context</param>
        /// <param name="bundleFile">Bundle Files</param>
        /// <returns>Bundle File Collection Item</returns>
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext bundleContext, IEnumerable<BundleFile> bundleFile)
        {
            return bundleFile;
        }

        #endregion Methods
    }
}