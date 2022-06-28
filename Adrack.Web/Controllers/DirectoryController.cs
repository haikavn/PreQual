// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DirectoryController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Service.Directory;
using Adrack.Service.Localization;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Directory Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class DirectoryController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Directory Controller
        /// </summary>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        public DirectoryController(IStateProvinceService stateProvinceService, ICountryService countryService,
            ILocalizedStringService localizedStringService, IAppContext appContext, ICacheManager cacheManager)
        {
            _stateProvinceService = stateProvinceService;
            _countryService = countryService;
            _localizedStringService = localizedStringService;
            _appContext = appContext;
            _cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     State Province Service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        ///     Country Service
        /// </summary>
        private readonly ICountryService _countryService;

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

        #region Country

        /// <summary>
        ///     Get State Province By Country Id
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="addSelectStateProvinceItem">if set to <c>true</c> [add select state province item].</param>
        /// <returns>Action Result Item</returns>
        /// <exception cref="ArgumentNullException">countryId</exception>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStateProvinceByCountryId(string countryId, bool addSelectStateProvinceItem)
        {
            if (string.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            var countryService = _countryService.GetCountryById(Convert.ToInt64(countryId));

            var stateProvinceService =
                _stateProvinceService.GetStateProvinceByCountryId(countryService != null ? countryService.Id : 0);

            var result = (from x in stateProvinceService
                          select new
                          {
                              id = x.Id,
                              name = x.GetLocalized(y => y.Name),
                              code = x.Code
                          })
                .ToList();

            if (countryService == null)
            {
                if (addSelectStateProvinceItem)
                    result.Insert(0,
                        new
                        {
                            id = Convert.ToInt64(0),
                            name = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"),
                            code = ""
                        });
                else
                    result.Insert(0,
                        new
                        {
                            id = Convert.ToInt64(0),
                            name = _localizedStringService.GetLocalizedString("Address.OtherNonUS"),
                            code = ""
                        });
            }
            else
            {
                if (result.Count == 0)
                {
                    result.Insert(0,
                        new
                        {
                            id = Convert.ToInt64(0),
                            name = _localizedStringService.GetLocalizedString("Address.OtherNonUS"),
                            code = ""
                        });
                }
                else
                {
                    if (addSelectStateProvinceItem)
                        result.Insert(0,
                            new
                            {
                                id = Convert.ToInt64(0),
                                name = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"),
                                code = ""
                            });
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Country

        #endregion Methods
    }
}