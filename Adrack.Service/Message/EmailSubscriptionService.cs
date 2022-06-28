// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailSubscriptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
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
    /// Represents a Email Subscription Service
    /// Implements the <see cref="Adrack.Service.Message.IEmailSubscriptionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailSubscriptionService" />
    public partial class EmailSubscriptionService : IEmailSubscriptionService
    {
        #region Constants

        /// <summary>
        /// Cache Email Subscription By Id Key
        /// </summary>
        private const string CACHE_EMAILSUBSCRIPTION_BY_ID_KEY = "App.Cache.EmailSubscription.By.Id-{0}";

        /// <summary>
        /// Cache Email Subscription By GuId Key
        /// </summary>
        private const string CACHE_EMAILSUBSCRIPTION_BY_GUID_KEY = "App.Cache.EmailSubscription.By.GuId-{0}";

        /// <summary>
        /// Cache Email Subscription By Email Key
        /// </summary>
        private const string CACHE_EMAILSUBSCRIPTION_BY_EMAIL_KEY = "App.Cache.EmailSubscription.By.Email-{0}";

        /// <summary>
        /// Cache Email Subscription All Key
        /// </summary>
        private const string CACHE_EMAILSUBSCRIPTION_ALL_KEY = "App.Cache.EmailSubscription.All";


        /// <summary>
        /// Cache Email Unsubscription All Key
        /// </summary>
        private const string CACHE_EMAILUNSUBSCRIPTION_ALL_KEY = "App.Cache.EmailUnsubscription.All";

        /// <summary>
        /// Cache Email Subscription Pattern Key
        /// </summary>
        private const string CACHE_EMAILSUBSCRIPTION_PATTERN_KEY = "App.Cache.EmailSubscription.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Email Subscription
        /// </summary>
        private readonly IRepository<EmailSubscription> _emailSubscriptionRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Email Subscription Application Event Publisher
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="isSubscribe">Is Subscribe</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        private void EmailSubscriptionAppEventPublisher(string email, bool isSubscribe, bool publishEmailSubscriptionEvent)
        {
            if (publishEmailSubscriptionEvent)
            {
                if (isSubscribe)
                {
                    _appEventPublisher.EmailSubscribeEventPublisher(email);
                }
                else
                {
                    _appEventPublisher.EmailUnsubscribeEventPublisher(email);
                }
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Email Subscription Service
        /// </summary>
        /// <param name="emailSubscriptionRepository">Email Subscription Repository</param>
        /// <param name="dbContext">Db Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public EmailSubscriptionService(IRepository<EmailSubscription> emailSubscriptionRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._emailSubscriptionRepository = emailSubscriptionRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Email Subscription By Id
        /// </summary>
        /// <param name="emailSubscriptionId">Email Subscription Identifier</param>
        /// <returns>Email Subscription Item</returns>
        public virtual EmailSubscription GetEmailSubscriptionById(long emailSubscriptionId)
        {
            if (emailSubscriptionId == 0)
                return null;

            string key = string.Format(CACHE_EMAILSUBSCRIPTION_BY_ID_KEY, emailSubscriptionId);

            return _cacheManager.Get(key, () => { return _emailSubscriptionRepository.GetById(emailSubscriptionId); });
        }

        /// <summary>
        /// Get Email Subscription By GuId
        /// </summary>
        /// <param name="guId">Globally Unique Identifier</param>
        /// <returns>Email Subscription Item</returns>
        public virtual EmailSubscription GetEmailSubscriptionByGuId(string guId)
        {
            if (string.IsNullOrWhiteSpace(guId))
                return null;

            string key = string.Format(CACHE_EMAILSUBSCRIPTION_BY_GUID_KEY, guId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailSubscriptionRepository.Table
                            where x.GuId == guId
                            orderby x.Id
                            select x;

                var emailSubscription = query.FirstOrDefault();

                return emailSubscription;
            });
        }

        /// <summary>
        /// Get Email Subscription By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Email Subscription Item</returns>
        public virtual EmailSubscription GetEmailSubscriptionByEmail(string email)
        {
            if (!CommonHelper.IsValidEmail(email))
                return null;

            email = email.Trim();

            string key = string.Format(CACHE_EMAILSUBSCRIPTION_BY_EMAIL_KEY, email);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailSubscriptionRepository.Table
                            where x.Email == email
                            orderby x.Id
                            select x;

                var emailSubscription = query.FirstOrDefault();

                return emailSubscription;
            });
        }

        /// <summary>
        /// Get All Email Subscriptions
        /// </summary>
        /// <returns>Email Subscription Collection Item</returns>
        public virtual IList<EmailSubscription> GetAllEmailSubscriptions()
        {
            string key = CACHE_EMAILSUBSCRIPTION_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailSubscriptionRepository.Table
                            orderby x.Email, x.Id
                            select x;

                var emailSubscriptions = query.ToList();

                return emailSubscriptions;
            });
        }

        /// <summary>
        /// Get All Unsubscribe Email List
        /// </summary>
        /// <returns>Email Subscription Collection Item</returns>
        public virtual IList<EmailSubscription> GetUnsubscribeEmailList()
        {
            string key = CACHE_EMAILUNSUBSCRIPTION_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailSubscriptionRepository.Table
                    where x.Active == false
                    orderby x.Email, x.Id
                    select x;

                var emailUnsubscriptions = query.ToList();

                return emailUnsubscriptions;
            });
        }

        /// <summary>
        /// Insert Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        /// <exception cref="ArgumentNullException">emailSubscription</exception>
        public virtual void InsertEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true)
        {
            if (emailSubscription == null)
                throw new ArgumentNullException("emailSubscription");

            emailSubscription.Email = CommonHelper.EnsureEmail(emailSubscription.Email);

            _emailSubscriptionRepository.Insert(emailSubscription);

            if (emailSubscription.Active)
            {
                EmailSubscriptionAppEventPublisher(emailSubscription.Email, true, publishEmailSubscriptionEvent);
            }

            _cacheManager.RemoveByPattern(CACHE_EMAILSUBSCRIPTION_PATTERN_KEY);

            _appEventPublisher.EntityInserted(emailSubscription);
        }

        /// <summary>
        /// Update Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        /// <exception cref="ArgumentNullException">emailSubscription</exception>
        public virtual void UpdateEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true)
        {
            if (emailSubscription == null)
                throw new ArgumentNullException("emailSubscription");

            emailSubscription.Email = CommonHelper.EnsureEmail(emailSubscription.Email);

            EmailSubscription originalEmailSubscription = _emailSubscriptionRepository.GetDbClientContext().LoadOriginalCopy(emailSubscription);

            _emailSubscriptionRepository.Update(emailSubscription);

            if ((originalEmailSubscription.Active == false && emailSubscription.Active) || (emailSubscription.Active && (originalEmailSubscription.Email != emailSubscription.Email)))
            {
                EmailSubscriptionAppEventPublisher(emailSubscription.Email, true, publishEmailSubscriptionEvent);
            }

            if ((originalEmailSubscription.Active && emailSubscription.Active) && (originalEmailSubscription.Email != emailSubscription.Email))
            {
                EmailSubscriptionAppEventPublisher(originalEmailSubscription.Email, false, publishEmailSubscriptionEvent);
            }

            if ((originalEmailSubscription.Active && !emailSubscription.Active))
            {
                EmailSubscriptionAppEventPublisher(originalEmailSubscription.Email, false, publishEmailSubscriptionEvent);
            }

            _cacheManager.RemoveByPattern(CACHE_EMAILSUBSCRIPTION_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(emailSubscription);
        }

        /// <summary>
        /// Delete Email Subscription
        /// </summary>
        /// <param name="emailSubscription">Email Subscription</param>
        /// <param name="publishEmailSubscriptionEvent">Publish Email Subscription Event</param>
        /// <exception cref="ArgumentNullException">emailSubscription</exception>
        public virtual void DeleteEmailSubscription(EmailSubscription emailSubscription, bool publishEmailSubscriptionEvent = true)
        {
            if (emailSubscription == null)
                throw new ArgumentNullException("emailSubscription");

            _emailSubscriptionRepository.Delete(emailSubscription);

            EmailSubscriptionAppEventPublisher(emailSubscription.Email, false, publishEmailSubscriptionEvent);

            _cacheManager.RemoveByPattern(CACHE_EMAILSUBSCRIPTION_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(emailSubscription);
        }

        #endregion Methods
    }
}