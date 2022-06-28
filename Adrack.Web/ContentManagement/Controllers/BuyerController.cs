// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.ContentManagement.Models.Lead.Reports;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Handles buyer controller actions
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class BuyerController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The buyer channel template service
        /// </summary>
        private readonly IBuyerChannelTemplateService _buyerChannelTemplateService;

        /// <summary>
        /// The buyer channel filter condition service
        /// </summary>
        private readonly IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The user setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// The user registration service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        /// The profile service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        private readonly IAffiliateService _affiliateService;

        private readonly IAffiliateChannelService _affiliateChannelService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="usersService">The users service.</param>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="departmentService">The department service.</param>
        /// <param name="userRegistrationService">The user registration service.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="userSetting">The user setting.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="buyerChannelTemplateService">The buyer channel template service.</param>
        /// <param name="buyerChannelFilterConditionService">The buyer channel filter condition service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        public BuyerController(IBuyerService buyerService, ILocalizedStringService localizedStringService, ICountryService countryService, IStateProvinceService stateProvinceService, IUserService usersService, IAccountingService accountingService, IHistoryService historyService, IPermissionService permissionService, IAppContext appContext,
            IRoleService roleService, IDepartmentService departmentService, IUserRegistrationService userRegistrationService, IProfileService profileService, UserSetting userSetting, IEmailService emailService, IBuyerChannelService buyerChannelService,
            IAuthenticationService authenticationService, ICampaignService campaignService, IBuyerChannelTemplateService buyerChannelTemplateService,
            IBuyerChannelFilterConditionService buyerChannelFilterConditionService, ICampaignTemplateService campaignTemplateService,
            IAffiliateService affiliateService, IAffiliateChannelService affiliateChannelService)
        {
            this._buyerService = buyerService;
            this._countryService = countryService;
            this._localizedStringService = localizedStringService;
            this._stateProvinceService = stateProvinceService;
            this._userService = usersService;
            this._accountingService = accountingService;
            this._historyService = historyService;
            this._permissionService = permissionService;
            this._userRegistrationService = userRegistrationService;
            this._profileService = profileService;
            this._userSetting = userSetting;
            this._roleService = roleService;
            this._emailService = emailService;
            this._appContext = appContext;
            this._buyerChannelService = buyerChannelService;
            this._authenticationService = authenticationService;
            this._campaignService = campaignService;
            this._buyerChannelTemplateService = buyerChannelTemplateService;
            this._buyerChannelFilterConditionService = buyerChannelFilterConditionService;
            this._campaignTemplateService = campaignTemplateService;
            this._affiliateService = affiliateService;
            this._affiliateChannelService = affiliateChannelService;
        }

        #endregion Constructor

        // GET: Affiliate
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            List<Buyer> buyers = (List<Buyer>)this._buyerService.GetAllBuyers();
            return View();
        }

        /// <summary>
        /// Displays buyer list
        /// </summary>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyers")]
        public ActionResult List()
        {
            ViewBag.Campaigns = _campaignService.GetAllCampaigns(0);

            return View();
        }

        /// <summary>
        /// Histories the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult History(long id)
        {
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                id = _appContext.AppUser.ParentId;
            }

            ViewBag.SelectedBuyerId = id;
            // Name/Description "Create Affiliate Chanel"
            return View();
        }

        /// <summary>
        /// Prepares buyer model
        /// </summary>
        /// <param name="model">BuerModel reference</param>
        protected void PrepareModel(BuyerModel model, Buyer buyer = null)
        {
            model.ListStatus.Add(new System.Web.Mvc.SelectListItem { Text = "Inactive", Value = "0" });
            model.ListStatus.Add(new System.Web.Mvc.SelectListItem { Text = "Active", Value = "1" });

            model.ListDoNotPresentPostMethod.Add(new SelectListItem { Text = "POST", Value = "POST" });
            model.ListDoNotPresentPostMethod.Add(new SelectListItem { Text = "GET", Value = "GET" });

            model.ListDoNotPresentStatus.Add(new System.Web.Mvc.SelectListItem { Text = "Off", Value = "0" });
            model.ListDoNotPresentStatus.Add(new System.Web.Mvc.SelectListItem { Text = "Local", Value = "1" });
            model.ListDoNotPresentStatus.Add(new System.Web.Mvc.SelectListItem { Text = "Url", Value = "2" });

            model.ListAlwaysSoldOption.Add(new SelectListItem { Text = "Online", Value = "0" });
            model.ListAlwaysSoldOption.Add(new SelectListItem { Text = "Storefront", Value = "1" });

            model.ListUser.Add(new System.Web.Mvc.SelectListItem { Text = _localizedStringService.GetLocalizedString("Affiliate.SelectAccountManager"), Value = "" });

            foreach (var value in _userService.GetUsersByType(UserTypes.Network))
            {
                if (value.Deleted) continue;

                model.ListUser.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = value.GetFullName(),
                    Value = value.Id.ToString(),
                    Selected = value.Id == model.ManagerId
                });
            }

            model.ListCountry.Add(new System.Web.Mvc.SelectListItem { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

            foreach (var value in _countryService.GetAllCountries())
            {
                model.ListCountry.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Id.ToString(),
                    Selected = value.Id == model.CountryId
                });
            }

            var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(model.CountryId).ToList();

            if (stateProvince.Count() > 0)
            {
                model.ListStateProvince.Add(new System.Web.Mvc.SelectListItem { Text = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"), Value = "" });

                foreach (var value in stateProvince)
                {
                    model.ListStateProvince.Add(new System.Web.Mvc.SelectListItem
                    {
                        Text = value.GetLocalized(x => x.Name),
                        Value = value.Id.ToString(),
                        Selected = value.Id == model.StateProvinceId
                    });
                }
            }
            else
            {
                bool anyCountrySelected = model.ListCountry.Any(x => x.Selected);

                model.ListStateProvince.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = _localizedStringService.GetLocalizedString(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectStateProvince"),
                    Value = "0"
                });
            }

            model.ListUserRole.Add(new SelectListItem { Text = "Select role", Value = "" });

            var roles = _roleService.GetAllRoles();

            foreach (var value in _roleService.GetAllRoles())
            {
                if (value.UserType != model.UserType) continue;

                model.ListUserRole.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }
          }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Buyer")]
        public ActionResult Create()
        {
            BuyerModel am = new BuyerModel();
            am.BuyerId = 0;
            am.UserType = SharedData.BuyerUserTypeId;
            am.UserRoleId = 6;
            am.ParentId = 0;

            am.DailyCap = 300;
            am.MaxDuplicateDays = 30;

            if (this._appContext.AppUser != null && this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                am.ParentId = this._appContext.AppUser.ParentId;
            }

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Displays buyer create/edit interface
        /// </summary>
        /// <param name="id">Buyer id</param>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Billing")]
        public ActionResult Item(long id = 0)
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return HttpNotFound();
            }

            if (this._appContext.AppUser != null && this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                id = this._appContext.AppUser.ParentId;
            }

            BuyerModel am = new BuyerModel();

            Buyer buyer = this._buyerService.GetBuyerById(id);

            am.BuyerId = 0;

            ViewBag.SelectedBuyerId = id;
            ViewBag.UserTypeId = 0;

            if (_appContext.AppUser != null)
            {
                ViewBag.UserTypeId = _appContext.AppUser.UserType;
            }

            if (buyer != null)
            {
                am.BuyerId = buyer.Id;
                am.CountryId = buyer.CountryId;
                am.StateProvinceId = (buyer.StateProvinceId.HasValue ? buyer.StateProvinceId.Value : 0);
                am.Name = buyer.Name;
                am.AddressLine1 = buyer.AddressLine1;
                am.AddressLine2 = buyer.AddressLine2;
                am.City = buyer.City;
                am.CompanyEmail = buyer.Email;
                am.CompanyPhone = buyer.Phone;
                am.ZipPostalCode = buyer.ZipPostalCode;
                am.Status = buyer.Status;
                am.BillFrequency = buyer.BillFrequency;
                am.FrequencyValue = (int)buyer.FrequencyValue;
                am.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
                am.ManagerId = (long)buyer.ManagerId;
                am.AlwaysSoldOption = buyer.AlwaysSoldOption;
                am.DailyCap = buyer.DailyCap;
                am.MaxDuplicateDays = buyer.MaxDuplicateDays;
                am.Description = buyer.Description;
                am.ExternalId = buyer.ExternalId;
                am.CoolOffEnabled = (buyer.CoolOffEnabled.HasValue ? buyer.CoolOffEnabled.Value : false);
                am.CoolOffStart = (buyer.CoolOffStart.HasValue ? buyer.CoolOffStart.Value : DateTime.UtcNow);
                am.CoolOffEnd = (buyer.CoolOffEnd.HasValue ? buyer.CoolOffEnd.Value : DateTime.UtcNow);

                am.DoNotPresentResultField = buyer.DoNotPresentResultField;
                am.DoNotPresentResultValue = buyer.DoNotPresentResultValue;
                am.DoNotPresentStatus = buyer.DoNotPresentStatus;
                am.DoNotPresentUrl = buyer.DoNotPresentUrl;
                am.DoNotPresentPostMethod = buyer.DoNotPresentPostMethod;
                am.DoNotPresentRequest = buyer.DoNotPresentRequest;
                am.CanSendLeadId = buyer.CanSendLeadId.HasValue ? buyer.CanSendLeadId.Value : false;
                am.AccountId = (buyer.AccountId.HasValue ? buyer.AccountId.Value : 0);
            }

            PrepareModel(am, buyer);

            //return View(am);
            return View(am);
        }

        /// <summary>
        /// Billings the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Buyer")]
        public ActionResult Billing(long id = 0)
        {
            if (this._appContext.AppUser != null && this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                id = this._appContext.AppUser.ParentId;
            }

            BuyerModel am = new BuyerModel();

            Buyer buyer = this._buyerService.GetBuyerById(id);

            am.BuyerId = 0;

            ViewBag.SelectedBuyerId = id;
            ViewBag.UserTypeId = 0;

            if (_appContext.AppUser != null)
            {
                ViewBag.UserTypeId = _appContext.AppUser.UserType;
            }

            if (buyer != null)
            {
                am.BuyerId = buyer.Id;
                am.CountryId = buyer.CountryId;
                am.StateProvinceId = (buyer.StateProvinceId.HasValue ? buyer.StateProvinceId.Value : 0);
                am.Name = buyer.Name;
                am.AddressLine1 = buyer.AddressLine1;
                am.AddressLine2 = buyer.AddressLine2;
                am.City = buyer.City;
                am.CompanyEmail = buyer.Email;
                am.Phone = buyer.Phone;
                am.ZipPostalCode = buyer.ZipPostalCode;
                am.Status = buyer.Status;
                am.BillFrequency = buyer.BillFrequency;
                am.FrequencyValue = (int)buyer.FrequencyValue;
                am.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
                am.ManagerId = (long)buyer.ManagerId;
            }

            PrepareModel(am);

            //return View(am);
            return View(am);
        }

        /// <summary>
        /// Handles buyer submit action
        /// </summary>
        /// <param name="buyerModel">BuyerModel reference</param>
        /// <param name="returnUrl">Redirect url after success</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Billing(BuyerModel buyerModel, string returnUrl)
        {
            Buyer buyer = _buyerService.GetBuyerById(buyerModel.BuyerId);

            buyer.BillFrequency = buyerModel.BillFrequency;
            buyer.FrequencyValue = buyerModel.FrequencyValue;

            _buyerService.UpdateBuyer(buyer);

            BuyerBalance bb = _accountingService.GetBuyerBalanceById(buyerModel.BuyerId);

            if (bb == null)
            {
                bb = new BuyerBalance();
                bb.BuyerId = buyer.Id;
                _accountingService.InsertBuyerBalance(bb);
            }

            bb.Credit = buyerModel.Credit;

            _accountingService.UpdateBuyerBalance(bb, "Credit");

            this._historyService.AddHistory("BuyerController", HistoryAction.Buyer_Edited, "Buyer", buyerModel.BuyerId, "", "", "", this._appContext.AppUser.Id);

            buyerModel.BuyerId = buyer.Id;

            PrepareModel(buyerModel);

            ViewBag.Campaigns = _campaignService.GetAllCampaigns(0);


            return View("List");
        }

        /// <summary>
        /// Registers the specified buyer model.
        /// </summary>
        /// <param name="buyerModel">The buyer model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Register(BuyerModel buyerModel)
        {
            var user = new User();

            if (ModelState.IsValid)
            {
                buyerModel.UserName = buyerModel.Email.Trim();

                Buyer buyer = _buyerService.GetBuyerByName(buyerModel.Name, 0);

                if (buyer != null)
                {
                    ModelState.AddModelError("", "Buyer with the specified name already exist");
                    PrepareModel(buyerModel);
                    return View("Create", buyerModel);
                }

                bool isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;

                user.UserType = SharedData.AffiliateUserTypeId;

                var registrationRequest = new UserRegistrationRequest(user, buyerModel.UserName, buyerModel.UserName, buyerModel.Password, buyerModel.Comment, buyerModel.ContactEmail, isApproved);

                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, false);

                if (registrationResult.Success)
                {
                    // External Account

                    // Profile
                    _profileService.InsertProfile(new Profile
                    {
                        UserId = user.Id,
                        FirstName = buyerModel.FirstName,
                        MiddleName = buyerModel.MiddleName,
                        LastName = buyerModel.LastName,
                        Summary = "",
                        Phone = buyerModel.Phone,
                        CellPhone = buyerModel.CellPhone
                    });

                    if (buyerModel.ParentId == 0)
                    {
                        if (buyerModel.UserRoleId != 1 && buyerModel.UserType != SharedData.BuiltInUserTypeId)
                        {
                            buyer = new Buyer()
                            {
                                Name = buyerModel.Name,
                                CountryId = buyerModel.CountryId,
                                StateProvinceId = (buyerModel.StateProvinceId == 0 ? null : (long?)buyerModel.StateProvinceId),
                                Email = buyerModel.CompanyEmail,
                                AddressLine1 = buyerModel.AddressLine1,
                                AddressLine2 = buyerModel.AddressLine2,
                                City = buyerModel.City,
                                ZipPostalCode = buyerModel.ZipPostalCode,
                                Phone = buyerModel.CompanyPhone,
                                CreatedOn = DateTime.UtcNow,
                                Status = 1,
                                ManagerId = buyerModel.ManagerId,
                                BillFrequency = "m",
                                FrequencyValue = 1,
                                AlwaysSoldOption = buyerModel.AlwaysSoldOption,
                                MaxDuplicateDays = buyerModel.MaxDuplicateDays,
                                DailyCap = buyerModel.DailyCap,
                                Description = buyerModel.Description,
                                DoNotPresentResultField = buyerModel.DoNotPresentResultField,
                                DoNotPresentResultValue = buyerModel.DoNotPresentResultValue,
                                DoNotPresentStatus = buyerModel.DoNotPresentStatus,
                                DoNotPresentPostMethod = buyerModel.DoNotPresentPostMethod,
                                DoNotPresentRequest = buyerModel.DoNotPresentRequest,
                                DoNotPresentUrl = buyerModel.DoNotPresentUrl,
                                CanSendLeadId = buyerModel.CanSendLeadId,
                                AccountId = buyerModel.AccountId
                        };

                            long buyerId = _buyerService.InsertBuyer(buyer);

                            user.ParentId = buyerId;

                            BuyerBalance bb = new BuyerBalance();
                            bb.Credit = buyerModel.Credit;
                            bb.Balance = buyerModel.Credit;
                            bb.BuyerId = buyerId;
                            bb.PaymentSum = 0;
                            bb.SoldSum = 0;
                            _accountingService.InsertBuyerBalance(bb);
                            this._historyService.AddHistory("BuyerController", HistoryAction.New_Buyer_User_Registered, "User", user.Id, "", "", "", this._appContext.AppUser.Id);
                            this._historyService.AddHistory("BuyerController", HistoryAction.Buyer_Added, "Buyer", buyer.Id, "", "", "", this._appContext.AppUser.Id);
                        }
                    }
                    else
                    {
                        user.ParentId = buyerModel.ParentId;
                    }

                    user.UserType = buyerModel.UserType;
                    user.DepartmentId = 1;

                    Role role = _roleService.GetRoleById(buyerModel.UserRoleId);

                    if (role == null)
                    {
                        if (buyerModel.UserType == SharedData.AffiliateUserTypeId || buyerModel.UserType == SharedData.BuyerUserTypeId)
                            role = _roleService.GetRoleById(2);
                    }

                    if (role != null)
                    {
                        user.Roles.Add(role);
                    }

                    user.Active = true;

                    user.ValidateOnLogin = true;

                    IGlobalAttributeService _globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();
                    _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, user.GuId);

                    _userService.UpdateUser(user);
                    _emailService.SendUserWelcomeMessageWithUsernamePassword(user, _appContext.AppLanguage.Id,EmailOperatorEnums.LeadNative, buyerModel.Email, buyerModel.Password);
                }

                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);

                if (registrationResult.Errors.Count > 0)
                {
                    PrepareModel(buyerModel);

                    return View("Create", buyerModel);
                }
            }

            PrepareModel(buyerModel);

            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays buyer create/edit partial interface
        /// </summary>
        /// <param name="id">Buyer id</param>
        /// <returns>PartialView result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyer")]
        public ActionResult PartialItem(long id = 0)
        {
            Buyer buyer = this._buyerService.GetBuyerById(id);

            BuyerModel am = new BuyerModel();

            am.BuyerId = 0;

            ViewBag.UserTypeId = 0;

            if (_appContext.AppUser != null) ViewBag.UserTypeId = _appContext.AppUser.UserType;

            if (buyer != null)
            {
                am.BuyerId = buyer.Id;
                am.CountryId = buyer.CountryId;
                am.StateProvinceId = (buyer.StateProvinceId.HasValue ? buyer.StateProvinceId.Value : 0);
                am.Name = buyer.Name;
                am.AddressLine1 = buyer.AddressLine1;
                am.AddressLine2 = buyer.AddressLine2;
                am.City = buyer.City;
                am.CompanyEmail = buyer.Email;
                am.CompanyPhone = buyer.Phone;
                am.ZipPostalCode = buyer.ZipPostalCode;
                am.Status = buyer.Status;
                am.BillFrequency = buyer.BillFrequency;
                am.FrequencyValue = (int)buyer.FrequencyValue;
                am.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
                am.ManagerId = (long)buyer.ManagerId;
                am.AlwaysSoldOption = buyer.AlwaysSoldOption;
                am.Description = buyer.Description;
                am.CoolOffEnabled = (buyer.CoolOffEnabled.HasValue ? buyer.CoolOffEnabled.Value : false);
                am.CoolOffStart = (buyer.CoolOffStart.HasValue ? buyer.CoolOffStart.Value : DateTime.UtcNow);
                am.CoolOffEnd = (buyer.CoolOffEnd.HasValue ? buyer.CoolOffEnd.Value : DateTime.UtcNow);
            }

            PrepareModel(am);

            //return View(am);
            return PartialView("Item", am);
        }

        /// <summary>
        /// Displays buyer create/edit partial interface
        /// </summary>
        /// <param name="id">Buyer id</param>
        /// <returns>PartialView result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyer")]
        public ActionResult PartialItem2(long id = 0)
        {
            Buyer buyer = this._buyerService.GetBuyerById(id);

            BuyerModel am = new BuyerModel();

            am.BuyerId = 0;

            ViewBag.UserTypeId = 0;

            if (_appContext.AppUser != null) ViewBag.UserTypeId = _appContext.AppUser.UserType;

            if (buyer != null)
            {
                am.BuyerId = buyer.Id;
                am.CountryId = buyer.CountryId;
                am.StateProvinceId = (buyer.StateProvinceId.HasValue ? buyer.StateProvinceId.Value : 0);
                am.Name = buyer.Name;
                am.AddressLine1 = buyer.AddressLine1;
                am.AddressLine2 = buyer.AddressLine2;
                am.City = buyer.City;
                am.CompanyEmail = buyer.Email;
                am.CompanyPhone = buyer.Phone;
                am.ZipPostalCode = buyer.ZipPostalCode;
                am.Status = buyer.Status;
                am.BillFrequency = buyer.BillFrequency;
                am.FrequencyValue = (int)buyer.FrequencyValue;
                am.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
                am.ManagerId = (long)buyer.ManagerId;
                am.AlwaysSoldOption = buyer.AlwaysSoldOption;
                am.Description = buyer.Description;
                am.CoolOffEnabled = (buyer.CoolOffEnabled.HasValue ? buyer.CoolOffEnabled.Value : false);
                am.CoolOffStart = (buyer.CoolOffStart.HasValue ? buyer.CoolOffStart.Value : DateTime.UtcNow);
                am.CoolOffEnd = (buyer.CoolOffEnd.HasValue ? buyer.CoolOffEnd.Value : DateTime.UtcNow);
            }

            PrepareModel(am);

            //return View(am);
            return PartialView("PartialItem", am);
        }

        /// <summary>
        /// Handles buyer submit action
        /// </summary>
        /// <param name="buyerModel">BuyerModel reference</param>
        /// <param name="returnUrl">Redirect url after success</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(BuyerModel buyerModel, string returnUrl)
        {
            Buyer buyer = null;
            string data1 = "";
            string data2 = "";

            if (_buyerService.GetBuyerByName(buyerModel.Name, buyerModel.BuyerId) != null)
            {
                return Json(new { error = "Buyer name already exists" }, JsonRequestBehavior.AllowGet);
            }

            if (buyerModel.BuyerId == 0)
            {
                buyer = new Buyer();
                buyer.BillFrequency = "m";
                buyer.FrequencyValue = 1;
            }
            else
            {
                buyer = _buyerService.GetBuyerById(buyerModel.BuyerId);

                data1 = "Name: " + buyer.Name + ";" +
                               "CountryId: " + buyer.CountryId.ToString() + ";" +
                               "StateProvinceId: " + buyer.StateProvinceId.ToString() + ";" +
                               "Phone: " + (!string.IsNullOrEmpty(buyer.Phone) ? buyer.Phone.ToString() : "") + ";" +
                               "Email: " + (!string.IsNullOrEmpty(buyer.Email) ? buyer.Email.ToString() : "") + ";" +
                               "City: " + (!string.IsNullOrEmpty(buyer.City) ? buyer.City.ToString() : "") + ";" +
                               "ZipPostalCode: " + (!string.IsNullOrEmpty(buyer.ZipPostalCode) ? buyer.ZipPostalCode.ToString() : "") + ";" +
                               "AddressLine1: " + (!string.IsNullOrEmpty(buyer.AddressLine1) ? buyer.AddressLine1.ToString() : "") + ";" +
                               "AddressLine2: " + (!string.IsNullOrEmpty(buyer.AddressLine2) ? buyer.AddressLine2.ToString() : "") + ";" +
                               "Status: " + buyer.Status.ToString() + ";" +
                               "ManagerId: " + buyer.ManagerId.ToString() + ";" +
                               "AlwaysSoldOption: " + buyer.AlwaysSoldOption.ToString() + ";" +
                               "DailyCap: " + buyer.DailyCap.ToString() + ";" +
                               "MaxDuplicateDays: " + buyer.MaxDuplicateDays.ToString() + ";" +
                               "KeepConsistentLeadId: " + (buyer.CanSendLeadId.HasValue ? false.ToString() : buyer.CanSendLeadId.ToString()) +
                               "DoNotPresentPostMethod: " + buyer.DoNotPresentPostMethod +
                               "DoNotPresentRequest: " + buyer.DoNotPresentRequest +
                               "DoNotPresentResultField: " + buyer.DoNotPresentResultField +
                               "DoNotPresentResultValue: " + buyer.DoNotPresentResultValue +
                               "DoNotPresentStatus: " + (buyer.DoNotPresentStatus.HasValue ? 0.ToString() : buyer.DoNotPresentStatus.ToString()) +
                               "DoNotPresentUrl: " + buyer.DoNotPresentUrl +
                               "AccountId: " + (buyer.AccountId.HasValue ? 0.ToString() : buyer.AccountId.ToString());
            }

            buyer.Name = buyerModel.Name.Trim();
            buyer.CountryId = buyerModel.CountryId;
            buyer.StateProvinceId = (buyerModel.StateProvinceId == 0 ? null : (long?)buyerModel.StateProvinceId);
            buyer.Phone = buyerModel.CompanyPhone;
            buyer.Email = buyerModel.CompanyEmail;
            buyer.City = buyerModel.City;
            buyer.ZipPostalCode = buyerModel.ZipPostalCode;
            buyer.AddressLine1 = buyerModel.AddressLine1;
            buyer.AddressLine2 = buyerModel.AddressLine2;
            buyer.Status = buyerModel.Status;
            buyer.CreatedOn = DateTime.UtcNow;
            buyer.ManagerId = buyerModel.ManagerId;
            buyer.AlwaysSoldOption = buyerModel.AlwaysSoldOption;
            buyer.DailyCap = buyerModel.DailyCap;
            buyer.MaxDuplicateDays = buyerModel.MaxDuplicateDays;
            buyer.Description = buyerModel.Description;
            buyer.ExternalId = buyerModel.ExternalId;
            buyer.CoolOffEnabled = buyerModel.CoolOffEnabled;
            buyer.CoolOffStart = buyerModel.CoolOffStart;
            buyer.CoolOffEnd = buyerModel.CoolOffEnd;

            buyer.DoNotPresentResultField = buyerModel.DoNotPresentResultField;
            buyer.DoNotPresentResultValue = buyerModel.DoNotPresentResultValue;
            buyer.DoNotPresentStatus = buyerModel.DoNotPresentStatus;
            buyer.DoNotPresentUrl = buyerModel.DoNotPresentUrl;
            buyer.DoNotPresentPostMethod = buyerModel.DoNotPresentPostMethod;
            buyer.DoNotPresentRequest = buyerModel.DoNotPresentRequest;
            buyer.CanSendLeadId = buyerModel.CanSendLeadId;
            buyer.AccountId = buyerModel.AccountId;

            BuyerBalance bb = null;

            if (buyerModel.BuyerId == 0)
            {
                bb = new BuyerBalance();
                long newId = _buyerService.InsertBuyer(buyer);
                bb.Credit = buyerModel.Credit;
                bb.Balance = buyerModel.Credit;
                bb.BuyerId = newId;
                bb.PaymentSum = 0;
                bb.SoldSum = 0;
                _accountingService.InsertBuyerBalance(bb);
                this._historyService.AddHistory("BuyerController", HistoryAction.Buyer_Added, "Buyer", newId, "", "", "", this._appContext.AppUser.Id);
            }
            else
            {
                buyer.BillFrequency = buyerModel.BillFrequency;
                buyer.FrequencyValue = buyerModel.FrequencyValue;

                _buyerService.UpdateBuyer(buyer);

                data2 = "Name: " + buyer.Name + ";" +
                               "CountryId: " + buyer.CountryId.ToString() + ";" +
                               "StateProvinceId: " + buyer.StateProvinceId.ToString() + ";" +
                               "Phone: " + (!string.IsNullOrEmpty(buyer.Phone) ? buyer.Phone.ToString() : "") + ";" +
                               "Email: " + (!string.IsNullOrEmpty(buyer.Email) ? buyer.Email.ToString() : "") + ";" +
                               "City: " + (!string.IsNullOrEmpty(buyer.City) ? buyer.City.ToString() : "") + ";" +
                               "ZipPostalCode: " + (!string.IsNullOrEmpty(buyer.ZipPostalCode) ? buyer.ZipPostalCode.ToString() : "") + ";" +
                               "AddressLine1: " + (!string.IsNullOrEmpty(buyer.AddressLine1) ? buyer.AddressLine1.ToString() : "") + ";" +
                               "AddressLine2: " + (!string.IsNullOrEmpty(buyer.AddressLine2) ? buyer.AddressLine2.ToString() : "") + ";" +
                               "Status: " + buyer.Status.ToString() + ";" +
                               "ManagerId: " + buyer.ManagerId.ToString() + ";" +
                               "AlwaysSoldOption: " + buyer.AlwaysSoldOption.ToString() + ";" +
                               "DailyCap: " + buyer.DailyCap.ToString() + ";" +
                               "MaxDuplicateDays: " + buyer.MaxDuplicateDays.ToString() + ";" +
                               "KeepConsistentLeadId: " + (buyer.CanSendLeadId.HasValue ? false.ToString() : buyer.CanSendLeadId.ToString()) +
                               "DoNotPresentPostMethod: " + buyer.DoNotPresentPostMethod +
                               "DoNotPresentRequest: " + buyer.DoNotPresentRequest +
                               "DoNotPresentResultField: " + buyer.DoNotPresentResultField +
                               "DoNotPresentResultValue: " + buyer.DoNotPresentResultValue +
                               "DoNotPresentStatus: " + (buyer.DoNotPresentStatus.HasValue ? 0.ToString() : buyer.DoNotPresentStatus.ToString()) +
                               "DoNotPresentUrl: " + buyer.DoNotPresentUrl +
                               "AccountId: " + (buyer.AccountId.HasValue ? 0.ToString() : buyer.AccountId.ToString());

                this._historyService.AddHistory("BuyerController", HistoryAction.Buyer_Edited, "Buyer", buyerModel.BuyerId, data1, data2, "", this._appContext.AppUser.Id);
            }

            buyerModel.BuyerId = buyer.Id;

            PrepareModel(buyerModel, buyer);

            return View("List");
        }

        /// <summary>
        /// Dashboards the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Dashboard(long id = 0)
        {
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                id = _appContext.AppUser.ParentId;
            }

            Buyer b = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
            if (b != null)
                ViewBag.BuyerCompanyName = (b != null && b.Name != "" ? "Buyer > " + b.Name + " > " : "");


            BuyerModel m = new BuyerModel();
            m.BuyerId = id;
            ViewBag.SelectedBuyerId = id;
            ViewBag.BuyerCompanyName = "";

            return View(m);
        }

        /// <summary>
        /// Partials the dashboard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PartialDashboard(long id = 0)
        {
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                id = _appContext.AppUser.ParentId;

            BuyerModel m = new BuyerModel();
            m.BuyerId = id;
            ViewBag.SelectedBuyerId = id;

            return PartialView("PartialDashboard", m);
        }

        /// <summary>
        /// Channelses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Channels(long id = 0)
        {
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                id = _appContext.AppUser.ParentId;

            Buyer b = _buyerService.GetBuyerById(id);
            if (b != null)
                ViewBag.BuyerCompanyName = (b != null && b.Name != "" ? "Buyer > " + b.Name + " > " : "");

            BuyerModel m = new BuyerModel();
            m.BuyerId = id;

            ViewBag.SelectedBuyerId = id;

            return View(m);
        }

        /// <summary>
        /// Userses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Users(long id = 0)
        {
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                id = _appContext.AppUser.ParentId;

            Buyer b = _buyerService.GetBuyerById(id);
            if (b != null)
                ViewBag.BuyerCompanyName = (b != null && b.Name != "" ? "Buyer > " + b.Name + " > " : "");

            BuyerModel m = new BuyerModel();
            m.BuyerId = id;

            ViewBag.SelectedBuyerId = id;

            return View(m);
        }

        /// <summary>
        /// Reportses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Reports(long id = 0)
        {
            if (id == 0 && _appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    id = _appContext.AppUser.ParentId;
                else
                    if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId) return HttpNotFound();
            }

            Buyer b = _buyerService.GetBuyerById(id);
            if (b != null)
                ViewBag.BuyerCompanyName = (b != null && b.Name != "" ? "Buyer > " + b.Name + " > " : "");

            ViewBag.SelectedBuyerId = id;

            BuyerModel m = new BuyerModel();
            m.BuyerId = id;

            return View(m);
        }

        /// <summary>
        /// Gets buyer list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyers()
        {
            if (_appContext.AppUser == null) return null;

            int campaignId = 0;
            int.TryParse(Request["campaignid"], out campaignId);

            short deleted = 0;

            short.TryParse(Request["d"], out deleted);

            int active = -1;
            int.TryParse(Request["a"], out active);

            List<long> buyerIds = _buyerChannelService.GetAllBuyerChannelsByCampaignId(campaignId).Select(x => x.BuyerId).ToList();

            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            List<Buyer> buyers = (List<Buyer>)this._buyerService.GetAllBuyers(_permissionService.Authorize(PermissionProvider.BuyersShowAll) ? null : _appContext.AppUser, deleted);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = buyers.Count;
            jd.recordsFiltered = buyers.Count;
            foreach (Buyer ai in buyers)
            {
                if (campaignId > 0 && !buyerIds.Contains(ai.Id))
                {
                    continue;
                }

                if (active == 2 && ai.Status == 1) continue;
                else
                if (active == 1 && ai.Status != 1) continue;

                Country country = _countryService.GetCountryById(ai.CountryId);

                Core.Domain.Membership.User user = _userService.GetUserById(ai.ManagerId == null ? 0 : (long)ai.ManagerId);

                string[] names1 = {
                                      ai.Id.ToString(),
                                      permissionService.Authorize(PermissionProvider.BuyersModify) ? "<a href=\"/Management/Buyer/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>" : "<b>" + ai.Name + "</b>",
                                      ai.AlwaysSoldOption == 0 ? "Online" : "Storefront",
                                      country == null ? "" : country.Name,
                                      ai.City,
                                      ai.AddressLine1,
                                      "<a href='" + ai.Email + "'>" + ai.Email + "</a>",
                                      ai.ZipPostalCode,
                                      string.IsNullOrEmpty(ai.Phone) ? "" : ai.Phone.ToString(),
                                      user == null ? "" : user.Username,
                                      ai.Status != 0 ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>",
                                      "<a href='#' onclick='deleteBuyer(" + ai.Id + ")'>" + (ai.Deleted.HasValue ? (ai.Deleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the buyer information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyerInfo()
        {
            long buyerId = 0;

            long.TryParse(Request["buyerId"], out buyerId);

            Buyer buyer = _buyerService.GetBuyerById(buyerId);

            if (buyer == null)
            {
                return Json(new { type = 0 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { type = buyer.AlwaysSoldOption, maxduplicatedays = buyer.MaxDuplicateDays }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the can delete.
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.String.</returns>
        [NonAction]
        public string CheckCanDelete(Buyer buyer)
        {
            List<User> users = (List<User>)_userService.GetUsersByParentId(buyer.Id, SharedData.BuyerUserTypeId, 0);

            if (users.Count > 0)
                return "Can not delete buyer because the are active users.";

            /*foreach (User u in users)
            {
                u.Deleted = (buyer.Deleted.HasValue ? buyer.Deleted.Value : true);
            }*/

            List<BuyerChannel> buyerChannels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannels(buyer.Id, 0);

            if (buyerChannels.Count > 0)
                return "Can not delete buyer because the are active buyer channels.";

            /*foreach (BuyerChannel bc in buyerChannels)
            {
                if (buyer.Deleted.HasValue && buyer.Deleted.Value)
                {
                    List<UserBuyerChannel> userBuyerChannels = (List<UserBuyerChannel>)_userService.GetBuyerChannelUsers(bc.Id);
                    foreach (UserBuyerChannel ubc in userBuyerChannels)
                    {
                        _userService.DetachBuyerChannel(ubc);
                    }
                }

                bc.Deleted = buyer.Deleted;
                _buyerChannelService.UpdateBuyerChannel(bc);
            }*/

            return "";
        }

        /// <summary>
        /// Deletes the buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteBuyer()
        {
            string message = "";

            if (_appContext.AppUser.UserType != SharedData.BuiltInUserTypeId)
            {
                return Json(new { result = false, message = "" }, JsonRequestBehavior.AllowGet);
            }

            long buyerid = 0;

            if (long.TryParse(Request["buyerid"], out buyerid))
            {
                Buyer buyer = _buyerService.GetBuyerById(buyerid);
                if (buyer != null)
                {
                    if (buyer.Deleted.HasValue)
                        buyer.Deleted = !buyer.Deleted.Value;
                    else
                        buyer.Deleted = true;

                    if (buyer.Deleted.Value)
                        message = CheckCanDelete(buyer);

                    if (message.Length > 0)
                    {
                        return Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    _buyerService.UpdateBuyer(buyer);

                    this._historyService.AddHistory("BuyerController", HistoryAction.Buyer_Deleted, "Buyer", buyer.Id, "Name:" + buyer.Name, "", "", this._appContext.AppUser.Id);
                }
            }

            return Json(new { result = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Logins as.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult LoginAs()
        {
            long id = 0;

            long.TryParse(Request["id"], out id);

            Buyer buyer = _buyerService.GetBuyerById(id);

            if (buyer != null)
            {
                User user = _userService.GetUserByParentId(buyer.Id, SharedData.BuyerUserTypeId);

                if (user != null)
                {
                    _appContext.SetBackLoginUser(_appContext.AppUser);

                    _authenticationService.SignOut(_appContext);

                    _authenticationService.SignIn(user, false);
                    _appContext.AppUser = user;

                    user.LoginDate = DateTime.UtcNow;
                    user.FailedPasswordAttemptCount = 0;

                    _userService.UpdateUser(user);

                    return RedirectToAction("Dashboard", "Buyer", new { area = "management" });
                }
            }

            return Redirect(Helper.GetBaseUrl(Request) + "/Management/Home/Dashboard");
        }

        /// <summary>
        /// Bulks the filter changes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult BulkFilterChanges()
        {
            BuyerReportModel model = new BuyerReportModel();
            model.BaseUrl = Helper.GetBaseUrl(Request);

            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                model.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(0);
            foreach (var b in buyers)
            {
                model.ListBuyers.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(0);
            foreach (var a in affiliates)
            {
                model.ListAffiliates.Add(new SelectListItem() { Text = a.Name, Value = a.Id.ToString() });
            }

            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0);
            foreach (var a in affiliateChannels)
            {
                model.ListAffiliateChannels.Add(new SelectListItem() { Text = a.Name, Value = a.Id.ToString() });
            }

            return View(model);
        }

        /// <summary>
        /// Applies the bulk filters.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult ApplyBulkFilters()
        {
            List<object> results = new List<object>();

            dynamic filters = JsonConvert.DeserializeObject(Request["filters"]);

            string[] buyerChannelIds = Request["buyerChannelIds"].Split(new char[1] { ',' });

            for (int j = 0; j < buyerChannelIds.Length; j++)
            {
                BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(long.Parse(buyerChannelIds[j]));
                if (buyerChannel == null) continue;

                bool zipFound = false;
                bool ageFound = false;
                bool stateFound = false;

                for (int i = 0; i < filters.Count; i++)
                {
                    string field = filters[i]["field"].ToString();
                    string condition = filters[i]["condition"].ToString();
                    string value = filters[i]["value"].ToString();

                    long campaignTemplateId = long.Parse(field);

                    CampaignField campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(campaignTemplateId);

                    if (campaignTemplate == null) continue;

                    BuyerChannelTemplate buyerChannelTemplate = _buyerChannelTemplateService.GetBuyerChannelTemplate(campaignTemplateId);

                    if (buyerChannelTemplate == null)
                    {
                        results.Add(new { message = "Campaign field '" + campaignTemplate.TemplateField + "' is not integrated with it's corresponding buyer channel's field" });
                        continue;
                    }

                    var items = _buyerChannelFilterConditionService.GetFilterConditionsByBuyerChannelIdAndCampaignTemplateId(buyerChannel.Id, campaignTemplateId);
                    BuyerChannelFilterCondition fc = null;

                    if (items.Count > 0)
                        fc = items[0];

                    bool isNew = false;
                    if (fc == null)
                    {
                        isNew = true;
                        fc = new BuyerChannelFilterCondition();
                    }

                    if (campaignTemplate.Validator == 7)
                    {
                        zipFound = true;
                        buyerChannel.EnableZipCodeTargeting = true;
                        buyerChannel.ZipCodeTargeting = value;
                        buyerChannel.ZipCodeCondition = short.Parse(condition);
                    }
                    else if (campaignTemplate.Validator == 11)
                    {
                        stateFound = true;
                        buyerChannel.EnableStateTargeting = true;
                        buyerChannel.StateTargeting = value;
                        buyerChannel.StateCondition = short.Parse(condition);
                    }
                    else if (campaignTemplate.Validator == 14)
                    {
                        ageFound = true;
                        buyerChannel.EnableAgeTargeting = true;

                        string[] values = value.Split(new char[1] { '-' });

                        short v = 0;
                        if (values.Length > 0)
                            short.TryParse(values[0], out v);

                        short v2 = 0;
                        if (values.Length > 1)
                            short.TryParse(values[1], out v2);

                        buyerChannel.MinAgeTargeting = v;
                        buyerChannel.MaxAgeTargeting = v2;
                    }

                    fc.BuyerChannelId = buyerChannel.Id;
                    fc.Condition = short.Parse(condition);
                    fc.Value = value.Trim();
                    fc.ConditionOperator = short.Parse(condition);
                    fc.CampaignTemplateId = campaignTemplateId;
                    if (isNew)
                        _buyerChannelFilterConditionService.InsertFilterCondition(fc);
                    else
                        _buyerChannelFilterConditionService.UpdateFilterCondition(fc);
                }

                if (!zipFound)
                {
                    buyerChannel.EnableZipCodeTargeting = false;
                    buyerChannel.ZipCodeTargeting = "";
                }

                if (!stateFound)
                {
                    buyerChannel.EnableStateTargeting = false;
                    buyerChannel.StateTargeting = "";
                }

                if (!ageFound)
                {
                    buyerChannel.EnableAgeTargeting = false;
                    buyerChannel.MaxAgeTargeting = 0;
                    buyerChannel.MinAgeTargeting = 0;
                }

                results.Add(new { message = "Filters for '" + buyerChannel.Name + "' are updated." });
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bulks the filter changes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult BulkAffiliateChannelSettings()
        {
            BuyerReportModel model = new BuyerReportModel();
            model.BaseUrl = Helper.GetBaseUrl(Request);

            List<BuyerChannel> buyerChannels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannels(0);
            foreach (var a in buyerChannels)
            {
                model.ListBuyerChannels.Add(new SelectListItem() { Text = a.Name, Value = a.Id.ToString() });
            }


            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0);
            foreach (var a in affiliateChannels)
            {
                model.ListAffiliateChannels.Add(new SelectListItem() { Text = a.Name, Value = a.Id.ToString() });
            }

            return View(model);
        }

        /// <summary>
        /// Applies the bulk filters.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult ApplyBulkFilters2()
        {
            string[] buyerChannelIdsStr = Request["buyerChannelIds"].Split(new char[1] { ',' });
            string[] affiliateChannelIdsStr = Request["affiliateChannelIds"].Split(new char[1] { ',' });
            string action = Request["action"];

            foreach (string buyerChannelIdStr in buyerChannelIdsStr)
            {
                BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(int.Parse(buyerChannelIdStr));

                if (buyerChannel != null)
                {
                    string allowedAffiliates = "";
                    if (action == "2")
                    {
                        allowedAffiliates = !string.IsNullOrEmpty(buyerChannel.AllowedAffiliateChannels) ? buyerChannel.AllowedAffiliateChannels : "";
                    }

                    foreach (string affiliateChannelIdStr in affiliateChannelIdsStr)
                    {
                        if (action == "1")
                        {
                            allowedAffiliates += $":{affiliateChannelIdStr};";
                        }
                        else
                        {
                            allowedAffiliates = allowedAffiliates.Replace($":{affiliateChannelIdStr};", "");
                        }
                    }

                    buyerChannel.AllowedAffiliateChannels = allowedAffiliates;
                    _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                }
            }


            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}