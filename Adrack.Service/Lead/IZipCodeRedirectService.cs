// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IZipCodeRedirectService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IZipCodeRedirectService
    /// </summary>
    public partial interface IZipCodeRedirectService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="zipCodeRedirectId">The zip code redirect identifier.</param>
        /// <returns>Profile Item</returns>
        ZipCodeRedirect GetZipCodeRedirectById(long zipCodeRedirectId);

        /// <summary>
        /// Gets the zip code redirect by zip code.
        /// </summary>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        IList<ZipCodeRedirect> GetZipCodeRedirectByZipCode(string zipCode);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<ZipCodeRedirect> GetAllZipCodeRedirects();

        /// <summary>
        /// Gets all zip code redirects.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        IList<ZipCodeRedirect> GetAllZipCodeRedirects(long buyerChannelId);

        /// <summary>
        /// Gets all zip code redirects.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <returns>IList&lt;ZipCodeRedirect&gt;.</returns>
        IList<ZipCodeRedirect> GetAllZipCodeRedirects(long buyerChannelId, string zipCode);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="zipCodeRedirect">The zip code redirect.</param>
        /// <returns>System.Int64.</returns>
        long InsertZipCodeRedirect(ZipCodeRedirect zipCodeRedirect);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="zipCodeRedirect">The zip code redirect.</param>
        void UpdateZipCodeRedirect(ZipCodeRedirect zipCodeRedirect);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteZipCodeRedirect(ZipCodeRedirect profile);

        #endregion Methods
    }
}