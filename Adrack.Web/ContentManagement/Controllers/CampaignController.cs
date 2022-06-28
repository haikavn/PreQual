// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CampaignController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class CampaignController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class CampaignController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

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

        /// <summary>
        /// The report service
        /// </summary>
        private readonly IReportService _reportService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="verticalService">The vertical service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        public CampaignController(ICampaignService campaignService, IBuyerChannelService buyerChannelService, ICampaignTemplateService campaignTemplateService, IVerticalService verticalService, ILocalizedStringService localizedStringService, IHistoryService historyService, IAppContext appContext, IReportService reportService, IBuyerService buyerService, IUserService userService, IAffiliateChannelService affiliateChannelService)
        {
            this._campaignService = campaignService;
            this._localizedStringService = localizedStringService;
            this._verticalService = verticalService;
            this._campaignTemplateService = campaignTemplateService;
            this._buyerChannelService = buyerChannelService;
            this._historyService = historyService;
            this._appContext = appContext;
            this._reportService = reportService;
            this._buyerService = buyerService;
            this._userService = userService;
            this._affiliateChannelService = affiliateChannelService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the validator type HTML.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="model">The model.</param>
        /// <returns>System.String.</returns>
        public string GetValidatorTypeHtml(short type, object model)
        {
            ValidatorTypeController c = Helper.CreateController<ValidatorTypeController>();
            return c.GetValidatorTypeHtml(type, model);
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Campaigns")]
        public ActionResult List()
        {
            ViewBag.AllCampaignsList = (List<Campaign>)this._campaignService.GetAllCampaigns();
            return View();
        }

        /// <summary>
        /// Templates the list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Campaigns")]
        public ActionResult TemplateList()
        {
            return View();
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        protected void LoadItems(XmlNode parent, List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> children)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem();
                ti.title = node.Name;
                LoadItems(node, ti.children);
                if (ti.children.Count > 0)
                {
                    ti.Id = 0;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.Validator = 0;
                    ti.DatabaseField = "";
                    ti.Required = false;
                    ti.IsHash = false;
                    ti.IsHidden = false;
                    ti.IsFilterable = false;
                    ti.Label = "";
                    ti.ColumnNumber = 0;
                    ti.PageNumber = 0;
                    ti.IsFormField = false;
                    ti.OptionValues = "";
                    ti.FieldType = 0;
                    ti.Description = "";
                    ti.PossibleValue = "";
                }
                else
                {
                    ti.Id = 0;
                    ti.folder = false;
                    ti.expanded = false;
                    ti.Validator = 0;
                    ti.DatabaseField = "";
                    ti.Required = false;
                    ti.IsHash = false;
                    ti.IsHidden = false;
                    ti.IsFilterable = false;
                    ti.Label = "";
                    ti.ColumnNumber = 0;
                    ti.PageNumber = 0;
                    ti.IsFormField = false;
                    ti.OptionValues = "";
                    ti.FieldType = 0;
                    ti.Description = "";
                    ti.PossibleValue = "";
                }
                children.Add(ti);
            }
        }

        /// <summary>
        /// Loads the campaign template list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadCampaignTemplateList()
        {
            long cid = 0;
            long.TryParse(Request["campaignid"], out cid);

            bool dbOnly = false;
            bool.TryParse(Request["dbonly"], out dbOnly);

            bool? isFilterable = null;
            bool f = false;
            if (bool.TryParse(Request["filterable"], out f))
            {
                isFilterable = f;
            }

            Campaign campaign = _campaignService.GetCampaignById(cid);

            List<object> items = new List<object>();

            if (campaign != null)
            {
                List<CampaignField> fields = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(cid);

                foreach (CampaignField ct in fields)
                {
                    if (dbOnly && (string.IsNullOrEmpty(ct.DatabaseField) || ct.DatabaseField.ToLower() == "none")) continue;
                    if (isFilterable.HasValue && (!ct.IsFilterable.HasValue || isFilterable.Value != ct.IsFilterable)) continue;

                    items.Add(new { id = ct.Id, name = ct.TemplateField, validator = ct.Validator, parent = ct.SectionName });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign template.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="campaignFields">The campaign fields.</param>
        /// <returns>CampaignModel.TreeItem.</returns>
        [NonAction]
        public CampaignModel.TreeItem GetCampaignTemplate(Campaign campaign, List<CampaignField> campaignFields, bool isClone)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem parent = new Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem();
            parent.Id = 0;
            parent.title = "root";
            parent.folder = true;
            parent.expanded = true;
            parent.Validator = 0;
            parent.Description = "";
            parent.Required = false;
            parent.IsHash = false;
            parent.IsHidden = false;
            parent.IsFilterable = false;
            parent.DatabaseField = "";
            parent.MinLength = 0;
            parent.MaxLength = 0;
            parent.DataFormatHtml = "";
            parent.Label = "";
            parent.ColumnNumber = 0;
            parent.PageNumber = 0;
            parent.IsFormField = false;
            parent.OptionValues = "";
            parent.FieldType = 0;
            parent.FieldFilterSettings = "";
            parent.ValidatorValue = "";

            if (campaign != null)
            {
                List<CampaignField> fields = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesBySection(campaign.Id, "root");
                foreach (CampaignField ct in fields)
                {
                    campaignFields.Add(ct);

                    Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem();
                    ti.Id = (!isClone ? ct.Id : 0);
                    ti.title = ct.TemplateField;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.Validator = ct.Validator;
                    ti.DatabaseField = ct.DatabaseField;
                    ti.Description = ct.Description;
                    ti.PossibleValue = ct.PossibleValue;
                    ti.Required = ct.Required;
                    ti.IsHash = (bool)ct.IsHash;
                    ti.IsFilterable = (bool)ct.IsFilterable;
                    ti.IsHidden = (bool)ct.IsHidden;
                    ti.MaxLength = ct.MaxLength;
                    ti.MinLength = ct.MinLength;
                    ti.Label = ct.Label;
                    ti.ColumnNumber = (ct.ColumnNumber.HasValue ? ct.ColumnNumber.Value : 0);
                    ti.PageNumber = (ct.PageNumber.HasValue ? ct.PageNumber.Value : 0);
                    ti.IsFormField = (ct.IsFormField.HasValue ? ct.IsFormField.Value : false);
                    ti.OptionValues = ct.OptionValues;
                    ti.FieldType = (ct.FieldType.HasValue ? ct.FieldType.Value : (short)0);
                    ti.FieldFilterSettings = (!string.IsNullOrEmpty(ct.FieldFilterSettings) ? ct.FieldFilterSettings : "");
                    ti.ValidatorValue = (!string.IsNullOrEmpty(ct.ValidatorSettings) ? ct.ValidatorSettings : "");
                    parent.DataFormatHtml = this.GetValidatorTypeHtml(ct.Validator, ct.ValidatorSettings);

                    /*switch (ti.DatabaseField.ToLower())
                    {
                        case "ip": ti.Validators = "1"; break;
                        case "minprice": ti.Validators = "2,16"; break;
                        case "firstname": ti.Validators = "1"; break;
                        case "lastname": ti.Validators = "1"; break;
                        case "address": ti.Validators = "1"; break;
                        case "city": ti.Validators = "1"; break;
                        case "state": ti.Validators = "1"; break;
                        case "zip": ti.Validators = "1,7"; break;
                        case "dob": ti.Validators = "10,14"; break;
                        case "age": ti.Validators = "2"; break;
                        case "requestedamount": ti.Validators = "2,16"; break;
                        case "ssn": ti.Validators = "1,6"; break;
                        case "homephone": ti.Validators = "1,8"; break;
                        case "cellphone": ti.Validators = "1,8"; break;
                        case "email": ti.Validators = "1,3"; break;
                        case "payfrequency": ti.Validators = "2"; break;
                        case "directdeposit": ti.Validators = "2"; break;
                        case "accounttype": ti.Validators = "1"; break;
                        case "incometype": ti.Validators = "1"; break;
                        case "netmonthlyincome": ti.Validators = "2,16"; break;
                        case "emptime": ti.Validators = "2"; break;
                        case "addressmonth": ti.Validators = "2"; break;
                        case "affiliatesubid": ti.Validators = "1,13"; break;
                        case "affiliatesubid2": ti.Validators = "1,13"; break;
                    }*/

                    LoadChildren(campaign, ct.TemplateField, ti.children, campaignFields, isClone);

                    //fix from Arman:buyer channel folder condition
                    if (ti.children == null || ti.children.Count == 0)
                        ti.folder = false;
                    parent.children.Add(ti);
                }
            }

            items.Add(parent);

            return items[0];

            //return Json(new { items = items[0], xml = campaign.XmlTemplate }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the items from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>List&lt;Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem&gt;.</returns>
        [NonAction]
        public List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> GetItemsFromXml(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);

            Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem();
            ti.Id = 0;
            ti.title = xmldoc.DocumentElement.Name;
            ti.folder = true;
            ti.expanded = true;
            ti.Validator = 0;
            ti.Required = false;
            ti.IsHash = false;
            ti.IsHidden = false;
            ti.IsFilterable = false;
            ti.DatabaseField = "";
            ti.Label = "";
            ti.ColumnNumber = 0;
            ti.PageNumber = 0;
            ti.IsFormField = false;
            ti.OptionValues = "";
            ti.FieldType = 0;
            ti.Description = "";
            ti.PossibleValue = "";
            ti.FieldFilterSettings = "";
            ti.ValidatorValue = "";

            LoadItems(xmldoc.DocumentElement, ti.children);

            //fix from Arman:buyer channel folder condition
            if (ti.children == null || ti.children.Count == 0)
                ti.folder = false;

            List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem>();
            items.Add(ti);

            return items;
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadFromXml()
        {
            long campaignId = 0;
            string xml = Request.Unvalidated["xml"];
            if (string.IsNullOrEmpty(xml) && long.TryParse(Request["campaignid"], out campaignId))
            {
                Campaign campaign = _campaignService.GetCampaignById(campaignId);
                if (campaign != null)
                {
                    xml = campaign.DataTemplate;
                }
            }

            List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> items = GetItemsFromXml(xml);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET: Affiliate
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            List<Campaign> campaigns = (List<Campaign>)this._campaignService.GetAllCampaigns();

            return View();
        }

        /// <summary>
        /// Gets all campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetAllCampaigns()
        {
            List<Campaign> allCampaigns = (List<Campaign>)this._campaignService.GetAllCampaigns();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;

            foreach (Campaign cam in allCampaigns)
            {
                string[] names1 = {
                                      cam.Id.ToString(),
                                      "<div class='no-margin text-bold'>" + cam.Name + "</div>",
                                      cam.Description,
                                      ""
                                      //"<a href='/Management/AffiliateChannel/Create?campaignid=" + cam.Id.ToString() + "'>Create Affiliate channel</a>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(CampaignModel model)
        {
            model.ListPingTreeCycle.Add(new SelectListItem { Text = "Distribute each 5 leads", Value = "5" });
            model.ListPingTreeCycle.Add(new SelectListItem { Text = "Distribute each 10 leads", Value = "10" });
            model.ListPingTreeCycle.Add(new SelectListItem { Text = "Distribute each 20 leads", Value = "20" });
            model.ListPingTreeCycle.Add(new SelectListItem { Text = "Distribute each 100 leads", Value = "100" });
            model.ListPingTreeCycle.Add(new SelectListItem { Text = "Ignore percentage distribution", Value = "0" });


            model.ListStatus.Add(new SelectListItem { Text = "Inactive", Value = "0" });
            model.ListStatus.Add(new SelectListItem { Text = "Active", Value = "1" });

            model.ListVisibility.Add(new SelectListItem { Text = "Public", Value = "0" });
            model.ListVisibility.Add(new SelectListItem { Text = "Private", Value = "1" });
            model.ListVisibility.Add(new SelectListItem { Text = "Apply and run", Value = "2" });

            PropertyInfo[] properties = typeof(LeadContent).GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.ToLower() == "id" ||
                    pi.Name.ToLower() == "leadid" ||
                    pi.Name.ToLower() == "affiliateid" ||
                    pi.Name.ToLower() == "campaigntype" ||
                    pi.Name.ToLower() == "minpricestr" ||
                    pi.Name.ToLower() == "created") continue;
                model.ListSystemField.Add(new SelectListItem() { Text = pi.Name, Value = pi.Name });
            }
            model.ListSystemField = model.ListSystemField.OrderBy(x => x.Text).ToList();
            model.ListSystemField.Insert(0, new SelectListItem() { Text = "NONE", Value = "NONE" });

            model.ListDataType.Add(new SelectListItem() { Text = "String", Value = "1" });
            model.ListDataType.Add(new SelectListItem() { Text = "Number", Value = "2" });
            model.ListDataType.Add(new SelectListItem() { Text = "EMail", Value = "3" });
            model.ListDataType.Add(new SelectListItem() { Text = "Unsigned number", Value = "4" });
            model.ListDataType.Add(new SelectListItem() { Text = "Account number", Value = "5" });
            model.ListDataType.Add(new SelectListItem() { Text = "SSN", Value = "6" });
            model.ListDataType.Add(new SelectListItem() { Text = "ZIP", Value = "7" });
            model.ListDataType.Add(new SelectListItem() { Text = "Phone", Value = "8" });
            model.ListDataType.Add(new SelectListItem() { Text = "Password", Value = "9" });
            model.ListDataType.Add(new SelectListItem() { Text = "DateTime", Value = "10" });
            model.ListDataType.Add(new SelectListItem() { Text = "State", Value = "11" });
            model.ListDataType.Add(new SelectListItem() { Text = "Routing number", Value = "12" });
            model.ListDataType.Add(new SelectListItem() { Text = "Affiliate refferal field", Value = "13" });
            model.ListDataType.Add(new SelectListItem() { Text = "Date of birth", Value = "14" });
            model.ListDataType.Add(new SelectListItem() { Text = "Regular expression", Value = "15" });
            model.ListDataType.Add(new SelectListItem() { Text = "Decimal", Value = "16" });
            model.ListDataType.Add(new SelectListItem() { Text = "Sub Id", Value = "17" });
            model.ListDataType = model.ListDataType.OrderBy(x => x.Text).ToList();
            model.ListDataType.Insert(0, new SelectListItem() { Text = "NONE", Value = "0" });

            model.ListCampaignType.Add(new SelectListItem { Text = "Lead", Value = "0" });
            //model.ListCampaignType.Add(new SelectListItem { Text = "Click", Value = "1" });

            List<Vertical> vlist = (List<Vertical>)_verticalService.GetAllVerticals();

            model.ListVertical.Add(new SelectListItem() { Text = "Select vertical", Value = "", Selected = true });

            foreach (Vertical item in vlist)
            {
                model.ListVertical.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = item.Id == model.VerticalId });
            }
        }

        /// <summary>
        /// Loads the children.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        /// <param name="campaignFields">The campaign fields.</param>
        protected void LoadChildren(Campaign campaign, string parent, List<Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem> children, List<CampaignField> campaignFields, bool isClone)
        {
            List<CampaignField> fields = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesBySection(campaign.Id, parent);

            foreach (CampaignField node in fields)
            {
                campaignFields.Add(node);

                Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.CampaignModel.TreeItem();
                ti.title = node.TemplateField;
                LoadChildren(campaign, node.TemplateField, ti.children, campaignFields, isClone);
                if (ti.children.Count > 0)
                {
                    ti.Id = (!isClone ? node.Id : 0);
                    ti.folder = true;
                    ti.expanded = true;
                    ti.Validator = 0;
                    ti.DatabaseField = "";
                    ti.Required = false;
                    ti.IsHash = false;
                    ti.IsFilterable = false;
                    ti.IsHidden = (bool)node.IsHidden;
                    ti.BlackListTypeId = 0;
                    ti.Description = "";
                    ti.PossibleValue = "";
                    ti.MinLength = 0;
                    ti.MaxLength = 0;
                    ti.DataFormatHtml = "";
                    ti.Label = "";
                    ti.ColumnNumber = 0;
                    ti.PageNumber = 0;
                    ti.IsFormField = false;
                    ti.OptionValues = "";
                    ti.FieldType = 0;
                    ti.FieldFilterSettings = "";
                    ti.ValidatorValue = "";
                }
                else
                {                    
                    ti.Id = (!isClone ? node.Id : 0);
                    ti.folder = false;
                    ti.expanded = false;
                    ti.Validator = node.Validator;
                    ti.DatabaseField = node.DatabaseField;
                    ti.title = node.TemplateField;
                    ti.Required = node.Required;
                    ti.IsHash = (bool)node.IsHash;
                    ti.IsHidden = (bool)node.IsHidden;
                    ti.IsFilterable = (bool)node.IsFilterable;
                    ti.Description = node.Description;
                    ti.PossibleValue = node.PossibleValue;
                    ti.BlackListTypeId = node.BlackListTypeId == null ? 0 : (long)node.BlackListTypeId;
                    ti.MinLength = node.MinLength;
                    ti.MaxLength = node.MaxLength;
                    ti.Label = node.Label;
                    ti.ColumnNumber = (node.ColumnNumber.HasValue ? node.ColumnNumber.Value : 0);
                    ti.PageNumber = (node.PageNumber.HasValue ? node.PageNumber.Value : 0);
                    ti.IsFormField = (node.IsFormField.HasValue ? node.IsFormField.Value : false);
                    ti.FieldType = (node.FieldType.HasValue ? node.FieldType.Value : (short)0);
                    ti.OptionValues = node.OptionValues;
                    ti.DataFormatHtml = this.GetValidatorTypeHtml(node.Validator, node.ValidatorSettings);
                    ti.FieldFilterSettings = (!string.IsNullOrEmpty(node.FieldFilterSettings) ? node.FieldFilterSettings : "");
                    ti.ValidatorValue = (!string.IsNullOrEmpty(node.ValidatorSettings) ? node.ValidatorSettings : "");
                }
                children.Add(ti);
            }
        }

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Campaign")]
        public ActionResult Create(long id = 0)
        {
            CampaignModel am = new CampaignModel();

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Forms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Campaign")]
        public ActionResult Form(long id = 0)
        {
            CampaignModel model = new CampaignModel();

            Campaign campaign = _campaignService.GetCampaignById(id);

            model.CampaignId = id;
            model.Name = campaign.Name;
            model.HtmlFormId = (campaign.HtmlFormId.HasValue ? campaign.HtmlFormId.ToString() : "");

            return View(model);
        }

        /// <summary>
        /// Forms the specified campaign model.
        /// </summary>
        /// <param name="campaignModel">The campaign model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Form(CampaignModel campaignModel, string returnUrl)
        {
            string json = Request.Unvalidated["json"];
            dynamic o = JsonConvert.DeserializeObject(json);
            Campaign campaign = _campaignService.GetCampaignById(campaignModel.CampaignId);

            if (campaign != null)
            {
                campaign.HtmlFormId = Guid.NewGuid();

                _campaignService.UpdateCampaign(campaign);

                for (int i = 0; i < o.Count; i++)
                {
                    if (o[i].Count == 0) continue;
                    string id = o[i][0].ToString();
                    string field = o[i][1].ToString();
                    string isFormField = o[i][2].ToString();
                    string label = o[i][3].ToString();
                    string columnNumber = o[i][4].ToString();
                    string pageNumber = o[i][5].ToString();
                    string fieldType = o[i][6].ToString();
                    string optionValues = o[i][7].ToString();

                    CampaignField ct = this._campaignTemplateService.GetCampaignTemplateById(int.Parse(id));

                    if (ct != null)
                    {
                        ct.Label = label;

                        try
                        {
                            ct.IsFormField = bool.Parse(isFormField);
                        }
                        catch
                        {
                            ct.IsFormField = false;
                        }
                        try
                        {
                            ct.PageNumber = int.Parse(pageNumber);
                        }
                        catch
                        {
                            ct.PageNumber = 0;
                        }

                        try
                        {
                            ct.ColumnNumber = int.Parse(columnNumber);
                        }
                        catch
                        {
                            ct.ColumnNumber = 0;
                        }

                        try
                        {
                            ct.FieldType = short.Parse(fieldType);
                        }
                        catch
                        {
                            ct.FieldType = 0;
                        }

                        ct.OptionValues = optionValues;

                        _campaignTemplateService.UpdateCampaignTemplate(ct);
                    }
                }

                string IframeCode = "<iframe width='100%' height='100%' frameBorder='0' src='" + Helper.GetBaseUrl(Request) + "/home/htmlform/" + (campaign.HtmlFormId.HasValue ? campaign.HtmlFormId.Value.ToString() : "0") + "'></iframe>";

                return Json(new { id = campaign.Id, name = campaign.Name, iframe = IframeCode, HtmlFormId = (campaign.HtmlFormId.HasValue ? campaign.HtmlFormId.Value.ToString() : "0") }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { id = 0, name = "", iframe = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// HTMLs the form.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult HtmlForm(long id)
        {
            Campaign campaign = _campaignService.GetCampaignById(id);
            Dictionary<int, List<CampaignField>> pages = new Dictionary<int, List<CampaignField>>();

            if (campaign != null)
            {
                List<CampaignField> fields = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(id);

                int MaxCols = 1;

                foreach (CampaignField field in fields)
                {
                    if (!(field.IsFormField.HasValue ? field.IsFormField.Value : false)) continue;

                    List<CampaignField> inputs = null;

                    if (!pages.ContainsKey((field.PageNumber.HasValue ? field.PageNumber.Value : 0)))
                    {
                        inputs = new List<CampaignField>();
                        pages.Add((field.PageNumber.HasValue ? field.PageNumber.Value : 0), inputs);
                    }
                    else
                    {
                        inputs = pages[(field.PageNumber.HasValue ? field.PageNumber.Value : 0)];
                    }

                    inputs.Add(field);

                    if (field.ColumnNumber > MaxCols)
                        MaxCols = (field.ColumnNumber.HasValue ? field.ColumnNumber.Value : 0);
                }
            }

            /* foreach(int p in pages.Keys)
             {
                 pages[p] = pages[p].OrderBy(x => x.ColumnNumber).ToList();
             }*/

            ViewBag.Pages = pages;
            ViewBag.CampaignId = id;

            return View();
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Campaign")]
        public ActionResult Item(long id = 0)
        {
            Campaign campaign = this._campaignService.GetCampaignById(id);

            CampaignModel am = new CampaignModel();

            am.NetworkTargetRevenue = 20;
            am.NetworkMinimumRevenue = 1;

            am.CampaignId = 0;

            if (campaign != null)
            {
                am.CampaignId = campaign.Id;
                am.Name = campaign.Name;
                am.Start = campaign.Start;
                am.Finish = campaign.Finish;
                am.XmlTemplate = campaign.DataTemplate;
                am.Status = campaign.Status;
                am.PriceFormat = campaign.PriceFormat;
                am.VerticalId = campaign.VerticalId;
                am.Visibility = 0;
                am.NetworkMinimumRevenue = Math.Round(campaign.NetworkMinimumRevenue, 0);
                am.NetworkTargetRevenue = Math.Round(campaign.NetworkTargetRevenue, 0);
                am.CampaignType = campaign.CampaignType;
                am.PingTreeCycle = campaign.PingTreeCycle;
                am.PrioritizedEnabled = (campaign.PrioritizedEnabled.HasValue ? campaign.PrioritizedEnabled.Value : true);
            }

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Items the specified campaign model.
        /// </summary>
        /// <param name="campaignModel">The campaign model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Item(CampaignModel campaignModel, string returnUrl)
        {
            Campaign campaign = null;

            if (_campaignService.GetCampaignByName(campaignModel.Name, campaignModel.CampaignId) != null)
            {
                return Json(new { error = "Campaign name already exists" }, JsonRequestBehavior.AllowGet);
            }

            string json = Request.Unvalidated["json"];
            string xml = Request.Unvalidated["xml"];
            bool changed = false;//bool.Parse(Request["changed"]);

            dynamic o = JsonConvert.DeserializeObject(json);
            string dataBefore = "";
            string dataAfter = "";

            if (campaignModel.CampaignId == 0)
            {
                campaign = new Campaign();
                campaign.CampaignKey = Helper.GetUniqueKey(7);
            }
            else
            {
                campaign = _campaignService.GetCampaignById(campaignModel.CampaignId);
                dataBefore = $"Name:{campaign.Name};NetworkMinimumRevenue:{campaign.NetworkMinimumRevenue.ToString("F2")};NetworkTargetRevenue:{campaign.NetworkTargetRevenue.ToString("F2")};";
            }

            campaign.Name = campaignModel.Name.Trim();
            campaign.Start = DateTime.UtcNow;
            campaign.Finish = DateTime.UtcNow;
            campaign.Status = campaignModel.Status;
            campaign.DataTemplate = campaignModel.XmlTemplate;
            campaign.CreatedOn = DateTime.UtcNow;
            campaign.VerticalId = campaignModel.VerticalId;
            campaign.Visibility = 0;//campaignModel.Visibility;
            campaign.IsTemplate = false;
            campaign.NetworkMinimumRevenue = Math.Round(campaignModel.NetworkMinimumRevenue, 2);
            campaign.NetworkTargetRevenue = Math.Round(campaignModel.NetworkTargetRevenue, 2);
            campaign.CampaignType = campaignModel.CampaignType;
            campaign.PingTreeCycle = campaignModel.PingTreeCycle;
            campaign.PrioritizedEnabled = campaignModel.PrioritizedEnabled;

            if (campaignModel.CampaignId == 0)
            {
                long newId = _campaignService.InsertCampaign(campaign);
                this._historyService.AddHistory("CampaignController", HistoryAction.Campaign_Added, "Campaign", newId, "Name:" + campaign.Name, "", "", this._appContext.AppUser.Id);
            }
            else
            {
                dataAfter = $"Name:{campaign.Name};NetworkMinimumRevenue:{campaign.NetworkMinimumRevenue.ToString("F2")};NetworkTargetRevenue:{campaign.NetworkTargetRevenue.ToString("F2")};";
                _campaignService.UpdateCampaign(campaign);
                this._historyService.AddHistory("CampaignController", HistoryAction.Campaign_Edited, "Campaign", campaignModel.CampaignId, dataBefore, dataAfter, "", this._appContext.AppUser.Id);
            }

            campaignModel.CampaignId = campaign.Id;

            PrepareModel(campaignModel);

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                if (campaign.CampaignType == 0)
                {
                    xmldoc.LoadXml(xml);
                }
                else
                {
                    XmlElement root = xmldoc.CreateElement("CLICK");

                    root.AppendChild(xmldoc.CreateElement("CHANNELID")).InnerText = "";
                    root.AppendChild(xmldoc.CreateElement("IP")).InnerText = "";
                    root.AppendChild(xmldoc.CreateElement("ZIPCODE")).InnerText = "";
                    root.AppendChild(xmldoc.CreateElement("STATE")).InnerText = "";
                    root.AppendChild(xmldoc.CreateElement("OS")).InnerText = "";
                    root.AppendChild(xmldoc.CreateElement("BROWSER")).InnerText = "";
                    
                    xmldoc.AppendChild(root);
                }
            }
            catch
            {
            }

            if (campaign.CampaignType == CampaignTypes.ClickCampaign)
                _campaignService.DeleteCampaignTemplates(campaign.Id);

            if (campaign.CampaignType == CampaignTypes.LeadCampaign)
            {
                for (int i = 0; i < o.Count; i++)
                {
                    if (o[i].Count == 0) continue;
                    string id = o[i][0].ToString();
                    string tfield = o[i][1].ToString();
                    string sfield = o[i][2].ToString();
                    string validator = o[i][3].ToString();
                    string comments = o[i][4].ToString();
                    string possible = o[i][5].ToString();
                    string required = o[i][6].ToString();
                    string ishash = o[i][7].ToString();
                    string ishidden = o[i][8].ToString();
                    string isfilterable = o[i][9].ToString();
                    string filterSettings = o[i][10].ToString();
                    string parent = o[i][11].ToString();
                    //string blacklist = o[i][6].ToString();

                    CampaignField ct = this._campaignTemplateService.GetCampaignTemplateById(int.Parse(id));

                    if (ct != null)
                        dataBefore = $"TemplateDield:{(!string.IsNullOrEmpty(ct.TemplateField) ? ct.TemplateField : "")},SystemField={ct.DatabaseField},Validator={ct.Validator},Required={ct.Required},Hashed={(ct.IsHash.HasValue ? ct.IsHash.Value : false)},Hidden={(ct.IsHidden.HasValue ? ct.IsHidden.Value : false)},Filterable={(ct.IsFilterable.HasValue ? ct.IsFilterable.Value : false)};";

                    bool isnew = false;

                    if (ct == null || changed)
                    {
                        ct = new CampaignField();
                        isnew = true;
                    }

                    ct.CampaignId = campaign.Id;
                    ct.DatabaseField = sfield;
                    ct.TemplateField = tfield;

                    string[] validatorStr = validator.Split(new char[1] { ';' });

                    try
                    {
                        ct.Validator = short.Parse(validatorStr[0]);
                    }
                    catch
                    {
                        ct.Validator = 0;
                    }

                    if (ct.Validator == 1)
                    {
                        try
                        {
                            ct.MinLength = short.Parse(validatorStr[1]);
                        }
                        catch
                        {
                            ct.MinLength = 0;
                        }

                        try
                        {
                            ct.MaxLength = short.Parse(validatorStr[2]);
                        }
                        catch
                        {
                            ct.MaxLength = 0;
                        }
                    }

                    ct.ValidatorSettings = "";

                    for (int j = 1; j < validatorStr.Length; j++)
                    {
                        ct.ValidatorSettings += validatorStr[j];
                        if (j < validatorStr.Length - 1)
                        {
                            ct.ValidatorSettings += ";";
                        }
                    }

                    ct.Description = comments;
                    ct.PossibleValue = possible;
                    try
                    {
                        ct.Required = bool.Parse(required);
                    }
                    catch
                    {
                        ct.Required = false;
                    }

                    try
                    {
                        ct.IsHash = bool.Parse(ishash);
                    }
                    catch
                    {
                        ct.IsHash = false;
                    }

                    try
                    {
                        ct.IsHidden = bool.Parse(ishidden);
                    }
                    catch
                    {
                        ct.IsHidden = false;
                    }

                    try
                    {
                        ct.IsFilterable = bool.Parse(isfilterable);
                    }
                    catch
                    {
                        ct.IsFilterable = false;
                    }

                    ct.SectionName = parent;
                    ct.BlackListTypeId = 0;//long.Parse(blacklist);
                    ct.FieldFilterSettings = filterSettings;

                    if (!isnew)
                    {
                        if (ct != null)
                            dataAfter = $"TemplateDield:{(!string.IsNullOrEmpty(ct.TemplateField) ? ct.TemplateField : "")},SystemField={ct.DatabaseField},Validator={ct.Validator},Required={ct.Required},Hashed={(ct.IsHash.HasValue ? ct.IsHash.Value : false)},Hidden={(ct.IsHidden.HasValue ? ct.IsHidden.Value : false)},Filterable={(ct.IsFilterable.HasValue ? ct.IsFilterable.Value : false)};";
                        this._historyService.AddHistory("CampaignController", HistoryAction.Campaign_Edited, "Campaign", ct.CampaignId, dataBefore, dataAfter, "", this._appContext.AppUser.Id);
                        _campaignTemplateService.UpdateCampaignTemplate(ct);
                    }
                    else
                    {
                        ct.IsFormField = false;
                        ct.Label = "";
                        ct.ColumnNumber = 0;
                        ct.PageNumber = 0;
                        ct.FieldType = 0;
                        ct.OptionValues = "";
                        _campaignTemplateService.InsertCampaignTemplate(ct);
                    }

                    if (parent == "root") continue;

                    XmlNodeList nodes = xmldoc.GetElementsByTagName(parent);
                    XmlElement parentEl = null;

                    if (nodes.Count == 0)
                    {
                        parentEl = xmldoc.CreateElement(parent);

                        if (xmldoc.DocumentElement == null)
                            xmldoc.AppendChild(parentEl);
                    }
                    else
                        parentEl = (XmlElement)nodes[0];

                    XmlNode foundNode = null;

                    nodes = xmldoc.GetElementsByTagName(tfield);

                    foreach (XmlNode node in nodes)
                    {
                        if (node.ParentNode != null && node.ParentNode.Name.ToLower() == parent.ToLower())
                        {
                            foundNode = node;
                            break;
                        }
                    }

                    if (foundNode == null)
                    {
                        XmlElement el = xmldoc.CreateElement(tfield);
                        parentEl.AppendChild(el);
                    }
                }
            }
            else
            {
                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "NONE",
                    TemplateField = "CLICK",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "root",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "NONE",
                    TemplateField = "CHANNELID",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "Ip",
                    TemplateField = "IP",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "Zip",
                    TemplateField = "ZIPCODE",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "State",
                    TemplateField = "STATE",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "OS",
                    TemplateField = "OS",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "Browser",
                    TemplateField = "BROWSER",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                /*_campaignTemplateService.InsertCampaignTemplate(new CampaignTemplate()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "Banners",
                    TemplateField = "BANNERS",
                    Validator = 0,
                    ValidatorValue = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });*/

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "Banners",
                    TemplateField = "BANNERS",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "SubId1",
                    TemplateField = "SUBID1",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "SubId2",
                    TemplateField = "SUBID2",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "SubId3",
                    TemplateField = "SUBID3",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });

                _campaignTemplateService.InsertCampaignTemplate(new CampaignField()
                {
                    CampaignId = campaign.Id,
                    DatabaseField = "UniqueKey",
                    TemplateField = "UNIQUEKEY",
                    Validator = 0,
                    ValidatorSettings = "",
                    Description = "",
                    PossibleValue = "",
                    Required = false,
                    IsHash = false,
                    IsHidden = false,
                    IsFilterable = false,
                    FieldFilterSettings = "",
                    SectionName = "CLICK",
                    BlackListTypeId = 0
                });
            }

            campaign.DataTemplate = xmldoc.OuterXml;

            _campaignService.UpdateCampaign(campaign);

            if (!string.IsNullOrEmpty(Request.Unvalidated["pingTree"]))
            {
                dynamic pingTree = JsonConvert.DeserializeObject(Request.Unvalidated["pingTree"]);

                for (int i = 0; i < pingTree.Count; i++)
                {
                    string strId = pingTree[i]["id"];
                    string strOrder = pingTree[i]["order"];
                    string strFixed = pingTree[i]["fixed"];
                    string strRate = pingTree[i]["rate"];

                    long id = 0;
                    int order = 0;
                    int rate = 0;
                    bool isFixed = false;
                    if (long.TryParse(strId, out id) && int.TryParse(strOrder, out order) && bool.TryParse(strFixed, out isFixed) && int.TryParse(strRate, out rate))
                    {
                        BuyerChannel bc = _buyerChannelService.GetBuyerChannelById(id);
                        if (bc != null)
                        {
                            bc.OrderNum = order;
                            bc.IsFixed = isFixed;
                            bc.LeadAcceptRate = rate;
                            _buyerChannelService.UpdateBuyerChannel(bc);
                        }
                    }
                }

                SharedData.ResetBuyerChannelLeadsCount(campaign.Id);
            }

            return Json(new { id = campaign.Id, name = campaign.Name }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Templates the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult TemplateItem(long id = 0)
        {
            Campaign campaign = this._campaignService.GetCampaignById(id);

            CampaignModel am = new CampaignModel();

            am.CampaignId = 0;

            if (campaign != null)
            {
                am.CampaignId = campaign.Id;
                am.Name = campaign.Name;
                am.Start = campaign.Start;
                am.Finish = campaign.Finish;
                am.XmlTemplate = campaign.DataTemplate;
                am.Status = campaign.Status;
                am.PriceFormat = campaign.PriceFormat;
                am.VerticalId = campaign.VerticalId;
                am.Visibility = campaign.Visibility;
            }

            PrepareModel(am);

            return View(am);
        }

        /// <summary>
        /// Templates the item.
        /// </summary>
        /// <param name="campaignModel">The campaign model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TemplateItem(CampaignModel campaignModel, string returnUrl)
        {
            Campaign campaign = null;

            string json = Request.Unvalidated["json"];
            string xml = Request.Unvalidated["xml"];
            bool changed = false;
            bool.TryParse(Request["changed"], out changed);

            dynamic o = JsonConvert.DeserializeObject(json);

            if (campaignModel.CampaignId == 0)
            {
                campaign = new Campaign();
            }
            else
            {
                campaign = _campaignService.GetCampaignById(campaignModel.CampaignId);
            }

            campaign.Name = campaignModel.Name;
            campaign.Start = DateTime.UtcNow;
            campaign.Finish = DateTime.UtcNow;
            campaign.Status = 0;
            campaign.DataTemplate = campaignModel.XmlTemplate;
            campaign.CreatedOn = DateTime.UtcNow;
            campaign.VerticalId = campaignModel.VerticalId;
            campaign.Visibility = 0;
            campaign.IsTemplate = true;

            if (campaignModel.CampaignId == 0)
                _campaignService.InsertCampaign(campaign);
            else
                _campaignService.UpdateCampaign(campaign);

            campaignModel.CampaignId = campaign.Id;

            PrepareModel(campaignModel);

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                if (xml!="")
                    xmldoc.LoadXml(xml);
            }
            catch
            {
            }

            _campaignService.DeleteCampaignTemplates(campaign.Id);

            for (int i = 0; i < o.Count; i++)
            {
                if (o[i].Count == 0) continue;
                string id = o[i][0].ToString();
                string tfield = o[i][1].ToString();
                string sfield = o[i][2].ToString();
                string validator = o[i][3].ToString();
                string comments = o[i][4].ToString();
                string possible = o[i][5].ToString();
                string required = o[i][6].ToString();
                string ishash = o[i][7].ToString();
                string ishidden = o[i][8].ToString();
                string isfilterable = o[i][9].ToString();
                string parent = o[i][10].ToString();
                //string blacklist = o[i][6].ToString();

                CampaignField ct = this._campaignTemplateService.GetCampaignTemplateById(int.Parse(id));

                bool isnew = false;

                if (ct == null || changed)
                {
                    ct = new CampaignField();
                    isnew = true;
                }

                ct.CampaignId = campaign.Id;
                ct.DatabaseField = sfield;
                ct.TemplateField = tfield;

                string[] validatorStr = validator.Split(new char[1] { ';' });

                try
                {
                    ct.Validator = short.Parse(validatorStr[0]);
                }
                catch
                {
                    ct.Validator = 0;
                }

                if (ct.Validator == 1)
                {
                    try
                    {
                        ct.MinLength = short.Parse(validatorStr[1]);
                    }
                    catch
                    {
                        ct.MinLength = 0;
                    }

                    try
                    {
                        ct.MaxLength = short.Parse(validatorStr[2]);
                    }
                    catch
                    {
                        ct.MaxLength = 0;
                    }
                }

                ct.ValidatorSettings = "";

                for (int j = 1; j < validatorStr.Length; j++)
                {
                    ct.ValidatorSettings += validatorStr[j];
                    if (j < validatorStr.Length - 1)
                    {
                        ct.ValidatorSettings += ";";
                    }
                }

                ct.Description = comments;
                ct.PossibleValue = possible;
                try
                {
                    ct.Required = bool.Parse(required);
                }
                catch
                {
                    ct.Required = false;
                }

                try
                {
                    ct.IsHash = bool.Parse(ishash);
                }
                catch
                {
                    ct.IsHash = false;
                }

                try
                {
                    ct.IsHidden = bool.Parse(ishidden);
                }
                catch
                {
                    ct.IsHidden = false;
                }

                try
                {
                    ct.IsFilterable = bool.Parse(isfilterable);
                }
                catch
                {
                    ct.IsFilterable = false;
                }

                ct.SectionName = parent;
                ct.BlackListTypeId = 0;//long.Parse(blacklist);

                if (!isnew)
                    _campaignTemplateService.UpdateCampaignTemplate(ct);
                else
                    _campaignTemplateService.InsertCampaignTemplate(ct);

                if (parent == "root") continue;

                XmlNodeList nodes = xmldoc.GetElementsByTagName(parent);
                XmlElement parentEl = null;

                if (nodes.Count == 0)
                {
                    parentEl = xmldoc.CreateElement(parent);

                    if (xmldoc.DocumentElement == null)
                        xmldoc.AppendChild(parentEl);
                }
                else
                    parentEl = (XmlElement)nodes[0];

                XmlNode foundNode = null;

                nodes = xmldoc.GetElementsByTagName(tfield);

                foreach (XmlNode node in nodes)
                {
                    if (node.ParentNode != null && node.ParentNode.Name.ToLower() == parent.ToLower())
                    {
                        foundNode = node;
                        break;
                    }
                }

                if (foundNode == null)
                {
                    XmlElement el = xmldoc.CreateElement(tfield);
                    parentEl.AppendChild(el);
                }
            }

            campaign.DataTemplate = xmldoc.OuterXml;

            _campaignService.UpdateCampaign(campaign);

            return Json(new { id = 0 });//View("TemplateList");
        }

        /// <summary>
        /// Gets the campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetCampaigns()
        {
            short deleted = 0;

            short.TryParse(Request["d"], out deleted);            

            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            List<Campaign> campaigns = (List<Campaign>)this._campaignService.GetAllCampaigns(deleted);

            long SoldSum = 0;
            long ReceivedSum = 0;
            long PostedSum = 0;
            decimal BPriceSum = 0;
            decimal ProfitSum = 0;
            decimal APriceSum = 0;

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;
            foreach (Campaign ai in campaigns)
            {
                /*List<ReportByDays> Report = (List<ReportByDays>)_reportService.ReportByDays(new DateTime(2016, 1, 1), DateTime.Now, ai.Id, 0);
                foreach (ReportByDays r in Report)
                {
                    SoldSum += r.Sold;
                    ReceivedSum += r.Received;
                    PostedSum += r.Total;
                    BPriceSum += r.BuyerPrice;
                    ProfitSum += r.Profit;
                    APriceSum += r.Profit - r.BuyerPrice;
                }*/

                Vertical vertical = _verticalService.GetVerticalById(ai.VerticalId);

                string[] names1 = {
                                      ai.Id.ToString(),
                                      permissionService.Authorize(PermissionProvider.CampaignsModify) ? "<a href=\"/Management/Campaign/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>" : "<b>" + ai.Name + "</b>",
                                      vertical == null ? "" : vertical.Name,
                                      "<div style='text-align:center;'>" + String.Format("{0:###,###,###}", ReceivedSum) + "</div>",
                                      "<div style='text-align:center;'>" + String.Format("{0:###,###,###}", PostedSum) + "</div>",
                                      "<div style='text-align:center;'>" + String.Format("{0:###,###,###}", SoldSum) + "</div>",
                                      "<div style='text-align:center;'>" + String.Format("{0:$###,###,###.00}", BPriceSum) + "</div>",
                                      "<div style='text-align:center;'>" + String.Format("{0:$###,###,###.00}", APriceSum) + "</div>",
                                      "<div style='text-align:center;'>" + String.Format("{0:$###,###,###.00}", ProfitSum)  + "</div>",
                                      ai.Status != 0 ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>",
                                      "<a href='#' onclick='deleteCampaign(" + ai.Id + ")'>" + (ai.IsDeleted.HasValue ? (ai.IsDeleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the can delete.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns>System.String.</returns>
        [NonAction]
        public string CheckCanDelete(Campaign campaign)
        {
            List<BuyerChannel> buyerChannels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannelsByCampaignId(campaign.Id, 0);

            if (buyerChannels.Count > 0)
            {
                return "Can not delete campaign because there are active buyer channels.";
            }

            /*foreach (BuyerChannel bc in buyerChannels)
            {
                if (campaign.Deleted.HasValue && campaign.Deleted.Value)
                {
                    List<UserBuyerChannel> userBuyerChannels = (List<UserBuyerChannel>)_userService.GetBuyerChannelUsers(bc.Id);
                    foreach (UserBuyerChannel ubc in userBuyerChannels)
                    {
                        _userService.DetachBuyerChannel(ubc);
                    }
                }

                bc.Deleted = campaign.Deleted;
                _buyerChannelService.UpdateBuyerChannel(bc);
            }*/

            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByCampaignId(campaign.Id, 0);

            if (affiliateChannels.Count > 0)
            {
                return "Can not delete campaign because there are active affiliate channels.";
            }

            /*foreach (AffiliateChannel ac in affiliateChannels)
            {
                ac.Deleted = campaign.Deleted;
                _affiliateChannelService.UpdateAffiliateChannel(ac);
            }*/

            return "";
        }

        /// <summary>
        /// Deletes the campaign.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteCampaign()
        {
            string message = "";

            long campaignId = 0;

            if (long.TryParse(Request["campaignid"], out campaignId))
            {
                Campaign campaign = _campaignService.GetCampaignById(campaignId);
                if (campaign != null)
                {
                    if (campaign.IsDeleted.HasValue)
                        campaign.IsDeleted = !campaign.IsDeleted.Value;
                    else
                        campaign.IsDeleted = true;

                    if (campaign.IsDeleted.Value)
                        message = CheckCanDelete(campaign);

                    if (message.Length > 0)
                    {
                        campaign.IsDeleted = !campaign.IsDeleted.Value;
                        return Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    _campaignService.UpdateCampaign(campaign);

                    this._historyService.AddHistory("CampaignController", HistoryAction.Campaign_Deleted, "Affiliate", campaign.Id, "Name:" + campaign.Name, "", "", this._appContext.AppUser.Id);
                }
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign templates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetCampaignTemplates()
        {
            List<Campaign> campaigns = (List<Campaign>)this._campaignService.GetTemplateCampaigns();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = campaigns.Count;
            jd.recordsFiltered = campaigns.Count;
            foreach (Campaign ai in campaigns)
            {
                Vertical vertical = _verticalService.GetVerticalById(ai.VerticalId);

                string[] names1 = {
                                      "<div style='text-align:center;'>" + ai.Id.ToString() + "</div>",
                                      "<div style='text-align:center;'><b><a href=\"/Management/Campaign/TemplateItem/" + ai.Id.ToString() + "\">" + ai.Name + "</a><b></div>",
                                      vertical == null ? "" : "<div style='text-align:center;'>" + vertical.Name + "</div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign possible values.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetCampaignPossibleValues()
        {
            List<CampaignField> templates = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(long.Parse(Request["campaignid"]));

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = templates.Count;
            jd.recordsFiltered = templates.Count;
            foreach (CampaignField ai in templates)
            {
                if (ai.DatabaseField.ToLower() == "none") continue;

                string[] names1 = {
                                      ai.TemplateField,
                                      "<input type='text' value='" + ai.PossibleValue + "' />"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaigns by vertical identifier.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCampaignsByVerticalId()
        {
            long verticalId = long.Parse(Request["verticalId"]);

            var campaigns = _campaignService.GetCampaignsByVerticalId(verticalId, 0);

            var result = new List<object>();

            result.Add(new { id = 0, name = "Select campaign" });

            foreach (var c in campaigns)
            {
                if (c.IsTemplate) continue;
                result.Add(new { id = c.Id, name = c.Name });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign templates by vertical identifier.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCampaignTemplatesByVerticalId()
        {
            long verticalId = long.Parse(Request["verticalId"]);

            var campaigns = _campaignService.GetCampaignTemplatesByVerticalId(verticalId);

            var result = new List<object>();

            result.Add(new { id = 0, name = "Select campaign templates" });

            foreach (var c in campaigns)
            {
                if (!c.IsTemplate) continue;
                result.Add(new { id = c.Id, name = c.Name });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign fields.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCampaignFields()
        {
            long campaignid = long.Parse(Request["campaignid"]);

            var campaigns = _campaignTemplateService.GetCampaignTemplatesByCampaignId(campaignid, isfilterable: 1);

            var result = new List<object>();

            result.Add(new { id = 0, name = "Select campaign field" });

            foreach (var c in campaigns)
            {
                int filterValueType = 0;
                string filterValueContent = "";

                if (!string.IsNullOrEmpty(c.FieldFilterSettings))
                {
                    try
                    {
                        JObject jobject = JObject.Parse(c.FieldFilterSettings);
                        filterValueType = jobject.Value<int>("filterType");
                        filterValueContent = jobject.Value<string>("filterTypeValue");
                    }
                    catch
                    {

                    }
                }

                result.Add(new { id = c.Id, name = c.TemplateField, validator = c.Validator, parent = c.SectionName, filterSettings = c.FieldFilterSettings, validatorValue = c.ValidatorSettings });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCampaignInfo()
        {
            long campaignid = long.Parse(Request["campaignid"]);

            bool isClone = false;
            bool.TryParse(Request["isClone"], out isClone);

            var campaign = _campaignService.GetCampaignById(campaignid);

            List<CampaignField> fields = new List<CampaignField>();

            CampaignModel.TreeItem items = GetCampaignTemplate(campaign, fields, isClone);

            List<object> fieldItems = new List<object>();

            foreach (CampaignField ct in fields)
            {
                fieldItems.Add(new { id = ct.Id, name = ct.TemplateField, validator = ct.Validator, parent = ct.SectionName });
            }

            return Json(new { items = items, fields = fieldItems, xml = campaign.DataTemplate, NetworkTargetRevenue = campaign.NetworkTargetRevenue, NetworkMinimumRevenue = campaign.NetworkMinimumRevenue, CampaignType = campaign.CampaignType }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <param name="tpl">The TPL.</param>
        /// <returns>System.String.</returns>
        [NonAction]
        public string GetInput(CampaignField tpl)
        {
            if (!(tpl.IsFormField.HasValue ? tpl.IsFormField.Value : false)) return "";

            string input = "";

            if (tpl.Label.Length > 0)
            {
                input += "<label for='" + tpl.SectionName + "-" + tpl.TemplateField + "'>" + tpl.Label + "</label><br>";
            }

            if (tpl.FieldType != 1)
                input += "<input id='" + tpl.SectionName + "-" + tpl.TemplateField + "' name='" + tpl.SectionName + "-" + tpl.TemplateField + "' type='text' value='' />";
            else
            {
                input += "<select id='" + tpl.SectionName + "-" + tpl.TemplateField + "' name='" + tpl.SectionName + "-" + tpl.TemplateField + "'>";
                if (!string.IsNullOrEmpty(tpl.OptionValues))
                {
                    string[] options = tpl.OptionValues.Split(new char[1] { '\n' });
                    foreach (string o in options)
                    {
                        input += "<option>" + o + "</option>";
                    }
                }
                input += "</select>";
            }

            return input;
        }

        /// <summary>
        /// Generates the HTML.
        /// </summary>
        /// <param name="campaignid">The campaignid.</param>
        /// <returns>System.String.</returns>
        public string GenerateHtml(long campaignid)
        {
            string html = "<form>";

            List<CampaignField> fields = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(campaignid);

            Dictionary<int, List<CampaignField>> pages = new Dictionary<int, List<CampaignField>>();

            foreach (CampaignField field in fields)
            {
                List<CampaignField> inputs = null;

                if (!pages.ContainsKey((field.PageNumber.HasValue ? field.PageNumber.Value : 0)))
                {
                    inputs = new List<CampaignField>();
                    pages.Add((field.PageNumber.HasValue ? field.PageNumber.Value : 0), inputs);
                }
                else
                {
                    inputs = pages[(field.PageNumber.HasValue ? field.PageNumber.Value : 0)];
                }

                if ((field.IsFormField.HasValue ? field.IsFormField.Value : false))
                    inputs.Add(field);
            }

            foreach (int page in pages.Keys)
            {
                if (page > 0 && pages[page].Count == 0) continue;

                if (page > 0)
                {
                    html += "<div id='page-" + page.ToString() + "'>";
                    html += "<h3>Page " + page.ToString() + "</h3>";
                }

                foreach (CampaignField t in pages[page])
                {
                    string input = GetInput(t);

                    if (input.Length > 0)
                        html += input + "<br><br>";
                }

                if (page > 0)
                    html += "</div>";
            }

            html += "<input type='submit' value='Submit'>";

            html += "</form>";

            return html;
        }

        /// <summary>
        /// Generates the HTML form.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateHtmlForm()
        {
            long campaignId = 0;

            long.TryParse(Request["campaignid"], out campaignId);

            return Content(GenerateHtml(campaignId));
        }

        public ActionResult GetCampaignTemplateFilterSettings(long id)
        {
            CampaignField campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(id);

            int filterValueType = 0;
            string filterValueContent = "";

            try
            {
                JObject jobject = JObject.Parse(campaignTemplate.FieldFilterSettings);
                filterValueType = jobject.Value<int>("filterType");
                filterValueContent = jobject.Value<string>("filterTypeValue");
            }
            catch
            {

            }

            return Json(new { filterType = filterValueType, filterTypeValue = filterValueContent }, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}