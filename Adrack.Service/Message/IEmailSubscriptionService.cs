// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailSubscriptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Subscription Service
    /// </summary>
    public partial interface IEmailSubscriptionService
    {
        #region Methods

        /// <summary>
        /// Get Email Subscription By Id
        /// </summary>
        /// <param name="emailSubscriptionId">Email Subscription Identifier</param>
        /// <returns>Email Subscription Item</returns>
        EmailSubscription GetEmailSubscriptionById(long emailSubscriptionId);

        /// <summary>
        /// Get Email Subscription By GuId
        /// </summary>
        /// <param name="guId">Globally Unique Identifier</param>
        /// <returns>Email Subscription Item</returns>
        EmailSubscription GetEmailSubscriptionByGuId(string guId);

        /// <summary>
        /// Get Email Subscription By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Email Subscription Item</returns>
        EmailSubscription GetEmailSubscriptionByEmail(string email);

        /// <summary>
        /// Get All Email Subscriptions
        /// </summary>
        /// <returns>Email Subscription Collection Item</returns>
        IList<EmailSubscription> GetAllEmailSubscriptions();

        /// <summary>
        /// Get All Unsubscribe Email List
        /// </summary>
        /// <returns>Email Subscription Collection Item</returns>
        IList<EmailSubscription> GetUnsubscribeEmailList();

        /// <summary>
        /// Insert Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        void InsertEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true);

        /// <summary>
        /// Update Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        void UpdateEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true);

        /// <summary>
        /// Delete Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        void DeleteEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true);

        #endregion Methods
    }
}