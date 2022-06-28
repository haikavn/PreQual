using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.Security;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Xunit;

namespace Adrack.WebApi.Tests
{
    /// <summary>
    /// Summary description for RoleControllerUnitTests
    /// </summary>
    public class RoleControllerUnitTests
    {
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly Mock<IPermissionService> _mockPermissionService;
        private readonly Mock<IRolePermissionService> _mockRolePermissionService;
        private readonly Mock<IAddonService> _mockAddonService;
        private readonly Mock<IAppContext> _mockAppContext;

        #region constructors

        public RoleControllerUnitTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _mockPermissionService = new Mock<IPermissionService>();
            _mockRolePermissionService = new Mock<IRolePermissionService>();
        }

        #endregion

        #region Get Role By Id

        [Fact]
        public void GetRoleById_ExistingRole_ReturnsOkResultWithRole()
        {
            // Arrange
            const long id = 2;
            _mockRoleService.Setup(x => x.GetRoleById(id)).Returns(new Role { Id = id });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel { Id = id });
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetRoleById(id);
            var contentResult = response as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(contentResult?.Content.Id ?? 0, id);
        }

        [Fact]
        public void GetRoleById_NonExistingRole_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockRoleService.Setup(x => x.GetRoleById(id));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                                null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetRoleById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no role was found for given id {id}");
        }

        [Fact]
        public void GetRoleById_RoleServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 2;
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<int>())).Throws<Exception>();

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetRoleById(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Get Role List

        [Fact]
        public void GetRoleList_ReturnsOkResultWithRole()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.GetAllRoles(null, false, -1)).Returns(new List<Role>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel { Id = id });

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var actionResult = controller.GetRoleList();
            var contentResult = actionResult as OkNegotiatedContentResult<List<RoleModel>>;

            // Assert
            Assert.IsAssignableFrom<List<RoleModel>>(contentResult?.Content);
        }

        #endregion

        #region Insert Role

        [Fact]
        public void InsertRole_CorrectRoleModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var role = new RoleRequestModel
            {
                Name = "new role",
                UserType = 0
            };
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.InsertRole(It.IsAny<Role>())).Returns(1);
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                                        Returns(new PermissionModel { Id = id });
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.InsertRole(role);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertRole_CorrectRoleModelWithPermissions_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var role = new RoleRequestModel
            {
                Name = "new role",
                UserType = 0
            };
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.InsertRole(It.IsAny<Role>())).Returns(1);
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null)).
                                                                                        Returns(new PermissionModel { Id = id });
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.InsertRole(role);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void InsertRole_WithNullModel_ReturnsBadRequest()
        {
            // Arrange
            _mockRoleService.Setup(x => x.InsertRole(It.IsAny<Role>()));
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null));
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            // Act
            var response = controller.InsertRole(null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"role model is null");
        }

        [Fact]
        public void InsertRole__RoleServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.InsertRole(It.IsAny<Role>())).Throws<Exception>();

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.InsertRole(new RoleRequestModel());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Update Role

        [Fact]
        public void UpdateRole_CorrectRoleModel_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var role = new RoleRequestModel
            {
                Name = "Name",
                UserType = 0
            };
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>()));
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                        Returns(new PermissionModel { Id = id });
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            controller.UpdateRole(id, role);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void UpdateRole_CorrectRoleModelWithPermissions_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            var role = new RoleRequestModel
            {
                Name = "Name",
                UserType = 0
            };
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>()));
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                       Returns(new PermissionModel { Id = id });
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.UpdateRole(id, role);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void UpdateRole_WithNullModel_ReturnsBadRequest()
        {
            // 
            const long id = 1;
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>()));
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = 1 });
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(), It.IsAny<List<Permission>>(), null));
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            // Act
            var response = controller.UpdateRole(id, null);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"role model is null");
        }

        [Fact]
        public void UpdateRole__RoleServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>())).Throws<Exception>();

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.UpdateRole(id, new RoleRequestModel());
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Soft Delete Role

        [Fact]
        public void SoftDeleteRole_ExistingRole_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = id, Deleted = true });
            _mockRoleService.Setup(x => x.DeleteRole(It.IsAny<Role>(), false));
            _mockRoleService.Setup(x => x.GetAllRoles(0, false, -1)).Returns(new List<Role>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                     Returns(new PermissionModel { Id = id });
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                                null,
                                                null);

            controller.SoftDeleteRole(id);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void SoftDeleteRole_CheekIsDeleted_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>()));
            _mockRoleService.Setup(x => x.DeleteRole(It.IsAny<Role>(), false));
            _mockRoleService.Setup(x => x.GetAllRoles(0, false, -1)).Returns(new List<Role>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                     Returns(new PermissionModel { Id = id});
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.SoftDeleteRole(id);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Null(contentResult);
        }

        [Fact]
        public void SoftDeleteRole_RoleNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockRoleService.Setup(x => x.DeleteRole(It.IsAny<Role>(), false));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

           
            // Act
            var response = controller.SoftDeleteRole(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no role was found for given id {id}");
        }

        [Fact]
        public void SoftDeleteRole__RoleServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.DeleteRole(It.IsAny<Role>(), false)).Throws<Exception>();

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.SoftDeleteRole(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion

        #region Deactivate Role

        [Fact]
        public void DeactivateRole_ExistingRole_ReturnsOkResult()
        {
            // Arrange
            const int id = 1;
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).
                                   Returns(new Role { Id = 1, Active = false });
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>()));
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRoleService.Setup(x => x.GetAllRoles(0, false, -1)).Returns(new List<Role>());
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                       Returns(new PermissionModel { Id = 1});
            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.DeactivateRole(id);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.Equal(id, contentResult?.Content.Id);
        }

        [Fact]
        public void DeactivatePermission_CheekIsNonActive_ReturnsOkResult()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.GetRoleById(It.IsAny<long>())).Returns(new Role { Id = id, Active = false });
            _mockRoleService.Setup(x => x.DeleteRole(It.IsAny<Role>(), false));
            _mockRoleService.Setup(x => x.GetAllRoles(0, false, -1)).Returns(new List<Role>());
            _mockPermissionService.Setup(x => x.GetAllPermissions(0)).Returns(new List<Permission>());
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);
            _mockRolePermissionService.Setup(x => x.BuildPermissionTree(It.IsAny<PermissionModel>(),
                                                                        It.IsAny<List<Permission>>(), null)).
                                     Returns(new PermissionModel { Id = id});

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null);

            controller.DeactivateRole(id);
            var actionResult = controller.GetRoleById(id);
            var contentResult = actionResult as OkNegotiatedContentResult<RoleModel>;

            // Assert
            Assert.NotNull(contentResult?.Content);
        }
        [Fact]
        public void DeactivateRole_RoleNotFound_ReturnsBadRequest()
        {
            // Arrange
            const long id = -1;
            _mockRoleService.Setup(x => x.GetRoleById(id));
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>()));
            _mockPermissionService.Setup(x => x.Authorize(It.IsAny<string>(), PermissionState.Access)).Returns(true);

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.SoftDeleteRole(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var errorViewModel = contentResult.Content.ReadAsAsync(typeof(ErrorViewModel)).Result;

            // Assert
            Assert.Equal(((ErrorViewModel)errorViewModel)?.Message, $"no role was found for given id {id}");
        }

        [Fact]
        public void DeactivateRole__RoleServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            const long id = 1;
            _mockRoleService.Setup(x => x.UpdateRole(It.IsAny<Role>())).Throws<Exception>();

            var controller = new RoleController(_mockRoleService.Object,
                                                _mockPermissionService.Object,
                                                _mockRolePermissionService.Object,
                                               null,
                                                null)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.DeactivateRole(id);
            var contentResult = response.ExecuteAsync(CancellationToken.None).Result;

            var code = contentResult.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, JsonConvert.DeserializeObject<ErrorViewModel>(code).Status);
        }

        #endregion
    }
}
