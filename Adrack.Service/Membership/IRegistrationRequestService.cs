// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IRegistrationRequestService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a RegistrationRequest Service
    /// </summary>
    public partial interface IRegistrationRequestService
    {
        #region Methods

        /// <summary>
        /// Gets the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>IList&lt;RegistrationRequest&gt;.</returns>
        IList<RegistrationRequest> GetRegistrationRequest(string email);

        /// <summary>
        /// Gets the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="code">The code.</param>
        /// <returns>RegistrationRequest.</returns>
        RegistrationRequest GetRegistrationRequest(string email, string code);

        /// <summary>
        /// Inserts the registration request.
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        void InsertRegistrationRequest(RegistrationRequest registrationRequest);

        /// <summary>
        /// Deletes the registration request.
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        void DeleteRegistrationRequest(RegistrationRequest registrationRequest);

        /// <summary>
        /// Deletes the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        void DeleteRegistrationRequest(string email);

        #endregion Methods
    }
}