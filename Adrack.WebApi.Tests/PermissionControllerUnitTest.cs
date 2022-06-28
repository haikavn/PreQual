using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.Security;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for PermissionControllerUnitTest
    /// </summary>
    public class PermissionControllerUnitTest
    {
        private readonly Mock<IPermissionService> _mockPermissionService;
        private readonly Mock<IRolePermissionService> _mockRolePermissionService;

        #region constructors

        public PermissionControllerUnitTest()
        {
            _mockPermissionService = new Mock<IPermissionService>();
            _mockRolePermissionService = new Mock<IRolePermissionService>();
        }

        #endregion

        #region Get Permission By Id

        [Fact]
        public void GetPermissionById_ExistingPermission_ReturnsOkResultWithPermissionTree()
        {
            // Arrange
            const long id = 2;

            _mockPermissionService.Setup(x => x.GetPermissionById(id)).Returns(new Permission { Id = id });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel { Id = id });

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetPermissionById(id);
            var contentResult = response as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetPermissionById_NonExistingPermission_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;

            _mockPermissionService.Setup(x => x.GetPermissionById(id));

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetPermissionById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no permission was found for given id {id}");

        }

        [Fact]
        public void GetPermissionById_PermissionServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 2;
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<int>())).Throws<Exception>();

            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetPermissionById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Get Permission List

        [Fact]
        public void GetPermissionList_ReturnsOkResultWithPermissionTree()
        {
            // Arrange
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel());

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var actionResult = controller.GetPermissionList();
            var contentResult = actionResult as OkNegotiatedContentResult<List<PermissionModel>>;

            // Assert
            Assert.IsAssignableFrom<List<PermissionModel>>(contentResult?.Content);
        }

        [Fact]
        public void GetPermissionList_CheekPermissionTree_ReturnsOkResultWithPermissionTree()
        {
            // Arrange
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission> { new Permission() });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel
                                        {
                                            Id = 1,
                                            SubPermissions = new List<PermissionModel>
                                                                        { new PermissionModel
                                                                            {
                                                                               Id = 2,
                                                                               SubPermissions = new List<PermissionModel>
                                                                               {
                                                                               new PermissionModel()
                                                                               }
                                                                             }
                                                                        }
                                        });

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var actionResult = controller.GetPermissionList();
            var contentResult = actionResult as OkNegotiatedContentResult<List<PermissionModel>>;

            // Assert
            Assert.True(contentResult?.Content.Find(x => x.Id == 1).SubPermissions.Count > 0);
        }
        #endregion

        #region Insert Permission

        [Fact]
        public void InsertPermission_CorrectPermissionModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var permission = new Permission
            {
                ParentId = 0,
                Name = "testPermission",
                Key = "test-View",
                EntityName = "test",
                Active = true,
                Deleted = false,
                Description = "",
                Order = null,
                Id = id
            };
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<long>())).Returns(permission);
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.InsertPermission(new Permission())).Callback(() => permission = null);
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                                        Returns(new PermissionModel { Id = id });
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object);

            controller.InsertPermission((PermissionModel)permission);
            var actionResult = controller.GetPermissionById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.Equal(permission.Id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertPermission_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.InsertPermission(null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"permission model is null");
        }

        [Fact]
        public void InsertPermission__PermissionServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var permission = new Permission
            {
                ParentId = 0,
                Name = "testPermission",
                Key = "test-View",
                EntityName = "test",
                Active = true,
                Deleted = false,
                Description = "",
                Order = null,
                Id = 0
            };
            _mockPermissionService.Setup(x => x.InsertPermission(It.IsAny<Permission>())).Throws<Exception>();

            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };


            var response = controller.InsertPermission((PermissionModel)permission);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);

        }

        #endregion

        #region Soft Delete Permission

        [Fact]
        public void SoftDeletePermission_ExistingPermission_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<long>())).Returns(new Permission { Id = id, Deleted = true });
            _mockPermissionService.Setup(x => x.DeletePermission(It.IsAny<Permission>(), false));
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                                Returns(new PermissionModel { Id = id});
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object);

            controller.SoftDeletePermission(id);
            var actionResult = controller.GetPermissionById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void SoftDeletePermission_CheekIsDeleted_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<long>()));
            _mockPermissionService.Setup(x => x.DeletePermission(It.IsAny<Permission>(), false));
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                                Returns(new PermissionModel { Id = id});
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object);

            controller.SoftDeletePermission(id);
            var actionResult = controller.GetPermissionById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.Null(contentResult?.Content);
        }
        [Fact]
        public void SoftDeletePermission_PermissionNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockPermissionService.Setup(x => x.DeletePermission(It.IsAny<Permission>(), false));

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.SoftDeletePermission(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no permission was found for given id {id}");
        }

        [Fact]
        public void SoftDeletePermission__PermissionServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockPermissionService.Setup(x => x.DeletePermission(It.IsAny<Permission>(), false)).Throws<Exception>();

            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.SoftDeletePermission(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Deactivate Permission

        [Fact]
        public void DeactivatePermission_ExistingPermission_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<long>())).
                                                                    Returns(new Permission { Id = 1, Active = false });
            _mockPermissionService.Setup(x => x.UpdatePermission(It.IsAny<Permission>()));
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                               Returns(new PermissionModel { Id = 1});
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object);

            controller.DeactivatePermission(id);
            var actionResult = controller.GetPermissionById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void DeactivatePermission_CheekIsNonActive_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            _mockPermissionService.Setup(x => x.GetPermissionById(It.IsAny<long>())).
                                                                    Returns(new Permission { Id = 1, Active = false });
            _mockPermissionService.Setup(x => x.UpdatePermission(It.IsAny<Permission>()));
            _mockPermissionService.Setup(x => x.GetAllPermissions()).Returns(new List<Permission>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                               Returns(new PermissionModel { Id = 1});
            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object);

            controller.DeactivatePermission(id);
            var actionResult = controller.GetPermissionById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<PermissionModel>;

            // Assert
            Assert.NotNull(contentResult?.Content);
        }
        [Fact]
        public void DeactivatePermission_PermissionNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockPermissionService.Setup(x => x.UpdatePermission(It.IsAny<Permission>()));

            var controller = new PermissionController(_mockPermissionService.Object,
                                                      _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.SoftDeletePermission(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no permission was found for given id {id}");
        }

        [Fact]
        public void DeactivatePermission__PermissionServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockPermissionService.Setup(x => x.UpdatePermission(It.IsAny<Permission>())).Throws<Exception>();

            var controller = new PermissionController(_mockPermissionService.Object, _mockRolePermissionService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.DeactivatePermission(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion
    }
}
