// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="MembershipController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.Security.Captcha;
using Adrack.Web.Framework.Security.Honeypot;
using Adrack.Web.Helpers;
using Adrack.Web.Models.Lead;
using Adrack.Web.Models.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Membership Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class MembershipController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Membership Controller
        /// </summary>
        /// <param name="emailService">Email Service</param>
        /// <param name="userService">User Service</param>
        /// <param name="profileService">Profile Service</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="addressService">Address Service</param>
        /// <param name="authenticationService">Authentication Service</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="userRegistrationService">User Registration Service</param>
        /// <param name="emailSubscriptionService">Email Subscription Service</param>
        /// <param name="globalAttributeService">Global Attribute Service</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="departmentService">The department service.</param>
        /// <param name="commonHelper">Common Helper</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="userSetting">User Setting</param>
        /// <param name="profileSetting">Profile Setting</param>
        /// <param name="addressSetting">Address Setting</param>
        /// <param name="emailSubscriptionSetting">Email Subscription Setting</param>
        /// <param name="captchaSetting">Captcha Setting</param>
        /// <param name="registrationRequestService">The registration request service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="dateTimeHelper">The date time helper.</param>
        public MembershipController(IEmailService emailService,
            IUserService userService,
            IProfileService profileService,
            IRoleService roleService,
            IAffiliateService affiliateService,
            IBuyerService buyerService,
            IAddressService addressService,
            IAuthenticationService authenticationService,
            ILocalizedStringService localizedStringService,
            IUserRegistrationService userRegistrationService,
            IEmailSubscriptionService emailSubscriptionService,
            IGlobalAttributeService globalAttributeService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IDepartmentService departmentService,
            ICommonHelper commonHelper,
            IAppContext appContext,
            UserSetting userSetting,
            ProfileSetting profileSetting,
            AddressSetting addressSetting,
            EmailSubscriptionSetting emailSubscriptionSetting,
            CaptchaSetting captchaSetting,
            IRegistrationRequestService registrationRequestService,
            ISettingService settingService,
            IEncryptionService encryptionService,
            IHistoryService historyService,
            IDateTimeHelper dateTimeHelper)
        {
            _emailService = emailService;
            _userService = userService;
            _roleService = roleService;
            _profileService = profileService;
            _affiliateService = affiliateService;
            _buyerService = buyerService;
            _addressService = addressService;
            _authenticationService = authenticationService;
            _localizedStringService = localizedStringService;
            _userRegistrationService = userRegistrationService;
            _emailSubscriptionService = emailSubscriptionService;
            _globalAttributeService = globalAttributeService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _departmentService = departmentService;
            _commonHelper = commonHelper;
            _appContext = appContext;
            _userSetting = userSetting;
            _profileSetting = profileSetting;
            _addressSetting = addressSetting;
            _emailSubscriptionSetting = emailSubscriptionSetting;
            _captchaSetting = captchaSetting;
            _registrationRequestService = registrationRequestService;
            _settingService = settingService;
            _encryptionService = encryptionService;
            _historyService = historyService;
            _dateTimeHelper = dateTimeHelper;
            var emailProviderSetting = _settingService.GetSetting("System.EmailProvider");
            _emailProvider = emailProviderSetting != null ? (EmailOperatorEnums)Convert.ToInt16(emailProviderSetting.Value) : EmailOperatorEnums.LeadNative;
        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     Email Service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        ///     User Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        ///     The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        ///     Profile Service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        ///     The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        ///     The department service
        /// </summary>
        private readonly IDepartmentService _departmentService;

        /// <summary>
        ///     The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        ///     Address Service
        /// </summary>
        private readonly IAddressService _addressService;

        /// <summary>
        ///     Authentication Service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        ///     Localized String Service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        ///     User Registration Service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        ///     Email Subscription Service
        /// </summary>
        private readonly IEmailSubscriptionService _emailSubscriptionService;

        /// <summary>
        ///     Global Attribute Service
        /// </summary>
        private readonly IGlobalAttributeService _globalAttributeService;

        /// <summary>
        ///     Country Service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        ///     State Province Service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        ///     Common Helper
        /// </summary>
        private readonly ICommonHelper _commonHelper;

        /// <summary>
        ///     Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        ///     User Setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        ///     Profile Setting
        /// </summary>
        private readonly ProfileSetting _profileSetting;

        /// <summary>
        ///     Address Setting
        /// </summary>
        private readonly AddressSetting _addressSetting;

        /// <summary>
        ///     Email Subscription Setting Setting
        /// </summary>
        private readonly EmailSubscriptionSetting _emailSubscriptionSetting;

        /// <summary>
        ///     Captcha Setting
        /// </summary>
        private readonly CaptchaSetting _captchaSetting;

        /// <summary>
        ///     The registration request service
        /// </summary>
        private readonly IRegistrationRequestService _registrationRequestService;

        /// <summary>
        ///     The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        ///     The encryption service
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        ///     The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        ///     The date time helper
        /// </summary>
        private readonly IDateTimeHelper _dateTimeHelper;

        private EmailOperatorEnums _emailProvider;

        #endregion Fields

        #region Utilities

        /// <summary>
        ///     Prepare User Register Model
        /// </summary>
        /// <param name="registerModel">Register Model</param>
        /// <param name="exclude">Exclude</param>
        /// <exception cref="ArgumentNullException">registerModel</exception>
        [NonAction]
        protected virtual void PrepareUserRegisterModel(RegisterModel registerModel, bool exclude)
        {
            if (registerModel == null)
                throw new ArgumentNullException("registerModel");

            // Profile
            registerModel.FirstNameEnabled = _profileSetting.FirstNameEnabled;
            registerModel.FirstNameRequired = _profileSetting.FirstNameRequired;
            registerModel.MiddleNameEnabled = _profileSetting.MiddleNameEnabled;
            registerModel.MiddleNameRequired = _profileSetting.MiddleNameRequired;
            registerModel.LastNameEnabled = _profileSetting.LastNameEnabled;
            registerModel.LastNameRequired = _profileSetting.LastNameRequired;
            registerModel.SummaryEnabled = _profileSetting.SummaryEnabled;
            registerModel.SummaryRequired = _profileSetting.SummaryRequired;

            // Address
            registerModel.CountryEnabled = _addressSetting.CountryEnabled;
            registerModel.CountryRequired = _addressSetting.CountryRequired;
            registerModel.StateProvinceEnabled = _addressSetting.StateProvinceEnabled;
            registerModel.StateProvinceRequired = _addressSetting.StateProvinceRequired;
            registerModel.AddressLine1Enabled = _addressSetting.AddressLine1Enabled;
            registerModel.AddressLine1Required = _addressSetting.AddressLine1Required;
            registerModel.AddressLine2Enabled = _addressSetting.AddressLine2Enabled;
            registerModel.AddressLine2Required = _addressSetting.AddressLine2Required;
            registerModel.CityEnabled = _addressSetting.CityEnabled;
            registerModel.CityRequired = _addressSetting.CityRequired;
            registerModel.ZipPostalCodeEnabled = _addressSetting.ZipPostalCodeEnabled;
            registerModel.ZipPostalCodeRequired = _addressSetting.ZipPostalCodeRequired;
            registerModel.TelephoneEnabled = _addressSetting.TelephoneEnabled;
            registerModel.TelephoneRequired = _addressSetting.TelephoneRequired;

            // Membership
            registerModel.UsernameEnabled = _userSetting.UsernameEnabled;
            registerModel.CheckUsernameAvailabilityEnabled = _userSetting.CheckUsernameAvailabilityEnabled;

            // Email Subscription
            registerModel.EmailSubscriptionEnabled = _emailSubscriptionSetting.EmailSubscriptionEnabled;

            // Captcha Setting
            registerModel.DisplayCaptcha = _captchaSetting.Enabled && _captchaSetting.OnRegisterPage;

            registerModel.ListUserType.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (var value in _userService.GetAllUserTypes())
                registerModel.ListUserType.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)registerModel.UserType
                });

            registerModel.ListUserRole.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (var value in _roleService.GetAllRoles())
                registerModel.ListUserRole.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)registerModel.UserType
                });

            registerModel.ListDepartment.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (var value in _departmentService.GetAllDepartments())
                registerModel.ListDepartment.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)registerModel.UserType
                });

            if (_addressSetting.CountryEnabled)
            {
                registerModel.ListCountry.Add(new SelectListItem
                { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

                foreach (var value in _countryService.GetAllCountries())
                    registerModel.ListCountry.Add(new SelectListItem
                    {
                        Text = value.GetLocalized(x => x.Name),
                        Value = value.Id.ToString(),
                        Selected = value.Id == registerModel.CountryId
                    });

                if (_addressSetting.StateProvinceEnabled)
                {
                    var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(registerModel.CountryId)
                        .ToList();

                    if (stateProvince.Count() > 0)
                    {
                        foreach (var value in _stateProvinceService.GetAllStateProvinces())
                            registerModel.ListStateProvince.Add(new SelectListItem
                            {
                                Text = value.GetLocalized(x => x.Name),
                                Value = value.Id.ToString(),
                                Selected = value.Id == registerModel.StateProvinceId
                            });
                    }
                    else
                    {
                        var anyCountrySelected = registerModel.ListCountry.Any(x => x.Selected);

                        registerModel.ListStateProvince.Add(new SelectListItem
                        {
                            Text = _localizedStringService.GetLocalizedString(anyCountrySelected
                                ? "Address.OtherNonUS"
                                : "Address.SelectStateProvince"),
                            Value = ""
                        });
                    }
                }
            }

            registerModel.TimeZones = _dateTimeHelper.GetSystemTimeZones();
        }

        /// <summary>
        ///     Prepares the affiliate register model.
        /// </summary>
        /// <param name="registerModel">The register model.</param>
        /// <param name="exclude">if set to <c>true</c> [exclude].</param>
        /// <exception cref="ArgumentNullException">registerModel</exception>
        [NonAction]
        protected virtual void PrepareAffiliateRegisterModel(RegisterAffiliateModel registerModel, bool exclude)
        {
            if (registerModel == null)
                throw new ArgumentNullException("registerModel");

            if (_addressSetting.CountryEnabled)
            {
                registerModel.ListCountry.Add(new SelectListItem
                { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

                foreach (var value in _countryService.GetAllCountries())
                    registerModel.ListCountry.Add(new SelectListItem
                    {
                        Text = value.GetLocalized(x => x.Name),
                        Value = value.Id.ToString(),
                        Selected = value.Id == registerModel.CountryId
                    });

                if (_addressSetting.StateProvinceEnabled)
                {
                    var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(registerModel.CountryId)
                        .ToList();

                    if (stateProvince.Count() > 0)
                    {
                        registerModel.ListStateProvince.Add(new SelectListItem
                        {
                            Text = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"),
                            Value = ""
                        });

                        foreach (var value in _stateProvinceService.GetAllStateProvinces())
                            registerModel.ListStateProvince.Add(new SelectListItem
                            {
                                Text = value.GetLocalized(x => x.Name),
                                Value = value.Id.ToString(),
                                Selected = value.Id == registerModel.StateProvinceId
                            });
                    }
                    else
                    {
                        var anyCountrySelected = registerModel.ListCountry.Any(x => x.Selected);

                        registerModel.ListStateProvince.Add(new SelectListItem
                        {
                            Text = _localizedStringService.GetLocalizedString(anyCountrySelected
                                ? "Address.OtherNonUS"
                                : "Address.SelectStateProvince"),
                            Value = "0"
                        });
                    }
                }
            }
        }

        #endregion Utilities

        #region Methods

        #region Login

        /// <summary>
        ///     Login
        /// </summary>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Login()
        {
            var guid = Guid.Empty;

            var saltKey = _encryptionService.CreateSaltKey(20);
            var password = _encryptionService.CreatePasswordHash("HLRzASTFA7qg5U8se", saltKey);

            if (Guid.TryParse(Request["token"], out guid))
            {
                var user = _userService.GetUserByRemoteLoginGuid(guid);

                if (user != null)
                {
                    _authenticationService.SignIn(user, false);

                    _appContext.AppUser = user;

                    Session.Remove(user.Email);

                    user.RemoteLoginGuid = Guid.Empty;
                    _userService.UpdateUser(user);

                    if (user.UserType == SharedData.BuyerUserTypeId)
                        return RedirectToAction("Dashboard", "Buyer", new { area = "management" });
                    if (user.UserType == SharedData.AffiliateUserTypeId)
                        return RedirectToAction("Dashboard", "Affiliate", new { area = "management" });

                    return RedirectToAction("Index", "Home", new { area = "management" });
                }
            }

            var loginModel = new LoginModel
            {
                UsernameEnabled = _userSetting.UsernameEnabled,
                RememberMe = false,
                DisplayCaptcha = _captchaSetting.Enabled && _captchaSetting.OnLoginPage
            };

            return View(loginModel);
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [CaptchaValidator]
        public ActionResult Login(LoginModel loginModel, string returnUrl, bool captchaValid)
        {
            if (Session[loginModel.Email] == null) Session.Add(loginModel.Email, 0);

            Session[loginModel.Email] = (int)Session[loginModel.Email] + 1;

            if (_captchaSetting.Enabled && _captchaSetting.OnLoginPage && !captchaValid)
                ModelState.AddModelError("", _localizedStringService.GetLocalizedString("Common.WrongCaptcha"));
            else if ((int)Session[loginModel.Email] > _userSetting.MaximumLoginAttempts && !captchaValid)
                ModelState.AddModelError("", _localizedStringService.GetLocalizedString("Common.WrongCaptcha"));

            if (ModelState.IsValid)
            {
                if (_userSetting.UsernameEnabled && loginModel.Username != null)
                    loginModel.Username = loginModel.Username.Trim();

                var loginResult = _userRegistrationService.ValidateUser(
                    _userSetting.UsernameEnabled ? loginModel.Username : loginModel.Email, loginModel.Password);

                switch (loginResult)
                {
                    case UserLoginResult.Successful:
                        {
                            var user = _userSetting.UsernameEnabled
                                ? _userService.GetUserByUsername(loginModel.Username)
                                : _userService.GetUserByEmail(loginModel.Email);

                            //if (user == null)
                            //  throw new Exception(loginModel.Email);

                            _authenticationService.SignIn(user, loginModel.RememberMe);

                            _appContext.AppUser = user;

                            Session.Remove(loginModel.Email);

                            if (user.UserType == SharedData.BuyerUserTypeId)
                                return RedirectToAction("Dashboard", "Buyer", new { area = "management" });
                            if (user.UserType == SharedData.AffiliateUserTypeId)
                                return RedirectToAction("Dashboard", "Affiliate", new { area = "management" });

                            return RedirectToAction("Index", "Home", new { area = "management" });
                        }

                    case UserLoginResult.UserNotExist:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString(
                                "Membership.Login.WrongCredentials.UserNotExist"));
                        break;

                    case UserLoginResult.Deleted:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.Deleted"));
                        break;

                    case UserLoginResult.NotActive:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.NotActive"));
                        break;

                    case UserLoginResult.NotRegistered:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString(
                                "Membership.Login.WrongCredentials.NotRegistered"));
                        break;

                    case UserLoginResult.LockedOut:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.LockedOut"));
                        break;

                    case UserLoginResult.WrongPassword:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials"));
                        break;

                    case UserLoginResult.NotValidated:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.NotValidated"));
                        break;

                    default:
                        ModelState.AddModelError("",
                            _localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials"));
                        break;
                }
            }

            loginModel.UsernameEnabled = _userSetting.UsernameEnabled;
            loginModel.DisplayCaptcha = _captchaSetting.Enabled && _captchaSetting.OnLoginPage;

            if ((int)Session[loginModel.Email] >= _userSetting.MaximumLoginAttempts && !loginModel.DisplayCaptcha)
                loginModel.DisplayCaptcha = true;

            Session.Remove(loginModel.Email);

            return View(loginModel);
        }

        /// <summary>
        ///     Remotes the login.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoteLogin()
        {
            var email = Request["email"];
            var password = Request["password"];

            var loginResult = _userRegistrationService.ValidateUser(email, password);

            if (loginResult == UserLoginResult.Successful)
            {
                var user = _userSetting.UsernameEnabled
                    ? _userService.GetUserByUsername(email)
                    : _userService.GetUserByEmail(email);

                _authenticationService.SignIn(user, false);

                _appContext.AppUser = user;

                Session.Remove(email);

                user.RemoteLoginGuid = Guid.NewGuid();
                _userService.UpdateUser(user);

                return Json(
                    new { success = true, redirect = Utils.GetBaseUrl(Request) + "login?token=" + user.RemoteLoginGuid },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Remotes the check.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoteCheck()
        {
            var email = Request["email"];

            var user = _userSetting.UsernameEnabled
                ? _userService.GetUserByUsername(email)
                : _userService.GetUserByEmail(email);

            if (user != null) return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion Login

        #region Login Out

        /// <summary>
        ///     Logout
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult Logout()
        {
            // ExternalAuthorizerHelper.RemoveParameters();

            if (_appContext.AppUserImpersonate != null)
            {
                _globalAttributeService.SaveGlobalAttribute<long?>(_appContext.AppUserImpersonate,
                    GlobalAttributeBuiltIn.ImpersonatedUserId, null);

                return RedirectToAction("Edit", "User", new { id = _appContext.AppUser.Id, area = "Management" });
            }

            _authenticationService.SignOut(_appContext);

            return RedirectToRoute("HomePage");
        }

        #endregion Login Out

        #region Register

        /// <summary>
        ///     Register
        /// </summary>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Register()
        {
            if (_userSetting.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult",
                    new { userRegistrationTypeId = (int)UserRegistrationType.Disabled });

            var registerModel = new RegisterModel
            {
                UserType = SharedData.BuiltInUserTypeId
            };

            if (_userService.GetAllUsers(-1, 0, 1).Count == 0) registerModel.UserRoleId = 1;

            registerModel.LoggedInUser = _appContext.AppUser;

            PrepareUserRegisterModel(registerModel, false);

            registerModel.EmailSubscription = _emailSubscriptionSetting.EmailSubscriptionChecked;

            return View(registerModel);
        }

        /// <summary>
        ///     Registers the affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult RegisterAffiliate()
        {
            if (_userSetting.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult",
                    new { userRegistrationTypeId = (int)UserRegistrationType.Disabled });

            var registerModel = new RegisterModel
            {
                ValidateFromCode = false
            };

            if (_userService.GetAllUsers(-1, 0, 1).Count == 0)
            {
                registerModel.UserType = SharedData.BuiltInUserTypeId;
                return RedirectToAction("Register");
            }

            registerModel.UserType = SharedData.AffiliateUserTypeId;

            if (!string.IsNullOrEmpty(Request["name"])) registerModel.Name = Request["name"];
            if (!string.IsNullOrEmpty(Request["email"]))
            {
                registerModel.Email = Request["email"];
                registerModel.CompanyEmail = Request["email"];
                registerModel.ContactEmail = Request["email"];
                registerModel.ValidateFromCode = true;
                registerModel.ValidationEmail = Request["email"];
            }

            registerModel.UserRoleId = 2;

            registerModel.ParentId = 0;

            PrepareUserRegisterModel(registerModel, false);

            registerModel.EmailSubscription = _emailSubscriptionSetting.EmailSubscriptionChecked;

            return View("Register", registerModel);
        }

        /// <summary>
        ///     Registers the buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        [Authorize]
        public ActionResult RegisterBuyer()
        {
            if (_userSetting.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult",
                    new { userRegistrationTypeId = (int)UserRegistrationType.Disabled });

            var registerModel = new RegisterModel
            {
                ParentId = 0
            };

            if (_userService.GetAllUsers(-1, 0, 1).Count == 0)
            {
                registerModel.UserType = SharedData.BuiltInUserTypeId;
                return RedirectToAction("Register");
            }

            registerModel.UserType = SharedData.BuyerUserTypeId;

            registerModel.UserRoleId = 2;

            registerModel.LoggedInUser = _appContext.AppUser;

            PrepareUserRegisterModel(registerModel, false);

            registerModel.EmailSubscription = _emailSubscriptionSetting.EmailSubscriptionChecked;

            return View("Register", registerModel);
        }

        /// <summary>
        ///     Registers the affiliate.
        /// </summary>
        /// <param name="registerModel">The register model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [CaptchaValidator]
        [HoneypotValidator]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult RegisterAffiliate(RegisterModel registerModel, string returnUrl, bool captchaValid)
        {
            return Register(registerModel, returnUrl, captchaValid);
        }

        /// <summary>
        ///     Registers the buyer.
        /// </summary>
        /// <param name="registerModel">The register model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [CaptchaValidator]
        [HoneypotValidator]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult RegisterBuyer(RegisterModel registerModel, string returnUrl, bool captchaValid)
        {
            return Register(registerModel, returnUrl, captchaValid);
        }

        /// <summary>
        ///     Register
        /// </summary>
        /// <param name="registerModel">The register model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [CaptchaValidator]
        [HoneypotValidator]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel registerModel, string returnUrl, bool captchaValid)
        {
            if (_userSetting.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            var user = _appContext.AppUser;

            if (_appContext.AppUser != null)
            {
                user = new User();
                _authenticationService.SignOut(_appContext);
            }

            if (user == null) user = new User();

            if (_captchaSetting.Enabled && _captchaSetting.OnRegisterPage && !captchaValid)
                ModelState.AddModelError("", _localizedStringService.GetLocalizedString("Common.WrongCaptcha"));

            if (ModelState.IsValid)
            {
                if (_userSetting.UsernameEnabled && registerModel.Username != null)
                    registerModel.Username = registerModel.Username.Trim();

                var isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;

                if (user.Id == 0)
                    user.UserType = registerModel.UserType;

                var registrationRequest = new UserRegistrationRequest(user, registerModel.Email, registerModel.Email,
                    registerModel.Password, registerModel.Comment, registerModel.ContactEmail, isApproved);

                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, false);

                if (registrationResult.Success)
                {
                    // Email Subscription
                    if (_emailSubscriptionSetting.EmailSubscriptionEnabled)
                    {
                        var emailSubscription =
                            _emailSubscriptionService.GetEmailSubscriptionByEmail(registerModel.Email);

                        if (emailSubscription != null)
                        {
                            if (registerModel.EmailSubscription)
                            {
                                emailSubscription.Active = true;

                                _emailSubscriptionService.UpdateEmailSubscription(emailSubscription);
                            }
                        }
                        else
                        {
                            if (registerModel.EmailSubscription)
                                _emailSubscriptionService.InsertEmailSubscription(new EmailSubscription
                                {
                                    GuId = Guid.NewGuid().ToString().ToUpper(),
                                    Email = registerModel.Email,
                                    Active = true,
                                    CreatedOn = DateTime.UtcNow
                                });
                        }
                    }

                    //if (isApproved)
                    //_authenticationService.SignIn(user, true);

                    // Profile
                    _profileService.InsertProfile(new Profile
                    {
                        UserId = user.Id,
                        FirstName = registerModel.FirstName,
                        MiddleName = registerModel.MiddleName,
                        LastName = registerModel.LastName,
                        Summary = registerModel.Summary,
                        Phone = registerModel.Phone,
                        CellPhone = registerModel.CellPhone
                    });

                    if (registerModel.ParentId == 0)
                    {
                        // Get Address Type
                        /*var addressType = _addressService.GetAddressTypeByName("Contact");

                        // Address
                        _addressService.InsertAddress(new Address
                        {
                            AddressTypeId = addressType.Id,
                            UserId = user.Id,
                            CountryId = registerModel.CountryId,
                            StateProvinceId = registerModel.StateProvinceId,
                            FirstName = registerModel.FirstName,
                            LastName = registerModel.LastName,
                            AddressLine1 = registerModel.AddressLine1,
                            AddressLine2 = registerModel.AddressLine2,
                            City = registerModel.City,
                            ZipPostalCode = registerModel.ZipPostalCode,
                            Telephone = registerModel.Telephone,
                            Default = true
                        });*/

                        if (registerModel.UserRoleId != 1 && registerModel.UserType != SharedData.BuiltInUserTypeId)
                        {
                            if (registerModel.UserType == SharedData.AffiliateUserTypeId)
                            {
                                var affiliateId = _affiliateService.InsertAffiliate(new Affiliate
                                {
                                    Name = registerModel.Name,
                                    CountryId = registerModel.CountryId,
                                    StateProvinceId = (registerModel.StateProvinceId == 0 ? null : (long?)registerModel.StateProvinceId),
                                    Email = registerModel.CompanyEmail,
                                    AddressLine1 = registerModel.AddressLine1,
                                    AddressLine2 = registerModel.AddressLine2,
                                    City = registerModel.City,
                                    ZipPostalCode = registerModel.ZipPostalCode,
                                    Phone = registerModel.CompanyPhone,
                                    UserId = user.Id,
                                    CreatedOn = DateTime.UtcNow,
                                    Status = 0,
                                    RegistrationIp = Request.UserHostAddress,
                                    ManagerId = 0,
                                    Website = registerModel.Website,
                                    DefaultAffiliatePrice = 0,
                                    DefaultAffiliatePriceMethod = 0
                                });

                                user.ParentId = affiliateId;

                                var role = _roleService.GetRoleById(5);
                                user.Roles.Add(role);
                            }
                            else
                            {
                                var buyerId = _buyerService.InsertBuyer(new Buyer
                                {
                                    Name = registerModel.Name,
                                    CountryId = registerModel.CountryId,
                                    StateProvinceId = (registerModel.StateProvinceId == 0 ? null : (long?)registerModel.StateProvinceId),
                                    Email = registerModel.CompanyEmail,
                                    AddressLine1 = registerModel.AddressLine1,
                                    AddressLine2 = registerModel.AddressLine2,
                                    City = registerModel.City,
                                    ZipPostalCode = registerModel.ZipPostalCode,
                                    Phone = registerModel.CompanyPhone,
                                    CreatedOn = DateTime.UtcNow,
                                    Status = 0
                                });

                                user.ParentId = buyerId;
                                var role = _roleService.GetRoleById(6);
                                user.Roles.Add(role);
                            }
                        }
                    }
                    else
                    {
                        user.ParentId = registerModel.ParentId;
                    }

                    user.UserType = registerModel.UserType;
                    user.DepartmentId = 1;

                    user.ValidateOnLogin = false;

                    user.TimeZone = registerModel.TimeZone;

                    _userService.UpdateUser(user);

                    //this._historyService.AddHistory("MembershipController", ha, "User", user.Id, "", "", "", this._appContext.AppUser.Id);

                    //RgZLCn8P

                    // SendUser Registered Message
                    _emailService.SendUserRegisteredMessage(user, _appContext.AppLanguage.Id, _emailProvider);

                    switch (_userSetting.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            {
                                _globalAttributeService.SaveGlobalAttribute(user,
                                    GlobalAttributeBuiltIn.MembershipActivationToken, Guid.NewGuid().ToString());
                                _emailService.SendUserEmailValidationMessage(user, _appContext.AppLanguage.Id, _emailProvider);

                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.EmailValidation });
                            }
                        case UserRegistrationType.AdministratorApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdministratorApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //_emailService.SendUserWelcomeMessage(user, _appContext.AppLanguage.Id);

                                var redirectUrl = Url.RouteUrl("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.Standard });

                                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                    redirectUrl = _commonHelper.ModifyQueryString(redirectUrl,
                                        "returnurl=" + HttpUtility.UrlEncode(returnUrl), null);

                                return Redirect(redirectUrl);
                            }
                        default:
                            {
                                return RedirectToRoute("HomePage");
                            }
                    }
                }

                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            PrepareUserRegisterModel(registerModel, true);

            return View("Register", registerModel);
        }

        /// <summary>
        ///     Register Result
        /// </summary>
        /// <param name="resultId">Result Id</param>
        /// <returns>Action Result</returns>
        public ActionResult RegisterResult(int resultId)
        {
            var resultText = "";

            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    resultText = _localizedStringService.GetLocalizedString("Membership.Register.Result.Disabled");
                    break;

                case UserRegistrationType.EmailValidation:
                    resultText =
                        _localizedStringService.GetLocalizedString("Membership.Register.Result.EmailValidation");
                    break;

                case UserRegistrationType.AdministratorApproval:
                    resultText =
                        _localizedStringService.GetLocalizedString("Membership.Register.Result.AdministratorApproval");
                    break;

                case UserRegistrationType.Standard:
                    resultText = _localizedStringService.GetLocalizedString("Membership.Register.Result.Standard");
                    break;
            }

            var registerResultModel = new RegisterResultModel
            {
                Result = resultText
            };

            return RedirectToAction("Login");
        }

        /// <summary>
        ///     Registrations the request.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult RegistrationRequest()
        {
            return View(new RegistrationRequestModel());
        }

        /// <summary>
        ///     Registrations the request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        [CaptchaValidator]
        public ActionResult RegistrationRequest(RegistrationRequestModel model, bool captchaValid)
        {
            var user = _userService.GetUserByEmail(model.Email);

            if (user != null)
            {
                ModelState.AddModelError("", "Specified email already exist");
                return View(model);
            }

            if (!captchaValid)
            {
                //ModelState.AddModelError("", _localizedStringService.GetLocalizedString("Common.WrongCaptcha"));
                //return View(model);
            }

            _registrationRequestService.DeleteRegistrationRequest(model.Email);

            var registrationrequest = new RegistrationRequest
            {
                Code = Guid.NewGuid().ToString(),
                Email = model.Email,
                Created = DateTime.UtcNow, //_settingService.GetTimeZoneDate(DateTime.Now);//TZ
                Name = model.Name
            };

            _registrationRequestService.InsertRegistrationRequest(registrationrequest);

            _emailService.SendUserWelcomeMessageWithValidationCode(model.Email, model.Name, registrationrequest.Code,
                _appContext.AppLanguage.Id, _emailProvider);

            return RedirectToRoute("RegisterAffiliate", new { name = model.Name, email = model.Email });
        }

        /// <summary>
        ///     Validates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Validate()
        {
            var result = false;

            RegistrationRequest rr = null;

            bool resend;
            if (!bool.TryParse(Request["resend"], out resend))
            {
                rr = _registrationRequestService.GetRegistrationRequest(Request["email"], Request["ValidationCode"]);

                result = rr != null;

                if (rr != null) _registrationRequestService.DeleteRegistrationRequest(rr);
            }
            else
            {
                _registrationRequestService.DeleteRegistrationRequest(Request["email"]);

                var registrationrequest = new RegistrationRequest
                {
                    Code = Guid.NewGuid().ToString(),
                    Email = Request["email"],
                    Created = DateTime.UtcNow, //_settingService.GetTimeZoneDate(DateTime.Now);//TZ
                    Name = Request["Name"]
                };

                _registrationRequestService.InsertRegistrationRequest(registrationrequest);

                _emailService.SendUserWelcomeMessageWithValidationCode(Request["email"], Request["Name"],
                    registrationrequest.Code, _appContext.AppLanguage.Id, _emailProvider);
            }

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Check Username Availability
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult CheckUsernameAvailability(string username)
        {
            var usernameAvailable = false;

            var statusText =
                _localizedStringService.GetLocalizedString("Membership.CheckUsernameAvailability.NotAvailable");

            if (_userSetting.UsernameEnabled && !string.IsNullOrWhiteSpace(username))
            {
                if (_appContext.AppUser != null &&
                    _appContext.AppUser.Username != null &&
                    _appContext.AppUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    statusText =
                        _localizedStringService.GetLocalizedString(
                            "Membership.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var user = _userService.GetUserByUsername(username);

                    if (user == null)
                    {
                        statusText =
                            _localizedStringService.GetLocalizedString(
                                "Membership.CheckUsernameAvailability.Available");
                        usernameAvailable = true;
                    }
                }
            }

            return Json(new { Available = usernameAvailable, Text = statusText });
        }

        /// <summary>
        ///     Activation
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="email">Email</param>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Activation(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);

            if (user == null)
                return RedirectToRoute("HomePage");

            var membershipActivationToken =
                user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.MembershipActivationToken);

            if (string.IsNullOrEmpty(membershipActivationToken))
                return RedirectToRoute("HomePage");

            if (!membershipActivationToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return RedirectToRoute("HomePage");

            user.Active = true;
            user.ValidateOnLogin = false;

            _userService.UpdateUser(user);
            _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, "");

            //_emailService.SendUserWelcomeMessage(user, _appContext.AppLanguage.Id);

            var membershipActivationModel = new MembershipActivationModel
            {
                Result = _localizedStringService.GetLocalizedString("Membership.MembershipActivation.Activated")
            };

            return RedirectToAction("Login");
        }

        /// <summary>
        ///     Gets the users.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetUsers()
        {
            short deleted = 0;

            short.TryParse(Request["d"], out deleted);

            var users = (List<User>)_userService.GetSuperUsers(deleted);

            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = users.Count,
                recordsFiltered = users.Count
            };
            foreach (var ai in users)
            {
                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                var p = _profileService.GetProfileByUserId(ai.Id);

                string[] names1 =
                {
                    permissionService.Authorize(PermissionProvider.UserRolesNetworkUsersModify)
                        ? "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.GetFullName() + "</a>"
                        : "<b>" + ai.GetFullName() + "</b>",
                    ai.Email,
                    p == null ? "" : p.Phone,
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    role == null ? "" : role.Name,
                    ai.ValidateOnLogin ? "<span style='color: red'>Pending Activation</span>" :
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>",
                    "<a href='#' onclick='deleteUser(" + ai.Id + ")'>" + (ai.Deleted ? "Restore" : "Delete") + "</a>"
                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Gets the affiliate users.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAffiliateUsers()
        {
            short deleted = 0;

            short.TryParse(Request["d"], out deleted);

            var users = (List<User>)_userService.GetAffiliateUsers(deleted);
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = users.Count,
                recordsFiltered = users.Count
            };

            foreach (var ai in users)
            {
                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                string[] names1 =
                {
                    ai.Id.ToString(),
                    permissionService.Authorize(PermissionProvider.UserRolesAffiliateUserModify)
                        ? "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.Email + "</a>"
                        : "<b>" + ai.Email + "</b>",
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    role == null ? "" : role.Name,
                    ai.ValidateOnLogin ? "<span style='color: red'>Pending Activation</span>" :
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>",
                    "<a href='#' onclick='deleteUser(" + ai.Id + ")'>" + (ai.Deleted ? "Restore" : "Delete") + "</a>"
                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Gets the buyer users.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerUsers()
        {
            short deleted = 0;

            short.TryParse(Request["d"], out deleted);

            var users = (List<User>)_userService.GetBuyerUsers(deleted);
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = users.Count,
                recordsFiltered = users.Count
            };
            foreach (var ai in users)
            {
                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                string[] names1 =
                {
                    ai.Id.ToString(),
                    permissionService.Authorize(PermissionProvider.UserRolesBuyerUserModify)
                        ? "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.Email + "</a>"
                        : "<b>" + ai.Email + "</b>",
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    role == null ? "" : role.Name,
                    ai.ValidateOnLogin ? "<span style='color: red'>Pending Activation</span>" :
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>",
                    "<a href='#' onclick='deleteUser(" + ai.Id + ")'>" + (ai.Deleted ? "Restore" : "Delete") + "</a>"
                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Gets the users by affiliate.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetUsersByAffiliate(long affiliateId = 0)
        {
            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                    affiliateId = _appContext.AppUser.ParentId;
                else if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    return Json(new JsonData { draw = 1, recordsFiltered = 0, recordsTotal = 0 },
                        JsonRequestBehavior.AllowGet);
            }

            var users = (List<User>)_userService.GetUsersByAffiliateId(affiliateId);

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = 3,
                recordsFiltered = 3
            };
            foreach (var ai in users)
            {
                var affiliate = _affiliateService.GetAffiliateById(ai.ParentId, true);

                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                string[] names1 =
                {
                    ai.Id.ToString(),
                    "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.Email + "</a>",
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    role == null ? "" : role.Name,
                    ai.ValidateOnLogin ? "<span style='color: red'>Pending Activation</span>" :
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>"
                };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Gets the users by buyer.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetUsersByBuyer(long buyerId = 0)
        {
            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    buyerId = _appContext.AppUser.ParentId;
                else if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                    return Json(new JsonData { draw = 1, recordsFiltered = 0, recordsTotal = 0 },
                        JsonRequestBehavior.AllowGet);
            }

            var users = (List<User>)_userService.GetUsersByBuyerId(buyerId);

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = users.Count,
                recordsFiltered = users.Count
            };
            foreach (var ai in users)
            {
                var buyer = _buyerService.GetBuyerById(ai.ParentId);

                if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId &&
                    ai.Id != _appContext.AppUser.Id) continue;

                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                string[] names1 =
                {
                    ai.Id.ToString(),
                    "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.Email + "</a>",
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    role == null ? "" : role.Name,
                    ai.ValidateOnLogin ? "<span style='color: red'>Pending Activation</span>" :
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>"
                };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        #endregion Register

        #region Forgot Password

        /// <summary>
        ///     Forgot Password
        /// </summary>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult ForgotPassword()
        {
            var forgotPassword = new ForgotPasswordModel();

            return View(forgotPassword);
        }

        /// <summary>
        ///     Forgot Password
        /// </summary>
        /// <param name="forgotPasswordModel">The forgot password model.</param>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        [HttpPost]
        [ActionName("ForgotPassword")]
        [PublicAntiForgery]
        public ActionResult ForgotPasswordSend(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(forgotPasswordModel.Email);

                if (user != null && user.Active && !user.Deleted && !user.LockedOut)
                {
                    var forgotPasswordToken = Guid.NewGuid();
                    _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.ForgotPasswordToken,
                        forgotPasswordToken.ToString());

                    DateTime? requestedDateTime = DateTime.UtcNow;
                    _globalAttributeService.SaveGlobalAttribute(user,
                        GlobalAttributeBuiltIn.ForgotPasswordTokenRequestedDate, requestedDateTime);

                    _emailService.SendUserForgotPasswordMessage(user, _appContext.AppLanguage.Id, _emailProvider);

                    forgotPasswordModel.Result =
                        _localizedStringService.GetLocalizedString("Membership.ForgotPassword.EmailSent");
                }
                else
                {
                    forgotPasswordModel.Result =
                        _localizedStringService.GetLocalizedString("Membership.ForgotPassword.EmailNotFound");
                }

                return View(forgotPasswordModel);
            }

            return View(forgotPasswordModel);
        }

        /// <summary>
        ///     Forgot Password Confirmation
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="email">The email.</param>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult ForgotPasswordConfirmation(string token, string email)
        {
            var userService = _userService.GetUserByEmail(email);

            if (userService == null)
                return RedirectToRoute("HomePage");

            var forgotPasswordConfirmationModel = new ForgotPasswordConfirmationModel();

            if (!userService.IsForgotPasswordTokenValid(token))
            {
                forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                forgotPasswordConfirmationModel.Result =
                    _localizedStringService.GetLocalizedString("Membership.ForgotPassword.WrongToken");
            }

            if (userService.IsForgotPasswordLinkExpired(_userSetting))
            {
                forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                forgotPasswordConfirmationModel.Result =
                    _localizedStringService.GetLocalizedString("Membership.ForgotPassword.LinkExpired");
            }

            return View(forgotPasswordConfirmationModel);
        }

        /// <summary>
        ///     Forgot Password Confirmation
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="email">The email.</param>
        /// <param name="forgotPasswordConfirmationModel">The forgot password confirmation model.</param>
        /// <returns>Action Result</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        [HttpPost]
        [PublicAntiForgery]
        public ActionResult ForgotPasswordConfirmation(string token, string email,
            ForgotPasswordConfirmationModel forgotPasswordConfirmationModel)
        {
            var userService = _userService.GetUserByEmail(email);

            if (userService == null)
                return RedirectToRoute("HomePage");

            if (!userService.IsForgotPasswordTokenValid(token))
            {
                forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                forgotPasswordConfirmationModel.Result =
                    _localizedStringService.GetLocalizedString("Membership.ForgotPassword.WrongToken");
            }

            if (userService.IsForgotPasswordLinkExpired(_userSetting))
            {
                forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                forgotPasswordConfirmationModel.Result =
                    _localizedStringService.GetLocalizedString("Membership.ForgotPassword.LinkExpired");

                return View(forgotPasswordConfirmationModel);
            }

            if (ModelState.IsValid)
            {
                var changePasswordRequest = _userRegistrationService.ChangePassword(
                    new ChangePasswordRequest(false, email, forgotPasswordConfirmationModel.NewPassword));

                if (changePasswordRequest.Success)
                {
                    _globalAttributeService.SaveGlobalAttribute(userService, GlobalAttributeBuiltIn.ForgotPasswordToken,
                        "");

                    _emailService.SendUserPasswordChangeMessage(userService, _appContext.AppLanguage.Id, _emailProvider);

                    forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                    forgotPasswordConfirmationModel.Result =
                        _localizedStringService.GetLocalizedString(
                            "Membership.ForgotPassword.PasswordSuccessfullyChanged");
                }
                else
                {
                    forgotPasswordConfirmationModel.Result = changePasswordRequest.Errors.FirstOrDefault();
                }

                return View(forgotPasswordConfirmationModel);
            }

            return View(forgotPasswordConfirmationModel);
        }

        /// <summary>
        ///     Changes the password.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ChangePassword(long id = 0)
        {
            var model = new ChangePasswordModel();
            return View(model);
        }

        /// <summary>
        ///     Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult ChangePassword(ChangePasswordModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var saltKey = _encryptionService.CreateSaltKey(20);
                _appContext.AppUser.SaltKey = saltKey;
                _appContext.AppUser.Password = _encryptionService.CreatePasswordHash(model.Password, saltKey);
                _appContext.AppUser.ChangePassOnLogin = false;
                _userService.UpdateUser(_appContext.AppUser);

                return Redirect(Utils.GetBaseUrl(Request) + "Management/Home/Dashboard");
            }

            return View(model);
        }

        /// <summary>
        ///     Deletes the user.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery(true)]
        public ActionResult DeleteUser()
        {
            if (_appContext.AppUser.UserType != SharedData.BuiltInUserTypeId)
                return Json(new { result = false, message = "" }, JsonRequestBehavior.AllowGet);

            long userId;
            if (long.TryParse(Request["userid"], out userId))
            {
                var user = _userService.GetUserById(userId);
                if (user != null && user.UserType != SharedData.BuiltInUserTypeId)
                {
                    user.Deleted = !user.Deleted;
                    _userService.UpdateUser(user);

                    _historyService.AddHistory("MembershipController", HistoryAction.User_Deleted, "User", user.Id, "",
                        "", "", _appContext.AppUser.Id);
                }
                else
                {
                    return Json(new { result = false, message = "Can not delete user" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = true, message = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Passwords the strength.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PasswordStrength(string id)
        {
            ViewBag.PasswordElement = id;

            return PartialView();
        }

        #endregion Forgot Password

        #endregion Methods
    }
}