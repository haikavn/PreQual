// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailQueueService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Queue Service
    /// Implements the <see cref="Adrack.Service.Message.IEmailQueueService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailQueueService" />
    public partial class EmailQueueService : IEmailQueueService
    {
        #region Constants

        /// <summary>
        /// Cache Email Queue By Id Key
        /// </summary>
        private const string CACHE_EMAILQUEUE_BY_ID_KEY = "App.Cache.EmailQueue.By.Id-{0}";

        /// <summary>
        /// Cache Email Queue All Key
        /// </summary>
        private const string CACHE_EMAILQUEUE_ALL_KEY = "App.Cache.EmailQueue.All";

        /// <summary>
        /// Cache Email Queue Pattern Key
        /// </summary>
        private const string CACHE_EMAILQUEUE_PATTERN_KEY = "App.Cache.EmailQueue.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Email Queue
        /// </summary>
        private readonly IRepository<EmailQueue> _emailQueueRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Queue Service
        /// </summary>
        /// <param name="emailQueueRepository">Email Queue Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public EmailQueueService(IRepository<EmailQueue> emailQueueRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._emailQueueRepository = emailQueueRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Email Queue By Id
        /// </summary>
        /// <param name="emailQueueId">Email Queue Identifier</param>
        /// <returns>Email Queue Item</returns>
        public virtual EmailQueue GetEmailQueueById(long emailQueueId)
        {
            if (emailQueueId == 0)
                return null;

            string key = string.Format(CACHE_EMAILQUEUE_BY_ID_KEY, emailQueueId);

            return _cacheManager.Get(key, () => { return _emailQueueRepository.GetById(emailQueueId); });
        }

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
        public virtual IList<EmailQueue> SearchEmailQueues(string sender, string recipient, DateTime? createdFrom, DateTime? createdTo, bool emailSent, bool newEmail, int deliveryAttempts)
        {
            sender = (sender ?? String.Empty).Trim();
            recipient = (recipient ?? String.Empty).Trim();

            var query = _emailQueueRepository.Table;

            if (!String.IsNullOrEmpty(sender))
                query = query.Where(x => x.Sender.Contains(sender));

            if (!String.IsNullOrEmpty(recipient))
                query = query.Where(x => x.Recipient.Contains(recipient));

            if (createdFrom.HasValue)
                query = query.Where(x => x.CreatedOn >= createdFrom);

            if (createdTo.HasValue)
                query = query.Where(x => x.CreatedOn <= createdTo);

            if (emailSent)
                query = query.Where(x => !x.SentOn.HasValue);

            query = query.Where(x => x.DeliveryAttempts < deliveryAttempts);

            query = newEmail ? query.OrderByDescending(x => x.CreatedOn) : query.OrderByDescending(x => x.Priority).ThenBy(x => x.CreatedOn);

            var emailQueues = query.ToList();

            return emailQueues;
        }

        /// <summary>
        /// Get All Email Queues
        /// </summary>
        /// <returns>Email Queue Collection Item</returns>
        public virtual IList<EmailQueue> GetAllEmailQueues()
        {
            string key = CACHE_EMAILQUEUE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailQueueRepository.Table //where x.Id.Equals(124) //test
                            orderby x.Id
                            select x;

                var emailQueues = query.ToList();

                return emailQueues;
            });
        }

        /// <summary>
        /// Insert Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        /// <exception cref="ArgumentNullException">emailQueue</exception>
        public virtual void InsertEmailQueue(EmailQueue emailQueue)
        {
            if (emailQueue == null)
                throw new ArgumentNullException("emailQueue");

            _emailQueueRepository.Insert(emailQueue);

            _cacheManager.RemoveByPattern(CACHE_EMAILQUEUE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(emailQueue);
        }

        /// <summary>
        /// Update Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        /// <exception cref="ArgumentNullException">emailQueue</exception>
        public virtual void UpdateEmailQueue(EmailQueue emailQueue)
        {
            if (emailQueue == null)
                throw new ArgumentNullException("emailQueue");

            _emailQueueRepository.Update(emailQueue);

            _cacheManager.RemoveByPattern(CACHE_EMAILQUEUE_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(emailQueue);
        }

        /// <summary>
        /// Delete Email Queue
        /// </summary>
        /// <param name="emailQueue">Email Queue</param>
        /// <exception cref="ArgumentNullException">emailQueue</exception>
        public virtual void DeleteEmailQueue(EmailQueue emailQueue)
        {
            if (emailQueue == null)
                throw new ArgumentNullException("emailQueue");

            _emailQueueRepository.Delete(emailQueue);

            _cacheManager.RemoveByPattern(CACHE_EMAILQUEUE_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(emailQueue);
        }

        #endregion Methods
    }
}