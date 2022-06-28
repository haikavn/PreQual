// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Data;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using AdRack.Buffering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Handles buyer channel actions
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class BuyerChannelController : BaseContentManagementController
    {
        /// <summary>
        /// Class ScheduleItem.
        /// </summary>
        public class ScheduleItem
        {
            /// <summary>
            /// The day
            /// </summary>
            public string Day = "";

            /// <summary>
            /// From hour
            /// </summary>
            public string FromHour = "";

            /// <summary> 
            /// From minute
            /// </summary>
            public string FromMinute = "";

            /// <summary>
            /// Converts to hour.
            /// </summary>
            public string ToHour = "";

            /// <summary>
            /// Converts to minute.
            /// </summary>
            public string ToMinute = "";

            /// <summary>
            /// The quantity
            /// </summary>
            public string Quantity = "0";

            /// <summary>
            /// The posted wait
            /// </summary>
            public string PostedWait = "0";

            /// <summary>
            /// The sold wait
            /// </summary>
            public string SoldWait = "0";

            /// <summary>
            /// The hour maximum
            /// </summary>
            public string HourMax = "0";

            /// <summary>
            /// The price
            /// </summary>
            public string Price = "0";

            public string LeadStatus = "0"; 
        }

        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The affiliate channel template service
        /// </summary>
        private readonly IAffiliateChannelTemplateService _affiliateChannelTemplateService;

        /// <summary>
        /// The posted date service
        /// </summary>
        private readonly IPostedDataService _postedDateService;

        /// <summary>
        /// The buyer channel template service
        /// </summary>
        private readonly IBuyerChannelTemplateService _buyerChannelTemplateService;

        /// <summary>
        /// The buyer channel template matching service
        /// </summary>
        private readonly IBuyerChannelTemplateMatchingService _buyerChannelTemplateMatchingService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The filter service
        /// </summary>
        private readonly IFilterService _filterService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The lead schedule service
        /// </summary>
        private readonly IBuyerChannelScheduleService _buyerChannelScheduleService;

        /// <summary>
        /// The buyer channel filter condition service
        /// </summary>
        private readonly IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The date time helper
        /// </summary>
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly ISubIdWhiteListService _subIdWhiteListService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="filterService">The filter service.</param>
        /// <param name="buyerChannelFilterConditionService">The buyer channel filter condition service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="buyerChannelTemplateService">The buyer channel template service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="buyerChannelScheduleService">The lead schedule service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="postedDataService">The posted data service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="buyerChannelTemplateMatchingService">The buyer channel template matching service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="affiliateChannelTemplateService">The affiliate channel template service.</param>

        /// <param name="userService">The user service.</param>
        /// <param name="permissionService">The permission service.</param>
        public BuyerChannelController(ICampaignService campaignService, IBuyerService buyerService, IFilterService filterService, IBuyerChannelFilterConditionService buyerChannelFilterConditionService, ICampaignTemplateService campaignTemplateService, IBuyerChannelService buyerChannelService, IBuyerChannelTemplateService buyerChannelTemplateService, ILocalizedStringService localizedStringService, IBuyerChannelScheduleService buyerChannelScheduleService, IAffiliateChannelService affiliateChannelService, IPostedDataService postedDataService, IHistoryService historyService, IBuyerChannelTemplateMatchingService buyerChannelTemplateMatchingService, IAppContext appContext, IAffiliateChannelTemplateService affiliateChannelTemplateService, IUserService userService, IPermissionService permissionService, IDateTimeHelper dateTimeHelper, ISettingService settingService, ISubIdWhiteListService subIdWhiteListService)
        {
            this._campaignService = campaignService;
            this._buyerService = buyerService;
            this._campaignTemplateService = campaignTemplateService;
            this._buyerChannelService = buyerChannelService;
            this._postedDateService = postedDataService;
            this._affiliateChannelService = affiliateChannelService;
            this._buyerChannelTemplateService = buyerChannelTemplateService;
            this._buyerChannelScheduleService = buyerChannelScheduleService;
            this._filterService = filterService;
            this._buyerChannelFilterConditionService = buyerChannelFilterConditionService;
            this._historyService = historyService;
            this._buyerChannelTemplateMatchingService = buyerChannelTemplateMatchingService;
            this._appContext = appContext;
            this._affiliateChannelTemplateService = affiliateChannelTemplateService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._settingService = settingService;
            this._subIdWhiteListService = subIdWhiteListService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Displays buyer channel list interface
        /// </summary>
        /// <returns>View result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyer Channels List")]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Load xml items recursively
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="children">Children list</param>
        /// <param name="cloneFromId">The clone from identifier.</param>
        protected void LoadItems(XmlNode parent, List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> children, long cloneFromId)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
                ti.title = node.Name;
                ti.TemplateField = node.Name;

                BuyerChannelTemplate tpl = _buyerChannelTemplateService.GetBuyerChannelTemplate(cloneFromId, node.Name, node.ParentNode.Name);

                if (tpl != null)
                    ti.CampaignTemplateId = tpl.CampaignTemplateId;

                LoadItems(node, ti.children, cloneFromId);
                if (ti.children.Count > 0)
                {
                    ti.BuyerChannelTemplateId = 0;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.TemplateField = node.Name;
                    ti.DefaultValue = (tpl == null ? "" : tpl.DefaultValue);
                }
                else
                {
                    ti.BuyerChannelTemplateId = 0;
                    ti.folder = false;
                    ti.expanded = false;
                    ti.TemplateField = node.Name;
                    ti.DefaultValue = (tpl == null ? "" : tpl.DefaultValue);
                }

                string matchingsJson = "[";

                if (tpl != null)
                {
                    List<BuyerChannelTemplateMatching> matchings = (List<BuyerChannelTemplateMatching>)_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(tpl.Id);

                    foreach (BuyerChannelTemplateMatching m in matchings)
                    {
                        matchingsJson += "{";

                        matchingsJson += "\"input\": \"" + m.InputValue + "\",";
                        matchingsJson += "\"output\": \"" + m.OutputValue + "\"";

                        matchingsJson += "},";
                    }
                }

                if (matchingsJson != "[")
                    matchingsJson = matchingsJson.Remove(matchingsJson.Length - 1);

                matchingsJson += "]";

                ti.Matchings = matchingsJson;

                children.Add(ti);
            }
        }

        /// <summary>
        /// Loads buyer channel template
        /// </summary>
        /// <returns>Json result</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadBuyerChannelTemplate()
        {
            long cid = 0;

            try
            {
                cid = long.Parse(Request["achannelid"]);
            }
            catch
            {
            }

            BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(cid);

            List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem parent = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
            parent.BuyerChannelTemplateId = 0;
            parent.title = "root";
            parent.folder = true;
            parent.expanded = true;
            parent.TemplateField = "";
            parent.DefaultValue = "";

            if (buyerChannel != null)
            {
                //XmlDocument xmldoc = new XmlDocument();
                //xmldoc.LoadXml(buyerChannel.XmlTemplate);

                List<BuyerChannelTemplate> templates = (List<BuyerChannelTemplate>)this._buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(cid);

                var list = templates.Where(x => x.SectionName == "root").ToList();

                foreach (BuyerChannelTemplate ct in list)
                {
                    Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
                    ti.BuyerChannelTemplateId = ct.Id;
                    ti.title = ct.TemplateField;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.DefaultValue = string.IsNullOrEmpty(ct.DefaultValue) ? "" : ct.DefaultValue;

                    List<BuyerChannelTemplateMatching> matchings = (List<BuyerChannelTemplateMatching>)_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(ct.Id);

                    string matchingsJson = "[";

                    foreach (BuyerChannelTemplateMatching m in matchings)
                    {
                        matchingsJson += "{";

                        matchingsJson += "\"input\": \"" + m.InputValue + "\",";
                        matchingsJson += "\"output\": \"" + m.OutputValue + "\"";

                        matchingsJson += "},";
                    }

                    matchingsJson = matchingsJson.Remove(matchingsJson.Length - 1);

                    matchingsJson += "]";

                    ti.Matchings = matchingsJson;

                    ti.CampaignTemplateId = ct.CampaignTemplateId;

                    LoadChildren(buyerChannel, ct.TemplateField, ti.children);
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
        /// Loads from xml
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadFromXml()
        {
            string data = Request.Unvalidated["xml"];
            long buyerchannelid = 0;

            long.TryParse(Request["BuyerChannelId"], out buyerchannelid);

            if (!string.IsNullOrEmpty(data))
                data = Regex.Replace(data, @"\t|\n|\r", " ");

            if (!string.IsNullOrEmpty(data) && buyerchannelid == 0)
            {
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
            }
            else
            {
                data = "";

                BuyerChannel bc = _buyerChannelService.GetBuyerChannelById(buyerchannelid);

                if (bc != null)
                {
                    data = bc.XmlTemplate;

                    if (string.IsNullOrEmpty(data))
                    {
                        Campaign campaign = _campaignService.GetCampaignById(bc.CampaignId);
                        data = campaign.DataTemplate;
                    }
                }
            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(data);
            Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
            ti.BuyerChannelTemplateId = 0;
            ti.title = xmldoc.DocumentElement.Name;
            ti.folder = true;
            ti.expanded = true;
            ti.TemplateField = xmldoc.DocumentElement.Name;
            ti.DefaultValue = "";

            LoadItems(xmldoc.DocumentElement, ti.children, buyerchannelid);

            if (ti.children == null || ti.children.Count == 0)
                ti.folder = false;

            List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem>();
            items.Add(ti);

            return Json(new { items = items, xml = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads from campaign xml
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        public ActionResult LoadFromCampaignXml()
        {
            Campaign campaign = _campaignService.GetCampaignById(long.Parse(Request["campaignid"]));

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(campaign.DataTemplate);
            Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
            ti.BuyerChannelTemplateId = 0;
            ti.title = xmldoc.DocumentElement.Name;
            ti.folder = true;
            ti.expanded = true;
            ti.TemplateField = "";
            ti.DefaultValue = "";
            ti.Matchings = "[]";

            LoadItems(xmldoc.DocumentElement, ti.children, 0);

            //fix from Arman:buyer channel folder condition
            if (ti.children == null || ti.children.Count == 0)
                ti.folder = false;

            List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem>();
            items.Add(ti);

            return Json(new { items = items, xml = campaign.DataTemplate, campaignType = campaign.CampaignType }, JsonRequestBehavior.AllowGet);
        }

        // GET: Buyer
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Prepares Buyer channel model
        /// </summary>
        /// <param name="model">BuyerChannelModel reference</param>
        /// <param name="buyerId">The buyer identifier.</param>
        protected void PrepareModel(BuyerChannelModel model, long buyerId = 0)
        {
            model.ListResponseFormats.Add(new SelectListItem { Text = "AUTO", Value = "0" });
            model.ListResponseFormats.Add(new SelectListItem { Text = "XML", Value = "1" });
            model.ListResponseFormats.Add(new SelectListItem { Text = "JSON", Value = "2" });
            model.ListResponseFormats.Add(new SelectListItem { Text = "STRING", Value = "3" });
            model.ListResponseFormats.Add(new SelectListItem { Text = "DETECT", Value = "4" });

            model.ListAlwaysSoldOption.Add(new SelectListItem { Text = "Online", Value = "0" });
            model.ListAlwaysSoldOption.Add(new SelectListItem { Text = "Storefront", Value = "1" });

            model.ListWinResponsePostMethod.Add(new SelectListItem { Text = "POST", Value = "POST" });
            model.ListWinResponsePostMethod.Add(new SelectListItem { Text = "GET", Value = "GET" });
            
            model.ListDataFormat.Add(new SelectListItem { Text = "XML", Value = "0" });
            model.ListDataFormat.Add(new SelectListItem { Text = "JSON", Value = "1" });
            model.ListDataFormat.Add(new SelectListItem { Text = "QueryStringPOST", Value = "2" });
            model.ListDataFormat.Add(new SelectListItem { Text = "QueryStringGET", Value = "3" });
            model.ListDataFormat.Add(new SelectListItem { Text = "Email", Value = "4" });
            model.ListDataFormat.Add(new SelectListItem { Text = "SOAP", Value = "5" });

            model.ListStatus.Add(new SelectListItem { Text = "Inactive", Value = "0" });
            model.ListStatus.Add(new SelectListItem { Text = "Active", Value = "1" });
            model.ListStatus.Add(new SelectListItem { Text = "Paused", Value = "2" });

            model.ListDeliveryMethod.Add(new SelectListItem { Text = "XML", Value = "0" });
            model.ListDeliveryMethod.Add(new SelectListItem { Text = "EMAIL", Value = "1" });

            model.ListFromFieldType.Add(new SelectListItem { Text = "Value", Value = "0" });
            model.ListFromFieldType.Add(new SelectListItem { Text = "Field", Value = "1" });

            model.ListBuyerPriceOption.Add(new SelectListItem { Text = "Fixed", Value = "0" });
            model.ListBuyerPriceOption.Add(new SelectListItem { Text = "Dynamic", Value = "1" });            

            model.ListAffiliatePriceOption.Add(new SelectListItem { Text = "Fixed", Value = "0" });
            model.ListAffiliatePriceOption.Add(new SelectListItem { Text = "Revenue share", Value = "1" });
            model.ListAffiliatePriceOption.Add(new SelectListItem { Text = "Take from Affiliate Channel", Value = "2" });

            List<Campaign> list = (List<Campaign>)_campaignService.GetAllCampaigns(0, 0);

            model.ListCampaign.Add(new SelectListItem() { Text = "Select campaign", Value = "" });

            foreach (Campaign item in list)
            {
                model.ListCampaign.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            List<Buyer> alist = (List<Buyer>)_buyerService.GetAllBuyers(0);

            model.ListBuyer.Add(new SelectListItem() { Text = "Select buyer", Value = "" });

            foreach (Buyer item in alist)
            {
                if (buyerId != 0 && buyerId != item.Id) continue;

                model.ListBuyer.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            List<BuyerChannel> bclist = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannelsByCampaignId(model.CampaignId);
            string childChannels = model.ChildChannels;

            string[] childIds = null;

            if (!string.IsNullOrEmpty(childChannels))
            {
                if (childChannels[childChannels.Length - 1] != ',') childChannels += ",";
                childIds = childChannels.Split(new char[1] { ',' });
            }

            model.ListBuyerChannels.Add(new SelectListItem() { Text = "Select buyer channel", Value = "", Selected = true });
            bclist = bclist.OrderBy(x => x.Name).ToList();


            foreach (BuyerChannel item in bclist)
            {
                model.ListChildChannels.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (childIds != null && childIds.Contains(item.Id.ToString()) ? true : false) });

                if (model.BuyerChannelId > 0 && item.Id != model.BuyerChannelId)
                {
                    model.ListBuyerChannels.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = false });
                }
            }

            PrepareFilterConditions(model);

            if (model.BuyerChannelId > 0 || model.ListCampaign.Count == 0)
            {
                model.CampaignTemplate = (List<CampaignField>)this._campaignTemplateService.GetCampaignTemplatesByCampaignId(model.CampaignId, isfilterable: 1);
                model.Filters = (List<Core.Domain.Lead.Filter>)_filterService.GetFiltersByCampaignId(model.CampaignId);
            }

            model.TimeZones = _dateTimeHelper.GetSystemTimeZones("");
        }

        protected void PrepareFilterConditions(BuyerChannelModel model, long parentId = 0)
        {
            if (model.FilterConditions == null)
                model.FilterConditions = new List<BuyerChannelFilterCondition>();

            var conditions = (List<Core.Domain.Lead.BuyerChannelFilterCondition>)_buyerChannelFilterConditionService.GetFilterConditionsByBuyerChannelId(model.BuyerChannelId, parentId);

            foreach(BuyerChannelFilterCondition item in conditions)
            {
                model.FilterConditions.Add(item);
                PrepareFilterConditions(model, item.Id);
            }
        }

        /// <summary>
        /// Loads the children.
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        protected void LoadChildren(BuyerChannel buyerChannel, string parent, List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> children)
        {
            var list = _buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id).Where(x => x.SectionName == parent).ToList();

            foreach (BuyerChannelTemplate node in list)
            {
                Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
                ti.title = node.TemplateField;
                LoadChildren(buyerChannel, node.TemplateField, ti.children);
                if (ti.children.Count > 0)
                {
                    ti.folder = true;
                    ti.expanded = true;
                    ti.TemplateField = "";
                    ti.CampaignTemplateId = 0;
                    ti.BuyerChannelTemplateId = 0;
                    ti.DefaultValue = "";// string.IsNullOrEmpty(node.DefaultValue) ? "" : node.DefaultValue;
                }
                else
                {
                    ti.BuyerChannelTemplateId = node.Id;
                    ti.folder = false;
                    ti.expanded = false;
                    ti.TemplateField = node.TemplateField;
                    ti.CampaignTemplateId = node.CampaignTemplateId;
                    ti.DefaultValue = string.IsNullOrEmpty(node.DefaultValue) ? "" : node.DefaultValue;

                    List<BuyerChannelTemplateMatching> matchings = (List<BuyerChannelTemplateMatching>)_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(node.Id);

                    string matchingsJson = "[";

                    foreach (BuyerChannelTemplateMatching m in matchings)
                    {
                        matchingsJson += "{";

                        matchingsJson += "\"input\": \"" + m.InputValue + "\",";
                        matchingsJson += "\"output\": \"" + m.OutputValue + "\"";

                        matchingsJson += "},";
                    }

                    if (matchingsJson != "[")
                        matchingsJson = matchingsJson.Remove(matchingsJson.Length - 1);

                    matchingsJson += "]";

                    ti.Matchings = matchingsJson;
                }
                children.Add(ti);
            }
        }

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Buyer Channel")]
        public ActionResult Create(long id = 0)
        {
            BuyerChannelModel am = new BuyerChannelModel();

            long buyerid = 0;
            if (long.TryParse(Request["buyerid"], out buyerid))
            {
                am.BuyerId = buyerid;
                am.ReturnToLocalList = true;
            }

            long campaignid = 0;

            if (long.TryParse(Request["campaignid"], out campaignid))
            {
                am.CampaignId = campaignid;
            }

            am.ScheduleItems.Add(new ScheduleItem() { Day = "Sunday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Monday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Tuseday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Wednesday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Thursday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Friday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            am.ScheduleItems.Add(new ScheduleItem() { Day = "Saturday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });

            am.BuyerPrice = 10;

            Buyer buyer = _buyerService.GetBuyerById(buyerid);

            if (buyer != null)
            {
                am.AlwaysSoldOption = buyer.AlwaysSoldOption;
            }

            am.CampaignType = 0;

            Setting tzStr = _settingService.GetSetting("TimeZoneStr");

            am.SelectedTimeZone = (tzStr == null ? "" : tzStr.Value);

            PrepareModel(am, buyerid);

            am.Timeout = 60;
            am.AfterTimeout = 0;

            return View(am);
        }

        /// <summary>
        /// Gets the sample XML.
        /// </summary>
        /// <param name="campaignid">The campaignid.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="affiliateChannelField">The affiliate channel field.</param>
        /// <param name="affiliatePasswordField">The affiliate password field.</param>
        /// <param name="channelValue">The channel value.</param>
        /// <param name="passwordValue">The password value.</param>
        /// <returns>XDocument.</returns>
        [NonAction]
        public XDocument GetSampleXml(long campaignid, string xml, string affiliateChannelField, string affiliatePasswordField, string channelValue, string passwordValue)
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
                        if (e.Name.LocalName == affiliateChannelField)
                        {
                            e.Value = channelValue;
                        }
                        else if (e.Name.LocalName == affiliatePasswordField)
                        {
                            e.Value = passwordValue;
                        }
                        else
                            e.Value = tpl.PossibleValue;
                    }
                }
            }

            return xmldoc;
        }

        /// <summary>
        /// Offers the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Offer")]
        public ActionResult Offer(long id = 0)
        {
            return Item(id);
        }

        /// <summary>
        /// Shows buyer channel create/edit interface
        /// </summary>
        /// <param name="id">Buyer channel</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Buyer Channel")]
        public ActionResult Item(long id = 0)
        {
            BuyerChannel buyerChannels = this._buyerChannelService.GetBuyerChannelById(id);

            BuyerChannelModel am = new BuyerChannelModel();

            am.BuyerChannelId = id;
            long buyerId = 0;
            if (long.TryParse(Request["buyerid"], out buyerId))
            {
                am.ReturnToLocalList = true;
            }

            if (buyerChannels != null)
            {
                am.BuyerChannelId = buyerChannels.Id;
                am.Name = buyerChannels.Name;
                am.XmlTemplate = buyerChannels.XmlTemplate;
                am.Status = buyerChannels.Status;
                am.CampaignId = buyerChannels.CampaignId;
                am.BuyerId = buyerChannels.BuyerId;

                am.AcceptedField = buyerChannels.AcceptedField;
                am.AcceptedValue = buyerChannels.AcceptedValue;
                am.AcceptedFromField = buyerChannels.AcceptedFrom;

                am.ErrorField = buyerChannels.ErrorField;
                am.ErrorValue = buyerChannels.ErrorValue;
                am.ErrorFromField = buyerChannels.ErrorFrom;

                am.RejectedField = buyerChannels.RejectedField;
                am.RejectedValue = buyerChannels.RejectedValue;
                am.RejectedFromField = buyerChannels.RejectedFrom;

                am.TestField = buyerChannels.TestField;
                am.TestValue = buyerChannels.TestValue;
                am.TestFromField = buyerChannels.TestFrom;

                am.RedirectField = buyerChannels.RedirectField;
                am.MessageField = buyerChannels.MessageField;
                am.PriceField = buyerChannels.PriceField;
                am.AccountIdField = buyerChannels.AccountIdField;
                am.Delimeter = buyerChannels.Delimeter;
                am.PriceRejectField = buyerChannels.PriceRejectField;
                am.PriceRejectValue = buyerChannels.PriceRejectValue;

                am.PostingUrl = buyerChannels.PostingUrl;
                am.Timeout = buyerChannels.Timeout;
                am.AfterTimeout = 0;
                am.NotificationEmail = buyerChannels.NotificationEmail;
                am.DeliveryMethod = buyerChannels.DeliveryMethod;

                am.AffiliatePrice = Math.Round(buyerChannels.AffiliatePrice, 2);
                am.BuyerPrice = Math.Round(buyerChannels.BuyerPrice, 2);

                am.TimeoutNotification = buyerChannels.TimeoutNotification;
                am.CapReachedNotification = buyerChannels.CapReachedNotification;

                am.DataFormat = buyerChannels.DataFormat;

                am.PostingHeaders = buyerChannels.PostingHeaders;

                am.AffiliatePriceOption = buyerChannels.AffiliatePriceOption;
                am.BuyerPriceOption = (short)buyerChannels.BuyerPriceOption;
                am.MaxDuplicateDays = (buyerChannels.MaxDuplicateDays.HasValue ? buyerChannels.MaxDuplicateDays.Value : (short)0);

                am.RedirectUrl = buyerChannels.RedirectUrl;

                am.SelectedTimeZone = buyerChannels.TimeZoneStr;
                am.SubIdWhiteListEnabled = (buyerChannels.SubIdWhiteListEnabled.HasValue ? buyerChannels.SubIdWhiteListEnabled.Value : false);
                am.EnableCustomPriceReject = (buyerChannels.EnableCustomPriceReject.HasValue ? buyerChannels.EnableCustomPriceReject.Value : false);
                am.PriceRejectWinResponse = buyerChannels.PriceRejectWinResponse;
                am.FieldAppendEnabled = (buyerChannels.FieldAppendEnabled.HasValue ? buyerChannels.FieldAppendEnabled.Value : false);

                am.WinResponseUrl = buyerChannels.WinResponseUrl;
                am.WinResponsePostMethod = buyerChannels.WinResponsePostMethod;
                am.LeadIdField = buyerChannels.LeadIdField;
                am.ChildChannels = buyerChannels.ChildChannels;
                if (!string.IsNullOrEmpty(am.ChildChannels))
                {
                    if (am.ChildChannels[am.ChildChannels.Length - 1] == ',')
                    {
                        am.ChildChannels = am.ChildChannels.Remove(am.ChildChannels.Length - 1, 1);
                    }
                }

                am.ResponseFormat = (buyerChannels.ResponseFormat.HasValue ? buyerChannels.ResponseFormat.Value : (short)0);
                am.ChannelMappingUniqueId = buyerChannels.ChannelMappingUniqueId;

                am.StatusAutoChange = (buyerChannels.StatusAutoChange.HasValue ? buyerChannels.StatusAutoChange.Value : false);
                am.StatusChangeMinutes = (buyerChannels.StatusChangeMinutes.HasValue ? buyerChannels.StatusChangeMinutes.Value : (short)0);
                am.ChangeStatusAfterCount = (buyerChannels.ChangeStatusAfterCount.HasValue ? buyerChannels.ChangeStatusAfterCount.Value : (short)0);


                am.MinAgeTargeting = buyerChannels.MinAgeTargeting;
                am.MaxAgeTargeting = buyerChannels.MaxAgeTargeting;

                am.DailyCap = (buyerChannels.DailyCap.HasValue ? buyerChannels.DailyCap.Value : (short)0);
                am.Note = buyerChannels.Note;

                List<SelectListItem> holidayItems = new List<SelectListItem>();

                if (!string.IsNullOrEmpty(buyerChannels.Holidays))
                {
                    string[] holidays = buyerChannels.Holidays.Split(new char[1] { ',' });

                    foreach (string s in holidays)
                    {
                        string[] m = s.Split(new char[1] { '-' });
                        int day = 0;
                        int.TryParse(m[1], out day);

                        int month = 0;
                        int.TryParse(m[0], out month);

                        holidayItems.Add(new SelectListItem() { Value = month.ToString() + "-" + day.ToString() });
                    }
                }

                am.Holidays = holidayItems;

                Campaign camp = this._campaignService.GetCampaignById(buyerChannels.CampaignId);
                if (camp != null)
                {
                    am.CampaignType = camp.CampaignType;
                    ViewBag.CampaignName = "Campaign: " + camp.Name + " / " + buyerChannels.CampaignId.ToString();
                }

                //am.AlwaysSoldOption = buyerChannels.AlwaysSoldOption;

                Buyer buyer = _buyerService.GetBuyerById(buyerChannels.BuyerId);

                if (buyer != null)
                {
                    am.AlwaysSoldOption = buyer.AlwaysSoldOption;
                    ViewBag.BuyerName = "Buyer: " + buyer.Name + " / " + buyerChannels.BuyerId.ToString();
                }

                List<AffiliateChannel> aclist = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByCampaignId(buyerChannels.CampaignId, 0);

                foreach (AffiliateChannel ai in aclist)
                {
                    bool isAllowed = false;

                    string[] ids = buyerChannels.AllowedAffiliateChannels.Split(new char[1] { ';' });

                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i].Replace(":", "") == ai.Id.ToString())
                        {
                            isAllowed = true;
                            break;
                        }
                    }

                    am.ListAffiliateChannels.Add(new SelectListItem() { Text = ai.Name, Value = ai.Id.ToString(), Selected = isAllowed });
                }

                string[] allowed = buyerChannels.AllowedAffiliateChannels.Split(new char[1] { ';' });

                for (int i = 0; i < allowed.Length; i++)
                {
                    long l;
                    if (long.TryParse(allowed[i].Replace(":", ""), out l))
                    {
                        am.AllowedAffiliateChannels += "[" + allowed[i].Replace(":", "") + ", true]";
                        if (i < allowed.Length - 1)
                            am.AllowedAffiliateChannels += ",";
                    }
                }

                List<CampaignField> list = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(buyerChannels.CampaignId);

                am.ListCampaignField.Add(new SelectListItem() { Text = "NONE", Value = "0" });

                foreach (CampaignField item in list)
                {
                    am.ListCampaignField.Add(new SelectListItem() { Text = item.TemplateField, Value = item.Id.ToString() });
                }

                List<BuyerChannelSchedule> slist = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(buyerChannels.Id, true, true);

                foreach (var item in slist)
                {
                    ScheduleItem si = new ScheduleItem();

                    switch (item.DayValue)
                    {
                        case 1: si.Day = "Sunday"; break;
                        case 2: si.Day = "Monday"; break;
                        case 3: si.Day = "Tuseday"; break;
                        case 4: si.Day = "Wednesday"; break;
                        case 5: si.Day = "Thursday"; break;
                        case 6: si.Day = "Friday"; break;
                        case 7: si.Day = "Saturday"; break;
                    }

                    double h = (double)Math.Floor((decimal)item.FromTime / 60);

                    TimeSpan span = TimeSpan.FromMinutes(item.FromTime);

                    si.FromHour = (h < 10 ? "0" : "") + h.ToString();

                    si.FromMinute = span.Minutes.ToString();
                    if (span.Minutes < 10) si.FromMinute = "0" + si.FromMinute;

                    h = (double)Math.Floor((decimal)item.ToTime / 60);

                    span = TimeSpan.FromMinutes(item.ToTime);
                    si.ToHour = (h < 10 ? "0" : "") + h.ToString();

                    si.ToMinute = span.Minutes.ToString();
                    if (span.Minutes < 10) si.ToMinute = "0" + si.ToMinute;

                    si.Quantity = (item.Quantity >= 0 ? item.Quantity.ToString() : "");
                    si.PostedWait = item.PostedWait.ToString();
                    si.SoldWait = item.SoldWait.ToString();

                    si.HourMax = item.HourMax.ToString();

                    if (item.Price.HasValue)
                    {
                        if (item.Price.Value > 0)
                            si.Price = Math.Round(item.Price.Value, 2).ToString();
                        else
                            si.Price = "";
                    }
                    else
                    {
                        si.Price = "";
                    }

                    short leadStatus = (item.LeadStatus.HasValue ? item.LeadStatus.Value : (short)-1);

                    si.LeadStatus = leadStatus.ToString();

                    am.ScheduleItems.Add(si);
                }
            }
            else
            {
                List<SelectListItem> holidayItems = new List<SelectListItem>();

                string[] holidays = ("1-1,2-18,28-5,7-4,5-1,10-8,11-11,12-25").Split(new char[1] { ',' });

                foreach (string s in holidays)
                {
                    string[] m = s.Split(new char[1] { '-' });
                    int day = 0;
                    int.TryParse(m[1], out day);

                    int month = 0;
                    int.TryParse(m[0], out month);

                    holidayItems.Add(new SelectListItem() { Value = month.ToString() + "-" + day.ToString() });
                }

                am.Holidays = holidayItems;

                am.ScheduleItems.Add(new ScheduleItem() { Day = "Sunday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Monday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Tuseday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Wednesday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Thursday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Friday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
                am.ScheduleItems.Add(new ScheduleItem() { Day = "Saturday", FromHour = "00", FromMinute = "00", ToHour = "24", ToMinute = "00", Quantity = "", HourMax = "500", Price = "0", LeadStatus = "-1" });
            }

            PrepareModel(am);

            Campaign campaign = _campaignService.GetCampaignById(am.CampaignId);

            CampaignField campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(am.CampaignId, 13);
            string AffiliateXmlField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

            campaignTemplate = this._campaignTemplateService.GetCampaignTemplateByValidator(am.CampaignId, 9);
            string passwordField = campaignTemplate != null ? campaignTemplate.TemplateField : "";

            string affiliateChannelValue = "";
            string affiliatePasswordValue = "";

            IList<AffiliateChannel> achList = this._affiliateChannelService.GetAllAffiliateChannelsByCampaignId(am.CampaignId);

            if (achList.Count > 0)
            {
                int achannelIndex = 0;

                for (int i = 0; i < achList.Count; i++)
                {
                    if (achList[i].Status == 1)
                    {
                        achannelIndex = i;
                        break;
                    }
                }

                IList<AffiliateChannelTemplate> achtList = this._affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(achList[achannelIndex].Id);
                foreach (AffiliateChannelTemplate acht in achtList)
                {
                    if (acht.TemplateField.ToLower() == AffiliateXmlField.ToLower())
                    {
                        affiliateChannelValue = acht.DefaultValue;
                    }
                    if (acht.TemplateField.ToLower() == passwordField.ToLower())
                    {
                        affiliatePasswordValue = acht.DefaultValue;
                    }
                }
            }

            if (campaign != null && campaign.DataTemplate != null)
            {
                XDocument xdoc = GetSampleXml(am.CampaignId, campaign.DataTemplate, AffiliateXmlField, passwordField, affiliateChannelValue, affiliatePasswordValue);
                ViewBag.XmlTemplate = xdoc.ToString();
            }

            ViewBag.BaseUrl = Helper.GetBaseUrl(Request);

            //if (campaign.CampaignType == 0)
            return View(am);
            //else
            //return View("Offer", am);
        }

        /// <summary>
        /// Handles buyer channel submit action
        /// </summary>
        /// <param name="buyerChannelModel">BuyerChannelModel reference</param>
        /// <param name="returnUrl">Redirect url after complete</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Item(BuyerChannelModel buyerChannelModel, string returnUrl)
        {
            bool canClone = false;
            if (!bool.TryParse(Request["canclone"], out canClone))
                canClone = false;

            string data1 = "", data2 = "";

            BuyerChannel buyerChannel = null;
            if (canClone)
                buyerChannelModel.BuyerChannelId = 0;

            if (_buyerChannelService.GetBuyerChannelByName(buyerChannelModel.Name, buyerChannelModel.BuyerChannelId) != null)
            {
                return Json(new { error = "Buyer name already exists" }, JsonRequestBehavior.AllowGet);
            }

            string json = Request.Unvalidated["json"];
            string schedule = Request.Unvalidated["schedule"];
            string xml = Request.Unvalidated["xml"];
            string conditions = Request.Unvalidated["conditions"];
            string pricesJson = Request.Unvalidated["prices"];

            string allowedjson = Request.Unvalidated["allowed"];

            dynamic o = JsonConvert.DeserializeObject(json);
            dynamic so = JsonConvert.DeserializeObject(schedule);
            dynamic co = JsonConvert.DeserializeObject(conditions);
            //dynamic allowed = JsonConvert.DeserializeObject(allowedjson);

            //dynamic prices = JsonConvert.DeserializeObject(pricesJson);

            if (buyerChannelModel.BuyerChannelId == 0)
            {
                buyerChannel = new BuyerChannel();
                buyerChannel.DataFormat = 0;
                buyerChannel.AllowedAffiliateChannels = "";
                buyerChannel.Holidays = "1-1,2-18,5-28,7-4,5-1,10-8,11-11,12-25";
                buyerChannel.LeadAcceptRate = 1;
            }
            else
            {
                buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelModel.BuyerChannelId);
                buyerChannel.Holidays = Request.Unvalidated["holidays"];
            }

            data1 = "Name: " + buyerChannel.Name + ";" +
                           "Status: " + buyerChannel.Status.ToString() + ";" +
                           "CampaignId: " + buyerChannel.CampaignId.ToString() + ";" +
                           "BuyerId: " + buyerChannel.BuyerId.ToString() + ";" +
                           "NotificationEmail: " + (!string.IsNullOrEmpty(buyerChannel.NotificationEmail) ? buyerChannel.NotificationEmail.ToString() : "") + ";" +
                           "AffiliatePrice: " + Math.Round(buyerChannel.AffiliatePrice, 2).ToString() + ";" +
                           "BuyerPrice: " + Math.Round(buyerChannel.BuyerPrice, 2).ToString() + ";" +
                           "CapReachedNotification: " + buyerChannel.CapReachedNotification.ToString() + ";" +
                           "DataFormat: " + buyerChannel.DataFormat.ToString() + ";" +
                           "AffiliatePriceOption: " + buyerChannel.AffiliatePriceOption.ToString() + ";" +
                           "BuyerPriceOption: " + buyerChannel.AffiliatePriceOption.ToString() + ";" +

                           "AcceptedField: " + (!string.IsNullOrEmpty(buyerChannel.AcceptedField) ? buyerChannel.AcceptedField.ToString() : "") + ";" +
                           "AcceptedValue: " + (!string.IsNullOrEmpty(buyerChannel.AcceptedValue) ? buyerChannel.AcceptedValue.ToString() : "") + ";" +
                           "AcceptedFrom: " + buyerChannel.AcceptedFrom.ToString() + ";" +
                           "RejectedField: " + (!string.IsNullOrEmpty(buyerChannel.RejectedField) ? buyerChannel.RejectedField.ToString() : "") + ";" +
                           "RejectedValue: " + (!string.IsNullOrEmpty(buyerChannel.RejectedValue) ? buyerChannel.RejectedValue.ToString() : "") + ";" +
                           "RejectedFrom: " + buyerChannel.RejectedFrom.ToString() + ";" +
                           "ErrorField: " + (!string.IsNullOrEmpty(buyerChannel.ErrorField) ? buyerChannel.ErrorField.ToString() : "") + ";" +
                           "ErrorValue: " + (!string.IsNullOrEmpty(buyerChannel.ErrorValue) ? buyerChannel.ErrorValue.ToString() : "") + ";" +
                           "ErrorFrom: " + buyerChannel.ErrorFrom.ToString() + ";" +
                           "TestField: " + (!string.IsNullOrEmpty(buyerChannel.TestField) ? buyerChannel.TestField.ToString() : "") + ";" +
                           "TestValue: " + (!string.IsNullOrEmpty(buyerChannel.TestValue) ? buyerChannel.TestValue.ToString() : "") + ";" +
                           "TestFrom: " + buyerChannel.TestFrom.ToString() + ";" +
                           "RedirectField: " + (!string.IsNullOrEmpty(buyerChannel.RedirectField) ? buyerChannel.RedirectField.ToString() : "") + ";" +
                           "MessageField: " + (!string.IsNullOrEmpty(buyerChannel.MessageField) ? buyerChannel.MessageField.ToString() : "") + ";" +
                           "PriceField: " + (!string.IsNullOrEmpty(buyerChannel.PriceField) ? buyerChannel.PriceField.ToString() : "") + ";" +
                           "PostingHeaders: " + (!string.IsNullOrEmpty(buyerChannel.PostingHeaders) ? buyerChannel.PostingHeaders.ToString() : "") + ";" +
                           "TimeoutNotification: " + buyerChannel.TimeoutNotification.ToString() + ";" +
                           "PostingUrl: " + (!string.IsNullOrEmpty(buyerChannel.PostingUrl) ? buyerChannel.PostingUrl.ToString() : "") + ";" +
                           "TimeoutNotification: " + buyerChannel.Timeout.ToString() + ";";


            buyerChannel.Name = buyerChannelModel.Name.Trim();
            buyerChannel.Status = buyerChannelModel.Status;
            buyerChannel.XmlTemplate = xml;
            buyerChannel.CampaignId = buyerChannelModel.CampaignId;
            buyerChannel.BuyerId = buyerChannelModel.BuyerId;
            buyerChannel.MaxDuplicateDays = (buyerChannelModel.MaxDuplicateDays.HasValue ? buyerChannelModel.MaxDuplicateDays.Value : (short)0);
            buyerChannel.Timeout = buyerChannelModel.Timeout;
            buyerChannel.AfterTimeout = 0;

            buyerChannel.DeliveryMethod = 0;//buyerChannelModel.DeliveryMethod;
            buyerChannel.NotificationEmail = buyerChannelModel.NotificationEmail;

            buyerChannel.AffiliatePrice = Math.Round(buyerChannelModel.AffiliatePrice, 2);
            buyerChannel.BuyerPrice = Math.Round(buyerChannelModel.BuyerPrice, 2);

            buyerChannel.CapReachedNotification = buyerChannelModel.CapReachedNotification;

            buyerChannel.DataFormat = buyerChannelModel.DataFormat;

            buyerChannel.AffiliatePriceOption = buyerChannelModel.AffiliatePriceOption;
            buyerChannel.BuyerPriceOption = (BuyerPriceOptions)buyerChannelModel.BuyerPriceOption;

            buyerChannel.RedirectUrl = Request.Unvalidated["redirecturl"];

            buyerChannel.TimeZoneStr = buyerChannelModel.SelectedTimeZone;
            buyerChannel.SubIdWhiteListEnabled = buyerChannelModel.SubIdWhiteListEnabled;
            buyerChannel.EnableCustomPriceReject = buyerChannelModel.EnableCustomPriceReject;
            buyerChannel.PriceRejectWinResponse = buyerChannelModel.PriceRejectWinResponse;
            buyerChannel.FieldAppendEnabled = buyerChannelModel.FieldAppendEnabled;
            buyerChannel.WinResponseUrl = buyerChannelModel.WinResponseUrl;
            buyerChannel.WinResponsePostMethod = buyerChannelModel.WinResponsePostMethod;
            buyerChannel.LeadIdField = buyerChannelModel.LeadIdField;
            buyerChannel.ChildChannels = (!string.IsNullOrEmpty(buyerChannelModel.ChildChannels) ? buyerChannelModel.ChildChannels.Trim() : "");
            if (!string.IsNullOrEmpty(buyerChannel.ChildChannels))
                if (buyerChannel.ChildChannels[buyerChannel.ChildChannels.Length - 1] != ',') buyerChannel.ChildChannels += ",";

            buyerChannel.ResponseFormat = buyerChannelModel.ResponseFormat;
            buyerChannel.ChannelMappingUniqueId = buyerChannelModel.ChannelMappingUniqueId;

            buyerChannel.StatusChangeMinutes = buyerChannelModel.StatusChangeMinutes;
            buyerChannel.StatusAutoChange = buyerChannelModel.StatusAutoChange;
            buyerChannel.ChangeStatusAfterCount = buyerChannelModel.ChangeStatusAfterCount;

            buyerChannel.DailyCap = buyerChannelModel.DailyCap;
            buyerChannel.Note = buyerChannelModel.Note;

            if ((!buyerChannelModel.StatusAutoChange || buyerChannelModel.Status != BuyerChannelStatuses.Paused) && buyerChannel.StatusExpireDate.HasValue)
            {
                buyerChannel.StatusExpireDate = null;
                buyerChannel.StatusStr = "";
            }

            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(buyerChannelModel.SelectedTimeZone);
            buyerChannel.TimeZone = zone.Id;

            if (!string.IsNullOrEmpty(buyerChannel.RedirectUrl) && !buyerChannel.RedirectUrl.StartsWith("http://") && !buyerChannel.RedirectUrl.StartsWith("https://"))
            {
                buyerChannel.RedirectUrl = "http://" + buyerChannel.RedirectUrl;
            }

            Buyer buyer = _buyerService.GetBuyerById(buyerChannel.BuyerId);

            if (buyer != null && buyer.AlwaysSoldOption == 0)
            {
                buyerChannel.AlwaysSoldOption = buyer.AlwaysSoldOption;

                buyerChannel.AcceptedField = buyerChannelModel.AcceptedField;
                buyerChannel.AcceptedValue = buyerChannelModel.AcceptedValue;
                buyerChannel.AcceptedFrom = buyerChannelModel.AcceptedFromField;

                buyerChannel.RejectedField = buyerChannelModel.RejectedField;
                buyerChannel.RejectedValue = buyerChannelModel.RejectedValue;
                buyerChannel.RejectedFrom = buyerChannelModel.RejectedFromField;

                buyerChannel.ErrorField = buyerChannelModel.ErrorField;
                buyerChannel.ErrorValue = buyerChannelModel.ErrorValue;
                buyerChannel.ErrorFrom = buyerChannelModel.ErrorFromField;

                buyerChannel.TestField = buyerChannelModel.TestField;
                buyerChannel.TestValue = buyerChannelModel.TestValue;
                buyerChannel.TestFrom = buyerChannelModel.TestFromField;

                buyerChannel.RedirectField = buyerChannelModel.RedirectField;
                buyerChannel.MessageField = buyerChannelModel.MessageField;
                buyerChannel.PriceField = buyerChannelModel.PriceField;
                buyerChannel.AccountIdField = buyerChannelModel.AccountIdField;

                buyerChannel.Delimeter = buyerChannelModel.Delimeter;
                buyerChannel.PriceRejectField = buyerChannelModel.PriceRejectField;
                buyerChannel.PriceRejectValue = buyerChannelModel.PriceRejectValue;

                buyerChannel.PostingHeaders = buyerChannelModel.PostingHeaders;
                buyerChannel.TimeoutNotification = buyerChannelModel.TimeoutNotification;
                buyerChannel.PostingUrl = buyerChannelModel.PostingUrl;
            }
            else if (buyer != null && buyer.AlwaysSoldOption == 2)
            {
                buyerChannel.PostingUrl = buyerChannelModel.PostingUrl;
                buyerChannel.PostingHeaders = buyerChannelModel.PostingHeaders;
            }

            

            if (buyerChannelModel.BuyerChannelId == 0)
            {
                buyerChannel.OrderNum = 0;
                buyerChannel.LeadAcceptRate = 0;

                long newId = _buyerChannelService.InsertBuyerChannel(buyerChannel);
                this._historyService.AddHistory("BuyerChannelController", HistoryAction.Buyer_Channel_Added, "BuyerChannel", newId, "", data1, "", this._appContext.AppUser.Id);
            }
            else
            {
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                data2 = "Name: " + buyerChannel.Name + ";" +
                               "Status: " + buyerChannel.Status.ToString() + ";" +
                               "CampaignId: " + buyerChannel.CampaignId.ToString() + ";" +
                               "BuyerId: " + buyerChannel.BuyerId.ToString() + ";" +
                               "NotificationEmail: " + buyerChannel.NotificationEmail.ToString() + ";" +
                               "AffiliatePrice: " + Math.Round(buyerChannel.AffiliatePrice, 2).ToString() + ";" +
                               "BuyerPrice: " + Math.Round(buyerChannel.BuyerPrice, 2).ToString() + ";" +
                               "CapReachedNotification: " + buyerChannel.CapReachedNotification.ToString() + ";" +
                               "DataFormat: " + buyerChannel.DataFormat.ToString() + ";" +
                               "AffiliatePriceOption: " + buyerChannel.AffiliatePriceOption.ToString() + ";" +
                               "BuyerPriceOption: " + buyerChannel.AffiliatePriceOption.ToString() + ";" +

                               "AcceptedField: " + (!string.IsNullOrEmpty(buyerChannel.AcceptedField) ? buyerChannel.AcceptedField.ToString() : "") + ";" +
                               "AcceptedValue: " + (!string.IsNullOrEmpty(buyerChannel.AcceptedValue) ? buyerChannel.AcceptedValue.ToString() : "") + ";" +
                               "AcceptedFrom: " + buyerChannel.AcceptedFrom.ToString() + ";" +
                               "RejectedField: " + (!string.IsNullOrEmpty(buyerChannel.RejectedField) ? buyerChannel.RejectedField.ToString() : "") + ";" +
                               "RejectedValue: " + (!string.IsNullOrEmpty(buyerChannel.RejectedValue) ? buyerChannel.RejectedValue.ToString() : "") + ";" +
                               "RejectedFrom: " + buyerChannel.RejectedFrom.ToString() + ";" +
                               "ErrorField: " + (!string.IsNullOrEmpty(buyerChannel.ErrorField) ? buyerChannel.ErrorField.ToString() : "") + ";" +
                               "ErrorValue: " + (!string.IsNullOrEmpty(buyerChannel.ErrorValue) ? buyerChannel.ErrorValue.ToString() : "") + ";" +
                               "ErrorFrom: " + buyerChannel.ErrorFrom.ToString() + ";" +
                               "TestField: " + (!string.IsNullOrEmpty(buyerChannel.TestField) ? buyerChannel.TestField.ToString() : "") + ";" +
                               "TestValue: " + (!string.IsNullOrEmpty(buyerChannel.TestValue) ? buyerChannel.TestValue.ToString() : "") + ";" +
                               "TestFrom: " + buyerChannel.TestFrom.ToString() + ";" +
                               "RedirectField: " + (!string.IsNullOrEmpty(buyerChannel.RedirectField) ? buyerChannel.RedirectField.ToString() : "") + ";" +
                               "MessageField: " + (!string.IsNullOrEmpty(buyerChannel.MessageField) ? buyerChannel.MessageField.ToString() : "") + ";" +
                               "PriceField: " + (!string.IsNullOrEmpty(buyerChannel.PriceField) ? buyerChannel.PriceField.ToString() : "") + ";" +
                               "PostingHeaders: " + (!string.IsNullOrEmpty(buyerChannel.PostingHeaders) ? buyerChannel.PostingHeaders.ToString() : "") + ";" +
                               "TimeoutNotification: " + buyerChannel.TimeoutNotification.ToString() + ";" +
                               "PostingUrl: " + (!string.IsNullOrEmpty(buyerChannel.PostingUrl) ? buyerChannel.PostingUrl.ToString() : "") + ";" +
                               "TimeoutNotification: " + buyerChannel.Timeout.ToString() + ";";

                this._historyService.AddHistory("BuyerChannelController", HistoryAction.Buyer_Channel_Edited, "BuyerChannel", buyerChannelModel.BuyerChannelId, data1, data2, "", this._appContext.AppUser.Id);
            }

            buyerChannelModel.BuyerChannelId = buyerChannel.Id;

            PrepareModel(buyerChannelModel);

            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(xml);
            }
            catch
            {
            }

            string allowedstr = "";
            string[] allowedar = allowedjson.Split(new char[1] { ',' });
            for (int i = 0; i < allowedar.Length; i++)
            {
                allowedstr += ":" + allowedar[i] + ";";
            }

            _buyerChannelFilterConditionService.DeleteFilterConditions(buyerChannelModel.BuyerChannelId);

            bool zipFound = false;
            bool ageFound = false;
            bool stateFound = false;

            long parentId = 0;

            for (int i = 0; i < co.Count; i++)
            {
                string field = co[i]["field"].ToString();
                string condition = co[i]["condition"].ToString();
                string value = co[i]["value"].ToString();
                string op = co[i]["operator"].ToString();
                string parent = co[i]["parent"].ToString();

                value = Regex.Replace(value, @"\t|\n|\r", " ");

                BuyerChannelFilterCondition fc = new BuyerChannelFilterCondition();                   

                fc.BuyerChannelId = buyerChannelModel.BuyerChannelId;
                fc.Condition = short.Parse(condition);
                fc.Value = value.Trim();
                fc.ConditionOperator = short.Parse(op);
                fc.CampaignTemplateId = long.Parse(field);
                fc.ParentId = (parent != "0" ? parentId : 0);

                this._buyerChannelFilterConditionService.InsertFilterCondition(fc);

                if (parent == "0")
                    parentId = fc.Id;
            }

            List<BuyerChannelFilterCondition> filters = (List<BuyerChannelFilterCondition>)this._buyerChannelFilterConditionService.GetFilterConditionsByBuyerChannelId(buyerChannel.Id);
            foreach(BuyerChannelFilterCondition fc in filters)
            {
                CampaignField ct = _campaignTemplateService.GetCampaignTemplateById(fc.CampaignTemplateId);

                if (_buyerChannelFilterConditionService.HasChildren(fc.Id) || (fc.ParentId.HasValue && fc.ParentId.Value > 0)) continue;

                if (ct != null)
                {
                    if (ct.Validator == 7)
                    {
                        zipFound = true;
                        buyerChannel.EnableZipCodeTargeting = true;
                        buyerChannel.ZipCodeTargeting = fc.Value;
                        buyerChannel.ZipCodeCondition = fc.Condition;
                    }
                    else if (ct.Validator == 11)
                    {
                        stateFound = true;
                        buyerChannel.EnableStateTargeting = true;
                        buyerChannel.StateTargeting = fc.Value;
                        buyerChannel.StateCondition = fc.Condition;
                    }
                    else if (ct.Validator == 14)
                    {
                        /*buyerChannel.EnableAgeTargeting = true;

                        string[] values = fc.Value.Split(new char[1] { '-' });

                        short v = 0;
                        if (values.Length > 0)
                            short.TryParse(values[0], out v);

                        short v2 = 0;
                        if (values.Length > 1)
                            short.TryParse(values[1], out v2);

                        buyerChannel.MinAgeTargeting = v;
                        buyerChannel.MaxAgeTargeting = v2;*/
                    }
                }
            }

            //arman
            _buyerChannelScheduleService.DeleteBuyerChannelSchedulesByBuyerChannelId(buyerChannel.Id);

            for (int i = 0; i < so.Count; i++)
            {
                string day = so[i]["day"].ToString();
                string fromHour = so[i]["fromHour"].ToString();
                string fromMinute = so[i]["fromMinute"].ToString();

                string toHour = so[i]["toHour"].ToString();
                string toMinute = so[i]["toMinute"].ToString();

                string quantity = so[i]["quantity"].ToString();

                string pwait = so[i]["pwait"].ToString();
                string swait = so[i]["swait"].ToString();

                string hmax = so[i]["hmax"].ToString();

                string price = so[i]["price"].ToString();

                string leadStatus = so[i]["status"].ToString();

                BuyerChannelSchedule ls = new BuyerChannelSchedule();

                ls.BuyerChannelId = buyerChannel.Id;
                switch (day)
                {
                    case "Sunday": ls.DayValue = 1; break;
                    case "Monday": ls.DayValue = 2; break;
                    case "Tuseday": ls.DayValue = 3; break;
                    case "Wednesday": ls.DayValue = 4; break;
                    case "Thursday": ls.DayValue = 5; break;
                    case "Friday": ls.DayValue = 6; break;
                    case "Saturday": ls.DayValue = 7; break;
                }

                ls.FromTime = (int.Parse(fromHour)) * 60 + int.Parse(fromMinute);
                ls.ToTime = (int.Parse(toHour)) * 60 + int.Parse(toMinute);
                try
                {
                    ls.Quantity = int.Parse(quantity);
                }
                catch
                {
                    ls.Quantity = -1;
                }

                try
                {
                    ls.PostedWait = int.Parse(pwait);
                }
                catch
                {
                    ls.PostedWait = 0;
                }

                try
                {
                    ls.SoldWait = int.Parse(swait);
                }
                catch
                {
                    ls.SoldWait = 0;
                }

                try
                {
                    ls.HourMax = int.Parse(hmax);
                }
                catch
                {
                    ls.HourMax = 0;
                }

                try
                {
                    ls.Price = decimal.Parse(price);
                }
                catch
                {
                    ls.Price = 0;
                }

                try
                {
                    ls.LeadStatus = short.Parse(leadStatus);
                }
                catch
                {
                    ls.LeadStatus = 0;
                }

                _buyerChannelScheduleService.InsertBuyerChannelSchedule(ls);
            }

            _buyerChannelTemplateService.DeleteBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id);

            for (int i = 0; i < o.Count; i++)
            {
                if (o[i].Count == 0)
                    continue;
                string tfield = o[i][0].ToString();
                string sfield = o[i][1].ToString();
                string defaultvalue = o[i][2].ToString();
                string matchingsJson = o[i][3].ToString();
                string parent = o[i][4].ToString();
                //string blacklist = o[i][6].ToString();

                if (matchingsJson == "")
                    matchingsJson = "[]";

                dynamic matchings = JsonConvert.DeserializeObject(matchingsJson);

                BuyerChannelTemplate ct = new BuyerChannelTemplate();
                ct.BuyerChannelId = buyerChannel.Id;
                ct.CampaignTemplateId = long.Parse(sfield);
                ct.TemplateField = tfield;
                ct.DefaultValue = defaultvalue;
                ct.SectionName = parent;

                _buyerChannelTemplateService.InsertBuyerChannelTemplate(ct);

                CampaignField campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(ct.CampaignTemplateId);
                if (campaignTemplate != null && campaignTemplate.Validator == 14)
                {
                    if (buyerChannelModel.MaxAgeTargeting > 0 && buyerChannelModel.MinAgeTargeting > 0)
                    {
                        ageFound = true;
                        buyerChannel.EnableAgeTargeting = true;
                    }
                    else
                    {
                        ageFound = false;
                    }
                }

                try
                {
                    if (matchings!=null)
                    for (int j = 0; j < matchings.Count; j++)
                    {
                        BuyerChannelTemplateMatching btm = new BuyerChannelTemplateMatching();
                        btm.BuyerChannelTemplateId = ct.Id;
                        btm.InputValue = matchings[j]["input"].ToString();
                        btm.OutputValue = matchings[j]["output"].ToString();
                        _buyerChannelTemplateMatchingService.InsertBuyerChannelTemplateMatching(btm);
                    }
                }
                catch
                {
                }

                if (parent == "root") continue;

                /*XmlNodeList nodes = xmldoc.GetElementsByTagName(parent);
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
                }*/
            }

            buyerChannel.AllowedAffiliateChannels = allowedstr;

            buyerChannel.XmlTemplate = xmldoc.OuterXml;

            buyerChannel.MaxAgeTargeting = buyerChannelModel.MaxAgeTargeting;
            buyerChannel.MinAgeTargeting = buyerChannelModel.MinAgeTargeting;

            if (!zipFound)
            {
                buyerChannel.EnableZipCodeTargeting = false;
                buyerChannel.ZipCodeTargeting = "";
            }

            if (!stateFound)
            {
                buyerChannel.EnableStateTargeting = false;
                buyerChannel.StateTargeting = "";
            }

            if (!ageFound)
            {
                buyerChannel.EnableAgeTargeting = false;
            }

            _buyerChannelService.UpdateBuyerChannel(buyerChannel);

            HttpPostedFileBase file = (Request.Files.Count > 0 ? Request.Files[0] : null);

            if (file != null && file.ContentLength > 0)
            {
                _subIdWhiteListService.DeleteAllSubIdWhiteList(buyerChannel.Id);

                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                string dir = Server.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(dir, fileName);
                file.SaveAs(path);

                StreamReader sr = new StreamReader(path);
                string line = "";
                int lineNumber = 0;
                int index = -1;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');

                    if (lineNumber == 0)
                    {
                        index = Array.IndexOf(row, "MAX_SUBID");
                    }
                    else if (index >= 0 && index < row.Length)
                    {
                        _subIdWhiteListService.InsertSubIdWhiteList(new SubIdWhiteList() { SubId = row[index], BuyerChannelId = buyerChannel.Id });
                    }

                    lineNumber++;
                }
                sr.Close();

                System.IO.File.Delete(path);
            }

            SharedData.ResetBuyerChannelLeadsCount(buyerChannel.CampaignId);

            return Json(new { id = buyerChannel.Id, name = buyerChannel.Name }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets buyer channel order
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult SetBuyerChannelOrder()
        {
            long id = 0;
            long.TryParse(Request["id"], out id);

            long campaignid = 0;
            long.TryParse(Request["campaignid"], out campaignid);

            int? order = null;
            int o = 0;
            if (!string.IsNullOrEmpty(Request["order"]))
            {
                int.TryParse(Request["order"], out o);
                order = o;
            }

            bool? isfixed = null;
            bool b = false;
            if (!string.IsNullOrEmpty(Request["isfixed"]))
            {
                bool.TryParse(Request["isfixed"], out b);
                isfixed = b;
            }

            List<BuyerChannel> list = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannelsByCampaignId(campaignid).OrderBy(x => x.OrderNum).ToList();

            BuyerChannel bc = _buyerChannelService.GetBuyerChannelById(id);
            if (bc == null) return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
            if (bc != null)
            {
                var fixedChannel = list.Where(x => x.OrderNum == o && x.Id != bc.Id && x.IsFixed).FirstOrDefault();

                if (fixedChannel != null)
                {
                    return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (order.HasValue)
                    bc.OrderNum = order.Value;
                if (isfixed.HasValue)
                    bc.IsFixed =  isfixed.Value;

                _buyerChannelService.UpdateBuyerChannel(bc);
            }

            if (isfixed.HasValue)
            {
                return Json(new { list = list }, JsonRequestBehavior.AllowGet);
            }

            List<BuyerChannel> newList = new List<BuyerChannel>();

            list = (List<BuyerChannel>)list.OrderBy(x => x.OrderNum).Where(x => x.IsFixed == false).ToList();

            for(int i = 0; i < list.Count; i++)
            {
                list[i].OrderNum = i + 1;
                newList.Add(list[i]);
            }

            var fList = (List<BuyerChannel>)list.OrderBy(x => x.OrderNum).Where(x => x.IsFixed == true).ToList();

            foreach(BuyerChannel item in fList)
            {
                newList.Insert(item.OrderNum - 1, item);
            }

            for (int i = 0; i < newList.Count; i++)
            {
                newList[i].OrderNum = i + 1;
                _buyerChannelService.UpdateBuyerChannel(newList[i]);
            }

            return Json(new { list = newList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get buyer channel list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyerChannels()
        {
            JsonData jd = new JsonData();

            if (_appContext.AppUser == null)
            {
                return Json(jd, JsonRequestBehavior.AllowGet);
            }

            short mode = 0;

            short.TryParse(Request["mode"], out mode);

            string o = Request["o"];

            int active = -1;
            if (!string.IsNullOrEmpty(Request["a"]))
                int.TryParse(Request["a"], out active);

            string c = Request["Params"];

            string bid = Request["bid"];

            short showOffers = 0;
            short.TryParse(Request["offers"], out showOffers);

            short deleted = 0;

            short.TryParse(Request["d"], out deleted);

            string id = Request["id"];


            List<BuyerChannel> buyerChannels = new List<BuyerChannel>();

            if (string.IsNullOrEmpty(o))
                buyerChannels = (List<BuyerChannel>)this._buyerChannelService.GetAllBuyerChannels(deleted);
            else
                buyerChannels = (List<BuyerChannel>)this._buyerChannelService.GetAllBuyerChannelsByOrder(deleted);

            List<UserBuyerChannel> userBuyerChannels = (List<UserBuyerChannel>)_userService.GetUserBuyerChannels(_appContext.AppUser.Id);

            buyerChannels = buyerChannels.OrderBy(x => x.Name).ToList();

            List<object> data = new List<object>();

            int row = 1;

            jd.draw = 1;
            jd.recordsTotal = buyerChannels.Count;
            jd.recordsFiltered = buyerChannels.Count;
            foreach (BuyerChannel ai in buyerChannels)
            {
                if (!string.IsNullOrEmpty(id) && ai.Id.ToString() != bid) continue;

                if (!string.IsNullOrEmpty(c) && ai.CampaignId.ToString() != c) continue;

                if (active == 0 && ai.Status == BuyerChannelStatuses.Active) continue;
                else
                if (active == 1 && ai.Status != BuyerChannelStatuses.Active && ai.Status != BuyerChannelStatuses.Paused) continue;

                if (!string.IsNullOrEmpty(bid) && ai.BuyerId.ToString() != bid) continue;

                if (mode == 0)
                {
                    Campaign campaign = _campaignService.GetCampaignById(ai.CampaignId);

                    Buyer buyer = _buyerService.GetBuyerById(ai.BuyerId);

                    string ctrl = "BuyerChannel";
                    if (showOffers == 1) ctrl = "Offer";

                    string strStatus = "";

                    switch (ai.Status)
                    {
                        case BuyerChannelStatuses.Inactive: strStatus = "<span style='color: red'>Inactive</span>"; break;
                        case BuyerChannelStatuses.Active: strStatus = "<span style='color: green'>Active</span>"; break;
                        case BuyerChannelStatuses.Paused: strStatus = "<span style='color: orange'>Paused<br>(" + (!string.IsNullOrEmpty(ai.StatusStr) ? ai.StatusStr : "") + ")</span>"; break;
                    }

                    if (!string.IsNullOrEmpty(o))
                    {
                        object names1 = new
                        {
                            row = ai.OrderNum.ToString(),
                            order = ai.OrderNum.ToString(),//"<input id='bcorder-" + ai.Id.ToString() + "' type='number' width='10' class='order-num form-control' value='" + ai.OrderNum.ToString() + "' onchange='distrib_sort(this)' onkeyup='distrib_sort(this)' " + (ai.IsFixed ? "readonly" : "") + (ai.IsFixed ? " readonly='readonly'" : "") + " style='width: 100px; display: inline;' /><span style='opacity: 0'>" + ai.OrderNum.ToString() + "</span>",
                            name = "<a href=\"/Management/" + ctrl + "/Item/" + ai.Id.ToString() + (!string.IsNullOrEmpty(bid) ? "?buyerid=" + bid : "") + "\">" + ai.Name + "</a>",
                            status = strStatus,
                            bprice = Math.Round(ai.BuyerPrice, 0).ToString(),
                            buyerPriceOption = ai.BuyerPriceOption,
                            @fixed = ai.IsFixed,
                            id = ai.Id.ToString(),
                            rate = (ai.LeadAcceptRate.HasValue ? ai.LeadAcceptRate.Value.ToString() : "1")
                        };
                        data.Add(names1);
                    }
                    else
                    {
                        string[] names1 = {
                                      ai.Id.ToString(),
                                      //(_appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId || _appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId || userBuyerChannels.Count > 0 ? "<a href=\"/Management/" + ctrl + "/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>" : "<b>" + ai.Name + "</b>" ),
                                      "<a href=\"/Management/" + ctrl + "/Item/" + ai.Id.ToString() + (!string.IsNullOrEmpty(bid) ? "?buyerid=" + bid : "") + "\">" + ai.Name + "</a>",
                                      buyer == null ? "" : buyer.Name,
                                      campaign == null ? "" : campaign.Name,
                                      strStatus,
                                      "<a href='#' onclick='deleteBuyerChannel(" + ai.Id + ")'>" + (ai.Deleted.HasValue ? (ai.Deleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                                 };
                        jd.data.Add(names1);
                    }
                }
                else
                {
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      ai.Name,
                                 };
                    jd.data.Add(names1);
                }

                row++;
            }

            if (data.Count > 0)
            {
                return Json(new { data = data }, JsonRequestBehavior.AllowGet); ;
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get posted data list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetPostedData()
        {
            string bid = Request["bid"];

            List<PostedData> postedDatas = (List<PostedData>)this._postedDateService.GetPostedDatasByBuyerChannelId(long.Parse(bid));

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = postedDatas.Count;
            jd.recordsFiltered = postedDatas.Count;
            foreach (PostedData ai in postedDatas)
            {
                string[] names1 = {
                                      ai.LeadId != null ? ai.LeadId.ToString() : "0",
                                      ai.Created.ToShortDateString() + " " + ai.Created.ToShortTimeString(),
                                      HttpUtility.HtmlEncode(ai.Posted)
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get allowed affiliate channel list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAllowedFrom()
        {
            long buyerchannelid = 0;

            long.TryParse(Request["BuyerChannelId"], out buyerchannelid);

            BuyerChannel bc = _buyerChannelService.GetBuyerChannelById(buyerchannelid);

            long campaignid = 0;

            if (bc != null)
            {
                campaignid = bc.CampaignId;
            }
            else
            {
                long.TryParse(Request["campaignid"], out campaignid);
            }

            List<AffiliateChannel> list = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByCampaignId(campaignid);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = list.Count;
            jd.recordsFiltered = list.Count;
            foreach (AffiliateChannel ai in list)
            {
                bool isAllowed = false;

                if (bc != null)
                {
                    string[] ids = bc.AllowedAffiliateChannels.Split(new char[1] { ';' });

                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i].Replace(":", "") == ai.Id.ToString())
                        {
                            isAllowed = true;
                            break;
                        }
                    }
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"/Management/AffiliateChannel/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      "<input type='checkbox' class='fixed-col' onchange='allowedChanged(this)' " + (isAllowed ? "checked" : "") + " data-id='" + ai.Id.ToString() + "' />"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get allowed affiliate channel list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerChannelsByCampaign()
        {
            short mode = 0;

            short.TryParse(Request["mode"], out mode);

            string id = Request["id"];

            Campaign campaign = _campaignService.GetCampaignById(long.Parse(id.ToString()));

            List<BuyerChannel> list = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannelsByCampaignId(long.Parse(id.ToString()), 0);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = list.Count;
            jd.recordsFiltered = list.Count;
            foreach (BuyerChannel ai in list)
            {
                string strStatus = "";

                switch (ai.Status)
                {
                    case BuyerChannelStatuses.Inactive: strStatus = "<span style='color: red'>Inactive</span>"; break;
                    case BuyerChannelStatuses.Active: strStatus = "<span style='color: green'>Active</span>"; break;
                    case BuyerChannelStatuses.Paused: strStatus = "<span style='color: orange'>Paused</span>"; break;
                }

                string ctrl = "BuyerChannel";
                if (campaign != null && campaign.CampaignType == CampaignTypes.ClickCampaign) ctrl = "Offer";

                if (mode == 0)
                {
                    bool isAllowed = _buyerChannelService.CheckAllowedAffiliateChannel(long.Parse(Request["aid"]), ai.Id);
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"/Management/" + ctrl + "/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      "<input type='checkbox' class='fixed-col bcallowed' onchange='allowedChanged(this)' " + (isAllowed ? "checked" : "")   + " data-id='"  + ai.Id.ToString() + "' />",
                                      strStatus
                                };

                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      ai.Name
                                };

                    jd.data.Add(names1);
                }
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the allowed.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult UpdateAllowed()
        {
            string allowedJson = Request.Unvalidated["allowed"];

            if (!string.IsNullOrEmpty(allowedJson))
            {
                dynamic o = JsonConvert.DeserializeObject(allowedJson);

                for (int i = 0; i < o.Count; i++)
                {
                    _buyerChannelService.UpdateAllowed(long.Parse(Request["aid"]), long.Parse(o[i]["bid"].ToString()), bool.Parse(o[i]["add"].ToString()));
                }
            }
            else
            {
                _buyerChannelService.UpdateAllowed(long.Parse(Request["aid"]), long.Parse(Request["bid"]), bool.Parse(Request["add"]));
            }

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the campaign temporary.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetCampaignTemp()
        {
            List<BuyerChannel> buyerChannels = (List<BuyerChannel>)this._buyerChannelService.GetAllBuyerChannels();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = buyerChannels.Count;
            jd.recordsFiltered = buyerChannels.Count;
            foreach (BuyerChannel ai in buyerChannels)
            {
                Campaign campaign = _campaignService.GetCampaignById(ai.CampaignId);

                string strStatus = "";

                switch (ai.Status)
                {
                    case BuyerChannelStatuses.Inactive: strStatus = "<span style='color: red'>Inactive</span>"; break;
                    case BuyerChannelStatuses.Active: strStatus = "<span style='color: green'>Active</span>"; break;
                    case BuyerChannelStatuses.Paused: strStatus = "<span style='color: orange'>Paused</span>"; break;
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<a href=\"/Management/BuyerChannel/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>",
                                      campaign == null ? "" : campaign.Name,
                                      strStatus
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get allowed affiliate channel list
        /// </summary>
        /// <returns>Json result</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerChannelTemplateMatchings()
        {
            long id = 0;

            long.TryParse(Request["id"], out id);

            List<BuyerChannelTemplateMatching> list = (List<BuyerChannelTemplateMatching>)_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(id);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = list.Count;
            jd.recordsFiltered = list.Count;
            foreach (BuyerChannelTemplateMatching ai in list)
            {
                string[] names1 = {
                                      "<input type='text' class='fixed-col' value='" + ai.InputValue + "'/>",
                                      "<input type='text' class='fixed-col' value='" + ai.OutputValue + "'/>",
                                      "<a href='javascript:void()'>Delete</a>"
                                };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the buyer channel information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerChannelInfo()
        {
            long id = 0;

            long.TryParse(Request["id"], out id);

            BuyerChannel bc = (BuyerChannel)_buyerChannelService.GetBuyerChannelById(id);

            List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem> items = new List<Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem parent = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
            parent.BuyerChannelTemplateId = 0;
            parent.title = "root";
            parent.folder = true;
            parent.expanded = true;
            parent.TemplateField = "";
            parent.DefaultValue = "";

            if (bc != null)
            {
                //XmlDocument xmldoc = new XmlDocument();
                //xmldoc.LoadXml(buyerChannel.XmlTemplate);

                List<BuyerChannelTemplate> templates = (List<BuyerChannelTemplate>)this._buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(id);

                var list = templates.Where(x => x.SectionName == "root").ToList();

                foreach (BuyerChannelTemplate ct in list)
                {
                    Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem ti = new Adrack.Web.ContentManagement.Models.Lead.BuyerChannelModel.TreeItem();
                    ti.BuyerChannelTemplateId = ct.Id;
                    ti.title = ct.TemplateField;
                    ti.folder = true;
                    ti.expanded = true;
                    ti.DefaultValue = string.IsNullOrEmpty(ct.DefaultValue) ? "" : ct.DefaultValue;

                    List<BuyerChannelTemplateMatching> matchings = (List<BuyerChannelTemplateMatching>)_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(ct.Id);

                    string matchingsJson = "[";

                    foreach (BuyerChannelTemplateMatching m in matchings)
                    {
                        matchingsJson += "{";

                        matchingsJson += "\"input\": \"" + m.InputValue + "\",";
                        matchingsJson += "\"output\": \"" + m.OutputValue + "\"";

                        matchingsJson += "},";
                    }

                    if (matchingsJson != "[")
                        matchingsJson = matchingsJson.Remove(matchingsJson.Length - 1);

                    matchingsJson += "]";

                    ti.Matchings = matchingsJson;

                    ti.CampaignTemplateId = ct.CampaignTemplateId;

                    LoadChildren(bc, ct.TemplateField, ti.children);

                    //fix from Arman:buyer channel folder condition
                    if (ti.children == null || ti.children.Count == 0)
                        ti.folder = false;

                    parent.children.Add(ti);
                }
            }

            items.Add(parent);

            return Json(new
            {
                PostingUrl = bc.PostingUrl,
                PostingHeaders = bc.PostingHeaders,
                AcceptedField = bc.AcceptedField,
                AcceptedValue = bc.AcceptedValue,
                AcceptedFrom = bc.AcceptedFrom,
                ErrorField = bc.ErrorField,
                ErrorValue = bc.ErrorValue,
                ErrorFrom = bc.ErrorFrom,
                RejectedField = bc.RejectedField,
                RejectedValue = bc.RejectedValue,
                RejectedFrom = bc.RejectedFrom,
                TestField = bc.TestField,
                TestValue = bc.TestValue,
                TestFrom = bc.TestFrom,
                RedirectField = bc.RedirectField,
                MessageField = bc.MessageField,
                PriceField = bc.PriceField,
                DataFormat = bc.DataFormat,
                ResponseFormat = (bc.ResponseFormat.HasValue ? bc.ResponseFormat.Value : 0),
                WinResponseUrl = bc.WinResponseUrl,
                PriceRejectWinResponse = bc.PriceRejectWinResponse,
                WinResponsePostMethod = bc.WinResponsePostMethod,
                LeadIdField = bc.LeadIdField,
                AccountIdField = bc.AccountIdField,
                PriceRejectField = bc.PriceRejectField,
                PriceRejectValue = bc.PriceRejectValue,
                Delimeter = bc.Delimeter,
                EnableCustomPriceReject = (bc.EnableCustomPriceReject.HasValue ? bc.EnableCustomPriceReject.Value : false),
                FieldAppendEnabled = (bc.FieldAppendEnabled.HasValue ? bc.FieldAppendEnabled.Value : false),
                items = items[0]
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the buyer channel template matching.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult AddBuyerChannelTemplateMatching()
        {
            BuyerChannelTemplateMatching bcm = new BuyerChannelTemplateMatching();

            bcm.BuyerChannelTemplateId = long.Parse(Request["templateid"]);

            bcm.InputValue = Request["inputValue"];
            bcm.OutputValue = Request["outputValue"];

            _buyerChannelTemplateMatchingService.InsertBuyerChannelTemplateMatching(bcm);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the buyer channel template matching.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult DeleteBuyerChannelTemplateMatching()
        {
            BuyerChannelTemplateMatching bcm = _buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingById(long.Parse(Request["id"]));

            _buyerChannelTemplateMatchingService.DeleteBuyerChannelTemplateMatching(bcm);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the buyer channel template matching.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult UpdateBuyerChannelTemplateMatching()
        {
            BuyerChannelTemplateMatching bcm = _buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingById(long.Parse(Request["id"]));

            if (short.Parse(Request["type"]) == 1)
                bcm.InputValue = Request["value"];
            else
                bcm.OutputValue = Request["value"];

            _buyerChannelTemplateMatchingService.UpdateBuyerChannelTemplateMatching(bcm);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Clone()
        {
            long id = 0;
            long.TryParse(Request["id"], out id);

            return Json(new { id = _buyerChannelService.Clone(id, Request["name"]) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the buyer channel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteBuyerChannel()
        {
            long buyerchannelid = 0;

            if (long.TryParse(Request["BuyerChannelId"], out buyerchannelid))
            {
                BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerchannelid);
                if (buyerChannel != null)
                {
                    if (buyerChannel.Deleted.HasValue)
                        buyerChannel.Deleted = !buyerChannel.Deleted.Value;
                    else
                        buyerChannel.Deleted = true;

                    _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                    this._historyService.AddHistory("BuyerChannelController", HistoryAction.Buyer_Channel_Deleted, "Buyer channel", buyerChannel.Id, "Name:" + buyerChannel.Name, "", "", this._appContext.AppUser.Id);
                }
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}