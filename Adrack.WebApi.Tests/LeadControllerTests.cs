using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.New.Lead;
using Moq;
using Newtonsoft.Json;
using Xunit;
using static Adrack.Web.Framework.Security.ContentManagementApiAuthorizeAttribute;

namespace Adrack.WebApi.Tests
{
    public class LeadControllerTests
    {
        private readonly Mock<ISettingService> _fakeSettingsService;
        private readonly Mock<IAppContext> _fakeAppContext;
        private readonly Mock<ILeadMainService> _fakeLeadMainService;
        private readonly Mock<ILeadDemoModeService> _fakeLeadDemoModeService;
        private readonly Mock<IAffiliateService> _fakeAffiliateService;
        private readonly Mock<IAffiliateChannelService> _fakeAffiliateChannelService;
        private readonly Mock<IBuyerService> _fakeBuyerService;
        private readonly Mock<IBuyerChannelService> _fakeBuyerChannelService;
        private readonly Mock<ICampaignService> _fakeCampaignService;
        private readonly Mock<IAffiliateResponseService> _fakeAffiliateResponseService;
        private readonly Mock<ILeadMainResponseService> _fakeLeadMainResponseService;
        private readonly Mock<ILeadContentDublicateService> _fakeContentDuplicateService;
        private readonly Mock<IRedirectUrlService> _fakeRedirectUrlService;
        private readonly Mock<ICampaignTemplateService> _fakeCampaignTemplateService;
        private readonly Mock<IPermissionService> _fakePermissionService;
        private readonly Mock<IReportService> _reportService;

        public LeadControllerTests()
        {
            _fakeSettingsService = new Mock<ISettingService>();
            _fakeAppContext = new Mock<IAppContext>();
            _fakeLeadMainService = new Mock<ILeadMainService>();
            _fakeLeadDemoModeService = new Mock<ILeadDemoModeService>();
            _fakeAffiliateService = new Mock<IAffiliateService>();
            _fakeAffiliateChannelService = new Mock<IAffiliateChannelService>();
            _fakeBuyerService = new Mock<IBuyerService>();
            _fakeBuyerChannelService = new Mock<IBuyerChannelService>();
            _fakeCampaignService = new Mock<ICampaignService>();
            _fakeAffiliateResponseService = new Mock<IAffiliateResponseService>();
            _fakeLeadMainResponseService = new Mock<ILeadMainResponseService>();
            _fakeContentDuplicateService = new Mock<ILeadContentDublicateService>();
            _fakeRedirectUrlService = new Mock<IRedirectUrlService>(); 
            _fakeCampaignTemplateService = new Mock<ICampaignTemplateService>();
            _fakePermissionService = new Mock<IPermissionService>();
            _reportService = new Mock<IReportService>();
        }

        private LeadController GetLeadController()
        {
            var controller = new LeadController(
                _fakeSettingsService.Object,
                _fakeAppContext.Object,
                _fakeLeadMainService.Object,
                _fakeLeadDemoModeService.Object,
                _fakeAffiliateService.Object,
                _fakeAffiliateChannelService.Object,
                _fakeBuyerService.Object,
                _fakeBuyerChannelService.Object,
                _fakeCampaignService.Object,
                _fakeAffiliateResponseService.Object,
                _fakeLeadMainResponseService.Object,
                _fakeContentDuplicateService.Object,
                _fakeRedirectUrlService.Object,
                _fakeCampaignTemplateService.Object,
                _fakePermissionService.Object,
                _reportService.Object
                )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
            return controller;
        }


        #region GetLeads



        [Fact]
        public void GetLeads_CallsLeadMainServiceGetLeadsAllOnce()
        {
            //Setup
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };

            var leadsList = new List<LeadMainContent>();
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<short>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Returns(leadsList);

            var controller = GetLeadController();


            //Act
            controller.GetLeads(leadRequestModel);

            //Assert
            _fakeLeadMainService.Verify(mock => mock.GetLeadsAll(
                leadRequestModel.DateFrom,
                leadRequestModel.DateTo,
                0,
                "",
                string.Join(",", leadRequestModel.FilterAffiliateIds),
                string.Join(",", leadRequestModel.FilterAffiliateChannelIds),
                "",
                string.Join(",", leadRequestModel.FilterBuyerIds),
                string.Join(",", leadRequestModel.FilterBuyerChannelIds),
                string.Join(",", leadRequestModel.FilterCampaignIds),
                leadRequestModel.Status,
                "",
                "",
                "",
                "",
                0,
                "",
                "",
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public void GetLeads_ForEachLeadMainContent_ReturnsNamesOfAffiliates()
        {
            //Setup
            var affiliate1Id = 100;
            const int affiliate2Id = 200;
            const string affiliate1Name = "Name1";
            const string affiliate2Name = "Name2";
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };

            var leadsList = new List<LeadMainContent>
            {
                new LeadMainContent
                {
                    AffiliateId = affiliate1Id
                },
                new LeadMainContent
                {
                    AffiliateId = affiliate2Id
                }
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<short>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(leadsList);

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(affiliate1Id, It.IsAny<bool>()))
                .Returns(new Affiliate { Name = affiliate1Name });

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(affiliate2Id, It.IsAny<bool>()))
                .Returns(new Affiliate { Name = affiliate2Name });

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new AffiliateChannel());

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Buyer());

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new BuyerChannel());

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Campaign());

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<LeadMainContentItemViewModel>>(responseContentString);


            //Assert
            Assert.NotNull(responseObject.FirstOrDefault(x => x.AffiliateName == affiliate1Name));
            Assert.NotNull(responseObject.FirstOrDefault(x => x.AffiliateName == affiliate2Name));
        }

        [Fact]
        public void GetLeads_ForEachLeadMainContent_ReturnsNamesOfAffiliateChannels()
        {
            //Setup
            const int affiliateChannel1Id = 100;
            const int affiliateChannel2Id = 200;
            const string affiliateChannel1Name = "Name1";
            const string affiliateChannel2Name = "Name2";
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };

            var leadsList = new List<LeadMainContent>
            {
                new LeadMainContent
                {
                    AffiliateChannelId = affiliateChannel1Id
                },
                new LeadMainContent
                {
                    AffiliateChannelId = affiliateChannel2Id
                }
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<short>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(leadsList);

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(affiliateChannel1Id, It.IsAny<bool>()))
                .Returns(new AffiliateChannel { Name = affiliateChannel1Name });

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(affiliateChannel2Id, It.IsAny<bool>()))
                .Returns(new AffiliateChannel { Name = affiliateChannel2Name });

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Affiliate());

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Buyer());

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new BuyerChannel());

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Campaign());

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<LeadMainContentItemViewModel>>(responseContentString);


            //Assert
            Assert.NotNull(responseObject.FirstOrDefault(x => x.AffiliateChannelName == affiliateChannel1Name));
            Assert.NotNull(responseObject.FirstOrDefault(x => x.AffiliateChannelName == affiliateChannel2Name));
        }

        [Fact]
        public void GetLeads_ForEachLeadMainContent_ReturnsNamesOfBuyers()
        {
            //Setup
            const int buyer1Id = 100;
            const int buyer2Id = 200;
            const string buyer1Name = "Name1";
            const string buyer2Name = "Name2";
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };
            var leadsList = new List<LeadMainContent>
            {
                new LeadMainContent
                {
                    BuyerId = buyer1Id
                },
                new LeadMainContent
                {
                    BuyerId = buyer2Id
                }
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<short>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(leadsList);

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(buyer1Id, It.IsAny<bool>()))
                .Returns(new Buyer { Name = buyer1Name });

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(buyer2Id, It.IsAny<bool>()))
                .Returns(new Buyer { Name = buyer2Name });

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Affiliate());

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new AffiliateChannel());

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new BuyerChannel());

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Campaign());

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<LeadMainContentItemViewModel>>(responseContentString);


            //Assert
            Assert.NotNull(responseObject.FirstOrDefault(x => x.BuyerName == buyer1Name));
            Assert.NotNull(responseObject.FirstOrDefault(x => x.BuyerName == buyer2Name));
        }

        [Fact]
        public void GetLeads_ForEachLeadMainContent_ReturnsNamesOfBuyerChannels()
        {
            //Setup
            const int buyerChannel1Id = 100;
            const int buyerChannel2Id = 200;
            const string buyerChannel1Name = "Name1";
            const string buyerChannel2Name = "Name2";
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };
            var leadsList = new List<LeadMainContent>
            {
                new LeadMainContent
                {
                    BuyerChannelId = buyerChannel1Id
                },
                new LeadMainContent
                {
                    BuyerChannelId = buyerChannel2Id
                }
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<short>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(leadsList);

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(buyerChannel1Id, It.IsAny<bool>()))
                .Returns(new BuyerChannel { Name = buyerChannel1Name });

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(buyerChannel2Id, It.IsAny<bool>()))
                .Returns(new BuyerChannel { Name = buyerChannel2Name });

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new AffiliateChannel());

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Affiliate());

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Buyer());

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Campaign());

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<LeadMainContentItemViewModel>>(responseContentString);


            //Assert
            Assert.NotNull(responseObject.FirstOrDefault(x => x.BuyerChannelName == buyerChannel1Name));
            Assert.NotNull(responseObject.FirstOrDefault(x => x.BuyerChannelName == buyerChannel2Name));
        }

        [Fact]
        public void GetLeads_ForEachLeadMainContent_ReturnsNamesOfCampaigns()
        {
            //Setup
            const int campaign1Id = 100;
            const int campaign2Id = 200;
            const string campaign1Name = "Name1";
            const string campaign2Name = "Name2";
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };
            var leadsList = new List<LeadMainContent>
            {
                new LeadMainContent
                {
                    CampaignId = campaign1Id
                },
                new LeadMainContent
                {
                    CampaignId = campaign2Id
                }
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<short>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(leadsList);

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(campaign1Id, It.IsAny<bool>()))
                .Returns(new Campaign { Name = campaign1Name });

            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(campaign2Id, It.IsAny<bool>()))
                .Returns(new Campaign { Name = campaign2Name });

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Buyer());

            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new Affiliate());

            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new AffiliateChannel());

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById(It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(new BuyerChannel());

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<List<LeadMainContentItemViewModel>>(responseContentString);


            //Assert
            Assert.NotNull(responseObject.FirstOrDefault(x => x.CampaignName == campaign1Name));
            Assert.NotNull(responseObject.FirstOrDefault(x => x.CampaignName == campaign2Name));
        }

        [Fact]
        public void GetLeads_LeadMainContentServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            var random = new Random();
            var leadRequestModel = new LeadRequestModel
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Status = (short)random.Next(),
                FilterAffiliateIds = new List<long>(),
                FilterAffiliateChannelIds = new List<long>(),
                FilterBuyerIds = new List<long>(),
                FilterBuyerChannelIds = new List<long>(),
                FilterCampaignIds = new List<long>()
            };
            //Setup
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<short>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Throws<Exception>();

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeads(leadRequestModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region GetLeadsCsv

        [Fact]
        public void GetLeadsCsv_CallsLeadMainServiceGetLeadsAllOnce()
        {
            //Setup
            var random = new Random();
            var filterAffiliate = new List<long>();
            var filterAffiliateChannel = new List<long>();
            var filterBuyer = new List<long>();
            var filterBuyerChannel = new List<long>();
            var filterCampaign = new List<long>();
            var dateFromStr = DateTime.Now;
            var dateToStr = DateTime.Now;
            var status = (short)random.Next();

            var leadsList = new List<LeadMainContent>();
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<short>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Returns(leadsList);

            var controller = GetLeadController();
            controller.GetLeadsCsv(
                dateFromStr,
                dateToStr,
                status,
                filterAffiliate,
                filterAffiliateChannel,
                filterBuyer,
                filterBuyerChannel,
                filterCampaign);

            //Assert
            _fakeLeadMainService.Verify(mock => mock.GetLeadsAll(
                dateFromStr,
                dateToStr,
                0,
                "",
                string.Join(",", filterAffiliate),
                string.Join(",", filterAffiliateChannel),
                "",
                string.Join(",", filterBuyer),
                string.Join(",", filterBuyerChannel),
                string.Join(",", filterCampaign),
                status,
                "",
                "",
                "",
                "",
                0,
                "",
                "",
                0,
                int.MaxValue), Times.Once);

        }


        [Fact]
        public void GetLeadsCsv_LoadsCsv()
        {
            //Setup
            var random = new Random();

            var filterAffiliate = new List<long>();
            var filterAffiliateChannel = new List<long>();
            var filterBuyer = new List<long>();
            var filterBuyerChannel = new List<long>();
            var filterCampaign = new List<long>();
            var dateFromStr = DateTime.Now;
            var dateToStr = DateTime.Now;
            var status = (short)random.Next();

            var leadsList = new List<LeadMainContent>();
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService.Setup(mock => mock.GetLeadsAll(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<short>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Returns(leadsList);

            var controller = GetLeadController();


            //Act
            controller.GetLeadsCsv(
                dateFromStr,
                dateToStr,
                status,
                filterAffiliate,
                filterAffiliateChannel,
                filterBuyer,
                filterBuyerChannel,
                filterCampaign);

            //Assert
            _fakeLeadMainService.Verify(mock => mock.GetLeadsAll(
                dateFromStr,
                dateToStr,
                0,
                "",
                string.Join(",", filterAffiliate),
                string.Join(",", filterAffiliateChannel),
                "",
                string.Join(",", filterBuyer),
                string.Join(",", filterBuyerChannel),
                string.Join(",", filterCampaign),
                status,
                "",
                "",
                "",
                "",
                0,
                "",
                "",
                0,
                int.MaxValue), Times.Once);
        }

        #endregion

        #region GetLeadById

        [Fact]
        public void GetLeadById_CallsLeadMainServiceGetLeadMainByIdAndUpdateLeadMainOnce()
        {
            //Setup
            const long leadId = 100;
            var lead = new LeadMain
            {
                Id = leadId
            };
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            _fakeLeadMainService
                .Setup(mock => mock.GetLeadMainById(leadId))
                .Returns(lead);
           
            var controller = GetLeadController();


            //Act
            controller.GetLeadById(leadId);


            //Assert
            _fakeLeadMainService.Verify(mock=>mock.GetLeadMainById(leadId), Times.Once);
            _fakeLeadMainService.Verify(mock=>mock.UpdateLeadMain(lead), Times.Once);
            
        }

        [Fact]
        public void GetLeadById_CallsLeadsMainServiceGetLeadsAllByIdOnce()
        {
            //Setup
            const long leadId = 100;
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = GetLeadController();


            //Act
            controller.GetLeadById(leadId);


            //Assert
            _fakeLeadMainService.Verify(mock => mock.GetLeadsAllById(leadId), Times.Once);
           
        }

        [Fact]
        public void GetLeadById_CallsSubDataServicesOnce()
        {
            //Setup
            const long leadId = 100;
            var random = new Random();
            var leadMainContent = new LeadMainContent
            {
                Id = leadId,
                CampaignId = random.Next(),
                AffiliateId = random.Next(),
                AffiliateChannelId = random.Next(),
                BuyerId = random.Next(),
                BuyerChannelId = random.Next(),
                ReceivedData = "<REQUEST><REFERRAL><CHANNELID>9f0aead</CHANNELID><PASSWORD>q6zvrk3c</PASSWORD><SUBID>-12</SUBID><SUBID2></SUBID2><REFERRINGURL>911payday.com</REFERRINGURL><LEADMINPRICE>2</LEADMINPRICE></REFERRAL><CUSTOMER><PERSONALINFO><IPADDRESS>75.2.92.149</IPADDRESS><REQUESTEDAMOUNT>1406</REQUESTEDAMOUNT><SSN>999999999</SSN><DATEOFBIRTH>09-16-1980</DATEOFBIRTH><FIRSTNAME>John</FIRSTNAME><LASTNAME>Doe</LASTNAME><ADDRESS>123 Main St.</ADDRESS><CITY>Chicago</CITY><STATECODE>IL</STATECODE><ZIPCODE>60610</ZIPCODE><HOMEPHONE>3125558901</HOMEPHONE><CELLPHONE></CELLPHONE><DLSTATE>IL</DLSTATE><DLNUMBER>Qe509454</DLNUMBER><ARMEDFORCES>Yes</ARMEDFORCES><CONTACTTIME></CONTACTTIME><RENTOROWN></RENTOROWN><EMAIL>test@nags.us</EMAIL><ADDRESSLENGHT>107</ADDRESSLENGHT><CITIZENSHIP>Yes</CITIZENSHIP></PERSONALINFO><EMPLOYMENTINFO><INCOMETYPE>Job Income</INCOMETYPE><EMPLENGHT>115</EMPLENGHT><EMPNAME></EMPNAME><EMPPHONE>3125558957</EMPPHONE><JOBTITLE>programmer</JOBTITLE><PAYFREQUENCY>Weekly</PAYFREQUENCY><NEXTPAYDATE>03-01-2016</NEXTPAYDATE><SECONDPAYDATE>03-16-2016</SECONDPAYDATE></EMPLOYMENTINFO><BANKINFO><BANKNAME>US Bank</BANKNAME><BANKPHONE></BANKPHONE><ACCOUNTTYPE>Checking Account</ACCOUNTTYPE><ROUTINGNUMBER>071000013</ROUTINGNUMBER><ACCOUNTNUMBER>78979456</ACCOUNTNUMBER><BANKLENGHT>90</BANKLENGHT><NETMONTHLYINCOME>5000</NETMONTHLYINCOME><DIRECTDEPOSIT>Yes</DIRECTDEPOSIT></BANKINFO></CUSTOMER></REQUEST>"
            };

            _fakeLeadMainService
                .Setup(mock => mock.GetLeadsAllById(leadId))
                .Returns(leadMainContent);
            _fakeAffiliateResponseService.Setup(mock => mock.GetAffiliateResponsesByLeadId(leadId))
                .Returns(new List<AffiliateResponse>());
            _fakeCampaignTemplateService.Setup(mock => mock.CampaignTemplateAllowedNames(It.IsAny<long>()))
                .Returns(new List<string>());
            _fakeLeadMainResponseService.Setup(x => x.GetLeadMainResponseByLeadId(leadId))
                .Returns(new List<LeadResponse>());
            _fakeContentDuplicateService.Setup(x => x.GetLeadContentDublicateBySSN(leadId, ""))
                .Returns(new List<LeadContentDublicate> { new LeadContentDublicate()});
            _fakeAffiliateService.Setup(x => x.GetAffiliateById(It.IsAny<long>(), true)).Returns(new Affiliate());
            _fakeAffiliateChannelService.Setup(x => x.GetAffiliateChannelById(It.IsAny<long>(), false))
                .Returns(new AffiliateChannel());
            var controller = GetLeadController();
            _fakeCampaignService.Setup(x => x.GetCampaignById(It.IsAny<long>(), false))
                .Returns(new Campaign {Id = leadMainContent.CampaignId});
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            //Act
            controller.GetLeadById(leadId);


            //Assert
            _fakeAffiliateService.Verify(mock=>mock.GetAffiliateById(leadMainContent.AffiliateId, It.IsAny<bool>()), Times.Once);
            _fakeAffiliateChannelService.Verify(mock=>mock.GetAffiliateChannelById(leadMainContent.AffiliateChannelId, It.IsAny<bool>()), Times.Once);
            _fakeAffiliateResponseService.Verify(mock=>mock.GetAffiliateResponsesByLeadId(leadId), Times.Once);
            _fakeLeadMainResponseService.Verify(mock=>mock.GetLeadMainResponseByLeadId(leadId), Times.Once);
            _fakeContentDuplicateService.Verify(mock=>mock.GetLeadContentDublicateBySSN(leadId, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetLeadById_BindsSubDataAndReturnsViewModels()
        {
            //Setup
            const long leadId = 100;
            var random = new Random();
            var leadMainContent = new LeadMainContent
            {
                Id = leadId,
                CampaignId = random.Next(),
                AffiliateId = random.Next(),
                AffiliateChannelId = random.Next(),
                BuyerId = random.Next(),
                BuyerChannelId = random.Next(),
                ReceivedData = "<REQUEST><REFERRAL><CHANNELID>9f0aead</CHANNELID><PASSWORD>q6zvrk3c</PASSWORD><SUBID>-12</SUBID><SUBID2></SUBID2><REFERRINGURL>911payday.com</REFERRINGURL><LEADMINPRICE>2</LEADMINPRICE></REFERRAL><CUSTOMER><PERSONALINFO><IPADDRESS>75.2.92.149</IPADDRESS><REQUESTEDAMOUNT>1406</REQUESTEDAMOUNT><SSN>999999999</SSN><DATEOFBIRTH>09-16-1980</DATEOFBIRTH><FIRSTNAME>John</FIRSTNAME><LASTNAME>Doe</LASTNAME><ADDRESS>123 Main St.</ADDRESS><CITY>Chicago</CITY><STATECODE>IL</STATECODE><ZIPCODE>60610</ZIPCODE><HOMEPHONE>3125558901</HOMEPHONE><CELLPHONE></CELLPHONE><DLSTATE>IL</DLSTATE><DLNUMBER>Qe509454</DLNUMBER><ARMEDFORCES>Yes</ARMEDFORCES><CONTACTTIME></CONTACTTIME><RENTOROWN></RENTOROWN><EMAIL>test@nags.us</EMAIL><ADDRESSLENGHT>107</ADDRESSLENGHT><CITIZENSHIP>Yes</CITIZENSHIP></PERSONALINFO><EMPLOYMENTINFO><INCOMETYPE>Job Income</INCOMETYPE><EMPLENGHT>115</EMPLENGHT><EMPNAME></EMPNAME><EMPPHONE>3125558957</EMPPHONE><JOBTITLE>programmer</JOBTITLE><PAYFREQUENCY>Weekly</PAYFREQUENCY><NEXTPAYDATE>03-01-2016</NEXTPAYDATE><SECONDPAYDATE>03-16-2016</SECONDPAYDATE></EMPLOYMENTINFO><BANKINFO><BANKNAME>US Bank</BANKNAME><BANKPHONE></BANKPHONE><ACCOUNTTYPE>Checking Account</ACCOUNTTYPE><ROUTINGNUMBER>071000013</ROUTINGNUMBER><ACCOUNTNUMBER>78979456</ACCOUNTNUMBER><BANKLENGHT>90</BANKLENGHT><NETMONTHLYINCOME>5000</NETMONTHLYINCOME><DIRECTDEPOSIT>Yes</DIRECTDEPOSIT></BANKINFO></CUSTOMER></REQUEST>"
            };

            _fakeLeadMainService
                .Setup(mock => mock.GetLeadsAllById(leadId))
                .Returns(leadMainContent);
            _fakeLeadMainService.Setup(mock => mock.GetNextPrevLeadId(leadId))
                .Returns(new long[]{0,2});

            _fakeAffiliateResponseService.Setup(mock => mock.GetAffiliateResponsesByLeadId(leadId))
                .Returns(new List<AffiliateResponse>());
            _fakeCampaignTemplateService.Setup(mock => mock.CampaignTemplateAllowedNames(It.IsAny<long>()))
                .Returns(new List<string>());
            _fakeLeadMainResponseService.Setup(x => x.GetLeadMainResponseByLeadId(leadId))
                .Returns(new List<LeadResponse>());
            _fakeContentDuplicateService.Setup(x => x.GetLeadContentDublicateBySSN(leadId, ""))
                .Returns(new List<LeadContentDublicate>());
            _fakeCampaignService
                .Setup(mock => mock.GetCampaignById(leadMainContent.CampaignId, It.IsAny<bool>()))
                .Returns(new Campaign{Id = leadMainContent.CampaignId});
            
            _fakeAffiliateService
                .Setup(mock => mock.GetAffiliateById(leadMainContent.AffiliateId, It.IsAny<bool>()))
                .Returns(new Affiliate{Id = leadMainContent.AffiliateId});
            
            _fakeAffiliateChannelService
                .Setup(mock => mock.GetAffiliateChannelById(leadMainContent.AffiliateChannelId, It.IsAny<bool>()))
                .Returns(new AffiliateChannel{Id = leadMainContent.AffiliateChannelId});
            
            _fakeAffiliateResponseService
                .Setup(mock => mock.GetAffiliateResponsesByLeadId(leadMainContent.Id))
                .Returns(new List<AffiliateResponse>{new AffiliateResponse()});

            _fakeBuyerService
                .Setup(mock => mock.GetBuyerById((long) leadMainContent.BuyerId, It.IsAny<bool>()))
                .Returns(new Buyer{Id = (long)leadMainContent.BuyerId});

            _fakeBuyerChannelService
                .Setup(mock => mock.GetBuyerChannelById((long) leadMainContent.BuyerChannelId, It.IsAny<bool>()))
                .Returns(new BuyerChannel{Id = (long)leadMainContent.BuyerChannelId});

            _fakeLeadMainResponseService
                .Setup(mock => mock.GetLeadMainResponseByLeadId(leadMainContent.Id))
                .Returns(new List<LeadResponse>());

            _fakeContentDuplicateService
                .Setup(mock => mock.GetLeadContentDublicateBySSN(leadMainContent.Id, It.IsAny<string>()))
                .Returns(new List<LeadContentDublicate> {new LeadContentDublicate()});
            _fakePermissionService.Setup(x => x.Authorize(It.IsAny<string>(), Core.Domain.Security.PermissionState.Access)).Returns(true);
            var controller = GetLeadController();
            

            //Act
            var httpActionResult = controller.GetLeadById(leadId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var leadMainContentDetailsViewModel = JsonConvert.DeserializeObject<LeadItemModel>(responseContentString);

            //Assert
            Assert.NotNull(leadMainContentDetailsViewModel.AffiliateResponses);
            Assert.NotEmpty(leadMainContentDetailsViewModel.AffiliateResponses);

            Assert.NotNull(leadMainContentDetailsViewModel.LeadJourneys);
            Assert.NotEmpty(leadMainContentDetailsViewModel.LeadJourneys);

            Assert.NotNull(leadMainContentDetailsViewModel.LeadDuplicateMonitors);
            Assert.NotEmpty(leadMainContentDetailsViewModel.LeadDuplicateMonitors);
        }

        [Fact]
        public void GetLeadById_LeadMainServiceGetLeadsAllByIdReturnsNull_ReturnsNotFoundStatusCode()
        {
            //Setup
            const long leadId = 100;

            _fakeLeadMainService
                .Setup(mock => mock.GetLeadsAllById(It.IsAny<long>()))
                .Returns<LeadMainContent>(null);

            _fakeRedirectUrlService.Setup(x => x.GetRedirectUrlByLeadId(It.IsAny<long>()));
            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeadById(leadId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        [Fact]
        public void GetLeadById_LeadMainServiceGetLeadsAllByIdThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            const long leadId = 100;

            _fakeLeadMainService
                .Setup(mock => mock.GetLeadsAllById(It.IsAny<long>()))
                .Throws<Exception>();

            var controller = GetLeadController();


            //Act
            var httpActionResult = controller.GetLeadById(leadId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }


        #endregion

    }
}
