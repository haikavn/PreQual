using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.BuyerChannels;
using Adrack.WebApi.Models.New.BuyerChannel;
using Castle.Components.DictionaryAdapter;
using Moq;
using Xunit;

namespace Adrack.WebApi.Tests
{
    public class BuyerChannelControllerTests
    {
        private const long BuyerChannelId = 100; //Any long number for testing
        private readonly Mock<IBuyerChannelService> _fakeBuyerChannelService;
        private readonly Mock<IPermissionService> _fakePermissionService;
        private readonly Mock<IBuyerChannelFilterConditionService> _fakeBuyerChannelFilterConditionService;
        private readonly Mock<IBuyerChannelScheduleService> _fakeBuyerChannelScheduleService;
        private readonly Mock<IBuyerChannelTemplateService> _fakeBuyerChannelTemplateService;
        private readonly Mock<IBuyerChannelTemplateMatchingService> _fakeBuyerChannelTemplateMatchingService;
        private readonly Mock<ICampaignService> _fakeCampaignService;
        private readonly Mock<ICampaignTemplateService> _fakeCampaignTemplateService;

        private readonly Mock<ISearchService> _fakeSearchService;
        private readonly Mock<IBuyerService> _fakeBuyerService;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<IRoleService> _fakeRoleService;

        private readonly Mock<IAppContext> _fakeAppContext;
        private readonly Mock<ICountryService> _fakeCountryService;

        private readonly Mock<IRepository<Campaign>> _fakeCampaignRepo;
        private readonly Mock<IRepository<Buyer>> _fakeBuyerRepo;
        private readonly Mock<IPlanService> _fakePlanService;
        private readonly Mock<IAffiliateService> _fakeAffiliateService;
        private readonly Mock<IAffiliateChannelService> _fakeAffiliateChannelService;
        private readonly Mock<ISettingService> _fakeSettingService;
        private readonly Mock<IEmailService> _fakeEmailService;
        private readonly Mock<ISmtpAccountService> _fakeSmtpAccountServiceService;

        private readonly Mock<IEntityChangeHistoryService> _entityChangeHistoryService;

        public BuyerChannelControllerTests()
        {
            _fakeBuyerChannelService = new Mock<IBuyerChannelService>();
            _fakeBuyerChannelFilterConditionService = new Mock<IBuyerChannelFilterConditionService>();
            _fakeBuyerChannelScheduleService = new Mock<IBuyerChannelScheduleService>();
            _fakeBuyerChannelTemplateService = new Mock<IBuyerChannelTemplateService>();
            _fakeBuyerChannelTemplateMatchingService = new Mock<IBuyerChannelTemplateMatchingService>();
            _fakeCampaignService = new Mock<ICampaignService>();
            _fakeCampaignTemplateService = new Mock<ICampaignTemplateService>();
            _fakeSearchService = new Mock<ISearchService>();
            _fakeBuyerService = new Mock<IBuyerService>();
            _fakeCampaignRepo = new Mock<IRepository<Campaign>>();
            _fakeBuyerRepo = new Mock<IRepository<Buyer>>();
            _fakeUserService = new Mock<IUserService>();
            _fakeRoleService = new Mock<IRoleService>();
            _fakeAppContext = new Mock<IAppContext>();
            _fakeCountryService = new Mock<ICountryService>();
            _fakePermissionService = new Mock<IPermissionService>();
            _fakePlanService = new Mock<IPlanService>();
            _fakeAffiliateService = new Mock<IAffiliateService>();
            _fakeAffiliateChannelService = new Mock<IAffiliateChannelService>();
            _fakeSettingService = new Mock<ISettingService>();
            _fakeEmailService = new Mock<IEmailService>();
            _fakeSmtpAccountServiceService = new Mock<ISmtpAccountService>();
            _entityChangeHistoryService = new Mock<IEntityChangeHistoryService>();
        }

        private BuyerChannelController CreateBuyerChannelController()
        {
            var controller = new BuyerChannelController(_fakeBuyerChannelService.Object,
                                                        _fakeBuyerChannelTemplateService.Object,
                                                        _fakeBuyerChannelTemplateMatchingService.Object,
                                                        _fakeBuyerChannelFilterConditionService.Object,
                                                        _fakeBuyerChannelScheduleService.Object,
                                                        _fakeCampaignService.Object,
                                                        _fakeCampaignTemplateService.Object,
                                                        _fakeSearchService.Object,
                                                        _fakeBuyerService.Object,
                                                        _fakeCampaignRepo.Object,
                                                        _fakeBuyerRepo.Object,
                                                        _fakeUserService.Object,
                                                        _fakeAppContext.Object,
                                                        _fakeRoleService.Object,
                                                        _fakeCountryService.Object,
                                                        _fakePermissionService.Object,
                                                        _fakePlanService?.Object,
                                                        _fakeAffiliateService.Object,
                                                        _fakeAffiliateChannelService.Object,
                                                        _fakeSettingService.Object,
                                                        _fakeEmailService.Object,
                                                        _fakeSmtpAccountServiceService.Object,
                                                        _entityChangeHistoryService.Object
                                                        )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
            return controller;
        }


        #region GetBuyerChannelById

        [Fact]
        public void GetBuyerChannelById_CallsBuyerChannelServiceGetBuyerChannelByIdOnce()
        {
            //Setup
            var controller = CreateBuyerChannelController();
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);

            //Act
            controller.GetBuyerChannelById(BuyerChannelId);


            //Assert
            _fakeBuyerChannelService.Verify(mock => mock.GetBuyerChannelById(BuyerChannelId, It.IsAny<bool>()), Times.Once);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(),Core.Domain.Security.PermissionState.Access)).Returns(true);
        }


        [Fact]
        public void GetBuyerChannelById_NonExistingBuyerChannel_ReturnsNotFoundStatusCode()
        {
            //Setup
            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns<BuyerChannel>(null);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();

            //Act
            var httpActionResult = controller.GetBuyerChannelById(BuyerChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }




        [Fact]
        public void GetBuyerChannelById_ExistingBuyerChannel_ReturnsOkStatusCodeWithBuyerChannel()
        {
            //Setup
            var expectedBuyerChannel = new BuyerChannel
            {
                Id = BuyerChannelId
            };

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(BuyerChannelId, It.IsAny<bool>()))
                .Returns(expectedBuyerChannel);
            _fakeBuyerChannelService
            .Setup(mock => mock.GetBuyerChannelScheduleDays(BuyerChannelId, It.IsAny<short>(), It.IsAny<bool>()))
            .Returns(new List<BuyerChannelScheduleDay>());
            _fakeUserService.Setup(x => x.GetEntityOwnership(It.IsAny<string>(), It.IsAny<long>())).Returns(new
                EditableList<EntityOwnership>());
            _fakeBuyerChannelTemplateService
                .Setup(mock => mock.GetAllBuyerChannelTemplatesByBuyerChannelId(It.IsAny<long>()))
                .Returns(new List<BuyerChannelTemplate>());
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();


            //Act
            var contentResult = controller.GetBuyerChannelById(BuyerChannelId) as OkNegotiatedContentResult<BuyerChannelModel>;


            //Assert
           Assert.Equal(expectedBuyerChannel.Id, contentResult?.Content.Id);
        }


        [Fact]
        public void GetBuyerChannelById_BuyerChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Throws<Exception>();

            var controller = CreateBuyerChannelController();


            //Act
            var httpActionResult = controller.GetBuyerChannelById(BuyerChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            var errorViewModel = httpResponseMessage.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel).Status);

        }

        #endregion

        #region AddBuyerChannel

        [Fact]
        public void AddBuyerChannel_ValidModel_CallsBuyerChannelServiceInsertBuyerChannelOnce()
        {
            //Setup
            var controller = CreateBuyerChannelController();
            controller.ModelState.Clear();
            var buyerChannelCreateModel = new BuyerChannelModel
            {
                BuyerChannelFields = new List<BuyerChannelIntegrationModel>(),
                BuyerChannelFilters = new List<BuyerChannelFilterCreateModel>(),
                BuyerChannelSchedules = new List<BuyerChannelScheduleDayModel>()
            };
            _fakeCampaignService.Setup(x => x.GetCampaignById(It.IsAny<long>(), false)).Returns(new Campaign());
            _fakeBuyerService.Setup(x => x.GetBuyerById(It.IsAny<long>(), false)).Returns(new Buyer());
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(),Core.Domain.Security.PermissionState.Access)).Returns(true);

            //Act
            controller.CreateBuyerChannel(buyerChannelCreateModel);

            //Assert
            //_fakeBuyerChannelService.Verify(mock => mock.InsertBuyerChannel(It.IsAny<BuyerChannel>()), Times.Once);
        }

        [Fact]
        public void AddBuyerChannel_ValidModel_ReturnsCreatedAtStatusCode()
        {
            //Setup
            _fakeBuyerChannelService
                .Setup(mock => mock.InsertBuyerChannel(It.IsAny<BuyerChannel>()))
                .Returns(BuyerChannelId);
            _fakeCampaignService.Setup(x => x.GetCampaignById(It.IsAny<long>(), false)).Returns(new Campaign());
            _fakeBuyerService.Setup(x => x.GetBuyerById(It.IsAny<long>(), false)).Returns(new Buyer());
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();
            controller.ModelState.Clear();
            var createBuyerChannelModel = new BuyerChannelModel
            {
                BuyerChannelFields = new List<BuyerChannelIntegrationModel>(),
                BuyerChannelFilters = new List<BuyerChannelFilterCreateModel>(),
                BuyerChannelSchedules = new List<BuyerChannelScheduleDayModel>()
            };

            //Act
            var httpActionResult = controller.CreateBuyerChannel(createBuyerChannelModel);
            var httpResponseMessage = httpActionResult as OkNegotiatedContentResult<BuyerChannelModel>;

            //Assert
            //Assert.Equal(createBuyerChannelModel, httpResponseMessage?.Content);
        }


        [Fact]
        public void AddBuyerChannel_InvalidModel_ReturnsBadRequestStatusCodeWithErrorMessage()
        {
            //Setup
            var controller = CreateBuyerChannelController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");
            var createBuyerChannelModel = new BuyerChannelModel();

            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Act
            var httpActionResult = controller.CreateBuyerChannel(createBuyerChannelModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var errorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;


            //Assert
            var errorViewModel = httpResponseMessage.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel).Status);
            Assert.Equal("{\"Status\":400,\"Message\":\"testErrorMessage1, testErrorMessage2\",\"Errors\":[]}", errorMessage);
        }

        [Fact]
        public void AddBuyerChannel_BuyerChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeBuyerChannelService
                .Setup(mock => mock.InsertBuyerChannel(It.IsAny<BuyerChannel>()))
                .Throws<Exception>();

            var controller = CreateBuyerChannelController();
            var createBuyerChannelModel = new BuyerChannelModel();


            //Act
            var httpActionResult = controller.CreateBuyerChannel(createBuyerChannelModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel= httpResponseMessage.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel).Status);
        }

        [Fact]
        public void AddBuyerChannel_NestedBuyerChannelFilters_CallsBuyerChannelFilterConditionServiceForEachFilter()
        {
            //Setup
            _fakeBuyerChannelFilterConditionService
                .Setup(mock => mock.InsertFilterCondition(It.IsAny<BuyerChannelFilterCondition>()))
                .Returns(200);
            _fakeCampaignService.Setup(x => x.GetCampaignById(It.IsAny<long>(), false)).Returns(new Campaign());
            _fakeBuyerService.Setup(x => x.GetBuyerById(It.IsAny<long>(), false)).Returns(new Buyer());
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);

            var controller = CreateBuyerChannelController();

            var createBuyerChannelModel = new BuyerChannelModel
            {
                BuyerChannelFields = new List<BuyerChannelIntegrationModel>(),
                BuyerChannelSchedules = new List<BuyerChannelScheduleDayModel>(),
                BuyerChannelFilters = new List<BuyerChannelFilterCreateModel>
                {
                    new BuyerChannelFilterCreateModel
                    {
                        CampaignFieldId =1,
                        Condition = 1,
                        Values = new List<Models.Campaigns.FilterConditionValueModel>()
                    },
                    new BuyerChannelFilterCreateModel
                    {
                        CampaignFieldId =1,
                        Condition = 1,
                        Values = new List<Models.Campaigns.FilterConditionValueModel>()
                    }
                }
            };

            //Act
            controller.CreateBuyerChannel(createBuyerChannelModel);

            //Assert
            //_fakeBuyerChannelFilterConditionService.Verify(mock => mock.InsertFilterCondition(It.IsAny<BuyerChannelFilterCondition>()), Times.Exactly(2));
        }

        [Fact]
        public void AddBuyerChannel_LeadSchedule_CallsLeadScheduleServiceInsertLeadScheduleForEachItem()
        {
            //Setup
            var controller = CreateBuyerChannelController();
            _fakeCampaignService.Setup(x => x.GetCampaignById(It.IsAny<long>(), false)).Returns(new Campaign());
            _fakeBuyerService.Setup(x => x.GetBuyerById(It.IsAny<long>(), false)).Returns(new Buyer());
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var createBuyerChannelModel = new BuyerChannelModel
            {
                BuyerChannelFields = new List<BuyerChannelIntegrationModel>(),
                BuyerChannelFilters = new List<BuyerChannelFilterCreateModel>(),
                BuyerChannelSchedules = new List<BuyerChannelScheduleDayModel>()
                {
                    new BuyerChannelScheduleDayModel(),
                    new BuyerChannelScheduleDayModel()
                }
            };

            //Act
            controller.CreateBuyerChannel(createBuyerChannelModel);

            //Assert
            //_fakeBuyerChannelService.Verify(mock => mock.InsertBuyerChannelScheduleDay(It.IsAny<BuyerChannelScheduleDay>()), Times.Exactly(2));
        }


        #endregion

        #region DeleteBuyerChannel

        [Fact]
        public void DeleteBuyerChannel_ExistingBuyerChannel_CallsBuyerChannelServiceDeleteBuyerChannelOnce()
        {
            //Setup
            var buyerChannel = new BuyerChannel
            {
                Id = BuyerChannelId
            };

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(BuyerChannelId, It.IsAny<bool>()))
                .Returns(buyerChannel);
            _fakeBuyerChannelService
               .Setup(mock => mock.DeleteBuyerChannel(buyerChannel));

            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();


            //Act
            controller.DeleteBuyerChannel(BuyerChannelId);

            //Assert
            _fakeBuyerChannelService.Verify(mock => mock.UpdateBuyerChannel(buyerChannel), Times.Once);
        }

        [Fact]
        public void DeleteBuyerChannel_ExistingBuyerChannel_ReturnsOkStatusCode()
        {
            //Setup
            var buyerChannel = new BuyerChannel
            {
                Id = BuyerChannelId
            };
            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(BuyerChannelId, It.IsAny<bool>()))
                .Returns(buyerChannel);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();


            //Act
            var httpActionResult = controller.DeleteBuyerChannel(BuyerChannelId);
            var resultHttpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            Assert.Equal(HttpStatusCode.OK, resultHttpResponseMessage.StatusCode);
        }

        [Fact]
        public void DeleteBuyerChannel_NonExistingBuyerChannel_ReturnsNotFoundStatusCode()
        {
            //Setup
            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns<BuyerChannel>(null);
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = CreateBuyerChannelController();


            //Act
            var httpActionResult = controller.DeleteBuyerChannel(BuyerChannelId);
            var resultHttpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            Assert.Equal(HttpStatusCode.NotFound, resultHttpResponseMessage.StatusCode);
        }

        [Fact]
        public void DeleteBuyerChannel_BuyerChannelServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            var buyerChannel = new BuyerChannel
            {
                Id = BuyerChannelId
            };
            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(BuyerChannelId, It.IsAny<bool>()))
                .Returns(buyerChannel);

            _fakeBuyerChannelService
                .Setup(mock => mock.DeleteBuyerChannel(It.IsAny<BuyerChannel>()))
                .Throws<Exception>();

            var controller = CreateBuyerChannelController();


            //Act
            var httpActionResult = controller.DeleteBuyerChannel(BuyerChannelId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            var errorViewModel = httpResponseMessage.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel).Status);

        }

        #endregion
    }
}