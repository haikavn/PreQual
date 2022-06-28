// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Authentication.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Authentication
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Authentication : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the Identifier
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or Sets the Identifier Display
        /// </summary>
        /// <value>The identifier display.</value>
        public string IdentifierDisplay { get; set; }

        /// <summary>
        /// Gets or Sets the OAuth Token
        /// </summary>
        /// <value>The o authentication token.</value>
        public string OAuthToken { get; set; }

        /// <summary>
        /// Gets or Sets the OAuth Token Access
        /// </summary>
        /// <value>The o authentication token access.</value>
        public string OAuthTokenAccess { get; set; }

        /// <summary>
        /// Gets or Sets the Provider Key
        /// </summary>
        /// <value>The provider key.</value>
        public string ProviderKey { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

        #endregion Navigation Properties
    }
}