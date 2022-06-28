// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BlackListController.cs" company="Adrack.com">
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
using System.Reflection;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Handles black list actions
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class BlackListController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IBlackListService _blackListService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        public BlackListController(IBlackListService blackListService, ILocalizedStringService localizedStringService)
        {
            this._blackListService = blackListService;
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
            return View();
        }

        /// <summary>
        /// Show blacklist partial item
        /// </summary>
        /// <param name="BlackListType">black list type</param>
        /// <param name="ParentId">Black list parent id</param>
        /// <returns>View result</returns>
        public ActionResult ItemPartial(short BlackListType = 0, long ParentId = 0)
        {
            BlackListModel am = new BlackListModel();

            am.BlackListTypeId = 0;
            am.BlackType = BlackListType;
            am.ParentId = ParentId;

            PrepareModel(am);

            return PartialView("Item", am);
        }

        /// <summary>
        /// Displays black list partial interface
        /// </summary>
        /// <param name="BlackListType">Black list type</param>
        /// <param name="ParentId">Parent id</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ListPartial(short BlackListType = 0, long ParentId = 0)
        {
            ViewBag.BlackListType = BlackListType;
            ViewBag.ParentId = ParentId;
            return PartialView("List");
        }

        /// <summary>
        /// Shows black list view
        /// </summary>
        /// <param name="BlackListType">Black list type</param>
        /// <param name="ParentId">Parent id</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Black Lists")]
        public ActionResult List(short BlackListType = 0, long ParentId = 0)
        {
            ViewBag.BlackListType = BlackListType;
            ViewBag.ParentId = ParentId;
            return View();
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(BlackListModel model)
        {
            PropertyInfo[] properties = typeof(LeadContent).GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.ToLower() == "id" ||
                    pi.Name.ToLower() == "leadid" ||
                    pi.Name.ToLower() == "affiliateid" ||
                    pi.Name.ToLower() == "campaigntype" ||
                    pi.Name.ToLower() == "minpricestr" ||
                    pi.Name.ToLower() == "created") continue;
                model.BlackListNames.Add(new SelectListItem() { Text = pi.Name, Value = pi.Name });
            }
        }

        /// <summary>
        /// Shows black list item create/edit interface
        /// </summary>
        /// <param name="id">Black list id</param>
        /// <returns>View result</returns>

        public ActionResult Item(long id = 0)
        {
            BlackListType bt = this._blackListService.GetBlackListTypeById(id);

            BlackListModel am = new BlackListModel();

            am.BlackListTypeId = 0;
            am.BlackType = 0;
            am.ParentId = 0;

            am.Conditions.Add(new SelectListItem() { Value = "1", Text = "EQUAL" });
            am.Conditions.Add(new SelectListItem() { Value = "2", Text = "STARTS WITH" });
            am.Conditions.Add(new SelectListItem() { Value = "3", Text = "ENDS WITH" });
            am.Conditions.Add(new SelectListItem() { Value = "4", Text = "CONTAINS" });

            if (bt != null)
            {
                am.BlackListTypeId = bt.Id;
                am.Name = bt.Name;
                am.BlackType = bt.BlackType;
                am.ParentId = bt.ParentId;
            }
            else
            {
                short t = 0;
                long p = 0;
                short.TryParse(Request["t"], out t);
                long.TryParse(Request["p"], out p);

                am.BlackType = t;
                am.ParentId = p;
            }

            PrepareModel(am);

            //return View(am);
            return View(am);
        }

        /// <summary>
        /// Handles black list item submit action
        /// </summary>
        /// <param name="model">BlackListModel reference</param>
        /// <param name="returnUrl">Redirect url after complete</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(BlackListModel model, string returnUrl)
        {
            BlackListType bt = null;

            if (model.BlackListTypeId == 0)
            {
                bt = new BlackListType();
            }
            else
            {
                bt = _blackListService.GetBlackListTypeById(model.BlackListTypeId);
            }

            bt.Name = model.Name;
            bt.BlackType = model.BlackType;
            bt.ParentId = model.ParentId;

            if (model.BlackListTypeId == 0)
                _blackListService.InsertBlackListType(bt);
            else
                _blackListService.UpdateBlackListType(bt);

            model.BlackListTypeId = bt.Id;

            PrepareModel(model);

            return Redirect("List");
        }

        /// <summary>
        /// Adds black list value
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult AddBlackListValue()
        {
            BlackListValue bv = new BlackListValue();

            bv.BlackListTypeId = long.Parse(Request["blacklisttypeid"]);
            bv.Value = Request["value"];
            bv.Condition = short.Parse(Request["condition"]);

            _blackListService.InsertBlackListValue(bv);

            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds custom blacklist value
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult AddCustomBlackListValue()
        {
            CustomBlackListValue bv = new CustomBlackListValue();

            bv.ChannelId = long.Parse(Request["id"]);
            bv.ChannelType = short.Parse(Request["type"]);
            bv.Value = Request["value"];
            bv.TemplateFieldId = long.Parse(Request["field"]);

            _blackListService.InsertCustomBlackListValue(bv);

            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes black list value
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult RemoveBlackListValue()
        {
            BlackListValue bv = _blackListService.GetBlackListValueById(long.Parse(Request["id"]));

            if (bv != null)
                _blackListService.DeleteBlackListValue(bv);

            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes black list custom value
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult RemoveCustomBlackListValue()
        {
            CustomBlackListValue bv = _blackListService.GetCustomBlackListValueById(long.Parse(Request["id"]));
            if (bv != null)
                _blackListService.DeleteCustomBlackListValue(bv);

            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets black list types
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBlackListTypes()
        {
            short type = 0;
            long parentid = 0;

            short.TryParse(Request["t"], out type);
            long.TryParse(Request["p"], out parentid);

            List<BlackListType> btypes = (List<BlackListType>)this._blackListService.GetAllBlackListTypesByParentId(type, parentid);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = btypes.Count;
            jd.recordsFiltered = btypes.Count;
            foreach (BlackListType ai in btypes)
            {
                string[] names1 = {
                                      "<div style='text-align:center;'>" + ai.Id.ToString() + "</div>",
                                      "<div style='text-align:center;'><b><a href=\"/Management/BlackList/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a></b></div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get black list values
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBlackListValues()
        {
            List<BlackListValue> btypes = (List<BlackListValue>)this._blackListService.GetAllBlackListValues(long.Parse(Request["params"]));

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = btypes.Count;
            jd.recordsFiltered = btypes.Count;
            foreach (BlackListValue ai in btypes)
            {
                string conditionStr = "";

                switch (ai.Condition)
                {
                    case 1: conditionStr = "EQUAL"; break;
                    case 2: conditionStr = "STARTS WITH"; break;
                    case 3: conditionStr = "ENDS WITH"; break;
                    case 4: conditionStr = "CONTAINS"; break;
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      ai.Value,
                                      conditionStr,
                                      "<div class=\"value_remove\" data-id='" + ai.Id.ToString() + "' onclick='value_remove(this)'><i class=\"glyphicon glyphicon-remove red\"></i></div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get custom black list values
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetCustomBlackListValues()
        {
            List<CustomBlackListValue> btypes = (List<CustomBlackListValue>)this._blackListService.GetCustomBlackListValues(long.Parse(Request["id"]), short.Parse(Request["type"]), long.Parse(Request["field"]));

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = btypes.Count;
            jd.recordsFiltered = btypes.Count;
            foreach (CustomBlackListValue ai in btypes)
            {
                string[] names1 = {
                                      ai.Id.ToString(),
                                      ai.Value,
                                      "<div class=\"value_remove\" data-id='" + ai.Id.ToString() + "' onclick='value_remove(this)'><i class=\"glyphicon glyphicon-remove red\"></i></div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}