// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailTokenDictionary.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a
    /// </summary>
    public partial interface IEmailTokenDictionary
    {
        #region Methods

        #region Application

        /// <summary>
        /// Add Application Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="smtpAccount">Smtp Account</param>
        void AddApplicationToken(IList<EmailToken> emailToken, SmtpAccount smtpAccount);

        #endregion Application

        #region Membership

        /// <summary>
        /// Add User Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="user">User</param>
        void AddUserToken(IList<EmailToken> emailToken, User user);


        /// <summary>
        /// Add User Invitation Token
        /// </summary>
        void AddUserInvitationToken(IList<EmailToken> emailToken, string email, string token, string inviterType, string inviterName);

        /// <summary>
        /// Add Addon Status
        /// </summary>
        void AddAddonStatus(IList<EmailToken> emailToken, string addonStatus);


        /// <summary>
        /// Adds the custom token.
        /// </summary>
        /// <param name="emailToken">The email token.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddCustomToken(IList<EmailToken> emailToken, string key, string value);

        #endregion Membership

        #region Message

        /// <summary>
        /// Add Email Subscription Token
        /// </summary>
        /// <param name="emailToken">Email Token</param>
        /// <param name="emailSubscription">Email Subscription</param>
        void AddEmailSubscriptionToken(IList<EmailToken> emailToken, EmailSubscription emailSubscription);

        #endregion Message

        #endregion Methods

        #region Email Token List

        /// <summary>
        /// Get Email Token List
        /// </summary>
        /// <returns>System.String[].</returns>
        string[] GetEmailTokenList();

        #endregion Email Token List
    }
}