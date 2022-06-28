using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Adrack.WebApi.Controllers;
using System.Web.Http;
using Adrack.Core.Domain.Content;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.WebApi.Infrastructure.Core.DTOs;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.History;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for HistoryControllerUnitTests
    /// </summary>
    public class HistoryControllerUnitTests
    {
        #region properties

        private string UnreachableCodeExecuted => "Unreachable Code Executed";

        private readonly ITestOutputHelper _output = new TestOutputHelper();

        public FakeContextInitializer FakeContext { get; set; }

        #endregion

        #region constructors

        private HistoryControllerUnitTests(FakeContextInitializer fakeContextInitializer)
        {
            // Initialize Engine Context
            FakeContext = fakeContextInitializer;
        }

        public HistoryControllerUnitTests()
        : this(new FakeContextInitializer())
        {
            // Initialize Engine Context
        }

        #endregion

        #region test methods

        //[Fact]
        //public void GetReturnsCorrectHistoryEntityModelTypeById()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");

        //    var historyPageService = AppEngineContext.Current.Resolve<IHistoryPageService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var controller = new HistoryController(historyPageService, settingService)
        //    {
        //        Request = new HttpRequestMessage(), 
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    try
        //    {
        //        var response = controller.Get(id);
        //        // Assert
        //        Assert.IsAssignableFrom<HistoryEntityModel>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);  
        //    }
        //}

        //[Fact]
        //public void GetHistoryByIdWithExistedValue()
        //{
        //    // Arrange
        //    const int id = 1;

        //    var historyPageService = AppEngineContext.Current.Resolve<IHistoryPageService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var controller = new HistoryController(historyPageService, settingService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    HistoryEntityModel historyEntity = null;
        //    try
        //    {
        //        historyEntity = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, $"Shouldn't throw an exception {e.Message}");
        //    }
            
        //    // Assert
        //    Assert.NotNull(historyEntity);
        //}

        //[Fact]
        //public void GeHistoryByIdWithNegativeValue()
        //{
        //    // Arrange
        //    const int id = -1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");

        //    var historyPageService = AppEngineContext.Current.Resolve<IHistoryPageService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var controller = new HistoryController(historyPageService, settingService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    HistoryEntityModel historyEntity = null;
        //    try
        //    {
        //        historyEntity = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }

        //    // Assert
        //    Assert.Null(historyEntity);
        //}

        //[Fact]
        //public void GetHistoryByIdWithZeroValue()
        //{
        //    // Arrange
        //    const int id = 0;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");

        //    var historyPageService = AppEngineContext.Current.Resolve<IHistoryPageService>();
        //    var settingService = AppEngineContext.Current.Resolve<ISettingService>();
        //    var controller = new HistoryController(historyPageService, settingService)
        //    {
        //        Request = new HttpRequestMessage(),
        //        Configuration = new HttpConfiguration()
        //    };

        //    // Act
        //    HistoryEntityModel historyEntity = null;
        //    try
        //    {
        //        historyEntity = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }

        //    // Assert
        //    Assert.Null(historyEntity);
        //}

        //[Fact]
        //public void GetHistoryByIdWithCorrectValue()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(id)).Returns(historyEntity);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);


        //    // Act
        //    HistoryEntityModel historyEntityModel = null;
        //    try
        //    {
        //        historyEntityModel = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, $"Shouldn't throw an exception {e.Message}");
        //    }

        //    // Assert
        //    Assert.NotNull(historyEntityModel);
        //}

        //[Fact]
        //public void GetHistoryByIdWithIncorrectValue()
        //{
        //    // Arrange
        //    const int id = -1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(id)).Returns((HistoryEntityModel)null).Callback(() => throw notFoundException);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryEntityModel historyEntityModel = null;
        //    try
        //    {
        //        historyEntityModel = controller.Get(id);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.Equal(notFoundException.Message, e.Message);
        //    }

        //    // Assert
        //    Assert.Null(historyEntityModel);
        //}

        //[Fact]
        //public void GetReturnsCorrectHistoryEntityModelType()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(id)).Returns(historyEntity);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryEntityModel historyEntityModel = null;
        //    try
        //    {
        //        historyEntityModel = controller.Get(id);
        //        // Assert
        //        Assert.IsAssignableFrom<HistoryEntityModel>(historyEntityModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, $"Shouldn't throw an exception {e.Message}");
        //    }

        //    // Assert
        //    Assert.NotNull(historyEntityModel);
        //}

        //[Fact]
        //public void GetHistoryReturnsCorrectType()
        //{
        //    // Arrange
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);
        //    var historyEntityList = new List<HistoryEntityModel> {historyEntity};
        //    var historyModel = new HistoryModel(historyEntityList);
        //    var historyFilterModel = new HistoryFilterModel
        //    {
        //        Dates = string.Join(";", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString()),
        //        Action = "0",
        //        EntityId = "0",
        //        PageSize = "100",
        //        PageStart = "1",
        //        UserId = "0"
        //    };

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(It.IsAny<HistoryFilterDto>())).Returns(historyModel);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryModel historyResult = null;
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //        // Assert
        //        Assert.IsAssignableFrom<HistoryModel>(historyModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, $"Shouldn't throw an exception {e.Message}");
        //    }

        //    // Assert
        //    Assert.NotNull(historyResult);
        //}

        //[Fact]
        //public void GetListFailedIssueWithIncorrectFilterDates()
        //{
        //    // Arrange
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);
        //    var historyEntityList = new List<HistoryEntityModel> { historyEntity };
        //    var historyModel = new HistoryModel(historyEntityList);
        //    var correctDates = string.Join(";", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
        //    var historyFilterModel = new HistoryFilterModel
        //    {
        //        Dates = null,
        //    };

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(It.IsAny<HistoryFilterDto>())).Returns(historyModel);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryModel historyResult = null;
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<ArgumentNullException>(e);
        //    }
            
        //    // Assert
        //    Assert.NotNull(historyResult);

        //    // Arrange
        //    historyResult = null;
        //    historyFilterModel.Dates = "a;a";

        //    // Act
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<FormatException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyResult = null;
        //    historyFilterModel.Dates = correctDates;

        //    // Act
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        _output.WriteLine(e.Message);
        //        Assert.True(false, UnreachableCodeExecuted);
        //    }

        //    // Assert
        //    Assert.NotNull(historyResult);
        //}

        //[Fact]
        //public void GetListFailedIssueWithIncorrectFilterIds()
        //{
        //    // Arrange
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);
        //    var historyEntityList = new List<HistoryEntityModel> { historyEntity };
        //    var historyModel = new HistoryModel(historyEntityList);
        //    var historyFilterModel = new HistoryFilterModel
        //    {
        //        Dates = string.Join(";", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString()),
        //        Action = null,
        //        EntityId = null,
        //        UserId = null
        //    };

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(It.IsAny<HistoryFilterDto>())).Returns(historyModel);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryModel historyResult = null;
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<ArgumentNullException>(e);
        //    }

        //    // Assert
        //    Assert.NotNull(historyResult);

        //    // Arrange
        //    historyResult = null;
        //    historyFilterModel.Action = "123456789123456789";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<OverflowException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyFilterModel.Action = "0";
        //    historyFilterModel.EntityId = "123456789123456789123456789";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<OverflowException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyFilterModel.EntityId = "0";
        //    historyFilterModel.UserId = "123456789123456789123456789";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<OverflowException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyFilterModel.UserId = "0";
        //    historyFilterModel.Action = "aaa";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<FormatException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyFilterModel.Action = "0";
        //    historyFilterModel.EntityId = "aaa";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<FormatException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);

        //    // Arrange
        //    historyFilterModel.EntityId = "0";
        //    historyFilterModel.UserId = "aaa";
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<FormatException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);
        //}

        //[Fact]
        //public void GetListFailedIssueWithIncorrectFilterData()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);
        //    var historyEntityList = new List<HistoryEntityModel> { historyEntity };
        //    var historyModel = new HistoryModel(historyEntityList);
        //    var historyFilterModel = new HistoryFilterModel
        //    {
        //        Dates = null,
        //        Action = null,
        //        EntityId = null,
        //        UserId = null
        //    };

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockHistoryPageService.Setup(x => x.GetHistory(It.IsAny<HistoryFilterDto>())).
        //        Returns(historyModel).
        //        Callback(() => throw notFoundException);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryModel historyResult = null;
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<ArgumentNullException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);
        //}

        //[Fact]
        //public void GetListFailedIssueWithIncorrectFilterTimeZoneDateFormatIssue()
        //{
        //    // Arrange
        //    const int id = 1;
        //    var notFoundException = new ArgumentNullException(nameof(id), $"there is no history record with id equals to {id}");
        //    var fakeHistory = new History();
        //    var historyEntity = new HistoryEntityModel(fakeHistory);
        //    var historyEntityList = new List<HistoryEntityModel> { historyEntity };
        //    var historyModel = new HistoryModel(historyEntityList);
        //    var historyFilterModel = new HistoryFilterModel
        //    {
        //        Dates = null,
        //        Action = null,
        //        EntityId = null,
        //        UserId = null
        //    };

        //    var mockHistoryPageService = new Mock<IHistoryPageService>();
        //    var mockSettingService = new Mock<ISettingService>();
        //    mockSettingService.Setup(x => x.GetTimeZoneDate(It.IsAny<DateTime>(), null, null, 0)).
        //        Returns(DateTime.Now).
        //        Callback(() => throw new FormatException());
        //    mockHistoryPageService.Setup(x => x.GetHistory(It.IsAny<HistoryFilterDto>())).
        //        Returns(historyModel).
        //        Callback(() => throw notFoundException);

        //    var controller = new HistoryController(mockHistoryPageService.Object, mockSettingService.Object);

        //    // Act
        //    HistoryModel historyResult = null;
        //    try
        //    {
        //        historyResult = controller.Get(historyFilterModel);
        //    }
        //    catch (Exception e)
        //    {
        //        // Assert
        //        //_output.WriteLine(e.Message);
        //        Assert.IsAssignableFrom<FormatException>(e);
        //    }

        //    // Assert
        //    Assert.Null(historyResult);
        //}
        #endregion
    }
}
