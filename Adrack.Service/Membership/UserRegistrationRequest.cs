// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserRegistrationRequest.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Registration Request
    /// </summary>
    public class UserRegistrationRequest
    {
        #region Constructor

        /// <summary>
        /// User Registration Request
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="username">Username</param>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <param name="comment">The comment.</param>
        /// <param name="approved">Approved</param>
        public UserRegistrationRequest(User user, string username, string email, string password, string comment, string contactemail, bool approved = true)
        {
            this.User = user;
            this.Username = username;
            this.Email = email;
            this.ContactEmail = contactemail;
            this.Password = password;
            this.Approved = approved;
            this.Comment = comment;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public User User { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the contact Email
        /// </summary>
        /// <value>The contact email.</value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets the Approved
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        public bool Approved { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        #endregion Properties
    }
}