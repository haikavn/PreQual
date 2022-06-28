using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Security;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/setting/permission")]
    public class PermissionController : BaseApiController
    {
        #region fields

        private readonly IPermissionService _permissionService;
        private readonly IRolePermissionService _rolePermissionService;

        #endregion fields

        #region constructors

        public PermissionController(IPermissionService permissionService,
                                    IRolePermissionService rolePermissionService)
        {
            _permissionService = permissionService;
            _rolePermissionService = rolePermissionService;
        }

        #endregion constructors

        #region route methods

        /// <summary>
        /// Get All Permissions
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getPermissionList")]
        public IHttpActionResult GetPermissionList()
        {
            var permissions = _permissionService.GetAllPermissions(). Where(x => !x.Deleted).ToList();
            var permissionsTree = new List<PermissionModel>();
            foreach (var item in permissions.Where(x => x.ParentId == 0))
            {
                var permission = _rolePermissionService.BuildPermissionTree((PermissionModel)item, permissions);
                permissionsTree.Add(permission);
            }
            return Ok(permissionsTree);
        }

        /// <summary>
        /// Get Permission By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getPermissionById/{id}")]
        public IHttpActionResult GetPermissionById(long id)
        {
            var permission = _permissionService.GetPermissionById(id);

            if (permission == null)
                return HttpBadRequest($"no permission was found for given id {id}");

            var permissionWithSubPermission = _rolePermissionService.BuildPermissionTree((PermissionModel)permission, null);
            return Ok(permissionWithSubPermission);
        }

        /// <summary>
        /// Insert Permission
        /// </summary>
        /// <param name="permissionModel">PermissionModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("insertPermission")]
        public IHttpActionResult InsertPermission([FromBody]PermissionModel permissionModel)
        {
            try
            {
                if (permissionModel == null)
                    return HttpBadRequest($"permission model is null");

                var permission = (Permission)permissionModel;
                _permissionService.InsertPermission(permission);

                return Ok(permission);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Soft Delete Permission
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("softDeletePermission/{id}")]
        public IHttpActionResult SoftDeletePermission(long id)
        {
            try
            {
                var permission = _permissionService.GetPermissionById(id);

                if (permission == null)
                    return HttpBadRequest($"no permission was found for given id {id}");

                _permissionService.DeletePermission(permission, false);

                return Ok(permission);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deactivate Permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("deactivatePermission/{id}")]
        public IHttpActionResult DeactivatePermission(long id)
        {
            try
            {
                var permission = _permissionService.GetPermissionById(id);

                if (permission == null)
                    return HttpBadRequest($"no permission was found for given id {id}");

                permission.Active = false;
                _permissionService.UpdatePermission(permission);

                return Ok(permission);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }
        #endregion methods
    }
}
