// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IDepartmentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IDepartmentService
    /// </summary>
    public partial interface IDepartmentService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <returns>Profile Item</returns>
        Department GetDepartmentById(long departmentId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<Department> GetAllDepartments();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>System.Int64.</returns>
        long InsertDepartment(Department department);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="department">The department.</param>
        void UpdateDepartment(Department department);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="department">The department.</param>
        void DeleteDepartment(Department department);

        #endregion Methods
    }
}