// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AppEventPublisherExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Message;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Application Event Publisher Extensions
    /// </summary>
    public static class AppEventPublisherExtensions
    {
        #region Methods

        /// <summary>
        /// Email Subscribe Event Publisher
        /// </summary>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="email">Email</param>
        public static void EmailSubscribeEventPublisher(this IAppEventPublisher appEventPublisher, string email)
        {
            appEventPublisher.Publish(new EmailSubscribeEventPublisher(email));
        }

        /// <summary>
        /// Emails the unsubscribe event publisher.
        /// </summary>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="email">Email</param>
        public static void EmailUnsubscribeEventPublisher(this IAppEventPublisher appEventPublisher, string email)
        {
            appEventPublisher.Publish(new EmailUnsubscribeEventPublisher(email));
        }

        /// <summary>
        /// Entity Token Added Event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="U">Token</typeparam>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="entity">Entity</param>
        /// <param name="tokens">Tokens</param>
        public static void EntityTokenAddedEvent<T, U>(this IAppEventPublisher appEventPublisher, T entity, IList<U> tokens) where T : BaseEntity
        {
            appEventPublisher.Publish(new EntityTokenAddedEvent<T, U>(entity, tokens));
        }

        /// <summary>
        /// Email Template Token Added Event
        /// </summary>
        /// <typeparam name="U">Token</typeparam>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="tokens">Tokens</param>
        public static void EmailTemplateTokenAddedEvent<U>(this IAppEventPublisher appEventPublisher, EmailTemplate emailTemplate, IList<U> tokens)
        {
            appEventPublisher.Publish(new EmailTemplateTokenAddedEvent<U>(emailTemplate, tokens));
        }

        #endregion Methods
    }
}