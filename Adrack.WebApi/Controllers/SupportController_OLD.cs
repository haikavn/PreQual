using Adrack.Core.Domain.Membership;
using Adrack.Service.Helpers;
using Adrack.WebApi.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Adrack.Core;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Content;
using Adrack.Service.Message;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/old_support/tickets")]
    public class SupportController_OLD : BaseApiController
    {
        #region fields

        private readonly IAppContext _appContext;

        private readonly IAffiliateService _affiliateService;

        private readonly IBuyerService _buyerService;

        private readonly IEmailService _emailService;

        private readonly IFileUploadService _fileUploadService;

        private readonly ISupportPageService _supportPageService;

        private readonly IUsersExtensionService _usersExtensionService;

        private readonly IUserService _userService;

        #endregion

        #region constructors

        public SupportController_OLD(IAppContext appContext,
            IAffiliateService affiliateService,
            IBuyerService buyerService,
            IEmailService emailService,
            IFileUploadService fileUploadService,
            ISupportPageService supportPageService,
            IUsersExtensionService usersExtensionService,
            IUserService userService
        )
        {
            _appContext = appContext;
            _affiliateService = affiliateService;
            _buyerService = buyerService;
            _emailService = emailService;
            _fileUploadService = fileUploadService;
            _supportPageService = supportPageService;
            _usersExtensionService = usersExtensionService;
            _userService = userService;
        }

        #endregion

        #region methods

        #region route methods

        // GET: api/<controller>/tickets
        [Route("")]
        public TicketsModel GetTickets()
        {
            var ticketsModel = GetTicketsByUser();
            return ticketsModel;
        }

        // GET: api/<controller>/tickets/status/0
        [Route("status/{status}")]
        public TicketsModel GetTicketsByStatus([FromUri]int status)
        {
            var ticketsModel = GetTicketsByUser(status: status);
            return ticketsModel;
        }

        // GET: api/<controller>/tickets/user?buyerId=5
        [Route("user")]
        public TicketsUsersInfoModel GetTicketsUserInfo([FromUri]long? buyerId = null)
        {
            var ticketsUserInfoModel = new TicketsUsersInfoModel
            {
                SelectedBuyerId = buyerId ?? 0
            };

            IList<User> usersList = this._userService.GetSuperUsers();

            if (usersList != null)
            {
                foreach (var user in usersList)
                {
                    if (user.UserType != SharedData.NetowrkUserTypeId) continue;
                    ticketsUserInfoModel.UsersNameList.Add(
                        new KeyValuePair<string, long>($"{_usersExtensionService.GetFullName(user)} ({user.Username})", user.Id));
                }
            }

            if (this._appContext.AppUser != null)
            {
                long? managerId = null;
                var parentId = this._appContext.AppUser.ParentId;

                if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId) // Buyer
                {
                    ticketsUserInfoModel.BuyerId = parentId;
                    var buyer = this._buyerService.GetBuyerById(parentId);
                    managerId = buyer?.ManagerId;
                }
                else if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId) // Affiliate
                {
                    ticketsUserInfoModel.AffiliateId = parentId;
                    var affiliate = this._affiliateService.GetAffiliateById(parentId, true);
                    managerId = affiliate?.ManagerId;
                }

                if (managerId != null)
                {
                    var managerUser = this._userService.GetUserById((long)managerId);
                    ticketsUserInfoModel.ManagerUser = new UserSimpleModel()
                    {
                        Id = managerUser?.Id ?? (long)managerId,
                        FullName = managerUser != null ? _usersExtensionService.GetFullName(managerUser) : null
                    };
                }
            }

            return ticketsUserInfoModel;
        }

        // GET: api/<controller>/tickets/user/5
        [Route("user/{id}")]
        public TicketsModel GetTicketsByUserId([FromUri] long id)
        {
            var ticketsModel = GetTicketsByUser(id);
            return ticketsModel;
        }

        // GET: api/<controller>/ticket/user/5/status/0
        [Route("user/{id}/status/{status}")]
        public TicketsModel GetTicketsByUserAndStatus([FromUri] long id, [FromUri] int status)
        {
            var ticketsModel = GetTicketsByUser(id, status);
            return ticketsModel;
        }

        // POST: api/<controller>/tickets/add
        [Route("item")]
        public void Post([FromBody]TicketAddModel ticketAddModel)
        {
            if (!long.TryParse(ticketAddModel.ManagerId, out var managerId))
            {
                managerId = 1;
                if (short.TryParse(ticketAddModel.SendTo, out var sendTo) && sendTo == 2)
                {
                    var parentId = _appContext.AppUser.ParentId;
                    if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                    {
                        Affiliate affiliate = _affiliateService.GetAffiliateById(parentId, true);
                        managerId = affiliate?.ManagerId ?? 1;
                    }
                    else if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    {
                        Buyer buyer = _buyerService.GetBuyerById(parentId);
                        managerId = buyer?.ManagerId ?? 1;
                    }
                }
            }

            var supportTickets = new SupportTickets
            {
                DateTime = DateTime.Now,
                ManagerID = managerId,
                Message = ticketAddModel.Message,
                Priority = int.TryParse(ticketAddModel.Priority, out var priority) ? priority : (int)TicketPriority.Normal, 
                Status = 1,
                Subject = ticketAddModel.Subject,
                UserID = this._appContext.AppUser?.Id ?? 0
            };

            long retValId = this._supportPageService.AddNewSupportTickets(supportTickets, ticketAddModel.Cc);

            var supportTicketsMessages = new SupportTicketsMessages
            {
                AuthorID = this._appContext.AppUser?.Id ?? 0,
                DateTime = DateTime.Now,
                FilePath = string.Empty,
                IsNew = true,
                Message = supportTickets.Message,
                TicketID = retValId
            };

            //TODO #region fileUpload should be set fileName
            supportTicketsMessages.FilePath = _fileUploadService.UploadFile(supportTicketsMessages.TicketID);

            var retVal = this._supportPageService.InsertSupportTicketsMessage(supportTicketsMessages);

            _emailService.SendUserNewTicket(
                this._userService.GetUserById(supportTickets.ManagerID),
                this._appContext.AppLanguage.Id,
                EmailOperatorEnums.LeadNative);
        }

        // GET: api/<controller>/tickets/5
        [Route("{id}")]
        public TicketsModel Get([FromUri] long id)
        {
            var ticketMessageViewItems = this._supportPageService.GetTicketsByTicketId(id);

            TicketsModel tickets = new TicketsModel();

            if (this._appContext.AppUser == null)
            {
                return tickets;
            }

            if (ticketMessageViewItems == null)
            {
                return tickets;
            }

            tickets.TicketMessageViewItems = ticketMessageViewItems;

            return tickets;
        }

        // POST: api/<controller>/tickets/5/closed
        [Route("{id}/close")]
        public void PostTicketsStatusClosed([FromUri]long id)
        {
            SupportTickets supportTickets = _supportPageService.ChangeTicketsStatus(id, (int)TicketStatus.Closed);

            SendUserNewTicketMessage(supportTickets, this._appContext.AppUser?.Id ?? 0);

        }

        // GET: api/<controller>/tickets/5/messages
        [Route("{id}/messages")]
        public TicketsModel GetSupportTicketsMessages([FromUri]long id)
        {
            var ticketsMessagesViews = this._supportPageService.GetSupportTicketsMessages(id);

            var tickets = new TicketsModel();
            if (ticketsMessagesViews == null)
            {
                return tickets;
            }

            tickets.TicketMessageViewItems = ticketsMessagesViews;

            return tickets;
        }

        // POST: api/<controller>/tickets/5/messages/add
        [Route("{id}/messages/item")]
        public void PostTicketMessage([FromUri]long id, [FromBody]TicketMessagesModel supportTicketMessagesModel)
        {
            if (supportTicketMessagesModel == null)
            {
                throw new ArgumentException(nameof(supportTicketMessagesModel), 
                    $"Support ticket messages model is missing" );
            }

            var supportTicketsMessages = new SupportTicketsMessages
            {
                AuthorID = this._appContext.AppUser?.Id ?? 0,
                DateTime = DateTime.Now,
                IsNew = true,
                Message = supportTicketMessagesModel.Message,
                TicketID = id,
                FilePath = supportTicketMessagesModel.FilePath ?? string.Empty,
            };

            //TODO #region fileupload should be set fileName
            supportTicketsMessages.FilePath = _fileUploadService.UploadFile(supportTicketsMessages.TicketID);

            var retVal = this._supportPageService.InsertSupportTicketsMessage(supportTicketsMessages);
            var supportTickets = this._supportPageService.GetSupportTicketById(supportTicketsMessages.TicketID);

            SendUserNewTicketMessage(supportTickets, supportTicketsMessages.AuthorID);
        }

        #endregion route methods

        #region private methods

        private void SendUserNewTicketMessage(SupportTickets supportTickets, long authorId)
        {
            if (supportTickets != null && supportTickets.ManagerID != authorId) // If Not Self Message
            {
                _emailService.SendUserNewTicketMessage(this._userService.GetUserById(supportTickets.ManagerID),
                    _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative);
            }
        }

        private TicketsModel GetTicketsByUser(long userId = 0L, int? status = null)
        {
            var ticketsModel = new TicketsModel();
            if (this._appContext.AppUser == null)
            {
                return ticketsModel;
            }

            ticketsModel.SetInstanceValues();

            var userIds = GetUserIds(ref userId);

            ticketsModel.TicketMessageViewItems = _supportPageService.GetTicketsByUserId(userId, status, userIds);

            return ticketsModel;
        }

        private string GetUserIds(ref long userId)
        {
            string userIds = null;
            if (this._appContext.AppUser.UserType != SharedData.BuiltInUserTypeId)
            {
                userIds = string.Empty;
                if (_appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    bool found = false;
                    List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(_appContext.AppUser, 0);
                    List<User> users;
                    foreach (Buyer buyer in buyers)
                    {
                        users = (List<User>)_userService.GetUsersByParentId(buyer.Id, SharedData.BuyerUserTypeId);
                        if (users != null)
                        {
                            foreach (User user in users)
                            {
                                if (user.Id == userId)
                                    found = true;
                                userIds += $"{user.Id.ToString()},";
                            }
                        }
                    }

                    List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(_appContext.AppUser, 0);
                    foreach (Affiliate affiliate in affiliates)
                    {
                        users = (List<User>)_userService.GetUsersByParentId(affiliate.Id, SharedData.AffiliateUserTypeId);
                        foreach (User user in users)
                        {
                            if (user.Id == userId)
                                found = true;
                            userIds += $"{user.Id.ToString()},";
                        }
                    }

                    if (!found)
                        userId = this._appContext.AppUser.Id;

                    if (userIds.Length > 0)
                        userIds = userIds.Remove(userIds.Length - 1);
                }
                else
                    userId = this._appContext.AppUser.Id;
            }

            return userIds;
        }

        #endregion private methods

        #endregion methods
    }
}
