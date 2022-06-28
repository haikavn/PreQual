// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="INavigationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;
using System.Collections.Generic;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Navigation Service
    /// </summary>
    public partial interface INavigationService
    {
        #region Methods

        /// <summary>
        /// Get Navigation By Id
        /// </summary>
        /// <param name="navigationId">Navigation Identifier</param>
        /// <returns>Navigation Item</returns>
        Navigation GetNavigationById(long navigationId);

        /// <summary>
        /// Get Navigation By Layout
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns>Navigation Collection Item</returns>
        IList<Navigation> GetNavigationByLayout(string layout);

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="controler">The controler.</param>
        /// <param name="action">The action.</param>
        /// <param name="onlyPublished">if set to <c>true</c> [only published].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckPermission(string layout, string controler, string action, bool onlyPublished = true);

        /// <summary>
        /// Get All Navigations
        /// </summary>
        /// <returns>Navigation Collection Item</returns>
        IList<Navigation> GetAllNavigations();

        /// <summary>
        /// Insert Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        void InsertNavigation(Navigation navigation);

        /// <summary>
        /// Update Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        void UpdateNavigation(Navigation navigation);

        /// <summary>
        /// Delete Navigation
        /// </summary>
        /// <param name="navigation">Navigation</param>
        void DeleteNavigation(Navigation navigation);

        #endregion Methods
    }
}