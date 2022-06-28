using Adrack.Core.Domain.Content;
using Adrack.WebApi.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface ISupportPageService
    {
        SupportTickets ChangeTicketsStatus(long id, int status);
        List<TicketMessageViewItem> GetSupportTicketsMessages(long ticketId);
        List<TicketMessageViewItem> GetTicketsByTicketId(long id);
        List<TicketMessageViewItem> GetTicketsByUserId(long userId, int? status, string userIds);
        SupportTickets GetSupportTicketById(long ticketId);
        long AddNewSupportTickets(SupportTickets supportTickets, string cc);
        long InsertSupportTicketsMessage(SupportTicketsMessages supportTicketsMessages);
    }
}
