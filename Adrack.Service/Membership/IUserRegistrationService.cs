// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IUserRegistrationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Registration Service
    /// </summary>
    public partial interface IUserRegistrationService
    {
        #region Methods

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="usernameOrEmail">Username Or Email</param>
        /// <param name="password">Password</param>
        /// <returns>User Login Result Item</returns>
        UserLoginResult ValidateUser(string usernameOrEmail, string password);

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userRegistrationRequest">User Registration Request</param>
        /// <param name="isInsert">if set to <c>true</c> [is insert].</param>
        /// <returns>User Registration Result Item</returns>
        UserRegistrationResult RegisterUser(UserRegistrationRequest userRegistrationRequest, bool isInsert);

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="changePasswordRequest">Change Password Request</param>
        /// <returns>Change Password Result Item</returns>
        ChangePasswordResult ChangePassword(ChangePasswordRequest changePasswordRequest);

        /// <summary>
        /// Set Username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        void SetUsername(User user, string newUsername);

        /// <summary>
        /// Set Email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New Email</param>
        void SetEmail(User user, string newEmail);

        #endregion Methods
    }
}