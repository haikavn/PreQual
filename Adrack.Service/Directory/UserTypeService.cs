// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserTypeService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Cache;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Class UserTypeService.
    /// Implements the <see cref="Adrack.Service.Directory.IUserTypeService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Directory.IUserTypeService" />
    public partial class UserTypeService : IUserTypeService
    {
        #region Constants

        /// <summary>
        /// Cache Country By Id Key
        /// </summary>
        private const string CACHE_USER_TYPE_BY_ID_KEY = "App.Cache.User.Type.By.Id-{0}";

        /// <summary>
        /// Cache Country All Key
        /// </summary>
        private const string CACHE_USER_TYPE_ALL_KEY = "App.Cache.User.Type.All-{0}";

        /// <summary>
        /// Cache Country Pattern Key
        /// </summary>
        private const string CACHE_USER_TYPE_PATTERN_KEY = "App.Cache.User.Type.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Country
        /// </summary>
        private readonly IRepository<UserType> _userTypeRepository;

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
        /// Country Service
        /// </summary>
        /// <param name="userTypeRepository">The user type repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public UserTypeService(IRepository<UserType> userTypeRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._userTypeRepository = userTypeRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Country Item</returns>
        public virtual UserType GetUserTypeById(long Id)
        {
            if (Id == 0)
                return null;

            string key = string.Format(CACHE_USER_TYPE_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () => { return _userTypeRepository.GetById(Id); });
        }

        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Country Collection Item</returns>
        public virtual IList<UserType> GetAllUserTypes()
        {
            string key = CACHE_USER_TYPE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _userTypeRepository.Table
                            orderby x.DisplayOrder descending, x.Name
                            select x;

                var country = query.ToList();

                return country;
            });
        }

        /// <summary>
        /// Insert Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        /// <exception cref="ArgumentNullException">user type</exception>
        public virtual void InsertUserType(UserType userType)
        {
            if (userType == null)
                throw new ArgumentNullException("user type");

            _userTypeRepository.Insert(userType);

            _cacheManager.RemoveByPattern(CACHE_USER_TYPE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userType);
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        /// <exception cref="ArgumentNullException">userType</exception>
        public virtual void UpdateUserType(UserType userType)
        {
            if (userType == null)
                throw new ArgumentNullException("userType");

            _userTypeRepository.Update(userType);

            _cacheManager.RemoveByPattern(CACHE_USER_TYPE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(userType);
        }

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        /// <exception cref="ArgumentNullException">userType</exception>
        public virtual void DeleteUserType(UserType userType)
        {
            if (userType == null)
                throw new ArgumentNullException("userType");

            _userTypeRepository.Delete(userType);

            _cacheManager.RemoveByPattern(CACHE_USER_TYPE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(userType);
        }

        #endregion Methods
    }
}