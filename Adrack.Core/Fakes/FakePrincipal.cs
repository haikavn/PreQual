// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakePrincipal.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Linq;
using System.Security.Principal;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Principal
    /// Implements the <see cref="System.Security.Principal.IPrincipal" />
    /// </summary>
    /// <seealso cref="System.Security.Principal.IPrincipal" />
    public class FakePrincipal : IPrincipal
    {
        #region Fields

        /// <summary>
        /// Identity
        /// </summary>
        private readonly IIdentity _identity;

        /// <summary>
        /// Role
        /// </summary>
        private readonly string[] _roles;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Principal
        /// </summary>
        /// <param name="identity">Identity</param>
        /// <param name="roles">The roles.</param>
        public FakePrincipal(IIdentity identity, string[] roles)
        {
            _identity = identity;
            _roles = roles;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Is In Role
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns><see langword="true" /> if the current principal is a member of the specified role; otherwise, <see langword="false" />.</returns>
        public bool IsInRole(string role)
        {
            return _roles != null && _roles.Contains(role);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Identity
        /// </summary>
        /// <value>The identity.</value>
        public IIdentity Identity
        {
            get { return _identity; }
        }

        #endregion Properties
    }
}