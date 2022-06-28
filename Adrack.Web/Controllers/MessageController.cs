// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="MessageController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Service.Localization;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Message Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class MessageController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Message Controller
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        public MessageController(ILocalizedStringService localizedStringService, IAppContext appContext,
            ICacheManager cacheManager)
        {
            _localizedStringService = localizedStringService;
            _appContext = appContext;
            _cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     Localized String Service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        ///     Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        ///     Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     Activation
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="active">Active</param>
        /// <returns>Action Result Item</returns>
        public ActionResult Activation(Guid token, bool active)
        {
            return View();
        }

        #endregion Methods
    }
}