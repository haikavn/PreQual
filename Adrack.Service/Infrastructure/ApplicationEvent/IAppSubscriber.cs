// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAppSubscriber.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Application Subscriber
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppSubscriber<T>
    {
        #region Methods

        /// <summary>
        /// Handle Event
        /// </summary>
        /// <param name="eventMessage">Event Message</param>
        void HandleEvent(T eventMessage);

        #endregion Methods
    }
}