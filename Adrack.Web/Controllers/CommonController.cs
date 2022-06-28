// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CommonController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Service.Common;
using Adrack.Service.Directory;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.UI.Navigation;
using Adrack.Web.Models.Common;
using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Common Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class CommonController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Common Controller
        /// </summary>
        /// <param name="searchSetting">Search Setting</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appContext">Application Context</param>
        public CommonController(SearchSetting searchSetting, ICacheManager cacheManager, IAppContext appContext, ICountryService countryService, IGeoZipService geoZipService)
        {
            _searchSetting = searchSetting;
            _cacheManager = cacheManager;
            _appContext = appContext;
            this._countryService = countryService;
            this._geoZipService = geoZipService;

        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        ///     Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        ///     Search Setting
        /// </summary>
        private readonly SearchSetting _searchSetting;

        private readonly ICountryService _countryService;

        private readonly IGeoZipService _geoZipService;


        #endregion Fields

        #region Methods

        /// <summary>
        ///     Page Error
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>Action Result</returns>
        public ActionResult PageError(int statusCode)
        {
            Response.StatusCode = statusCode;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///     Page Unauthorized
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageUnauthorized()
        {
            Response.StatusCode = 401;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///     Page Forbidden
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageForbidden()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///     Page Not Found
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///     Page Internal Server Error
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageInternalServerError()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///     Page Slug Url
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageSlugUrl()
        {
            return InvokeHttp404();
        }

        /// <summary>
        ///     Site Maintenance
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult SiteMaintenance()
        {
            return View();
        }

        /// <summary>
        ///     Search
        /// </summary>
        /// <param name="searchModel">Search Model</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        [ValidateInput(false)]
        public ActionResult Search(SearchModel searchModel)
        {
            if (searchModel == null)
                searchModel = new SearchModel();

            searchModel.NoResults = true;

            return View(searchModel);
        }

        /// <summary>
        ///     Search Bar
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
        ///     Navigation
        /// </summary>
        /// <returns>Action Result Item</returns>
        [ChildActionOnly]
        public ActionResult Navigation()
        {
            var layout = new NavigationBuilder("Web");

            var navigationManager = new NavigationManager(layout);

            navigationManager.Load();

            return PartialView(navigationManager);
        }

        /// <summary>
        ///     About Us
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        ///     Contact Us
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult ContactUs()
        {
            return View();
        }


        /// <summary>
        /// GetGeoDataByZip
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]

        public ActionResult GetGeoDataByZip(int zip)
        {
            GeoZip gz = this._geoZipService.GetGeoDataByZip(zip);
            return Json(gz, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBankByAbaNumber
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        
        public ActionResult GetBankByAbaNumber(long abanumber)
        {
            AbaNumber gz = this._geoZipService.GetBankByAbaNumber(abanumber);
            return Json(gz, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}