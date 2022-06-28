// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ProfileService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Membership.IProfileService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IProfileService" />
    public partial class ProfileService : IProfileService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.Profile.By.Id-{0}";

        /// <summary>
        /// The cache profile by userid key
        /// </summary>
        private const string CACHE_PROFILE_BY_USERID_KEY = "App.Cache.Profile.By.UserId-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.Profile.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.Profile.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Profile> _profileRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="profileRepository">Profile Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public ProfileService(IRepository<Profile> profileRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._profileRepository = profileRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="profileId">Profile Identifier</param>
        /// <returns>Profile Item</returns>
        public virtual Profile GetProfileById(long profileId)
        {
            if (profileId == 0)
                return null;

            return _profileRepository.GetById(profileId);
        }

        /// <summary>
        /// Gets the profile by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Profile.</returns>
        public virtual Profile GetProfileByUserId(long userId)
        {
            if (userId == 0)
                return null;

            var query = from x in _profileRepository.Table
                        where x.UserId == userId
                        orderby x.Id
                        select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Profile> GetAllProfiles()
        {
            string key = CACHE_PROFILE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _profileRepository.Table
                            orderby x.Id
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        /// <exception cref="ArgumentNullException">profile</exception>
        public virtual void InsertProfile(Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            _profileRepository.Insert(profile);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(profile);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        /// <exception cref="ArgumentNullException">profile</exception>
        public virtual void UpdateProfile(Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            _profileRepository.Update(profile);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(profile);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        /// <exception cref="ArgumentNullException">profile</exception>
        public virtual void DeleteProfile(Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            _profileRepository.Delete(profile);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(profile);
        }

        #endregion Methods
    }
}