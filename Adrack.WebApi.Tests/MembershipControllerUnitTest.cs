using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Core.Domain.Security;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security.Captcha;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Constants;
using Adrack.WebApi.Models.Membership.Password;
using Adrack.WebApi.Models.Membership.Register;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Web.Http.Results;
using Adrack.Core.Domain.Directory;
using Adrack.WebApi.Infrastructure.Core.WrapperData;
using Adrack.WebApi.Models.New.Membership;
using System.Threading;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models;
using Adrack.Service.Configuration;
using Adrack.Core.Cache;

namespace Adrack.WebApi.Tests
{
    public class MembershipControllerUnitTest
    {
        #region properties
        private readonly Mock<IAppContext> _mockAppContext;
        private readonly Mock<ProfileSetting> _mockProfileSetting;
        private readonly Mock<UserSetting> _mockUserSetting;
        private readonly Mock<AddressSetting> _mockAddressSetting;
        private readonly Mock<EmailSubscriptionSetting> _mockEmailSubscriptionSetting;
        private readonly Mock<CaptchaSetting> _mockCaptchaSetting;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly Mock<ILocalizedStringService> _mockLocalizedStringService;
        private readonly Mock<IUserRegistrationService> _mockUserRegistrationService;
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<IDepartmentService> _mockDepartmentService;
        private readonly Mock<ICountryService> _mockCountryService;
        private readonly Mock<IStateProvinceService> _mockStateProvinceService;
        private readonly Mock<IEmailSubscriptionService> _mockEmailSubscriptionService;
        private readonly Mock<IProfileService> _mockProfileService;
        private readonly Mock<IAffiliateService> _mockAffiliateService;
        private readonly Mock<IBuyerService> _mockBuyerService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IGlobalAttributeService> _mockGlobalAttributeService;
        private readonly Mock<IRegistrationRequestService> _mockRegistrationRequestService;
        private readonly Mock<IHistoryService> _mockHistoryService;
        private readonly Mock<IAccountingService> _mockAccountingService;
        private readonly Mock<IDateTimeHelper> _mockDateTimeHelper;
        private readonly Mock<IJWTTokenService> _mockJwtTokenHelper;
        private readonly Mock<ISharedDataWrapper> _mockSharedDataWrapper;
        private readonly Mock<IRolePermissionService> _mockRolePermissionService;
        private readonly Mock<IPermissionService> _mockPermissionService;
        private readonly Mock<IUsersExtensionService> _mockUserExtensionService;
        private readonly Mock<IAddonService> _mockAddonService;
        private readonly Mock<ISettingService> _mockSettingService;
        private readonly Mock<ICacheManager> _mockCacheManager;

        private readonly Mock<IPlanService> _mockPlanService;
        private readonly Mock<IPaymentService> _mockPaymentService;


        #endregion

        #region constructors
        public MembershipControllerUnitTest()
        {
            _mockAppContext = new Mock<IAppContext>();
            _mockProfileSetting = new Mock<ProfileSetting>();
            _mockUserSetting = new Mock<UserSetting>();
            _mockAddressSetting = new Mock<AddressSetting>();
            _mockEmailSubscriptionSetting = new Mock<EmailSubscriptionSetting>();
            _mockCaptchaSetting = new Mock<CaptchaSetting>();
            _mockUserService = new Mock<IUserService>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockEncryptionService = new Mock<IEncryptionService>();
            _mockLocalizedStringService = new Mock<ILocalizedStringService>();
            _mockUserRegistrationService = new Mock<IUserRegistrationService>();
            _mockRoleService = new Mock<IRoleService>();
            _mockDepartmentService = new Mock<IDepartmentService>();
            _mockCountryService = new Mock<ICountryService>();
            _mockStateProvinceService = new Mock<IStateProvinceService>();
            _mockEmailSubscriptionService = new Mock<IEmailSubscriptionService>();
            _mockProfileService = new Mock<IProfileService>();
            _mockAffiliateService = new Mock<IAffiliateService>();
            _mockBuyerService = new Mock<IBuyerService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockGlobalAttributeService = new Mock<IGlobalAttributeService>();
            _mockRegistrationRequestService = new Mock<IRegistrationRequestService>();
            _mockHistoryService = new Mock<IHistoryService>();
            _mockAccountingService = new Mock<IAccountingService>();
            _mockDateTimeHelper = new Mock<IDateTimeHelper>();
            _mockJwtTokenHelper = new Mock<IJWTTokenService>();
            _mockSharedDataWrapper = new Mock<ISharedDataWrapper>();
            _mockRolePermissionService = new Mock<IRolePermissionService>();
            _mockPermissionService = new Mock<IPermissionService>();
            _mockUserExtensionService = new Mock<IUsersExtensionService>();
            _mockAddonService = new Mock<IAddonService>();
            _mockSettingService = new Mock<ISettingService>();
            _mockCacheManager = new Mock<ICacheManager>();
            _mockPlanService = new Mock<IPlanService>();
            _mockPaymentService = new Mock<IPaymentService>();
        }

        #endregion

        #region test methods

        #region login

        [Fact]
        public void Login_Success_LoginModel()
        {
            // Arrange
            var loginModel = new Models.Membership.Login.SignInModel()
            {
                UserName = "temp@gmail.com",
                Password = "abc123",
            };
            IHttpActionResult result = null;

            _mockAppContext.Setup(x => x.AppUser).
                Returns(new User());

            _mockUserRegistrationService.Setup(x =>
                    x.ValidateUser(loginModel.UserName, loginModel.Password))
                .Returns(UserLoginResult.Successful);

            _mockUserService.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(new User
            {
                Id = 25,
                Username = loginModel.UserName
            });
            _mockUserService.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(new User
            {
                Id = 25,
                Username = loginModel.UserName
            });

            _mockJwtTokenHelper.Setup(x =>
                x.GenerateAccessToken(It.IsAny<long>(), It.IsAny<string>())).Returns(new Guid().ToString());

            var controller = new MembershipController(_mockUserSetting.Object,
                                                      _mockUserService.Object,
                                                      _mockAuthenticationService.Object,
                                                      _mockAppContext.Object,
                                                      _mockEncryptionService.Object,
                                                      _mockLocalizedStringService.Object,
                                                      _mockUserRegistrationService.Object,
                                                      _mockRoleService.Object,
                                                      _mockCountryService.Object,
                                                      _mockStateProvinceService.Object,
                                                      _mockProfileService.Object,
                                                      _mockAffiliateService.Object,
                                                      _mockBuyerService.Object,
                                                      _mockEmailService.Object,
                                                      _mockGlobalAttributeService.Object,
                                                      _mockRegistrationRequestService.Object,
                                                      _mockAccountingService.Object,
                                                      _mockJwtTokenHelper.Object,
                                                      _mockSharedDataWrapper.Object,
                                                      _mockRolePermissionService.Object,
                                                      _mockPermissionService.Object,
                                                      _mockUserExtensionService.Object,
                                                      _mockAddonService.Object,
                                                      _mockSettingService.Object,
                                                      _mockCacheManager.Object,
                                                      _mockPaymentService.Object,
                                                      _mockPlanService.Object,
                                                      _mockEmailSubscriptionService.Object
                                                      );

            // Act
            //result = controller.Login(loginModel);

            // Assert
            //Assert.NotNull(result);
        }

        [Fact]
        public void Login_WithWrongData_ReturnsBadRequest()
        {
            // Arrange
            IHttpActionResult result = null;

            var loginModel = new Models.Membership.Login.SignInModel()
            {
                UserName = "temp@gmail.com",
                Password = "abc123",
            };

            _mockAppContext.Setup(x => x.AppUser).
                Returns(new User());

            _mockUserRegistrationService.Setup(x =>
                    x.ValidateUser(loginModel.UserName, loginModel.Password))
                .Returns(UserLoginResult.UserNotExist);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            //var response = controller.Login(loginModel);
            //var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            //var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            //Assert.Equal(((ErrorViewModel)errorViewModel).Status, (int)HttpStatusCode.BadRequest);
        }

        #endregion

        #region register

        #region register affiliate  

        [Fact]
        public void RegisterAffiliate_Success_RegisterModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };
            var code = Guid.NewGuid().ToString();

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockUserService.Setup(x => x.GetAllUsers(-1, 0, 1)).
                            Returns(new Pagination<User>(new List<User> { new User { Id = 1 } }, 0, 1));
            _mockRegistrationRequestService.Setup(x =>
                x.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()));
            _mockEmailService.Setup(x =>
                            x.SendUserWelcomeMessageWithValidationCode(registrationModel.Email,
                                registrationModel.FirstName, code, 1, EmailOperatorEnums.LeadNative));
            _mockRegistrationRequestService.Setup(x =>
                             x.GetRegistrationRequest(registrationModel.Email, code)).Returns(new RegistrationRequest());
            _mockRegistrationRequestService.Setup(x => x.DeleteRegistrationRequest(It.IsAny<RegistrationRequest>()));
            _mockUserSetting.Object.UserRegistrationType = UserRegistrationType.Standard;
            _mockUserRegistrationService.Setup(x =>
                             x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Returns(new UserRegistrationResult());
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockAffiliateService.Setup(x => x.InsertAffiliate(It.IsAny<Affiliate>())).Returns(100);
            _mockBuyerService.Setup(x => x.InsertBuyer(It.IsAny<Buyer>())).Returns(100);
            _mockRoleService.Setup(x => x.GetRoleById(5)).Returns(new Role());
            _mockRoleService.Setup(x => x.GetRoleByKey(It.IsAny<string>())).Returns(new Role { Id = 1 });
            _mockUserService.Setup(x => x.UpdateUser(new User { Id = 1 }));
            _mockEmailService.Setup(x => x.SendUserRegisteredMessage(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative));
            _mockEmailSubscriptionSetting.Object.EmailSubscriptionEnabled = true;
            _mockHistoryService.Setup(x => x.AddHistory(It.IsAny<History>()));
            _mockSharedDataWrapper.Setup(x => x.GetAffiliateUserTypeId()).Returns(UserTypes.Affiliate);
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<long>())).Returns(new User());

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            { Request = new HttpRequestMessage() };

            // Act
            controller.RegisterAffiliate(registrationModel);
            var user = controller.GetUserInfo();
            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public void RegisterAffiliate_EmailAlreadyExist_RegistrationRequestModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email)).Returns(new User());

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act

            var requestResult = controller.RegisterAffiliate(registrationModel);
            var message = ((NegotiatedContentResult<ErrorViewModel>)requestResult)?.Content?.Message;
            // Assert
            Assert.Equal("specified email already exist", message);
        }

        [Fact]
        public void RegisterAffiliate_RegistrationRequestSuccess_RegistrationRequestModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockRegistrationRequestService.Setup(x => x.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(new RegistrationRequest());
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(It.IsAny<string>())).Returns(new Role());
            _mockUserService.Setup(x => x.AddUserRole(It.IsAny<long>(), It.IsAny<long>()));

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var requestResult = controller.RegisterAffiliate(registrationModel);

            // Assert
            var result = (OkNegotiatedContentResult<User>)requestResult;
            Assert.NotNull(result?.Content);
        }

        [Fact]
        public void RegisterAffiliate_RegisterThrowException()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockRegistrationRequestService.Setup(x => x.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(new RegistrationRequest());
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Throws<Exception>();

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            //Act
            var httpActionResult = controller.RegisterAffiliate(registrationModel);
            var httpResponse = ((NegotiatedContentResult<ErrorViewModel>)httpActionResult)?.Content?.Status;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, httpResponse);
        }
        #endregion

        #region register user  

        [Fact]
        public void RegisterNetworkUser_Success_RegisterModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };
            var code = Guid.NewGuid().ToString();

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockUserService.Setup(x => x.GetAllUsers(-1, 0, 1)).
                            Returns(new Pagination<User>(new List<User> { new User { Id = 1 } }, 0, 1));
            _mockRegistrationRequestService.Setup(x =>
                x.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()));
            _mockEmailService.Setup(x =>
                            x.SendUserWelcomeMessageWithValidationCode(registrationModel.Email,
                                registrationModel.FirstName, code, 1, EmailOperatorEnums.LeadNative));
            _mockRegistrationRequestService.Setup(x =>
                             x.GetRegistrationRequest(registrationModel.Email, code)).Returns(new RegistrationRequest());
            _mockRegistrationRequestService.Setup(x => x.DeleteRegistrationRequest(It.IsAny<RegistrationRequest>()));
            _mockUserSetting.Object.UserRegistrationType = UserRegistrationType.Standard;
            _mockUserRegistrationService.Setup(x =>
                             x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Returns(new UserRegistrationResult());
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockAffiliateService.Setup(x => x.InsertAffiliate(It.IsAny<Affiliate>())).Returns(100);
            _mockBuyerService.Setup(x => x.InsertBuyer(It.IsAny<Buyer>())).Returns(100);
            _mockRoleService.Setup(x => x.GetRoleById(5)).Returns(new Role());
            _mockRoleService.Setup(x => x.GetRoleByKey(It.IsAny<string>())).Returns(new Role { Id = 1 });
            _mockUserService.Setup(x => x.UpdateUser(new User { Id = 1 }));
            _mockEmailService.Setup(x => x.SendUserRegisteredMessage(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative));
            _mockEmailSubscriptionSetting.Object.EmailSubscriptionEnabled = true;
            _mockHistoryService.Setup(x => x.AddHistory(It.IsAny<History>()));
            _mockSharedDataWrapper.Setup(x => x.GetAffiliateUserTypeId()).Returns(UserTypes.Affiliate);
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<long>())).Returns(new User());

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            { Request = new HttpRequestMessage() };

            // Act
            controller.RegisterNetworkUser(registrationModel);
            var user = controller.GetUserInfo();
            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public void RegisterNetworkUser_EmailAlreadyExist_RegistrationRequestModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email)).Returns(new User());
            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act

            var requestResult = controller.RegisterNetworkUser(registrationModel);
            var message = ((NegotiatedContentResult<ErrorViewModel>)requestResult)?.Content?.Message;
            // Assert
            Assert.Equal("specified email already exist", message);
        }

        [Fact]
        public void RegisterAffiliate_NetworkUserRequestSuccess_RegistrationRequestModel()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockRegistrationRequestService.Setup(x => x.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(new RegistrationRequest());
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(It.IsAny<string>())).Returns(new Role());
            _mockUserService.Setup(x => x.AddUserRole(It.IsAny<long>(), It.IsAny<long>()));
            
            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var requestResult = controller.RegisterNetworkUser(registrationModel);

            // Assert
            var result = (OkNegotiatedContentResult<User>)requestResult;
            Assert.NotNull(result?.Content);
        }

        [Fact]
        public void RegisterNetworkUser_RegisterThrowException()
        {
            // Arrange
            var registrationModel = new RegistrationModel
            {
                FirstName = "test",
                LastName = "test",
                MiddleNameName = "test",
                Email = "test@adrack.com",
                JobTitle = "test",
                Password = "P@ssw0rd",
                RepeatPassword = "P@ssw0rd",
                CompanyName = "test",
                CompanyWebSite = "test",
                VerticalId = 1,
                CountryId = 80,
                StateProvinceId = null,
                City = "test",
                Address = "test",
                SecondaryAddress = "test",
                ZipCode = "test"
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockUserService.Setup(x => x.GetUserByEmail(registrationModel.Email));
            _mockRegistrationRequestService.Setup(x => x.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(new RegistrationRequest());
            _mockCountryService.Setup(x => x.GetAllCountries()).Returns(new List<Country> { new Country { Id = 80 } });
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true)).Throws<Exception>();
            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            //Act
            var httpActionResult = controller.RegisterNetworkUser(registrationModel);
            var httpResponse = ((NegotiatedContentResult<ErrorViewModel>)httpActionResult)?.Content?.Status;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, httpResponse);
        }
        #endregion

        #region register affiliate by admin

        [Fact]
        public void RegisterAffiliateByAdmin_Success_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Affiliate,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 5,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null,
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                           .Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );
            // Act
            var result = controller.RegisterAffiliateByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotNull(result);
            Assert.Empty(modelState.Values);
        }

        [Fact]
        public void RegisterAffiliateByAdmin_NotSuccess_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Affiliate,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 5,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x =>
                    x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                .Returns(new UserRegistrationResult() { Errors = new List<string> { "Internal server error" } });
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterAffiliateByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotEmpty(modelState.Values);
        }
        #endregion

        #region register buyer by admin
        [Fact]
        public void RegisterBuyerByAdmin_Success_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Buyer,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 6,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                           .Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterBuyerByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotNull(result);
            Assert.Empty(modelState.Values);
        }

        [Fact]
        public void RegisterBuyerByAdmin_NotSuccess_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Buyer,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 6,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x =>
                    x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                .Returns(new UserRegistrationResult() { Errors = new List<string> { "Internal server error" } });
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterBuyerByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotEmpty(modelState.Values);
        }

        [Fact]
        public void RegisterBuyerByAdmin_BuyerAlreadyExist_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Buyer,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 6,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x =>
                    x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                .Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockBuyerService.Setup(x => x.GetBuyerByName(It.IsAny<string>(), It.IsAny<long>())).
                Returns(new Buyer());
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterBuyerByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.Equal("Buyer with the specified name already exist", modelState["RegisterModel"].Errors[0].ErrorMessage);
        }
        #endregion

        #region register user by admin

        [Fact]
        public void RegisterUserByAdmin_UserIsNullSuccess_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Network,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 3,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), false))
                           .Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<long>())).Returns(new User());
            _mockProfileService.Setup(x => x.GetProfileByUserId(It.IsAny<long>())).Returns(new Profile());
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterUserByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotNull(result);
            Assert.Empty(modelState.Values);
        }

        [Fact]
        public void RegisterUserByAdmin_UserNotNullSuccess_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Network,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 3,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x => x.RegisterUser(It.IsAny<UserRegistrationRequest>(), true))
                           .Returns(new UserRegistrationResult());
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<long>())).Returns(new User());
            _mockProfileService.Setup(x => x.GetProfileByUserId(It.IsAny<long>())).Returns(new Profile());
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );

            // Act
            var result = controller.RegisterUserByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotNull(result);
            Assert.Empty(modelState.Values);
        }

        [Fact]
        public void RegisterUserByAdmin_NotSuccess_RegisterModel()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Id = 0,
                ProfileId = 0,
                FirstNameEnabled = false,
                FirstNameRequired = false,
                FirstName = "test",
                MiddleNameEnabled = false,
                MiddleNameRequired = false,
                MiddleName = null,
                LastNameEnabled = false,
                LastNameRequired = false,
                LastName = "test",
                SummaryEnabled = false,
                SummaryRequired = false,
                Summary = null,
                CountryRequired = false,
                CountryEnabled = false,
                CountryId = 80,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                StateProvinceId = 6,
                AddressLine1Enabled = false,
                AddressLine1Required = false,
                AddressLine1 = null,
                AddressLine2Enabled = false,
                AddressLine2Required = false,
                AddressLine2 = null,
                CityEnabled = false,
                CityRequired = false,
                City = null,
                ZipPostalCodeEnabled = false,
                ZipPostalCodeRequired = false,
                ZipPostalCode = null,
                TelephoneEnabled = false,
                TelephoneRequired = false,
                Phone = null,
                CompanyPhone = null,
                CompanyEmail = null,
                UsernameEnabled = false,
                Username = "test",
                CheckUsernameAvailabilityEnabled = false,
                Email = "test@adrack.com",
                IsMaskEmail = false,
                ContactEmail = null,
                UserType = UserTypes.Network,
                Password = "qwerty147852369",
                ConfirmPassword = "qwerty147852369",
                EmailSubscriptionEnabled = false,
                EmailSubscription = false,
                SecurityQuestion = null,
                SecurityAnswer = null,
                Name = null,
                UserRoleId = 3,
                ParentId = 0,
                Comment = null,
                CellPhone = null,
                LoggedInUser = null,
                Website = null,
                ValidateFromCode = false,
                ValidationEmail = null,
                TimeZone = null,
                ManagerId = null,
                WhiteIp = null,
                IsActive = false,
                IsLockedOut = false,
                IsChangePassOnLogin = false,
                Credit = 0,
                AlwaysSoldOption = 0,
                MaxDuplicateDays = 0,
                DailyCap = 0,
                Description = null,
                DoNotPresentResultField = null,
                DoNotPresentResultValue = null,
                DoNotPresentStatus = null,
                DoNotPresentPostMethod = null,
                DoNotPresentRequest = null,
                DoNotPresentUrl = null,
                CanSendLeadId = null,
                AccountId = null
            };

            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockEmailSubscriptionService.Setup(x =>
                             x.GetEmailSubscriptionByEmail(It.IsAny<string>())).Returns(new EmailSubscription());
            _mockEmailSubscriptionService.Setup(x =>
                             x.UpdateEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockEmailSubscriptionService.Setup(x =>
                            x.InsertEmailSubscription(It.IsAny<EmailSubscription>(), true));
            _mockProfileService.Setup(x => x.InsertProfile(It.IsAny<Profile>()));
            _mockEmailService.Setup(x =>
                           x.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), 1, EmailOperatorEnums.LeadNative, "", ""));
            _mockUserRegistrationService.Setup(x =>
                    x.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
               .Returns(new UserRegistrationResult() { Errors = new List<string> { "Internal server error" } });
            _mockRoleService.Setup(x => x.GetRoleByKey(UserRoleKeys.GlobalAdministratorsKey))
                .Returns(new Role() { Id = 3 });
            _mockUserService.Setup(x => x.GetUserById(It.IsAny<long>()));
            _mockProfileService.Setup(x => x.GetProfileByUserId(It.IsAny<long>())).Returns(new Profile());
            _mockSharedDataWrapper.Setup(x => x.GetBuiltInUserTypeId()).Returns(UserTypes.Super);

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            );
            // Act
            var result = controller.RegisterUserByAdmin(registerModel);

            // Assert
            var modelState = controller.ModelState;
            Assert.NotEmpty(modelState.Values);
        }

        #endregion

        #endregion

        #region password

        #region forgot password 

        [Fact]
        public void ForgotPassword_ExistingEmail_ReturnsOkResultWithMessage()
        {
            // Arrange
            var forgotPasswordModel = new ForgotPasswordModel { Email = "test@adrack.com" };

            _mockUserService.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(new User
            {
                Active = true,
                LockedOut = false,
                Deleted = false
            });
            _mockAppContext.Setup(x => x.AppLanguage).Returns(new Language() { Id = 1 });
            _mockGlobalAttributeService.Setup(x =>
                x.SaveGlobalAttribute(It.IsAny<User>(), GlobalAttributeBuiltIn.ForgotPasswordToken, new Guid()));
            _mockGlobalAttributeService.Setup(x =>
                x.SaveGlobalAttribute(It.IsAny<User>(), GlobalAttributeBuiltIn.ForgotPasswordTokenRequestedDate,
                DateTime.Now));
            _mockEmailService.Setup(x => x.SendUserForgotPasswordMessage(It.IsAny<User>(), 1,EmailOperatorEnums.LeadNative));
            _mockLocalizedStringService.Setup(x => x.GetLocalizedString("Membership.ForgotPassword.EmailSent"))
                                                     .Returns("Email with instructions has been sent to you");

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ForgotPassword(forgotPasswordModel);
            var contentResult = response as OkNegotiatedContentResult<string>;

            // Assert
            Assert.Equal("Email with instructions has been sent to you", contentResult?.Content);
        }

        [Fact]
        public void ForgotPassword_NonExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var forgotPasswordModel = new ForgotPasswordModel { Email = "test@adrack.com" };

            _mockUserService.Setup(x => x.GetUserByEmail(It.IsAny<string>()));
            _mockLocalizedStringService.Setup(x => x.GetLocalizedString("Membership.ForgotPassword.EmailNotFound"))
                                                     .Returns("Email not found");

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ForgotPassword(forgotPasswordModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal("Email not found", ((ErrorViewModel)errorViewModel)?.Message);
        }

        #endregion

        #region change password

        [Fact]
        public void ChangePassword_CorrectData_ReturnsOkResultWithMessage()
        {
            // Arrange
            var changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "qwerty147852369",
                Password = "qwerty123",
                ConfirmPassword = "qwerty123"
            };
            _mockUserService.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(new User
            {
                Active = true,
                LockedOut = false,
                Deleted = false
            });
            _mockEncryptionService.Setup(x => x.CreateSaltKey(20)).Returns(It.IsAny<string>());
            _mockEncryptionService.Setup(x =>
                    x.CreatePasswordHash(changePasswordModel.Password, It.IsAny<string>(), "SHA1"))
                .Returns(It.IsAny<string>());
            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<User>()));
            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockLocalizedStringService.Setup(x => x.GetLocalizedString(It.IsAny<string>()))
                                                                .Returns("Password was changed");

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ChangePassword(changePasswordModel);
            var contentResult = response as OkNegotiatedContentResult<string>;

            // Assert
            Assert.Equal("Password was changed", contentResult?.Content);
        }

        [Fact]
        public void ChangePassword_NonExistingUser_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "qwerty147852369",
                Password = "qwerty123",
                ConfirmPassword = "qwerty123"
            };
            _mockUserService.Setup(x => x.GetUserByUsername(It.IsAny<string>()));
            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<User>()));
            _mockAppContext.Setup(x => x.AppUser).Returns(new User());

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ChangePassword(changePasswordModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal("User not found", ((ErrorViewModel)errorViewModel)?.Message);
        }

        [Fact]
        public void ChangePassword_IncorrectPassword_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "qwerty147852369",
                Password = "qwerty123",
                ConfirmPassword = "qwerty123"
            };
            _mockUserService.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(new User
            {
                Active = true,
                LockedOut = false,
                Deleted = false,
                Password = "fdgfg"
            });
            _mockEncryptionService.Setup(x =>
                            x.CreatePasswordHash(changePasswordModel.Password, It.IsAny<string>(), "SHA1"));
            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<User>()));
            _mockAppContext.Setup(x => x.AppUser).Returns(new User());

            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ChangePassword(changePasswordModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal("Password is incorrect", ((ErrorViewModel)errorViewModel)?.Message);
        }

        [Fact]
        public void ChangePassword_IncorrectConfirmPassword_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "qwerty147852369",
                Password = "qwerty123",
                ConfirmPassword = "qwerty1231"
            };
            _mockUserService.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(new User
            {
                Active = true,
                LockedOut = false,
                Deleted = false
            });
            _mockEncryptionService.Setup(x =>
                            x.CreatePasswordHash(changePasswordModel.Password, It.IsAny<string>(), "SHA1"));
            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<User>()));
            _mockAppContext.Setup(x => x.AppUser).Returns(new User());
            _mockLocalizedStringService.Setup(x => x.GetLocalizedString(It.IsAny<string>()))
                                            .Returns("The password and confirmation password do not match");
            var controller = new MembershipController(_mockUserSetting.Object,
                _mockUserService.Object,
                _mockAuthenticationService.Object,
                _mockAppContext.Object,
                _mockEncryptionService.Object,
                _mockLocalizedStringService.Object,
                _mockUserRegistrationService.Object,
                _mockRoleService.Object,
                _mockCountryService.Object,
                _mockStateProvinceService.Object,
                _mockProfileService.Object,
                _mockAffiliateService.Object,
                _mockBuyerService.Object,
                _mockEmailService.Object,
                _mockGlobalAttributeService.Object,
                _mockRegistrationRequestService.Object,
                _mockAccountingService.Object,
                _mockJwtTokenHelper.Object,
                _mockSharedDataWrapper.Object,
                _mockRolePermissionService.Object,
                _mockPermissionService.Object,
                _mockUserExtensionService.Object,
                _mockAddonService.Object,
                _mockSettingService.Object,
                _mockCacheManager.Object,
                _mockPaymentService.Object,
                _mockPlanService?.Object,
                _mockEmailSubscriptionService?.Object
            )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.ChangePassword(changePasswordModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal("The password and confirmation password do not match", ((ErrorViewModel)errorViewModel)?.Message);
        }

        #endregion

        #endregion

        #endregion
    }
}
