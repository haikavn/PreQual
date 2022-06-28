// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class FilterController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class FilterController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IFilterService _filterService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The vertical service
        /// </summary>
        private readonly IVerticalService _verticalService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="filterService">The filter service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="verticalService">The vertical service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="appContext">Application Context</param>
        public FilterController(IFilterService filterService, ICampaignService campaignService, ICampaignTemplateService campaignTemplateService, IVerticalService verticalService, ILocalizedStringService localizedStringService, IHistoryService historyService, IAppContext appContext)
        {
            this._filterService = filterService;
            this._localizedStringService = localizedStringService;
            this._campaignService = campaignService;
            this._campaignTemplateService = campaignTemplateService;
            this._verticalService = verticalService;
            this._historyService = historyService;
            this._appContext = appContext;
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
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult List()
        {
            return View();
        }

        protected void PrepareFilterConditions(FilterModel model, long parentId = 0)
        {
            if (model.FilterConditions == null)
                model.FilterConditions = new List<FilterCondition>();

            var conditions = (List<Core.Domain.Lead.FilterCondition>)_filterService.GetFilterConditionsByFilterId(model.FilterId, parentId);

            foreach (FilterCondition item in conditions)
            {
                model.FilterConditions.Add(item);
                PrepareFilterConditions(model, item.Id);
            }
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(FilterModel model)
        {
            List<Vertical> verticals = (List<Vertical>)_verticalService.GetAllVerticals();

            int i = 0;
            int cc = 0;

            foreach (var v in verticals)
            {
                if (model.VerticalId == v.Id) i = cc;
                model.ListVertical.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString(), Selected = (model.VerticalId == 0 || model.VerticalId == v.Id ? true : false) });
                cc++;
            }

            if (verticals.Count > 0)
            {
                List<Campaign> campaigns = (List<Campaign>)_campaignService.GetCampaignsByVerticalId(verticals[i].Id);

                long cid = 0;

                foreach (var c in campaigns)
                {
                    if (cid == 0)
                        cid = c.Id;
                    model.ListCampaign.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (model.CampaignId == c.Id ? true : false) });
                }

                if (model.CampaignId > 0) cid = model.CampaignId;
                else
                    model.CampaignId = cid;

                model.CampaignTemplate = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(cid, isfilterable: 1);
            }

            PrepareFilterConditions(model);
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Framework.NavigationBreadCrumb(Clear = false, Label = "Filter")]
        public ActionResult Item(long id = 0)
        {
            Adrack.Core.Domain.Lead.Filter filter = this._filterService.GetFilterById(id);

            FilterModel am = new FilterModel();

            am.FilterId = 0;

            if (filter != null)
            {
                am.FilterId = filter.Id;
                am.Name = filter.Name;

                am.VerticalId = filter.VerticalId.HasValue ? filter.VerticalId.Value : 0;
                am.CampaignId = filter.CampaignId;
            }
            else if (id < 0)
            {
                Session["CampaignId"] = (int)Math.Abs(id);

                Campaign c = _campaignService.GetCampaignById((int)Math.Abs(id));
                if (c != null)
                {
                    am.CampaignId = (int)Math.Abs(id);
                    am.VerticalId = c.VerticalId;
                }

                PrepareModel(am);

                return RedirectToAction("Item", new { id = 0 });
            }
            else if (Session["CampaignId"] != null)
            {
                Campaign c = _campaignService.GetCampaignById((int)Session["CampaignId"]);

                if (c != null)
                {
                    am.CampaignId = (int)Session["CampaignId"];
                    am.VerticalId = c.VerticalId;
                }

                Session["CampaignId"] = null;

                am.IsCampaignReadOnly = true;
            }

            PrepareModel(am);

            //return View(am);
            return View(am);
        }

        /// <summary>
        /// Items the specified filter model.
        /// </summary>
        /// <param name="FilterModel">The filter model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Item(FilterModel FilterModel, string returnUrl)
        {
            Adrack.Core.Domain.Lead.Filter filter = null;

            if (FilterModel.FilterId == 0)
            {
                filter = new Adrack.Core.Domain.Lead.Filter();
            }
            else
            {
                filter = _filterService.GetFilterById(FilterModel.FilterId);
            }

            filter.Name = FilterModel.Name;

            filter.CampaignId = FilterModel.CampaignId;
            filter.VerticalId = 1;

            if (FilterModel.FilterId == 0)
            {
                long newId = _filterService.InsertFilter(filter);
                this._historyService.AddHistory("FilterController", HistoryAction.Filter_Added, "Affiliate", newId, "Name:" + FilterModel.Name, "", "", this._appContext.AppUser.Id);
            }
            else
            {
                _filterService.UpdateFilter(filter);
                this._historyService.AddHistory("FilterController", HistoryAction.Filter_Edited, "Affiliate", FilterModel.FilterId, "Name:" + FilterModel.Name, "", "", this._appContext.AppUser.Id);
            }

            string json = Request.Unvalidated["json"];
            dynamic o = JsonConvert.DeserializeObject(json);

            _filterService.DeleteFilterConditions(filter.Id);

            long parentId = 0;

            for (int i = 0; i < o.Count; i++)
            {
                string field = o[i]["field"].ToString();
                string condition = o[i]["condition"].ToString();
                string value = o[i]["value"].ToString();
                string op = o[i]["operator"].ToString();
                string parent = o[i]["parent"].ToString();

                FilterCondition fc = new FilterCondition();

                fc.FilterId = filter.Id;
                fc.Condition = short.Parse(condition);
                fc.Value = value.Trim();
                fc.ConditionOperator = short.Parse(op);
                fc.CampaignTemplateId = long.Parse(field);
                fc.ParentId = (parent != "0" ? parentId : 0);

                this._filterService.InsertFilterCondition(fc);

                if (parent == "0")
                    parentId = fc.Id;
            }

            FilterModel.FilterId = filter.Id;

            PrepareModel(FilterModel);

            return View("List");
        }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetFilters()
        {
            string p = Request["params"];

            List<Adrack.Core.Domain.Lead.Filter> filters = (List<Adrack.Core.Domain.Lead.Filter>)this._filterService.GetAllFilters();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = filters.Count;
            jd.recordsFiltered = filters.Count;
            foreach (Adrack.Core.Domain.Lead.Filter ai in filters)
            {
                if (!string.IsNullOrEmpty(p) && p != ai.CampaignId.ToString()) continue;

                Campaign campaign = _campaignService.GetCampaignById(ai.CampaignId);

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"/Management/Filter/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      campaign == null ? "Unknown" : "<a href=\"/Management/Campaign/Item/" + campaign.Id.ToString() + "\">" + campaign.Name + "</a>",
                                     "<a href='#' onclick='deleteFilterSet(" + ai.Id.ToString() + "," + ai.CampaignId.ToString() + ")'>Delete</a>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the filter set.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteFilterSet()
        {
            long id = 0;

            if (long.TryParse(Request["id"], out id))
            {
                Adrack.Core.Domain.Lead.Filter filter = _filterService.GetFilterById(id);
                if (filter != null)
                {
                    _filterService.DeleteFilter(filter);
                }

            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the filter conditions.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetFilterConditions()
        {
            long l;

            List<object> results = new List<object>();

            if (!long.TryParse(Request["filterid"], out l))
            {
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            FilterModel model = new FilterModel();
            model.FilterId = long.Parse(Request["filterid"]);
            PrepareFilterConditions(model);

            foreach (FilterCondition ai in model.FilterConditions)
            {
                results.Add(new { field = ai.CampaignTemplateId, condition = ai.Condition, op = ai.ConditionOperator, value = ai.Value, value2 = ai.Value2, parent = (ai.ParentId.HasValue ? ai.ParentId.Value : 0) });
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the filters by campaign identifier.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetFiltersByCampaignId()
        {
            List<Adrack.Core.Domain.Lead.Filter> filters = (List<Adrack.Core.Domain.Lead.Filter>)_filterService.GetFiltersByCampaignId(long.Parse(Request["campaignid"]));

            List<object> results = new List<object>();

            foreach (Adrack.Core.Domain.Lead.Filter ai in filters)
            {
                results.Add(new { id = ai.Id.ToString(), name = ai.Name });
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}