using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Security;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/role")]
    public class RoleController : BaseApiController
    {
        #region fields

        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IAddonService _addonService;
        private readonly IAppContext _appContext;

        #endregion fields

        #region static fields

        private static string _editRolePermissionKey { get; set; } = "edit-roles-usermanagement";
        private static string _viewRolePermissionKey  { get; set; } = "view-roles-usermanagement";

        #endregion

        #region constructors

        public RoleController(IRoleService roleService,
                              IPermissionService permissionService,
                              IRolePermissionService rolePermissionService,
                              IAddonService addonService,
                              IAppContext appContext)
        {
            _roleService = roleService;
            _permissionService = permissionService;
            _rolePermissionService = rolePermissionService;
            _addonService = addonService;
            _appContext = appContext;
        }

        #endregion constructors

        #region route methods



        [HttpGet]
        [Route("generateSystemRoles")]
        public IHttpActionResult GenerateSystemRoles()
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                _roleService.GenerateSystemRoles();

                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getRoleList")]
        public IHttpActionResult GetRoleList(UserTypes? userType = null, bool withUsers = false, short systemRole = -1, bool withPermissions = false)
        {
            if (!_permissionService.Authorize(_viewRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var roles = _roleService.GetAllRoles(userType, withUsers, systemRole).Where(x => !x.Deleted).ToList();
                var roleList = new List<RoleModel>();
                foreach (var role in roles)
                {
                    if (_appContext.AppUser != null)
                    {
                        if (_appContext.AppUser.UserType != UserTypes.Super &&
                            _appContext.AppUser.UserType != UserTypes.Network &&
                            _appContext.AppUser.UserType != role.UserType)
                            continue;
                    }

                    var roleModule = (RoleModel)role;

                    if (role.IsSystemRole.HasValue && role.IsSystemRole.Value)
                    {
                        roleModule.Name += " (System role)";
                    }

                    if (withUsers && role.Users.Any())
                    {
                        foreach (var user in role.Users.Where(x => !x.Deleted))
                        {
                            var userModel = new UserSimpleModel
                            {
                                Id = user.Id,
                                FullName = user.GetFullName()
                            };
                            roleModule.Users.Add(userModel);
                        }
                    }

                    var permissions = _permissionService.GetPermissionsByRoleId(role.Id);
                    if (withPermissions && permissions.Any())
                    {
                        foreach (var permission in permissions)
                        {
                            var permissionModel = new PermissionModel
                            {
                                ParentId = permission.ParentId,
                                Name = permission.Name,
                                Key = permission.Key,
                                Description = permission.Description,
                                Id = permission.Id
                            };
                            roleModule.Permissions.Add(permissionModel);
                        }
                    }

                    roleList.Add(roleModule);
                }
                return Ok(roleList);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <summary>
        /// Get Role By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getRoleById/{id}")]
        public IHttpActionResult GetRoleById(long id)
        {
            if (!_permissionService.Authorize(_viewRolePermissionKey))
            { 
                return HttpBadRequest("access-denied");
            }

            string message = "";
            var roleModel = GetRoleModel(id, out message);

            if (roleModel == null)
            {
                return HttpBadRequest(message);
            }

            return Ok(roleModel);
        }

        [HttpGet]
        [Route("getRolePermissionsByUserType/{userType}")]
        public IHttpActionResult GetPermissionsByUserType(UserTypes userType)
        {
            if (!_permissionService.Authorize(_viewRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }

            var permissions = GetPermissions(userType);

            return Ok(permissions);
        }

        /// <summary>
        /// Insert Role
        /// </summary>
        /// <param name="roleRequestModel">RoleRequestModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("addRole")]
        public IHttpActionResult InsertRole([FromBody]RoleRequestModel roleRequestModel)
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (roleRequestModel == null)
                {
                    return HttpBadRequest($"role model is null");
                }

                var role = new Role
                {
                    Id = 0,
                    Name = roleRequestModel.Name,
                    Key = roleRequestModel.Name.Replace(" ", String.Empty),
                    Deleted = false,
                    BuiltIn = false,
                    UserType = roleRequestModel.UserType,
                    Active = roleRequestModel.IsActive
                };

                if (!ValidatePermissions(null, roleRequestModel.Permissions, role: role))
                {
                    return HttpBadRequest($"permissions are not valid");
                }

                role.Id = _roleService.InsertRole(role);
                InsertRolePermissions(null, roleRequestModel.Permissions, role.Id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="roleRequestModel">RoleRequestModel</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("updateRole/{id}")]
        public IHttpActionResult UpdateRole(long id, [FromBody]RoleRequestModel roleRequestModel)
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var role = _roleService.GetRoleById(id);

                if (role == null || role.Deleted)
                    return HttpBadRequest($"no vertical was found for given id {id}");

                if (role.IsSystemRole.HasValue && role.IsSystemRole.Value)
                {
                    return HttpBadRequest($"can not change system role");
                }

                if (roleRequestModel == null)
                    return HttpBadRequest($"role model is null");

                if (!ValidatePermissions(null, roleRequestModel.Permissions, role: role))
                {
                    return HttpBadRequest($"permissions are not valid");
                }

                role.Name = roleRequestModel.Name;
                role.Key = roleRequestModel.Name.Replace(" ", String.Empty);
                role.UserType = roleRequestModel.UserType;
                role.Active = roleRequestModel.IsActive;
                _permissionService.ClearRolePermissions(id);
                InsertRolePermissions(null, roleRequestModel.Permissions, id);
                _roleService.UpdateRole(role);

                return Ok(role);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Soft Delete Role
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [HttpPut]
        [Route("softDeleteRole/{id}")]
        public IHttpActionResult SoftDeleteRole(long id)
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var role = _roleService.GetRoleById(id);

                if (role == null || role.Deleted)
                    return HttpBadRequest($"no role was found for given id {id}");

                if (role.IsSystemRole.HasValue && role.IsSystemRole.Value)
                {
                    return HttpBadRequest($"can not delete system role");
                }

                _roleService.DeleteRole(role, false);

                return Ok(role);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deactivate Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("deactivateRole/{id}")]
        public IHttpActionResult DeactivateRole(long id)
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var role = _roleService.GetRoleById(id);

                if (role == null || role.Deleted)
                    return HttpBadRequest($"no role was found for given id {id}");

                if (role.IsSystemRole.HasValue && role.IsSystemRole.Value)
                {
                    return HttpBadRequest($"can not deactivate system role");
                }

                role.Active = false;
                _roleService.UpdateRole(role);

                return Ok(role);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Change Role Permission State
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <param name="isAccess"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateRolePermissionState")]
        public IHttpActionResult UpdateRolePermissionState(long roleId, long permissionId, bool isAccess = true)
        {
            if (!_permissionService.Authorize(_editRolePermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (roleId == 0 || permissionId == 0)
                {
                    return HttpBadRequest($"role and permission must be selected");
                }

                var role = _roleService.GetRoleById(roleId);
                if (role == null)
                {
                    return HttpBadRequest($"this Role not found.");
                }

                var permission = _permissionService.GetPermissionById(permissionId);
                if (permission == null)
                {
                    return HttpBadRequest($"this Permission not found.");
                }

                if (!AddonValidation(permissionId))
                {
                    return HttpBadRequest($"this Permission is linked with Addon, but the User is not linked with Addon.");
                }

                _permissionService.UpdateRolePermission(roleId, permissionId, isAccess ? (byte)1 : (byte)0);

                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }
        #endregion methods

        #region private methods

        private bool AddonValidation(long permissionId)
        {
            IList<PermissionAddon> addons = _addonService.GetAddonsByPermissionId(permissionId);
            if (addons.Count > 0)
            {
                var userAddons = _addonService.GetAddonsByUserId(_appContext.AppUser.Id).ToList();

                foreach (var addon in addons)
                {
                    var userAddon = userAddons.Where(x => x.AddonId == addon.Id).FirstOrDefault();
                    if (userAddon != null)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        private void InsertRolePermissions(PermissionSimpleModel parent, List<PermissionSimpleModel> permissions, long roleId)
        {
            foreach (var p in permissions)
            {
                if (p.Id == 0) continue;
                if (parent != null && !parent.IsAccess)
                    p.IsAccess = false;
                _permissionService.AddRolePermission(roleId, p.Id, p.IsAccess ? (byte)1 : (byte)0);
                if (p.SubPermissions.Any())
                {
                    InsertRolePermissions(p, p.SubPermissions, roleId);
                }
            }
        }

        private bool ValidatePermissions(PermissionSimpleModel parent, List<PermissionSimpleModel> permissions, List<Permission> allPermissions = null, Role role = null, int level = 1)
        {
            if (allPermissions == null || !allPermissions.Any())
            { 
                allPermissions = _permissionService.GetAllPermissions().ToList();
            }

            foreach (var p in permissions)
            {
                if (parent != null && !parent.IsAccess)
                    p.IsAccess = false;

                var existingPermission = allPermissions.Where(x => x.Id == p.Id).FirstOrDefault();

                if (existingPermission == null ||
                    (existingPermission != null && role != null &&
                    role.UserType != UserTypes.Super && role.UserType != UserTypes.Network &&
                    !existingPermission.UserTypeList.Contains(role.UserType) && p.IsAccess) && level != 2)
                {
                    return false;
                }
               
                if (p.SubPermissions.Any())
                {
                    if (!ValidatePermissions(p, p.SubPermissions, allPermissions, role, level + 1))
                        return false;
                }
            }

            return true;
        }

        private RoleModel GetRoleModel(long id, out string outMessage)
        {
            outMessage = "";
            Role role = null;
            RoleModel roleModule = null;

            role = _roleService.GetRoleById(id);

            if (role == null)
            {
                outMessage = $"no role was found for given id {id}";
                return null;
            }

            roleModule = (RoleModel)role;

            var permissions = _permissionService.GetAllPermissions(0);

            foreach (var item in permissions)
            {
                if (!AddonValidation(item.Id))
                    continue;

                if (role.UserType != UserTypes.Super &&
                    role.UserType != UserTypes.Network
                    &&
                    !item.UserTypeList.Contains(role.UserType)) continue;

                var headPermission = (PermissionModel)item;

                var rolePermission = _permissionService.GetRolePermission(id, item.Id);
                //var rolePermission = role.RolePermissions.Where(x => x.PermissionId == item.Id).FirstOrDefault();
                headPermission.IsAccess = rolePermission != null && rolePermission.State == 1 ? true : false;
                var permissionModel = _rolePermissionService.BuildPermissionTreeWithState(headPermission, role.RolePermissions.ToList());
                roleModule.Permissions.Add(permissionModel);
            }

            return roleModule;
        }

        private List<PermissionModel> GetPermissions(UserTypes userType)
        {
            List<PermissionModel> list = new List<PermissionModel>();

            var permissions = _permissionService.GetAllPermissions(0);

            foreach (var item in permissions)
            {
                if (!AddonValidation(item.Id))
                    continue;

                if (userType != UserTypes.Super &&
                    userType != UserTypes.Network
                    &&
                    !item.UserTypeList.Contains(userType)) continue;

                var headPermission = (PermissionModel)item;

                headPermission.IsAccess = false;
                var permissionModel = _rolePermissionService.BuildPermissionTree(headPermission, permissions.ToList(), userType);
                list.Add(permissionModel);
            }

            return list;
        }
        #endregion

    }
}
