// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SupportTicketsMessagesService.cs" company="Adrack.com">
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
    /// Implements the <see cref="Adrack.Service.Content.ISupportTicketsMessagesService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.ISupportTicketsMessagesService" />
    public partial class SupportTicketsMessagesService : ISupportTicketsMessagesService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.SupportTicketsMessages.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.SupportTicketsMessages.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.SupportTicketsMessages.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<SupportTicketsMessages> _SupportTicketsMessagesRepository;

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
        /// <param name="supportTicketsMessagesRepository">The support tickets messages repository.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        public SupportTicketsMessagesService(IRepository<SupportTicketsMessages> supportTicketsMessagesRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._SupportTicketsMessagesRepository = supportTicketsMessagesRepository;
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
        public virtual IList<SupportTicketsMessages> GetAllSupportTicketsMessages()
        {
            var query = from x in _SupportTicketsMessagesRepository.Table
                            orderby x.Id
                            select x;

                var result = query.ToList();

                return result;            
        }

        /// <summary>
        /// Gets the support tickets messages.
        /// </summary>
        /// <param name="TicketId">The ticket identifier.</param>
        /// <returns>IList&lt;SupportTicketsMessagesView&gt;.</returns>
        public virtual IList<SupportTicketsMessagesView> GetSupportTicketsMessages(long TicketId)
        {
                var ticketId = _dataProvider.GetParameter();

                ticketId.ParameterName = "TicketId";
                ticketId.Value = TicketId;
                ticketId.DbType = DbType.Int64;

                var result = _SupportTicketsMessagesRepository.GetDbClientContext().SqlQuery<SupportTicketsMessagesView>("EXECUTE [dbo].[GetSupportTicketsMessages] @TicketId", ticketId).ToList();

                return result;            
        }

        /// <summary>
        /// InsertSupportTicketsMessage
        /// </summary>
        /// <param name="supportTicketsMessages">The support tickets messages.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">supportTicketsMessages</exception>
        public virtual long InsertSupportTicketsMessage(SupportTicketsMessages supportTicketsMessages)
        {
            if (supportTicketsMessages == null)
                throw new ArgumentNullException("supportTicketsMessages");

            _SupportTicketsMessagesRepository.Insert(supportTicketsMessages);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(supportTicketsMessages);
            return supportTicketsMessages.Id;
        }

        #endregion Methods
    }
}