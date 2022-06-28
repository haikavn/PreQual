// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAppSubscriptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Application Subscription Service
    /// </summary>
    public interface IAppSubscriptionService
    {
        #region Methods

        /// <summary>
        /// Get Application Subscriptions
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Application Subscriber Item Collection</returns>
        IList<IAppSubscriber<T>> GetAppSubscriptions<T>();

        #endregion Methods
    }
}