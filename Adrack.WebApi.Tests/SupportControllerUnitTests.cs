using System;
using System.Collections.Generic;
using System.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Lead;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Support;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Localization;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for SettingControllerUnitTests
    /// </summary>
    public class SupportControllerUnitTests
    {
        #region properties

        private string UnreachableCodeExecuted => "Unreachable Code Executed";

        private readonly ITestOutputHelper _output = new TestOutputHelper();

        public FakeContextInitializer FakeContext { get; set; }

        #endregion

        #region constructors

        private SupportControllerUnitTests(FakeContextInitializer fakeContextInitializer)
        {
            // Initialize Engine Context
            FakeContext = fakeContextInitializer;
        }

        public SupportControllerUnitTests()
        : this(new FakeContextInitializer())
        {
            // Initialize Engine Context
        }

        #endregion

        #region test methods

        //[Fact]
        //public void GetSupportTicketsById0UnitTest()
        //{
        //    // Arrange
        //    const long id = 0L;
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(x => x.GetTicketsByTicketId(id))
        //        .Returns((List<TicketMessageViewItem>) null);

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        var result = controller.Get(id);
        //        // Assert
        //        Assert.IsAssignableFrom<TicketsModel>(result);
        //        ticketsModel = result;
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.Empty(ticketsModel.TicketMessageViewItems);
        //}

        //[Fact]
        //public void GetSupportTicketsById1UnitTest()
        //{
        //    // Arrange
        //    const long id = 1L;
        //    var ticketMessageViewItems = new List<TicketMessageViewItem>
        //    {
        //        new TicketMessageViewItem(), new TicketMessageViewItem()
        //    };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(x => x.GetTicketsByTicketId(id)).
        //        Returns(ticketMessageViewItems);
        //    mockAppContext.Setup(x => x.AppUser).
        //        Returns(new User());

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.Get(0L);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.Equal(0L, ticketsModel.TicketMessageViewItems.Count);

        //    // Act
        //    try
        //    {
        //        ticketsModel = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.Equal(ticketMessageViewItems.Count, ticketsModel.TicketMessageViewItems.Count);
        //}

        //[Fact]
        //public void GetTicketsByUserId0UnitTest()
        //{
        //    // Arrange
        //    const long id = 0L;

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(x => x.GetTicketsByTicketId(id))
        //        .Returns((List<TicketMessageViewItem>) null);

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserId(0L);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    Assert.Empty(ticketsModel.TicketMessageViewItems);
        //}

        //[Fact]
        //public void GetTicketsByUserId1UnitTest()
        //{
        //    // Arrange
        //    const long id = 1L;
        //    var ticketMessageViewItems = new List<TicketMessageViewItem>
        //    {
        //        new TicketMessageViewItem(), new TicketMessageViewItem()
        //    };

        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockAppContext.Setup(x => x.AppUser).
        //        Returns(new User() { Id = 2000000000});
        //    mockSupportPageService.Setup(x => x.GetTicketsByUserId(It.IsNotNull<long>(), null, string.Empty)).
        //        Returns(ticketMessageViewItems);

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserId(0L);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    Assert.Equal(ticketMessageViewItems.Count, ticketsModel.TicketMessageViewItems.Count);

        //    // Act
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserId(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    Assert.Equal(ticketMessageViewItems.Count, ticketsModel.TicketMessageViewItems.Count);
        //}

        //[Theory]
        //[InlineData(0L)]
        //[InlineData(null)]
        //public void GetTicketsUserInfoWithEmptySystemUserListValuesUnitTest(long? buyerId)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsUsersInfoModel ticketsUsersInfoModel = null;
        //    try
        //    {
        //        ticketsUsersInfoModel = controller.GetTicketsUserInfo(buyerId);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsUsersInfoModel);
        //    Assert.NotNull(ticketsUsersInfoModel.UsersNameList);
        //    Assert.Null(ticketsUsersInfoModel.ManagerUser);
        //    Assert.Empty(ticketsUsersInfoModel.UsersNameList);
        //}

        //[Theory]
        //[InlineData(0L)]
        //[InlineData(1L)]
        //public void GetTicketsUserInfoWithSystemUserListUnitTest(long id)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockUsersExtensionService.Setup(x => x.GetFullName(It.IsNotNull<User>()))
        //        .Returns((User u) => u.Username);
        //    mockUserService.Setup(x => x.GetSystemUsers(0,0,int.MaxValue))
        //        .Returns(new Pagination<User>(new List<User>
        //        {
        //            new User() { Id = 1, GuId = Guid.NewGuid().ToString(), Username = "Test1", UserTypeId = 1}, 
        //            new User() { Id = 2, GuId = Guid.NewGuid().ToString(), Username = "Test2", UserTypeId = 4}
        //        }, 0, 100));

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object, 
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsUsersInfoModel ticketsUsersInfoModel = null;
        //    try
        //    {
        //        ticketsUsersInfoModel = controller.GetTicketsUserInfo(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsUsersInfoModel);
        //    Assert.NotNull(ticketsUsersInfoModel.UsersNameList);
        //    Assert.Null(ticketsUsersInfoModel.ManagerUser);
        //    Assert.Single(ticketsUsersInfoModel.UsersNameList);
        //}

        //[Theory]
        //[InlineData(1L, 1L)]
        //[InlineData(1L, 2L)]
        //[InlineData(1L, 3L)]
        //[InlineData(1L, 4L)]
        //public void GetTicketsUserInfoWithAppUserWithDifferentUserTypeUnitTest(long id, long userTypeId)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockUsersExtensionService.Setup(x => x.GetFullName(It.IsNotNull<User>()))
        //        .Returns((User u) => u.Username);
        //    mockUserService.Setup(x => x.GetSystemUsers(0, 0, int.MaxValue))
        //        .Returns(new Pagination<User>(new List<User>
        //        {
        //            new User { Id = 1, GuId = Guid.NewGuid().ToString(), Username = "Test1", UserTypeId = 1},
        //            new User { Id = 2, GuId = Guid.NewGuid().ToString(), Username = "Test2", UserTypeId = 4}
        //        }, 0, 100));
        //    mockUserService.Setup(a => a.GetUserById(It.IsAny<long>()))
        //        .Returns(new User { Id = 3, GuId = Guid.NewGuid().ToString(), Username = "Test3", UserTypeId = 2 });
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { UserTypeId = userTypeId, ParentId = 1, Username = "Test1"});
        //    mockAffiliateService.Setup(a => a.GetAffiliateById(It.IsAny<long>(), It.IsAny<bool>()))
        //        .Returns(new Affiliate { ManagerId = 1 });
        //    mockBuyerService.Setup(a => a.GetBuyerById(It.IsAny<long>(), false))
        //        .Returns(new Buyer { ManagerId = 1 });

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsUsersInfoModel ticketsUsersInfoModel = null;
        //    try
        //    {
        //        ticketsUsersInfoModel = controller.GetTicketsUserInfo(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsUsersInfoModel);
        //    Assert.NotNull(ticketsUsersInfoModel.UsersNameList);
        //    Assert.Single(ticketsUsersInfoModel.UsersNameList);
        //    if (userTypeId == 2L || userTypeId == 3L)
        //    {
        //        Assert.NotNull(ticketsUsersInfoModel.ManagerUser);
        //    }
        //    else
        //    {
        //        Assert.Null(ticketsUsersInfoModel.ManagerUser);
        //    }
        //}

        //[Theory]
        //[InlineData(1L)]
        //[InlineData(0L)]
        //public void GetSupportTicketsMessagesUnitTest(long ticketId)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(a => a.GetSupportTicketsMessages(1L))
        //        .Returns(new List<TicketMessageViewItem> { new TicketMessageViewItem { } });
        //    mockSupportPageService.Setup(a => a.GetSupportTicketsMessages(0L))
        //        .Returns((List<TicketMessageViewItem>)null);


        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetSupportTicketsMessages(ticketId);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel); 
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    if (ticketId == 0L) // ticket does not exist 
        //    {
        //        Assert.Empty(ticketsModel.TicketMessageViewItems);
        //    }
        //    else if(ticketId == 1L) // ticket exists
        //    {
        //        Assert.Single(ticketsModel.TicketMessageViewItems);
        //    }
        //}

        //[Fact]
        //public void GetTicketsByStatusWithoutAppUserUnitTest()
        //{
        //    // Arrange
        //    const int ticketStatus = 1;
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByStatus(ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    Assert.Empty(ticketsModel.TicketMessageViewItems);
        //}

        //[Theory]
        //[InlineData(0, 1)]
        //[InlineData(1, 1)]
        //[InlineData(1, 2)]
        //[InlineData(0, 3)]
        //public void GetTicketsByStatusWithNotNetworkAppUserUnitTest(int ticketStatus, long userTypeId)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 0, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem> { new TicketMessageViewItem() });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 1, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem>());
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { UserTypeId = userTypeId, ParentId = 1, Id = 1L, Username = "Test1" });

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByStatus(ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    if (ticketStatus == 0) // ticket exists
        //    {
        //        Assert.Single(ticketsModel.TicketMessageViewItems);
        //    }
        //    else if (ticketStatus == 1) // ticket does not exists
        //    {
        //        Assert.Empty(ticketsModel.TicketMessageViewItems);
        //    }
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(0)]
        //public void GetTicketsByStatusWithNetworkAppUserUnitTest(int ticketStatus)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockUserService.Setup(u => u.GetUsersByParentId(It.IsNotNull<long>(), It.IsNotNull<long>(), 0))
        //        .Returns(new List<User>{ new User {Id = 1}, new User { Id = 2} });
        //    mockBuyerService.Setup(b => b.GetAllBuyers(It.IsNotNull<User>(), 0))
        //        .Returns(new List<Buyer> {new Buyer {Id = 3}, new Buyer {Id = 4}});
        //    mockAffiliateService.Setup(b => b.GetAllAffiliates(It.IsNotNull<User>(), 0))
        //        .Returns(new List<Affiliate> { new Affiliate { Id = 5 }, new Affiliate { Id = 2 } });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 0, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem> { new TicketMessageViewItem() });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 1, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem>());
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { UserTypeId = 4, ParentId = 1, Id = 1L, Username = "Test1" });

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByStatus(ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    if (ticketStatus == 0) // ticket exists
        //    {
        //        Assert.Single(ticketsModel.TicketMessageViewItems);
        //    }
        //    else if (ticketStatus == 1) // ticket does not exists
        //    {
        //        Assert.Empty(ticketsModel.TicketMessageViewItems);
        //    }
        //}

        //[Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //public void GetTicketsByUserAndStatusWithoutAppUserUnitTest(long userId)
        //{
        //    // Arrange
        //    const int ticketStatus = 1;
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserAndStatus(userId, ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    Assert.Empty(ticketsModel.TicketMessageViewItems);
        //}

        //[Theory]
        //[InlineData(1L, 0, 1)]
        //[InlineData(1L, 1, 1)]
        //[InlineData(1L, 1, 2)]
        //[InlineData(1L, 0, 3)]
        //public void GetTicketsByUserAndStatusWithNotNetworkAppUserUnitTest(long userId, int ticketStatus, long userTypeId)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(userId, 0, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem> { new TicketMessageViewItem() });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(userId, 1, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem>());
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { UserTypeId = userTypeId, ParentId = 1, Id = userId, Username = "Test1" });

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserAndStatus(userId, ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    if (ticketStatus == 0) // ticket exists
        //    {
        //        Assert.Single(ticketsModel.TicketMessageViewItems);
        //    }
        //    else if (ticketStatus == 1) // ticket does not exists
        //    {
        //        Assert.Empty(ticketsModel.TicketMessageViewItems);
        //    }
        //}

        //[Theory]
        //[InlineData(1L, 1)]
        //[InlineData(1L, 0)]
        //[InlineData(2L, 1)]
        //[InlineData(2L, 0)]
        //[InlineData(9L, 0)]
        //public void GetTicketsByUserAndStatusWithNetworkAppUserUnitTest(long userId, int ticketStatus)
        //{
        //    // Arrange
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockUserService.Setup(u => u.GetUsersByParentId(It.IsNotNull<long>(), It.IsNotNull<long>(), 0))
        //        .Returns(new List<User> { new User { Id = 1 }, new User { Id = 2 } });
        //    mockBuyerService.Setup(b => b.GetAllBuyers(It.IsNotNull<User>(), 0))
        //        .Returns(new List<Buyer> { new Buyer { Id = 3 }, new Buyer { Id = 4 } });
        //    mockAffiliateService.Setup(b => b.GetAllAffiliates(It.IsNotNull<User>(), 0))
        //        .Returns(new List<Affiliate> { new Affiliate { Id = 5 }, new Affiliate { Id = 2 } });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 0, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem> { new TicketMessageViewItem() });
        //    mockSupportPageService.Setup(a => a.GetTicketsByUserId(It.IsAny<long>(), 1, It.IsAny<string>()))
        //        .Returns(new List<TicketMessageViewItem>());
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { UserTypeId = 4, ParentId = 1, Id = 1L, Username = "Test1" });

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    TicketsModel ticketsModel = null;
        //    try
        //    {
        //        ticketsModel = controller.GetTicketsByUserAndStatus(userId, ticketStatus);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(ticketsModel);
        //    Assert.NotNull(ticketsModel.TicketMessageViewItems);
        //    if (ticketStatus == 0) // ticket exists
        //    {
        //        Assert.Single(ticketsModel.TicketMessageViewItems);
        //    }
        //    else if (ticketStatus == 1) // ticket does not exists
        //    {
        //        Assert.Empty(ticketsModel.TicketMessageViewItems);
        //    }
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //[InlineData(4)]
        //public void PostByAppUserUnitTest(int userTypeId)
        //{
        //    // Arrange
        //    const string sendTo = "2";
        //    const string cc = "test@test.am";
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockUserService.Setup(u => u.GetUserById(It.IsNotNull<long>()))
        //        .Returns(new User { Id = 1 });
        //    mockBuyerService.Setup(b => b.GetBuyerById(It.IsNotNull<long>(), false))
        //        .Returns(new Buyer { Id = 3 }).Verifiable();
        //    mockAffiliateService.Setup(b => b.GetAffiliateById(It.IsNotNull<long>(), true))
        //        .Returns(new Affiliate { Id = 5 }).Verifiable();
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(new User { Id = 1, ParentId = 1, UserTypeId = userTypeId});

        //    mockSupportPageService.Setup(a => a.AddNewSupportTickets(It.IsAny<SupportTickets>(), cc))
        //        .Verifiable();
        //    mockSupportPageService.Setup(a => a.InsertSupportTicketsMessage(It.IsAny<SupportTicketsMessages>()))
        //        .Verifiable();
        //    mockEmailService.Setup(u => u.SendUserNewTicket(It.IsNotNull<User>(), It.IsNotNull<long>()))
        //        .Verifiable();
        //    mockFileUploadService.Setup(u => u.UploadFile(It.IsNotNull<long>()))
        //        .Verifiable();
        //    mockAppContext.Setup(a => a.AppLanguage).Returns(new Language { Id = 4 }).Verifiable();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    TicketAddModel ticketAddModel = new TicketAddModel
        //    {
        //        Cc = cc,
        //        Message = "Test message",
        //        Subject = "Test Topic",
        //        SendTo = sendTo,
        //        ManagerId = string.Empty,
        //        Priority = string.Empty,
        //    };

        //    // Act
        //    try
        //    {
        //        controller.Post(ticketAddModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSupportPageService.VerifyAll();
        //    mockFileUploadService.VerifyAll();
        //    mockEmailService.VerifyAll();
        //    mockAppContext.VerifyAll();
        //    if (userTypeId == 2)
        //    {
        //        mockBuyerService.VerifyAll();
        //    }

        //    if (userTypeId == 3)
        //    {
        //        mockAffiliateService.VerifyAll();
        //    }
        //}

        //[Theory]
        //[InlineData(1L)]
        //[InlineData(2L)]
        //[InlineData(0L)]
        //public void PostTicketsStatusClosedByTicketIdUnitTest(long ticketId)
        //{
        //    // Arrange
        //    const long languageId = 4;
        //    var user = new User {Id = ticketId, ParentId = 1};
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(user);

        //    mockUserService.Setup(u => u.GetUserById(It.IsNotNull<long>()))
        //        .Returns(user)
        //        .Verifiable();
        //    mockSupportPageService.Setup(a => a.ChangeTicketsStatus(ticketId, It.IsAny<int>()))
        //        .Returns((long x, int y) => (x == 0L ? (SupportTickets) null : new SupportTickets {ManagerID = 1L}))
        //        .Verifiable();
        //    mockEmailService.Setup(u => u.SendUserNewTicketMessage(user, languageId))
        //        .Returns(1)
        //        .Verifiable();
        //    mockAppContext.Setup(a => a.AppLanguage)
        //        .Returns(new Language { Id = languageId })
        //        .Verifiable();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    try
        //    {
        //        controller.PostTicketsStatusClosed(ticketId);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSupportPageService.VerifyAll();
        //    if (ticketId == 2L) // OL returns empty items, 1L is the same author id
        //    {
        //        mockEmailService.VerifyAll();
        //        mockAppContext.VerifyAll();
        //        mockUserService.VerifyAll();
        //    }
        //}

        //[Theory]
        //[InlineData(0L)]
        //[InlineData(1L)]
        //[InlineData(2L)]
        //public void PostTicketMessageByTicketIdUnitTest(long ticketId)
        //{
        //    // Arrange
        //    const long languageId = 4;
        //    var user = new User { Id = ticketId, ParentId = 1 };
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    mockSupportPageService.Setup(a => a.InsertSupportTicketsMessage(It.IsAny<SupportTicketsMessages>()))
        //        .Returns(1)
        //        .Verifiable();
        //    mockSupportPageService.Setup(a => a.GetSupportTicketById(ticketId))
        //        .Returns(new SupportTickets() { Id = 1L, ManagerID = 1L })
        //        .Verifiable();

        //    mockUserService.Setup(u => u.GetUserById(It.IsNotNull<long>()))
        //        .Returns(user)
        //        .Verifiable();
        //    mockEmailService.Setup(u => u.SendUserNewTicketMessage(user, languageId))
        //        .Returns(1)
        //        .Verifiable();
        //    mockAppContext.Setup(a => a.AppUser)
        //        .Returns(user);
        //    mockAppContext.Setup(a => a.AppLanguage)
        //        .Returns(new Language { Id = languageId })
        //        .Verifiable();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    var ticketMessagesModel = new TicketMessagesModel
        //    {
        //        Message = "Test message",
        //        FilePath = "Root"
        //    };

        //    // Act
        //    try
        //    {
        //        controller.PostTicketMessage(ticketId, ticketMessagesModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    mockSupportPageService.VerifyAll();
        //    mockSupportPageService.VerifyAll();
        //    if (ticketId == 2L) // OL returns empty items, 1L is the same author id
        //    {
        //        mockEmailService.VerifyAll();
        //        mockAppContext.VerifyAll();
        //        mockUserService.VerifyAll();
        //    }
        //}

        //[Fact]
        //public void PostTicketMessageByNullModelUnitTest()
        //{
        //    // Arrange
        //    const long ticketId = 1L;
        //    TicketMessagesModel supportTicketMessagesModel = null;
        //    var argumentNullException = new ArgumentException(nameof(supportTicketMessagesModel),
        //        $"Support ticket messages model is missing");
        //    var mockAppContext = new Mock<IAppContext>();
        //    var mockAffiliateService = new Mock<IAffiliateService>();
        //    var mockBuyerService = new Mock<IBuyerService>();
        //    var mockEmailService = new Mock<IEmailService>();
        //    var mockFileUploadService = new Mock<IFileUploadService>();
        //    var mockSupportPageService = new Mock<ISupportPageService>();
        //    var mockUsersExtensionService = new Mock<IUsersExtensionService>();
        //    var mockUserService = new Mock<IUserService>();

        //    var controller = new SupportController_OLD(mockAppContext.Object,
        //        mockAffiliateService.Object, mockBuyerService.Object, mockEmailService.Object,
        //        mockFileUploadService.Object, mockSupportPageService.Object,
        //        mockUsersExtensionService.Object, mockUserService.Object);

        //    // Act
        //    try
        //    {
        //        controller.PostTicketMessage(ticketId, supportTicketMessagesModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        Assert.Equal(argumentNullException.Message, e.Message);
        //        //_output.WriteLine(e.Message);
        //        return;
        //    }

        //    // Assert
        //    Assert.True(false, UnreachableCodeExecuted);
        //}

        #endregion
    }
}
