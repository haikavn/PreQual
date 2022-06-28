// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using System.Collections.Generic;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a Country Service
    /// </summary>
    public partial interface ICountryService
    {
        #region Methods

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="countryId">Country Identifier</param>
        /// <returns>Country Item</returns>
        Country GetCountryById(long countryId);

        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Country Collection Item</returns>
        IList<Country> GetAllCountries();

        /// <summary>
        /// Insert Country
        /// </summary>
        /// <param name="country">Country</param>
        void InsertCountry(Country country);

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="country">Country</param>
        void UpdateCountry(Country country);

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="country">Country</param>
        void DeleteCountry(Country country);

        #endregion Methods
    }
}