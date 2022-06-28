// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 05-05-2020
//
// Last Modified By : Arman Zakaryan 
// Last Modified On : 05-05-2020
// ***********************************************************************
// <copyright file="INavigationService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;
using System.Collections.Generic;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a GeoZip Service
    /// </summary>
    public partial interface IGeoZipService
    {
        #region Methods

        /// <summary>
        /// Get All Navigations
        /// </summary>
        /// <returns>Navigation Collection Item</returns>
        GeoZip GetGeoDataByZip(int zip);

        AbaNumber GetBankByAbaNumber(long abanumber);

        #endregion Methods
    }
}