// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RoleController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Data;
using Adrack.Service.Directory;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Security;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class RoleController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class RoleController : BaseContentManagementController
    {
        /// <summary>
        /// The role service
        /// </summary>
        protected IRoleService _roleService;

        /// <summary>
        /// The permission service
        /// </summary>
        protected IPermissionService _permissionService;

        /// <summary>
        /// The user type service
        /// </summary>
        protected IUserTypeService _userTypeService;

        /// <summary>
        /// The user service
        /// </summary>
        protected IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        /// <param name="userTypeService">The user type service.</param>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="userService">The user service.</param>
        public RoleController(IRoleService roleService, IUserTypeService userTypeService, IPermissionService permissionService, IUserService userService)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._userTypeService = userTypeService;
            this._userService = userService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Users / Roles")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Item(long Id = 0)
        {
            Role role = _roleService.GetRoleById(Id);

            RoleModel model = new RoleModel();

            model.RoleId = 0;

            if (role != null)
            {
                model.RoleId = role.Id;
                model.Active = role.Active;
                model.Name = role.Name;
                model.Key = role.Key;
                model.UserType = role.UserType;
            }
            else
            {
                model.Active = false;
            }

            model.PermissionStatuses = new List<bool>();

            // List<Permission> permissions = (List<Permission>)_permissionService.GetAllPermissions().OrderBy(e => e.EntityName).ToList();
            List<Permission> permissions = (List<Permission>)_permissionService.GetAllPermissions().ToList();

            foreach (Permission p in permissions)
            {
                if (role != null)
                    p.Active = role.RolePermissions.Any(x=>x.Permission == p);
                else
                    p.Active = false;
            }

            model.Permissions = permissions;

            foreach (var value in _userTypeService.GetAllUserTypes())
            {
                model.ListUserType.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == (long)model.UserType
                });
            }

            return View(model);
        }

        /// <summary>
        /// Items the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(RoleModel model, string returnUrl)
        {
            Role role = null;

            //if (ModelState.IsValid)
            {
                if (model.RoleId == 0)
                {
                    role = new Role();
                }
                else
                {
                    role = _roleService.GetRoleById(model.RoleId);
                }

                role.Name = model.Name;
                role.Key = model.Name.Replace(" ", "");
                role.Active = model.Active;
                role.UserType = model.UserType;
                role.Deleted = false;
                role.BuiltIn = false;

                if (model.RoleId == 0)
                    _roleService.InsertRole(role);
                else
                    _roleService.UpdateRole(role);

                _permissionService.ClearRolePermissions(role.Id);

                foreach (Permission p in model.Permissions)
                {
                    if (p.Active)
                        _permissionService.AddRolePermission(role.Id, p.Id, (byte)PermissionState.Access );
                }

                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetRoles()
        {
            List<Role> roles = (List<Role>)_roleService.GetAllRoles();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = roles.Count;
            jd.recordsFiltered = roles.Count;
            foreach (Role ai in roles)
            {
                string userTypeName = Enum.GetName(typeof(UserTypes), ai.UserType);

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href='/Management/Role/Item/" + ai.Id + "'>" + ai.Name + "</a>",
                                      userTypeName,
                                      //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                                      ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>"
                                  };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetUsers()
        {
            JsonData jd = new JsonData();

            List<User> users = new List<User>();
            if (Request["roleid"] != null)
            {
                users = (List<User>)this._userService.GetUsersByRoleId(long.Parse(Request["roleid"]));
            }

            jd.draw = 1;
            jd.recordsTotal = users.Count;
            jd.recordsFiltered = users.Count;

            foreach (User ai in users)
            {
                Role role = null;

                if (ai.Roles.Count > 0)
                    role = ai.Roles.ElementAt(0);

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href='/Management/User/Item/" + ai.Id + "'>" + ai.GetFullName() + "</a>",
                                      ai.Email
                                };
                jd.data.Add(names1);
            }
            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}