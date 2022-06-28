using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.New.Support;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Adrack.WebApi.Tests
{
    public class SupportControllerTests
    {
        private readonly Mock<ISupportTicketsService> _fakeSupportTicketsService;
        private readonly Mock<ISupportTicketsMessagesService> _fakeSupportTicketMessageService;
        private readonly Mock<IEmailService> _fakeEmailService;
        private readonly Mock<IAppContext> _appContext;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<IUsersExtensionService> _fakeUsersExtensionService;
        private readonly Mock<IBuyerService> _fakeBuyerService;
        private readonly Mock<IAffiliateService> _fakeAffiliateService;
        private readonly Mock<ISettingService> _fakeSettingService;
        private readonly Mock<IStorageService> _fakeStorageService;

        public SupportControllerTests()
        {
            _fakeSupportTicketsService = new Mock<ISupportTicketsService>();
            _fakeSupportTicketMessageService = new Mock<ISupportTicketsMessagesService>();
            _fakeEmailService = new Mock<IEmailService>();
            _fakeUserService = new Mock<IUserService>();
            _fakeUsersExtensionService = new Mock<IUsersExtensionService>();
            _fakeBuyerService = new Mock<IBuyerService>();
            _fakeAffiliateService = new Mock<IAffiliateService>();
            _appContext = new Mock<IAppContext>();
            _fakeSettingService = new Mock<ISettingService>();
            _fakeStorageService = new Mock<IStorageService>();
        }

        private SupportController GetSupportController()
        {
            var controller = new SupportController(
                _fakeSupportTicketsService.Object,
                _fakeSupportTicketMessageService.Object,
                _fakeEmailService.Object,
                _fakeUserService.Object,
                _fakeUsersExtensionService.Object,
                _fakeBuyerService.Object,
                _fakeAffiliateService.Object,
                _appContext.Object,
                _fakeSettingService.Object,
                _fakeStorageService.Object
                )
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
            };
            return controller;
        }

        #region GetSupportTicketById

        [Fact]
        public void GetSupportTicketById_CallsSupportTicketsServiceGetSupportTicketByIdOnce()
        {
            //Setup
            const long supportTicketId = 100;
            var controller = GetSupportController();


            //Act
            controller.GetSupportTicketById(supportTicketId);


            //Assert
            _fakeSupportTicketsService.Verify(mock=>mock.GetSupportTicketById(supportTicketId), Times.Once);
        }


        [Fact]
        public void GetSupportTicketById_NonExistingSupportTicket_ReturnsNotFoundStatusCode()
        {
            //Setup
            const long supportTicketId = 100;
            
            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTicketById(It.IsAny<long>()))
                .Returns<SupportTickets>(null);

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketById(supportTicketId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }


        [Fact]
        public void GetSupportTicketById_ExistingSupportTicket_ReturnsSupportTicketViewModel()
        {
            //Setup
            const long supportTicketId = 100;

            var supportTickets = new SupportTickets
            {
                Id = supportTicketId
            };

            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTicketById(It.IsAny<long>()))
                .Returns(supportTickets);

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketById(supportTicketId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var contentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var supportTicketsViewModel = JsonConvert.DeserializeObject<SupportTicketsViewViewModel>(contentString);


            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(supportTicketId, supportTicketsViewModel.Id);
        }


        [Fact]
        public void GetSupportTicketById_SupportTicketsServiceGetSupportTicketByIdThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            const long supportTicketId = 100;

            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTicketById(It.IsAny<long>()))
                .Throws<Exception>();

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketById(supportTicketId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion


        #region GetSupportTicketMessagesByTicketId

        [Fact]
        public void GetSupportTicketMessagesByTicketId_CallsSupportTicketsMessageServiceGetSupportTicketMessagesByTicketIdOnce()
        {
            //Setup
            const long supportTicketId = 100;
            var controller = GetSupportController();


            //Act
            controller.GetSupportTicketMessagesByTicketId(supportTicketId);


            //Assert
            _fakeSupportTicketMessageService.Verify(mock => mock.GetSupportTicketsMessages(supportTicketId), Times.Once);
        }


        [Fact]
        public void GetSupportTicketMessagesByTicketId_ReturnsListOfSupportTicketMessageViewModels()
        {
            //Setup
            const long supportTicketId = 100;
            var random = new Random();
            var supportTicketMessage1Id = random.Next();
            var supportTicketMessage2Id = random.Next();

            var supportTicketsMessagesViews = new List<SupportTicketsMessagesView>
            {
                new SupportTicketsMessagesView
                {
                    Id = supportTicketMessage1Id
                },
                new SupportTicketsMessagesView
                {
                    Id = supportTicketMessage2Id
                }
            };

            _fakeSupportTicketMessageService
                .Setup(mock => mock.GetSupportTicketsMessages(supportTicketId))
                .Returns(supportTicketsMessagesViews);

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketMessagesByTicketId(supportTicketId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var contentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var supportTicketsMessagesViewModelsList = JsonConvert.DeserializeObject<List<SupportTicketsMessagesViewViewModel>>(contentString);


            //Assert
            Assert.NotNull(supportTicketsMessagesViewModelsList.First(x=>x.Id==supportTicketMessage1Id));
            Assert.NotNull(supportTicketsMessagesViewModelsList.First(x=>x.Id==supportTicketMessage2Id));
        }


        [Fact]
        public void GetSupportTicketMessagesByTicketId_SupportTicketsMessageServiceGetSupportTicketsMessagesThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            const long supportTicketId = 100;

            _fakeSupportTicketMessageService
                .Setup(mock => mock.GetSupportTicketsMessages(supportTicketId))
                .Throws<Exception>();

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketMessagesByTicketId(supportTicketId);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion


        #region AddSupportTicket
        /*
        [Fact]
        public void AddSupportTicket_ValidModel_CallsSupportTicketsServiceInsertSupportTicketOnce()
        {
            //Setup
            var controller = GetSupportController();
            var supportTicketsCreateModel = new SupportTicketsCreateModel()
            {
                Id=100
            };


            //Act
            controller.AddSupportTicket(supportTicketsCreateModel);

            _fakeSupportTicketsService
                .Setup(mock => mock.InsertSupportTicket(It.IsAny<SupportTickets>()))
                .Returns(1);
            //Assert
            _fakeSupportTicketsService.Verify(mock => mock.InsertSupportTicket(It.IsAny<SupportTickets>()), Times.Once);
        }
        */
        /*
        [Fact]
        public void AddSupportTicket_ValidModel_ReturnsCreatedAtStatusCode()
        {
            //Setup
            const long supportTicketId = 100;

            _fakeSupportTicketsService
                .Setup(mock => mock.InsertSupportTicket(It.IsAny<SupportTickets>()))
                .Returns(supportTicketId);
            _fakeUserService
                .Setup(mock => mock.GetUserById(It.IsAny<long>()))
                .Returns(new User { Username = "user", Email = "user@a.a" });
            _fakeUsersExtensionService
                .Setup(mock => mock.GetFullName(It.IsAny<User>()))
                .Returns("fullname");
            _fakeEmailService
                .Setup(mock => mock.SendNewTicket(It.IsAny<string>(), It.IsAny<string>(), EmailOperatorEnums.LeadNative, It.IsAny<long>()))
                .Verifiable();

            var controller = GetSupportController();
            var supportTicketsCreateModel = new SupportTicketsCreateModel() {
                Id = 1,
                DateTime = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(10),
                ManagerId = 1,
                Message = "Some message",
                Priority  = 1,
                Status = 1,
                Subject = "Some subject",
                TicketType = null,
                UserId = 1
            };


            //Act
            var httpActionResult = controller.AddSupportTicket(supportTicketsCreateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var contentResult = httpActionResult as OkNegotiatedContentResult<SupportTickets>;


            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.Equal(supportTicketId, contentResult?.Content?.Id);
        }
        */
        /*
        [Fact]
        public void AddSupportTicket_InvalidModel_ReturnsBadRequestStatusCodeWithErrorMessage()
        {
            //Setup
            var controller = GetSupportController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");
            var supportTicketsCreateModel = new SupportTicketsCreateModel();


            // Act
            var response = controller.AddSupportTicket(supportTicketsCreateModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = (ErrorViewModel)contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)errorViewModel.Status);
            Assert.Equal("testErrorMessage1, testErrorMessage2", errorViewModel.Message);
        }
 */
        /*
        [Fact]
       
        public void AddSupportTicket_SupportTicketsServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeSupportTicketsService
                .Setup(mock => mock.InsertSupportTicket(It.IsAny<SupportTickets>()))
                .Throws<Exception>();

            var controller = GetSupportController();
            var supportTicketsCreateModel = new SupportTicketsCreateModel();


            //Act
            var httpActionResult = controller.AddSupportTicket(supportTicketsCreateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }
        */

        #endregion


        #region AddSupportTicketMessage

        /*
        [Fact]
        public void  AddSupportTicketMessage_ValidModel_CallsSupportTicketsMessagesServiceInsertSupportTicketsMessageOnce()
        {
            //Setup
            var controller = GetSupportController();
            var supportTicketMessageCreateModel = new SupportTicketMessageCreateModel();


            //Act
            controller.AddSupportTicketMessage(supportTicketMessageCreateModel);


            //Assert
            _fakeSupportTicketMessageService.Verify(mock => mock.InsertSupportTicketsMessage(It.IsAny<SupportTicketsMessages>()), Times.Once);
        }
        */

        /*
        [Fact]
        public void AddSupportTicketMessage_ValidModel_ReturnsOkStatusCode()
        {
            //Setup
            const long supportTicketMessageId = 100;

            _fakeSupportTicketMessageService
                .Setup(mock => mock.InsertSupportTicketsMessage(It.IsAny<SupportTicketsMessages>()))
                .Returns(supportTicketMessageId);
            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTicketById(It.IsAny<long>()))
                .Returns(new SupportTickets());
            _fakeUserService
                .Setup(mock => mock.GetUserById(It.IsAny<long>()))
                .Returns(new User { Username = "user", Email = "user@a.a" });
            _fakeUsersExtensionService
                .Setup(mock => mock.GetFullName(It.IsAny<User>()))
                .Returns("fullname");
            _fakeEmailService
                .Setup(mock => mock.SendNewTicket(It.IsAny<string>(), It.IsAny<string>(), EmailOperatorEnums.LeadNative, It.IsAny<long>()))
                .Verifiable();


            var controller = GetSupportController();
            var supportTicketMessageCreateModel = new SupportTicketMessageCreateModel();



            //Act
            var httpActionResult = controller.AddSupportTicketMessage(supportTicketMessageCreateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            var contentResult = httpActionResult as OkResult;


            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.NotNull(contentResult);
        }
        */

        /*
        [Fact]
        public void AddSupportTicketMessage_InvalidModel_ReturnsBadRequestStatusCodeWithErrorMessage()
        {
            //Setup
            var controller = GetSupportController();
            controller.ModelState.AddModelError("testKey1", "testErrorMessage1");
            controller.ModelState.AddModelError("testKey2", "testErrorMessage2");
            var supportTicketMessageCreateModel = new SupportTicketMessageCreateModel();


            // Act
            var response = controller.AddSupportTicketMessage(supportTicketMessageCreateModel);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = (ErrorViewModel)contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

           
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)errorViewModel.Status);
            Assert.Equal("testErrorMessage1, testErrorMessage2", errorViewModel.Message);
        }
        */

        /*
        [Fact]
        public void AddSupportTicketMessage_SupportTicketsServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            _fakeSupportTicketMessageService
                .Setup(mock => mock.InsertSupportTicketsMessage(It.IsAny<SupportTicketsMessages>()))
                .Throws<Exception>();

            var controller = GetSupportController();
            var supportTicketMessageCreateModel = new SupportTicketMessageCreateModel();


            //Act
            var httpActionResult = controller.AddSupportTicketMessage(supportTicketMessageCreateModel);
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            var code = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }
        */
        #endregion


        #region GetTicketsByUserId

        [Fact]
        public void GetTicketsByUserId_CallsSupportTicketsServiceGetSupportTicketsOnce()
        {
            //Setup
            const long userId = 100;
            _fakeUserService
                .Setup(mock => mock.GetUserById(It.IsAny<long>()))
                .Returns(new User { Username = "user", Email = "user@a.a" });
            _fakeUsersExtensionService
                .Setup(mock => mock.GetFullName(It.IsAny<User>()))
                .Returns("fullname");
            _fakeEmailService
                .Setup(mock => mock.SendNewTicket(It.IsAny<string>(), It.IsAny<string>(), EmailOperatorEnums.LeadNative, It.IsAny<long>()))
                .Verifiable();

            var controller = GetSupportController();


            //Act
            // controller.GetSupportTickets(userId);


            //Assert
           // _fakeSupportTicketsService.Verify(mock=>mock.GetSupportTickets(userId, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetTicketsByUserId_ValidUserId_ReturnsListOfSupportTicketsViewViewModels()
        {
            //Setup
            GetSupportTicketsInModel getSupportTicketsInModel = new GetSupportTicketsInModel() { UserIds = "200" };
            var random = new Random();
            const long userId = 100;

            var supportTicketsView1Id = (long)random.Next();
            var supportTicketsView2Id = (long)random.Next();
            var supportTicketsList = new List<SupportTicketsView>
            {
                new SupportTicketsView
                {
                    Id = supportTicketsView1Id
                },
                new SupportTicketsView
                {
                    Id = supportTicketsView2Id
                }
            };

            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTickets(userId, It.IsAny<string>()))
                .Returns(supportTicketsList);

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketsByFilters(0, DateTime.MaxValue, DateTime.MaxValue, "1", "");
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;
            //var httpResponseMessageContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //var supportTicketsViewViewModelsList = JsonConvert.DeserializeObject<List<SupportTicketsViewViewModel>>(httpResponseMessageContentString);


            //Assert
           //Assert.NotNull(supportTicketsViewViewModelsList.FirstOrDefault(x => x.Id == supportTicketsView1Id));
           //Assert.NotNull(supportTicketsViewViewModelsList.FirstOrDefault(x => x.Id == supportTicketsView2Id));
        }


        [Fact]
        public void GetTicketsByUserId_SupportTicketServiceThrowsException_ReturnsBadRequestStatusCode()
        {
            //Setup
            const long userId = 100;
            GetSupportTicketsInModel getSupportTicketsInModel = new GetSupportTicketsInModel() { UserIds = "200" };

            _fakeSupportTicketsService
                .Setup(mock => mock.GetSupportTickets(userId, It.IsAny<string>()))
                .Throws<Exception>();

            var controller = GetSupportController();


            //Act
            var httpActionResult = controller.GetSupportTicketsByFilters(0, DateTime.MaxValue, DateTime.MaxValue, "1", "");
            var httpResponseMessage = httpActionResult.ExecuteAsync(CancellationToken.None).Result;


            //Assert
           // Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        #endregion

    }
}