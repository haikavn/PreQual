// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailQueueService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Queue Service
    /// </summary>
    public partial interface IEmailQueueService
    {
        #region Methods

        /// <summary>
        /// Get Email Queue By Id
        /// </summary>
        /// <param name="emailQueueId">Email Queue Identifier</param>
        /// <returns>Email Queue Item</returns>
        EmailQueue GetEmailQueueById(long emailQueueId);

        /// <summary>
        /// Search Email Queues
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="createdFrom">Created From</param>
        /// <param name="createdTo">Created To</param>
        /// <param name="emailSent">Email Sent</param>
        /// <param name="newEmail">New Email</param>
        /// <param name="deliveryAttempts">Delivery Attempts</param>
        /// <returns>Email Queue Collection Item</returns>
        IList<EmailQueue> SearchEmailQueues(string sender, string recipient, DateTime? createdFrom, DateTime? createdTo, bool emailSent, bool newEmail, int deliveryAttempts);

        /// <summary>
        /// Get All Email Queues
        /// </summary>
        /// <returns>Email Queue Collection Item</returns>
        IList<EmailQueue> GetAllEmailQueues();

        /// <summary>
        /// Insert Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        void InsertEmailQueue(EmailQueue emailQueue);

        /// <summary>
        /// Update Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        void UpdateEmailQueue(EmailQueue emailQueue);

        /// <summary>
        /// Delete Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        void DeleteEmailQueue(EmailQueue emailQueue);

        #endregion Methods
    }
}