// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="HistoryController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class HistoryController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class HistoryController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryController"/> class.
        /// </summary>
        /// <param name="settingService">The setting service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="_buyerService">The buyer service.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        public HistoryController(ISettingService settingService,
                                    IHistoryService historyService,
                                    IUserService userService,
                                    IAffiliateService affiliateService,
                                    IBuyerService _buyerService,
                                    IAppContext appContext,
                                    IAffiliateChannelService affiliateChannelService,
                                    IBuyerChannelService buyerChannelService,
                                    IPermissionService permissionService)
        {
            this._settingService = settingService;
            this._historyService = historyService;
            this._userService = userService;
            this._affiliateService = affiliateService;
            this._buyerService = _buyerService;
            this._appContext = appContext;
            this._affiliateChannelService = affiliateChannelService;
            this._buyerChannelService = buyerChannelService;
            this._permissionService = permissionService;
        }

        #endregion Constructor

        // GET: History
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / History")]
        public ActionResult Index()
        {
            if (this._appContext.AppUser != null && (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId))
            {
                return null;
            }

            IPagination<User> uList = this._userService.GetAllUsers(0);
            List<KeyValuePair<long, string>> UsersIdName = new List<KeyValuePair<long, string>>();

            if (this._appContext.AppUser.UserType == SharedData.NetowrkUserTypeId || this._appContext.AppUser.UserType == SharedData.BuiltInUserTypeId)
            {
                uList.Clear();

                UsersIdName.Add(new KeyValuePair<long, string>(this._appContext.AppUser.Id, this._appContext.AppUser.GetFullName()));

                List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(this._appContext.AppUser, 0);

                foreach (Affiliate aff in affiliates)
                {
                    User user = _userService.GetUserByParentId(aff.Id, SharedData.AffiliateUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }

                List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(this._appContext.AppUser, 0);

                foreach (Buyer buyer in buyers)
                {
                    User user = _userService.GetUserByParentId(buyer.Id, SharedData.BuyerUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }
            }

            foreach (User u in uList)
            {
                UsersIdName.Add(new KeyValuePair<long, string>(u.Id, u.GetFullName()));
            }
            ViewBag.UsersList = UsersIdName;
            return View();
        }

        /// <summary>
        /// Indexes the partial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult IndexPartial(long id, string entity)
        {
            IPagination<User> uList = this._userService.GetAllUsers();

            List<KeyValuePair<long, string>> UsersIdName = new List<KeyValuePair<long, string>>();

            if (this._appContext.AppUser.UserType == SharedData.NetowrkUserTypeId || this._appContext.AppUser.UserType == SharedData.BuiltInUserTypeId)
            {
                uList.Clear();

                UsersIdName.Add(new KeyValuePair<long, string>(this._appContext.AppUser.Id, this._appContext.AppUser.GetFullName()));

                List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(this._appContext.AppUser, 0);

                foreach (Affiliate aff in affiliates)
                {
                    User user = _userService.GetUserByParentId(aff.Id, SharedData.AffiliateUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }

                List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(this._appContext.AppUser, 0);

                foreach (Buyer buyer in buyers)
                {
                    User user = _userService.GetUserByParentId(buyer.Id, SharedData.BuyerUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }
            }

            foreach (User u in uList)
            {
                UsersIdName.Add(new KeyValuePair<long, string>(u.Id, u.GetFullName()));
            }

            ViewBag.UsersList = UsersIdName;

            ViewBag.Entity = entity;
            ViewBag.EntityId = (entity.Length > 0 ? id : 0);
            ViewBag.UserId = (entity.Length == 0 ? id : 0);

            /*User user = null;

            switch(entity)
            {
                case "Buyer":
                    user = _userService.GetUserByParentId(id, SharedData.BuyerUserTypeId);
                    if (user != null)
                    {
                        ViewBag.UserId = user.Id;
                    }
                    break;

                case "Affiliate":
                    user = _userService.GetUserByParentId(id, SharedData.AffiliateUserTypeId);
                    if (user != null)
                    {
                        ViewBag.UserId = user.Id;
                    }
                    break;
            }*/

            return PartialView();
        }

        /// <summary>
        /// Gets the history ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetHistoryAjax()
        {
            var actionsJson = Request["actions"];
            int page = 1;
            int pageSize = 100;

            if (!int.TryParse(Request["pageSize"], out pageSize)) pageSize = 100;
            if (!int.TryParse(Request["page"], out page)) page = 1;

            DateTime dateFrom = Convert.ToDateTime("2016-01-01"); // Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.Now).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            int action = 0;
            long userid = 0;

            Setting st = _settingService.GetSetting("TimeZone");

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0].Trim() + " 00:00:00");
                dateTo = Convert.ToDateTime(dates[1].Trim() + " 23:59:59");
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["action"] != null)
            {
                action = int.Parse(Request["action"]);
            }

            if (Request["userid"] != null)
            {
                userid = long.Parse(Request["userid"]);
            }

            if (userid != 0 && _appContext.AppUser.UserType != SharedData.BuiltInUserTypeId)
            {
                userid = 0;
            }

            string userIds = "";
            if (_appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
            {
                userid = _appContext.AppUser.Id;
                List<User> users = null;
                List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(_appContext.AppUser, 0);
                foreach (Buyer b in buyers)
                {
                    users = (List<User>)_userService.GetUsersByParentId(b.Id, SharedData.BuyerUserTypeId);
                    foreach (User u in users)
                    {
                        userIds += u.Id.ToString() + ",";
                    }
                }

                List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(_appContext.AppUser, 0);
                foreach (Affiliate a in affiliates)
                {
                    users = (List<User>)_userService.GetUsersByParentId(a.Id, SharedData.AffiliateUserTypeId);
                    foreach (User u in users)
                    {
                        userIds += u.Id.ToString() + ",";
                    }
                }

                if (userIds.Length > 0)
                    userIds = userIds.Remove(userIds.Length - 1);
            }

            if (_permissionService.Authorize(PermissionProvider.BuyerChannelsShowHistory))
                userid = 0;

            string entity = "";
            if (Request["entity"] != null)
            {
                entity = Request["entity"];
            }

            long entityid = 0;
            if (Request["entityid"] != null)
            {
                entityid = long.Parse(Request["entityid"]);
            }

            bool simpleMode = false;

            bool.TryParse(Request["simplemode"], out simpleMode);

            List<History> HistoryList = (List<History>)this._historyService.GetAllHistory(dateFrom, dateTo, action, userid, "", entity, entityid, (page - 1) * pageSize, pageSize);// (dateFrom, dateTo, FilterLeadId, FilterEmail, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, Status, IP, State, page * pageSize, pageSize);

            JsonData jd = new JsonData();
            jd.draw = page;
            jd.recordsTotal = HistoryList.Count();
            jd.recordsFiltered = 5;
            jd.TimeZoneNowStr = ""; // this._settingService.GetTimeZoneDate(DateTime.Now).ToString("yyyy-MM-dd");

            HistoryAction ah;
            string ahStr;
            string UserNameStr = "";
            foreach (History h in HistoryList)
            {
                ah = h.Action != 0 ? (HistoryAction)(int)h.Action : 0;
                ahStr = ah.ToString().Replace('_', ' ');

                UserNameStr = ""; //h.UserID.ToString();

                Core.Domain.Membership.User u = this._userService.GetUserById(h.UserID);
                if (u != null)
                {
                    UserNameStr += u.GetFullName();

                    switch (u.UserType)
                    {
                        case UserTypes.Network: // Network User
                            {
                                UserNameStr += "<br>Network User";
                                break;
                            }
                        case UserTypes.Affiliate: // Affiliate
                            {
                                UserNameStr += "<br>" + this._affiliateService.GetAffiliateById(u.ParentId, true).Name;
                                break;
                            }
                        case UserTypes.Buyer: // Buyer
                            {
                                UserNameStr += "<br>" + this._buyerService.GetBuyerById(u.ParentId).Name;
                                break;
                            }
                    }
                }

                if (!simpleMode)
                {
                    string[] names1 = {
                                    "<a href='#' class='text-center'>" + h.Id.ToString() + "</a>",
                                    "<p class='text-center'>" + h.Date.AddHours(double.Parse((string.IsNullOrEmpty(st.Value) ? "0" : st.Value))).ToString() + "</p>",
                                    "",
                                    "<p class='text-center'>" + ahStr + "</p>",
                                    //"<p class='text-center'>" + "<a href='/management/" + h.Entity + "/item/" + h.EntityID.ToString() + "' target='_blank'>Entity</a>" + "</p>",
                                    "<p class='text-center'>" + h.Note + "</p>",
                                    "<p class='text-center'>" + UserNameStr + "</p>"
                                };
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                    "<a href='#' class='text-center'>" + h.Id.ToString() + "</a>",
                                    "<p class='text-center'>" + h.Date.AddHours(double.Parse((string.IsNullOrEmpty(st.Value) ? "0" : st.Value))).ToString() + "</p>",
                                    "<p class='text-center'>" + ahStr + "</p>",
                                    "<p class='text-center'>" + h.Note + "</p>",
                                    "<p class='text-center'>" + UserNameStr + "</p>"
                                };
                    jd.data.Add(names1);
                }
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the history by buyer identifier ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetHistoryByBuyerIdAjax()
        {
            var actionsJson = Request["actions"];
            int page = 0;

            DateTime dateFrom = Convert.ToDateTime("2016-01-01"); // Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.Now).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            int action = 0;
            long userid = 0;

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["action"] != null)
            {
                action = int.Parse(Request["action"]);
            }

            if (Request["uid"] != null)
            {
                userid = long.Parse(Request["uid"]);
            }

            List<History> HistoryList = new List<History>();

            /*if (Request["utype"] != null && Request["utype"] == "b")
            {
                HistoryList = (List<History>)this._historyService.GetHistoryByBuyerId(dateFrom, dateTo, action, userid, 0, 100);
            }
            else
            {
                HistoryList = (List<History>)this._historyService.GetHistoryByAffiliateId(dateFrom, dateTo, action, userid, 0, 100);
            }*/

            string userIds = "";

            if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId)
                userIds = userid.ToString();
            else
            if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId && userid != _appContext.AppUser.Id)
            {
                userIds = _appContext.AppUser.Id.ToString();
                /*Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
                if (buyer != null)
                {
                    userIds = _appContext.AppUser.Id.ToString();

                    List<User> users = (List<User>)_userService.GetUsersByParentId(_appContext.AppUser.Id, SharedData.BuyerUserTypeId);
                    for (int i = 0; i < users.Count; i++)
                    {
                        userIds += users[i].Id.ToString();
                        if (i < users.Count - 1)
                            userIds += ",";
                    }
                }*/
            }
            else
            {
                userIds = "0";
            }

            HistoryList = (List<History>)this._historyService.GetHistoryByUsers(dateFrom, dateTo, action, userIds, 0, 100);

            JsonData jd = new JsonData();
            jd.draw = page;
            jd.recordsTotal = HistoryList.Count();
            jd.recordsFiltered = 5;
            jd.TimeZoneNowStr = "";

            HistoryAction ah;
            string ahStr;
            string UserNameStr = "";
            foreach (History h in HistoryList)
            {
                ah = h.Action != 0 ? (HistoryAction)(int)h.Action : 0;
                ahStr = ah.ToString().Replace('_', ' ');

                UserNameStr = h.UserID.ToString();

                Core.Domain.Membership.User u = this._userService.GetUserById(h.UserID);
                if (u != null)
                {
                    UserNameStr += "# " + u.GetFullName();

                    switch (u.UserType)
                    {
                        case UserTypes.Network: // Network User
                            {
                                UserNameStr += "<br>Network User";
                                break;
                            }
                        case UserTypes.Affiliate: // Affiliate
                            {
                                UserNameStr += "<br>" + this._affiliateService.GetAffiliateById(u.ParentId, true).Name;
                                break;
                            }
                        case UserTypes.Buyer: // Buyer
                            {
                                Buyer buyer = this._buyerService.GetBuyerById(u.Id);

                                UserNameStr += "<br>" + (buyer != null ? buyer.Name : "Unknown");
                                break;
                            }
                    }
                }

                string[] names1 = {
                                      "<a href='#'>" + h.Id.ToString() + "</a>",
                                      h.Date.ToString(),
                                      h.Module,
                                      ahStr,
                                      //"<a href='/management/" + h.Entity + "/item/" + h.EntityID.ToString() + "' target='_blank'>Entity</a>",
                                      h.Note,
                                      UserNameStr
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the history by identifier ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetHistoryByIdAjax()
        {
            JsonData jd = new JsonData();

            History h = (History)this._historyService.GetHistoryById(long.Parse(Request["id"]));

            HistoryAction ah = h.Action != 0 ? (HistoryAction)(int)h.Action : 0;
            string ahStr = ah.ToString().Replace('_', ' ');

            User user = _userService.GetUserById(h.UserID);

            string username = (user == null ? "" : user.Username);

            string name = "Entity";
            Setting st = _settingService.GetSetting("TimeZone");

            switch (h.Module)
            {
                case "AffiliateController":
                    Affiliate affiliate = _affiliateService.GetAffiliateById(h.EntityID, true);
                    if (affiliate != null)
                        name = affiliate.Name;
                    break;

                case "BuyerController":
                    Buyer buyer = _buyerService.GetBuyerById(h.EntityID);
                    if (buyer != null)
                        name = buyer.Name;
                    break;

                case "AffiliateChannelController":
                    AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(h.EntityID);
                    if (affiliateChannel != null)
                        name = affiliateChannel.Name;
                    break;

                case "BuyerChannelController":
                    BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(h.EntityID);
                    if (buyerChannel != null)
                        name = buyerChannel.Name;
                    break;
            }

            string[] basic = {
                                      "<a href='#'>" + h.Id.ToString() + "</a>",
                                      h.Date.AddHours(int.Parse(st.Value)).ToString(),
                                      ahStr,
                                      h.Module,
                                      "<a href='/management/" + h.Entity + "/item/" + h.EntityID.ToString() + "' target='_blank'>" +  name + " / " + h.EntityID.ToString() + "</a>",
                                      h.Note,
                                      h.UserID.ToString(),
                                      username
                                };

            jd.basicData.Add(basic);

            string[] Data1Arr = null;
            string[] Data2Arr = null;
            string[] DataArr = null;

            if (h.Data1 != null && h.Data1 != "")
            {
                Data1Arr = Regex.Split(h.Data1, ";");
            }
            if (h.Data2 != null && h.Data2 != "")
            {
                Data2Arr = Regex.Split(h.Data2, ";");
            }

            if (Data1Arr == null && Data2Arr == null)
            {
                return Json(jd, JsonRequestBehavior.AllowGet);
            }

            DataArr = Data1Arr != null ? Data1Arr : Data2Arr;

            string[] Data1Str = null;
            string[] Data2Str = null;
            string[] DataStr = null;

            for (int i = 0; i < DataArr.Count(); i++)
            {
                if (DataArr[i] == "")
                {
                    continue;
                }
                Data1Str = Data1Arr != null ? Regex.Split(Data1Arr[i], ":") : null;
                Data2Str = Data2Arr != null ? Regex.Split(Data2Arr[i], ":") : null;

                string d1 = "";
                string d2 = "";

                if (Data1Str != null && Data1Str.Length > 1)
                {
                    d1 = Data1Str[1];
                    DataStr = Data1Str;
                }

                if (Data2Str != null && Data2Str.Length > 1)
                {
                    d2 = Data2Str[1];
                    DataStr = Data2Str;
                }

                if (d1 == d2) continue;

                string[] names1 = {
                                      DataStr[0],
                                      d1,
                                      CompareStr(d1, d2),
                                      d2
                                };

                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Compares the string.
        /// </summary>
        /// <param name="str2">The STR2.</param>
        /// <param name="str1">The STR1.</param>
        /// <returns>System.String.</returns>
        private string CompareStr(string str2, string str1)
        {
            char[] ch1 = str1.ToCharArray();
            char[] ch2 = str2.ToCharArray();

            string color1 = "<span style=\"color: #FF0000\">";
            string colorR = "<span style=\"color: #00F000\">";
            string color2 = "</span>";
            System.Text.StringBuilder strB = new System.Text.StringBuilder();

            int iCount = str1.Length > str2.Length ? str1.Length : str2.Length;
            for (int i = 0; i < iCount; i++)
            {
                if (i < str1.Length && i < str2.Length)
                {
                    if (ch1[i] == ch2[i])
                    {
                        strB.Append(ch1[i]);
                    }
                    else
                    {
                        strB.Append(colorR + ch1[i] + color2);
                    }
                }
                else
                {
                    if (str1.Length >= str2.Length)
                    {
                        strB.Append(colorR + ch1[i] + color2);
                    }
                    else
                    {
                        strB.Append(color1 + ch2[i] + color2);
                    }
                }
            }

            return "<b>" + strB.ToString() + "</b>";
        }
    }
}