// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AuthenticationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using System;
using System.Web;
using System.Web.Security;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Authentication Service
    /// Implements the <see cref="Adrack.Service.Membership.IAuthenticationService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IAuthenticationService" />
    public partial class AuthenticationService : IAuthenticationService
    {
        #region Fields

        /// <summary>
        /// User Setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// User Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Time Span
        /// </summary>
        private readonly TimeSpan _expirationTimeSpan;

        /// <summary>
        /// Http Context Base
        /// </summary>
        private readonly HttpContextBase _httpContextBase;

        /// <summary>
        /// User
        /// </summary>
        private User _cachedUser;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Authentication Service
        /// </summary>
        /// <param name="userSetting">User Setting</param>
        /// <param name="userService">User Service</param>
        /// <param name="httpContextBase">Http Context Base</param>
        public AuthenticationService(UserSetting userSetting, IUserService userService, HttpContextBase httpContextBase)
        {
            this._userSetting = userSetting;
            this._userService = userService;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
            this._httpContextBase = httpContextBase;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Sign In
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="createPersistentCookie">Create Persistent Cookie</param>
        public virtual void SignIn(User user, bool createPersistentCookie)
        {
            var utcDateTimeNow = DateTime.UtcNow.ToLocalTime();

            var formsAuthenticationTicket = new FormsAuthenticationTicket(
                1,
                _userSetting.UsernameEnabled ? user.Username : user.Email,
                utcDateTimeNow,
                utcDateTimeNow.Add(_expirationTimeSpan),
                createPersistentCookie,
                _userSetting.UsernameEnabled ? user.Username : user.Email,
                FormsAuthentication.FormsCookiePath);

            var formsAuthenticationEncryptedTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);

            var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, formsAuthenticationEncryptedTicket)
            {
                HttpOnly = true
            };

            if (formsAuthenticationTicket.IsPersistent)
            {
                httpCookie.Expires = formsAuthenticationTicket.Expiration;
            }

            httpCookie.Secure = FormsAuthentication.RequireSSL;
            httpCookie.Path = FormsAuthentication.FormsCookiePath;

            if (FormsAuthentication.CookieDomain != null)
            {
                httpCookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContextBase.Response.Cookies.Add(httpCookie);

            _cachedUser = user;

            try
            {
                HttpContext.Current.Request.Headers["CurrentUserId"] = user.Id.ToString();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Sign Out
        /// </summary>
        public virtual void SignOut()
        {
            _cachedUser = null;

            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Sign Out
        /// </summary>
        /// <param name="_wepAppContext">The wep application context.</param>
        public virtual void SignOut(IAppContext _wepAppContext)
        {
            _cachedUser = null;

            _wepAppContext.ClearUserCookie();

            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Get Authenticated User
        /// </summary>
        /// <returns>User Item</returns>
        public virtual User GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (_httpContextBase == null || _httpContextBase.Request == null || !_httpContextBase.Request.IsAuthenticated || !(_httpContextBase.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContextBase.User.Identity;

            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);

            if (user != null && user.Active && !user.Deleted)// && (user.IsMember() || user.IsContentManager() || user.IsGlobalAdministrator())) //HAYK
                _cachedUser = user;

            return _cachedUser;
        }

        /// <summary>
        /// Get Authenticated User From Ticket
        /// </summary>
        /// <param name="formsAuthenticationTicket">The forms authentication ticket.</param>
        /// <returns>User Item</returns>
        /// <exception cref="ArgumentNullException">formsAuthenticationTicket</exception>
        public virtual User GetAuthenticatedUserFromTicket(FormsAuthenticationTicket formsAuthenticationTicket)
        {
            if (formsAuthenticationTicket == null)
                throw new ArgumentNullException("formsAuthenticationTicket");

            var usernameOrEmail = formsAuthenticationTicket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;

            var user = _userSetting.UsernameEnabled ? _userService.GetUserByUsername(usernameOrEmail) : _userService.GetUserByEmail(usernameOrEmail);

            return user;
        }

        #endregion Methods
    }
}