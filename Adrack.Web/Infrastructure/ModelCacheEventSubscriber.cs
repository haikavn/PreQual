// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ModelCacheEventSubscriber.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.ApplicationEvent;
using Adrack.Service.Infrastructure.ApplicationEvent;

namespace Adrack.Web.Infrastructure
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}'
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}'

    /// <summary>
    ///     Represents a Model Cache Event Subscriber
    ///     Implements the <see cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}" />
    ///     Implements the <see cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}" />
    ///     Implements the <see cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}" />
    /// </summary>
    /// <seealso cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}" />
    /// <seealso cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}" />
    /// <seealso cref="Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}" />
    public class ModelCacheEventSubscriber :
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}'
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityUpdated{Setting}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityDeleted{Setting}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'Service.Infrastructure.ApplicationEvent.IAppSubscriber{EntityInserted{Setting}}'
        // Setting
        IAppSubscriber<EntityInserted<Setting>>,
        IAppSubscriber<EntityUpdated<Setting>>,
        IAppSubscriber<EntityDeleted<Setting>>
    {
        #region Fields

        /// <summary>
        ///     Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Constructor

        /// <summary>
        ///     Model Cache Event Subscriber
        /// </summary>
        public ModelCacheEventSubscriber()
        {
            _cacheManager =
                AppEngineContext.Current.ContainerManager.Resolve<ICacheManager>("Application.Cache.Manager_Static");
        }

        #endregion Constructor

        #region Constants

        #region Setting

        /// <summary>
        ///     Cache Model Setting Key
        /// </summary>
        public const string CACHE_MODEL_SETTING_KEY = "App.Cache.Model.Setting-{0}";

        /// <summary>
        ///     Cache Model Setting Pattern Key
        /// </summary>
        public const string CACHE_MODEL_SETTING_PATTERN_KEY = "App.Cache.Model.Setting";

        #endregion Setting

        #endregion Constants

        #region Methods

        #region Setting

        /// <summary>
        ///     Handle Event
        /// </summary>
        /// <param name="eventMessage">Event Message</param>
        public void HandleEvent(EntityInserted<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CACHE_MODEL_SETTING_PATTERN_KEY);
        }

        /// <summary>
        ///     Handle Event
        /// </summary>
        /// <param name="eventMessage">Event Message</param>
        public void HandleEvent(EntityUpdated<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CACHE_MODEL_SETTING_PATTERN_KEY);
        }

        /// <summary>
        ///     Handle Event
        /// </summary>
        /// <param name="eventMessage">Event Message</param>
        public void HandleEvent(EntityDeleted<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(CACHE_MODEL_SETTING_PATTERN_KEY);
        }

        #endregion Setting

        #endregion Methods
    }
}