// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserRegistrationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Message;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Registration Service
    /// Implements the <see cref="Adrack.Service.Membership.IUserRegistrationService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IUserRegistrationService" />
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        /// <summary>
        /// User Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Role Service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// Encryption Service
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// Localized String Service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// Email Subscription Service
        /// </summary>
        private readonly IEmailSubscriptionService _emailSubscriptionService;

        /// <summary>
        /// User Setting
        /// </summary>
        private readonly UserSetting _userSetting;

        private readonly ISettingService _settingService;

        private readonly EmailOperatorEnums _emailProvider;


        #endregion Fields

        #region Constructor

        /// <summary>
        /// User Registration Service
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="localizedStringService">The localized string service.</param>
        /// <param name="emailSubscriptionService">The email subscription service.</param>
        /// <param name="userSetting">The user setting.</param>
        public UserRegistrationService(IUserService userService, IRoleService roleService, IAffiliateService affiliateService, IBuyerService buyerService, IEncryptionService encryptionService, ILocalizedStringService localizedStringService, IEmailSubscriptionService emailSubscriptionService, UserSetting userSetting, ISettingService settingService)
        {
            this._userService = userService;
            this._roleService = roleService;
            this._affiliateService = affiliateService;
            this._buyerService = buyerService;
            this._encryptionService = encryptionService;
            this._localizedStringService = localizedStringService;
            this._emailSubscriptionService = emailSubscriptionService;
            this._userSetting = userSetting;
            this._settingService = settingService;

            var emailProviderSetting = _settingService.GetSetting("System.EmailProvider");

            _emailProvider = emailProviderSetting != null ? (EmailOperatorEnums)Convert.ToInt16(emailProviderSetting.Value) : EmailOperatorEnums.LeadNative;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="usernameOrEmail">Username Or Email</param>
        /// <param name="password">Password</param>
        /// <returns>User Login Result Item</returns>
        public virtual UserLoginResult ValidateUser(string usernameOrEmail, string password)
        {
            User user = _userService.GetUserByEmail(usernameOrEmail);
            if (user == null && usernameOrEmail.ToLower() == "admin")
                user = _userService.GetUserByUsername(usernameOrEmail);

            if (user == null)
                return UserLoginResult.UserNotExist;

            if (user.UserType != UserTypes.Super)
            {
                if (user.Deleted)
                    return UserLoginResult.Deleted;

                if (!user.Active)
                    return UserLoginResult.NotActive;

                if (user.LockedOut)
                    return UserLoginResult.LockedOut;
            }

            if (user.UserType != UserTypes.Super && user.Roles != null && user.Roles.Where(x => !x.Deleted && x.Active).Count() == 0)
            {
                return UserLoginResult.NotActive;
            }

            if (user.ValidateOnLogin)
            {
                IGlobalAttributeService _globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();
                IEmailService _emailService = AppEngineContext.Current.Resolve<IEmailService>();
                IAppContext _appContext = AppEngineContext.Current.Resolve<IAppContext>();

                List<GlobalAttribute> attributes = (List<GlobalAttribute>)_globalAttributeService.GetGlobalAttributeByEntityIdAndKeyGroup(user.Id, "User");
                if (attributes.Count == 0 || attributes[attributes.Count - 1].Value != user.GuId)
                {
                    _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, Guid.NewGuid().ToString());
                    _emailService.SendUserEmailValidationMessage(user, _appContext.AppLanguage.Id, _emailProvider);

                    return UserLoginResult.NotValidated;
                }
            }

            if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                Affiliate affiliate = _affiliateService.GetAffiliateById(user.ParentId, false);
                if (affiliate != null && affiliate.Status != 1) return UserLoginResult.NotActive;
            }
            else
                if (user.UserType == SharedData.BuyerUserTypeId)
            {
                Buyer buyer = _buyerService.GetBuyerById(user.ParentId);
                if (buyer != null && buyer.Status != 1) return UserLoginResult.NotActive;
            }

            var passwordHash = _encryptionService.CreatePasswordHash(password, user.SaltKey);

            bool isValid = passwordHash == user.Password;

            if (!isValid)
            {
                if (user.FailedPasswordAttemptCount == null)
                    user.FailedPasswordAttemptCount = 0;

                user.FailedPasswordAttemptCount++;

                _userService.UpdateUser(user);

                /*if (user.FailedPasswordAttemptCount >= _userSetting.MaximumInvalidPasswordAttempts && user.UserType != SharedData.BuiltInUserTypeId)
                {
                    user.LockedOut = true;
                    user.LockoutDate = DateTime.UtcNow;
                    user.FailedPasswordAttemptCount = 0;

                    _userService.UpdateUser(user);

                    return UserLoginResult.LockedOut;
                }*/

                return UserLoginResult.WrongPassword;
            }

            user.LoginDate = DateTime.UtcNow;
            user.FailedPasswordAttemptCount = 0;

            _userService.UpdateUser(user);

            return UserLoginResult.Successful;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userRegistrationRequest">User Registration Request</param>
        /// <param name="isInsert">if set to <c>true</c> [is insert].</param>
        /// <returns>User Registration Result Item</returns>
        /// <exception cref="ArgumentNullException">userRegistrationRequest</exception>
        /// <exception cref="ArgumentException">Can't load current user</exception>
        public virtual UserRegistrationResult RegisterUser(UserRegistrationRequest userRegistrationRequest, bool isInsert)
        {
            if (userRegistrationRequest == null)
                throw new ArgumentNullException("userRegistrationRequest");

            if (userRegistrationRequest.User == null)
                throw new ArgumentException("Can't load current user");

            var userRegistrationResult = new UserRegistrationResult();

            if (_userService.GetUserSearch(userRegistrationRequest.User.Id).Count > 0)
            {
                userRegistrationResult.AddError("Current user is already registered");

                return userRegistrationResult;
            }

            if (String.IsNullOrEmpty(userRegistrationRequest.Email))
            {
                userRegistrationResult.AddError(_localizedStringService.GetLocalizedString("Membership.Register.Error.EmailIsNotProvided"));

                return userRegistrationResult;
            }

            if (!CommonHelper.IsValidEmail(userRegistrationRequest.Email))
            {
                userRegistrationResult.AddError(_localizedStringService.GetLocalizedString("Common.WrongEmail"));

                return userRegistrationResult;
            }

            if (String.IsNullOrWhiteSpace(userRegistrationRequest.Password))
            {
                userRegistrationResult.AddError(_localizedStringService.GetLocalizedString("Membership.Register.Error.PasswordIsNotProvided"));

                return userRegistrationResult;
            }

            if (_userSetting.UsernameEnabled)
            {
                if (_userService.GetUserByUsername(userRegistrationRequest.Username) != null)
                {
                    userRegistrationResult.AddError(_localizedStringService.GetLocalizedString("Membership.Register.Error.UsernameAlreadyExists"));

                    return userRegistrationResult;
                }
            }

            if (_userService.GetUserByEmail(userRegistrationRequest.Email) != null)
            {
                userRegistrationResult.AddError(_localizedStringService.GetLocalizedString("Membership.Register.Error.EmailAlreadyExists"));

                return userRegistrationResult;
            }

            string saltKey = _encryptionService.CreateSaltKey(20);

            userRegistrationRequest.User.Username = userRegistrationRequest.Email;
            userRegistrationRequest.User.Email = userRegistrationRequest.Email;
            userRegistrationRequest.User.ContactEmail = userRegistrationRequest.ContactEmail;
            userRegistrationRequest.User.SaltKey = saltKey;
            userRegistrationRequest.User.Password = _encryptionService.CreatePasswordHash(userRegistrationRequest.Password, saltKey);
            userRegistrationRequest.User.Active = userRegistrationRequest.Approved;
            userRegistrationRequest.User.Comment = userRegistrationRequest.Comment;
            userRegistrationRequest.User.MaskEmail = true;

            if (!isInsert && userRegistrationRequest.User.Id != 0)
            {
                _userService.UpdateUser(userRegistrationRequest.User);
            }
            else
            {
                userRegistrationRequest.User.ActivityDate = DateTime.UtcNow;
                userRegistrationRequest.User.LockoutDate = DateTime.UtcNow;
                userRegistrationRequest.User.LoginDate = DateTime.UtcNow;
                userRegistrationRequest.User.PasswordChangedDate = DateTime.UtcNow;
                userRegistrationRequest.User.RegistrationDate = DateTime.UtcNow;
                userRegistrationRequest.User.GuId = Guid.NewGuid().ToString();
                userRegistrationRequest.User.MaskEmail = true;
                _userService.InsertUser(userRegistrationRequest.User);
            }

            return userRegistrationResult;
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="changePasswordRequest">Change Password Request</param>
        /// <returns>Change Password Result Item</returns>
        /// <exception cref="ArgumentNullException">changePasswordRequest</exception>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (changePasswordRequest == null)
                throw new ArgumentNullException("changePasswordRequest");

            var changePasswordResult = new ChangePasswordResult();

            if (String.IsNullOrWhiteSpace(changePasswordRequest.Email))
            {
                changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.EmailIsNotProvided"));

                return changePasswordResult;
            }

            if (String.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
            {
                changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.PasswordIsNotProvided"));

                return changePasswordResult;
            }

            var user = _userService.GetUserByEmail(changePasswordRequest.Email);

            if (user == null)
            {
                changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.EmailNotFound"));

                return changePasswordResult;
            }

            var changePasswordRequestIsValid = false;

            if (changePasswordRequest.ValidateRequest)
            {
                string oldPasswordHash = _encryptionService.CreatePasswordHash(changePasswordRequest.OldPassword, user.SaltKey);

                bool oldPasswordIsValid = oldPasswordHash == user.Password;

                if (!oldPasswordIsValid)
                    changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.OldPasswordDoesntMatch"));

                if (oldPasswordIsValid)
                    changePasswordRequestIsValid = true;
            }
            else
                changePasswordRequestIsValid = true;

            if (_encryptionService.CreatePasswordHash(changePasswordRequest.OldPassword, user.SaltKey) == _encryptionService.CreatePasswordHash(changePasswordRequest.NewPassword, user.SaltKey))
            {
                changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.OldPasswordMatch"));
                changePasswordRequestIsValid = false;
            }

            var verifyAccount = _userService.GetVerifyAccountByUserId(user.Id);

            if (verifyAccount != null & changePasswordRequestIsValid)
            {
                foreach (VerifyAccount item in verifyAccount)
                {
                    string verifyAccountPasswordHash = _encryptionService.CreatePasswordHash(changePasswordRequest.NewPassword, item.SaltKey);

                    if (verifyAccountPasswordHash == item.Password)
                    {
                        changePasswordResult.AddError(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Error.OldPasswordMatch"));
                        changePasswordRequestIsValid = false;

                        return changePasswordResult;
                    }
                }
            }

            if (changePasswordRequestIsValid)
            {
                VerifyAccount account = new VerifyAccount
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Password = _encryptionService.CreatePasswordHash(changePasswordRequest.OldPassword, user.SaltKey),
                    SaltKey = user.SaltKey,
                    CreatedOn = DateTime.UtcNow
                };

                _userService.InsertVerifyAccount(account);

                user.Password = _encryptionService.CreatePasswordHash(changePasswordRequest.NewPassword, user.SaltKey);
                user.PasswordChangedDate = DateTime.UtcNow;

                _userService.UpdateUser(user);
            }

            return changePasswordResult;
        }

        /// <summary>
        /// Set Username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        /// <exception cref="ArgumentNullException">user</exception>
        /// <exception cref="AppException">Username is disabled
        /// or
        /// Changing usernames is not allowed
        /// or
        /// or</exception>
        public virtual void SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!_userSetting.UsernameEnabled)
                throw new AppException("Username is disabled");

            if (!_userSetting.AllowUserToChangeUsername)
                throw new AppException("Changing usernames is not allowed");

            newUsername = newUsername.Trim();

            if (newUsername.Length > 50)
                throw new AppException(_localizedStringService.GetLocalizedString("Membership.Username.Error.UsernameTooLong"));

            var user2 = _userService.GetUserByUsername(newUsername);

            if (user2 != null && user.Id != user2.Id)
                throw new AppException(_localizedStringService.GetLocalizedString("Membership.Username.Error.UsernameAlreadyExists"));

            user.Username = newUsername;
            _userService.UpdateUser(user);
        }

        /// <summary>
        /// Set Email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New Email</param>
        /// <exception cref="ArgumentNullException">user</exception>
        /// <exception cref="AppException">
        /// </exception>
        public virtual void SetEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            newEmail = newEmail.Trim();

            string oldEmail = user.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new AppException(_localizedStringService.GetLocalizedString("Membership.Email.Error.NewEmailIsNotValid"));

            if (newEmail.Length > 100)
                throw new AppException(_localizedStringService.GetLocalizedString("Membership.Email.Error.EmailTooLong"));

            var user2 = _userService.GetUserByEmail(newEmail);

            if (user2 != null && user.Id != user2.Id)
                throw new AppException(_localizedStringService.GetLocalizedString("Membership.Email.Error.EmailAlreadyExists"));

            user.Email = newEmail;

            _userService.UpdateUser(user);

            if (!String.IsNullOrEmpty(oldEmail) && !oldEmail.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase))
            {
                var emailSubscriptionOld = _emailSubscriptionService.GetEmailSubscriptionByEmail(oldEmail);

                if (emailSubscriptionOld != null)
                {
                    emailSubscriptionOld.Email = newEmail;
                    _emailSubscriptionService.UpdateEmailSubscription(emailSubscriptionOld);
                }
            }
        }

        #endregion Methods
    }
}