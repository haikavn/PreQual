// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="HomeController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Web.ContentManagement.Models.Home;
using Adrack.Web.Framework;
using System;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Represents a Home Controller
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class HomeController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="appContext">The application context.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="userService">The user service.</param>
        public HomeController(IAppContext appContext, IBuyerChannelService buyerChannelService, IAffiliateChannelService affiliateChannelService, IAffiliateService affiliateService, IBuyerService buyerService, ISettingService settingService, ICampaignService campaignService, IAuthenticationService authenticationService, IUserService userService)
        {
            this._appContext = appContext;
            this._buyerChannelService = buyerChannelService;
            this._affiliateChannelService = affiliateChannelService;
            this._affiliateService = affiliateService;
            this._buyerService = buyerService;
            this._settingService = settingService;
            this._campaignService = campaignService;
            this._authenticationService = authenticationService;
            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "")]
        public ActionResult Index()
        {
            ViewBag.TimeZoneNow = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToString("MM/dd/yyyy");

            return Redirect(Url.Action("Dashboard"));
        }

        /// <summary>
        /// Dashboard
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Dashboard")]
        public ActionResult Dashboard()
        {
            var dashboardModel = new DashboardModel();

            return View(dashboardModel);
        }

        /// <summary>
        /// Logins the back.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult LoginBack()
        {
            User user = _appContext.GetBackLoginUser();

            if (user != null)
            {
                _authenticationService.SignOut(_appContext);

                _authenticationService.SignIn(user, false);
                _appContext.AppUser = user;

                user.LoginDate = DateTime.UtcNow;
                user.FailedPasswordAttemptCount = 0;

                _userService.UpdateUser(user);
            }

            return Redirect(Helper.GetBaseUrl(Request) + "/Management/Home/Dashboard");
        }

        #endregion Methods
    }
}