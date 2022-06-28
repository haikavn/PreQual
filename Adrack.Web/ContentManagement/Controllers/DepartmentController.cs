// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DepartmentController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Lead;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class DepartmentController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class DepartmentController : BaseContentManagementController
    {
        /// <summary>
        /// The department service
        /// </summary>
        protected IDepartmentService _departmentService;

        // GET: Department
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentController"/> class.
        /// </summary>
        /// <param name="departmentService">The department service.</param>
        public DepartmentController(IDepartmentService departmentService)
        {
            this._departmentService = departmentService;
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Departments")]
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
            Department department = _departmentService.GetDepartmentById(Id);

            DepartmentModel model = new DepartmentModel();

            model.DepartmentId = 0;

            if (department != null)
            {
                model.DepartmentId = department.Id;
                model.Name = department.Name;
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
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(DepartmentModel model, string returnUrl)
        {
            Department department = null;

            //if (ModelState.IsValid)
            {
                if (model.DepartmentId == 0)
                {
                    department = new Department();
                }
                else
                {
                    department = _departmentService.GetDepartmentById(model.DepartmentId);
                }

                department.Name = model.Name.Trim();

                if (model.DepartmentId == 0)
                    _departmentService.InsertDepartment(department);
                else
                    _departmentService.UpdateDepartment(department);

                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// Gets the departments.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetDepartments()
        {
            List<Department> departments = (List<Department>)_departmentService.GetAllDepartments();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = departments.Count;
            jd.recordsFiltered = departments.Count;

            foreach (Department ai in departments)
            {
                string[] names1 = {
                                      "<div style='text-align:center;'>" + ai.Id.ToString() + "</div>",
                                      "<div style='text-align:center;'><b><a href='/Management/Department/Item/" + ai.Id + "'>" + ai.Name + "</a></b></div>"
                                  };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}