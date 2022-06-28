// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IDoNotPresentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface ICachedUrlService
    /// </summary>
    public partial interface ICachedUrlService
    {
        #region Methods

        /// <summary>
        /// Insert CachedUrl
        /// </summary>
        /// <param name="filter">The CachedUrl.</param>
        /// <returns>System.Int64.</returns>
        long InsertCachedUrl(CachedUrl cachedUrl);

        bool CheckCachedUrl(string url);

        void DeleteCheckCachedUrl(CachedUrl cachedUrl);

        void DeleteCheckCachedUrls();

        #endregion Methods
    }
}