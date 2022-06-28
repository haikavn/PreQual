// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="Extensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure.ApplicationEvent;

namespace Adrack.Service.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Extensions
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Entity Inserted
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this IAppEventPublisher appEventPublisher, T entity) where T : BaseEntity
        {
            appEventPublisher.Publish(new EntityInserted<T>(entity));
        }

        /// <summary>
        /// Entity Updated
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this IAppEventPublisher appEventPublisher, T entity) where T : BaseEntity
        {
            appEventPublisher.Publish(new EntityUpdated<T>(entity));
        }

        /// <summary>
        /// Entity Deleted
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this IAppEventPublisher appEventPublisher, T entity) where T : BaseEntity
        {
            appEventPublisher.Publish(new EntityDeleted<T>(entity));
        }

        #endregion Methods
    }
}