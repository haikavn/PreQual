// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using System;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Extensions
    /// </summary>
    public static class UserExtensions
    {
        #region Methods

        /// <summary>
        /// Get Full Name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>String</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public static string GetFullName(this User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var profileService = AppEngineContext.Current.Resolve<IProfileService>().GetProfileByUserId(user.Id);

            string fullName = "";

            if (profileService != null)
            {
                if (!String.IsNullOrWhiteSpace(profileService.FirstName) && !String.IsNullOrWhiteSpace(profileService.LastName))
                {
                    fullName = string.Format("{0} {1}", profileService.FirstName, profileService.LastName);
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(profileService.FirstName))
                        fullName = profileService.FirstName;

                    if (!String.IsNullOrWhiteSpace(profileService.LastName))
                        fullName = profileService.LastName;
                }
            }
            else
            {
                fullName = user.Username;
            }

            return fullName;
        }

        /// <summary>
        /// Is Forgot Password Token Valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token</param>
        /// <returns>Boolean Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public static bool IsForgotPasswordTokenValid(this User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var forgotPasswordToken = user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.ForgotPasswordToken);

            if (String.IsNullOrEmpty(forgotPasswordToken))
                return false;

            if (!forgotPasswordToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Is Forgot Password Link Expired
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userSetting">User Setting</param>
        /// <returns>Boolean Iem</returns>
        /// <exception cref="ArgumentNullException">user
        /// or
        /// userSetting</exception>
        public static bool IsForgotPasswordLinkExpired(this User user, UserSetting userSetting)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (userSetting == null)
                throw new ArgumentNullException("userSetting");

            if (userSetting.ForgotPasswordLinkDaysValid == 0)
                return false;

            var forgotPasswordTokenRequested = user.GetGlobalAttribute<DateTime?>(GlobalAttributeBuiltIn.ForgotPasswordTokenRequestedDate);

            if (!forgotPasswordTokenRequested.HasValue)
                return false;

            var daysPassed = (DateTime.UtcNow - forgotPasswordTokenRequested.Value).TotalDays;

            if (daysPassed > userSetting.ForgotPasswordLinkDaysValid)
                return true;

            return false;
        }



        /// <summary>
        /// Is First Password Token Valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token</param>
        /// <returns>Boolean Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public static bool IsFirstPasswordTokenValid(this User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var firstPasswordToken = user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.FirstPasswordToken);

            if (String.IsNullOrEmpty(firstPasswordToken))
                return false;

            if (!firstPasswordToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

 
        #endregion Methods
    }
}