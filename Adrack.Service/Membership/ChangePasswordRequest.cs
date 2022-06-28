// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ChangePasswordRequest.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Change Password Request
    /// </summary>
    public class ChangePasswordRequest
    {
        #region Constructor

        /// <summary>
        /// Change Password Request
        /// </summary>
        /// <param name="validateRequest">Validate Request</param>
        /// <param name="email">Email</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="oldPassword">Old Password</param>
        public ChangePasswordRequest(bool validateRequest, string email, string newPassword, string oldPassword = "")
        {
            this.ValidateRequest = validateRequest;
            this.Email = email;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Validate Request
        /// </summary>
        /// <value><c>true</c> if [validate request]; otherwise, <c>false</c>.</value>
        public bool ValidateRequest { get; set; }

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the New Password
        /// </summary>
        /// <value>The new password.</value>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or Sets the Old Password
        /// </summary>
        /// <value>The old password.</value>
        public string OldPassword { get; set; }

        #endregion Properties
    }
}