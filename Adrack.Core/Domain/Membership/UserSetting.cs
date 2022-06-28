// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a User Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class UserSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Username Enabled
        /// </summary>
        /// <value><c>true</c> if [username enabled]; otherwise, <c>false</c>.</value>
        public bool UsernameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Check Username Availability Enabled
        /// </summary>
        /// <value><c>true</c> if [check username availability enabled]; otherwise, <c>false</c>.</value>
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Allow User To Change Username
        /// </summary>
        /// <value><c>true</c> if [allow user to change username]; otherwise, <c>false</c>.</value>
        public bool AllowUserToChangeUsername { get; set; }

        /// <summary>
        /// Gets or Sets the User Registration Type
        /// </summary>
        /// <value>The type of the user registration.</value>
        public UserRegistrationType UserRegistrationType { get; set; }

        /// <summary>
        /// Gets or Sets the Minimum Required Password Length
        /// </summary>
        /// <value>The minimum length of the required password.</value>
        public int MinimumRequiredPasswordLength { get; set; }

        /// <summary>
        /// Gets or Sets the Maximum Invalid Password Attempts
        /// </summary>
        /// <value>The maximum invalid password attempts.</value>
        public int MaximumInvalidPasswordAttempts { get; set; }

        /// <summary>
        /// Gets or Sets the Maximum Login Attempts
        /// </summary>
        /// <value>The maximum login attempts.</value>
        public int MaximumLoginAttempts { get; set; }

        /// <summary>
        /// Gets or Sets the Cookie Expire Minutes
        /// </summary>
        /// <value>The cookie expire minutes.</value>
        public int CookieExpireMinutes { get; set; }

        /// <summary>
        /// Gets or Sets the Forgot Password Link Days Valid
        /// </summary>
        /// <value>The forgot password link days valid.</value>
        public int ForgotPasswordLinkDaysValid { get; set; }

        /// <summary>
        /// Gets or Sets the Last Visited Page
        /// </summary>
        /// <value><c>true</c> if [last visited page]; otherwise, <c>false</c>.</value>
        public bool LastVisitedPage { get; set; }

        #endregion Properties
    }
}