// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using Adrack.Web.Management.Models.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Represents a User Controller
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class UserController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// The profile service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The department service
        /// </summary>
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The encryption service
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Country Service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The user registration service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The date time helper
        /// </summary>
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="emailService">The email service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="localizedStringService">The localized string service.</param>
        /// <param name="countryService">The country service.</param>
        /// <param name="stateProvinceService">The state province service.</param>
        /// <param name="userRegistrationService">The user registration service.</param>
        /// <param name="departmentService">The department service.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="dateTimeHelper">The date time helper.</param>
        public UserController(IEmailService emailService, IUserService userService,
                                    IProfileService profileService,
                                    IRoleService roleService,
                                    ILocalizedStringService localizedStringService,
                                    ICountryService countryService,
                                    IStateProvinceService stateProvinceService,
                                    IUserRegistrationService userRegistrationService,
                                    IDepartmentService departmentService,
                                    IEncryptionService encryptionService,
                                    IBuyerService buyerService,
                                    IAffiliateService affiliateService,
                                    IBuyerChannelService buyerChannelService,
                                    IAppContext appContext,
                                    IHistoryService historyService,
                                    IDateTimeHelper dateTimeHelper)
        {
            this._emailService = emailService;
            this._userService = userService;
            this._roleService = roleService;
            this._profileService = profileService;
            this._localizedStringService = localizedStringService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._userRegistrationService = userRegistrationService;
            this._departmentService = departmentService;
            this._encryptionService = encryptionService;
            this._buyerService = buyerService;
            this._affiliateService = affiliateService;
            this._buyerChannelService = buyerChannelService;
            this._appContext = appContext;
            this._historyService = historyService;
            this._dateTimeHelper = dateTimeHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Membership")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Network Users")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Affiliate Users")]
        public ActionResult AffiliateList()
        {
            return View();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyer Users")]
        public ActionResult BuyerList()
        {
            return View();
        }

        /// <summary>
        /// Prepares the user model.
        /// </summary>
        /// <param name="model">The model.</param>
        [NonAction]
        public void PrepareUserModel(UserModel model)
        {
            model.ListStatus.Add(new SelectListItem() { Text = "Inactive", Value = "0" });
            model.ListStatus.Add(new SelectListItem() { Text = "Active", Value = "1" });

            model.ListLockedOut.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.ListLockedOut.Add(new SelectListItem() { Text = "Yes", Value = "1" });

            model.ListAffiliate.Add(new SelectListItem { Text = "Select affiliate", Value = "" });
            foreach (var value in _affiliateService.GetAllAffiliates(0))
            {
                if (this._appContext.AppUser != null)
                {
                    if (this._appContext.AppUser.UserType == SharedData.BuiltInUserTypeId ||
                        this._appContext.AppUser.UserType == SharedData.NetowrkUserTypeId ||
                        (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId && this._appContext.AppUser.ParentId == value.Id))
                    {
                        model.ListAffiliate.Add(new SelectListItem
                        {
                            Text = value.Name,
                            Value = value.Id.ToString()
                        });
                    }
                }
            }

            model.ListBuyer.Add(new SelectListItem { Text = "Select buyer", Value = "" });
            foreach (var value in _buyerService.GetAllBuyers(0))
            {
                if (this._appContext.AppUser != null)
                {
                    if (this._appContext.AppUser.UserType == SharedData.BuiltInUserTypeId ||
                        this._appContext.AppUser.UserType == SharedData.NetowrkUserTypeId ||
                        (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId && this._appContext.AppUser.ParentId == value.Id))
                    {
                        model.ListBuyer.Add(new SelectListItem
                        {
                            Text = value.Name,
                            Value = value.Id.ToString()
                        });
                    }
                }
            }

            /*model.ListBuyerChannel.Add(new SelectListItem { Text = "Select buyer channel", Value = "" });
            if (this._appContext.AppUser.UserTypeId == SharedData.BuyerUserTypeId)
            {
                foreach (var value in _buyerChannelService.GetAllBuyerChannels())
                {
                    if (this._appContext.AppUser != null)
                    {
                        if (this._appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId ||
                            this._appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId ||
                            (this._appContext.AppUser.UserTypeId == SharedData.BuyerUserTypeId && this._appContext.AppUser.ParentId == value.BuyerId))
                        {
                            model.ListBuyer.Add(new SelectListItem
                            {
                                Text = value.Name,
                                Value = value.Id.ToString()
                            });
                        }
                    }
                }
            }*/

            model.ListUserType.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var value in _userService.GetAllUserTypes())
            {
                model.ListUserType.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            foreach (var value in _roleService.GetAllRoles(model.UserType))
            {
                model.ListUserRole.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            model.ListCountry.Add(new SelectListItem { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

            foreach (var value in _countryService.GetAllCountries())
            {
                model.ListCountry.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Id.ToString(),
                    Selected = value.Id == model.CountryId
                });
            }

            var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(model.CountryId).Count;

            if (stateProvince > 0)
            {
                model.ListStateProvince.Add(new SelectListItem { Text = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"), Value = "" });

                foreach (var value in _stateProvinceService.GetAllStateProvinces())
                {
                    model.ListStateProvince.Add(new SelectListItem
                    {
                        Text = value.GetLocalized(x => x.Name),
                        Value = value.Id.ToString(),
                        Selected = value.Id == model.StateProvinceId
                    });
                }
            }
            else
            {
                bool anyCountrySelected = true;// model.ListCountry.Any(x => x.Selected);

                model.ListStateProvince.Add(new SelectListItem
                {
                    Text = _localizedStringService.GetLocalizedString(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectStateProvince"),
                    Value = ""
                });
            }

            model.ListDepartment.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var value in _departmentService.GetAllDepartments())
            {
                model.ListDepartment.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            if (model.UserType == SharedData.AffiliateUserTypeId)
                model.RedirectUrl = "/Management/Affiliate/Item/" + model.ParentId;
            else
                if (model.UserType == SharedData.BuyerUserTypeId)
                model.RedirectUrl = "/Management/Buyer/Item/" + model.ParentId;

            model.TimeZones = _dateTimeHelper.GetSystemTimeZones(model.TimeZone);
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Item(long Id = 0)
        {
            /*AZ
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            if (! permissionService.Authorize(PermissionProvider.UserRolesNetworkUsersModify))
            {
                return null;
            }
            */

            User user = _userService.GetUserById(Id);

            var registerModel = new UserModel();

            registerModel.LoggedInUser = this._appContext.AppUser;

            long parentId = 0;
            UserTypes userTypeId = SharedData.NetowrkUserTypeId;

            if (_appContext.AppUser != null)
            {
                parentId = _appContext.AppUser.ParentId;
                if ((_appContext.AppUser.UserType == SharedData.BuyerUserTypeId && _appContext.AppUser.Id != user.Id) || _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                {
                    return HttpNotFound();
                }
            }

            registerModel.ParentId = parentId;
            registerModel.UserId = 0;
            registerModel.UserType = userTypeId;

            if (user != null)
            {
                Profile profile = _profileService.GetProfileByUserId(Id);

                registerModel.UserType = user.UserType;
                registerModel.ParentId = user.ParentId;
                registerModel.UserId = user.Id;
                registerModel.ProfileId = profile.Id;

                registerModel.Username = user.Username;
                registerModel.FirstName = profile.FirstName;
                registerModel.LastName = profile.LastName;
                registerModel.MiddleName = profile.MiddleName;
                registerModel.Email = user.Email;
                registerModel.ContactEmail = user.ContactEmail;
                registerModel.Password = "#kuku$$%^#$%&@#$SFADGF";//user.Password;
                registerModel.ConfirmPassword = registerModel.Password;//user.Password;
                registerModel.Phone = profile.Phone;
                registerModel.CellPhone = profile.CellPhone;
                registerModel.Comment = user.Comment;
                registerModel.DepartmentId = user.DepartmentId !=null ? (long)user.DepartmentId : 0;

                registerModel.ChangePassOnLogin = (user.ChangePassOnLogin.HasValue ? user.ChangePassOnLogin.Value : false);

                registerModel.MaskEmail = user.MaskEmail;

                registerModel.TimeZone = user.TimeZone;

                registerModel.UserStatus = (short)(user.Active ? 1 : 0);
                registerModel.LockedOut = (short)(user.LockedOut ? 1 : 0);

                Role role = (from x in user.Roles
                             where x.Id != 3
                             select x).FirstOrDefault();

                if (role != null)
                    registerModel.UserRoleId = role.Id;
            }

            PrepareUserModel(registerModel);

            List<System.Web.Mvc.SelectListItem> buyerChannels = new List<System.Web.Mvc.SelectListItem>();

            if (user != null)
            {
                ViewBag.BuyerChannels = buyerChannels;

                ViewBag.SelectedBuyerChannels = (List<UserBuyerChannel>)_userService.GetUserBuyerChannels(user.Id);
            }
            else
            {
                ViewBag.SelectedBuyerChannels = new List<UserBuyerChannel>();
            }

            return View(registerModel);
        }

        /// <summary>
        /// Affiliates the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Affiliate(long Id = 0)
        {
            var registerModel = new UserModel();

            registerModel.LoggedInUser = this._appContext.AppUser;

            UserTypes userTypeId = SharedData.AffiliateUserTypeId;

            if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                Id = _appContext.AppUser.ParentId;
            }

            registerModel.ParentId = Id;
            registerModel.UserId = 0;
            registerModel.UserType = userTypeId;

            registerModel.LoggedInUser = this._appContext.AppUser;

            PrepareUserModel(registerModel);

            return View("Item", registerModel);
        }

        /// <summary>
        /// Buyers the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.Yes)]
        public ActionResult Buyer(long Id = 0)
        {
            var registerModel = new UserModel();

            registerModel.LoggedInUser = this._appContext.AppUser;

            UserTypes userTypeId = SharedData.BuyerUserTypeId;

            if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                Id = _appContext.AppUser.ParentId;
            }

            registerModel.ParentId = Id;
            registerModel.UserId = 0;
            registerModel.UserType = userTypeId;

            registerModel.LoggedInUser = this._appContext.AppUser;

            PrepareUserModel(registerModel);

            return View("Item", registerModel);
        }

        /// <summary>
        /// Items the specified register model.
        /// </summary>
        /// <param name="registerModel">The register model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(UserModel registerModel, string returnUrl)
        {
            /*
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            if(!permissionService.Authorize(PermissionProvider.UserRolesNetworkUsersModify))
            {
                return null;
            }
            */

            if (ModelState.IsValid)
            {
                string channels = Request["channels"];

                if (registerModel.Username != null)
                {
                    registerModel.Username = registerModel.Username.Trim();
                }

                Adrack.Core.Domain.Membership.User user = _userService.GetUserById(registerModel.UserId);

                if (registerModel.UserRoleId == 0)
                {
                    if (registerModel.UserType == SharedData.AffiliateUserTypeId || registerModel.UserType == SharedData.BuyerUserTypeId) registerModel.UserRoleId = 3;
                    else
                        registerModel.UserRoleId = 2;
                }

                if (user == null)
                {
                    user = new Core.Domain.Membership.User();

                    user.ValidateOnLogin = true;

                    user.GuId = Guid.NewGuid().ToString();

                    user.ParentId = registerModel.ParentId;

                    user.DepartmentId = 1;

                    user.MaskEmail = registerModel.MaskEmail;

                    user.UserType = registerModel.UserType;

                    user.TimeZone = registerModel.TimeZone;

                    registerModel.ConfirmPassword = registerModel.Password;

                    var registrationRequest = new UserRegistrationRequest(user, registerModel.Email, registerModel.Email, registerModel.Password, registerModel.Comment, registerModel.ContactEmail, true);

                    var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, true);

                    if (registrationResult.Success)
                    {
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

                        Role role = _roleService.GetRoleById(registerModel.UserRoleId);

                        if (role == null)
                        {
                            if (registerModel.UserType == SharedData.AffiliateUserTypeId || registerModel.UserType == SharedData.BuyerUserTypeId)
                                role = _roleService.GetRoleById(2);
                        }

                        if (role != null)
                        {
                            user.Roles.Add(role);
                        }

                        user.ChangePassOnLogin = registerModel.ChangePassOnLogin;

                        IGlobalAttributeService _globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();
                        _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, user.GuId);

                        user.Active = (registerModel.UserStatus == 1 ? true : false);
                        if (user.Active && _appContext.AppUser != null && (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId))
                            user.ValidateOnLogin = false;
                        user.LockedOut = (registerModel.LockedOut == 1 ? true : false);

                        _userService.UpdateUser(user);

                        HistoryAction ha = HistoryAction.New_Affiliate_User_Registered;

                        if (user.UserType == SharedData.AffiliateUserTypeId)
                        {
                            ha = HistoryAction.New_Affiliate_User_Registered;
                        }
                        else if (user.UserType == SharedData.BuyerUserTypeId)
                        {
                            ha = HistoryAction.New_Buyer_User_Registered;
                        }
                        else if (user.UserType == SharedData.NetowrkUserTypeId)
                        {
                            ha = HistoryAction.New_System_User_Registered;
                        }

                        this._historyService.AddHistory("UserController", ha, "User", user.Id, "", "", "", this._appContext.AppUser.Id);

                        // SendUser Registered Message
                        //_emailService.SendUserRegisteredMessage(user, _appContext.AppLanguage.Id);
                        _emailService.SendUserWelcomeMessageWithUsernamePassword(user, _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative, registerModel.Email, registerModel.Password);
                    }
                    else
                    {
                        foreach (var error in registrationResult.Errors)
                            ModelState.AddModelError("", error);

                        PrepareUserModel(registerModel);

                        return View(registerModel);
                    }
                }
                else
                {
                    Profile profile = _profileService.GetProfileByUserId(registerModel.UserId);

                    user.Username = registerModel.Username;
                    profile.FirstName = registerModel.FirstName;
                    profile.LastName = registerModel.LastName;
                    profile.MiddleName = registerModel.MiddleName;
                    profile.CellPhone = registerModel.CellPhone;
                    profile.Phone = registerModel.Phone;
                    user.Email = registerModel.Email;
                    user.ContactEmail = registerModel.ContactEmail;
                    user.Username = registerModel.Email;
                    user.UserType = registerModel.UserType;
                    user.MaskEmail = registerModel.MaskEmail;
                    user.TimeZone = registerModel.TimeZone;
                    user.ParentId = registerModel.ParentId;

                    if (registerModel.Password != "#kuku$$%^#$%&@#$SFADGF")
                    {
                        string saltKey = _encryptionService.CreateSaltKey(20);
                        user.SaltKey = saltKey;
                        user.Password = _encryptionService.CreatePasswordHash(registerModel.Password, saltKey);
                    }

                    Role role = (from x in user.Roles
                                 where x.Id != 3
                                 select x).FirstOrDefault();

                    if (role != null)
                    {
                        user.Roles.Remove(role);
                    }

                    role = _roleService.GetRoleById(registerModel.UserRoleId);
                    user.Roles.Add(role);

                    user.Active = (registerModel.UserStatus == 1 ? true : false);
                    if (user.Active && _appContext.AppUser != null && (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId))
                        user.ValidateOnLogin = false;
                    user.LockedOut = (registerModel.LockedOut == 1 ? true : false);

                    _profileService.UpdateProfile(profile);
                    _userService.UpdateUser(user);

                    this._historyService.AddHistory("UserController", HistoryAction.User_Edited, "User", user.Id, "", "", "", this._appContext.AppUser.Id);
                }
            }

            PrepareUserModel(registerModel);

            if (!ModelState.IsValid)
                return View(registerModel);

            return Redirect(registerModel.RedirectUrl);
        }

        /// <summary>
        /// Gets the buyer first user.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyerFirstUser()
        {
            long buyerId = 0;

            try
            {
                buyerId = long.Parse(Request["buyerId"]);
            }
            catch
            {
            }

            List<User> users = (List<User>)this._userService.GetUsersByBuyerId(buyerId);

            foreach (User ai in users)
            {
                if (ai.UserType == SharedData.BuiltInUserTypeId)
                {
                    return Json(new { id = ai.Id, name = ai.Username, email = ai.Email }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { id = 0, name = "", email = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Passwords the strength.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PasswordStrength(string id)
        {
            ViewBag.PasswordElement = id;

            return PartialView();
        }

        #endregion Methods
    }
}