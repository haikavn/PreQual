// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserLoginResult.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a User Login Result Enumeration
    /// </summary>
    public enum UserLoginResult
    {
        #region Enumeration

        /// <summary>
        /// Login Successful
        /// </summary>
        Successful = 100,

        /// <summary>
        /// User Does Not Exist
        /// </summary>
        UserNotExist = 200,

        /// <summary>
        /// Wrong Password
        /// </summary>
        WrongPassword = 300,

        /// <summary>
        /// User Acount Have Not Been Activated
        /// </summary>
        NotActive = 400,

        /// <summary>
        /// User Account Has Been Deleted
        /// </summary>
        Deleted = 500,

        /// <summary>
        /// User Is Not Registered
        /// </summary>
        NotRegistered = 600,

        /// <summary>
        /// User Is Locked Out
        /// </summary>
        LockedOut = 700,

        /// <summary>
        /// User Is Not Validated
        /// </summary>
        NotValidated = 800

        #endregion Enumeration
    }
}