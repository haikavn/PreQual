// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="CommonController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Common;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.UI.Navigation;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Adrack.Web.ContentManagement.Controllers
{

    class WebClientWithCookies : WebClient
    {
        private CookieContainer _container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;

            if (request != null)
            {
                request.Method = "Post";
                request.CookieContainer = _container;
            }

            return request;
        }
    }

    /// <summary>
    /// Represents a Common Controller
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    ///
    /// 


    public partial class CommonController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// Permission Service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Search Setting
        /// </summary>
        private readonly SearchSetting _searchSetting;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Common Controller
        /// </summary>
        /// <param name="permissionService">Permission Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="searchSetting">Search Setting</param>
        /// <param name="settingService">The setting service.</param>
        public CommonController(IPermissionService permissionService, IAppContext appContext, ICacheManager cacheManager, SearchSetting searchSetting, ISettingService settingService)
        {
            this._permissionService = permissionService;
            this._appContext = appContext;
            this._cacheManager = cacheManager;
            this._searchSetting = searchSetting;
            this._settingService = settingService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Search Bar
        /// </summary>
        /// <returns>Action Result Item</returns>
        [ChildActionOnly]
        public ActionResult SearchBar()
        {
            var searchModel = new SearchBarModel
            {
                SearchMinimumLength = _searchSetting.SearchMinimumLength,
                AutoCompleteEnabled = _searchSetting.AutoCompleteEnabled,
                ShowImagesInSearchAutoComplete = _searchSetting.ShowImagesInSearchAutoComplete
            };

            return PartialView(searchModel);
        }

        /// <summary>
        /// Navigation
        /// </summary>
        /// <returns>Action Result Item</returns>
        [ChildActionOnly]
        public ActionResult Navigation()
        {
            NavigationBuilder layout = new NavigationBuilder("ContentManagement");

            var navigationManager = new NavigationManager(layout);

            navigationManager.Load();

            return PartialView(navigationManager);
        }

        /// <summary>
        /// Clear Cache Manager
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>Action Result</returns>
        public ActionResult ClearCacheManager(string returnUrl)
        {
            if (!_permissionService.Authorize(PermissionProvider.AccessContentManagementPageApplication))
                return PageUnauthorized();

            var cacheManager = AppEngineContext.Current.ContainerManager.Resolve<ICacheManager>("Application.Cache.Manager_Static");

            cacheManager.Clear();

            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Management" });

            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Management" });

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Get Timezone Now
        /// </summary>
        /// <returns>string</returns>
        [ContentManagementAntiForgery(true)]
        public string GetTimezoneNow()
        {
            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            Setting tz = _settingService.GetSetting("TimeZone");
            Setting tzStr = _settingService.GetSetting("TimeZoneStr");

            if (tz != null && tzStr != null)
            {
                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(tzStr.Value);
                start = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
                TimeSpan ts = start - DateTime.UtcNow;
                tz.Value = ts.TotalHours.ToString();
                _settingService.UpdateSetting(tz,false);
            }

            return start.ToString();
        }

        
        

        private static string _cookies = string.Empty;


        static void client_OpenReadCompleted(object sender,
            System.Net.OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (Stream stream = e.Result)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }

                WebClientWithCookies client = sender as WebClientWithCookies;

                if (client != null)
                {
                    _cookies = client.ResponseHeaders["Set-Cookie"];
                    Console.WriteLine(_cookies);
                }
            }
            else
            {
                Console.WriteLine(e.Error.Message);
            }
        }
        

        #endregion Methods
    }
}