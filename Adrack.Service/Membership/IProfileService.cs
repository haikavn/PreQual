// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IProfileService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IProfileService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="profileId">Profile Identifier</param>
        /// <returns>Profile Item</returns>
        Profile GetProfileById(long profileId);

        /// <summary>
        /// Gets the profile by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Profile.</returns>
        Profile GetProfileByUserId(long userId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<Profile> GetAllProfiles();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void InsertProfile(Profile profile);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void UpdateProfile(Profile profile);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteProfile(Profile profile);

        #endregion Methods
    }
}