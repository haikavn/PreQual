// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAuthenticationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Authentication Service
    /// </summary>
    public partial interface IAuthenticationService
    {
        #region Methods

        /// <summary>
        /// Sign In
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="createPersistentCookie">Create Persistent Cookie</param>
        void SignIn(User user, bool createPersistentCookie);

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        /// <param name="_wepAppContext">The wep application context.</param>
        void SignOut(IAppContext _wepAppContext);

        /// <summary>
        /// Sign Out
        /// </summary>
        void SignOut();

        /// <summary>
        /// Get Authenticated User
        /// </summary>
        /// <returns>User Item</returns>
        User GetAuthenticatedUser();

        #endregion Methods
    }
}