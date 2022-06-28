// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SecurityController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using Adrack.Data;
using Adrack.Service.Security;
using Adrack.Web.Controllers;
using Adrack.Web.Framework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    ///     Class SecutiryController.
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class SecutiryController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecutiryController" /> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public SecutiryController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     Gets or sets the role service.
        /// </summary>
        /// <value>The role service.</value>
        protected IRoleService _roleService { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Membership")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     List
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Label = "User")]
        public ActionResult RoleList()
        {
            return View();
        }

        /// <summary>
        ///     Edit
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Label = "User")]
        public ActionResult RoleItem()
        {
            return View();
        }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetRoles(long affiliateId = 0)
        {
            var roles = (List<Role>)_roleService.GetAllRoles();

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = roles.Count,
                recordsFiltered = roles.Count
            };
            foreach (var ai in roles)
            {
                string[] names1 =
                {
                    ai.Id.ToString(),
                    "<a href='/Role/Item/" + ai.Id + "'>" + ai.Name + "</a>",
                    "<span>" + ai.Key + "</span>",
                    //"<a href=\"/Affiliate/Item/" + ai.ParentId.ToString() + "\">" + affiliate.Name + "</a>",
                    ai.Active ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>"
                };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}