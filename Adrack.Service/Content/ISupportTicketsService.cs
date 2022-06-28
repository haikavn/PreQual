// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ISupportTicketsService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ISupportTicketsService
    {
        #region Methods

        /// <summary>
        /// GetAllAffiliateInvoices
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<SupportTickets> GetAllSupportTickets();

        /// <summary>
        /// Gets the support tickets.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIds">The user ids.</param>
        /// <returns>IList&lt;SupportTicketsView&gt;.</returns>
        IList<SupportTicketsView> GetSupportTickets(long userId, string userIds = "");

        /// <summary>
        /// GetSupportTicketsByFilters
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <param name="managerIds">The managerIds.</param>
        /// <param name="status">status.</param>
        /// <param name="date">date.</param>
        /// <returns>Tickets List</returns>
        IList<SupportTicketsView> GetSupportTicketsByFilters(string userIds, string managerIds, int status, DateTime date, DateTime? dueDate = null);

        /// <summary>
        /// GetSupportTicketsByKeyword
        /// </summary>
        /// <param name="keyword">keyword</param>
        IList<SupportTicketsView> GetSupportTicketsByKeyword(string keyword);

        /// <summary>
        /// Gets the support ticketsby users.
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>IList&lt;SupportTicketsView&gt;.</returns>
        IList<SupportTicketsView> GetSupportTicketsbyUsers(long AffiliateId, long BuyerId);

        /// <summary>
        /// GetAllAffiliateInvoices
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Collection Item</returns>
        SupportTickets GetSupportTicketById(long Id);

        /// <summary>
        /// Changes the tickets status.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>System.Int32.</returns>
        int ChangeTicketsStatus(long ticketId, int status);

        /// <summary>
        /// Inserts the support ticket.
        /// </summary>
        /// <param name="supportTicket">The support ticket.</param>
        /// <returns>System.Int64.</returns>
        long InsertSupportTicket(SupportTickets supportTicket);

        /// <summary>
        /// Update the support ticket.
        /// </summary>
        /// <param name="supportTicket">The support ticket.</param>
        /// <returns>System.Int64.</returns>
        long UpdateSupportTicket(SupportTickets supportTicket);

        /// <summary>
        /// Sets the ticket messages read.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>System.Int32.</returns>
        int SetTicketMessagesRead(long ticketId);

        /// <summary>
        /// Adds the support ticket user.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int64.</returns>
        long AddSupportTicketUser(long ticketId, long userId);

        #endregion Methods
    }
}