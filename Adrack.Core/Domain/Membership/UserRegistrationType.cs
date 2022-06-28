// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserRegistrationType.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a User Registration Type Enumeration
    /// </summary>
    public enum UserRegistrationType : int
    {
        #region Enumeration

        /// <summary>
        /// Standard account registration
        /// </summary>
        Standard = 100,

        /// <summary>
        /// Administrator Approval is required after registration
        /// </summary>
        AdministratorApproval = 200,

        /// <summary>
        /// Email Validation is required after registration
        /// </summary>
        EmailValidation = 300,

        /// <summary>
        /// Registration is disabled
        /// </summary>
        Disabled = 400,

        #endregion Enumeration
    }
}