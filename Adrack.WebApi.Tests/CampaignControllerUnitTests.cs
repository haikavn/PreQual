using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Lead;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using Adrack.Service.Content;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Core.Services;
using Adrack.WebApi.Models.BaseModels;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Threading;
using System.Net;
using Adrack.WebApi.Models;
using Newtonsoft.Json;
using Adrack.Service.Membership;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for CampaignControllerUnitTests
    /// </summary>
    public class CampaignControllerUnitTests
    {
        #region properties

        private readonly Mock<ICampaignService> _mockCampaignService;
        private readonly Mock<IVerticalService> _mockVerticalService;
        private readonly Mock<ICampaignTemplateService> _mockCampaignTemplateService;
        private readonly Mock<IAffiliateChannelService> _mockAffiliateChannelService;
        private readonly Mock<IBuyerChannelService> _mockBuyerChannelService;
        private readonly Mock<IFilterService> _mockFilterService;
        private readonly Mock<ISearchService> _mockSearchService;
        private readonly Mock<IPermissionService> _mockPermissionService;

        private readonly Mock<IAppContext> _mockAppContext;
        private readonly Mock<IPlanService> _mockPlanService;

        private readonly Mock<IReportService> _mockReportService;
        private readonly Mock<IEntityChangeHistoryService> _entityChangeHistoryService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IUserRegistrationService> _userRegistrationService;


        #endregion

        #region constructors

        public CampaignControllerUnitTests()
        {
            _mockCampaignService = new Mock<ICampaignService>();
            _mockVerticalService = new Mock<IVerticalService>();
            _mockCampaignTemplateService = new Mock<ICampaignTemplateService>();
            _mockAffiliateChannelService = new Mock<IAffiliateChannelService>();
            _mockBuyerChannelService = new Mock<IBuyerChannelService>();
            _mockFilterService = new Mock<IFilterService>();
            _mockSearchService = new Mock<ISearchService>();
            _mockPermissionService = new Mock<IPermissionService>();
            _mockReportService = new Mock<IReportService>();
            _entityChangeHistoryService = new Mock<IEntityChangeHistoryService>();
            _userService = new Mock<IUserService>();
            _userRegistrationService = new Mock<IUserRegistrationService>();

        }

        #endregion

        #region test methods

        #region Get Campaign By Id

        [Fact]
        public void GetCampaignById_ExistingCampaign_ReturnsOkResultWithCampaign()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object,
                                                    null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignById(id);
            var contentResult = response as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetCampaignById_NonExistingCampaign_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object,
                                                    null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            
            var deleteResult = controller.GetCampaignById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no campaign was found for given id {id}");
        }

        [Fact]
        public void GetCampaignById_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.GetCampaignById(It.IsAny<int>(), It.IsAny<bool>())).
                                                                                Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Get Campaign List 

        [Fact]
        public void GetCampaignList_ReturnsOkResultWithCampaign()
        {
            // Arrange
            var verticalsList = new List<Vertical> { new Vertical { Id = 1, Name = "vert1" }, new Vertical { Id = 2, Name = "vert2" } };

            var campaignList = new List<Campaign> { new Campaign { Id = 1, Status = ActivityStatuses.Active, Name = "name1", VerticalId = 1 }, new Campaign { Id = 2, Status = ActivityStatuses.Active, Name = "name2", VerticalId = 2 } };

            _mockCampaignService.Setup(x => x.GetAllCampaignsByStatus(0, -1)).Returns(campaignList);
            _mockVerticalService.Setup(x => x.GetAllVerticals()).Returns(verticalsList);
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object,
                                                    _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var actionResult = controller.GetCampaignList(ActivityStatuses.All);
            var contentResult = actionResult as OkNegotiatedContentResult<List<CampaignListModel>>;

            // Assert
            //Assert.IsAssignableFrom<List<CampaignListModel>>(contentResult?.Content);
        }

        [Fact]
        public void GetCampaignList_CorrectCount_ReturnsOkResultWithCampaignList()
        {
            // Arrange
            var verticalsList = new List<Vertical> { new Vertical { Id = 1, Name = "vert1" }, new Vertical { Id = 2, Name = "vert2" } };

            var campaignList = new List<Campaign> { new Campaign { Id = 1, Status = ActivityStatuses.Active, Name = "name1", VerticalId = 1 }, new Campaign { Id = 2, Status = ActivityStatuses.Active, Name = "name2", VerticalId = 2 } };

            _mockCampaignService.Setup(x => x.GetAllCampaignsByStatus(0, -1)).Returns(campaignList);
            _mockVerticalService.Setup(x => x.GetAllVerticals()).Returns(verticalsList);
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object,
                                                    _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var actionResult = controller.GetCampaignList(ActivityStatuses.All);
            var contentResult = actionResult as OkNegotiatedContentResult<List<CampaignListModel>>;

            //Assert.Equal(campaignList.Count, contentResult?.Content.Count());
        }

        #endregion

        #region Insert Campaign

        [Fact]
        public void InsertCampaign_CorrectCampaignModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new AddCampaignModel
            {
                Id = 0,
                Name = "new campaign",
                CampaignType = 0
            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.InsertCampaign(campaign)).Returns(id);
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            controller.InsertCampaign(campaignModel);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertCampaign_CorrectCampaignModelWithFields_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new AddCampaignModel
            {
                Id = 0,
                Name = "new campaign",
                CampaignType = 0,
                CampaignFields = new List<CampaignFieldModel>
                {
                    new CampaignFieldModel
                    {
                    Id = 0,
                    CampaignId = 0,
                    TemplateField = "new field"
                    }
                }
            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.InsertCampaign(campaign)).Returns(id);
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.InsertCampaign(campaignModel);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertCampaign_WithNullModel_ReturnsBadRequest()
        {
            // Arrange

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var response = controller.InsertCampaign(null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"campaign model is null");
        }

        [Fact]
        public void InsertCampaign_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockCampaignService.Setup(x => x.InsertCampaign(It.IsAny<Campaign>())).Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.InsertCampaign(It.IsAny<AddCampaignModel>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Update Campaign

        [Fact]
        public void UpdateCampaign_CorrectCampaignModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new CampaignModel
            {
                Id = id,
                Name = "new campaign",
                CampaignType = 0
            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.UpdateCampaign(campaign));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.UpdateCampaign(id, campaignModel);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void UpdateCampaign_CorrectCampaignModelWithFields_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new CampaignModel
            {
                Id = id,
                Name = "new campaign",
                CampaignType = 0,
                CampaignFields = new List<CampaignFieldModel>
                {
                    new CampaignFieldModel
                    {
                    Id = 11,
                    CampaignId = id,
                    TemplateField = "new field"
                    }
                }
            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.UpdateCampaign(campaign));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.UpdateCampaign(id, campaignModel);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void UpdateCampaign_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.UpdateCampaign(id, null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"campaign model is null for given id {id}");
        }

        [Fact]
        public void UpdateCampaign_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.UpdateCampaign(It.IsAny<Campaign>())).Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.UpdateCampaign(id, It.IsAny<CampaignModel>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Soft Delete Campaign

        [Fact]
        public void SoftDeleteCampaign_ExistingCampaignReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new CampaignModel
            {
                Id = id,
                Name = "new campaign",
                CampaignType = 0            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.UpdateCampaign(campaign));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id, IsDeleted = true });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.SoftDeleteCampaign(id);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void SoftDeleteCampaign_CheekIsDeleted_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignModel = new CampaignModel
            {
                Id = id,
                Name = "new campaign",
                CampaignType = 0
            };
            var campaign = (Campaign)campaignModel;
            _mockCampaignService.Setup(x => x.UpdateCampaign(campaign));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).
                                        Returns(new Campaign { Id = id, IsDeleted = true });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.SoftDeleteCampaign(id);
            var actionResult = controller.GetCampaignById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Campaign>;

            // Assert
            Assert.Equal(true, contentResult?.Content.IsDeleted);
        }

        [Fact]
        public void SoftDeleteCampaign_CampaignNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false));

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var response = controller.SoftDeleteCampaign(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no campaign was found for given id {id}");
        }

        [Fact]
        public void SoftDeleteCampaign_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.UpdateCampaign(It.IsAny<Campaign>())).Throws<Exception>();
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign());

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.SoftDeleteCampaign(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Delete Campaign

        [Fact]
        public void DeleteCampaign_ExistingCampaignReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>()));
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0));
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaign(id);
            var deleteContentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            // Assert
            //Assert.NotNull(deleteContentResult);
        }

        [Fact]
        public void DeleteCampaign_CampaignNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>()));
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0));
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no campaign field was found for given id {id}");
        }

        [Fact]
        public void DeleteCampaign_CampaignContainFields_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            var campaign = new Campaign
            {
                Id = 0,
                Name = "new campaign",
                CampaignType = 0,
                CampaignFields = new List<CampaignField>
                {
                    new CampaignField
                    {
                    Id = 0,
                    CampaignId = 0,
                    TemplateField = "new field"
                    }
                }
            };
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>())).Throws<Exception>();
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0));
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(campaign);

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.NotEmpty(((ErrorViewModel)errorViewModel)?.Message);
        }

        [Fact]
        public void DeleteCampaign_CampaignWithAffiliateChannels_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            var affiliateChannels = new List<AffiliateChannel>
            {
                new AffiliateChannel
                {
                    Id = 1, CampaignId = id
                }
            };
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>()));
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0)).Returns(affiliateChannels);
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal($"no campaign field was found for given id {id}", ((ErrorViewModel)errorViewModel).Message);
        }

        [Fact]
        public void DeleteCampaign_CampaignWithBuyerChannels_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            var buyerChannels = new List<BuyerChannel>
            {
                new BuyerChannel
                {
                    Id = 1, CampaignId = id
                }
            };
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>()));
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0));
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0)).Returns(buyerChannels);
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false)).Returns(new Campaign { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaign(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            //Assert.Equal($"can not delete campaign because there are active buyer channels", ((ErrorViewModel)errorViewModel).Message);
        }

        [Fact]
        public void DeleteCampaign_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignService.Setup(x => x.DeleteCampaign(It.IsAny<Campaign>())).Throws<Exception>();
            _mockAffiliateChannelService.Setup(x => x.GetAllAffiliateChannelsByCampaignId(id, 0));
            _mockBuyerChannelService.Setup(x => x.GetAllBuyerChannelsByCampaignId(id, 0));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, false));

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel)?.Status);
        }

        #endregion

        #region Get Campaign By Vertical Id

        [Fact]
        public void GetCampaignsByVerticalId_ExistingCampaigns_ReturnsOkResultWithCampaigns()
        {
            // Arrange
            const long id = 1;
            var campaignList = new List<Campaign>
            {
                new Campaign
                {
                    Id = id
                }
            };
            _mockCampaignService.Setup(x => x.GetCampaignsByVerticalId(id, 0)).Returns(campaignList);
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignsByVerticalId(id);
            var contentResult = response as OkNegotiatedContentResult<IList<Campaign>>;

            Assert.Equal(campaignList.Count, contentResult?.Content.Count());
        }

        [Fact]
        public void GetCampaignsByVerticalId_CampaignServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockCampaignService.Setup(x => x.GetCampaignsByVerticalId(It.IsAny<long>(), It.IsAny<short>())).
                                                                                Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignsByVerticalId(It.IsAny<long>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Get Campaign Field List By Campaign Id

        [Fact]
        public void GetCampaignFieldListByCampaignId_ExistingCampaign_ReturnsOkResultWithCampaign()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x =>
             x.GetCampaignTemplatesByCampaignId(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<short>())).
                                                                Returns(new List<CampaignField>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignFieldListByCampaignId(id);
            var contentResult = response as OkNegotiatedContentResult<IList<CampaignField>>;

            // Assert
            Assert.IsAssignableFrom<List<CampaignField>>(contentResult?.Content);
        }

        [Fact]
        public void GetCampaignFieldListByCampaignId_CampaignTemplateServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockCampaignTemplateService.Setup(x =>
                x.GetCampaignTemplatesByCampaignId(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<short>())).
                                                                                Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignFieldListByCampaignId(It.IsAny<long>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Get Campaign Field By Id

        [Fact]
        public void GetCampaignFieldById_ExistingCampaignField_ReturnsOkResultWithCampaignField()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false)).
                                                                Returns(new CampaignField { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignFieldById(id);
            var contentResult = response as OkNegotiatedContentResult<CampaignField>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetCampaignFieldById_NonExistingCampaignField_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            var getResult = controller.GetCampaignFieldById(id);
            var contentResult = getResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no campaign field was found for given id {id}");
        }

        [Fact]
        public void GetCampaignFieldById_CampaignTemplateServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(It.IsAny<int>(), It.IsAny<bool>())).
                                                                                Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetCampaignFieldById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Insert Campaign Field

        [Fact]
        public void InsertCampaign_CorrectCampaignFieldModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignFieldModel = new CampaignFieldModel
            {
                Id = 0,
                CampaignId = 1
            };
            var campaignField = (CampaignField)campaignFieldModel;
            _mockCampaignTemplateService.Setup(x => x.InsertCampaignTemplate(campaignField)).Returns(id);
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false)).
                                                                        Returns(new CampaignField { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            controller.InsertCampaignField(campaignFieldModel);
            var actionResult = controller.GetCampaignFieldById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<CampaignField>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertCampaignField_WithNullModel_ReturnsBadRequest()
        {
            // Arrange

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);

            var response = controller.InsertCampaignField(null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"campaign field model is null");

        }

        [Fact]
        public void InsertCampaignField_CampaignTemplateServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockCampaignTemplateService.Setup(x => x.InsertCampaignTemplate(It.IsAny<CampaignField>())).
                                                                                        Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.InsertCampaignField(It.IsAny<CampaignFieldModel>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Update Campaign Field

        [Fact]
        public void UpdateCampaignField_CorrectCampaignFieldModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var campaignFieldModel = new CampaignIntegrationModel
            {
                CampaignField = "test",
                SystemField = "test",
                DataType = 1,
                Description = "test",
                PossibleValue = "test",
                IsRequired = false,
                IsHash = false,
                IsFilterable = false
            };

            _mockCampaignTemplateService.Setup(x => x.UpdateCampaignTemplate(It.IsAny<CampaignField>()));
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false)).
                                                    Returns(new CampaignField { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.UpdateCampaignFieldById(id, campaignFieldModel);
            var actionResult = controller.GetCampaignFieldById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<CampaignField>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void UpdateCampaignField_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false)).
                                                             Returns(new CampaignField { Id = id });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.UpdateCampaignFieldById(id,null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"campaign field model is null for given id {id}");
        }

        [Fact]
        public void UpdateCampaignField_CampaignTemplateServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.UpdateCampaignTemplate(It.IsAny<CampaignField>())).
                                                                                        Throws<Exception>();

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.UpdateCampaignFieldById(id, It.IsAny<CampaignIntegrationModel>());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Delete Campaign

        [Fact]
        public void DeleteCampaignField_ExistingCampaignFieldReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.DeleteCampaignTemplate(It.IsAny<CampaignField>()));
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false)).
                                                                        Returns(new CampaignField { Id = id });

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var deleteContentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            // Assert
            //Assert.NotNull(deleteContentResult);
        }

        [Fact]
        public void DeleteCampaignField_CampaignFieldNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.DeleteCampaignTemplate(It.IsAny<CampaignField>()));
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = new CampaignController(_mockCampaignService.Object,
                                                    _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;
            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no campaign field was found for given id {id}");
        }

        [Fact]
        public void DeleteCampaignField_CampaignTemplateServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockCampaignTemplateService.Setup(x => x.DeleteCampaignTemplate(It.IsAny<CampaignField>())).
                                                                                        Throws<Exception>();
            _mockCampaignTemplateService.Setup(x => x.GetCampaignTemplateById(id, false));

            var controller = new CampaignController(_mockCampaignService.Object, _mockVerticalService.Object,
                                                    _mockCampaignTemplateService.Object,
                                                    _mockAffiliateChannelService.Object,
                                                    _mockBuyerChannelService.Object,
                                                    _mockFilterService.Object,
                                                    _mockSearchService.Object,
                                                    _mockPermissionService.Object,
                                                    _mockAppContext?.Object,
                                                    _mockPlanService?.Object,
                                                    _mockReportService?.Object,
                                                    _userRegistrationService.Object,
                                                    _userService.Object,
                                                    _entityChangeHistoryService.Object, null
                                                    )
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var deleteResult = controller.DeleteCampaignFieldById(id);
            var contentResult = deleteResult.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;
            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)((ErrorViewModel)errorViewModel).Status);
        }

        #endregion

        #endregion
    }
}
