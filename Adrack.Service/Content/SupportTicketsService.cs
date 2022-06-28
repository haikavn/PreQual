// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SupportTicketsService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Content.ISupportTicketsService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.ISupportTicketsService" />
    public partial class SupportTicketsService : ISupportTicketsService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_TICKETS_BY_ID_KEY = "App.Cache.SupportTickets.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_TICKETS_ALL_KEY = "App.Cache.SupportTickets.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_TICKETS_PATTERN_KEY = "App.Cache.SupportTickets.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<SupportTickets> _SupportTicketsRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="supportTicketsRepository">The support tickets repository.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        public SupportTicketsService(IRepository<SupportTickets> supportTicketsRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._SupportTicketsRepository = supportTicketsRepository;

            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Invoices of Afiliates
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<SupportTickets> GetAllSupportTickets()
        {
            string key = CACHE_TICKETS_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _SupportTicketsRepository.Table
                            orderby x.Id
                            select x;

                var result = query.ToList();

                return result;
            });
        }

        /// <summary>
        /// Get All Support Tickets
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIds">The user ids.</param>
        /// <returns>Tickets List</returns>
        public virtual IList<SupportTicketsView> GetSupportTickets(long userId, string userIds = "")
        {
            var UserIdParam = _dataProvider.GetParameter();
            UserIdParam.ParameterName = "UserId";
            UserIdParam.Value = userId;
            UserIdParam.DbType = DbType.Int64;

            var UserIdsParam = _dataProvider.GetParameter();
            UserIdsParam.ParameterName = "UserIds";
            UserIdsParam.Value = userIds;
            UserIdsParam.DbType = DbType.String;

            var result = _SupportTicketsRepository.GetDbClientContext().SqlQuery<SupportTicketsView>("EXECUTE [dbo].[GetSupportTickets] @UserId, @UserIds", UserIdParam, UserIdsParam).ToList();

            return result;
        }

        /// <summary>
        /// Get All Support Tickets by Filters
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <param name="managerIds">The managerIds.</param>
        /// <param name="status">status.</param>
        /// <param name="date">date.</param>
        /// <returns>Tickets List</returns>
        public virtual IList<SupportTicketsView> GetSupportTicketsByFilters(string userIds, string managerIds, int status, DateTime date, DateTime? dueDate = null)
        {
            var UserIdsParam = _dataProvider.GetParameter();
            UserIdsParam.ParameterName = "userIds";
            UserIdsParam.Value = userIds;
            UserIdsParam.DbType = DbType.String;

            var ManagerIdsParam = _dataProvider.GetParameter();
            ManagerIdsParam.ParameterName = "managerIds";
            ManagerIdsParam.Value = managerIds;
            ManagerIdsParam.DbType = DbType.String;

            var StatusParam = _dataProvider.GetParameter();
            StatusParam.ParameterName = "status";
            StatusParam.Value = status;
            StatusParam.DbType = DbType.Int32;

            var DateParam = _dataProvider.GetParameter();
            DateParam.ParameterName = "date";
            DateParam.Value = date;
            DateParam.DbType = DbType.DateTime;

            var dueDateParam = _dataProvider.GetParameter();
            dueDateParam.ParameterName = "duedate";
            dueDateParam.Value = dueDate;
            dueDateParam.DbType = DbType.DateTime;

            var result = _SupportTicketsRepository.GetDbClientContext().SqlQuery<SupportTicketsView>("EXECUTE [dbo].[GetSupportTicketsByFilters] @UserIds, @ManagerIds, @Status, @Date, @DueDate", UserIdsParam, ManagerIdsParam, StatusParam, DateParam, dueDateParam).ToList();

            return result;
        }

        /// <summary>
        /// GetSupportTicketsByKeyword
        /// </summary>
        /// <param name="keyword">keyword</param>
        public virtual IList<SupportTicketsView> GetSupportTicketsByKeyword(string keyword)
        {
            var keywordParam = _dataProvider.GetParameter();
            keywordParam.ParameterName = "keyword";
            keywordParam.Value = keyword;
            keywordParam.DbType = DbType.String;

            var result = _SupportTicketsRepository.GetDbClientContext().SqlQuery<SupportTicketsView>("EXECUTE [dbo].[GetSupportTicketsByKeyword] @keyword", keywordParam).ToList();

            return result;
        }

        /// <summary>
        /// Get All Support Tickets
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>Tickets List</returns>
        public virtual IList<SupportTicketsView> GetSupportTicketsbyUsers(long AffiliateId, long BuyerId)
        {
            var ParentIdParam = _dataProvider.GetParameter();
            ParentIdParam.ParameterName = "ParentId";
            ParentIdParam.Value = AffiliateId != 0 ? AffiliateId : BuyerId;
            ParentIdParam.DbType = DbType.Int64;

            var TypeParam = _dataProvider.GetParameter();
            TypeParam.ParameterName = "Type";
            TypeParam.Value = AffiliateId != 0 ? 3 : 4; // 3-affiliate, 4-buyer
            TypeParam.DbType = DbType.Int32;

            var result = _SupportTicketsRepository.GetDbClientContext().SqlQuery<SupportTicketsView>("EXECUTE [dbo].[GetSupportTicketsByUsers] @ParentId, @Type", ParentIdParam, TypeParam).ToList();

            return result;
        }

        /// <summary>
        /// Get All Invoices of Afiliates
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Profile Collection Item</returns>

        public virtual int SetTicketMessagesRead(long ticketId)
        {
            var ticketIdParam = _dataProvider.GetParameter();
            ticketIdParam.ParameterName = "ticketId";
            ticketIdParam.Value = ticketId;
            ticketIdParam.DbType = DbType.Int64;

            int retVal = _SupportTicketsRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[SetTicketMessagesRead] @ticketId", ticketIdParam).FirstOrDefault();
            return retVal;
        }

        /// <summary>
        /// Get All Invoices of Afiliates
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual SupportTickets GetSupportTicketById(long ticketId)
        {
                var query = from x in _SupportTicketsRepository.Table
                            where x.Id == ticketId
                            select x;

                SupportTickets st = query.FirstOrDefault();

                return st;            
        }

        /// <summary>
        /// Inserts the support ticket.
        /// </summary>
        /// <param name="supportTicket">The support ticket.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">supportTicket</exception>
        public virtual long InsertSupportTicket(SupportTickets supportTicket)
        {
            if (supportTicket == null)
                throw new ArgumentNullException("supportTicket");

            _SupportTicketsRepository.Insert(supportTicket);

            _cacheManager.RemoveByPattern(CACHE_TICKETS_PATTERN_KEY);

            _appEventPublisher.EntityInserted(supportTicket);
            return supportTicket.Id;
        }

        /// <summary>
        /// Update the support ticket.
        /// </summary>
        /// <param name="supportTicket">The support ticket.</param>
        /// <returns>System.Int64.</returns>
        public virtual long UpdateSupportTicket(SupportTickets supportTicket)
        {
            if (supportTicket == null)
                throw new ArgumentNullException("supportTicket");

            SupportTickets supportTicketItem = _SupportTicketsRepository.GetById(supportTicket.Id);

            supportTicketItem.DateTime = supportTicket.DateTime;
            supportTicketItem.DueDate = supportTicket.DueDate;
            supportTicketItem.ManagerID = supportTicket.ManagerID;
            supportTicketItem.Priority = supportTicket.Priority;
            supportTicketItem.Status = supportTicket.Status;
            supportTicketItem.TicketType = supportTicket.TicketType;

            _SupportTicketsRepository.Update(supportTicketItem);
            _cacheManager.RemoveByPattern(CACHE_TICKETS_PATTERN_KEY);
            
            _appEventPublisher.EntityInserted(supportTicket);

            return supportTicket.Id;
        }



        /// <summary>
        /// Adds the support ticket user.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int64.</returns>
        public virtual long AddSupportTicketUser(long ticketId, long userId)
        {
            var ticketIdParam = _dataProvider.GetParameter();
            ticketIdParam.ParameterName = "TicketId";
            ticketIdParam.Value = ticketId;
            ticketIdParam.DbType = DbType.Int64;

            var userIdParam = _dataProvider.GetParameter();
            userIdParam.ParameterName = "UserId";
            userIdParam.Value = userId;
            userIdParam.DbType = DbType.Int64;

            _SupportTicketsRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[AddSupportTicketUser] @TicketId, @UserId", ticketIdParam, userIdParam).FirstOrDefault();
            return 0;
        }

        /// <summary>
        /// Changes the tickets status.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="ArgumentNullException">supportTicket</exception>
        public virtual int ChangeTicketsStatus(long ticketId, int status)
        {
            SupportTickets supportTicket = _SupportTicketsRepository.GetById(ticketId);

            if (supportTicket == null)
                return 0;

            supportTicket.Status = status;
            _SupportTicketsRepository.Update(supportTicket);

            _cacheManager.RemoveByPattern(CACHE_TICKETS_PATTERN_KEY);

            _appEventPublisher.EntityInserted(supportTicket);
            return supportTicket.Status;
        }

        #endregion Methods
    }
}