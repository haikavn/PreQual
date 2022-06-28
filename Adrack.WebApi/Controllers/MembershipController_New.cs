using System;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Service.Helpers;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Infrastructure.Constants;
using Adrack.WebApi.Infrastructure.Core.WrapperData;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.New.Membership;
using Adrack.Service.Configuration;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/new_authentication")]
    public class MembershipController_New : BaseApiController
    {
        private readonly UserSetting _userSetting;
        private readonly EmailSubscriptionSetting _emailSubscriptionSetting;

        private readonly IUserService _userService;
        private readonly IRegistrationRequestService _registrationRequestService;
        private readonly IEmailService _emailService;
        private readonly IAppContext _appContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISharedDataWrapper _sharedDataWrapper;
        private readonly IEmailSubscriptionService _emailSubscriptionService;
        private readonly IRoleService _roleService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IGlobalAttributeService _globalAttributeService;
        private readonly IProfileService _profileService;
        private readonly IBuyerService _buyerService;
        private readonly IAffiliateService _affiliateService;
        private readonly IAccountingService _accountingService;
        private readonly ILocalizedStringService _localizedStringService;
        private readonly ISettingService _settingService;
        private readonly EmailOperatorEnums _emailProvider;


        public MembershipController_New(
            UserSetting userSetting,
            EmailSubscriptionSetting emailSubscriptionSetting,
            IUserService userService,
            IRegistrationRequestService registrationRequestService,
            IEmailService emailService,
            IAppContext appContext,
            IAuthenticationService authenticationService,
            ISharedDataWrapper sharedDataWrapper,
            IEmailSubscriptionService emailSubscriptionService,
            IRoleService roleService,
            IUserRegistrationService userRegistrationService,
            IGlobalAttributeService globalAttributeService,
            IProfileService profileService,
            IBuyerService buyerService,
            IAffiliateService affiliateService,
            IAccountingService accountingService, 
            ILocalizedStringService localizedStringService,
            ISettingService settingService)
        {
            _userSetting = userSetting;
            _emailSubscriptionSetting = emailSubscriptionSetting;
            _userService = userService;
            _registrationRequestService = registrationRequestService;
            _emailService = emailService;
            _appContext = appContext;
            _authenticationService = authenticationService;
            _sharedDataWrapper = sharedDataWrapper;
            _emailSubscriptionService = emailSubscriptionService;
            _roleService = roleService;
            _userRegistrationService = userRegistrationService;
            _globalAttributeService = globalAttributeService;
            _profileService = profileService;
            _buyerService = buyerService;
            _affiliateService = affiliateService;
            _accountingService = accountingService;
            _localizedStringService = localizedStringService;
            _settingService = settingService;
            _emailProvider = EmailOperatorEnums.SendGrid;
        }


        [HttpPost]
        [Route("registrationRequest")]
        public IHttpActionResult CreateRegistrationRequest(RegistrationRequestCreateModel model)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var user = _userService.GetUserByEmail(model.Email);
                if (user != null)
                {
                    return HttpBadRequest("Specified email already exists");
                }

                _registrationRequestService.DeleteRegistrationRequest(model.Email);

                var registrationRequest = model.GetRegistrationRequest();

                _registrationRequestService.InsertRegistrationRequest(registrationRequest);

                _emailService.SendUserWelcomeMessageWithValidationCode(model.Email,
                    model.Name,
                    registrationRequest.Code,
                    _appContext.AppLanguage.Id,
                    _emailProvider);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(RegisterModel registerModel)
        {
            try
            {

                if (_userSetting.UserRegistrationType == UserRegistrationType.Disabled)
                {
                    ModelState.AddModelError("RegisterModel", "User registration type is disabled");
                    return HttpBadRequest(ModelState.GetErrorMessage());
                }

                if (!ModelState.IsValid)
                    return HttpBadRequest(ModelState.GetErrorMessage());

                if (_appContext.AppUser != null)
                {
                    _authenticationService.SignOut(_appContext);
                }

                if (_userSetting.UsernameEnabled && registerModel.Username != null)
                    registerModel.Username = registerModel.Username.Trim();

                var isApproved = _userSetting.UserRegistrationType == UserRegistrationType.Standard;

                var user = new User();

                RegisterUser(registerModel, user, isApproved);

                SubscribeEmail(registerModel);
                return Ok(registerModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("emailValidation")]
        public IHttpActionResult ValidateEmail(RegisterValidationModel registerValidation)
        {
            if (registerValidation == null)
            {
                ModelState.AddModelError("RegisterValidationModel", "Invalid parameters");
                return HttpBadRequest(ModelState.GetErrorMessage());
            }

            try
            {
                bool result;
                if (!registerValidation.Resend)
                {
                    var registrationRequest = _registrationRequestService.GetRegistrationRequest(registerValidation.Email, registerValidation.ValidationCode);
                    result = registrationRequest != null;

                    if (registrationRequest != null)
                        _registrationRequestService.DeleteRegistrationRequest(registrationRequest);
                }
                else
                {
                    _registrationRequestService.DeleteRegistrationRequest(registerValidation.Email);
                    var registrationRequest = registerValidation.GetRegistrationRequest();
                    _registrationRequestService.InsertRegistrationRequest(registrationRequest);
                    _emailService.SendUserWelcomeMessageWithValidationCode(registrationRequest.Email, registrationRequest.Name, registrationRequest.Code, _appContext.AppLanguage.Id, _emailProvider);
                    result = true;
                }
                return Ok(result);

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpGet]
        [Route("usernameValidation")]
        public IHttpActionResult CheckUsernameAvailability(string username)
        {
            try
            {
                var validationModel = new ValidationModel
                {
                    Result = false,
                    Message = _localizedStringService.GetLocalizedString("Membership.CheckUsernameAvailability.NotAvailable")
                };

                if (!_userSetting.UsernameEnabled || string.IsNullOrWhiteSpace(username))
                    return Ok(validationModel);

                if (_appContext.AppUser != null && _appContext.AppUser.Username != null && _appContext.AppUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    validationModel.Message = _localizedStringService.GetLocalizedString("Membership.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var user = _userService.GetUserByUsername(username);
                    if (user == null)
                    {
                        validationModel.Message = _localizedStringService.GetLocalizedString("Membership.CheckUsernameAvailability.Available");
                        validationModel.Result = true;
                    }
                }
                return Ok(validationModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        #region Private Methods


        private void SubscribeEmail(RegisterModel registerModel)
        {
            if (_emailSubscriptionSetting.EmailSubscriptionEnabled)
            {
                var emailSubscription = _emailSubscriptionService.GetEmailSubscriptionByEmail(registerModel.Email);
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
                        _emailSubscriptionService.InsertEmailSubscription(registerModel.Email.GetEmailSubscription());
                }
            }
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
            _profileService.InsertProfile(registerModel.GetProfile(user.Id));

            if (registerModel.ParentId == 0)
            {
                if (registerModel.UserRoleId != _roleService.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey).Id &&
                    registerModel.UserType == _sharedDataWrapper.GetBuyerUserTypeId())
                {
                    var buyerId = _buyerService.InsertBuyer(registerModel.GetBuyer());
                    user.ParentId = buyerId;
                    _accountingService.InsertBuyerBalance(registerModel.GetBuyerBalance(buyerId));
                }
                else if (registerModel.UserRoleId != _roleService.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey).Id &&
                        registerModel.UserType == _sharedDataWrapper.GetAffiliateUserTypeId())
                {
                    var affiliateId = _affiliateService.InsertAffiliate(registerModel.GetAffiliate(user.Id, Request.GetClientIp()));
                    user.ParentId = affiliateId;
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
            _globalAttributeService.SaveGlobalAttribute(user, GlobalAttributeBuiltIn.MembershipActivationToken, user.GuId);
            _userService.UpdateUser(user);

            _emailService.SendUserWelcomeMessageWithUsernamePassword(user, _appContext.AppLanguage.Id, _emailProvider, registerModel.Email, registerModel.Password);
        }

        #endregion

    }
}