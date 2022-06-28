// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AppEventPublisher.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;
using System;
using System.Linq;

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Application Event Publisher
    /// Implements the <see cref="Adrack.Service.Infrastructure.ApplicationEvent.IAppEventPublisher" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Infrastructure.ApplicationEvent.IAppEventPublisher" />
    public class AppEventPublisher : IAppEventPublisher
    {
        #region Fields

        /// <summary>
        /// Application Subscription Service
        /// </summary>
        private readonly IAppSubscriptionService _appSubscriptionService;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Publish To Subscriber
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="appSubscriber">Application Subscriber</param>
        /// <param name="eventMessage">Event Message</param>
        protected virtual void PublishToSubscriber<T>(IAppSubscriber<T> appSubscriber, T eventMessage)
        {
            try
            {
                appSubscriber.HandleEvent(eventMessage);
            }
            catch (Exception ex)
            {
                var logger = AppEngineContext.Current.Resolve<ILogService>();
                try
                {
                    logger.Error(ex.Message, ex);
                }
                catch (Exception)
                {
                    //
                }
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        /// <param name="appSubscriptionService">Application Subscription Service</param>
        public AppEventPublisher(IAppSubscriptionService appSubscriptionService)
        {
            _appSubscriptionService = appSubscriptionService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Publish Event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event Message</param>
        public virtual void Publish<T>(T eventMessage)
        {
            var appSubscriptions = _appSubscriptionService.GetAppSubscriptions<T>();

            appSubscriptions.ToList().ForEach(x => PublishToSubscriber(x, eventMessage));
        }

        #endregion Methods
    }
}