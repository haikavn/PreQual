// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
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
using Adrack.Web.ContentManagement.Models.Lead;
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
    /// Handles affiliate actions
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class AffiliateController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The affiliate note service
        /// </summary>
        private readonly IAffiliateNoteService _affiliateNoteService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The user registration service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// The department service
        /// </summary>
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// The profile service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The user setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The affiliate channel filter condition service
        /// </summary>
        private readonly IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// AffiliateController constructor
        /// </summary>
        /// <param name="affiliateService">AffiliateService reference</param>
        /// <param name="affiliateNoteService">AffiliateNoteService reference</param>
        /// <param name="affiliateChannelService">AffiliateChannelService reference</param>
        /// <param name="settingService">SettingService reference</param>
        /// <param name="affiliateChannelFilterConditionService">AffiliateChannelFilterConditionService reference</param>
        /// <param name="localizedStringService">LocalizedStringService reference</param>
        /// <param name="countryService">CountryService reference</param>
        /// <param name="stateProvinceService">StateProvinceService reference</param>
        /// <param name="usersService">UsersService reference</param>
        /// <param name="historyService">HistoryService reference</param>
        /// <param name="permissionService">PermissionService reference</param>
        /// <param name="roleService">RoleService reference</param>
        /// <param name="departmentService">DepartmentService reference</param>
        /// <param name="userRegistrationService">UserRegistrationService reference</param>
        /// <param name="profileService">ProfileService reference</param>
        /// <param name="userSetting">UserSetting reference</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="appContext">AppContext reference</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>

        /// <param name="authenticationService">The authentication service.</param>
        public AffiliateController(IAffiliateService affiliateService, IAffiliateNoteService affiliateNoteService, IAffiliateChannelService affiliateChannelService, ISettingService settingService, IAffiliateChannelFilterConditionService affiliateChannelFilterConditionService, ILocalizedStringService localizedStringService, ICountryService countryService, IStateProvinceService stateProvinceService, IUserService usersService, IHistoryService historyService, IPermissionService permissionService, IRoleService roleService, IDepartmentService departmentService, IUserRegistrationService userRegistrationService, IProfileService profileService, UserSetting userSetting, IEmailService emailService, IAppContext appContext, ICampaignService campaignService, IBuyerChannelService buyerChannelService, IAuthenticationService authenticationService)
        {
            this._affiliateService = affiliateService;
            this._affiliateNoteService = affiliateNoteService;
            this._affiliateChannelService = affiliateChannelService;
            this._countryService = countryService;
            this._localizedStringService = localizedStringService;
            this._stateProvinceService = stateProvinceService;
            this._userService = usersService;
            this._settingService = settingService;
            this._affiliateChannelFilterConditionService = affiliateChannelFilterConditionService;
            this._permissionService = permissionService;
            this._historyService = historyService;
            this._roleService = roleService;
            this._departmentService = departmentService;
            this._userRegistrationService = userRegistrationService;
            this._profileService = profileService;
            this._userSetting = userSetting;
            this._appContext = appContext;
            this._emailService = emailService;
            this._campaignService = campaignService;
            this._buyerChannelService = buyerChannelService;
            this._authenticationService = authenticationService;
        }

        #endregion Constructor

        // GET: Affiliate

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows affiliate list interface
        /// </summary>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Affiliates")]
        public ActionResult List()
        {
            IList<StatusCountsClass> stList = this._affiliateService.GetAffiliatesStatusCounts();

            string[] stArr = { "0", "0", "0", "0", "0", "0", "0" };
            int TotalStatuses = 0;
            foreach (StatusCountsClass scc in stList)
            {
                TotalStatuses += scc.Counts;
                stArr[scc.Status] = scc.Counts.ToString();
            }
            ViewBag.StatusArray = stArr;

            ViewBag.TotalStatuses = TotalStatuses;

            return View();
        }

        /// <summary>
        /// Prepares an AffiliateModel
        /// </summary>
        /// <param name="model">AffiliateModel reference</param>
        protected void PrepareModel(AffiliateModel model)
        {
            model.ListStatus.Add(new SelectListItem { Text = "Inactive", Value = "0" });
            model.ListStatus.Add(new SelectListItem { Text = "Active", Value = "1" });

            model.ListUser.Add(new SelectListItem { Text = _localizedStringService.GetLocalizedString("Affiliate.SelectAccountManager"), Value = "" });

            foreach (var value in _userService.GetUsersByType(UserTypes.Network))
            {
                if (value.Deleted) continue;

                model.ListUser.Add(new SelectListItem
                {
                    Text = value.GetFullName(),
                    Value = value.Id.ToString(),
                    Selected = value.Id == model.ManagerId
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

            var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(model.CountryId).ToList();

            if (stateProvince.Count() > 0)
            {
                foreach (var value in stateProvince)
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
                bool anyCountrySelected = model.ListCountry.Any(x => x.Selected);

                model.ListStateProvince.Add(new SelectListItem
                {
                    Text = _localizedStringService.GetLocalizedString(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectStateProvince"),
                    Value = ""
                });
            }

            model.ListUserType.Add(new SelectListItem { Text = "Select user type", Value = "" });

            foreach (var value in _userService.GetAllUserTypes())
            {
                model.ListUserType.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            model.ListUserRole.Add(new SelectListItem { Text = "Select role", Value = "" });

            foreach (var value in _roleService.GetAllRoles())
            {
                model.ListUserRole.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            model.ListDepartment.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (var value in _departmentService.GetAllDepartments())
            {
                model.ListDepartment.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            model.ListDefaultAffiliatePriceMethod.Add(new SelectListItem { Text = "Fixed", Value = "0" });
            model.ListDefaultAffiliatePriceMethod.Add(new SelectListItem { Text = "Revshare", Value = "1" });
        }

        /// <summary>
        /// Displays affiliate item create/edit interface
        /// </summary>
        /// <param name="id">Affiliate id</param>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Affiliate")]
        public ActionResult Item(long id = 0)
        {
            AffiliateModel am = new AffiliateModel();

            ViewBag.SelectedAffiliateId = id;

            Affiliate b = new Affiliate();
            b = _affiliateService.GetAffiliateById(id, false);
            ViewBag.BuyerCompanyName = b != null && b.Name != "" ? "Affiliate > " + b.Name + " > " : "";


            if (id == 0)
            {
                am.AffiliateId = 0;
                am.UserType = SharedData.AffiliateUserTypeId;
                am.UserRoleId = 5;
                am.ParentId = 0;
                PrepareModel(am);
                return View("Register", am);
            }

            Affiliate affiliate = this._affiliateService.GetAffiliateById(id, false);

            am.LoggedInUser = _appContext.AppUser;

            am.AffiliateId = 0;

            if (affiliate != null)
            {
                am.AffiliateId = affiliate.Id;
                am.CountryId = affiliate.CountryId;
                am.StateProvinceId = (affiliate.StateProvinceId.HasValue ? affiliate.StateProvinceId.Value : 0);
                am.Name = affiliate.Name;
                am.AddressLine1 = affiliate.AddressLine1;
                am.AddressLine2 = affiliate.AddressLine2;
                am.City = affiliate.City;
                am.CompanyEmail = affiliate.Email;
                am.CompanyPhone = affiliate.Phone;
                am.ZipPostalCode = affiliate.ZipPostalCode;
                am.BillFrequency = affiliate.BillFrequency;
                am.FrequencyValue = affiliate.FrequencyValue != null ? (int)affiliate.FrequencyValue : 0;
                am.BillWithin = affiliate.BillWithin != null ? (int)affiliate.BillWithin : 0;
                if (affiliate.ManagerId == null)
                    am.ManagerId = 0;
                else
                    am.ManagerId = (long)affiliate.ManagerId;

                am.Website = affiliate.Website;
                am.WhiteIp = affiliate.WhiteIp;

                am.Status = affiliate.Status;

                am.Notes = (List<AffiliateNote>)_affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id);

                am.DefaultAffiliatePrice = Math.Round((affiliate.DefaultAffiliatePrice.HasValue ? affiliate.DefaultAffiliatePrice.Value : 0), 2);
                am.DefaultAffiliatePriceMethod = (affiliate.DefaultAffiliatePriceMethod.HasValue ? affiliate.DefaultAffiliatePriceMethod.Value : (short)0);

                ViewBag.AffiliateId = affiliate.Id;
                ViewBag.BuyerId = 0;
            }

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Shows affiliate item create/edit partial item
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>PartialView result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Affiliate")]
        public ActionResult PartialItem(long id = 0)
        {
            AffiliateModel am = new AffiliateModel();

            if (id == 0)
            {
                am.AffiliateId = 0;
                am.UserType = SharedData.AffiliateUserTypeId;
                am.UserRoleId = 2;
                am.ParentId = 0;
                PrepareModel(am);
                return PartialView("Register", am);
            }

            Affiliate affiliate = this._affiliateService.GetAffiliateById(id, false);

            am.LoggedInUser = _appContext.AppUser;

            am.AffiliateId = 0;

            if (affiliate != null)
            {
                am.AffiliateId = affiliate.Id;
                am.CountryId = affiliate.CountryId;
                am.StateProvinceId = (affiliate.StateProvinceId.HasValue ? affiliate.StateProvinceId.Value : 0);
                am.Name = affiliate.Name;
                am.AddressLine1 = affiliate.AddressLine1;
                am.AddressLine2 = affiliate.AddressLine2;
                am.City = affiliate.City;
                am.CompanyEmail = affiliate.Email;
                am.CompanyPhone = affiliate.Phone;
                am.ZipPostalCode = affiliate.ZipPostalCode;
                am.Status = affiliate.Status;
                am.BillFrequency = affiliate.BillFrequency;
                am.FrequencyValue = affiliate.FrequencyValue != null ? (int)affiliate.FrequencyValue : 0;
                am.BillWithin = affiliate.BillWithin != null ? (int)affiliate.BillWithin : 0;

                am.Website = affiliate.Website;
                am.WhiteIp = affiliate.WhiteIp;

                am.Notes = (List<AffiliateNote>)_affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id);

                ViewBag.AffiliateId = affiliate.Id;
                ViewBag.BuyerId = 0;
            }

            PrepareModel(am);

            return PartialView("Item", am);
        }

        /// <summary>
        /// Handles affiliate item submit action
        /// </summary>
        /// <param name="affiliateModel">AffiliateModel reference</param>
        /// <param name="command">The command.</param>
        /// <returns>RedirectAction result</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(AffiliateModel affiliateModel, string command)
        {
            Affiliate affiliate = null;
            string data1 = "", data2 = "";

            if (_affiliateService.GetAffiliateByName(affiliateModel.Name, affiliateModel.AffiliateId) != null)
            {
                ModelState.AddModelError("", "Affiliate with the specified name already exists");

                PrepareModel(affiliateModel);

                if (affiliateModel.AffiliateId == 0)
                {
                    affiliateModel.UserType = SharedData.AffiliateUserTypeId;
                    affiliateModel.UserRoleId = 5;
                    affiliateModel.ParentId = 0;
                    return View("Register", affiliateModel);
                }

                return View(affiliateModel);
            }

            short status = 0;

            if (affiliateModel.AffiliateId == 0)
            {
                return Register(affiliateModel);
            }
            else
            {
                affiliate = _affiliateService.GetAffiliateById(affiliateModel.AffiliateId, false);
                status = affiliateModel.Status;

                data1 = "Name: " + affiliate.Name + ";" +
                                "CountryId: " + affiliate.CountryId.ToString() + ";" +
                                "StateProvinceId: " + affiliate.StateProvinceId.ToString() + ";" +
                                "Phone: " + (!string.IsNullOrEmpty(affiliate.Phone) ? affiliate.Phone.ToString() : "") + ";" +
                                "Email: " + (!string.IsNullOrEmpty(affiliate.Email) ? affiliate.Email.ToString() : "") + ";" +
                                "City: " + (!string.IsNullOrEmpty(affiliate.City) ? affiliate.City.ToString() : "") + ";" +
                                "ZipPostalCode: " + (!string.IsNullOrEmpty(affiliate.ZipPostalCode) ? affiliate.ZipPostalCode.ToString() : "") + ";" +
                                "AddressLine1: " + (!string.IsNullOrEmpty(affiliate.AddressLine1) ? affiliate.AddressLine1.ToString() : "") + ";" +
                                "AddressLine2: " + (!string.IsNullOrEmpty(affiliate.AddressLine2) ? affiliate.AddressLine2.ToString() : "") + ";" +
                                "Status: " + affiliate.Status.ToString() + ";" +
                                "ManagerId: " + affiliate.ManagerId.ToString() + ";" +
                                "BillFrequency: " + (!string.IsNullOrEmpty(affiliate.BillFrequency) ? affiliate.BillFrequency.ToString() : "") + ";" +
                                "FrequencyValue: " + affiliate.FrequencyValue.ToString() + ";" +
                                "BillWithin: " + (affiliate.BillWithin.HasValue ? affiliate.BillWithin.Value.ToString() : "0") + ";" +
                                "Website: " + (!string.IsNullOrEmpty(affiliate.Website) ? affiliate.Website.ToString() : "") + ";" +
                                "WhiteIp: " + (!string.IsNullOrEmpty(affiliate.WhiteIp) ? affiliate.WhiteIp.ToString() : "");
            }

            if (affiliateModel.AffiliateId > 0)
            {
                User user = new User()
                {
                    Email = affiliateModel.CompanyEmail,
                    GuId = Guid.NewGuid().ToString(),
                    Username = affiliateModel.Name
                };
            }

            affiliate.Name = affiliateModel.Name.Trim();
            affiliate.CountryId = affiliateModel.CountryId;
            affiliate.StateProvinceId = (affiliateModel.StateProvinceId == 0 ? null : (long?)affiliateModel.StateProvinceId);
            affiliate.Phone = affiliateModel.CompanyPhone;
            affiliate.Email = affiliateModel.CompanyEmail;
            affiliate.ZipPostalCode = affiliateModel.ZipPostalCode;
            affiliate.City = affiliateModel.City;
            affiliate.AddressLine1 = affiliateModel.AddressLine1;
            affiliate.AddressLine2 = affiliateModel.AddressLine2;
            affiliate.Status = status;
            affiliate.ManagerId = affiliateModel.ManagerId;
            affiliate.CreatedOn = DateTime.UtcNow;

            affiliate.BillFrequency = affiliateModel.BillFrequency;
            affiliate.FrequencyValue = affiliateModel.FrequencyValue;
            affiliate.BillWithin = affiliateModel.BillWithin;

            affiliate.Website = affiliateModel.Website;
            affiliate.WhiteIp = affiliateModel.WhiteIp;

            affiliate.DefaultAffiliatePrice = affiliateModel.DefaultAffiliatePrice;
            affiliate.DefaultAffiliatePriceMethod = affiliateModel.DefaultAffiliatePriceMethod;

            if (affiliateModel.AffiliateId == 0)
            {
                long newId = _affiliateService.InsertAffiliate(affiliate);
                this._historyService.AddHistory("AffiliateController", HistoryAction.Affiliate_Added, "Affiliate", newId, "Name:" + affiliate.Name, "", "", this._appContext.AppUser.Id);
            }
            else
            {
                _affiliateService.UpdateAffiliate(affiliate);

                data2 = "Name: " + affiliate.Name + ";" +
                                "CountryId: " + affiliate.CountryId.ToString() + ";" +
                                "StateProvinceId: " + affiliate.StateProvinceId.ToString() + ";" +
                                "Phone: " + (!string.IsNullOrEmpty(affiliate.Phone) ? affiliate.Phone.ToString() : "") + ";" +
                                "Email: " + (!string.IsNullOrEmpty(affiliate.Email) ? affiliate.Email.ToString() : "") + ";" +
                                "City: " + (!string.IsNullOrEmpty(affiliate.City) ? affiliate.City.ToString() : "") + ";" +
                                "ZipPostalCode: " + (!string.IsNullOrEmpty(affiliate.ZipPostalCode) ? affiliate.ZipPostalCode.ToString() : "") + ";" +
                                "AddressLine1: " + (!string.IsNullOrEmpty(affiliate.AddressLine1) ? affiliate.AddressLine1.ToString() : "") + ";" +
                                "AddressLine2: " + (!string.IsNullOrEmpty(affiliate.AddressLine2) ? affiliate.AddressLine2.ToString() : "") + ";" +
                                "Status: " + affiliate.Status.ToString() + ";" +
                                "ManagerId: " + affiliate.ManagerId.ToString() + ";" +
                                "BillFrequency: " + (!string.IsNullOrEmpty(affiliate.BillFrequency) ? affiliate.BillFrequency.ToString() : "") + ";" +
                                "FrequencyValue: " + affiliate.FrequencyValue.ToString() + ";" +
                                "BillWithin: " + (affiliate.BillWithin.HasValue ? affiliate.BillWithin.Value.ToString() : "0") + ";" +
                                "Website: " + (!string.IsNullOrEmpty(affiliate.Website) ? affiliate.Website.ToString() : "") + ";" +
                                "WhiteIp: " + (!string.IsNullOrEmpty(affiliate.WhiteIp) ? affiliate.WhiteIp.ToString() : "");

                this._historyService.AddHistory("AffiliateController", HistoryAction.Affiliate_Edited, "Affiliate", affiliateModel.AffiliateId, data1, data2, "", this._appContext.AppUser.Id);
            }

            string json = Request.Unvalidated["notes"];
            dynamic o = JsonConvert.DeserializeObject(json);

            for (int i = 0; i < o.Count; i++)
            {
                string date = o[i][0].ToString();
                string note = o[i][1].ToString();
                long id = long.Parse(o[i][2].ToString());

                AffiliateNote an = _affiliateNoteService.GetAffiliateNoteById(Math.Abs(id));

                if (an != null && id < 0)
                {
                    _affiliateNoteService.DeleteAffiliateNote(an);
                    continue;
                }

                if (an == null)
                {
                    an = new AffiliateNote();
                    an.Created = DateTime.UtcNow;
                }

                an.AffiliateId = affiliate.Id;
                an.Note = note;

                if (id == 0)
                    _affiliateNoteService.InsertAffiliateNote(an);
                else
                    _affiliateNoteService.UpdateAffiliateNote(an);
            }

            affiliateModel.AffiliateId = affiliate.Id;

            return RedirectToAction("List");
        }

        /// <summary>
        /// Registers new affiliate and user
        /// </summary>
        /// <param name="affiliateModel">AffiliateModel reference</param>
        /// <returns>View result</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Register(AffiliateModel affiliateModel)
        {
            var user = new User();

            if (ModelState.IsValid)
            {
                affiliateModel.UserName = affiliateModel.Email.Trim();
                bool isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;
                user.UserType = UserTypes.Affiliate;
                var registrationRequest = new UserRegistrationRequest(user, affiliateModel.UserName, affiliateModel.UserName, affiliateModel.Password, affiliateModel.Comment, affiliateModel.ContactEmail, isApproved);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, false);

                if (registrationResult.Success)
                {
                    _profileService.InsertProfile(new Profile
                    {
                        UserId = user.Id,
                        FirstName = affiliateModel.FirstName,
                        MiddleName = affiliateModel.MiddleName,
                        LastName = affiliateModel.LastName,
                        Summary = "",
                        Phone = affiliateModel.Phone,
                        CellPhone = affiliateModel.CellPhone
                    });

                    if (affiliateModel.ParentId == 0)
                    {
                        if (affiliateModel.UserRoleId != 1 && affiliateModel.UserType != SharedData.BuiltInUserTypeId)
                        {
                            long affiliateId = _affiliateService.InsertAffiliate(new Affiliate
                            {
                                Name = affiliateModel.Name,
                                CountryId = affiliateModel.CountryId,
                                StateProvinceId = (affiliateModel.StateProvinceId == 0 ? null : (long?)affiliateModel.StateProvinceId),
                                Email = affiliateModel.CompanyEmail,
                                AddressLine1 = affiliateModel.AddressLine1,
                                AddressLine2 = affiliateModel.AddressLine2,
                                City = affiliateModel.City,
                                ZipPostalCode = affiliateModel.ZipPostalCode,
                                Phone = affiliateModel.CompanyPhone,
                                UserId = user.Id,
                                CreatedOn = DateTime.UtcNow,
                                Status = 0,
                                RegistrationIp = Request.UserHostAddress,
                                ManagerId = affiliateModel.ManagerId,
                                WhiteIp = affiliateModel.WhiteIp
                            });

                            user.ParentId = affiliateId;
                            this._historyService.AddHistory("AffiliateController", HistoryAction.Affiliate_Added, "Affiliate", affiliateId, "Name:" + affiliateModel.Name, "", "", this._appContext.AppUser.Id);
                        }
                    }
                    else
                    {
                        user.ParentId = affiliateModel.ParentId;
                    }

                    user.DepartmentId = 1;

                    Role role = _roleService.GetRoleById(affiliateModel.UserRoleId);

                    if (role == null)
                    {
                        if (affiliateModel.UserType == SharedData.AffiliateUserTypeId || affiliateModel.UserType == SharedData.BuyerUserTypeId)
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

                    this._historyService.AddHistory("AffiliateController", HistoryAction.New_Affiliate_User_Registered, "User", user.Id, "Username:" + user.Email, "", "", this._appContext.AppUser.Id);

                    _emailService.SendUserWelcomeMessageWithUsernamePassword(user, _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative, affiliateModel.Email, affiliateModel.Password);
                }

                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);

                if (registrationResult.Errors.Count > 0)
                {
                    PrepareModel(affiliateModel);

                    return View("Register", affiliateModel);
                }
            }

            PrepareModel(affiliateModel);
            return RedirectToAction("List");
        }

        /// <summary>
        /// Changes the affiliate status
        /// </summary>
        /// <returns>Json result</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult SetAffiliateStatus()
        {
            if (this._appContext.AppUser == null) return Json(new { res = 0 }, JsonRequestBehavior.AllowGet);

            long id = 0;
            long.TryParse(Request["id"], out id);

            short status = 0;
            short.TryParse(Request["status"], out status);

            Affiliate a = _affiliateService.GetAffiliateById(id, false);

            if (a != null)
            {
                a.Status = status;
                _affiliateService.UpdateAffiliate(a);
            }

            return Json(new { res = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Dashboards the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Dashboard(long id = 0)
        {
            Affiliate b = new Affiliate();

            b = _affiliateService.GetAffiliateById(id, false);
            
            ViewBag.BuyerCompanyName = b!=null && b.Name !="" ? "Affiliate > " + b.Name + " > " : "";

            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;
            ViewBag.SelectedAffiliateId = id;


            return View(m);
        }

        /// <summary>
        /// Partials the dashboard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PartialDashboard(long id = 0)
        {

            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;
            ViewBag.SelectedAffiliateId = id;

            return PartialView("PartialDashboard", m);
        }

        /// <summary>
        /// Channelses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Channels(long id = 0)
        {
            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;

            Affiliate b = new Affiliate();
            b = _affiliateService.GetAffiliateById(id, false);
            ViewBag.BuyerCompanyName = b != null && b.Name != "" ? "Affiliate > " + b.Name + " > " : "";

            ViewBag.SelectedAffiliateId = id;

            return View(m);
        }

        /// <summary>
        /// Campaignses this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Campaigns()
        {
            // Name/Description "Create Affiliate Chanel"
            return View();
        }

        /// <summary>
        /// Payments the options.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PaymentOptions(long id = 0)
        {
            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;

            ViewBag.SelectedAffiliateId = id;

            return View(m);
        }

        /// <summary>
        /// Histories the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult History(long id)
        {
            ViewBag.SelectedAffiliateId = id;
            return View();
        }

        /// <summary>
        /// Userses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Users(long id = 0)
        {
            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;

            ViewBag.SelectedAffiliateId = id;

            Affiliate b = new Affiliate();
            b = _affiliateService.GetAffiliateById(id, false);
            ViewBag.BuyerCompanyName = b != null && b.Name != "" ? "Affiliate > " + b.Name + " > " : "";

            return View(m);
        }

        /// <summary>
        /// Reportses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Reports(long id = 0)
        {
            ViewBag.SelectedAffiliateId = id;

            AffiliateModel m = new AffiliateModel();
            m.AffiliateId = id;

            Affiliate b = new Affiliate();
            b = _affiliateService.GetAffiliateById(id, false);
            ViewBag.BuyerCompanyName = b != null && b.Name != "" ? "Affiliate > " + b.Name + " > " : "";


            return View(m);
        }

        /// <summary>
        /// Get affiliate list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetAffiliates()
        {
            short deleted = 1;

            short.TryParse(Request["d"], out deleted);
            
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            short status = -1;
            if (Request["st"] != null)
            {
                status = short.Parse(Request["st"]);
            }

            List<Affiliate> affiliates = (List<Affiliate>)this._affiliateService.GetAllAffiliates(_permissionService.Authorize(PermissionProvider.AffiliatesShowAll) ? null : _appContext.AppUser, deleted);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = affiliates.Count;
            jd.recordsFiltered = affiliates.Count;
            foreach (Affiliate ai in affiliates)
            {
                if (status != -1 && ai.Status != status) continue;

                Country country = _countryService.GetCountryById(ai.CountryId);
                Core.Domain.Membership.User user = _userService.GetUserById(ai.ManagerId == null ? 0 : (long)ai.ManagerId);
                List<AffiliateChannel> channels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(ai.Id);

                int activeChannels = 0;
                int inactiveChannels = 0;

                foreach (AffiliateChannel c in channels)
                {
                    if (c.Status == 1) activeChannels++;
                    else inactiveChannels++;
                }

                string astatus = "Active";
                string color = "yellow";

                switch (ai.Status)
                {
                    case 1: astatus = "Active"; color = "green"; break;
                    case 0: astatus = "Inactive"; color = "red"; break;                    
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      permissionService.Authorize(PermissionProvider.AffiliatesModify) ? "<a href=\"/Management/Affiliate/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>" : "<b>" + ai.Name + "</b>",
                                      ai.Email,
                                      activeChannels.ToString() + "/" + inactiveChannels.ToString(),
                                      user == null ? "" : user.Username,
                                      ai.CreatedOn.ToShortDateString(),
                                      ai.RegistrationIp,
                                      "<span style='color: " + color + "'>" + astatus + "</span>",
                                      "<a href='#' onclick='deleteAffiliate(" + ai.Id + ")'>" + (ai.IsDeleted.HasValue ? (ai.IsDeleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the can delete.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <returns>System.String.</returns>
        [NonAction]
        public string CheckCanDelete(Affiliate affiliate)
        {
            List<User> users = (List<User>)_userService.GetUsersByParentId(affiliate.Id, SharedData.AffiliateUserTypeId, 0);

            if (users.Count > 0)
            {
                return "Can not delete affiliate because there are active users.";
            }

            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliate.Id, 0);

            if (affiliateChannels.Count > 0)
            {
                return "Can not delete because there are active affiliate channels.";
            }

            List<AffiliateNote> notes = (List<AffiliateNote>)_affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id);

            if (notes.Count > 0)
            {
                return "Can not delete because there are active affiliate notes.";
            }

            return "";
        }

        /// <summary>
        /// Deletes the affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteAffiliate()
        {
            string message = "";

            long affiliateid = 0;

            if (long.TryParse(Request["affiliateid"], out affiliateid))
            {
                Affiliate affiliate = _affiliateService.GetAffiliateById(affiliateid, false);
                if (affiliate != null)
                {
                    if (affiliate.IsDeleted.HasValue)
                    {
                        affiliate.IsDeleted = !affiliate.IsDeleted.Value;
                    }
                    else
                        affiliate.IsDeleted = true;

                    if (affiliate.IsDeleted.Value)
                        message = CheckCanDelete(affiliate);

                    if (message.Length > 0)
                    {
                        affiliate.IsDeleted = !affiliate.IsDeleted.Value;
                        return Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    _affiliateService.UpdateAffiliate(affiliate);

                    this._historyService.AddHistory("AffiliateController", HistoryAction.Affiliate_Deleted, "Affiliate", affiliate.Id, "Name:" + affiliate.Name, "", "", this._appContext.AppUser.Id);
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

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);

            if (affiliate != null)
            {
                User user = _userService.GetUserByParentId(affiliate.Id, SharedData.AffiliateUserTypeId);

                if (user != null)
                {
                    _appContext.SetBackLoginUser(_appContext.AppUser);

                    _authenticationService.SignOut(_appContext);

                    _authenticationService.SignIn(user, false);
                    _appContext.AppUser = user;

                    user.LoginDate = DateTime.UtcNow;
                    user.FailedPasswordAttemptCount = 0;

                    _userService.UpdateUser(user);

                    return RedirectToAction("Dashboard", "Affiliate", new { area = "management" });
                }
            }

            return Redirect(Helper.GetBaseUrl(Request) + "/Management/Home/Dashboard");
        }
    }
}