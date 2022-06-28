// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-09-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-09-2021
// ***********************************************************************
// <copyright file="AddonService.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Membership.IAddonService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IAddonService" />
    public partial class AddonService : IAddonService
    {
        #region Constants
        private const string CACHE_ADDON_GetAllAddons = "App.Cache.Addon.GetAllAddons-{0}";
        private const string CACHE_ADDON_PATTERN_KEY = "App.Cache.Addon.";

        private const string CACHE_USERADDONS_GetUserAddons = "App.Cache.Addon.GetUserAddons";
        private const string CACHE_USERADDONS_GetAddonsByUserId = "App.Cache.Addon.GetAddonsByUserId-{0}";
        private const string CACHE_USERADDONS_GetUserAddon = "App.Cache.Addon.GetUserAddon-{0}-{1}";
        private const string CACHE_USERADDONS_PATTERN_KEY = "App.Cache.UserAddons.";

        private const string CACHE_PERMISSIONADDON_GetPermissionAddon = "App.Cache.Addon.GetPermissionAddon-{0}-{1}";
        private const string CACHE_PERMISSIONADDON_PATTERN_KEY = "App.Cache.PermissionAddon.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Addon
        /// </summary>
        private readonly IRepository<Addon> _addonRepository;

        /// <summary>
        /// UserAddons
        /// </summary>
        private readonly IRepository<UserAddons> _userAddonsRepository;

        /// <summary>
        /// PermissionAddon
        /// </summary>
        private readonly IRepository<PermissionAddon> _permissionAddonRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// IDataProvider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// ISettingService
        /// </summary>
        private readonly ISettingService _settingService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="addonRepository">The addon main repository.</param>
        /// <param name="userAddonsRepository">The user addons repository.</param>
        /// <param name="permissionAddonRepository">The permission addon repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        public AddonService(
                                IRepository<Addon> addonRepository,
                                IRepository<UserAddons> userAddonsRepository,
                                IRepository<PermissionAddon> permissionAddonRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher,
                                IDataProvider dataProvider,
                                ISettingService settingService
                                )
        {
            this._addonRepository = addonRepository;
            this._userAddonsRepository = userAddonsRepository;
            this._permissionAddonRepository = permissionAddonRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._settingService = settingService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Addons
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Addons Collection Item</returns>
        public virtual IList<Addon> GetAllAddons(short deleted = 0)
        {
            string key = string.Format(CACHE_ADDON_GetAllAddons, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _addonRepository.Table
                    where
                        (deleted == -1 || 
                         (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) ||
                         (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }

        /// <summary>
        /// Get Addon By Id
        /// </summary>
        /// <param name="id">Addon Id</param>
        /// <returns>Addon Item</returns>
        public virtual Addon GetAddonById(long id)
        {
            var query = from x in _addonRepository.Table
                where x.Id == id
                select x;

            var addon = query.FirstOrDefault();

            return addon;
        }

        /// <summary>
        /// Insert Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">addon</exception>
        public virtual long InsertAddon(Addon addon)
        {
            if (addon == null)
                throw new ArgumentNullException("addon");

            _addonRepository.Insert(addon);

            _cacheManager.RemoveByPattern(CACHE_ADDON_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(addon);

            return addon.Id;
        }


        /// <summary>
        /// Update Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <exception cref="ArgumentNullException">addon</exception>
        public virtual void UpdateAddon(Addon addon)
        {
            if (addon == null)
                throw new ArgumentNullException("addon");

            _addonRepository.Update(addon);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_ADDON_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(addon);
        }


        /// <summary>
        /// Delete Addon
        /// </summary>
        /// <param name="addon">The addon.</param>
        /// <exception cref="ArgumentNullException">addon</exception>
        public virtual void DeleteAddon(Addon addon)
        {
            if (addon == null)
                throw new ArgumentNullException("addon");

            addon.Deleted = true;
            _addonRepository.Update(addon);
            //_addonRepository.Delete(addon);

            _cacheManager.RemoveByPattern(CACHE_ADDON_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(addon);
        }
        #endregion Methods


        #region Methods UserAddons
        /// <summary>
        /// Get All User Addons
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        public virtual IList<UserAddons> GetAllUserAddons()
        {
            var query = from x in _userAddonsRepository.Table
                join a in _addonRepository.Table on x.AddonId equals a.Id
                where (a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue
                orderby x.Date descending
                select x;

            return query.ToList();
        }


        /// <summary>
        /// Get Addons By UserId
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        public virtual IList<UserAddons> GetAddonsByUserId(long id)
        {
            var query = from x in _userAddonsRepository.Table
                join a in _addonRepository.Table on x.AddonId equals a.Id
                where x.UserId == id && x.Status == 1 && ((a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue)
                orderby x.Date descending
                select x;
            
            return query.ToList();
        }

        /// <summary>
        /// Get Addons By UserId
        /// </summary>
        /// <returns>UserAddons Collection Item</returns>
        public virtual IList<Addon> GetActivatedAddonsByUserId(long id)
        {
            var query = from x in _addonRepository.Table
                        join a in _userAddonsRepository.Table on x.Id equals a.AddonId
                        where a.UserId == id && a.Status == 1 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)
                        orderby a.Date descending
                        select x;

            return query.ToList();
        }


        /// <summary>
        /// Get UserAddon
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="addonId">Addon Id</param>
        /// <returns>UserAddon Item</returns>
        public virtual UserAddons GetUserAddon(long userId, long addonId)
        {
            string key = string.Format(CACHE_USERADDONS_GetUserAddon, userId, addonId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _userAddonsRepository.Table
                    where x.UserId == userId && x.AddonId == addonId
                            orderby x.Date descending
                    select x;

                return query.FirstOrDefault();
            });
        }

        /// <summary>
        /// Update UserAddon
        /// </summary>
        /// <param name="userAddon">The userAddon.</param>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        public virtual void UpdateUserAddon(UserAddons userAddon)
        {
            if (userAddon == null)
                throw new ArgumentNullException("userAddon");

            _userAddonsRepository.Update(userAddon);

            _cacheManager.RemoveByPattern(CACHE_USERADDONS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userAddon);
        }


        /// <summary>
        /// Insert User Addon
        /// </summary>
        /// <param name="userAddon">The user's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        public virtual long InsertUserAddon(UserAddons userAddon)
        {
            if (userAddon == null)
                throw new ArgumentNullException("userAddon");

            _userAddonsRepository.Insert(userAddon);

            _cacheManager.RemoveByPattern(CACHE_USERADDONS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userAddon);

            return userAddon.Id;
        }


        /// <summary>
        /// Delete User Addon
        /// </summary>
        /// <param name="userAddon">The user's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userAddon</exception>
        public virtual void DeleteUserAddon(UserAddons userAddon)
        {
            if (userAddon == null)
                throw new ArgumentNullException("userAddon");

            _userAddonsRepository.Delete(userAddon);

            _cacheManager.RemoveByPattern(CACHE_USERADDONS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userAddon);
        }

        #endregion Methods


        #region Methods PermissionAddon

        /// <summary>
        /// Get All Permission Addons
        /// </summary>
        /// <returns>PermissionAddon Collection Item</returns>
        public virtual IList<PermissionAddon> GetAllPermissionAddons()
        {
            var query = from x in _permissionAddonRepository.Table
                join a in _addonRepository.Table on x.AddonId equals a.Id
                where (a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue
                orderby x.Id descending
                select x;

            return query.ToList();
        }


        /// <summary>
        /// Get Addons By PermissionId
        /// </summary>
        /// <returns>PermissionAddon Collection Item</returns>
        public virtual IList<PermissionAddon> GetAddonsByPermissionId(long id)
        {
            var query = from x in _permissionAddonRepository.Table
                join a in _addonRepository.Table on x.AddonId equals a.Id
                where x.PermissionId == id && ((a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue)
                orderby x.Id descending
                select x;

            return query.ToList();
        }

        public IList<long> GetPermissionIdsByAddonId(long id)
        {
            var query = from x in _permissionAddonRepository.Table
                        where x.AddonId == id
                        orderby x.Id descending
                        select x.PermissionId;
            return query.ToList();
        }


        /// <summary>
        /// Get PermissionAddon
        /// </summary>
        /// <param name="permissionId">Permission Id</param>
        /// <param name="addonId">Addon Id</param>
        /// <returns>PermissionAddon Item</returns>
        public virtual PermissionAddon GetPermissionAddon(long permissionId, long addonId)
        {
            string key = string.Format(CACHE_PERMISSIONADDON_GetPermissionAddon, permissionId, addonId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _permissionAddonRepository.Table
                            where x.PermissionId == permissionId && x.AddonId == addonId
                            orderby x.Id descending
                            select x;

                return query.FirstOrDefault();
            });
        }

        /// <summary>
        /// Insert Permission Addon
        /// </summary>
        /// <param name="permissionAddon">The permission's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">permissionAddon</exception>
        public virtual long InsertPermissionAddon(PermissionAddon permissionAddon)
        {
            if (permissionAddon == null)
                throw new ArgumentNullException("permissionAddon");

            _permissionAddonRepository.Insert(permissionAddon);

            _cacheManager.RemoveByPattern(CACHE_PERMISSIONADDON_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(permissionAddon);

            return permissionAddon.Id;
        }


        /// <summary>
        /// Delete Permission Addon
        /// </summary>
        /// <param name="permissionAddon">The permission's addon.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">permissionAddon</exception>
        public virtual void DeletePermissionAddon(PermissionAddon permissionAddon)
        {
            if (permissionAddon == null)
                throw new ArgumentNullException("permissionAddon");

            _permissionAddonRepository.Delete(permissionAddon);

            _cacheManager.RemoveByPattern(CACHE_PERMISSIONADDON_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(permissionAddon);
        }



        #endregion Methods
    }



}