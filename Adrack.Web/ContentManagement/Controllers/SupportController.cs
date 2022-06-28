// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SupportController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Web.ContentManagement.Models.Support;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class SupportController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class SupportController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly ISupportTicketsService _supportTicketsService;

        /// <summary>
        /// The support tickets messages service
        /// </summary>
        private readonly ISupportTicketsMessagesService _supportTicketsMessagesService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

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
        /// Email Service
        /// </summary>
        private readonly IEmailService _emailService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="supportTicketsService">The support tickets service.</param>
        /// <param name="supportTicketsMessagesService">The support tickets messages service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="userService">The user service.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="_buyerService">The buyer service.</param>

        public SupportController(
                                ISupportTicketsService supportTicketsService,
                                ISupportTicketsMessagesService supportTicketsMessagesService,
                                IAppContext appContext,
                                IUserService userService,
                                IEmailService emailService,
                                IAffiliateService affiliateService,
                                IBuyerService _buyerService)
        {
            this._supportTicketsService = supportTicketsService;
            this._supportTicketsMessagesService = supportTicketsMessagesService;
            this._appContext = appContext;
            this._userService = userService;
            this._emailService = emailService;
            this._affiliateService = affiliateService;
            this._buyerService = _buyerService;
        }

        #endregion Constructor

        // GET: Support
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Support")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ticketses this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Tickets")]
        public ActionResult Tickets()
        {
            if (Request["buyerid"] != null)
            {
                ViewBag.SelectedBuyerId = Request["buyerid"];
            }
            else if (Request["affiliateid"] != null)
            {
                ViewBag.SelectedAffiliateId = Request["affiliateid"];
            }

            SupportTicketsModel stModel = new SupportTicketsModel();

            IList<User> UsersList = this._userService.GetSuperUsers();

            stModel.UsersNameList = new List<KeyValuePair<string, long>>();
            foreach (User u in UsersList)
            {
                if (u.UserType != SharedData.NetowrkUserTypeId) continue;
                stModel.UsersNameList.Add(new KeyValuePair<string, long>(u.GetFullName() + " (" + u.Username + ")", u.Id));
            }

            ViewBag.AffiliateId = 0;
            ViewBag.BuyerId = 0;

            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId) // Buyer
            {
                ViewBag.BuyerId = this._appContext.AppUser.ParentId;
                Buyer b = this._buyerService.GetBuyerById(this._appContext.AppUser.ParentId);
                if (b.ManagerId != null)
                {
                    User ManagerUser = this._userService.GetUserById((long)b.ManagerId);
                    ViewBag.ManagerUser = ManagerUser;
                    ViewBag.ManagerUserName = ManagerUser.GetFullName();
                }
            }
            else if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId) //Affiliate
            {
                ViewBag.AffiliateId = this._appContext.AppUser.ParentId;

                Affiliate a = this._affiliateService.GetAffiliateById(this._appContext.AppUser.ParentId, true);
                if (a.ManagerId != null)
                {
                    User ManagerUser = this._userService.GetUserById((long)a.ManagerId);
                    ViewBag.ManagerUser = ManagerUser;
                    ViewBag.ManagerUserName = ManagerUser.GetFullName();
                }
            }

            return View(stModel);
        }

        /// <summary>
        /// Ticketses the partial.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult TicketsPartial(long Id, string type)
        {
            SupportTicketsModel stModel = new SupportTicketsModel();

            IList<User> UsersList = this._userService.GetSuperUsers();

            stModel.UsersNameList = new List<KeyValuePair<string, long>>();
            foreach (User u in UsersList)
            {
                stModel.UsersNameList.Add(new KeyValuePair<string, long>(u.GetFullName() + " (" + u.Username + ")", u.Id));
            }

            ViewBag.AffiliateId = 0;
            ViewBag.BuyerId = 0;
            if (type == "a")
            {
                ViewBag.AffiliateId = Id;
            }
            else if (type == "b")
            {
                ViewBag.BuyerId = Id;
                Buyer b = this._buyerService.GetBuyerById(Id);
                if (b.ManagerId != null)
                {
                    User ManagerUser = this._userService.GetUserById((long)b.ManagerId);
                    ViewBag.ManagerUser = ManagerUser;
                    ViewBag.ManagerUserName = ManagerUser.GetFullName();
                }
            }

            return View(stModel);
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Items")]
        public ActionResult Item(long id)
        {
            SupportTickets st = this._supportTicketsService.GetSupportTicketById(id);

            SupportTicketsMessagesModel sm = new SupportTicketsMessagesModel();

            if (st != null)
            {
                this._supportTicketsService.SetTicketMessagesRead(id);

                sm.TicketSubject = st.Subject;
                sm.TicketID = id;
                sm.AuthorID = this._appContext.AppUser.Id;
                sm.TicketStatus = st.Status;

                switch (st.Status)
                {
                    case 0: { ViewBag.Status = "Closed"; break; }
                    case 1: { ViewBag.Status = "Open"; break; }
                }


                User user1 = this._userService.GetUserById(st.UserID);
                User user2 = this._userService.GetUserById(st.ManagerID);

                ViewBag.Reporter = user1.GetFullName();
                ViewBag.Assigne = user2.GetFullName();

                switch (st.Priority)
                {
                    case 1: { ViewBag.Priority = "Low"; break; }
                    case 2: { ViewBag.Priority = "Normal"; break; }
                    case 3: { ViewBag.Priority = "High"; break; }
                }
            }

            return View(sm);
        }

        /// <summary>
        /// Gets the support tickets.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetSupportTickets()
        {
            JsonData jd = new JsonData();

            if (this._appContext.AppUser == null)
                return Json(jd, JsonRequestBehavior.AllowGet);

            List<SupportTicketsView> Tickets = new List<SupportTicketsView>();

            long userId = 0;
            long.TryParse(Request["userid"], out userId);

            if (this._appContext.AppUser.UserType == SharedData.BuiltInUserTypeId)
                Tickets = (List<SupportTicketsView>)this._supportTicketsService.GetSupportTickets(userId);
            else
            {
                string userIds = "";
                if (_appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    bool found = false;
                    List<User> users = null;
                    List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(_appContext.AppUser, 0);
                    foreach (Buyer b in buyers)
                    {
                        users = (List<User>)_userService.GetUsersByParentId(b.Id, SharedData.BuyerUserTypeId);
                        foreach (User u in users)
                        {
                            if (u.Id == userId)
                                found = true;
                            userIds += u.Id.ToString() + ",";
                        }
                    }

                    List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(_appContext.AppUser, 0);
                    foreach (Affiliate a in affiliates)
                    {
                        users = (List<User>)_userService.GetUsersByParentId(a.Id, SharedData.AffiliateUserTypeId);
                        foreach (User u in users)
                        {
                            if (u.Id == userId)
                                found = true;
                            userIds += u.Id.ToString() + ",";
                        }
                    }

                    if (!found)
                        userId = this._appContext.AppUser.Id;

                    if (userIds.Length > 0)
                        userIds = userIds.Remove(userIds.Length - 1);
                }
                else
                    userId = this._appContext.AppUser.Id;

                Tickets = (List<SupportTicketsView>)this._supportTicketsService.GetSupportTickets(userId, userIds);
            }

            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;

            int tt = 0;
            int.TryParse(Request["t"], out tt);


            foreach (SupportTicketsView st in Tickets)
            {
                if(st.Status== tt)
                {
                    continue;
                }
                string priorityStr = "";
                switch (st.Priority)
                {
                    case 1: { priorityStr = "<span class='text-info'> Low </span>"; break; }
                    case 2: { priorityStr = "<span class='text-success'> Normal </span>"; break; }
                    case 3: { priorityStr = "<span class='text-danger'> High </span>"; break; }
                }

                string newCountStr = "";
                if (st.NewCount > 0)
                {
                    newCountStr = " <span class='badge bg-warning-400'>" + st.NewCount.ToString() + "</span>";
                }

                User user1 = this._userService.GetUserById(st.UserID);
                User user2 = this._userService.GetUserById(st.ManagerID);

                /*
                                1/4 -Admin
                                3-Affili
                                2-Buyer
                */
                string companyName = "";

                if (user1 != null)
                {
                    switch (user1.UserType)
                    {
                        case UserTypes.Network: // Network User
                            {
                                companyName = "Network User";
                                break;
                            }
                        case UserTypes.Affiliate: // Affiliate
                            {
                                companyName = this._affiliateService.GetAffiliateById(user1.ParentId, true).Name;
                                break;
                            }
                        case UserTypes.Buyer: // Buyer
                            {
                                companyName = this._buyerService.GetBuyerById(user1.ParentId).Name;
                                break;
                            }
                    }
                }

                string User1Name = "";

                if (user1 != null)
                {
                    User1Name = companyName + "<br>" + user1.GetFullName();
                }

                string[] names1 = {
                                      "<a href=\"/Management/Support/Item/"+ st.Id.ToString() +"\">" + st.Id.ToString() + "</a>",
                                      "<h6 class=\"no-margin text-bold\"><a href=\"/Management/Support/Item/" + st.Id.ToString() + "\">" + st.Subject + newCountStr + " </a></h6>",
                                      User1Name,
                                      user2 != null ? user2.GetFullName() : "No manager",
                                      priorityStr,
                                      st.Status==1 ? "<span class='label label-danger'> Open </span>" : "<span class='label label-success'>Closed</span>",
                                      st.DateTime.ToString()
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the ticket.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddTicket()
        {
            SupportTickets st = new SupportTickets();
            st.DateTime = DateTime.Now;
            long managerId = 0;
            if (long.TryParse(Request["managerid"], out managerId))
                st.ManagerID = managerId;
            else
            {
                short sendTo = 2;
                short.TryParse(Request["sendto"], out sendTo);
                if (sendTo == 2)
                {
                    if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                    {
                        Affiliate affiliate = _affiliateService.GetAffiliateById(_appContext.AppUser.ParentId, true);
                        st.ManagerID = (affiliate == null ? 1 : (affiliate.ManagerId.HasValue ? affiliate.ManagerId.Value : 1));
                    }
                    else if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    {
                        Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
                        st.ManagerID = (buyer == null ? 1 : (buyer.ManagerId.HasValue ? buyer.ManagerId.Value : 1));
                    }
                    else
                        st.ManagerID = 1;
                }
                else
                {
                    st.ManagerID = 1;
                }
            }
            st.Message = Request["message"];
            st.Priority = int.Parse(Request["priority"]);
            st.Status = 1;
            st.Subject = Request["subject"];
            st.UserID = this._appContext.AppUser.Id;

            long retValId = this._supportTicketsService.InsertSupportTicket(st);

            if (!string.IsNullOrEmpty(Request["cc"]))
            {
                string[] ccString = Request["cc"].Split(',');

                foreach (string s in ccString)
                {
                    if (s != null && s != "null")
                    {
                        this._supportTicketsService.AddSupportTicketUser(retValId, long.Parse(s));
                    }
                }
            }

            if (retValId < 0)
            {
                return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
            }

            SupportTicketsMessages stm = new SupportTicketsMessages();
            stm.AuthorID = this._appContext.AppUser.Id;
            stm.DateTime = DateTime.Now;
            stm.FilePath = "";
            stm.IsNew = true;
            stm.Message = Request["message"];
            stm.TicketID = retValId;

            if (Request.Files.Count > 0)
            {
                Random rand = new Random();

                HttpPostedFileBase file = Request.Files[0];
                var fileName = stm.TicketID + "-" + rand.Next(0, 1000).ToString() + "-" + Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                file.SaveAs(path);

                stm.FilePath = fileName;
            }


            long retVal = this._supportTicketsMessagesService.InsertSupportTicketsMessage(stm);

            _emailService.SendUserNewTicket(this._userService.GetUserById(st.ManagerID), _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative);

            return Json(new { id = retVal }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MESSAGING
        /// </summary>
        /// <returns>System.String.</returns>
        [ContentManagementAntiForgery(true)]
        public string GetSupportTicketsMessages()
        {
            long ticketid = long.Parse(Request["ticketid"]);

            List<SupportTicketsMessagesView> TicketsMess = (List<SupportTicketsMessagesView>)this._supportTicketsMessagesService.GetSupportTicketsMessages(ticketid);
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            string retStr = "";

            foreach (SupportTicketsMessagesView st in TicketsMess)
            {
                string attachedFile = "";

                if (st.FilePath != null && st.FilePath != "")
                {
                    attachedFile = "<a download href='" + "/Uploads/" + st.FilePath + "' target='blank'> <i class='icon-attachment'></i> Download Attached File </a>";
                }

                if (st.AuthorID != this._appContext.AppUser.Id)
                {
                    retStr += @"
                    <li class='media'>
	                    <div class='media-left'>
		                    <a href='#'>
			                    <img src='/ContentManagement/Uploads/user.png' class='img-circle' alt=''>
		                    </a>
		                    <b>" + this._userService.GetUserById(st.AuthorID).GetFullName() + @"</b>
	                    </div>

	                    <div class='media-body'>
		                    <div class='media-content'>" + attachedFile + st.Message + @"</div>
		                    <span class='media-annotation display-block mt-10'>" + st.DateTime.ToString() + @"</span>
	                    </div>
                    </li>";
                }
                else
                {
                    retStr += @"
                    <li class='media reversed'>
                        <div class='media-body'>
                            <div class='media-content'>" + attachedFile + st.Message + @"</div>
                            <span class='media-annotation display-block mt-10'>" + st.DateTime.ToString() + @"</span>
                        </div>

                        <div class='media-right'>
                            <a href='#'>
                                <img src='/ContentManagement/Uploads/user.png' class='img-circle' alt=''>
                            </a>
                            <b>" + this._userService.GetUserById(st.AuthorID).GetFullName() + @"</b>
                        </div>
                    </li>
";
                }
            }

            return retStr;
        }

        /// <summary>
        /// Adds the tickets messages.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddTicketsMessages()
        {
            SupportTicketsMessages stm = new SupportTicketsMessages();
            stm.AuthorID = (this._appContext.AppUser != null ? this._appContext.AppUser.Id : 0);
            stm.DateTime = DateTime.Now;

            stm.IsNew = true;
            stm.Message = Request["message"];
            stm.TicketID = long.Parse(Request["ticketid"]);
            stm.FilePath = "";

            if (Request.Files.Count > 0)
            {
                Random rand = new Random();

                HttpPostedFileBase file = Request.Files[0];
                var fileName = stm.TicketID + "-" + rand.Next(0, 1000).ToString() + "-" + Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                file.SaveAs(path);

                stm.FilePath = fileName;
            }

            long retVal = this._supportTicketsMessagesService.InsertSupportTicketsMessage(stm);

            SupportTickets st = this._supportTicketsService.GetSupportTicketById(stm.TicketID);
            if (st != null && st.ManagerID != stm.AuthorID) // If Not Self Message
            {
                _emailService.SendUserNewTicketMessage(this._userService.GetUserById(st.ManagerID), _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative);
            }

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Changes the tickets status.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult ChangeTicketsStatus()
        {
            long retVal = this._supportTicketsService.ChangeTicketsStatus(long.Parse(Request["ticketid"]), int.Parse(Request["status"]));
            SupportTickets st = this._supportTicketsService.GetSupportTicketById(long.Parse(Request["ticketid"]));

            if (st != null)
            {
                long authorID = (this._appContext.AppUser != null ? this._appContext.AppUser.Id : 0);

                if (st != null && st.ManagerID != authorID) // If Not Self Message
                {
                    _emailService.SendUserNewTicketMessage(this._userService.GetUserById(st.ManagerID), _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative);
                }
            }

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }
    }
}