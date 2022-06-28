using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.Support;

namespace Adrack.WebApi.Infrastructure.Core.Services
{
    public class SupportPageService : ISupportPageService
    {
        #region fields

        private readonly IAppContext _appContext;

        private readonly IAffiliateService _affiliateService;

        private readonly IBuyerService _buyerService;

        private readonly ISupportTicketsMessagesService _supportTicketsMessagesService;

        private readonly ISupportTicketsService _supportTicketsService;

        private readonly IUserService _userService;
        #endregion

        #region constructor

        public SupportPageService(IAppContext appContext,
            IAffiliateService affiliateService,
            IBuyerService buyerService,
            ISupportTicketsMessagesService supportTicketsMessagesService,
            ISupportTicketsService supportTicketsService,
            IUserService userService
        )
        {
            _appContext = appContext;
            _affiliateService = affiliateService;
            _buyerService = buyerService;
            _supportTicketsMessagesService = supportTicketsMessagesService;
            _supportTicketsService = supportTicketsService;
            _userService = userService;
        }

        #endregion

        #region ISupportPageService interface implementation

        SupportTickets ISupportPageService.ChangeTicketsStatus(long id, int status)
        {
            long retVal = this._supportTicketsService.ChangeTicketsStatus(id, status);

            SupportTickets supportTickets = this._supportTicketsService.GetSupportTicketById(id);

            return supportTickets;
        }

        List<TicketMessageViewItem> ISupportPageService.GetTicketsByUserId(long userId, int? status, string userIds)
        {
            var ticketsViews = (List<SupportTicketsView>)this._supportTicketsService.GetSupportTickets(userId, userIds ?? string.Empty);

            var ticketMessageViewItems = new List<TicketMessageViewItem>();

            foreach (SupportTicketsView supportTicketsView in ticketsViews)
            {
                if (supportTicketsView.Status == (status ?? 0))
                {
                    continue;
                }

                User user1 = this._userService.GetUserById(supportTicketsView.UserID);
                User user2 = this._userService.GetUserById(supportTicketsView.ManagerID);

                /*
                                1/4 -Admin
                                3-Affiliate
                                2-Buyer
                */
                string companyName = string.Empty;

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

                string user1Name = string.Empty;

                if (user1 != null)
                {
                    user1Name = companyName + "<br>" + user1.GetFullName();
                }

                var priority = (TicketPriority)supportTicketsView.Priority;
                var ticketStatus = supportTicketsView.Status == 1 ? TicketStatus.Open : TicketStatus.Closed;

                var ticketMessageModel = new TicketMessageViewItem
                {
                    PriorityString = priority.ToString(),
                    StatusString = ticketStatus.ToString(),
                    TicketId = supportTicketsView.Id,
                    AuthorId = this._appContext.AppUser.Id,
                    Subject = supportTicketsView.Subject,
                    Reporter = user1Name,
                    Assignee = user2 != null ? user2.GetFullName() : "No manager",
                    CreateTime = supportTicketsView.DateTime.ToString(),
                    NewCount = supportTicketsView.NewCount.ToString()
                    //Message = supportTicketsView.Message;
                };

                ticketMessageViewItems.Add(ticketMessageModel);
            }

            return ticketMessageViewItems;
        }

        List<TicketMessageViewItem> ISupportPageService.GetSupportTicketsMessages(long ticketId)
        {
            var ticketsMessagesViews = this._supportTicketsMessagesService.
                GetSupportTicketsMessages(ticketId) as List<SupportTicketsMessagesView>;

            if (ticketsMessagesViews == null)
            {
                return null;
            }

            List<TicketMessageViewItem> ticketMessageViewItems = new List<TicketMessageViewItem>();
            foreach (var ticketsMessagesView in ticketsMessagesViews)
            {
                var ticketMessageView = new TicketMessageViewItem
                {
                    Message = ticketsMessagesView.Message,
                    FilePath = ticketsMessagesView.FilePath,
                    Reporter = this._userService.GetUserById(ticketsMessagesView.AuthorID).GetFullName(),
                    AuthorId = ticketsMessagesView.AuthorID,
                    CreateTime = ticketsMessagesView.DateTime.ToString(),
                    IsOwner = ticketsMessagesView.AuthorID == this._appContext.AppUser.Id
                };
                ticketMessageViewItems.Add(ticketMessageView);
            }

            return ticketMessageViewItems;
        }

        List<TicketMessageViewItem> ISupportPageService.GetTicketsByTicketId(long id)
        {
            SupportTickets supportTickets = this._supportTicketsService.GetSupportTicketById(id);

            if (supportTickets == null)
            {
                return null;
            }

            this._supportTicketsService.SetTicketMessagesRead(id);

            User user1 = this._userService.GetUserById(supportTickets.UserID);
            User user2 = this._userService.GetUserById(supportTickets.ManagerID);

            List<TicketMessageViewItem> ticketMessageViewItems = new List<TicketMessageViewItem>();
            var ticketMessageViewItem = new TicketMessageViewItem
            {
                TicketId = id,
                AuthorId = this._appContext.AppUser.Id,
                Subject = supportTickets.Subject,
                StatusString = ((TicketStatus)supportTickets.Status).ToString(),
                PriorityString = ((TicketPriority)supportTickets.Priority).ToString(),
                Reporter = user1.GetFullName(),
                Assignee = user2.GetFullName()
            };

            ticketMessageViewItems.Add(ticketMessageViewItem);

            return ticketMessageViewItems;
        }

        long ISupportPageService.InsertSupportTicketsMessage(SupportTicketsMessages supportTicketsMessages)
        {
            long retVal = this._supportTicketsMessagesService.InsertSupportTicketsMessage(supportTicketsMessages);

            return retVal;
        }

        SupportTickets ISupportPageService.GetSupportTicketById(long ticketId)
        {
            SupportTickets supportTickets = this._supportTicketsService.GetSupportTicketById(ticketId);

            return supportTickets;
        }

        long ISupportPageService.AddNewSupportTickets(SupportTickets supportTickets, string cc)
        {
            long retValId = this._supportTicketsService.InsertSupportTicket(supportTickets);

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccString = cc.Split(',');

                foreach (string str in ccString)
                {
                    if (str != "null")
                    {
                        this._supportTicketsService.AddSupportTicketUser(retValId, long.Parse(str));
                    }
                }
            }

            return retValId < 0 ? 0L : retValId;
        }

        #endregion ISupportPageService interface implementation
    }
}