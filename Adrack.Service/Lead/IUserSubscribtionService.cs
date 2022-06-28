// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IUserSubscribtionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IUserSubscribtionService
    /// </summary>
    public partial interface IUserSubscribtionService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        UserSubscribtion GetUserSubscribtionById(long Id);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<UserSubscribtion> GetUserSubscribtions();

        /// <summary>
        /// Gets the user subscribtions.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>IList&lt;UserSubscribtion&gt;.</returns>
        IList<UserSubscribtion> GetUserSubscribtions(long userId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        /// <returns>System.Int64.</returns>
        long InsertUserSubscribtion(UserSubscribtion userSubscribtion);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        void UpdateUserSubscribtion(UserSubscribtion userSubscribtion);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="userSubscribtion">The user subscribtion.</param>
        void DeleteUserSubscribtion(UserSubscribtion userSubscribtion);

        #endregion Methods
    }
}