using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Security;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Message;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Membership;
using Adrack.Service.Lead;
using Adrack.Service.Message;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.Security.Captcha;
using Adrack.WebApi.Infrastructure.Constants;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Core.WrapperData;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.Membership.Login;
using Adrack.WebApi.Models.Membership.Password;
using Adrack.WebApi.Models.Membership.Register;
using Adrack.WebApi.Models.New.Membership;
using Adrack.WebApi.Models.Security;
using Adrack.WebApi.Models.Users;
using Adrack.Service.Configuration;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Adrack.Core.Cache;
using Adrack.WebApi.Models.Sendgrid;
using Adrack.WebApi.Models.Employee;
using System.Web.Script.Serialization;
using Adrack.Core.Domain.Billing;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/authentication")]
    public class MembershipController : BaseApiPublicController

    {
        #region fields
        private readonly UserSetting _userSetting;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAppContext _appContext;
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizedStringService _localizedStringService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IRoleService _roleService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IProfileService _profileService;
        private readonly IAffiliateService _affiliateService;
        private readonly IBuyerService _buyerService;
        private readonly IEmailService _emailService;
        private readonly IGlobalAttributeService _globalAttributeService;
        private readonly IRegistrationRequestService _registrationRequestService;
        private readonly IAccountingService _accountingService;
        private readonly IJWTTokenService _jWTTokenService;
        private readonly ISharedDataWrapper _sharedDataWrapper;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IPermissionService _permissionService;
        private readonly IUsersExtensionService _usersExtensionService;
        private readonly IAddonService _addonService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly IPaymentService _paymentService;

        private readonly EmailOperatorEnums _emailProvider;
        private readonly IPlanService _planService;

        private readonly IEmailSubscriptionService _emailSubscriptionService;
        #endregion

        #region Constructor
        public MembershipController(UserSetting userSetting,
                                    IUserService userService,
                                    IAuthenticationService authenticationService,
                                    IAppContext appContext,
                                    IEncryptionService encryptionService,
                                    ILocalizedStringService localizedStringService,
                                    IUserRegistrationService userRegistrationService,
                                    IRoleService roleService,
                                    ICountryService countryService,
                                    IStateProvinceService stateProvinceService,
                                    IProfileService profileService,
                                    IAffiliateService affiliateService,
                                    IBuyerService buyerService,
                                    IEmailService emailService,
                                    IGlobalAttributeService globalAttributeService,
                                    IRegistrationRequestService registrationRequestService,
                                    IAccountingService accountingService,
                                    IJWTTokenService jWTTokenService,
                                    ISharedDataWrapper sharedDataWrapper,
                                    IRolePermissionService rolePermissionService,
                                    IPermissionService permissionService,
                                    IUsersExtensionService usersExtensionService,
                                    IAddonService addonService,
                                    ISettingService settingService,
                                    ICacheManager cacheManager,
                                    IPaymentService paymentService,
                                    IPlanService planService,
                                    IEmailSubscriptionService emailSubscriptionService)
        {
            _userSetting = userSetting;
            _userService = userService;
            _authenticationService = authenticationService;
            _appContext = appContext;
            _localizedStringService = localizedStringService;
            _userRegistrationService = userRegistrationService;
            _encryptionService = encryptionService;
            _roleService = roleService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _profileService = profileService;
            _affiliateService = affiliateService;
            _buyerService = buyerService;
            _emailService = emailService;
            _globalAttributeService = globalAttributeService;
            _accountingService = accountingService;
            _registrationRequestService = registrationRequestService;
            _jWTTokenService = jWTTokenService;
            _sharedDataWrapper = sharedDataWrapper;
            _rolePermissionService = rolePermissionService;
            _permissionService = permissionService;
            _usersExtensionService = usersExtensionService;
            _addonService = addonService;
            _settingService = settingService;
            _planService = planService;
            _emailSubscriptionService = emailSubscriptionService;
            _paymentService = paymentService;

            var emailProviderSetting = _settingService.GetSetting("System.EmailProvider");
            _emailProvider = emailProviderSetting != null ? (EmailOperatorEnums)Convert.ToInt16(emailProviderSetting.Value) : EmailOperatorEnums.LeadNative;
            _cacheManager = cacheManager;
            
        }
        #endregion

        #region properties
        private string _uploadFolderUrl => (Request != null && Request.RequestUri != null) ? $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/" : string.Empty;
        #endregion

        #region route methods

        #region Register

        [HttpPost]
        [Route("emailValidation")]
        public bool EmailValidation([FromBody]RegisterValidationModel registerValidation)
        {
            if (registerValidation == null)
            {
                ModelState.AddModelError("RegisterValidationModel", "Invalid parameters");
                return false;
            }

            var result = false;
            if (!registerValidation.Resend)
            {
                var registrationRequest = _registrationRequestService.GetRegistrationRequest(registerValidation.Email,
                                                                         registerValidation.ValidationCode);
                result = registrationRequest != null;

                if (registrationRequest != null)
                    _registrationRequestService.DeleteRegistrationRequest(registrationRequest);
            }
            else
            {
                _registrationRequestService.DeleteRegistrationRequest(registerValidation.Email);
                var registrationRequest = new RegistrationRequest
                {
                    Code = Guid.NewGuid().ToString(),
                    Email = registerValidation.Email,
                    Created = DateTime.UtcNow,
                    Name = registerValidation.Name
                };
                _registrationRequestService.InsertRegistrationRequest(registrationRequest);
                _emailService.SendUserWelcomeMessageWithValidationCode(registrationRequest.Email,
                                                                       registrationRequest.Name,
                                                                       registrationRequest.Code,
                                                                       _appContext.AppLanguage.Id,
                                                                       _emailProvider);
                result = true;
            }
            return result;
        }

        [HttpPost]
        [Route("registerNetworkUser")]
        public IHttpActionResult RegisterNetworkUser(RegistrationModel registerModel)
        {
            if (_appContext.AppUser != null)
            {
                _authenticationService.SignOut(_appContext);
            }    

            if (!ModelState.IsValid)
            {
                return HttpBadRequest("model is not valid");
            }

            var existedUser = _userService.GetUserByEmail(registerModel.Email);
            if (existedUser != null)
            {
                return HttpBadRequest("specified email already exist");
            }

            try
            {
                var validationMessage = ValidateDirectory(registerModel.CountryId, registerModel.StateProvinceId);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return HttpBadRequest(validationMessage);
                }

                var user = new User
                {
                    Id = 0,
                    ParentId = 0,
                    GuId = null,
                    Username = registerModel.Email,
                    Email = registerModel.Email,
                    ContactEmail = null,
                    Password = registerModel.Password,
                    SaltKey = null,
                    Active = true,
                    LockedOut = false,
                    Deleted = false,
                    BuiltIn = false,
                    BuiltInName = null,
                    RegistrationDate = DateTime.Now,
                    LoginDate = default,
                    ActivityDate = default,
                    PasswordChangedDate = null,
                    LockoutDate = null,
                    IpAddress = null,
                    FailedPasswordAttemptCount = null,
                    Comment = null,
                    DepartmentId = null,
                    MenuType = null,
                    MaskEmail = false,
                    ValidateOnLogin = false,
                    ChangePassOnLogin = null,
                    TimeZone = null,
                    RemoteLoginGuid = null,
                    ProfilePicturePath = null,
                    UserType = UserTypes.Super,
                    UserTypeId = (short)UserTypes.Super,
                    Department = null
                };

                var registrationRequest = new UserRegistrationRequest(user,
                    registerModel.Email,
                    registerModel.Email,
                    registerModel.Password,
                    "",
                    "",
                    false);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, true);
                if (registrationResult.Success)
                {
                    _profileService.InsertProfile(new Profile
                    {
                        UserId = user.Id,
                        FirstName = registerModel.FirstName,
                        MiddleName = registerModel.MiddleNameName,
                        LastName = registerModel.LastName,
                        Summary = string.Empty,
                        Phone = "",
                        CellPhone = "",
                        CompanyName = registerModel.CompanyName,
                        CompanyWebSite = registerModel.CompanyWebSite,
                        VerticalId = registerModel.VerticalId
                    });

                    user.DepartmentId = 1L;
                    user.Active = false;

                    _globalAttributeService.SaveGlobalAttribute(user,
                        GlobalAttributeBuiltIn.MembershipActivationToken,
                        user.GuId);

                    _userService.UpdateUser(user);

                    var role = _roleService.GetRoleByKey(UserRoleKeys.NetworkUsersKey);
                    if (role != null)
                    {
                        _userService.AddUserRole(role.Id, user.Id);
                    }
                     
                    _emailService.SendNetworkUserRegisteredMessage(user, _appContext.AppLanguage.Id);
                }

                return Ok(user);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("registerAffiliate")]
        public IHttpActionResult RegisterAffiliate(RegistrationModel registerModel)
        {
            if (_appContext.AppUser != null)
            {
                _authenticationService.SignOut(_appContext);
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest("model is not valid");
            }

            var existedUser = _userService.GetUserByEmail(registerModel.Email);
            if (existedUser != null)
            {
                return HttpBadRequest("specified email already exist");
            }

            var validationMessage = ValidateDirectory(registerModel.CountryId, registerModel.StateProvinceId);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                return HttpBadRequest(validationMessage);
            }

            try
            {
                var user = new User
                {
                    Id = 0,
                    ParentId = 0,
                    GuId = null,
                    Username = registerModel.Email,
                    Email = registerModel.Email,
                    ContactEmail = null,
                    Password = registerModel.Password,
                    SaltKey = null,
                    Active = false,
                    LockedOut = false,
                    Deleted = false,
                    BuiltIn = false,
                    BuiltInName = null,
                    RegistrationDate = DateTime.Now,
                    LoginDate = default,
                    ActivityDate = default,
                    PasswordChangedDate = null,
                    LockoutDate = null,
                    IpAddress = null,
                    FailedPasswordAttemptCount = null,
                    Comment = null,
                    DepartmentId = null,
                    MenuType = null,
                    MaskEmail = false,
                    ValidateOnLogin = false,
                    ChangePassOnLogin = null,
                    TimeZone = null,
                    RemoteLoginGuid = null,
                    ProfilePicturePath = null,
                    UserType = UserTypes.Affiliate,
                    UserTypeId = (short)UserTypes.Affiliate,
                    Department = null
                };

                var registrationRequest = new UserRegistrationRequest(user,
                    registerModel.Email,
                    registerModel.Email,
                    registerModel.Password,
                    "",
                    "",
                    false);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, true);
                if (registrationResult.Success)
                {
                    _profileService.InsertProfile(new Profile
                    {
                        UserId = user.Id,
                        FirstName = registerModel.FirstName,
                        MiddleName = registerModel.MiddleNameName,
                        LastName = registerModel.LastName,
                        Summary = string.Empty,
                        Phone = "",
                        CellPhone = "",
                        CompanyName = registerModel.CompanyName,
                        CompanyWebSite = registerModel.CompanyWebSite,
                        VerticalId = registerModel.VerticalId
                    });

                    var affiliateId = _affiliateService.InsertAffiliate(new Affiliate
                    {
                        Id = 0,
                        CountryId = registerModel.CountryId,
                        StateProvinceId = registerModel.StateProvinceId,
                        Name = registerModel.FirstName,
                        AddressLine1 = registerModel.Address,
                        AddressLine2 = registerModel.SecondaryAddress,
                        City = registerModel.City,
                        ZipPostalCode = registerModel.ZipCode,
                        Phone = "",
                        Email = registerModel.Email,
                        UserId = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        ManagerId = null,
                        Status = (short)AffiliateActivityStatuses.Applied,
                        BillFrequency = "",
                        FrequencyValue = null,
                        BillWithin = null,
                        RegistrationIp = "",
                        Website = registerModel.CompanyWebSite,
                        IsDeleted = null,
                        IsBiWeekly = null,
                        WhiteIp = "",
                        DefaultAffiliatePriceMethod = null,
                        DefaultAffiliatePrice = null,
                        IconPath = "",
                        Country = null,
                        StateProvince = null
                    });

                    user.ParentId = affiliateId;
                    user.DepartmentId = 1L;
                    user.Active = false;
                    user.ValidateOnLogin = true;
                    _userService.UpdateUser(user);

                    var role = _roleService.GetRoleByKey(UserRoleKeys.AffiliateUserKey);
                    _userService.AddUserRole(role.Id, user.Id);

                    _globalAttributeService.SaveGlobalAttribute(user,
                                                                 GlobalAttributeBuiltIn.MembershipActivationToken,
                                                                 user.GuId);

                    _emailService.SendEmailUserWelcomeMessageWithValidationCode(registerModel.Email,
                                                                           registerModel.FirstName,
                                                                           user.GuId,
                                                                           _appContext.AppLanguage.Id,
                                                                           _emailProvider);
                }

                return Ok(user);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("usernameValidation")]
        public ValidationModel CheckUsernameAvailability(string username)
        {
            var validationModel = new ValidationModel()
            {
                Result = false,
                Message = _localizedStringService.GetLocalizedString(
                                                 "Membership.CheckUsernameAvailability.NotAvailable")
            };
            if (_userSetting.UsernameEnabled && !string.IsNullOrWhiteSpace(username))
            {
                if (_appContext.AppUser != null &&
                    _appContext.AppUser.Username != null &&
                    _appContext.AppUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    validationModel.Message = _localizedStringService.GetLocalizedString(
                                                   "Membership.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var user = _userService.GetUserByUsername(username);
                    if (user == null)
                    {
                        validationModel.Message = _localizedStringService.GetLocalizedString(
                                                           "Membership.CheckUsernameAvailability.Available");
                        validationModel.Result = true;
                    }
                }
            }
            return validationModel;
        }

        [HttpPost]
        [Route("activateAffiliate")]
        public IHttpActionResult ActivateAffiliate(ActivationModel activationModel)
        {
            var user = _userService.GetUserByEmail(activationModel.Email);

            if (user == null)
            {
                return HttpBadRequest($"no user was found for given email {activationModel.Email}");
            }

            if (user.UserType != UserTypes.Affiliate)
            {
                return HttpBadRequest($"can't activate the user of this type");
            }

            var membershipActivationToken =
                user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.MembershipActivationToken);

            if (string.IsNullOrEmpty(membershipActivationToken) ||
                !membershipActivationToken.Equals(activationModel.Token, StringComparison.InvariantCultureIgnoreCase))
            {
                return HttpBadRequest($"token is incorrect");
            }

            user.Active = true;
            user.ValidateOnLogin = false;
            var affiliate = _affiliateService.GetAffiliateById(user.ParentId, false);
            affiliate.Status = (short)ActivationStatus.Active;
            _userService.UpdateUser(user);
            _affiliateService.UpdateAffiliate(affiliate);
            _globalAttributeService.SaveGlobalAttribute(user,
                                                        GlobalAttributeBuiltIn.MembershipActivationToken,
                                                        "");

            return Ok(_localizedStringService.GetLocalizedString("Membership.MembershipActivation.Activated"));
        }

        [HttpPost]
        [Route("activateNetworkUser")]
        public IHttpActionResult ActivateNetworkUser(string email)
        {
            var user = _userService.GetUserByEmail(email);

            if (user == null)
            {
                return HttpBadRequest($"no user was found for given email {email}");
            }

            if (user.UserType != UserTypes.Network)
            {
                return HttpBadRequest($"can't activate the user of this type");
            }

            user.Active = true;
            user.ValidateOnLogin = false;
            _userService.UpdateUser(user);
            _globalAttributeService.SaveGlobalAttribute(user,
                GlobalAttributeBuiltIn.MembershipActivationToken,
                "");

            return Ok(_localizedStringService.GetLocalizedString("Membership.MembershipActivation.Activated"));
        }

        [HttpGet]
        [Route("activation")]
        public IHttpActionResult UserActivation(string token, string email)
        {
            User user = _userService.GetUserByEmail(email);

            if (user == null)
            {
                return HttpBadRequest("user not found");
            }


            var membershipActivationToken =
                user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.MembershipActivationToken);

            if (string.IsNullOrEmpty(membershipActivationToken))
            {
                return HttpBadRequest("activation token not specified");
            }

            if (!membershipActivationToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return HttpBadRequest("activation token is invalid");

            user.Active = true;
            user.ValidateOnLogin = false;

            _userService.UpdateUser(user);
            _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, "");

            var setting = this._settingService.GetSetting("AppSetting.Url");

            if (setting != null && !string.IsNullOrEmpty(setting.Value))
                return Redirect(setting.Value);

            return Ok("user successfully activated");
        }


        [HttpPost]
        [Route("registerAffiliateByAdmin")]
        public RegisterModel RegisterAffiliateByAdmin([FromBody]RegisterModel registerModel)
        {
            var user = new User();

            if (ModelState.IsValid)
            {
                registerModel.Username = registerModel.Email.Trim();
                var isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;
                user.UserType = _sharedDataWrapper.GetAffiliateUserTypeId();
                RegisterUser(registerModel, user, isApproved);
            }
            return registerModel;
        }

        [HttpPost]
        [Route("registerBuyerByAdmin")]
        public RegisterModel RegisterBuyerByAdmin([FromBody]RegisterModel registerModel)
        {
            var user = new User();
            if (ModelState.IsValid)
            {
                registerModel.Username = registerModel.Email.Trim();
                var buyer = _buyerService.GetBuyerByName(registerModel.Name, 0);
                if (buyer != null)
                {
                    ModelState.AddModelError("RegisterModel", "Buyer with the specified name already exist");
                    return registerModel;
                }

                var isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;
                user.UserType = _sharedDataWrapper.GetBuyerUserTypeId();
                RegisterUser(registerModel, user, isApproved);
            }
            return registerModel;
        }

        [HttpPost]
        [Route("registerUserByAdmin")]
        public RegisterModel RegisterUserByAdmin([FromBody]RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (registerModel.Username != null)
                    registerModel.Username = registerModel.Username.Trim();

                if (registerModel.UserRoleId == 0)
                {
                    if (registerModel.UserType == _sharedDataWrapper.GetAffiliateUserTypeId() ||
                        registerModel.UserType == _sharedDataWrapper.GetBuyerUserTypeId())
                        registerModel.UserRoleId = (long)UserRoleType.NetworkUsers;
                    else
                        registerModel.UserRoleId = (long)UserRoleType.AccountManager;
                }
                var user = _userService.GetUserById(registerModel.Id);
                if (user == null)
                {
                    user = new User
                    {
                        ValidateOnLogin = true,
                        GuId = Guid.NewGuid().ToString(),
                        ParentId = registerModel.ParentId,
                        DepartmentId = 1,
                        MaskEmail = registerModel.IsMaskEmail,
                        UserType = registerModel.UserType,
                        TimeZone = registerModel.TimeZone
                    };

                    registerModel.ConfirmPassword = registerModel.Password;
                    RegisterUser(registerModel, user, true);
                }
                else
                {
                    var profile = _profileService.GetProfileByUserId(registerModel.Id);

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
                    user.MaskEmail = registerModel.IsMaskEmail;
                    user.TimeZone = registerModel.TimeZone;
                    user.ParentId = registerModel.ParentId;
                    string saltKey = _encryptionService.CreateSaltKey(20);
                    user.SaltKey = saltKey;
                    user.Password = _encryptionService.CreatePasswordHash(registerModel.Password, saltKey);
                    var role = (from x in user.Roles
                                where x.Id != 3
                                select x).FirstOrDefault();

                    if (role != null)
                    {
                        user.Roles.Remove(role);
                    }

                    role = _roleService.GetRoleById(registerModel.UserRoleId);
                    user.Roles.Add(role);

                    user.Active = registerModel.IsActive;
                    if (user.Active && _appContext.AppUser != null && (_appContext.AppUser.UserType == _sharedDataWrapper.GetBuiltInUserTypeId() ||
                                                                       _appContext.AppUser.UserType == _sharedDataWrapper.GetNetworkUserTypeId()))
                        user.ValidateOnLogin = false;
                    user.LockedOut = registerModel.IsLockedOut;

                    _profileService.UpdateProfile(profile);
                    _userService.UpdateUser(user);

                }
            }

            return registerModel;
        }

        #endregion

        #region Login
        [HttpPost]
        [Route("signIn")]
        public IHttpActionResult Login(SignInModel userModel)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest("UserName or Password is incorrect");

            var loginResult = _userRegistrationService.ValidateUser(userModel.UserName, userModel.Password);
            switch (loginResult)
            {
                case UserLoginResult.Successful:
                    {
                        var user = _userService.GetUserByUsername(userModel.UserName);

                        _authenticationService.SignIn(user, false);

                        var accessToken = _jWTTokenService.GenerateAccessToken(user.Id, user.Username);
                        var newRefreshToken = _jWTTokenService.GenerateRefreshToken(user.Id, user.Username);

                        _appContext.AppUser = user;

                        return Ok(new
                        {
                            accessToken = accessToken,
                            refreshToken = newRefreshToken
                        });
                    }

                case UserLoginResult.UserNotExist:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.UserNotExist"));
                case UserLoginResult.Deleted:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.Deleted"));
                case UserLoginResult.NotActive:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.NotActive"));
                case UserLoginResult.NotRegistered:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.NotRegistered"));
                case UserLoginResult.LockedOut:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials.LockedOut"));
                case UserLoginResult.WrongPassword:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials"));
                case UserLoginResult.NotValidated:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.NotValidated"));
                default:
                    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.Login.WrongCredentials"));
            }
        } 

        [HttpPost]
        [HttpGet]
        [Route("masterSignIn")]
        public async Task<IHttpActionResult> MasterLogin(string token, string key, string requestKey)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest("UserName or Password is incorrect");

            token = HttpUtility.UrlDecode(token);

            string email = _encryptionService.SimpleDecrypt(token, key);

            var user = _userSetting.UsernameEnabled
                ? _userService.GetUserByUsername(email)
                : _userService.GetUserByEmail(email);

            if (user == null)
                return HttpBadRequest("User not found");

            string masterAdminUrl = ConfigurationManager.AppSettings["MasterAdminUrl"];

            if (string.IsNullOrEmpty(masterAdminUrl))
                return HttpBadRequest("User not found");

            //var json = JsonConvert.SerializeObject(person);
            //var data = new StringContent("", Encoding.UTF8, "application/json");

            var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{masterAdminUrl}/api/authentication/checkLoginSession?sessionKey={key}", new StringContent(""));
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return HttpBadRequest("User not found");
                }

                _authenticationService.SignIn(user, false);

                var accessToken = _jWTTokenService.GenerateAccessToken(user.Id, user.Username);
                var newRefreshToken = _jWTTokenService.GenerateRefreshToken(user.Id, user.Username);

                _cacheManager.Set($"user{user.Id}-requestkey", requestKey, 500000);

                _appContext.AppUser = user;

                return Ok(new
                {
                    accessToken = accessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch(Exception ex)
            {
                return HttpBadRequest("User not found");
            }

            return Ok();
        }

        [HttpPost]
        [Route("remoteCheck")]
        public bool RemoteCheck(RemoteLoginModel remoteLoginModel)
        {
            var email = remoteLoginModel.Email;

            var user = _userSetting.UsernameEnabled
                ? _userService.GetUserByUsername(email)
                : _userService.GetUserByEmail(email);

            if (user != null) return true;

            return false;
        }

        [ContentManagementApiAuthorize(false)]
        [HttpPost]
        [Route("refreshToken")]
        public IHttpActionResult Refresh()
        {
            var token = GetTokenInRequestHeader();
            JWTVerficationResults jWTVerficationResult = JWTVerficationResults.None;

            var principal = _jWTTokenService.VerifyUser(token, out jWTVerficationResult);
            var userId = principal;

            var user = _userService.GetUserById(userId);

            if (jWTVerficationResult != JWTVerficationResults.Valid || user == null || !user.Active || user.Deleted)
            {
                return HttpBadRequest("session-expired");
            }

            var newJwtToken = _jWTTokenService.GenerateAccessToken(userId, user.Username);
            var newRefreshToken = _jWTTokenService.GenerateRefreshToken(userId, user.Username);

            return Ok(new
            {
                accessToken = newJwtToken,
                refreshTokens = newRefreshToken
            });
        }

        [ContentManagementApiAuthorize(false)]
        [HttpGet]
        [Route("me")]
        public IHttpActionResult GetUserInfo()
        {
            var user = _userService.GetUserById(_appContext.AppUser.Id);

            var userModel = (UserInfoViewModel)user;

            Role role = null;
            var roleId = user.Roles.FirstOrDefault()?.Id;
            if (roleId != null)
            {
                role = _roleService.GetRoleById(roleId.Value);
            }

            if (role == null && _appContext.AppUser.UserType == UserTypes.Super)
            {
                role = new Role()
                {
                    Id = 1,
                    Name = "SuperRole",
                    Key = "SuperRole",
                    Active = true,
                    BuiltIn = true,
                    Deleted = false,
                    UserType = UserTypes.Super
                };
            }

            if (role != null)
            {
                var permissions = _permissionService.GetAllPermissions(0);

                List<RolePermission> rolePermissions = new List<RolePermission>();
                if (role.RolePermissions != null)
                    rolePermissions = role.RolePermissions.ToList();

                userModel.Role = (RoleModel)role;
                foreach (var item in permissions)
                {
                    var headPermission = (PermissionModel)item;

                    RolePermission rolePermission = null; 
                    if (role.RolePermissions != null)
                    {
                        rolePermission = role.RolePermissions.FirstOrDefault(x => x.PermissionId == item.Id);
                    }

                    var addonPermissions = _addonService.GetAddonsByPermissionId(item.Id);

                    if (addonPermissions.Count == 0)
                        headPermission.IsAccess = (_appContext.AppUser.UserType == UserTypes.Super || (rolePermission != null && rolePermission.State == 1)) ? true : false;
                    else
                        headPermission.IsAccess = (rolePermission != null && rolePermission.State == 1) ? true : false;

                    if (_appContext.AppUser.UserType != UserTypes.Super && 
                        _appContext.AppUser.UserType != UserTypes.Network &&
                        !item.UserTypeList.Contains(_appContext.AppUser.UserType))
                    {
                        headPermission.IsAccess = false;
                    }

                    var permissionModel =
                    _rolePermissionService.BuildPermissionTreeWithState(headPermission,
                        rolePermissions);
                    userModel.Role.Permissions.Add(permissionModel);
                }
            }

            if (user.UserType != UserTypes.Super)
                userModel.Addons = _usersExtensionService.GetAddons(user.Id);
            else
            {
                userModel.Addons = new List<Models.BaseModels.UserAddonsModel>();
                var addons =_addonService.GetAllAddons();
                foreach(var addon in addons)
                {
                    var userAddon = _addonService.GetAddonsByUserId(user.Id).Where(x => x.AddonId == addon.Id).FirstOrDefault();

                    var addonTrialStatus = 0;
                    if (userAddon != null)
                        addonTrialStatus = CheckUserAddonTrial(userAddon, user);

                    userModel.Addons.Add(new Models.BaseModels.UserAddonsModel() {
                        AddData = addon.AddData,
                        Date = DateTime.UtcNow,
                        Key = addon.Key,
                        Name = addon.Name,
                        Id = addon.Id,
                        Amount = userAddon != null && userAddon.Amount.HasValue ? userAddon.Amount.Value : 0,
                        Status = addonTrialStatus == 2 ? (short)0 : (userAddon != null && userAddon.Status.HasValue ? userAddon.Status.Value : (short)0)
                    });

                }
            }

            userModel.TrialDays = (short)(DateTime.UtcNow.AddDays(14) - user.RegistrationDate).TotalDays;
            IList<Payment> p = _paymentService.GetPaymentsByUser(_appContext.AppUser.Id);
            
            if (p != null && p.Count > 0)
            {
                userModel.hasCard = true;
            }
            else
            {
                userModel.hasCard = false;
            }

            Uri uri;
            string imageUrl = user.ProfilePicturePath;

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out uri) && !string.IsNullOrEmpty(imageUrl))
                imageUrl = $"{_uploadFolderUrl}Avatars/{user.ProfilePicturePath}";

            userModel.Campaigns = _usersExtensionService.GetCampaignOwnerships(user.Id);
            userModel.Affiliates = _usersExtensionService.GetAffiliateOwnerships(user.Id);
            userModel.AffiliateChannels = _usersExtensionService.GetAffiliateChannelOwnerships(user.Id);
            userModel.Buyers = _usersExtensionService.GetBuyerOwnerships(user.Id);
            userModel.BuyerChannels = _usersExtensionService.GetBuyerChannelOwnerships(user.Id);
            userModel.ProfilePicture = imageUrl;
            userModel.TestVar = "test ";
            userModel.PlanLimitation = _planService.CheckPlanStatusesByUserId(user.Id);

            return Ok(userModel);
        }


        [HttpPost]
        [Route("signOut")]
        public IHttpActionResult SignOut()
        {
            var token = GetTokenInRequestHeader();
            var userId = _jWTTokenService.VerifyUser(token);
            if (userId == 0)
                return Ok();

            var refreshTokens = _jWTTokenService.GetAllRefreshTokens() ?? new Dictionary<long, string>();
            var refreshToken = refreshTokens.FirstOrDefault(x => x.Key == userId);
            //if (refreshToken.Equals(default(KeyValuePair<long, string>)))
                //return HttpBadRequest(null);
            refreshTokens[userId] = null;
            _jWTTokenService.InsertRefreshToken(refreshTokens);
            return Ok();
        }
        #endregion

        #region Forgot Password

        [HttpPost]
        [Route("forgotPassword")]
        [AppHttpsRequirement(SslRequirement.Yes)]
        public IHttpActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(null);
            }

            var user = _userService.GetUserByEmail(forgotPasswordModel.Email);

            if (user == null || !user.Active || user.Deleted || user.LockedOut)
            {
                return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.EmailNotFound"));
            }

            var forgotPasswordToken = Guid.NewGuid().ToString();
            _globalAttributeService.SaveGlobalAttribute(user,
                                                        GlobalAttributeBuiltIn.ForgotPasswordToken,
                                                        forgotPasswordToken);
            _globalAttributeService.SaveGlobalAttribute(user,
                                                        GlobalAttributeBuiltIn.ForgotPasswordTokenRequestedDate,
                                                        DateTime.UtcNow);
            _emailService.SendUserForgotPasswordMessage(user, _appContext.AppLanguage.Id, _emailProvider);

            return Ok(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.EmailSent"));
        }

        [HttpPost]
        [Route("forgotPasswordConfirmation")]
        [AppHttpsRequirement(SslRequirement.Yes)]
        public IHttpActionResult ForgotPasswordConfirmation(ForgotPasswordConfirmationModel forgotPasswordConfirmationModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(null);
            }

            var user = _userService.GetUserByEmail(forgotPasswordConfirmationModel.Email);

            if (user == null || !user.Active || user.Deleted || user.LockedOut)
            {
                return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.EmailNotFound"));
            }

            if (!user.IsForgotPasswordTokenValid(forgotPasswordConfirmationModel.Token))
            {
                forgotPasswordConfirmationModel.DisablePasswordChanging = true;
                return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.WrongToken"));
            }
            
            //if (user.IsForgotPasswordLinkExpired(_userSetting))
            //{
            //    forgotPasswordConfirmationModel.DisablePasswordChanging = true;
            //    return HttpBadRequest(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.LinkExpired"));
            //}

            if (!forgotPasswordConfirmationModel.NewPassword.Equals(forgotPasswordConfirmationModel.ConfirmNewPassword))
            {
                return HttpBadRequest(_localizedStringService.
                    GetLocalizedString("Membership.Field.ChangePassword.NewPassword.EnteredPasswordsDoNotMatch"));
            }

            var changePasswordRequest = _userRegistrationService.ChangePassword(new ChangePasswordRequest(false,
                                                                                forgotPasswordConfirmationModel.Email,
                                                                                forgotPasswordConfirmationModel.NewPassword));

            if (!changePasswordRequest.Success)
            {
                return HttpBadRequest(null);
            }

            _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.ForgotPasswordToken, "");
            _emailService.SendUserPasswordChangeMessage(user, _appContext.AppLanguage.Id, _emailProvider);
            forgotPasswordConfirmationModel.DisablePasswordChanging = true;

            return Ok(_localizedStringService.GetLocalizedString("Membership.ForgotPassword.PasswordSuccessfullyChanged"));
        }

        [HttpPost]
        [Route("sendTestEmail/{type}")]
        public IHttpActionResult SendTest(Byte type, [FromBody]EmailParameters parameters)
        {
            switch (type)
            {
                case 1:
                    _emailService.SendUserWelcomeMessage(new User() { Email = parameters.Email, BuiltInName =  parameters.Name },
                                                                   _appContext.AppLanguage.Id,
                                                                   EmailOperatorEnums.SendGrid);
                    break;
                case 2:
                    _emailService.SendUserRegisteredMessage(new User() { Username =  parameters.Name, Email =  parameters.Email, BuiltInName =  parameters.Name },
                                                                   _appContext.AppLanguage.Id,
                                                                   EmailOperatorEnums.SendGrid);
                    break;
                case 3:
                    _emailService.SendUserEmailValidationMessage(new User() { Username =  parameters.Name, Email =  parameters.Email, BuiltInName =  parameters.Name },
                                                                    _appContext.AppLanguage.Id,
                                                                    EmailOperatorEnums.SendGrid);
                    break;
                case 4:
                    _emailService.SendUserForgotPasswordMessage(new User() { Email =  parameters.Email, BuiltInName =  parameters.Name },
                                                        _appContext.AppLanguage.Id,
                                                        EmailOperatorEnums.SendGrid);
                    break;
                case 5:
                    _emailService.SendEmailSubscriptionActivationMessage(new EmailSubscription() { Email =  parameters.Email },
                                                        _appContext.AppLanguage.Id,
                                                        EmailOperatorEnums.SendGrid);
                    break;
                case 6:
                    _emailService.SendEmailSubscriptionDeactivationMessage(new EmailSubscription() { Email =  parameters.Email },
                                                        _appContext.AppLanguage.Id,
                                                        EmailOperatorEnums.SendGrid);
                    break;
                case 7:
                    _emailService.SendNewTicketMessage( parameters.Email, parameters.Name,
                                                        EmailOperatorEnums.SendGrid);
                    _emailService.SendUserNewTicketMessage(new User() { Email =  parameters.Email, BuiltInName =  parameters.Name }, 1, EmailOperatorEnums.SendGrid);
                    break;
                case 8:
                    _emailService.SendNewTicket( parameters.Email,
                                                         parameters.Name,
                                                        EmailOperatorEnums.SendGrid);
                    break;
                case 9:
                    _emailService.SendEmailUserWelcomeMessageWithValidationCode( parameters.Email,
                                                         parameters.Name,
                                                        "4638",
                                                        1,
                                                        EmailOperatorEnums.SendGrid);
                    _emailService.SendUserWelcomeMessageWithValidationCode( parameters.Email,
                                                         parameters.Name,
                                                        "4638",
                                                        1,
                                                        EmailOperatorEnums.SendGrid);
                    break;
                case 10:
                    _emailService.SendUserManagerRejectMessage(new User() { Email =  parameters.Email, BuiltInName =  parameters.Name }
                    , 1, EmailOperatorEnums.SendGrid);

                    break;
                case 11:
                    _emailService.SendUserWelcomeMessageWithUsernamePassword(new User() { Email =  parameters.Email, BuiltInName =  parameters.Name }
                    , 1,EmailOperatorEnums.SendGrid, parameters.UserName, "789456123");
                    break;
                case 12:
                    _emailService.SendUserPasswordChangeMessage(new User() { Email =  parameters.Email, BuiltInName =  parameters.Name }
                    , 1,EmailOperatorEnums.SendGrid);
                    break;
                case 13:
                    _emailService.SendCapReachNotification( parameters.Email, "Channel1" ,"Buyer1", parameters.Email,EmailOperatorEnums.SendGrid
                    );
                    break;
                case 14:
                    _emailService.SendTimeoutNotification( parameters.Email, "Channel1","Buyer1","Timeout message", parameters.Email,EmailOperatorEnums.SendGrid
                    );
                    break;
                default:
                    break;
            }

            return Ok();
        }


        [HttpGet]
        [Route("userInvitationForm")]
        public IHttpActionResult UserInvitationForm(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return HttpBadRequest("email");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return HttpBadRequest("token");
            }

            var invitedUserToken = _globalAttributeService.GetGlobalAttributeByKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, token);
            if (invitedUserToken == null)
            {
                return HttpBadRequest("invited User token is invalid");
            }

            var employeeModel = new InvitedEmployeeModel();
            employeeModel.Email = email;
            employeeModel.InvitedUserToken = token;

            return Ok(employeeModel);
        }



        [HttpPost]
        [Route("addInvitatedUser")]
        public IHttpActionResult AddInvitatedUser([FromBody] InvitedEmployeeModel employeeModel)
        {
            try
            {
                if (employeeModel == null)
                {
                    return HttpBadRequest(null);
                }

                var invitedUserToken = _globalAttributeService.GetGlobalAttributeByKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, employeeModel.InvitedUserToken);
                if (invitedUserToken == null)
                {
                    return HttpBadRequest("Wrong Invited User token");
                }

                if (string.IsNullOrEmpty(invitedUserToken.ExtraData))
                {
                    return HttpBadRequest("Wrong Invited User's ExtraData");
                }

                String[] extraData = invitedUserToken.ExtraData.Split(';');
                if (employeeModel.Email.Trim() != extraData[0].Trim())
                {
                    return HttpBadRequest("Wrong Invited User's Email");
                }

                long roleId = Convert.ToInt64(extraData[1]);
                UserTypes inviterUserType = (UserTypes)Convert.ToInt64(extraData[2]);
                long inviterId = Convert.ToInt64(extraData[3]);

                var role = _roleService.GetRoleById(roleId);
                if (role == null || role.Deleted || !role.Active)
                {
                    return HttpBadRequest($"no role was found for given id {roleId}");
                }

                User employee = _userService.GetUserByEmail(employeeModel.Email);
                if (employee != null)
                {
                    return HttpBadRequest($"User with '{employeeModel.Email}' email already exist");
                }

                if (string.IsNullOrWhiteSpace(employeeModel.Password) || employeeModel.Password.Length <= 7)
                {
                    return HttpBadRequest($"Password must be longer than 7 characters.");
                }
                string generatedPassword = employeeModel.Password;

                IEnumerable<string> headerValues;
                Request.Headers.TryGetValues("Authorization", out headerValues);
                string token = headerValues != null ? headerValues.First() : "";

                var headers = new Dictionary<string, string>();
                headers.Add("Authentication", "Bearer " + token);

                string requestKey = _cacheManager.Get($"user{_appContext.AppUser.Id}-requestkey")?.ToString();

                string masterAdminUrl = ConfigurationManager.AppSettings["MasterAdminUrl"];

                string json = new JavaScriptSerializer().Serialize(new
                {
                    firstName = employeeModel.FirstName,
                    lastName = employeeModel.LastName,
                    email = employeeModel.Email,
                    password = generatedPassword,
                    confirmPassword = generatedPassword,
                    zipCode = "33333",
                    webSite = "http://adrack.com",
                    companyName = "a",
                    address = "a",
                    phone = "8184567894",
                    city = "a",
                    countryId = 80,
                    token = requestKey
                });

                string remoteSignUpResult = "";//Helpers.Helpers.Post($"{masterAdminUrl}/api/authentication/remoteSignUp", json, "application/json", "POST", headers);
                remoteSignUpResult = "";

                if (remoteSignUpResult.ToLower() != "error")
                {
                    string saltKey = _encryptionService.CreateSaltKey(20);
                    string password = _encryptionService.CreatePasswordHash(generatedPassword, saltKey);

                    employee = new User
                    {
                        Id = 0,
                        ParentId = 0,
                        UserType = inviterUserType,
                        GuId = Guid.NewGuid().ToString(),
                        Username = employeeModel.Email,
                        Email = employeeModel.Email,
                        ContactEmail = employeeModel.Email,
                        Password = password,
                        SaltKey = saltKey,
                        Active = true,
                        LockedOut = false,
                        Deleted = false,
                        BuiltIn = false,
                        BuiltInName = null,
                        RegistrationDate = DateTime.Now,
                        LoginDate = DateTime.Now,
                        ActivityDate = DateTime.Now,
                        PasswordChangedDate = null,
                        LockoutDate = null,
                        IpAddress = null,
                        FailedPasswordAttemptCount = null,
                        Comment = null,
                        DepartmentId = 1,
                        MenuType = null,
                        MaskEmail = false,
                        ValidateOnLogin = false,
                        ChangePassOnLogin = false,
                        TimeZone = null,
                        RemoteLoginGuid = null
                    };
                    employee.Roles.Add(role);
                    _userService.InsertUser(employee);

                    var profile = new Profile
                    {
                        Id = 0,
                        UserId = employee.Id,
                        FirstName = employeeModel.FirstName,
                        LastName = employeeModel.LastName,
                        MiddleName = employeeModel.MiddleName,
                        JobTitle = employeeModel.JobTitle,
                        Phone = "",
                        CellPhone = "",
                        Summary = "",
                    };
                    _profileService.InsertProfile(profile);

                    /*
                    _userService.AddEntityOwnerships(employeeModel.Campaigns,
                        employeeModel.Buyers,
                        employeeModel.BuyerChannels,
                        employeeModel.Affiliates,
                        employeeModel.AffiliateChannels,
                        employee.Id);
                    */

                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = employee.Id,
                        EntityId = inviterId
                    };

                    if (inviterUserType == UserTypes.Affiliate)
                    {
                        access.EntityName = EntityType.Affiliate.ToString();
                    }
                    else if (inviterUserType == UserTypes.Buyer)
                    {
                        access.EntityName = EntityType.Buyer.ToString();
                    }

                    _userService.InsertEntityOwnership(access);

                    _globalAttributeService.DeleteGlobalAttributeByKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken.Value);

                    _affiliateService.UpdateInvitationStatus(employee.Email);
                    _buyerService.UpdateInvitationStatus(employee.Email);

                    /*var setting = this._settingService.GetSetting("AppSetting.Url");
                    if (setting != null && !string.IsNullOrEmpty(setting.Value))
                        return Redirect(setting.Value);*/

                    return Ok(employeeModel);
                }
                else
                {
                    return HttpBadRequest("Error");
                }
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("firstPasswordForm")]
        public IHttpActionResult FirstPasswordForm(string token, string email)
        {
            User user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return HttpBadRequest("user not found");
            }

            var firstPasswordForm = user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.FirstPasswordToken);
            if (string.IsNullOrEmpty(firstPasswordForm))
            {
                return HttpBadRequest("first password token not specified");
            }

            if (!firstPasswordForm.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return HttpBadRequest("first password token is invalid");

            var firstPasswordConfirmationModel = new FirstPasswordConfirmationModel();
            firstPasswordConfirmationModel.Email = email;
            firstPasswordConfirmationModel.Token = token;
            
            return Ok(firstPasswordConfirmationModel);
        }



        [HttpPost]
        [Route("firstPasswordConfirmation")]
        [AppHttpsRequirement(SslRequirement.Yes)]
        public IHttpActionResult FirstPasswordConfirmation(FirstPasswordConfirmationModel firstPasswordConfirmationModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(null);
            }

            var user = _userService.GetUserByEmail(firstPasswordConfirmationModel.Email);

            if (user == null || user.Deleted)
            {
                return HttpBadRequest("Email not found.");
            }

            if (!user.IsFirstPasswordTokenValid(firstPasswordConfirmationModel.Token))
            {
                //firstPasswordConfirmationModel.DisablePasswordChanging = true;
                return HttpBadRequest("Wrong first password token");
            }

            if (!firstPasswordConfirmationModel.NewPassword.Equals(firstPasswordConfirmationModel.ConfirmNewPassword))
            {
                return HttpBadRequest(_localizedStringService.
                    GetLocalizedString("Membership.Field.ChangePassword.NewPassword.EnteredPasswordsDoNotMatch"));
            }

            var changePasswordRequest = _userRegistrationService.ChangePassword(new ChangePasswordRequest(false,
                firstPasswordConfirmationModel.Email,
                firstPasswordConfirmationModel.NewPassword));

            if (!changePasswordRequest.Success)
            {
                return HttpBadRequest(null);
            }
           
            user.Active = true;
            _userService.UpdateUser(user);
            
            _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.FirstPasswordToken, "");
            //firstPasswordConfirmationModel.DisablePasswordChanging = true;

            //var setting = this._settingService.GetSetting("AppSetting.Url");
            //if (setting != null && !string.IsNullOrEmpty(setting.Value))
            //    return Redirect(setting.Value);

            return Ok("Password successfully updated");
        }

        [ContentManagementApiAuthorize(false)]
        [HttpPost]
        [Route("changePassword")]
        public IHttpActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(null);
            }

            var user = _userService.GetUserByUsername(_appContext.AppUser.Username);

            if (user == null || !user.Active || user.Deleted || user.LockedOut)
            {
                return HttpBadRequest("User not found");
            }

            if (user.Password != _encryptionService.CreatePasswordHash(changePasswordModel.OldPassword, user.SaltKey))
            {
                return HttpBadRequest("Password is incorrect");
            }

            if (!changePasswordModel.Password.Equals(changePasswordModel.ConfirmPassword))
            {
                return HttpBadRequest(_localizedStringService.
                    GetLocalizedString("Membership.Field.ChangePassword.NewPassword.EnteredPasswordsDoNotMatch"));
            }

            var saltKey = _encryptionService.CreateSaltKey(20);
            user.SaltKey = saltKey;
            user.Password = _encryptionService.CreatePasswordHash(changePasswordModel.Password, saltKey);
            user.ChangePassOnLogin = false;
            _userService.UpdateUser(user);
            _appContext.AppUser = user;

            return Ok(_localizedStringService.GetLocalizedString("Membership.ChangePassword.Success"));
        }

        #endregion


        [HttpPost]
        [Route("emailIsAvailable")]
        public bool EmailIsAvailable(string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user != null)
            {
                return false;
            }

            var affiliateInv = _affiliateService.GetAffiliateInvitationByEmail(email);
            if (affiliateInv != null)
            {
                return false;
            }

            var buyerInv = _buyerService.GetBuyerInvitationByEmail(email);
            if (buyerInv != null)
            {
                return false;
            }

            return true;
        }



        #endregion


        #region Email unsubscribe functionality



        [HttpGet]
        [Route("unsubscribeEmail")]
        public IHttpActionResult UnsubscribeEmail(string token)
        {
            var obj = _emailSubscriptionService.GetEmailSubscriptionByGuId(token);
            if (obj == null)
            {
                return HttpBadRequest("EmailSubscription not found");
            }

            obj.Active = false;
            _emailSubscriptionService.UpdateEmailSubscription(obj);

            return Ok();
        }

        [HttpGet]
        [Route("getUnsubscribeEmailList")]
        public IHttpActionResult GetUnsubscribeEmailList()
        {
            var emailUnsubscriptions = _emailSubscriptionService.GetUnsubscribeEmailList();
            
            return Ok(emailUnsubscriptions);
        }


        [HttpPost]
        [Route("sendEmailUnsubscribeMessage")]
        public IHttpActionResult SendEmailUnsubscribeMessage(long emailSubscriptionId)
        {
            var obj = _emailSubscriptionService.GetEmailSubscriptionById(emailSubscriptionId);
            if (obj == null)
            {
                return HttpBadRequest("EmailSubscription not found");
            }

            _emailService.SendEmailSubscriptionDeactivationMessage(obj,_appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative);

            return Ok();
        }
        #endregion

        #region private service methods

        private Role GetUserRoleIdByKey(string key)
        {
            return _roleService.GetRoleByKey(key);
        }

        private Role GetUserRoleById(long id)
        {
            return _roleService.GetRoleById(id);
        }

        #endregion

        #region Private methods

        protected int CheckUserAddonTrial(UserAddons userAddon, User user)
        {
            if (userAddon.IsTrial > 0)
            {
                var diffOfDates = DateTime.UtcNow.Subtract(userAddon.Date);
                if (diffOfDates.Days <= 7)
                {
                    return 1; //Trial
                }
                else
                {
                    userAddon.Status = (short)0; //0 - Deactivate
                    _addonService.UpdateUserAddon(userAddon);
                    _emailService.SendAddonChangeStatusMessage(user.Email, _appContext.AppLanguage.Id, "Deactivate ");

                    return 2; //Trial Expired
                }
            }

            return 0; //Not Trial
        }

        private void RegisterUser(RegisterModel registerModel, User user, bool isApproved)
        {
            var registrationRequest = new UserRegistrationRequest(user,
                registerModel.Username,
                registerModel.Email,
                registerModel.Password,
                registerModel.Comment,
                registerModel.ContactEmail,
                isApproved);
            var registrationResult = _userRegistrationService.RegisterUser(registrationRequest, false);
            if (registrationResult.Success)
                InsertUser(registerModel, user);

            foreach (var error in registrationResult.Errors)
                ModelState.AddModelError("RegisterModel", error);
        }
        private void InsertUser(RegisterModel registerModel, User user)
        {
            InsertProfile(registerModel, user);

            if (registerModel.ParentId == 0)
            {
                if (registerModel.UserRoleId != GetUserRoleIdByKey(UserRoleKeys.GlobalAdministratorsKey).Id &&
                    registerModel.UserType == _sharedDataWrapper.GetBuyerUserTypeId())
                {
                    InsertBuyer(registerModel, user);
                }
                else if (registerModel.UserRoleId != GetUserRoleIdByKey(UserRoleKeys.GlobalAdministratorsKey).Id &&
                        registerModel.UserType == _sharedDataWrapper.GetAffiliateUserTypeId())
                {
                    InsertAffiliate(registerModel, user);
                }
            }
            else
            {
                user.ParentId = registerModel.ParentId;
            }

            user.DepartmentId = 1L;
            var role = _roleService.GetRoleById(registerModel.UserRoleId);
            if (role == null)
            {
                if (registerModel.UserType == _sharedDataWrapper.GetAffiliateUserTypeId() ||
                    registerModel.UserType == _sharedDataWrapper.GetBuyerUserTypeId())
                    role = _roleService.GetRoleByKey(UserRoleKeys.AccountManagerKey);
            }

            if (role != null)
            {
                user.Roles.Add(role);
            }
            user.TimeZone = registerModel.TimeZone;
            user.Active = true;
            user.ChangePassOnLogin = registerModel.IsChangePassOnLogin;
            user.LockedOut = registerModel.IsLockedOut;
            if (user.Active && _appContext.AppUser != null &&
                (_appContext.AppUser.UserType == _sharedDataWrapper.GetBuiltInUserTypeId() ||
                 _appContext.AppUser.UserType == _sharedDataWrapper.GetNetworkUserTypeId()))
                user.ValidateOnLogin = false;
            else
                user.ValidateOnLogin = true;
            _globalAttributeService.SaveGlobalAttribute(user,
                                                        GlobalAttributeBuiltIn.MembershipActivationToken,
                                                        user.GuId);
            _userService.UpdateUser(user);

            _emailService.SendUserWelcomeMessageWithUsernamePassword(user,
                                                                    _appContext.AppLanguage.Id,
                                                                    _emailProvider,
                                                                    registerModel.Email,
                                                                    registerModel.Password);
        }
        private void InsertProfile(RegisterModel registerModel, User user)
        {
            _profileService.InsertProfile(new Profile
            {
                UserId = user.Id,
                FirstName = registerModel.FirstName,
                MiddleName = registerModel.MiddleName,
                LastName = registerModel.LastName,
                Summary = string.Empty,
                Phone = registerModel.Phone,
                CellPhone = registerModel.CellPhone
            });
        }

        private void InsertBuyer(RegisterModel registerModel, User user)
        {
            var buyer = new Buyer()
            {
                Name = registerModel.Name,
                CountryId = registerModel.CountryId,
                StateProvinceId = registerModel.StateProvinceId,
                Email = registerModel.CompanyEmail,
                AddressLine1 = registerModel.AddressLine1,
                AddressLine2 = registerModel.AddressLine2,
                City = registerModel.City,
                ZipPostalCode = registerModel.ZipPostalCode,
                Phone = registerModel.CompanyPhone,
                CreatedOn = DateTime.UtcNow,
                Status = 1,
                ManagerId = registerModel.ManagerId,
                BillFrequency = "m",
                FrequencyValue = 1,
                AlwaysSoldOption = registerModel.AlwaysSoldOption,
                MaxDuplicateDays = registerModel.MaxDuplicateDays,
                DailyCap = registerModel.DailyCap,
                Description = registerModel.Description,
                DoNotPresentResultField = registerModel.DoNotPresentResultField,
                DoNotPresentResultValue = registerModel.DoNotPresentResultValue,
                DoNotPresentStatus = registerModel.DoNotPresentStatus,
                DoNotPresentPostMethod = registerModel.DoNotPresentPostMethod,
                DoNotPresentRequest = registerModel.DoNotPresentRequest,
                DoNotPresentUrl = registerModel.DoNotPresentUrl,
                CanSendLeadId = registerModel.CanSendLeadId,
                AccountId = registerModel.AccountId
            };

            var buyerId = _buyerService.InsertBuyer(buyer);
            user.ParentId = buyerId;
            var buyerBalance = new BuyerBalance
            {
                Credit = registerModel.Credit,
                Balance = registerModel.Credit,
                BuyerId = buyerId,
                PaymentSum = 0M,
                SoldSum = 0M
            };
            _accountingService.InsertBuyerBalance(buyerBalance);
        }

        private void InsertAffiliate(RegisterModel registerModel, User user)
        {
            var affiliateId = _affiliateService.InsertAffiliate(new Affiliate
            {
                Name = registerModel.Name,
                CountryId = registerModel.CountryId,
                StateProvinceId = registerModel.StateProvinceId,
                Email = registerModel.CompanyEmail,
                AddressLine1 = registerModel.AddressLine1,
                AddressLine2 = registerModel.AddressLine2,
                City = registerModel.City,
                ZipPostalCode = registerModel.ZipPostalCode,
                Phone = registerModel.CompanyPhone,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                Status = (short)AffiliateActivityStatuses.Applied,
                RegistrationIp = Request.GetClientIp(),
                ManagerId = registerModel.ManagerId,
                WhiteIp = registerModel.WhiteIp
            });

            user.ParentId = affiliateId;
        }

        private string GetTokenInRequestHeader()
        {
            if (Request == null || !Request.Headers.Contains("Authorization"))
                throw new Exception("Access token is null");

            var token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            token = token.Replace("Bearer ", "").Replace("bearer ", "");
            return token;
        }

        private string ValidateDirectory(long countryId, long? stateProvinceId)
        {
            var validationMessage = "";

            var countries = _countryService.GetAllCountries();

            if (!countries.Select(x => x.Id).Contains(countryId))
            {
                validationMessage = "no country was found for given id";
            }

            var stateProvinces = _stateProvinceService.GetStateProvinceByCountryId(countryId);

            if (stateProvinceId != null && !stateProvinces.Select(x => x.Id).Contains(stateProvinceId.Value))
            {
                validationMessage = "no state province was found for given id";
            }

            return validationMessage;
        }
        #endregion
    }
}
