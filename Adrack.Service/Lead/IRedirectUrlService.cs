// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IRedirectUrlService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IRedirectUrlService
    /// </summary>
    public partial interface IRedirectUrlService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="redirectUrlId">The redirect URL identifier.</param>
        /// <returns>Profile Item</returns>
        RedirectUrl GetRedirectUrlById(long redirectUrlId);

        /// <summary>
        /// Gets the redirect URL by lead identifier.
        /// </summary>
        /// <param name="redirectUrlId">The redirect URL identifier.</param>
        /// <returns>RedirectUrl.</returns>
        RedirectUrl GetRedirectUrlByLeadId(long redirectUrlId);

        /// <summary>
        /// Gets the redirect URL by key.
        /// </summary>
        /// <param name="navkey">The navkey.</param>
        /// <returns>RedirectUrl.</returns>
        RedirectUrl GetRedirectUrlByKey(string navkey);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<RedirectUrl> GetAllRedirectUrls();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>System.Int64.</returns>
        long InsertRedirectUrl(RedirectUrl redirectUrl);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        void UpdateRedirectUrl(RedirectUrl redirectUrl);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteRedirectUrl(RedirectUrl profile);

        #endregion Methods
    }
}