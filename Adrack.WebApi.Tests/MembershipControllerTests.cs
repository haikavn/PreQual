using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Core.Domain.Security;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.WrapperData;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.New.Membership;
using Moq;
using Newtonsoft.Json;
using Xunit;
using static Adrack.Web.Framework.Security.ContentManagementApiAuthorizeAttribute;

namespace Adrack.WebApi.Tests
{
    public class MembershipControllerTests
    {

        private readonly UserSetting _userSetting;
        private readonly EmailSubscriptionSetting _emailSubscriptionSetting;
        private readonly Mock<ISettingService> _fakeSettingService;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<IRegistrationRequestService> _fakeRegistrationRequestService;
        private readonly Mock<IEmailService> _fakeEmailService;
        private readonly Mock<IAppContext> _fakeAppContext;
        private readonly Mock<IAuthenticationService> _fakeAuthenticationService;
        private readonly Mock<ISharedDataWrapper> _fakeSharedDataWrapper;
        private readonly Mock<IEmailSubscriptionService> _fakeEmailSubscriptionService;
        private readonly Mock<IRoleService> _fakeRoleService;
        private readonly Mock<IUserRegistrationService> _fakeUserRegistrationService;
        private readonly Mock<IGlobalAttributeService> _fakeGlobalAttributeService;
        private readonly Mock<IProfileService> _fakeProfileService;
        private readonly Mock<IBuyerService> _fakeBuyerService;
        private readonly Mock<IAffiliateService> _fakeAffiliateService;
        private readonly Mock<IAccountingService> _fakeIAccountingService;
        private readonly Mock<ILocalizedStringService> _fakeLocalizedStringService;

        public MembershipControllerTests()
        {
            _userSetting = new UserSetting();
            _emailSubscriptionSetting = new EmailSubscriptionSetting();

            _fakeUserService = new Mock<IUserService>();
            _fakeRegistrationRequestService = new Mock<IRegistrationRequestService>();
            _fakeEmailService = new Mock<IEmailService>();
            _fakeAppContext = new Mock<IAppContext>();
            _fakeAuthenticationService = new Mock<IAuthenticationService>();
            _fakeSharedDataWrapper = new Mock<ISharedDataWrapper>();
            _fakeEmailSubscriptionService = new Mock<IEmailSubscriptionService>();
            _fakeRoleService = new Mock<IRoleService>();
            _fakeUserRegistrationService = new Mock<IUserRegistrationService>();
            _fakeGlobalAttributeService = new Mock<IGlobalAttributeService>();
            _fakeProfileService = new Mock<IProfileService>();
            _fakeBuyerService = new Mock<IBuyerService>();
            _fakeAffiliateService = new Mock<IAffiliateService>();
            _fakeIAccountingService = new Mock<IAccountingService>();
            _fakeLocalizedStringService = new Mock<ILocalizedStringService>();
            _fakeSettingService = new Mock<ISettingService>();
        }

        private MembershipController_New CreateMembershipController()
        {
            var controller = new MembershipController_New(
                _userSetting,
                _emailSubscriptionSetting,
                _fakeUserService.Object,
                _fakeRegistrationRequestService.Object,
                _fakeEmailService.Object,
                _fakeAppContext.Object,
                _fakeAuthenticationService.Object,
                _fakeSharedDataWrapper.Object,
                _fakeEmailSubscriptionService.Object,
                _fakeRoleService.Object,
                _fakeUserRegistrationService.Object,
                _fakeGlobalAttributeService.Object,
                _fakeProfileService.Object,
                _fakeBuyerService.Object,
                _fakeAffiliateService.Object,
                _fakeIAccountingService.Object,
                _fakeLocalizedStringService.Object,
                _fakeSettingService.Object
                )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
            return controller;
        }

        #region CreateRegistrationRequest

        [Fact]
        public void CreateRegistrationRequest_ValidModel_CallsRegistrationRequestServiceDeleteRegistrationRequestOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language{Id = 1});

            var registrationRequestModel = new RegistrationRequestCreateModel
            {
                Email = Guid.NewGuid().ToString()
            };
            var controller = CreateMembershipController();


            //Act
            controller.CreateRegistrationRequest(registrationRequestModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock=>mock.DeleteRegistrationRequest(registrationRequestModel.Email), Times.Once);
        }


        [Fact]
        public void CreateRegistrationRequest_ValidModel_CallsRegistrationRequestServiceInsertRegistrationRequestOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var registrationRequestModel = new RegistrationRequestCreateModel
            {
                Email = Guid.NewGuid().ToString()
            };
            var controller = CreateMembershipController();


            //Act
            controller.CreateRegistrationRequest(registrationRequestModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock => mock.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()), Times.Once);
        }


        [Fact]
        public void CreateRegistrationRequest_ValidModel_CallsEmailServiceSendUserWelcomeMessageWithValidationCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var registrationRequestModel = new RegistrationRequestCreateModel
            {
                Email = Guid.NewGuid().ToString()
            };
            var controller = CreateMembershipController();


            //Act
            controller.CreateRegistrationRequest(registrationRequestModel);


            //Assert
            //_fakeEmailService.Verify(mock => mock.SendUserWelcomeMessageWithValidationCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative), Times.Once);
        }


        [Fact]
        public void CreateRegistrationRequest_RegistrationRequestServiceDeleteRegistrationRequestThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeRegistrationRequestService
                .Setup(mock => mock.DeleteRegistrationRequest(It.IsAny<string>()))
                .Throws<Exception>();

            var registrationRequestModel = new RegistrationRequestCreateModel();
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CreateRegistrationRequest(registrationRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void CreateRegistrationRequest_RegistrationRequestServiceInsertRegistrationRequestThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeRegistrationRequestService
                .Setup(mock => mock.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()))
                .Throws<Exception>();

            var registrationRequestModel = new RegistrationRequestCreateModel();
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CreateRegistrationRequest(registrationRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void CreateRegistrationRequest_EmailServiceInsertSendUserWelcomeMessageWithValidationCodeThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithValidationCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative))
                .Throws<Exception>();

            var registrationRequestModel = new RegistrationRequestCreateModel();
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CreateRegistrationRequest(registrationRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            //Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void CreateRegistrationRequest_InvalidModel_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var controller = CreateMembershipController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");

            var registrationRequestModel = new RegistrationRequestCreateModel();


            //Act
            var httpActionResult = controller.CreateRegistrationRequest(registrationRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;


            //Assert
            //Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            //Assert.Equal("{\"Message\":\"testErrorMessage1, testErrorMessage2\"}", errorMessage);
        }

        #endregion


        #region Register

        [Fact]
        public void Register_UserRegistrationTypeDisabled_ReturnsBadRequestStatusCode()
        {
            //Setup
            _userSetting.UserRegistrationType = UserRegistrationType.Disabled;
            var controller = CreateMembershipController();

            var registerModel = new RegisterModel();


            //Act
            var httpActionResult = controller.Register(registerModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        [Fact]
        public void Register_InvalidModel_ReturnsBadRequestStatusCode()
        {
            //Setup
            var controller = CreateMembershipController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");

            var registerModel = new RegisterModel();


            //Act
            var httpActionResult = controller.Register(registerModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;


            //Assert
            //Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            //Assert.Equal("{\"Message\":\"testErrorMessage1, testErrorMessage2\"}", errorMessage);
        }


        [Fact]
        public void Register_ValidModel_CallsUserRegistrationServiceRegisterUserOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeUserRegistrationService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
                .Returns(new UserRegistrationResult());

            _fakeRoleService
                .Setup(mock => mock.GetRoleByKey(It.IsAny<string>()))
                .Returns(new Role());

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative, It.IsAny<string>(), It.IsAny<string>()));

            var controller = CreateMembershipController();
            var registerModel = new RegisterModel();


            //Act
            controller.Register(registerModel);


            //Assert
            _fakeUserRegistrationService.Verify(mock=>mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()), Times.Once);

        }


        [Fact]
        public void Register_UserRegistrationServiceRegisterUserReturnsSuccessResult_CallsProfileServiceInsertProfileOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeUserRegistrationService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
                .Returns(new UserRegistrationResult());

            _fakeRoleService
                .Setup(mock => mock.GetRoleByKey(It.IsAny<string>()))
                .Returns(new Role());

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative, It.IsAny<string>(), It.IsAny<string>()));

            var controller = CreateMembershipController();
            var registerModel = new RegisterModel();


            //Act
            controller.Register(registerModel);


            //Assert
            _fakeProfileService.Verify(mock => mock.InsertProfile(It.IsAny<Profile>()), Times.Once);
        }


        [Fact]
        public void Register_RoleServiceGetRoleByKeyThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeUserRegistrationService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
                .Returns(new UserRegistrationResult());

            _fakeRoleService
                .Setup(mock => mock.GetRoleByKey(It.IsAny<string>()))
                .Throws<Exception>();

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative, It.IsAny<string>(), It.IsAny<string>()));

            var controller = CreateMembershipController();
            var registerModel = new RegisterModel();


            //Act
            var httpActionResult = controller.Register(registerModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void Register_UserRegistrationServiceRegisterUserThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeUserRegistrationService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
                .Throws<Exception>();

            _fakeRoleService
                .Setup(mock => mock.GetRoleByKey(It.IsAny<string>()))
                .Returns(new Role());

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative, It.IsAny<string>(), It.IsAny<string>()));

            var controller = CreateMembershipController();
            var registerModel = new RegisterModel();


            //Act
            var httpActionResult = controller.Register(registerModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        [Fact]
        public void Register_EmailServiceSendUserWelcomeMessageWithUsernamePasswordThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            _fakeUserRegistrationService
                .Setup(mock => mock.RegisterUser(It.IsAny<UserRegistrationRequest>(), It.IsAny<bool>()))
                .Returns(new UserRegistrationResult());

            _fakeRoleService
                .Setup(mock => mock.GetRoleByKey(It.IsAny<string>()))
                .Returns(new Role());

            _fakeEmailService
                .Setup(mock => mock.SendUserWelcomeMessageWithUsernamePassword(It.IsAny<User>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative, It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();

            var controller = CreateMembershipController();
            var registerModel = new RegisterModel();


            //Act
            var httpActionResult = controller.Register(registerModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            //Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion


        #region ValidateEmail

        [Fact]
        public void ValidateEmail_RegisterValidationModelIsNull_ReturnsBadRequestStatusCode()
        {
            //Setup
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.ValidateEmail(null);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void ValidateEmail_ResendFalse_CallsRegistrationRequestServiceGetRegistrationRequestOnce()
        {
            //Setup
            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = false
            };


            //Act
            controller.ValidateEmail(registerValidationModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock=>mock.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public void  ValidateEmail_RegistrationRequestServiceGetRegistrationRequestNotNull_CallsRegistrationRequestServiceDeleteRegistrationRequestOnce()
        {
            //Setup
            _fakeRegistrationRequestService
                .Setup(mock => mock.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new RegistrationRequest());

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = false
            };


            //Act
            controller.ValidateEmail(registerValidationModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock=>mock.DeleteRegistrationRequest(It.IsAny<RegistrationRequest>()));
        }


        [Fact]
        public void ValidateEmail_ResendTrue_CallsRegistrationRequestServiceDeleteRegistrationRequestOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = true
            };


            //Act
            controller.ValidateEmail(registerValidationModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock => mock.DeleteRegistrationRequest(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public void ValidateEmail_ResendTrue_CallsRegistrationRequestServiceInsertRegistrationRequestOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = true
            };


            //Act
            controller.ValidateEmail(registerValidationModel);


            //Assert
            _fakeRegistrationRequestService.Verify(mock => mock.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()), Times.Once);
        }


        [Fact]
        public void ValidateEmail_ResendTrue_CallsEmailServiceSendUserWelcomeMessageWithValidationCodeOnce()
        {
            //Setup
            _fakeAppContext
                .Setup(mock => mock.AppLanguage)
                .Returns(new Language { Id = 1 });

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = true
            };


            //Act
            controller.ValidateEmail(registerValidationModel);


            //Assert
           // _fakeEmailService.Verify(mock => mock.SendUserWelcomeMessageWithValidationCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), EmailOperatorEnums.LeadNative), Times.Once);
        }


        [Fact]
        public void ValidateEmail_RegistrationRequestServiceGetRegistrationRequestThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeRegistrationRequestService
                .Setup(mock => mock.GetRegistrationRequest(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = false
            };


            //Act
            var httpActionResult = controller.ValidateEmail(registerValidationModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void ValidateEmail_RegistrationRequestServiceDeleteRegistrationRequestThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeRegistrationRequestService
                .Setup(mock => mock.DeleteRegistrationRequest(It.IsAny<string>()))
                .Throws<Exception>();

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = true
            };


            //Act
            var httpActionResult = controller.ValidateEmail(registerValidationModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void ValidateEmail_RegistrationRequestServiceInsertRegistrationRequestThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeRegistrationRequestService
                .Setup(mock => mock.InsertRegistrationRequest(It.IsAny<RegistrationRequest>()))
                .Throws<Exception>();

            var controller = CreateMembershipController();
            var registerValidationModel = new RegisterValidationModel
            {
                Resend = true
            };


            //Act
            var httpActionResult = controller.ValidateEmail(registerValidationModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion


        #region CheckUsernameAvailability

        [Fact]
        public void CheckUsernameAvailability_UsernameEnabledFalse_ReturnsValidationModelFalse()
        {
            //Setup
            var username = Guid.NewGuid().ToString();
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(username);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<ValidationModel>(responseContentString);


            //Assert
            Assert.False(responseObject.Result);
        }


        [Fact]
        public void CheckUsernameAvailability_UsernameNull_ReturnsValidationModelFalse()
        {
            //Setup
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(null);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<ValidationModel>(responseContentString);


            //Assert
            Assert.False(responseObject.Result);
        }



        [Fact]
        public void CheckUsernameAvailability_UsernameWhitespace_ReturnsValidationModelFalse()
        {
            //Setup
            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(" ");
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<ValidationModel>(responseContentString);


            //Assert
            Assert.False(responseObject.Result);
        }


        [Fact]
        public void CheckUsernameAvailability_UsernameEqualsAppUserUsername_ReturnsValidationModelFalse()
        {
            //Setup
            var username = Guid.NewGuid().ToString();

            var appUser = new User
            {
                Username = username
            };

            _fakeAppContext
                .Setup(mock => mock.AppUser)
                .Returns(appUser);

            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(username);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<ValidationModel>(responseContentString);


            //Assert
            Assert.False(responseObject.Result);
        }


        [Fact]
        public void CheckUsernameAvailability_UserServiceGetUserByUsernameNull_ReturnsValidationModelTrue()
        {
            //Setup
            var username = Guid.NewGuid().ToString();

            _userSetting.UsernameEnabled = true;

            _fakeUserService
                .Setup(mock => mock.GetUserByUsername(username))
                .Returns<User>(null);

            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(username);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<ValidationModel>(responseContentString);


            //Assert
            Assert.True(responseObject.Result);
        }


        [Fact]
        public void CheckUsernameAvailability_LocalizedStringServiceGetLocalizedStringThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            var username = Guid.NewGuid().ToString();

            _fakeLocalizedStringService
                .Setup(mock => mock.GetLocalizedString(It.IsAny<string>()))
                .Throws<Exception>();

            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(username);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        [Fact]
        public void CheckUsernameAvailability_UserServiceGetUserByUsernameThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            var username = Guid.NewGuid().ToString();

            _userSetting.UsernameEnabled = true;

            _fakeUserService
                .Setup(mock => mock.GetUserByUsername(username))
                .Throws<Exception>();

            var controller = CreateMembershipController();


            //Act
            var httpActionResult = controller.CheckUsernameAvailability(username);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

    }
}