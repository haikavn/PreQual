using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Lead;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Models.Vertical;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for VerticalControllerUnitTests
    /// </summary>
    public class VerticalControllerUnitTests
    {
        #region Fields

        private readonly Mock<IVerticalService> _mockVerticalService;
        private readonly Mock<ICampaignService> _mockCampaignService;

        private string Finance => "Finance";
        #endregion

        #region Ctor

        public VerticalControllerUnitTests()
        {
            _mockVerticalService = new Mock<IVerticalService>();
            _mockCampaignService = new Mock<ICampaignService>();
        }

        #endregion

        #region Tests 

        #region Get Vertical and VerticalField By Id

        [Fact]
        public void GetVerticalById_ExistingVertical_ReturnsOkResultWithVertical()
        {
            // Arrange
            const int id = 4;
            _mockVerticalService.Setup(x => x.GetVerticalById(id)).Returns(new Vertical { Id = id });
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetVerticalById(id);
            var contentResult = response as OkNegotiatedContentResult<Vertical>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetVerticalById_NotFoundVertical_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalById(id));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetVerticalById(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical was found for given id {id}");
        }

        [Fact]
        public void GetVerticalByIdWithZero_NotFoundVertical_ReturnsBadRequest()
        {
            // Arrange
            const int id = 0;
            _mockVerticalService.Setup(x => x.GetVerticalById(id));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetVerticalById(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical was found for given id {id}");
        }

        [Fact]
        public void GetVerticalFieldById_ExistingVertical_ReturnsOkResultWithVertical()
        {
            // Arrange
            const int id = 1;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(id)).Returns(new VerticalField() { Id = id });
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetVerticalFieldById(id);
            var contentResult = response as OkNegotiatedContentResult<VerticalField>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetVerticalFieldById_NotFoundVertical_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(id));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            // Act
            var response = controller.GetVerticalFieldById(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical field was found for given id {id}");
        }

        [Fact]
        public void GetVerticalFieldByIdWithZero_NotFoundVertical_ReturnsBadRequest()
        {
            // Arrange
            const int id = 0;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(id));
            _mockCampaignService.Setup(x => x.GetCampaignById(id, true)).Returns(new Campaign { Id = id });

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetVerticalFieldById(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical field was found for given id {id}");
        }

        #endregion

        #region Get Vertical and VerticalField List

        [Fact]
        public void GetVerticalList_Success_ReturnsOkResultWithVerticalList()
        {
            // Arrange
            var vertical1 = new Vertical { Id = 1, Name = Finance, VerticalFields = new List<VerticalField>() };
            var vertical2 = new Vertical { Id = 2, Name = Finance, VerticalFields = new List<VerticalField>() };
            var verticalCollection = new List<Vertical> { vertical1, vertical2 };
            var campaignCollection = new List<Campaign> { new Campaign(), new Campaign() };

            _mockVerticalService.Setup(x => x.GetAllVerticals()).Returns(verticalCollection);
            _mockCampaignService.Setup(x => x.GetAllCampaigns(0)).Returns(campaignCollection);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var actionResult = controller.GetVerticalList();
            var contentResult = actionResult as OkNegotiatedContentResult<List<VerticalListModel>>;

            // Assert
            Assert.IsAssignableFrom<List<VerticalListModel>>(contentResult?.Content);
        }

        [Fact]
        public void GetVerticalList_CorrectCount_ReturnsOkResultWithVerticalList()
        {
            // Arrange
            var vertical1 = new Vertical { Id = 1, Name = Finance, VerticalFields = new List<VerticalField>() };
            var vertical2 = new Vertical { Id = 2, Name = Finance, VerticalFields = new List<VerticalField>() };
            var verticalCollection = new List<Vertical> { vertical1, vertical2 };
            var campaignCollection = new List<Campaign> { new Campaign(), new Campaign() };

            _mockVerticalService.Setup(x => x.GetAllVerticals()).Returns(verticalCollection);
            _mockCampaignService.Setup(x => x.GetAllCampaigns(0)).Returns(campaignCollection);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var actionResult = controller.GetVerticalList();
            var contentResult = actionResult as OkNegotiatedContentResult<List<VerticalListModel>>;

            Assert.Equal(verticalCollection.Count, contentResult?.Content.Count());
        }

        [Fact]
        public void GetVerticalFieldList_Success_ReturnsOkResultWithVerticalFieldList()
        {
            // Arrange
            var verticalField1 = new VerticalField() { Id = 1, Name = "Field1", DataType = "Type1", IsRequired = false, VerticalId = 1 };
            var verticalField2 = new VerticalField() { Id = 2, Name = "Field2", DataType = "Type2", IsRequired = true, VerticalId = 1 };
            var verticalFieldCollection = new List<VerticalField> { verticalField2, verticalField1 };
            var campaignCollection = new List<Campaign> { new Campaign(), new Campaign() };

            _mockVerticalService.Setup(x => x.GetAllVerticalFields()).Returns(verticalFieldCollection);
            _mockCampaignService.Setup(x => x.GetAllCampaigns(0)).Returns(campaignCollection);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var actionResult = controller.GetVerticalFieldList();
            var contentResult = actionResult as OkNegotiatedContentResult<IList<VerticalField>>;

            // Assert
            Assert.IsAssignableFrom<List<VerticalField>>(contentResult?.Content);
        }

        [Fact]
        public void GetVerticalFieldList_CorrectCount_ReturnsOkResultWithVerticalFieldList()
        {
            // Arrange
            var verticalField1 = new VerticalField() { Id = 1, Name = "Field1", DataType = "Type1", IsRequired = false, VerticalId = 1 };
            var verticalField2 = new VerticalField() { Id = 2, Name = "Field2", DataType = "Type2", IsRequired = true, VerticalId = 1 };
            var verticalFieldCollection = new List<VerticalField> { verticalField2, verticalField1 };
            var campaignCollection = new List<Campaign> { new Campaign(), new Campaign() };

            _mockVerticalService.Setup(x => x.GetAllVerticalFields()).Returns(verticalFieldCollection);
            _mockCampaignService.Setup(x => x.GetAllCampaigns(0)).Returns(campaignCollection);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var actionResult = controller.GetVerticalFieldList();
            var contentResult = actionResult as OkNegotiatedContentResult<IList<VerticalField>>;

            Assert.Equal(verticalFieldCollection.Count, contentResult?.Content.Count());
        }
        #endregion

        #region Insert Vertical and Fields

        [Fact]
        public void InsertVertical_CorrectVerticalModel_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalModel = new VerticalModel
            {
                Name = "Finance",
                /*Type = (byte)VerticalType.Financial,
                IsDeleted = false,
                Fields = new List<VerticalFieldModel> {new VerticalFieldModel()
                {
                    VerticalId = 0,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }*/
            };
            var vertical = new Vertical()
            {
                Id = 1,
                Name = "Finance",
                VerticalFields = new List<VerticalField> {new VerticalField
                {
                    Id = 1,
                    VerticalId = 1,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }
            };

            _mockVerticalService.Setup(x => x.GetVerticalById(1)).Returns(vertical);
            _mockVerticalService.Setup(x => x.InsertVertical(new Vertical())).Callback(() => vertical = null);
            _mockCampaignService.Setup(x => x.GetCampaignById(1, false)).Returns(new Campaign());

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            controller.AddVertical(verticalModel);
            var actionResult = controller.GetVerticalById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Vertical>;

            // Assert
            Assert.Equal(vertical, contentResult?.Content);
        }

        [Fact]
        public void InsertVertical_WithNullModel_ReturnsBadRequest()
        {
            // Arrange

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.AddVertical(null);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"vertical model is null");
        }

        [Fact]
        public void InsertVerticalField_CorrectVerticalFieldModel_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalFieldModel = new VerticalFieldModel()
            {
                VerticalId = 0,
                Name = "ChanelID1",
                DataType = "Type1",
                Description = null,
                IsRequired = false
            };
            var verticalField = new VerticalField
            {
                Id = 1,
                VerticalId = 1,
                Name = "ChanelID1",
                DataType = "Type1",
                Description = null,
                IsRequired = false
            };

            _mockVerticalService.Setup(x => x.GetVerticalFieldById(1)).Returns(verticalField);
            _mockVerticalService.Setup(x => x.InsertVerticalField(new VerticalField())).Callback(() => verticalField = null);

            var controller = new VerticalController(_mockVerticalService.Object, null);

            var result = controller.InsertVerticalField(verticalFieldModel);
            var actionResult = controller.GetVerticalFieldById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<VerticalField>;

            // Assert
            Assert.Equal(verticalField, contentResult?.Content);
        }

        [Fact]
        public void InsertVerticalField_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.InsertVerticalField(null);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"vertical field model is null");
        }
        #endregion

        #region Update Vertical and Fields

        [Fact]
        public void UpdateVertical_CorrectVerticalModel_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalModel = new VerticalModel
            {
                Name = "Finance",
                /*Type = (byte)VerticalType.Financial,
                IsDeleted = false,
                Fields = new List<VerticalFieldModel> {new VerticalFieldModel()
                {
                    VerticalId = 0,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }*/
            };
            var vertical = new Vertical()
            {
                Id = 1,
                Name = "Finance",
                VerticalFields = new List<VerticalField> {new VerticalField
                {
                    Id = 1,
                    VerticalId = 1,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }
            };

            _mockVerticalService.Setup(x => x.GetVerticalById(1)).Returns(vertical);
            _mockVerticalService.Setup(x => x.UpdateVertical(new Vertical())).Callback(() => vertical = null);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            controller.UpdateVertical(id, verticalModel);
            var actionResult = controller.GetVerticalById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Vertical>;

            // Assert
            Assert.Equal(vertical, contentResult?.Content);
        }

        [Fact]
        public void UpdateVertical_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            _mockVerticalService.Setup(x => x.GetVerticalById(1)).Returns(new Vertical());

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.UpdateVertical(id, null);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"vertical model is null for given id {id}");
        }

        [Fact]
        public void UpdateVerticalField_CorrectVerticalFieldModel_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalFieldModel = new VerticalFieldModel()
            {
                VerticalId = 0,
                Name = "ChanelID1",
                DataType = "Type1",
                Description = null,
                IsRequired = false
            };
            var verticalField = new VerticalField
            {
                Id = 1,
                VerticalId = 1,
                Name = "ChanelID1",
                DataType = "Type2",
                Description = null,
                IsRequired = false
            };

            _mockVerticalService.Setup(x => x.GetVerticalFieldById(1)).Returns(verticalField);
            _mockVerticalService.Setup(x => x.UpdateVerticalField(new VerticalField())).Callback(() => verticalField = null);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            var result = controller.UpdateVerticalField(id, verticalFieldModel);
            var actionResult = controller.GetVerticalFieldById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<VerticalField>;

            // Assert
            Assert.Equal(verticalField, contentResult?.Content);
        }

        [Fact]
        public void UpdateVerticalField_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            const int id = 1;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(1)).Returns(new VerticalField());
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.UpdateVerticalField(id, null);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"vertical field model is null for given id {id}");
        }

        [Fact]
        public void UpdateVertical_VerticalNotFound_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalById(id));
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.UpdateVertical(id, It.IsAny<VerticalModel>());
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical was found for given id {id}");
        }

        [Fact]
        public void UpdateVerticalField_VerticalFieldNotFound_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(id));
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.UpdateVerticalField(id, It.IsAny<VerticalFieldModel>());
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical field was found for given id {id}");
        }

        #endregion

        #region Delete Vertical And Fields

        [Fact]
        public void DeleteVertical_CorrectVerticalWithoutAnyFields_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var vertical = new Vertical()
            {
                Id = 1,
                Name = "Finance"
            };

            _mockVerticalService.Setup(x => x.GetVerticalById(1)).Returns(vertical);
            _mockVerticalService.Setup(x => x.DeleteVertical(new Vertical()));

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            var deleteResult = controller.DeleteVertical(id);
            var deleteContentResult = deleteResult as OkResult;
            var actionResult = controller.GetVerticalById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Vertical>;

            // Assert
            Assert.Equal(vertical, contentResult?.Content);
            Assert.NotNull(deleteContentResult);
        }

        [Fact]
        public void DeleteVertical_VerticalNotFound_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;

            _mockVerticalService.Setup(x => x.GetVerticalById(id));
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.DeleteVertical(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
           // Assert.Equal(contentResult?.Message, $"no vertical was found for given id {id}");
        }

        [Fact]
        public void DeleteVertical_VerticalFoundButCannotDelete_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            var vertical = new Vertical()
            {
                Id = 1,
                Name = "Finance",
                VerticalFields = new List<VerticalField> {new VerticalField
                {
                    Id = 1,
                    VerticalId = 1,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }
            };

            _mockVerticalService.Setup(x => x.GetVerticalById(id)).Returns(vertical);
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.DeleteVertical(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal("can't remove this vertical", contentResult?.Message);
        }

        [Fact]
        public void DeleteVerticalField_CorrectVerticalField_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalField = new VerticalField
            {
                Id = 1,
                VerticalId = 1,
                Name = "ChanelID1",
                DataType = "Type1",
                Description = null,
                IsRequired = false
            };

            _mockVerticalService.Setup(x => x.GetVerticalFieldById(1)).Returns(verticalField);
            _mockVerticalService.Setup(x => x.DeleteVerticalField(new VerticalField()));

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            var deleteResult = controller.DeleteVerticalField(id);
            var deleteContentResult = deleteResult as OkResult;
            var actionResult = controller.GetVerticalFieldById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<VerticalField>;

            // Assert
            Assert.Equal(verticalField, contentResult?.Content);
            Assert.NotNull(deleteContentResult);
        }

        [Fact]
        public void DeleteVerticalField_VerticalFieldNotFound_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalFieldById(id));
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.DeleteVerticalField(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical field was found for given id {id}");
        }

        [Fact]
        public void SoftDeleteVertical_CorrectVertical_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            var verticalModel = new VerticalModel
            {
                Name = "Finance",
                /*Type = (byte)VerticalType.Financial,
                IsDeleted = false,
                Fields = new List<VerticalFieldModel> {new VerticalFieldModel()
                {
                    VerticalId = 0,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }*/
            };
            var vertical = new Vertical()
            {
                Id = 1,
                Name = "Finance",
                VerticalFields = new List<VerticalField> {new VerticalField
                {
                    Id = 1,
                    VerticalId = 1,
                    Name = "ChanelID1",
                    DataType= "Type1",
                    Description = null,
                    IsRequired = false
                } }
            };

            _mockVerticalService.Setup(x => x.GetVerticalById(1)).Returns(vertical);
            _mockVerticalService.Setup(x => x.UpdateVertical(new Vertical())).Callback(() => vertical = null);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            controller.SoftDeleteVertical(id);
            var actionResult = controller.GetVerticalById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<Vertical>;

            // Assert
            Assert.Equal(vertical, contentResult?.Content);
        }

        [Fact]
        public void SoftDeleteVertical_VerticalNotFound_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            _mockVerticalService.Setup(x => x.GetVerticalById(id));
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.SoftDeleteVertical(id);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal(contentResult?.Message, $"no vertical was found for given id {id}");
        }

        #endregion

        #region Load Vertical Fields From Xml

        [Fact]
        public void LoadVerticalFieldsFromXml_CorrectXmlData_ReturnsOkResult()
        {
            // Arrange
            const string xml = "<REQUEST><Name></Name><Type></Type><VerticalId></VerticalId></REQUEST>";
            var result = new List<string> { " Name", "Type", "VerticalId" };
            _mockVerticalService.Setup(x => x.LoadFieldNamesFromXml(xml)).Returns(result);

            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            var actionResult = controller.LoadVerticalFieldsFromXml(xml);
            var contentResult = actionResult as OkNegotiatedContentResult<List<string>>;

            // Assert
            Assert.Equal(result, contentResult?.Content);
        }

        [Fact]
        public void LoadVerticalFieldsFromXml_XmlIsEmpty_BadRequest()
        {
            // Arrange
            const string xml = "";
            var controller = new VerticalController(_mockVerticalService.Object, _mockCampaignService.Object);

            // Act
            var response = controller.LoadVerticalFieldsFromXml(xml);
            var contentResult = response as BadRequestErrorMessageResult;

            // Assert
            //Assert.Equal("Xml is empty", contentResult?.Message);
        }

        #endregion

        #endregion
    }
}
