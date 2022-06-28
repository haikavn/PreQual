// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateChannelController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using AdRack.Buffering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Handles affiliate channel actions
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class AffiliateChannelController : BaseContentManagementController
    {
        /// <summary>
        /// Class LocalCampaignTemplate.
        /// </summary>
        public class LocalCampaignTemplate
        {
            /// <summary>
            /// The template field
            /// </summary>
            public string TemplateField = "";

            /// <summary>
            /// The description
            /// </summary>
            public string Description = "";

            /// <summary>
            /// The format
            /// </summary>
            public string Format = "";

            /// <summary>
            /// The required
            /// </summary>
            public bool Required = false;
        }

        /// <summary>
        /// Class RequestDetails.
        /// </summary>
        public class RequestDetails
        {
        }

        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The affiliate response service
        /// </summary>
        private readonly IAffiliateResponseService _affiliateResponseService;

        /// <summary>
        /// The affiliate channel template service
        /// </summary>
        private readonly IAffiliateChannelTemplateService _affiliateChannelTemplateService;

        /// <summary>
        /// The affiliate channel filter condition service
        /// </summary>
        private readonly IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The filter service
        /// </summary>
        private readonly IFilterService _filterService;

        /// <summary>
        /// The black list service
        /// </summary>
        private readonly IBlackListService _blackListService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="filterService">The filter service.</param>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="affiliateChannelFilterConditionService">The affiliate channel filter condition service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="affiliateResponseService">The affiliate response service.</param>
        /// <param name="affiliateChannelTemplateService">The affiliate channel template service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        public AffiliateChannelController(ICampaignService campaignService, IAffiliateService affiliateService, IFilterService filterService, IBlackListService blackListService, IAffiliateChannelFilterConditionService affiliateChannelFilterConditionService, ICampaignTemplateService campaignTemplateService, IAffiliateChannelService affiliateChannelService, IAffiliateResponseService affiliateResponseService, IAffiliateChannelTemplateService affiliateChannelTemplateService, ILocalizedStringService localizedStringService, IHistoryService historyService, ISettingService settingService, IAppContext appContext, IBuyerChannelService buyerChannelService, IPermissionService permissionService)
        {
            this._campaignService = campaignService;
            this._affiliateService = affiliateService;
            this._campaignTemplateService = campaignTemplateService;
            this._affiliateChannelService = affiliateChannelService;
            this._affiliateResponseService = affiliateResponseService;
            this._filterService = filterService;
            this._blackListService = blackListService;
            this._affiliateChannelTemplateService = affiliateChannelTemplateService;
            this._affiliateChannelFilterConditionService = affiliateChannelFilterConditionService;
            this._historyService = historyService;
            this._settingService = settingService;
            this._appContext = appContext;
            this._buyerChannelService = buyerChannelService;
            this._permissionService = permissionService;
        }

        #endregion Constructor


        #region Methods

        /// <summary>
        /// Show the affiliate channel list interface
        /// </summary>
        /// <returns>View</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Affiliate Channels List")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Building TreeItem tree from xml
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        protected void LoadItems(XmlNode parent, List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem> children)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
                ti.title = node.Name;
                LoadItems(node, ti.children);
                if (ti.children.Count > 0)
                {
                    ti.folder = true;
                    ti.expanded = true;
                    ti.TemplateField = "";
                    ti.DefaultValue = "";
                }
                else
                {
                    ti.folder = false;
                    ti.expanded = false;
                    ti.TemplateField = "";
                    ti.DefaultValue = "";
                }
                children.Add(ti);
            }
        }

        /// <summary>
        /// Loads affiliate channel template as json
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadAffiliateChannelTemplate()
        {
            long affiliateChannelId = 0;
            long.TryParse(Request["achannelid"], out affiliateChannelId);

            AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

            List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem parent = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
            parent.title = "root";
            parent.folder = true;
            parent.expanded = true;
            parent.TemplateField = "";
            parent.DefaultValue = "";

            if (affiliateChannel != null)
            {
                List<AffiliateChannelTemplate> templates = (List<AffiliateChannelTemplate>)this._affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannelId);

                var list = templates.Where(x => x.SectionName == "root").ToList();

                foreach (AffiliateChannelTemplate ct in list)
                {
                    Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
                    ti.title = ct.TemplateField;
                    ti.folder = true;
                    ti.expanded = true;

                    ti.CampaignTemplateId = ct.CampaignTemplateId;
                    ti.DefaultValue = string.IsNullOrEmpty(ct.DefaultValue) ? "" : ct.DefaultValue;

                    CampaignField campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(ct.CampaignTemplateId);
                    if (campaignTemplate != null && campaignTemplate.Validator == 13 && string.IsNullOrEmpty(ti.DefaultValue))
                    {
                        ti.DefaultValue = affiliateChannel.ChannelKey;
                    }

                    LoadChildren(affiliateChannel, ct.TemplateField, ti.children);
                    //fix from Arman:buyer channel folder condition
                    if (ti.children == null || ti.children.Count == 0)
                        ti.folder = false;

                    parent.children.Add(ti);
                }
            }

            items.Add(parent);

            return Json(items[0], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load items from XML
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadFromXml()
        {
            string data = Request.Unvalidated["xml"];

            if (StructuredDataBuffering.IsValidJson(data, false))
            {
                data = StructuredDataBuffering.JsonToXmlString(data);
            }
            else
            {
                if (!StructuredDataBuffering.IsMinimallyValidXml(data, true))
                {
                    NameValueCollection qscoll = HttpUtility.ParseQueryString(data);
                    data = "<?xml version = \"1.0\" encoding = \"UTF-8\" ?>";
                    data += "<tplroot>";
                    foreach (string key in qscoll)
                    {
                        data += "<" + key + "></" + key + ">";
                    }
                    data += "</tplroot>";
                }
            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(data);
            Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
            ti.title = xmldoc.DocumentElement.Name;
            ti.folder = true;
            ti.expanded = true;
            ti.TemplateField = "";
            ti.DefaultValue = "";

            LoadItems(xmldoc.DocumentElement, ti.children);
            //fix from Arman:buyer channel folder condition
            if (ti.children == null || ti.children.Count == 0)
                ti.folder = false;
            List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem>();
            items.Add(ti);

            return Json(new { items = items, xml = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads campaign XML
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadFromCampaignXml()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem>();

            long campaignid;
            Campaign campaign = null;

            if (long.TryParse(Request["campaignid"], out campaignid))
            {
                campaign = _campaignService.GetCampaignById(campaignid);

                if (campaign != null)
                {
                    if (string.IsNullOrEmpty(campaign.DataTemplate))
                        return Json(new { items = items, xml = campaign.DataTemplate }, JsonRequestBehavior.AllowGet);

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(campaign.DataTemplate);
                    Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
                    ti.title = xmldoc.DocumentElement.Name;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.TemplateField = "";

                    LoadItems(xmldoc.DocumentElement, ti.children);
                    //fix from Arman:buyer channel folder condition
                    if (ti.children == null || ti.children.Count == 0)
                        ti.folder = false;
                    items.Add(ti);
                }
            }

            return Json(new { items = items, xml = campaign != null ? campaign.DataTemplate : "", targetRevenue = campaign != null ? campaign.NetworkTargetRevenue : 0, minimumRevenue = campaign != null ? campaign.NetworkMinimumRevenue : 0, campaignType = campaign.CampaignType }, JsonRequestBehavior.AllowGet);
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
        /// Prepares an AffiliateChannelModel class
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="affiliateId">The affiliate identifier.</param>
        protected void PrepareModel(AffiliateChannelModel model, long affiliateId = 0)
        {
            model.ListAffiliatePriceMethod.Add(new SelectListItem { Text = "Fixed", Value = "0" });
            model.ListAffiliatePriceMethod.Add(new SelectListItem { Text = "Revshare", Value = "1" });
            model.ListAffiliatePriceMethod.Add(new SelectListItem { Text = "Inherit from Affiliate", Value = "2" });

            model.ListMinPriceOption.Add(new SelectListItem { Text = "Default", Value = "0" });
            model.ListMinPriceOption.Add(new SelectListItem { Text = "Fixed price", Value = "1" });
            model.ListMinPriceOption.Add(new SelectListItem { Text = "Rev share", Value = "2" });
            model.ListMinPriceOption.Add(new SelectListItem { Text = "Fixed commission", Value = "3" });
            model.ListMinPriceOption.Add(new SelectListItem { Text = "Max price", Value = "4" });

            if (_appContext.AppUser != null && (_appContext.AppUser.UserType == UserTypes.Super || _appContext.AppUser.UserType == UserTypes.Network || model.Status != 3))
            {
                model.ListStatus.Add(new SelectListItem { Text = "Inactive", Value = "0" });
                model.ListStatus.Add(new SelectListItem { Text = "Active", Value = "1" });
                model.ListStatus.Add(new SelectListItem { Text = "Test", Value = "2" });
            }
            model.ListStatus.Add(new SelectListItem { Text = "Suspended", Value = "3" });

            model.ListDataFormat.Add(new SelectListItem { Text = "XML", Value = "0" });
            model.ListDataFormat.Add(new SelectListItem { Text = "JSON", Value = "1" });

            List<Campaign> list = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            model.ListCampaign.Add(new SelectListItem() { Text = "Select campaign", Value = "" });

            foreach (Campaign item in list)
            {
                model.ListCampaign.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            List<Affiliate> alist = (List<Affiliate>)_affiliateService.GetAllAffiliates(0);

            foreach (Affiliate item in alist)
            {
                if (this._appContext.AppUser != null && this._appContext.AppUser.UserType == UserTypes.Affiliate && item.Id != this._appContext.AppUser.ParentId) continue;
                if (affiliateId != 0 && affiliateId != item.Id) continue;

                model.ListAffiliate.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }


            PrepareFilterConditions(model);

            model.CustomBlackLists = (List<Core.Domain.Lead.CustomBlackListValue>)_blackListService.GetCustomBlackListValues(model.AffiliateChannelId, 1);

            if (model.AffiliateChannelId > 0 || model.ListCampaign.Count == 0)
            {
                model.CampaignTemplate = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(model.CampaignId, isfilterable: 1);
                model.Filters = (List<Core.Domain.Lead.Filter>)_filterService.GetFiltersByCampaignId(model.CampaignId);
            }
            else
            {
                model.CampaignTemplate = new List<CampaignField>();
                model.Filters = new List<Core.Domain.Lead.Filter>();
            }

            model.CampaignTemplate = model.CampaignTemplate.OrderBy(x => x.TemplateField).ToList();

            List<CampaignField> campaignTemplateList = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(model.CampaignId);

            List<LocalCampaignTemplate> localCampaignTemplateList = new List<LocalCampaignTemplate>();

            foreach (CampaignField ctpl in campaignTemplateList)
            {
                if (ctpl.TemplateField.ToLower() == "request" ||
                    ctpl.TemplateField.ToLower() == "refferal" ||
                    ctpl.TemplateField.ToLower() == "personal" ||
                    ctpl.TemplateField.ToLower() == "bank" ||
                    ctpl.TemplateField.ToLower() == "employment" ||
                    ctpl.TemplateField.ToLower() == "employmentinfo")
                {
                    continue;
                }

                LocalCampaignTemplate lct = new LocalCampaignTemplate();
                lct.Description = ctpl.Description;
                lct.Format = "";
                lct.Required = ctpl.Required;
                lct.TemplateField = ctpl.TemplateField;

                LeadContent lc = new LeadContent();
                PropertyInfo pi = lc.GetType().GetProperty(ctpl.DatabaseField);

                /*if (pi != null)
                {
                    switch (pi.Name.ToLower())
                    {
                        case "ip": lct.Format = "IPv4 Address#[0..9] and [.] allowed"; break;
                        case "minprice": lct.Format = "Decimal#[0..9] and [.] allowed"; break;
                        case "firstname": lct.Format = "String#Alphabet characters"; break;
                        case "lastname": lct.Format = "String#Alphabet characters"; break;
                        case "address": lct.Format = "String#Alphabet or numeric characters"; break;
                        case "city": lct.Format = "String(2)#US city"; break;
                        case "state": lct.Format = "String(2)#US State Abbreviation"; break;
                        case "zip": lct.Format = "Numeric(5)#[0..9]"; break;
                        case "dob": lct.Format = "Date of Birth#[mm-dd-yyyy]"; break;
                        case "age": lct.Format = "Numeric#[0..9]"; break;
                        case "requestedamount": lct.Format = "Numeric#[0..9]"; break;
                        case "ssn": lct.Format = "Numeric#Valid SSN Number, [0..9]"; break;
                        case "homephone": lct.Format = "Numeric(10)#US Phone Number"; break;
                        case "cellphone": lct.Format = "Numeric(10)#US Phone Number"; break;
                        case "email": lct.Format = "E-mail#Valid e-mail address"; break;
                        case "payfrequency": lct.Format = "String#Weekly/Every 2 Weeks/Twice A Month / Monthly"; break;
                        case "directdeposit": lct.Format = "String#Yes/No"; break;
                        case "accounttype": lct.Format = "String#Checking Account/Savings Accoun"; break;
                        case "incometype": lct.Format = "String#Job Income/Benefits/Self Employed/Retirement Income/Disability Income"; break;
                        case "netmonthlyincome": lct.Format = "Numeric#[0..9]"; break;
                        case "emptime": lct.Format = "Numeric#Primary source of income"; break;
                        case "addressmonth": lct.Format = "Numeric#Between 0 and 100 Inclusive"; break;
                        case "affiliatesubid": lct.Format = "String#Alphabet characters"; break;
                        case "affiliatesubid2": lct.Format = "String#Alphabet characters"; break;
                    }
                }
                else
                {
                    switch (ctpl.Validator)
                    {
                        case 1: lct.Format = "String#Alphabet characters"; break;
                        case 2: lct.Format = "Number#[0 - 9]"; break;
                        case 3: lct.Format = "String#Valid E-mail address"; break;
                        case 5: lct.Format = "Number[4-30]#Valid account number"; break;
                        case 6: lct.Format = "Number#Valid SSN number"; break;
                        case 7: lct.Format = "Numeric(5)#Valid ZIP code"; break;
                        case 8: lct.Format = "Numeric(10)#US Phone Number"; break;
                        case 9: lct.Format = "String#Digits and numbers"; break;
                        case 10: lct.Format = "Date#[mm-dd-yyyy]"; break;
                        case 11: lct.Format = "String(2)#US State Abbreviation"; break;
                        case 12: lct.Format = "Number[9]#Valid ABA/Routing Number"; break;
                        case 13: lct.Format = "String#Alphabet and numeric characters"; break;
                        default: lct.Format = "None#Alphabet characters and numbers"; break;
                    }
                }*/

                switch (ctpl.Validator)
                {
                    case 1: lct.Format = "String#"; break;
                    case 2: lct.Format = "Number#"; break;
                    case 3: lct.Format = "String#"; break;
                    case 5: lct.Format = "Number[4-30]#"; break;
                    case 6: lct.Format = "Number#"; break;
                    case 7: lct.Format = "Numeric(5)#"; break;
                    case 8: lct.Format = "Numeric(10)#"; break;
                    case 9: lct.Format = "String#"; break;
                    case 10: lct.Format = "Date#"; break;
                    case 11: lct.Format = "String(2)#"; break;
                    case 12: lct.Format = "Number[9]#"; break;
                    case 13: lct.Format = "String#"; break;
                    default: lct.Format = "None#"; break;
                }

                lct.Format += ctpl.PossibleValue;

                localCampaignTemplateList.Add(lct);
            }

            ViewBag.CampaignTemplateList = localCampaignTemplateList;

            CampaignField campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(model.CampaignId, 13);
            string AffiliateXmlField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

            campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(model.CampaignId, 9);
            string passwordField = campaignTemplate != null ? campaignTemplate.TemplateField : "";
            long passwordFieldId = campaignTemplate != null ? campaignTemplate.Id : -1;

            IList<AffiliateChannelTemplate> achtList = this._affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(model.AffiliateChannelId);
            foreach (AffiliateChannelTemplate acht in achtList)
            {
                if (acht.TemplateField.ToLower() == AffiliateXmlField.ToLower())
                {
                    ViewBag.CHANNELID = acht.DefaultValue;
                }
                if (acht.TemplateField.ToLower() == passwordField.ToLower() || acht.CampaignTemplateId == passwordFieldId)
                {
                    ViewBag.PASSWORD = acht.DefaultValue;
                }

                Setting postingUrlSetting = this._settingService.GetSetting("System.PostingUrl");
                ViewBag.postingurl = (postingUrlSetting != null && !string.IsNullOrEmpty(postingUrlSetting.Value) ? (!postingUrlSetting.Value.EndsWith("/") ? postingUrlSetting.Value + "/" : postingUrlSetting.Value) : Helper.GetBaseUrl(Request));
            }

            if (model.CampaignId != 0)
            {
                ViewBag.Campaign = this._campaignService.GetCampaignById(model.CampaignId);

                if (ViewBag.Campaign != null)
                {
                    if (ViewBag.Campaign.XmlTemplate != null)
                    {
                        System.Xml.Linq.XDocument doc = GetSampleXml(model.CampaignId, ViewBag.Campaign.XmlTemplate);//System.Xml.Linq.XDocument.Parse(ViewBag.Campaign.XmlTemplate);
                        if (doc != null)
                            ViewBag.XmlTemplate = doc.ToString();
                    }

                    // HashCode Calculation
                    string EncodedID = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0:0000000}", model.AffiliateChannelId)));

                    Random rnd = new Random();
                    int randNum = 0;

                    char[] arr = new char[10];
                    for (int i = 0; i < 10; i++)
                    {
                        randNum = rnd.Next(65, 90);
                        arr[i] = Convert.ToChar(randNum);
                    }

                    string randNumStr = new string(arr);

                    ViewBag.HashCode = Helper.GetBaseUrl(Request) + "Management/AffiliateChannel/PostSpecification/?p=" + HttpUtility.UrlEncode(randNumStr + EncodedID);
                }
                else
                {
                    ViewBag.HashCode = "";
                    ViewBag.XmlTemplate = "";
                }
            }

            if (_appContext.AppUser != null)
            {
                ViewBag.UserTypeId = 0;
            }
            else
            {
                ViewBag.UserTypeId = 0;
            }
        }

        protected void PrepareFilterConditions(AffiliateChannelModel model, long parentId = 0)
        {
            if (model.FilterConditions == null)
                model.FilterConditions = new List<AffiliateChannelFilterCondition>();

            var conditions = (List<Core.Domain.Lead.AffiliateChannelFilterCondition>)_affiliateChannelFilterConditionService.GetFilterConditionsByAffiliateChannelId(model.AffiliateChannelId, parentId);

            foreach (AffiliateChannelFilterCondition item in conditions)
            {
                model.FilterConditions.Add(item);
                PrepareFilterConditions(model, item.Id);
            }
        }

        /// <summary>
        /// Loads the children.
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        protected void LoadChildren(AffiliateChannel affiliateChannel, string parent, List<Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem> children)
        {
            var list = _affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannel.Id).Where(x => x.SectionName == parent).ToList();

            foreach (AffiliateChannelTemplate node in list)
            {
                Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.AffiliateChannelModel.TreeItem();
                ti.title = node.TemplateField;
                LoadChildren(affiliateChannel, node.TemplateField, ti.children);
                if (ti.children.Count > 0)
                {
                    ti.folder = true;
                    ti.expanded = true;
                    ti.TemplateField = "";
                    ti.CampaignTemplateId = 0;
                    ti.DefaultValue = "";
                }
                else
                {
                    ti.folder = false;
                    ti.expanded = false;
                    ti.TemplateField = node.TemplateField;
                    ti.CampaignTemplateId = node.CampaignTemplateId;
                    ti.DefaultValue = string.IsNullOrEmpty(node.DefaultValue) ? "" : node.DefaultValue;
                }

                CampaignField campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(node.CampaignTemplateId);
                if (campaignTemplate != null && campaignTemplate.Validator == 13 && string.IsNullOrEmpty(ti.DefaultValue))
                {
                    ti.DefaultValue = affiliateChannel.ChannelKey;
                }

                children.Add(ti);
            }
        }

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Affiliate Channel")]
        public ActionResult Create(long id = 0)
        {
            AffiliateChannelModel am = new AffiliateChannelModel();

            long affiliateid = 0;
            if (long.TryParse(Request["affiliateid"], out affiliateid))
            {
                am.AffiliateId = affiliateid;
            }

            long campaignid = 0;

            if (long.TryParse(Request["campaignid"], out campaignid))
            {
                am.CampaignId = campaignid;
            }

            am.MinPriceOptionValue = 20;
            am.MinRevenue = 1;

            PrepareModel(am, affiliateid);

            return View(am);
        }

        /// <summary>
        /// Loads an Affiliate channel create/edit interface
        /// </summary>
        /// <param name="id">Affiliate channel id</param>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Affiliate Channel")]
        public ActionResult Item(long id = 0)
        {
            if (_appContext.AppUser.UserType == UserTypes.Buyer)
            {
                return HttpNotFound();
            }

            AffiliateChannel affiliateChannels = this._affiliateChannelService.GetAffiliateChannelById(id);

            AffiliateChannelModel am = new AffiliateChannelModel();

            am.BaseUrl = Helper.GetBaseUrl(Request);

            Setting s = _settingService.GetSetting("System.PostingUrl");

            if (s != null) am.BaseUrl = s.Value;

            am.SampleResponse = "Sold Response: \r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0001</id><status>sold</status><message></message><price>0.5</price><redirect><![CDATA[]]></redirect></response>") + "\r\n\r\nReject Response\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>reject</status><message>lead was not sold in marketplace</message><price></price><redirect></redirect></response>") + "\r\n\r\nError Response:\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>error</status><message>error message here</message><price></price><redirect></redirect></response>") + "\r\n\r\nTest Response:\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>test</status><message>message on lead quality</message><price>1</price><redirect><![CDATA[https://forms.storefrontloans.com/lead/sign/?id=0]]></redirect></response>").ToString();

            am.AffiliateChannelId = 0;

            if (affiliateChannels != null)
            {
                am.AffiliateChannelId = affiliateChannels.Id;
                am.Name = affiliateChannels.Name;
                am.XmlTemplate = (!string.IsNullOrEmpty(affiliateChannels.XmlTemplate) ? affiliateChannels.XmlTemplate.Replace(Environment.NewLine, "") : "");
                am.Status = affiliateChannels.Status;
                am.CampaignId = affiliateChannels.CampaignId??0;
                am.AffiliateId = affiliateChannels.AffiliateId;
                am.MinPriceOption = affiliateChannels.MinPriceOption;
                am.MinPriceOptionValue = (int)affiliateChannels.NetworkTargetRevenue;
                am.MinRevenue = Math.Round(affiliateChannels.NetworkMinimumRevenue, 2);
                am.AffiliateChannelKey = affiliateChannels.ChannelKey;
                am.DataFormat = affiliateChannels.DataFormat;
                am.AffiliatePrice = Math.Round((affiliateChannels.AffiliatePrice.HasValue ? affiliateChannels.AffiliatePrice.Value : 0), 2); 
                am.AffiliatePriceMethod = (affiliateChannels.AffiliatePriceMethod.HasValue ? affiliateChannels.AffiliatePriceMethod.Value : (short)0);
                am.Timeout = (affiliateChannels.Timeout.HasValue ? affiliateChannels.Timeout.Value : (short)0);

                am.Note = affiliateChannels.Note;

                Campaign camp = null;
                if (camp != null)
                {
                    this._campaignService.GetCampaignById(affiliateChannels.CampaignId.Value);
                }
                    
                if (camp != null)
                {
                    ViewBag.CampaignName = "Campaign: " + camp.Name + " / " + affiliateChannels.CampaignId.ToString();
                    am.CampaignType = camp.CampaignType;
                }

                Affiliate affiliate = this._affiliateService.GetAffiliateById(affiliateChannels.AffiliateId, false);
                if (affiliate != null)
                {
                    ViewBag.AffiliateName = "Affiliate: " + affiliate.Name + " / " + affiliateChannels.AffiliateId.ToString();
                }

                //List<CampaignTemplate> list = (List<CampaignTemplate>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(affiliateChannels.CampaignId);

                //am.ListCampaignField.Add(new SelectListItem() { Text = "NONE", Value = "0" });

                //foreach (CampaignTemplate item in list)
                //{
                //    am.ListCampaignField.Add(new SelectListItem() { Text = item.TemplateField, Value = item.Id.ToString() });
                //}
            }

            PrepareModel(am);

            if (ViewBag.CHANNELID == null || (ViewBag.CHANNELID != null && ((string)ViewBag.CHANNELID).Length == 0))
            {
                ViewBag.CHANNELID = affiliateChannels != null ? affiliateChannels.ChannelKey : "";
            }

            return View(am);
        }

        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        /// <param name="campaignid">The campaignid.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>XDocument.</returns>
        [NonAction]
        public XDocument GetSampleXml(long campaignid, string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                Campaign campaign = this._campaignService.GetCampaignById(campaignid);
                if (campaign != null)
                {
                    xml = campaign.DataTemplate;
                }
            }

            XDocument xmldoc = XDocument.Parse(xml);

            List<CampaignField> list = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(campaignid);

            foreach (CampaignField tpl in list)
            {
                List<XElement> elements = (List<XElement>)xmldoc.Descendants(tpl.TemplateField).ToList();

                foreach (XElement e in elements)
                {
                    if (e.Parent != null && e.Parent.Name == tpl.SectionName && e.Descendants().Count() == 0)
                    {
                        e.Value = tpl.PossibleValue;
                    }
                }
            }

            return xmldoc;
        }

        /// <summary>
        /// Handling affiliate channel item submit action
        /// </summary>
        /// <param name="affiliateChannelModel">Model returned from the view</param>
        /// <param name="returnUrl">Redirect url after finish</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Item(AffiliateChannelModel affiliateChannelModel, string returnUrl)
        {
            string data1 = "", data2 = "";

            if (_appContext.AppUser.UserType == UserTypes.Buyer)
            {
                return HttpNotFound();
            }

            AffiliateChannel affiliateChannel = null;

            if (_affiliateChannelService.GetAffiliateChannelByName(affiliateChannelModel.Name, affiliateChannelModel.AffiliateChannelId) != null)
            {
                return Json(new { error = "Affiliate channel name already exists" }, JsonRequestBehavior.AllowGet);
            }

            bool canClone = false;
            if (!bool.TryParse(Request["canclone"], out canClone))
                canClone = false;

            if (canClone)
                affiliateChannelModel.AffiliateChannelId = 0;

            string json = Request.Unvalidated["json"];
            string xml = Request.Unvalidated["xml"];

            if (affiliateChannelModel.AffiliateChannelId == 0)
            {
                affiliateChannel = new AffiliateChannel();
                affiliateChannel.ChannelKey = Helper.GetUniqueKey(7);
            }
            else
            {
                affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelModel.AffiliateChannelId);

                data1 = "Name: " + affiliateChannel.Name + ";" +
                               "Status: " + affiliateChannel.Status.ToString() + ";" +
                               "CampaignId: " + affiliateChannel.CampaignId.ToString() + ";" +
                               "AffiliateId: " + affiliateChannel.AffiliateId.ToString() + ";" +
                               "DataFormat: " + affiliateChannel.DataFormat.ToString() + ";" +
                               "MinPriceOptionValue: " + ((int)affiliateChannel.NetworkTargetRevenue).ToString() + ";" +
                               "MinRevenue: " + Math.Round(affiliateChannel.NetworkMinimumRevenue, 2).ToString() + ";";
            }

            affiliateChannel.Name = affiliateChannelModel.Name.Trim();
            affiliateChannel.Status = affiliateChannelModel.Status;
            affiliateChannel.CampaignId = affiliateChannelModel.CampaignId;
            affiliateChannel.AffiliateId = affiliateChannelModel.AffiliateId;
            affiliateChannel.MinPriceOption = 0;
            affiliateChannel.DataFormat = affiliateChannelModel.DataFormat;
            affiliateChannel.NetworkTargetRevenue = (decimal)affiliateChannelModel.MinPriceOptionValue;
            affiliateChannel.NetworkMinimumRevenue = affiliateChannelModel.MinRevenue;
            affiliateChannel.AffiliatePriceMethod = affiliateChannelModel.AffiliatePriceMethod;
            affiliateChannel.AffiliatePrice = affiliateChannelModel.AffiliatePrice;
            affiliateChannel.Timeout = affiliateChannelModel.Timeout;
            affiliateChannel.Note = affiliateChannelModel.Note;

            if (_appContext.AppUser != null && affiliateChannelModel.CampaignType == 0)
            {
                if (_appContext.AppUser.UserType == UserTypes.Super || _appContext.AppUser.UserType == UserTypes.Network)
                {
                    affiliateChannel.XmlTemplate = affiliateChannelModel.XmlTemplate;
                }
                else
                if (_appContext.AppUser.UserType == UserTypes.Affiliate)
                {
                    affiliateChannel.AffiliateId = _appContext.AppUser.ParentId;
                }
            }

            bool isnew = false;

            if (affiliateChannelModel.AffiliateChannelId == 0)
            {
                isnew = true;
                long newId = _affiliateChannelService.InsertAffiliateChannel(affiliateChannel);
                this._historyService.AddHistory("AffiliateChannelController", HistoryAction.Affiliate_Channel_Added, "AffiliateChannel", newId, "Name:" + affiliateChannel.Name, "", "", this._appContext.AppUser.Id);
            }
            else
            {
                _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);

                data2 = "Name: " + affiliateChannel.Name + ";" +
                               "Status: " + affiliateChannel.Status.ToString() + ";" +
                               "CampaignId: " + affiliateChannel.CampaignId.ToString() + ";" +
                               "AffiliateId: " + affiliateChannel.AffiliateId.ToString() + ";" +
                               "DataFormat: " + affiliateChannel.DataFormat.ToString() + ";" +
                               "MinPriceOptionValue: " + ((int)affiliateChannel.NetworkTargetRevenue).ToString() + ";" +
                               "MinRevenue: " + Math.Round(affiliateChannel.NetworkMinimumRevenue, 2).ToString();

                this._historyService.AddHistory("AffiliateChannelController", HistoryAction.Affiliate_Channel_Edited, "AffiliateChannel", affiliateChannelModel.AffiliateChannelId, data1, data2, "", this._appContext.AppUser.Id);
            }

            affiliateChannelModel.AffiliateChannelId = affiliateChannel.Id;

            PrepareModel(affiliateChannelModel);

            XDocument xdoc = GetSampleXml(affiliateChannelModel.CampaignId, xml);
            XmlDocument xmldoc = new XmlDocument();

            try
            {
                if (xml!="")
                    xmldoc.LoadXml(xml);
            }
            catch
            {
            }

            Campaign campaign = null;
            if (campaign != null)
            {
                campaign = _campaignService.GetCampaignById(affiliateChannel.CampaignId.Value);
            }
                
            Setting set = this._settingService.GetSetting("System.AffiliateXmlField");

            string xmlfield = "CHANNELID";
            if (set != null) xmlfield = set.Value;

            string conditions = Request.Unvalidated["conditions"];
            dynamic dConditions = JsonConvert.DeserializeObject(conditions);
            long parentId = 0;

            _affiliateChannelFilterConditionService.DeleteFilterConditions(affiliateChannelModel.AffiliateChannelId);

            for (int i = 0; i < dConditions.Count; i++)
            {
                string field = dConditions[i]["field"].ToString();
                string condition = dConditions[i]["condition"].ToString();
                string value = dConditions[i]["value"].ToString();
                string op = dConditions[i]["operator"].ToString();
                string parent = dConditions[i]["parent"].ToString();

                AffiliateChannelFilterCondition fc = new AffiliateChannelFilterCondition();

                fc.AffiliateChannelId = affiliateChannelModel.AffiliateChannelId;
                fc.Condition = short.Parse(condition);
                fc.Value = value.Trim();
                fc.ConditionOperator = short.Parse(op);
                fc.CampaignTemplateId = long.Parse(field);
                fc.ParentId = (parent != "0" ? parentId : 0);

                this._affiliateChannelFilterConditionService.InsertFilterCondition(fc);

                if (parent == "0")
                    parentId = fc.Id;
            }

            if (affiliateChannelModel.CampaignType == 0)
            {
                string blacklists = Request.Unvalidated["blacklists"];
                dynamic dBlackLists = JsonConvert.DeserializeObject(blacklists);

                _blackListService.DeleteCustomBlackListValues(affiliateChannelModel.AffiliateChannelId, 1);

                for (int i = 0; i < dBlackLists.Count; i++)
                {
                    string field = dBlackLists[i]["field"].ToString();
                    string value = dBlackLists[i]["value"].ToString();

                    CustomBlackListValue fc = new CustomBlackListValue();

                    fc.ChannelId = affiliateChannelModel.AffiliateChannelId;
                   // fc.TemplateFieldId = field;
                    fc.ChannelType = 1;
                    fc.Value = value;

                    this._blackListService.InsertCustomBlackListValue(fc);
                }
            }

            _affiliateChannelTemplateService.DeleteAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannel.Id);

            if (campaign.CampaignType == 0)
            {
                CampaignField campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(campaign.Id, 13);

                string AffiliateXmlField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

                campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(campaign.Id, 9);
                string passwordField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

                long passwordFieldId = campaignTemplate != null ? campaignTemplate.Id : -1;

                dynamic dFields = JsonConvert.DeserializeObject(json);

                for (int i = 0; i < dFields.Count; i++)
                {
                    if (dFields[i].Count == 0) continue;
                    string tfield = dFields[i][0].ToString();
                    string sfield = dFields[i][1].ToString();
                    string defaultvalue = dFields[i][2].ToString();
                    string parent = dFields[i][3].ToString();
                    //string blacklist = o[i][6].ToString();

                    AffiliateChannelTemplate ct = new AffiliateChannelTemplate();
                    ct.AffiliateChannelId = affiliateChannel.Id;
                    ct.CampaignTemplateId = long.Parse(sfield);
                    ct.TemplateField = tfield;
                    ct.DefaultValue = defaultvalue;
                    ct.SectionName = parent;

                    List<XElement> xel = (List<XElement>)xdoc.Descendants(tfield).ToList();

                    if ((tfield.ToLower() == xmlfield.ToLower() || (campaign != null && AffiliateXmlField.ToLower() == tfield.ToLower())))
                    {
                        if (string.IsNullOrEmpty(ct.DefaultValue))
                        {
                            affiliateChannel.ChannelKey = Helper.GetUniqueKey(7);
                        }
                        else 
                        {
                            if (!canClone)
                            {
                                affiliateChannel.ChannelKey = ct.DefaultValue;
                            }
                            else
                            {
                                affiliateChannel.ChannelKey = Helper.GetUniqueKey(7);
                                ct.DefaultValue = affiliateChannel.ChannelKey;
                            }
                        }

                        if (xel.Count > 0)
                        {
                            xel[0].Value = ct.DefaultValue;
                        }
                    }
                    else
                    if (tfield.ToLower() == passwordField.ToLower() || passwordFieldId == ct.CampaignTemplateId)
                    {
                        if (string.IsNullOrEmpty(ct.DefaultValue))
                        {
                            ct.DefaultValue = Helper.GetUniqueKey(8);
                        }

                        if (xel.Count > 0)
                        {
                            xel[0].Value = ct.DefaultValue;
                        }
                    }

                    _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(ct);

                    if (parent == "root") continue;
                }
            }
            else
            {
                xmldoc.LoadXml(campaign.DataTemplate);
                List<CampaignField> campaignTemplates = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(campaign.Id);

                foreach (CampaignField t in campaignTemplates)
                {
                    AffiliateChannelTemplate ct = new AffiliateChannelTemplate();
                    ct.AffiliateChannelId = affiliateChannel.Id;
                    ct.CampaignTemplateId = (t.SectionName.ToLower() == "root" ? 0 : t.Id);
                    ct.TemplateField = t.TemplateField;
                    ct.DefaultValue = "";
                    ct.SectionName = t.SectionName;
                    _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(ct);
                }
            }

            affiliateChannel.XmlTemplate = xdoc.ToString();//xmldoc.OuterXml;

            _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);

            SharedData.ClearBuyerChannelLeadsCount();

            return Json(new { id = affiliateChannel.Id }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets affiliate channel xml
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAffiliateChannelXml()
        {
            string json = Request.Unvalidated["json"] != null ? Request.Unvalidated["json"] : "";
            string xml = Request.Unvalidated["xml"] != null ? Request.Unvalidated["xml"].Replace("&lt;", "<").Replace("&gt;", ">") : "";

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                if (xml!="")
                    xmldoc.LoadXml(xml);
            }
            catch
            {
            }

            try
            {
                dynamic o = JsonConvert.DeserializeObject(json);

                for (int i = 0; i < o.Count; i++)
                {
                    if (o[i].Count == 0) continue;
                    string tfield = o[i][0].ToString();
                    string sfield = o[i][1].ToString();
                    string defaultvalue = o[i][2].ToString();
                    string parent = o[i][3].ToString();
                    //string blacklist = o[i][6].ToString();

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

                return Json(new { xml = System.Xml.Linq.XDocument.Parse(xmldoc.OuterXml).ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch { }

            return Json(new { });
        }

        /// <summary>
        /// Get affiliate channels
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAffiliateChannels()
        {
            if (this._appContext.AppUser == null)
                return Json(0, JsonRequestBehavior.AllowGet);

            string c = Request["Params"];

            string aid = Request["aid"];

            short deleted = 0;

            short.TryParse(Request["d"], out deleted);
            
            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)this._affiliateChannelService.GetAllAffiliateChannels(deleted);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = affiliateChannels.Count;
            jd.recordsFiltered = affiliateChannels.Count;
            foreach (AffiliateChannel ai in affiliateChannels)
            {
                if (!string.IsNullOrEmpty(c) && ai.CampaignId.ToString() != c) continue;
                if (!string.IsNullOrEmpty(aid) && ai.AffiliateId.ToString() != aid) continue;

                if (_appContext.AppUser.UserType == UserTypes.Affiliate && _appContext.AppUser.ParentId != ai.AffiliateId)
                    continue;

                Campaign campaign = null;
                if (campaign != null)
                {
                  campaign = _campaignService.GetCampaignById(ai.CampaignId.Value);
                }

                Affiliate affiliate = this._affiliateService.GetAffiliateById(ai.AffiliateId, false);

                string status = "<span style='color: red'>Inactive</span>";

                switch (ai.Status)
                {
                    case 1: status = "<span style='color: green'>Active</span>"; break;
                    case 2: status = "<span style='color: orange'>Test</span>"; break;
                    case 3: status = "<span style='color: red'>Suspended</span>"; break;
                }

                string[] data = {
                                      ai.Id.ToString(),
                                      ai.ChannelKey,
                                      "<a href=\"/Management/AffiliateChannel/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      affiliate == null ? "" : affiliate.Name,
                                      campaign == null ? "" : campaign.Name,
                                      status,
                                      "<a href='#' onclick='deleteAffiliateChannel(" + ai.Id + ")'>" + (ai.IsDeleted.HasValue ? (ai.IsDeleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                                };
                jd.data.Add(data);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of responses sent to affiliate
        /// </summary>
        /// <returns>JSON result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAffiliateResponses()
        {
            string aid = Request["aid"];

            List<AffiliateResponse> affiliateResponses = (List<AffiliateResponse>)this._affiliateResponseService.GetAffiliateResponsesByAffiliateChannelId(long.Parse(aid));

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = affiliateResponses.Count;
            jd.recordsFiltered = affiliateResponses.Count;
            foreach (AffiliateResponse ai in affiliateResponses)
            {
                string[] names1 = {
                                      ai.LeadId != null ? ai.LeadId.ToString() : "0",
                                      ai.Created.ToShortDateString() + " " + ai.Created.ToShortTimeString(),
                                      HttpUtility.HtmlEncode(ai.Response)
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign temporary.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GetCampaignTemp()
        {
            List<AffiliateChannel> affiliateChannels = (List<AffiliateChannel>)this._affiliateChannelService.GetAllAffiliateChannels();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = affiliateChannels.Count;
            jd.recordsFiltered = affiliateChannels.Count;
            foreach (AffiliateChannel ai in affiliateChannels)
            {
                Campaign campaign = null;
                if (campaign !=null)
                {
                    campaign = _campaignService.GetCampaignById(ai.CampaignId.Value);
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"/Management/AffiliateChannel/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      campaign == null ? "" : campaign.Name,
                                      ai.Status != 0 ? "<span style='color: green'>Active</span>" : "<span style='color: red'>Inactive</span>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads affiliate channel posting specification partial view
        /// </summary>
        /// <returns>PartialView result</returns>
        [AllowAnonymous]
        [ContentManagementAuthorize(true)]
        public ActionResult PostSpecification()
        {
            if (Request["p"] == null)
            {
                return null;
            }

            string idStr = Request["p"];
            string sss = "";
            long id = 0;

            try
            {
                idStr = idStr.Substring(10);
                sss = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(HttpUtility.UrlDecode(idStr)));
                id = long.Parse(sss);
            }
            catch
            {
                return null;
            }

            if (id == 0)
            {
                return null;
            }

            AffiliateChannel affiliateChannels = this._affiliateChannelService.GetAffiliateChannelById(id);

            AffiliateChannelModel am = new AffiliateChannelModel();

            am.BaseUrl = Helper.GetBaseUrl(Request);

            Setting s = _settingService.GetSetting("System.PostingUrl");

            if (s != null) am.BaseUrl = s.Value;


            am.SampleResponse = "Sold Response: \r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0001</id><status>sold</status><message></message><price>0.5</price><redirect><![CDATA[]]></redirect></response>") + "\r\n\r\nReject Response\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>reject</status><message>lead was not sold in marketplace</message><price></price><redirect></redirect></response>") + "\r\n\r\nError Response:\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>error</status><message>error message here</message><price></price><redirect></redirect></response>") + "\r\n\r\nTest Response:\r\n\r\n" + System.Xml.Linq.XDocument.Parse("<?xml version = \"1.0\" encoding = \"utf-8\" ?><response><id>0</id><status>test</status><message>message on lead quality</message><price>1</price><redirect><![CDATA[https://forms.storefrontloans.com/lead/sign/?id=0]]></redirect></response>").ToString();

            am.AffiliateChannelId = 0;

            if (affiliateChannels != null)
            {
                am.AffiliateChannelId = affiliateChannels.Id;
                am.Name = affiliateChannels.Name;
                am.XmlTemplate = affiliateChannels.XmlTemplate;
                am.Status = affiliateChannels.Status;
                am.CampaignId = affiliateChannels.CampaignId??0;
                am.AffiliateId = affiliateChannels.AffiliateId;
                am.MinPriceOption = affiliateChannels.MinPriceOption;
                am.MinPriceOptionValue = (int)Math.Round(affiliateChannels.NetworkTargetRevenue, 2);
                am.MinRevenue = Math.Round(affiliateChannels.NetworkMinimumRevenue, 2);
                am.AffiliateChannelKey = affiliateChannels.ChannelKey;

                if (affiliateChannels.CampaignId != null)
                {
                    List<CampaignField> list = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(affiliateChannels.CampaignId.Value);

                    am.ListCampaignField.Add(new SelectListItem() { Text = "NONE", Value = "0" });

                    foreach (CampaignField item in list)
                    {
                        am.ListCampaignField.Add(new SelectListItem() { Text = item.TemplateField, Value = item.Id.ToString() });
                    }
                }
            }

            am.ListMinPriceOption.Add(new SelectListItem { Text = "Default", Value = "0" });
            am.ListMinPriceOption.Add(new SelectListItem { Text = "Fixed price", Value = "1" });
            am.ListMinPriceOption.Add(new SelectListItem { Text = "Rev share", Value = "2" });
            am.ListMinPriceOption.Add(new SelectListItem { Text = "Fixed commission", Value = "3" });
            am.ListMinPriceOption.Add(new SelectListItem { Text = "Max price", Value = "4" });

            PrepareModel(am);

            am.FilterConditions = (List<Core.Domain.Lead.AffiliateChannelFilterCondition>)_affiliateChannelFilterConditionService.GetFilterConditionsByAffiliateChannelId(am.AffiliateChannelId);

            if (am.AffiliateChannelId > 0 || am.ListCampaign.Count == 0)
            {
                am.CampaignTemplate = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(am.CampaignId, isfilterable: 1);
                am.Filters = (List<Core.Domain.Lead.Filter>)_filterService.GetFiltersByCampaignId(am.CampaignId);
            }
            else
            {
                am.CampaignTemplate = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(long.Parse(am.ListCampaign[0].Value), isfilterable: 1);
                am.Filters = (List<Core.Domain.Lead.Filter>)_filterService.GetFiltersByCampaignId(long.Parse(am.ListCampaign[0].Value));
            }

            CampaignField campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(am.CampaignId, 13);
            string AffiliateXmlField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

            campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(am.CampaignId, 9);
            string passwordField = campaignTemplate != null ? campaignTemplate.TemplateField : "";
            long passwordFieldId = campaignTemplate != null ? campaignTemplate.Id : -1;

            // Request Details

            List<CampaignField> campaignTemplateList = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(am.CampaignId);

            List<LocalCampaignTemplate> localCampaignTemplateList = new List<LocalCampaignTemplate>();

            foreach (CampaignField ctpl in campaignTemplateList)
            {
                if (ctpl.TemplateField.ToLower() == "request" ||
                    ctpl.TemplateField.ToLower() == "refferal" ||
                    ctpl.TemplateField.ToLower() == "personal" ||
                    ctpl.TemplateField.ToLower() == "bank" ||
                    ctpl.TemplateField.ToLower() == "employment" ||
                    ctpl.TemplateField.ToLower() == "employmentinfo")
                {
                    continue;
                }

                LocalCampaignTemplate lct = new LocalCampaignTemplate();
                lct.Description = ctpl.Description;
                lct.Format = "";
                lct.Required = ctpl.Required;
                lct.TemplateField = ctpl.TemplateField;

                LeadContent lc = new LeadContent();
                PropertyInfo pi = lc.GetType().GetProperty(ctpl.DatabaseField);

                /*if (pi != null)
                {
                    switch (pi.Name.ToLower())
                    {
                        case "ip": lct.Format = "IPv4 Address#[0..9] and [.] allowed"; break;
                        case "minprice": lct.Format = "Decimal#[0..9] and [.] allowed"; break;
                        case "firstname": lct.Format = "String#Alphabet characters"; break;
                        case "lastname": lct.Format = "String#Alphabet characters"; break;
                        case "address": lct.Format = "String#Alphabet or numeric characters"; break;
                        case "city": lct.Format = "String(2)#US city"; break;
                        case "state": lct.Format = "String(2)#US State Abbreviation"; break;
                        case "zip": lct.Format = "Numeric(5)#[0..9]"; break;
                        case "dob": lct.Format = "Date of Birth#[mm-dd-yyyy]"; break;
                        case "age": lct.Format = "Numeric#[0..9]"; break;
                        case "requestedamount": lct.Format = "Numeric#[0..9]"; break;
                        case "ssn": lct.Format = "Numeric#Valid SSN Number, [0..9]"; break;
                        case "homephone": lct.Format = "Numeric(10)#US Phone Number"; break;
                        case "cellphone": lct.Format = "Numeric(10)#US Phone Number"; break;
                        case "email": lct.Format = "E-mail#Valid e-mail address"; break;
                        case "payfrequency": lct.Format = "String#Weekly/Every 2 Weeks/Twice A Month / Monthly"; break;
                        case "directdeposit": lct.Format = "String#Yes/No"; break;
                        case "accounttype": lct.Format = "String#Checking Account/Savings Accoun"; break;
                        case "incometype": lct.Format = "String#Job Income/Benefits/Self Employed/Retirement Income/Disability Income"; break;
                        case "netmonthlyincome": lct.Format = "Numeric#[0..9]"; break;
                        case "emptime": lct.Format = "Numeric#Primary source of income"; break;
                        case "addressmonth": lct.Format = "Numeric#Between 0 and 100 Inclusive"; break;
                        case "affiliatesubid": lct.Format = "String#Alphabet characters"; break;
                        case "affiliatesubid2": lct.Format = "String#Alphabet characters"; break;
                    }
                }
                else
                {
                    switch (ctpl.Validator)
                    {
                        case 1: lct.Format = "String#Alphabet characters"; break;
                        case 2: lct.Format = "Number#[0 - 9]"; break;
                        case 3: lct.Format = "String#Valid E-mail address"; break;
                        case 5: lct.Format = "Number[4-30]#Valid account number"; break;
                        case 6: lct.Format = "Number#Valid SSN number"; break;
                        case 7: lct.Format = "Numeric(5)#Valid ZIP code"; break;
                        case 8: lct.Format = "Numeric(10)#US Phone Number"; break;
                        case 9: lct.Format = "String#Digits and numbers"; break;
                        case 10: lct.Format = "Date#[mm-dd-yyyy]"; break;
                        case 11: lct.Format = "String(2)#US State Abbreviation"; break;
                        case 12: lct.Format = "Number[9]#Valid ABA/Routing Number"; break;
                        case 13: lct.Format = "String#Alphabet and numeric characters"; break;
                        default: lct.Format = "None#Alphabet characters and numbers"; break;
                    }
                }*/

                switch (ctpl.Validator)
                {
                    case 1: lct.Format = "String#"; break;
                    case 2: lct.Format = "Number#"; break;
                    case 3: lct.Format = "String#"; break;
                    case 5: lct.Format = "Number[4-30]#"; break;
                    case 6: lct.Format = "Number#"; break;
                    case 7: lct.Format = "Numeric(5)#"; break;
                    case 8: lct.Format = "Numeric(10)#"; break;
                    case 9: lct.Format = "String#"; break;
                    case 10: lct.Format = "Date#"; break;
                    case 11: lct.Format = "String(2)#"; break;
                    case 12: lct.Format = "Number[9]#"; break;
                    case 13: lct.Format = "String#"; break;
                    default: lct.Format = "None#"; break;
                }

                lct.Format += ctpl.PossibleValue;

                localCampaignTemplateList.Add(lct);
            }

            ViewBag.CampaignTemplateList = localCampaignTemplateList;

            IList<AffiliateChannelTemplate> achtList = this._affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(id);
            foreach (AffiliateChannelTemplate acht in achtList)
            {
                if (acht.TemplateField.ToLower() == AffiliateXmlField.ToLower())
                {
                    ViewBag.CHANNELID = acht.DefaultValue;
                }
                if (acht.TemplateField.ToLower() == passwordField.ToLower() || acht.CampaignTemplateId == passwordFieldId)
                {
                    ViewBag.PASSWORD = acht.DefaultValue;
                }

                Setting postingUrlSetting = this._settingService.GetSetting("System.PostingUrl");
                ViewBag.postingurl = (postingUrlSetting != null && !string.IsNullOrEmpty(postingUrlSetting.Value) ? (!postingUrlSetting.Value.EndsWith("/") ? postingUrlSetting.Value + "/" : postingUrlSetting.Value) : Helper.GetBaseUrl(Request));
            }

            ViewBag.Campaign = this._campaignService.GetCampaignById(am.CampaignId);

            if (ViewBag.Campaign.XmlTemplate != null)
            {
                XDocument xdoc = GetSampleXml(am.CampaignId, ViewBag.Campaign.XmlTemplate);
                ViewBag.XmlTemplate = xdoc.ToString();
            }

            if (ViewBag.CHANNELID == null || (ViewBag.CHANNELID != null && ((string)ViewBag.CHANNELID).Length == 0))
            {
                ViewBag.CHANNELID = affiliateChannels != null ? affiliateChannels.ChannelKey : "";
            }

            return PartialView(am);
        }

        /// <summary>
        /// Deletes the affiliate channel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteAffiliateChannel()
        {
            if (_appContext.AppUser.UserType != UserTypes.Super && !_permissionService.Authorize(PermissionProvider.AffiliateChannelsModify))
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            long affiliatechannelid = 0;

            if (long.TryParse(Request["affiliatechannelid"], out affiliatechannelid))
            {
                AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliatechannelid);
                if (affiliateChannel != null)
                {
                    if (affiliateChannel.IsDeleted.HasValue)
                        affiliateChannel.IsDeleted = !affiliateChannel.IsDeleted.Value;
                    else
                        affiliateChannel.IsDeleted = true;
                    _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);
                }
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Integrations the specified step index.
        /// </summary>
        /// <param name="StepIndex">Index of the step.</param>
        /// <param name="Show">The show.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Integration(int StepIndex, int Show)
        {
            ViewBag.Show = Show;
            ViewBag.StepIndex = StepIndex;

            return PartialView();
        }

        #endregion Methods
    }
}