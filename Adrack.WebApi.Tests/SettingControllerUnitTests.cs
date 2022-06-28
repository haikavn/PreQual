using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure;
using Adrack.Service.Message;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Settings;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for SettingControllerUnitTests
    /// </summary>
    public class SettingControllerUnitTests
    {
        #region properties

        private string UnreachableCodeExecuted => "Unreachable Code Executed";

        private readonly ITestOutputHelper _output = new TestOutputHelper();

        public FakeContextInitializer FakeContext { get; set; }

        #endregion

        #region constructors

        private SettingControllerUnitTests(FakeContextInitializer fakeContextInitializer)
        {
            // Initialize Engine Context
            FakeContext = fakeContextInitializer;
        }

        public SettingControllerUnitTests()
        : this(new FakeContextInitializer())
        {
            // Initialize Engine Context
        }

        #endregion

        #region test methods

        //[Fact]
        //public void GetReturnsExistingSettingModelTypeIntegrationTest()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"no setting was found for given id {id}");

        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(), 
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.Get();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);  
        //    }
        //}

        //[Fact]
        //public void GetSmtpSettingIntegrationTest()
        //{
        //    // Arrange
        //    var smtpAccounts = 0;
        //    var notFoundException = new ArgumentNullException(nameof(smtpAccounts), $"No smtp accounts has found.");

        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.GetSmtp();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingSmtpModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }
        //}

        //[Fact]
        //public void GetTimeZoneSettingIntegrationTest()
        //{
        //    // Arrange
        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.GetTimeZone();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingTimeZoneModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }
        //}

        //[Fact]
        //public void GetEmailTemplateSettingIntegrationTest()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no email template for given id equals to {id}");

        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplate(id);
        //        // Assert
        //        Assert.IsAssignableFrom<EmailTemplateModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }
        //}

        //[Fact]
        //public void GetEmailTemplatesSettingIntegrationTest()
        //{
        //    // Arrange
        //    const int emailTemplatesList = 1;
        //    var notFoundException = new ArgumentNullException(nameof(emailTemplatesList), $"Email template has not found.");

        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplates();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingEmailTemplatesModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }
        //}

        //[Fact]
        //public void GetReturnsExistingSettingValuesAreNotNullIntegrationTest()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"no setting was found for given id {id}");

        //    var appContext = AppEngineContext.Current.Resolve<IAppContext>();
        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();
        //    var emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();
        //    var fileUploadService = AppEngineContext.Current.Resolve<IFileUploadService>();
        //    var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();
        //    var controller = new SettingController(appContext, dateTimeHelper, emailTemplateService, 
        //        fileUploadService, permissionService, settingService, smtpAccountService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    SettingModel settingModel = null;
        //    try
        //    {
        //        settingModel = controller.Get();
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }

        //    // Assert
        //    // settingModel.ErrorMessage and settingModel.ProcessingDelayString can be null
        //    //Assert.NotNull(settingModel.AutoCacheMode);
        //    //Assert.NotNull(settingModel.IsSaved);
        //    //Assert.NotNull(settingModel.ProcessingDelay);
        //    Assert.NotNull(settingModel);
        //    Assert.NotNull(settingModel.AccountNumber);
        //    Assert.NotNull(settingModel.AccountType);
        //    Assert.NotNull(settingModel.AffiliateRedirectUrl);
        //    Assert.NotNull(settingModel.AffiliateXmlField);
        //    Assert.NotNull(settingModel.AllowAffiliateRedirect);
        //    Assert.NotNull(settingModel.AppUrl);
        //    Assert.NotNull(settingModel.City);
        //    Assert.NotNull(settingModel.CompanyAddress);
        //    Assert.NotNull(settingModel.CompanyAddress2);
        //    Assert.NotNull(settingModel.CompanyBank);
        //    Assert.NotNull(settingModel.CompanyEmail);
        //    Assert.NotNull(settingModel.CompanyLogoPath);
        //    Assert.NotNull(settingModel.CompanyName);
        //    Assert.NotNull(settingModel.Country);
        //    Assert.NotNull(settingModel.DebugMode);
        //    Assert.NotNull(settingModel.DuplicateMonitor);
        //    Assert.NotNull(settingModel.LeadEmail);
        //    Assert.NotNull(settingModel.LeadEmailFields);
        //    Assert.NotNull(settingModel.LeadEmailTo);
        //    Assert.NotNull(settingModel.LoginExpire);
        //    Assert.NotNull(settingModel.MaxProcessingLeads);
        //    Assert.NotNull(settingModel.MinProcessingMode);
        //    Assert.NotNull(settingModel.PageTitle);
        //    Assert.NotNull(settingModel.PostingUrl);
        //    Assert.NotNull(settingModel.RoutingNumber);
        //    Assert.NotNull(settingModel.State);
        //    Assert.NotNull(settingModel.SystemOnHold);
        //    Assert.NotNull(settingModel.WhiteIp);
        //    Assert.NotNull(settingModel.ZipCode);
        //}

        //[Fact]
        //public void GetReturnsCorrectSettingModelTypeUnitTest()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"no setting was found for given id {id}");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockDateTimeHelper.Setup(x => x.GetUserTimeZone(It.IsAny<Core.Domain.Membership.User>())).
        //        Returns((TimeZoneInfo)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object, 
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        var response = controller.Get();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }
        //}

        //[Fact]
        //public void GetSmtpSettingExceptionUnitTest()
        //{
        //    // Arrange
        //    IList<SmtpAccount> smtpAccounts = null;
        //    var notFoundException = new ArgumentNullException(nameof(smtpAccounts), $"No smtp accounts has found.");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockSmtpAccountService.Setup(x => x.GetAllSmtpAccounts()).
        //        Returns((IList<SmtpAccount>)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        var response = controller.GetSmtp();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingSmtpModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //        return;
        //    }

        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void GetSmtpSettingCorrectDataUnitTest()
        //{
        //    // Arrange
        //    const int id = 11;
        //    const string testUsername = "Test";
        //    List<SmtpAccount> smtpAccounts = new List<SmtpAccount> { new SmtpAccount { Id = id, Username = testUsername } };
        //    var notFoundException = new ArgumentNullException(nameof(smtpAccounts), $"No smtp accounts has found.");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockSmtpAccountService.Setup(x => x.GetAllSmtpAccounts()).
        //        Returns(smtpAccounts);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    SettingSmtpModel smtpSetting = null;
        //    // Act
        //    try
        //    {
        //        var response = controller.GetSmtp();
        //        Assert.IsAssignableFrom<SettingSmtpModel>(response);
        //        smtpSetting = response;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(smtpSetting);
        //    Assert.Equal(smtpSetting.SmtpUsername, testUsername);
        //}

        //[Fact]
        //public void GetTimeZoneSettingWithNullInfoUnitTest()
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();

        //    mockSettingService.Setup(x => x.GetSetting("TimeZoneStr", false)).
        //        Returns((Setting)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        dateTimeHelper, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    SettingTimeZoneModel timeZoneSetting = null;
        //    // Act
        //    try
        //    {
        //        var response = controller.GetTimeZone();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingTimeZoneModel>(response);
        //        timeZoneSetting = response;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(timeZoneSetting);
        //    Assert.NotNull(timeZoneSetting.TimeZones);
        //    Assert.NotEmpty(timeZoneSetting.TimeZones);
        //}

        //[Fact]
        //public void GetTimeZoneSettingSelectedUnitTest()
        //{
        //    // Arrange
        //    var timeZoneInfoValue = "Pacific Standard Time";
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    var dateTimeHelper = AppEngineContext.Current.Resolve<IDateTimeHelper>();

        //    mockSettingService.Setup(x => x.GetSetting("TimeZoneStr", false)).
        //        Returns(new Setting { Value = timeZoneInfoValue });

        //    var controller = new SettingController(mockAppContext.Object,
        //        dateTimeHelper, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);


        //    SettingTimeZoneModel timeZoneSetting = null;
        //    // Act
        //    try
        //    {
        //        var response = controller.GetTimeZone();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingTimeZoneModel>(response);
        //        timeZoneSetting = response;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(timeZoneSetting);
        //    Assert.NotNull(timeZoneSetting.TimeZones);
        //    Assert.True(timeZoneSetting.TimeZones.FirstOrDefault(x => x.Value == timeZoneInfoValue)?.Selected ?? false);
        //}

        //[Fact]
        //public void GetEmailTemplateSettingNotFoundUnitTest()
        //{
        //    // Arrange
        //    const int id = 0;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no email template for given id equals to {id}");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockEmailTemplateService.Setup(x => x.GetEmailTemplateById(0)).
        //        Returns((EmailTemplate)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplate(id);
        //        // Assert
        //        Assert.IsAssignableFrom<EmailTemplateModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //        return;
        //    }
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void GetEmailTemplateSettingUnitTest()
        //{
        //    // Arrange
        //    const int id = 1;
        //    const string subject = "Greeting email";

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockEmailTemplateService.Setup(x => x.GetEmailTemplateById(It.IsAny<long>())).
        //        Returns(new EmailTemplate {Id = id, Subject = subject });

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    EmailTemplateModel emailTemplate = null;
        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplate(id);
        //        // Assert
        //        Assert.IsAssignableFrom<EmailTemplateModel>(response);
        //        emailTemplate = response;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //        return;
        //    }
        //    Assert.NotNull(emailTemplate);
        //    Assert.Equal(subject, emailTemplate.Subject);
        //}

        //[Fact]
        //public void GetEmailTemplatesSettingNotFoundUnitTest()
        //{
        //    // Arrange
        //    const int emailTemplatesList = 1;
        //    var notFoundException = new ArgumentNullException(nameof(emailTemplatesList), $"Email template has not found.");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
        //    mockEmailTemplateService.Setup(x => x.GetAllEmailTemplates()).
        //        Returns((IList<EmailTemplate>)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplates();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingEmailTemplatesModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //        return;
        //    }

        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void GetEmailTemplatesSettingUnitTest()
        //{
        //    // Arrange
        //    var emailTemplates = new List<EmailTemplate>
        //    {
        //        new EmailTemplate { Id = 1, Subject = "Greeting", Name = string.Empty, Active = true}, 
        //        new EmailTemplate { Id = 2, Subject = "Technical Support issue", Name = string.Empty}
        //    };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockEmailTemplateService.Setup(x => x.GetAllEmailTemplates()).
        //        Returns(emailTemplates);
        //    mockPermissionService.Setup(x => x.Authorize(It.IsAny<Core.Domain.Security.Permission>())).
        //        Returns(true);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    SettingEmailTemplatesModel settingEmailTemplates = null;

        //    // Act
        //    try
        //    {
        //        var response = controller.GetEmailTemplates();
        //        // Assert
        //        Assert.IsAssignableFrom<SettingEmailTemplatesModel>(response);
        //        settingEmailTemplates = response;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //        return;
        //    }

        //    // Assert
        //    Assert.NotNull(settingEmailTemplates);
        //    Assert.NotNull(settingEmailTemplates.EmailTemplates);
        //    Assert.Equal(2, settingEmailTemplates.EmailTemplates.Count);
        //}

        //[Fact]
        //public void PostGeneralSettingUnitTest()
        //{
        //    // Arrange
        //    const string accountNumber = "123456";
        //    const string accountNumberFieldName = "Settings.AccountNumber";
        //    var settingModel = new SettingModel() { AccountNumber = accountNumber, LoginExpire = 10 };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockSettingService.Setup(x => x.GetSetting(It.IsNotNull<string>(), false)).
        //        Returns(new Setting()).Verifiable();
        //    mockSettingService.Setup(x => x.GetSetting(accountNumberFieldName, false)).
        //        Returns((Setting)null).Verifiable();
        //    mockSettingService.Setup(x => x.InsertSetting(It.IsNotNull<Setting>(), It.IsAny<bool>())).
        //        Verifiable();
        //    mockSettingService.Setup(x => x.UpdateSetting(It.IsNotNull<Setting>(), It.IsAny<bool>())).
        //        Verifiable();
        //    mockAppContext.Setup(x => x.SetUserExpire(It.IsNotNull<long>())).
        //        Verifiable();
        //    mockAppContext.Setup(x => x.AppUser).
        //        Returns(new User() { Id = 1L }).
        //        Verifiable();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSettingService.VerifyAll();
        //    mockAppContext.Verify();
        //}

        //[Fact]
        //public void PostGeneralSettingNullModelUnitTest()
        //{
        //    // Arrange
        //    var settingModel = (SettingModel) null;
        //    var argumentNullException = new ArgumentNullException(nameof(settingModel), "Setting model cannot be null");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockSettingService.Setup(x => x.GetSetting(It.IsNotNull<string>(), false)).
        //        Returns((Setting)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(argumentNullException.Message, e.Message);
        //        return;
        //    }

        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void PostSmtpSettingNullModelUnitTest()
        //{
        //    // Arrange
        //    SettingSmtpModel settingSmtpModel = null;
        //    var argumentError = new ArgumentNullException(nameof(settingSmtpModel),
        //                "Setting smtp parameters are empty.");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingSmtpModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argumentError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //        return;
        //    } 
            
        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void PostSmtpSettingUnitTest()
        //{
        //    // Arrange
        //    SettingSmtpModel settingSmtpModel = new SettingSmtpModel();

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();
            
        //    mockSmtpAccountService.Setup(x => x.UpdateSmtpAccount(It.IsAny<SmtpAccount>())).
        //        Verifiable();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingSmtpModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSmtpAccountService.Verify();
        //}

        //[Fact]
        //public void PostTimeZoneSettingNullModelUnitTest()
        //{
        //    // Arrange
        //    SettingTimeZoneInsertModel settingTimeZoneModel = null;
        //    var argumentError = new ArgumentNullException(nameof(settingTimeZoneModel), "Setting time zone parameters are empty.");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingTimeZoneModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argumentError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    // Act
        //    try
        //    {
        //        settingTimeZoneModel = new SettingTimeZoneInsertModel();
        //        controller.Post(settingTimeZoneModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argumentError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //        return;
        //    }

        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        //[Fact]
        //public void PostTimeZoneSettingValue0Value1UnitTest()
        //{
        //    // Arrange
        //    SettingTimeZoneInsertModel settingTimeZoneModel = new SettingTimeZoneInsertModel
        //    {
        //        SelectedTimeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault()?.Id
        //    };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockSettingService.Setup(x => x.GetSetting("TimeZone", false)).
        //        Returns((Setting)null);
        //    mockSettingService.Setup(x => x.GetSetting("TimeZoneStr", false)).
        //        Returns(new Setting { });
        //    mockSettingService.Setup(x => x.UpdateSetting(It.IsAny<Setting>(), true)).
        //        Verifiable();
        //    mockSettingService.Setup(x => x.InsertSetting(It.IsAny<Setting>(), true)).
        //        Verifiable();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingTimeZoneModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSettingService.VerifyAll();
        //}

        //[Fact]
        //public void PostTimeZoneSettingValue1Value0UnitTest()
        //{
        //    // Arrange
        //    SettingTimeZoneInsertModel settingTimeZoneModel = new SettingTimeZoneInsertModel
        //    {
        //        SelectedTimeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault()?.Id
        //    };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockSettingService.Setup(x => x.GetSetting("TimeZone", false)).
        //        Returns(new Setting { });
        //    mockSettingService.Setup(x => x.GetSetting("TimeZoneStr", false)).
        //        Returns((Setting)null);
        //    mockSettingService.Setup(x => x.UpdateSetting(It.IsAny<Setting>(), true)).
        //        Verifiable();
        //    mockSettingService.Setup(x => x.InsertSetting(It.IsAny<Setting>(), true)).
        //        Verifiable();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(settingTimeZoneModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSettingService.VerifyAll();
        //}

        //[Fact]
        //public void PostEmailTemplateSettingIncorrectModelUnitTest()
        //{
        //    // Arrange
        //    const long id = 0L;
        //    bool isErrorOccurred = true;
        //    EmailTemplateModel templateModel = new EmailTemplateModel();
        //    var argError = new ArgumentNullException(nameof(templateModel),
        //        "Please, fill the Name, Subject, Body, SmtpAccount, Attachment fields.");
        //    var idsError = new ArgumentNullException(nameof(templateModel),
        //        "Please, fill correct SmtpAccount, Attachment field values.");
        //    var idError = new ArgumentNullException(nameof(id),
        //        $"there is no email template for given id equals to {id}");

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockEmailTemplateService.Setup(x => x.GetEmailTemplateById(0L)).
        //        Returns((EmailTemplate)null);

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(id, null);
        //        isErrorOccurred = false;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    // Act
        //    try
        //    {
        //        controller.Post(id, templateModel);
        //        isErrorOccurred = false;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    // Act
        //    try
        //    {
        //        templateModel.Name = "Test";
        //        templateModel.Subject = "Test";
        //        templateModel.Body = "Test";
        //        templateModel.SmtpAccountId = "first";
        //        templateModel.AttachmentId = "1";

        //        controller.Post(id, templateModel);
        //        isErrorOccurred = false;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(idsError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    // Act
        //    try
        //    {
        //        templateModel.Name = "Test";
        //        templateModel.Subject = "Test";
        //        templateModel.Body = "Test";
        //        templateModel.SmtpAccountId = "1";
        //        templateModel.AttachmentId = "first";
        //        controller.Post(id, templateModel);
        //        isErrorOccurred = false;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(idsError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    // Act
        //    try
        //    {
        //        templateModel.Bcc = "bcc@test.am";
        //        templateModel.Name = "Test";
        //        templateModel.Subject = "Test";
        //        templateModel.Body = "Test";
        //        templateModel.SmtpAccountId = "1";
        //        templateModel.AttachmentId = "1";

        //        controller.Post(id, templateModel);
        //        isErrorOccurred = false;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(idError.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //    }

        //    if (!isErrorOccurred)
        //    {
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }
        //}

        //[Fact]
        //public void PostEmailTemplateSettingCorrectModelUnitTest()
        //{
        //    // Arrange
        //    const long id = 1L;
        //    var templateModel = new EmailTemplateModel
        //    {
        //        Id = id,
        //        Bcc = "bcc@test.am",
        //        Name = "Test",
        //        Subject = "Test",
        //        Body = "Test",
        //        SmtpAccountId = "1",
        //        AttachmentId = "1"
        //    };
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockDateTimeHelper = new Mock<IDateTimeHelper>();
        //    var mockEmailTemplateService = new Mock<IEmailTemplateService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockPermissionService = new Mock<IPermissionService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    var mockSmtpAccountService = new Mock<ISmtpAccountService>();

        //    mockEmailTemplateService.Setup(x => x.GetEmailTemplateById(id)).
        //        Returns(new EmailTemplate { Id = id, Subject = templateModel.Subject });
        //    mockEmailTemplateService.Setup(x => x.UpdateEmailTemplate(It.IsNotNull<EmailTemplate>())).
        //        Verifiable();

        //    var controller = new SettingController(mockAppContext.Object,
        //        mockDateTimeHelper.Object, mockEmailTemplateService.Object, mockFileUploadService.Object,
        //        mockPermissionService.Object, mockSettingService.Object, mockSmtpAccountService.Object);

        //    // Act
        //    try
        //    {
        //        controller.Post(id, templateModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    } 
            
        //    mockEmailTemplateService.VerifyAll();
        //}

        #endregion
    }
}
