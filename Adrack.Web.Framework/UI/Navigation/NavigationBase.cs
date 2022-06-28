// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="NavigationBase.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Navigation Base
    /// </summary>
    public abstract class NavigationBase
    {
        #region Fields

        /// <summary>
        /// Load
        /// </summary>
        /// <returns>Navigation Item Collection</returns>
        public abstract List<NavigationItem> Load();

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Navigation Base
        /// </summary>
        public NavigationBase()
        {
            Layout = string.Empty;
        }

        /// <summary>
        /// Navigation Base
        /// </summary>
        /// <param name="layout">Layout</param>
        public NavigationBase(string layout)
        {
            Layout = layout;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or Sets the Layout
        /// </summary>
        /// <value>The layout.</value>
        public string Layout { get; set; }

        #endregion Properties
    }
}