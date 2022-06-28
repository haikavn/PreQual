// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ZipCodeRedirectController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class ZipCodeRedirectController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class ZipCodeRedirectController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IZipCodeRedirectService _zipCodeRedirectService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The user setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="zipCodeReirectService">The zip code reirect service.</param>
        /// <param name="Service">The service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="usersService">The users service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="departmentService">The department service.</param>
        /// <param name="userRegistrationService">The user registration service.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="userSetting">The user setting.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        public ZipCodeRedirectController(IZipCodeRedirectService zipCodeReirectService, IZipCodeRedirectService Service, ISettingService settingService, ILocalizedStringService localizedStringService, ICountryService countryService, IStateProvinceService stateProvinceService, IUserService usersService, IHistoryService historyService, IPermissionService permissionService, IRoleService roleService, IDepartmentService departmentService, IUserRegistrationService userRegistrationService, IProfileService profileService, UserSetting userSetting, IAppContext appContext, IBuyerService buyerService, IBuyerChannelService buyerChannelService)
        {
            this._zipCodeRedirectService = zipCodeReirectService;
            this._localizedStringService = localizedStringService;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._userSetting = userSetting;
            this._appContext = appContext;
            this._buyerService = buyerService;
            this._buyerChannelService = buyerChannelService;
        }

        #endregion Constructor

        // GET: ZipCodeRedirect

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lists the specified buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult List(long buyerChannelId)
        {
            return PartialView(buyerChannelId);
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        [NavigationBreadCrumb(Clear = true, Label = "ZipCodeRedirect")]
        protected void PrepareModel(ZipCodeRedirectModel model)
        {
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "ZipCodeRedirect")]
        public ActionResult Item(long id = 0)
        {
            ZipCodeRedirectModel am = new ZipCodeRedirectModel();

            long buyerChannelId = 0;

            long.TryParse(Request["BuyerChannelId"], out buyerChannelId);

            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                {
                    buyerChannelId = _appContext.AppUser.ParentId;
                }
                else
                    if (_appContext.AppUser.UserType != SharedData.BuiltInUserTypeId && _appContext.AppUser.UserType != SharedData.NetowrkUserTypeId)
                {
                    buyerChannelId = 0;
                }
            }

            ZipCodeRedirect zipCodeRedirect = this._zipCodeRedirectService.GetZipCodeRedirectById(id);

            am.ZipCodeRedirectId = 0;
            am.BuyerChannelId = buyerChannelId;

            if (zipCodeRedirect != null)
            {
                am.ZipCodeRedirectId = zipCodeRedirect.Id;
                am.RedirectUrl = zipCodeRedirect.RedirectUrl;
                am.ZipCode = zipCodeRedirect.ZipCode;
                am.BuyerChannelId = zipCodeRedirect.BuyerChannelId;
                am.Title = zipCodeRedirect.Title;
                am.Description = zipCodeRedirect.Description;
                am.Address = zipCodeRedirect.Address;
            }

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Partials the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PartialItem(long id = 0, long buyerChannelId = 0)
        {
            ZipCodeRedirectModel am = new ZipCodeRedirectModel();

            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                {
                    buyerChannelId = _appContext.AppUser.ParentId;
                }
                else
                    if (_appContext.AppUser.UserType != SharedData.BuiltInUserTypeId && _appContext.AppUser.UserType != SharedData.NetowrkUserTypeId)
                {
                    buyerChannelId = 0;
                }
            }

            ZipCodeRedirect zipCodeRedirect = this._zipCodeRedirectService.GetZipCodeRedirectById(id);

            am.ZipCodeRedirectId = 0;
            am.BuyerChannelId = buyerChannelId;

            string buyerTitle = "";
            string buyerDescription = "";
            string buyerAddress = "";

            BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId);
            if (buyerChannel != null)
            {
                Buyer buyer = _buyerService.GetBuyerById(buyerChannel.BuyerId);
                if (buyer != null)
                {
                    buyerTitle = buyer.Name;
                    buyerDescription = buyer.Description;
                    buyerAddress = buyer.AddressLine1;
                    if (buyerAddress.Length == 0)
                    {
                        buyerAddress = buyer.AddressLine2;
                    }
                }
            }

            if (zipCodeRedirect != null)
            {
                am.ZipCodeRedirectId = zipCodeRedirect.Id;
                am.RedirectUrl = zipCodeRedirect.RedirectUrl;
                am.ZipCode = zipCodeRedirect.ZipCode;
                am.BuyerChannelId = zipCodeRedirect.BuyerChannelId;
                am.Title = buyerTitle.Length == 0 ? zipCodeRedirect.Title : buyerTitle;
                am.Description = buyerDescription.Length == 0 ? zipCodeRedirect.Description : buyerDescription;
                am.Address = buyerAddress.Length > 0 ? zipCodeRedirect.Address : buyerAddress;
            }
            else
            {
                am.Title = buyerTitle;
                am.Description = buyerDescription;
                am.Address = buyerAddress;
            }

            PrepareModel(am);

            return PartialView("Item", am);
        }

        /// <summary>
        /// Items the specified zip code redirect model.
        /// </summary>
        /// <param name="zipCodeRedirectModel">The zip code redirect model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(ZipCodeRedirectModel zipCodeRedirectModel)
        {
            ZipCodeRedirect zipCodeRedirect = null;

            //if (ModelState.IsValid)
            {
                if (zipCodeRedirectModel.ZipCodeRedirectId == 0)
                {
                    zipCodeRedirect = new ZipCodeRedirect();
                }
                else
                {
                    zipCodeRedirect = _zipCodeRedirectService.GetZipCodeRedirectById(zipCodeRedirectModel.ZipCodeRedirectId);
                }

                zipCodeRedirect.ZipCode = zipCodeRedirectModel.ZipCode;
                zipCodeRedirect.RedirectUrl = zipCodeRedirectModel.RedirectUrl;
                zipCodeRedirect.BuyerChannelId = zipCodeRedirectModel.BuyerChannelId;

                zipCodeRedirect.Title = zipCodeRedirectModel.Title;
                zipCodeRedirect.Description = zipCodeRedirectModel.Description;
                zipCodeRedirect.Address = zipCodeRedirectModel.Address;

                if (zipCodeRedirectModel.ZipCodeRedirectId == 0)
                {
                    long newId = _zipCodeRedirectService.InsertZipCodeRedirect(zipCodeRedirect);
                    //                    this._historyService.AddHistory("ZipCodeRedirectController", HistoryAction.New_ZipCodeRedirect_Registered, "ZipCodeRedirect", newId, "Name:" + affiliate.Name, "", "", this._appContext.AppUser.Id);
                }
                else
                {
                    _zipCodeRedirectService.UpdateZipCodeRedirect(zipCodeRedirect);
                    //this._historyService.AddHistory("ZipCodeRedirectController", HistoryAction.ZipCodeRedirect_Edited, "ZipCodeRedirect", affiliateModel.ZipCodeRedirectId, "Name:" + affiliate.Name, "", "", this._appContext.AppUser.Id);
                }

                zipCodeRedirectModel.ZipCodeRedirectId = zipCodeRedirect.Id;

                return RedirectToAction("Item", "BuyerChannel", new { id = zipCodeRedirectModel.BuyerChannelId });
            }
        }

        /// <summary>
        /// Removes the zip code redirect.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult RemoveZipCodeRedirect()
        {
            ZipCodeRedirect bv = _zipCodeRedirectService.GetZipCodeRedirectById(long.Parse(Request["id"]));

            if (bv != null)
                _zipCodeRedirectService.DeleteZipCodeRedirect(bv);

            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the zip code redirects.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetZipCodeRedirects()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            short status = -1;
            if (Request["st"] != null)
            {
                status = short.Parse(Request["st"]);
            }

            long buyerChannelId = 0;

            long.TryParse(Request["BuyerChannelId"], out buyerChannelId);

            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                {
                    buyerChannelId = _appContext.AppUser.ParentId;
                }
                else
                    if (_appContext.AppUser.UserType != SharedData.BuiltInUserTypeId && _appContext.AppUser.UserType != SharedData.NetowrkUserTypeId)
                {
                    buyerChannelId = 0;
                }
            }

            List<ZipCodeRedirect> zipCodeRedirects = (List<ZipCodeRedirect>)this._zipCodeRedirectService.GetAllZipCodeRedirects(buyerChannelId);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = zipCodeRedirects.Count;
            jd.recordsFiltered = zipCodeRedirects.Count;
            foreach (ZipCodeRedirect ai in zipCodeRedirects)
            {
                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"" + ai.RedirectUrl + "\" target=\"_blank\">" + ai.RedirectUrl + "</a> | " +
                                      "<a href='#' class='edit-zip-code' data-id='" + ai.Id.ToString() + "' data-url='" + ai.RedirectUrl + "' data-zip='" + ai.ZipCode + "' data-title='" + ai.Title + "' data-desc='" + ai.Description + "' data-address='" + ai.Address + "' onclick='editZipCode(this)'>Edit</a>",
                                      ai.ZipCode,
                                      "<div class=\"value_remove\" data-id='" + ai.Id.ToString() + "' onclick='zipRemove(this)'><i class=\"glyphicon glyphicon-remove red\"></i></div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}