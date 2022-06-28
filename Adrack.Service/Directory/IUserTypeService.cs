// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IUserTypeService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Directory;
using System.Collections.Generic;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Interface IUserTypeService
    /// </summary>
    public partial interface IUserTypeService
    {
        #region Methods

        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Country Item</returns>
        UserType GetUserTypeById(long Id);

        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Country Collection Item</returns>
        IList<UserType> GetAllUserTypes();

        /// <summary>
        /// Insert Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        void InsertUserType(UserType userType);

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        void UpdateUserType(UserType userType);

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="userType">Type of the user.</param>
        void DeleteUserType(UserType userType);

        #endregion Methods
    }
}