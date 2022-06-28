// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AppSubscriptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using System.Collections.Generic;

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Application Subscription Service
    /// Implements the <see cref="Adrack.Service.Infrastructure.ApplicationEvent.IAppSubscriptionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Infrastructure.ApplicationEvent.IAppSubscriptionService" />
    public class AppSubscriptionService : IAppSubscriptionService
    {
        #region Methods

        /// <summary>
        /// Get Subscriptions
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Application Subscriber Item Collection</returns>
        public IList<IAppSubscriber<T>> GetAppSubscriptions<T>()
        {
            return AppEngineContext.Current.ResolveAll<IAppSubscriber<T>>();
        }

        #endregion Methods
    }
}