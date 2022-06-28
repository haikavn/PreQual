// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="NavigationManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Navigation Manager
    /// </summary>
    public class NavigationManager
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationManager"/> class.
        /// </summary>
        /// <param name="navigation">Navigation Base</param>
        public NavigationManager(NavigationBase navigation)
        {
            Builder = navigation;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Load
        /// </summary>
        /// <returns>Navigation Item Collection</returns>
        public List<NavigationItem> Load()
        {
            Child = Builder.Load();

            return Child;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Child
        /// </summary>
        /// <value>The child.</value>
        public List<NavigationItem> Child { get; set; }

        /// <summary>
        /// Gets or Sets the Builder
        /// </summary>
        /// <value>The builder.</value>
        public NavigationBase Builder { get; set; }

        #endregion Properties
    }
}