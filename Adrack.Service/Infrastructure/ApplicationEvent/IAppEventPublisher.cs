// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAppEventPublisher.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Application Event Publisher
    /// </summary>
    public interface IAppEventPublisher
    {
        #region Methods

        /// <summary>
        /// Publish Event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event Message</param>
        void Publish<T>(T eventMessage);

        #endregion Methods
    }
}