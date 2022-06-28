// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerticalController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class VerticalController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class VerticalController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IVerticalService _verticalService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="verticalService">The vertical service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        public VerticalController(IVerticalService verticalService, ILocalizedStringService localizedStringService)
        {
            this._verticalService = verticalService;
            this._localizedStringService = localizedStringService;
        }

        #endregion Constructor

        // GET: Affiliate
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            List<Vertical> verticals = (List<Vertical>)this._verticalService.GetAllVerticals();
            return View();
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Verticals")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(VerticalModel model)
        {
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Item(long id = 0)
        {
            Vertical vertical = this._verticalService.GetVerticalById(id);

            VerticalModel am = new VerticalModel();

            am.VerticalId = 0;

            if (vertical != null)
            {
                am.VerticalId = vertical.Id;
                am.Name = vertical.Name;
            }

            PrepareModel(am);

            //return View(am);
            return View(am);
        }

        /// <summary>
        /// Items the specified vertical model.
        /// </summary>
        /// <param name="verticalModel">The vertical model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(VerticalModel verticalModel, string returnUrl)
        {
            Vertical vertical = null;

            if (verticalModel.VerticalId == 0)
            {
                vertical = new Vertical();
            }
            else
            {
                vertical = _verticalService.GetVerticalById(verticalModel.VerticalId);
            }

            vertical.Name = verticalModel.Name.Trim();

            if (verticalModel.VerticalId == 0)
                _verticalService.InsertVertical(vertical);
            else
                _verticalService.UpdateVertical(vertical);

            verticalModel.VerticalId = vertical.Id;

            PrepareModel(verticalModel);

            return View("List");
        }

        /// <summary>
        /// Gets the verticals.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetVerticals()
        {
            List<Vertical> verticals = (List<Vertical>)this._verticalService.GetAllVerticals();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = verticals.Count;
            jd.recordsFiltered = verticals.Count;
            foreach (Vertical ai in verticals)
            {
                string[] names1 = {
                                       "<div style='text-align:center;'>" + ai.Id.ToString() + "</div>",
                                       "<div style='text-align:center;'><b><a href=\"/Management/Vertical/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a></b></div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}