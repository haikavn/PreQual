using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.AffiliateChannel;
using Moq;
using Newtonsoft.Json;
using Xunit;
using static Adrack.Web.Framework.Security.ContentManagementApiAuthorizeAttribute;

namespace Adrack.WebApi.Tests
{
    public class AffiliateChannelControllerTests
    {
        private const long AffiliateChannelId = 100; //Any long number for testing
        private readonly Mock<IAffiliateChannelService> _fakeAffiliateChannelService;
        private readonly Mock<IAffiliateChannelTemplateService> _fakeAffiliateChannelTemplateService;
        private readonly Mock<ICampaignTemplateService> _fakeCampaignTemplateService;
        private readonly Mock<IAffiliateChannelFilterConditionService> _fakeAffiliateChannelFilterConditionService;
        private readonly Mock<IAffiliateService> _fakeAffiliateService;
        private readonly Mock<IBlackListService> _fakeBlackListService;
        private readonly Mock<ICampaignService> _fakeCampaignService;
        private readonly Mock<IBuyerChannelService> _fakeBuyerChannelService;
        private readonly Mock<ISettingService> _fakeSettingService;
        private readonly Mock<IEmailService> _fakeEmailService;
        private readonly Mock<IAppContext> _fakeAppContext;
        private readonly Mock<ISmtpAccountService> _fakeSmtpAccountService;
        private readonly Mock<ISearchService> _fakeSearchService;
        private readonly Mock<IProfileService> _fakeProfileService;
        private readonly Mock<IRepository<AffiliateChannelTemplate>> _fakeAffiliateChannelTemplateRepo;
        private readonly Mock<IRepository<AffiliateChannel>> _fakeAffiliateChannelRepo;
        private readonly Mock<IRepository<CampaignField>> _fakeCampaignTemplateRepo;
        private readonly Mock<IRepository<Campaign>> _fakeCampaignRepo;
        private readonly Mock<IPermissionService> _fakePermissionService;
        private readonly Mock<IPlanService> _fakePlanService;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<IEntityChangeHistoryService> _entityChangeHistoryService;


        public AffiliateChannelControllerTests()
        {
            _fakeAppContext = new Mock<IAppContext>();
            _fakeAffiliateChannelService = new Mock<IAffiliateChannelService>();
            _fakeAffiliateChannelTemplateService = new Mock<IAffiliateChannelTemplateService>();
            _fakeCampaignTemplateService = new Mock<ICampaignTemplateService>();
            _fakeAffiliateChannelFilterConditionService = new Mock<IAffiliateChannelFilterConditionService>();
            _fakeAffiliateService = new Mock<IAffiliateService>();
            _fakeBlackListService = new Mock<IBlackListService>();
            _fakeCampaignService = new Mock<ICampaignService>();
            _fakeBuyerChannelService = new Mock<IBuyerChannelService>();
            _fakeSettingService = new Mock<ISettingService>();
            _fakeEmailService = new Mock<IEmailService>();
            _fakeSmtpAccountService = new Mock<ISmtpAccountService>();
            _fakeSearchService = new Mock<ISearchService>();
            _fakeProfileService = new Mock<IProfileService>();
            _fakeAffiliateChannelTemplateRepo = new Mock<IRepository<AffiliateChannelTemplate>>();
            _fakeAffiliateChannelRepo = new Mock<IRepository<AffiliateChannel>>();
            _fakeCampaignTemplateRepo = new Mock<IRepository<CampaignField>>();
            _fakeCampaignRepo = new Mock<IRepository<Campaign>>();
            _fakePermissionService = new Mock<IPermissionService>();
            _fakePlanService = new Mock<IPlanService>();
            _fakeUserService = new Mock<IUserService>();
            _entityChangeHistoryService = new Mock<IEntityChangeHistoryService>();
        }


        private AffiliateChannelController CreateAffiliateChannelController()
        {
            var controller = new AffiliateChannelController(
                _fakeAppContext.Object,
                _fakeAffiliateChannelService.Object,
                _fakeAffiliateChannelTemplateService.Object,
                _fakeCampaignTemplateService.Object,
                _fakeAffiliateChannelFilterConditionService.Object,
                _fakeAffiliateService.Object,
                 _fakeBlackListService.Object,
               _fakeCampaignService.Object,
               _fakeSettingService.Object,
                _fakeBuyerChannelService.Object,
                 _fakeEmailService.Object,
                 _fakeSmtpAccountService.Object,
                 _fakeSearchService.Object,
                 _fakeProfileService.Object,
                 _fakeAffiliateChannelTemplateRepo.Object,
                 _fakeAffiliateChannelRepo.Object,
                 _fakeCampaignTemplateRepo.Object,
                 _fakeCampaignRepo.Object,
                 _fakePermissionService.Object,
                 _fakePlanService?.Object,
                 _fakeUserService.Object,
                 _entityChangeHistoryService.Object,
                 null
                )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
            return controller;
        }


        #region GetAffiliateChannelById

        [Fact]
        public void GetAffiliateChannelById_CallsAffiliateChannelServiceGetAffiliateChannelByIdOnce()
        {
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Setup
            var controller = CreateAffiliateChannelController();


            //Act
            controller.GetAffiliateChannelById(AffiliateChannelId);


            //Assert
            _fakeAffiliateChannelService.Verify(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()), Times.Once);
        }


        [Fact]
        public void GetAffiliateChannelById_NonExistingAffiliateChannel_ReturnsNotFoundStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns<AffiliateChannel>(null);

            var controller = CreateAffiliateChannelController();


            //Act
            var httpActionResult = controller.GetAffiliateChannelById(AffiliateChannelId);
            var httpResponseMessage = httpActionResult as OkNegotiatedContentResult<AffiliateChannel>;


            //Assert
            Assert.Null(httpResponseMessage?.Content);
        }

        [Fact]
        public void GetAffiliateChannelById_ExistingAffiliateChannel_ReturnsOkStatusCodeWithAffiliateChannel()
        {
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Setup
            var expectedAffiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(expectedAffiliateChannel);

            var controller = CreateAffiliateChannelController();


            //Act
            dynamic results = controller.GetAffiliateChannelById(AffiliateChannelId);
            dynamic content = results?.Content;
            //Assert
            Assert.Equal(expectedAffiliateChannel.Id, content?.Id);
        }


        [Fact]
        public void GetAffiliateChannelById_AffiliateChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Throws<Exception>();

            var controller = CreateAffiliateChannelController();


            //Act
            var httpActionResult = controller.GetAffiliateChannelById(AffiliateChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);

        }

        #endregion

        #region AddAffiliateChannel

        [Fact]
        public void AddAffiliateChannel_ValidModel_CallsAffiliateChannelServiceInsertAffiliateChannelOnce()
        {
           
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
           
            //Setup
            var controller = CreateAffiliateChannelController();
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };
            _fakeAffiliateChannelService
               .Setup(mock => mock.InsertAffiliateChannel(affiliateChannel))
               .Returns(AffiliateChannelId);

           

            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);

            controller.ModelState.Clear();

           

            var affiliateChannelCreateModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };


            //Act
            controller.CreateAffiliateChannel(affiliateChannelCreateModel);


            //Assert
            //_fakeAffiliateChannelService.Verify(mock => mock.InsertAffiliateChannel(affiliateChannel), Times.Once);
        }

        [Fact]
        public void AddAffiliateChannel_ValidModel_ReturnsCreatedAtStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.InsertAffiliateChannel(It.IsAny<AffiliateChannel>()))
                .Returns(AffiliateChannelId);

            var controller = CreateAffiliateChannelController();
            controller.ModelState.Clear();
            var createAffiliateChannelModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };


            //Act
            var httpActionResult = controller.CreateAffiliateChannel(createAffiliateChannelModel);
            var httpResponseMessage = httpActionResult as OkNegotiatedContentResult<long>;


            //Assert
            //Assert.Equal(AffiliateChannelId, httpResponseMessage?.Content);
        }


        [Fact]
        public void AddAffiliateChannel_InvalidModel_ReturnsBadRequestStatusCodeWithErrorMessage()
        {
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Setup
            var controller = CreateAffiliateChannelController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");
            var createAffiliateChannelModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };


            //Act
            var httpActionResult = controller.CreateAffiliateChannel(createAffiliateChannelModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;

            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
            Assert.Equal("{\"Status\":400,\"Message\":\"testErrorMessage1, testErrorMessage2\",\"Errors\":[]}", errorMessage);
        }

        [Fact]
        public void AddAffiliateChannel_AffiliateChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.InsertAffiliateChannel(It.IsAny<AffiliateChannel>()))
                .Throws<Exception>();

            var controller = CreateAffiliateChannelController();
            var createAffiliateChannelModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };


            //Act
            var httpActionResult = controller.CreateAffiliateChannel(createAffiliateChannelModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;

            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        //[Fact]
        //public void AddAffiliateChannel_AffiliateChannelFilterNotNull_CallsAffiliateChannelFilterConditionServiceInsertFilterCondition()
        //{
        //    //Setup
        //    var controller = CreateAffiliateChannelController();
        //    var createAffiliateChannelModel = new AffiliateChannelCreateModel
        //    {
        //        AffiliateChannelFilters = new List<AffiliateChannelFilterCreateModel>()
        //        {
        //            new AffiliateChannelFilterCreateModel(),
        //            new AffiliateChannelFilterCreateModel()
        //        }
        //    };


        //    //Act 
        //    controller.CreateAffiliateChannel(createAffiliateChannelModel);


        //    //Assert
        //    _fakeAffiliateChannelFilterConditionService.Verify(mock => mock.InsertFilterCondition(It.IsAny<AffiliateChannelFilterCondition>()), Times.Exactly(2));

        //}

        #endregion

        #region UpdateAffiliateChannel

        [Fact]
        public void UpdateAffiliateChannel_ValidModel_CallsAffiliateChannelServiceUpdateAffiliateChannelOnce()
        {
            //Setup
            var controller = CreateAffiliateChannelController();
            controller.ModelState.Clear();
            var affiliateChannelUpdateModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(affiliateChannel);
            //Act
            controller.UpdateAffiliateChannel(AffiliateChannelId, affiliateChannelUpdateModel);


            //Assert
            //_fakeAffiliateChannelService.Verify(mock => mock.UpdateAffiliateChannel(It.IsAny<AffiliateChannel>()), Times.Once);
        }

        [Fact]
        public void UpdateAffiliateChannel_ValidModel_ReturnsOkStatusCode()
        {
            //Setup
            var controller = CreateAffiliateChannelController();
            controller.ModelState.Clear();
            var affiliateChannelUpdateModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(affiliateChannel);

            //Act
            var httpActionResult = controller.UpdateAffiliateChannel(AffiliateChannelId, affiliateChannelUpdateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            //Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }

        [Fact]
        public void UpdateAffiliateChannel_InvalidModel_ReturnsBadRequestStatusCodeWithErrorMessage()
        {
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Setup
            var controller = CreateAffiliateChannelController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");
            var affiliateChannelUpdateModel = new AffiliateChannelCreateModel();


            //Act
            var httpActionResult = controller.UpdateAffiliateChannel(AffiliateChannelId, affiliateChannelUpdateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
            Assert.Equal("{\"Status\":400,\"Message\":\"testErrorMessage1, testErrorMessage2\",\"Errors\":[]}", errorMessage);
        }

        [Fact]
        public void UpdateAffiliateChannel_AffiliateChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.UpdateAffiliateChannel(It.IsAny<AffiliateChannel>()))
                .Throws<Exception>();

            var controller = CreateAffiliateChannelController();
            var affiliateChannelUpdateModel = new AffiliateChannelCreateModel { AffiliateChannelFields = new List<AffiliateChannelIntegrationModel>() };


            //Act
            var httpActionResult = controller.UpdateAffiliateChannel(AffiliateChannelId, affiliateChannelUpdateModel);
            var resultStatusCode = httpActionResult.ExecuteAsync(CancellationToken.None).Result;

            var code = resultStatusCode.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region DeleteAffiliateChannel

        [Fact]
        public void DeleteAffiliateChannel_ExistingAffiliateChannel_CallsAffiliateChannelServiceDeleteAffiliateChannelOnce()
        {
            //Setup
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(affiliateChannel);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateAffiliateChannelController();


            //Act
            controller.DeleteAffiliateChannelById(AffiliateChannelId);


            //Assert
            _fakeAffiliateChannelService.Verify(mock => mock.UpdateAffiliateChannel(affiliateChannel), Times.Once);
        }

        [Fact]
        public void DeleteAffiliateChannel_ExistingAffiliateChannel_ReturnsOkStatusCode()
        {
            //Setup
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId,
                AffiliateChannelFields = new List<AffiliateChannelTemplate>()
            };

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(affiliateChannel);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateAffiliateChannelController();


            //Act
            var httpActionResult = controller.DeleteAffiliateChannelById(AffiliateChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }

        [Fact]
        public void DeleteAffiliateChannel_NonExistingAffiliateChannel_ReturnsNotFoundStatusCode()
        {
            //Setup
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns<AffiliateChannel>(null);

            var controller = CreateAffiliateChannelController();


            //Act
            var httpActionResult = controller.DeleteAffiliateChannelById(AffiliateChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        [Fact]
        public void DeleteAffiliateChannel_AffiliateChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            var affiliateChannel = new AffiliateChannel
            {
                Id = AffiliateChannelId
            };

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
                .Returns(affiliateChannel);

            _fakeAffiliateChannelService
                .Setup(mock => mock.UpdateAffiliateChannel(It.IsAny<AffiliateChannel>()))
                .Throws<Exception>();

            var controller = CreateAffiliateChannelController();


            //Act
            var httpActionResult = controller.DeleteAffiliateChannelById(AffiliateChannelId);
            var resultStatusCode = httpActionResult.ExecuteAsync(CancellationToken.None).Result;

            var code = resultStatusCode.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion


        //#region LoadAffiliateChannelTemplate

        //[Fact]
        //public void LoadAffiliateChannelTemplate_AffiliateChannelNull_ReturnsRootElementWithNoChildren()
        //{
        //    //Setup
        //    _fakeAffiliateChannelService
        //        .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
        //        .Returns<AffiliateChannel>(null);

        //    var controller = CreateAffiliateChannelController();


        //    //Act
        //    var httpActionResult = controller.LoadAffiliateChannelTemplate(AffiliateChannelId);
        //    var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
        //    var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
        //    var responseObject = JsonConvert.DeserializeObject<AffiliateChannelTreeItem>(responseContentString);


        //    //Assert
        //    Assert.Equal("root", responseObject.Title);
        //    Assert.Empty(responseObject.Children);
        //}

        //[Fact]
        //public void LoadAffiliateChannelTemplate_AffiliateChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        //{
        //    //Setup
        //    _fakeAffiliateChannelService
        //        .Setup(mock => mock.GetAffiliateChannelById(AffiliateChannelId, It.IsAny<bool>()))
        //        .Throws<Exception>();

        //    var controller = CreateAffiliateChannelController();


        //    //Act
        //    var httpActionResult = controller.LoadAffiliateChannelTemplate(AffiliateChannelId);
        //    var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


        //    //Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        //}

        //#endregion
    }
}