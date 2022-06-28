// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ISupportTicketsMessagesService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ISupportTicketsMessagesService
    {
        #region Methods

        /// <summary>
        /// GetAllAffiliateInvoices
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<SupportTicketsMessages> GetAllSupportTicketsMessages();

        /// <summary>
        /// Gets the support tickets messages.
        /// </summary>
        /// <param name="TicketId">The ticket identifier.</param>
        /// <returns>IList&lt;SupportTicketsMessagesView&gt;.</returns>
        IList<SupportTicketsMessagesView> GetSupportTicketsMessages(long TicketId);

        /// <summary>
        /// Inserts the support tickets message.
        /// </summary>
        /// <param name="supportTicketsMessages">The support tickets messages.</param>
        /// <returns>System.Int64.</returns>
        long InsertSupportTicketsMessage(SupportTicketsMessages supportTicketsMessages);

        #endregion Methods
    }
}