// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailTokenDictionary.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Service.Common;
using Adrack.Service.Helpers;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Token Dictionary
    /// Implements the <see cref="Adrack.Service.Message.IEmailTokenDictionary" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailTokenDictionary" />
    public partial class EmailTokenDictionary : IEmailTokenDictionary
    {
        #region Fields

        /// <summary>
        /// Application Setting
        /// </summary>
        private readonly AppSetting _appSetting;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Token Dictionary
        /// </summary>
        /// <param name="appSetting">Application Setting</param>
        /// <param name="profileService">Profile Service</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public EmailTokenDictionary(AppSetting appSetting, IProfileService profileService, IAppEventPublisher appEventPublisher)
        {
            this._appSetting = appSetting;
            this._profileService = profileService;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        #region Application

        /// <summary>
        /// Add Application Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <exception cref="ArgumentException">smtpAccount</exception>
        public virtual void AddApplicationToken(IList<EmailToken> emailToken, SmtpAccount smtpAccount)
        {
            if (smtpAccount == null)
                throw new ArgumentException("smtpAccount");

            emailToken.Add(new EmailToken("Application.Name", _appSetting.Name));
            emailToken.Add(new EmailToken("Application.Url", _appSetting.Url, true));
            emailToken.Add(new EmailToken("Application.Email", smtpAccount.Email));
        }

        #endregion Application

        #region Membership

        /// <summary>
        /// Add User Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="user">User</param>
        /// <exception cref="ArgumentException">user</exception>
        /// <exception cref="Exception">Application Setting Url is invalid</exception>
        public virtual void AddUserToken(IList<EmailToken> emailToken, User user)
        {
            //_appSetting.Url = "http://livedemo.adrack.com";
            HttpRequestMessage httpRequestMessage = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            _appSetting.Url = httpRequestMessage.GetBaseUrl();
            //_appSetting.Url = _appSetting.Url.Replace(".live", ".xyz").Replace("-api.", ".");

            _appSetting.Url = HttpContext.Current.Request.Headers["Referer"];          

            if (user == null)
                throw new ArgumentException("user");

            var profileService = _profileService.GetProfileByUserId(user.Id);

            if (String.IsNullOrEmpty(_appSetting.Url))
                throw new Exception("Application Setting Url is invalid");

            emailToken.Add(new EmailToken("User.Email", user.Email));
            //emailToken.Add(new EmailToken("User.Email", user.Email));
            emailToken.Add(new EmailToken("User.Username", user.Username));

            if (profileService != null)
            {
                emailToken.Add(new EmailToken("User.FirstName", profileService.FirstName));
                emailToken.Add(new EmailToken("User.LastName", profileService.LastName));
            }
            
            string forgotPasswordUrl = string.Format("{0}forgotpassword/confirmation?token={1}&email={2}", (!_appSetting.Url.EndsWith("/") ? _appSetting.Url + "/" : _appSetting.Url), user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.ForgotPasswordToken), HttpUtility.UrlEncode(user.Email));
            emailToken.Add(new EmailToken("User.ForgotPasswordUrl", forgotPasswordUrl, true));

            //string changeFirstPasswordUrl = string.Format("{0}api/authentication/firstPasswordForm?token={1}&email={2}", (!_appSetting.Url.EndsWith("/") ? _appSetting.Url + "/" : _appSetting.Url), user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.FirstPasswordToken), HttpUtility.UrlEncode(user.Email));
            string changeFirstPasswordUrl = string.Format("{0}network/confirmation?token={1}&email={2}", (!_appSetting.Url.EndsWith("/") ? _appSetting.Url + "/" : _appSetting.Url), user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.FirstPasswordToken), HttpUtility.UrlEncode(user.Email));
            emailToken.Add(new EmailToken("User.ChangeFirstPasswordUrl", changeFirstPasswordUrl, true));

            string code = user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.MembershipActivationToken);
            //if (string.IsNullOrEmpty(code)) code = user.GuId;
            //HttpRequestMessage httpRequestMessage = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            string appUrl = httpRequestMessage.GetBaseUrl();
            string userActivationUrl = string.Format("{0}api/authentication/activation?token={1}&email={2}", (!appUrl.EndsWith("/") ? appUrl + "/" : appUrl), code, HttpUtility.UrlEncode(user.Email));
            emailToken.Add(new EmailToken("User.ActivationUrl", userActivationUrl, true));

            string loginUrl = string.Format("{0}login", _appSetting.Url);
            emailToken.Add(new EmailToken("User.LoginUrl", loginUrl, true));

            emailToken.Add(new EmailToken("User.ActivationCode", code, true));
        }




        /// <summary>
        /// Add User Invitation Token
        /// </summary>
        public virtual void AddUserInvitationToken(IList<EmailToken> emailToken, string email, string token, string inviterType, string inviterName)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("email");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token");

            //_appSetting.Url = "http://livedemo.adrack.com";

            HttpRequestMessage httpRequestMessage = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            /*AZ!
            _appSetting.Url = httpRequestMessage.GetBaseUrl();
            _appSetting.Url = _appSetting.Url.Replace(".live", ".xyz").Replace("-api", "");
            */
            //_appSetting.Url = "https://dev-lead-distribution.adrack.com/";
            _appSetting.Url = HttpContext.Current.Request.Headers["Referer"];


            emailToken.Add(new EmailToken("User.Type", inviterType));
            emailToken.Add(new EmailToken("User.Inviter", inviterName));

            string userInvitationUrl = string.Format("{0}user/confirmation?token={1}&email={2}", (!_appSetting.Url.EndsWith("/") ? _appSetting.Url + "/" : _appSetting.Url), token, HttpUtility.UrlEncode(email));
            emailToken.Add(new EmailToken("User.UserInvitationUrl", userInvitationUrl, true));
        }


        /// <summary>
        /// Add Addon Status
        /// </summary>
        public virtual void AddAddonStatus(IList<EmailToken> emailToken, string addonStatus)
        {
            emailToken.Add(new EmailToken("Addon.Status", addonStatus));
        }

        /// <summary>
        /// Adds the custom token.
        /// </summary>
        /// <param name="emailToken">The email token.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void AddCustomToken(IList<EmailToken> emailToken, string key, string value)
        {
            emailToken.Add(new EmailToken(key, value, true));
        }

        #endregion Membership

        #region Message

        /// <summary>
        /// Add Email Subscription Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <exception cref="ArgumentException">emailSubscription</exception>
        public virtual void AddEmailSubscriptionToken(IList<EmailToken> emailToken, EmailSubscription emailSubscription)
        {
            if (emailSubscription == null)
                throw new ArgumentException("emailSubscription");

            const string urlFormat = "{0}email-subscription/activation/{1}/{2}";

            string activationUrl = string.Format(urlFormat, _appSetting.Url, emailSubscription.GuId, "true");
            emailToken.Add(new EmailToken("EmailSubscription.ActivationUrl", activationUrl, true));

            string deactivationUrl = string.Format(urlFormat, _appSetting.Url, emailSubscription.GuId, "false");
            emailToken.Add(new EmailToken("EmailSubscription.DeactivationUrl", deactivationUrl, true));
        }

        #endregion Message

        #region Email Token List

        /// <summary>
        /// Get Email Token List
        /// </summary>
        /// <returns>System.String[].</returns>
        public virtual string[] GetEmailTokenList()
        {
            var emailToken = new List<string>
            {
                "%Application.Name%",
                "%Application.Url%",
                "%Application.Email%",
                "%User.Email%",
                "%User.Username%",
                "%User.FullName%",
                "%User.ForgotPasswordUrl%",
                "%User.ActivationUrl%",
                "%EmailSubscription.ActivationUrl%",
                "%EmailSubscription.DeactivationUrl%",
            };

            return emailToken.ToArray();
        }

        #endregion Email Token List

        #endregion Methods
    }
}