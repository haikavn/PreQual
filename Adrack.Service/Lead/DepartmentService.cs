// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="DepartmentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class DepartmentService.
    /// Implements the <see cref="Adrack.Service.Lead.IDepartmentService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IDepartmentService" />
    public partial class DepartmentService : IDepartmentService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_DEPARTMENT_BY_ID_KEY = "App.Cache.Department.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_DEPARTMENT_ALL_KEY = "App.Cache.Department.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_DEPARTMENT_PATTERN_KEY = "App.Cache.Department.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Department> _departmentRepository;

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
        /// <param name="departmentRepository">The department repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public DepartmentService(IRepository<Department> departmentRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._departmentRepository = departmentRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Department GetDepartmentById(long affiliateId)
        {
            if (affiliateId == 0)
                return null;

            return _departmentRepository.GetById(affiliateId);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Department> GetAllDepartments()
        {
            string key = CACHE_DEPARTMENT_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _departmentRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">department</exception>
        public virtual long InsertDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException("department");

            _departmentRepository.Insert(department);

            _cacheManager.RemoveByPattern(CACHE_DEPARTMENT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(department);

            return department.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="department">The department.</param>
        /// <exception cref="ArgumentNullException">department</exception>
        public virtual void UpdateDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException("department");

            _departmentRepository.Update(department);

            _cacheManager.RemoveByPattern(CACHE_DEPARTMENT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(department);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="department">The department.</param>
        /// <exception cref="ArgumentNullException">department</exception>
        public virtual void DeleteDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException("department");

            _departmentRepository.Delete(department);

            _cacheManager.RemoveByPattern(CACHE_DEPARTMENT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(department);
        }

        #endregion Methods
    }
}