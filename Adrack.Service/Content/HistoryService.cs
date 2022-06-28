// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="HistoryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>

    public enum HistoryAction
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Creates new _buyer_user_registered.
        /// </summary>
        New_Buyer_User_Registered,

        /// <summary>
        /// Creates new _affiliate_user_registered.
        /// </summary>
        New_Affiliate_User_Registered,

        /// <summary>
        /// Creates new _store_user_registered.
        /// </summary>
        New_Store_User_Registered,

        /// <summary>
        /// Creates new _system_user_registered.
        /// </summary>
        New_System_User_Registered,

        /// <summary>
        /// The user edited
        /// </summary>
        User_Edited,

        /// <summary>
        /// The user deleted
        /// </summary>
        User_Deleted,

        /// <summary>
        /// The payment added
        /// </summary>
        Payment_Added,

        /// <summary>
        /// The payment deleted
        /// </summary>
        Payment_Deleted,

        /// <summary>
        /// The payment edited
        /// </summary>
        Payment_Edited,

        /// <summary>
        /// The invoice deleted
        /// </summary>
        Invoice_Deleted,

        /// <summary>
        /// The invoice status changed
        /// </summary>
        Invoice_Status_Changed,

        /// <summary>
        /// The invoice edited custom adjustment
        /// </summary>
        Invoice_Edited_Custom_Adjustment,

        /// <summary>
        /// The invoice edited custom adjustment deleted
        /// </summary>
        Invoice_Edited_Custom_Adjustment_Deleted,

        /// <summary>
        /// The invoice payment added
        /// </summary>
        Invoice_Payment_Added,

        /// <summary>
        /// The invoice generated invoice
        /// </summary>
        Invoice_Generated_Invoice,

        /// <summary>
        /// The invoice generated custom invoice
        /// </summary>
        Invoice_Generated_Custom_Invoice,

        /// <summary>
        /// The payment notice deleted
        /// </summary>
        Payment_Notice_Deleted,

        /// <summary>
        /// The payment notice changed
        /// </summary>
        Payment_Notice_Changed,

        /// <summary>
        /// The payment notice custom adjustment
        /// </summary>
        Payment_Notice_Custom_Adjustment,

        /// <summary>
        /// The payment notice custom adjustment deleted
        /// </summary>
        Payment_Notice_Custom_Adjustment_Deleted,

        /// <summary>
        /// The payment notice payment added
        /// </summary>
        Payment_Notice_Payment_Added,

        /// <summary>
        /// The payment notice payment history deleted
        /// </summary>
        Payment_Notice_Payment_History_Deleted,

        /// <summary>
        /// The refund added
        /// </summary>
        Refund_Added,

        /// <summary>
        /// The refund deleted
        /// </summary>
        Refund_Deleted,

        /// <summary>
        /// The refund status changed
        /// </summary>
        Refund_Status_Changed,

        /// <summary>
        /// The affiliate channel added
        /// </summary>
        Affiliate_Channel_Added,

        /// <summary>
        /// The affiliate channel edited
        /// </summary>
        Affiliate_Channel_Edited,

        /// <summary>
        /// The affiliate channel deleted
        /// </summary>
        Affiliate_Channel_Deleted,

        /// <summary>
        /// The buyer channel added
        /// </summary>
        Buyer_Channel_Added,

        /// <summary>
        /// The buyer channel edited
        /// </summary>
        Buyer_Channel_Edited,

        /// <summary>
        /// The buyer channel deleted
        /// </summary>
        Buyer_Channel_Deleted,

        /// <summary>
        /// The campaign added
        /// </summary>
        Campaign_Added,

        /// <summary>
        /// The campaign edited
        /// </summary>
        Campaign_Edited,

        /// <summary>
        /// The campaign deleted
        /// </summary>
        Campaign_Deleted,

        /// <summary>
        /// The buyer added
        /// </summary>
        Buyer_Added,

        /// <summary>
        /// The buyer edited
        /// </summary>
        Buyer_Edited,

        /// <summary>
        /// The buyer deleted
        /// </summary>
        Buyer_Deleted,

        /// <summary>
        /// The affiliate added
        /// </summary>
        Affiliate_Added,

        /// <summary>
        /// The affiliate edited
        /// </summary>
        Affiliate_Edited,

        /// <summary>
        /// The affiliate deleted
        /// </summary>
        Affiliate_Deleted,

        /// <summary>
        /// The filter added
        /// </summary>
        Filter_Added,

        /// <summary>
        /// The filter edited
        /// </summary>
        Filter_Edited,
    }

    /// <summary>
    /// Class HistoryService.
    /// Implements the <see cref="Adrack.Service.Content.IHistoryService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.IHistoryService" />
    public partial class HistoryService : IHistoryService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_HISTORY_BY_ID_KEY = "App.Cache.History.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.History.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.History.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<History> _HistoryRepository;

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

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="historyRepository">The history repository.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="settingService">The setting service.</param>
        public HistoryService(IRepository<History> historyRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider, ISettingService settingService)
        {
            this._HistoryRepository = historyRepository;

            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._settingService = settingService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Invoices of Afiliates
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="action">The action.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userIds">The user ids.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<History> GetAllHistory(DateTime dateFrom, DateTime dateTo, int action, long userId, string userIds, string entity, long entityId, int StartAt, int Count)
        {
            

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var actionParam = _dataProvider.GetParameter();
            actionParam.ParameterName = "Action";
            actionParam.Value = action;
            actionParam.DbType = DbType.Int32;

            var userParam = _dataProvider.GetParameter();
            userParam.ParameterName = "UserId";
            userParam.Value = userId;
            userParam.DbType = DbType.Int64;

            var usersParam = _dataProvider.GetParameter();
            usersParam.ParameterName = "UserIds";
            usersParam.Value = userIds;
            usersParam.DbType = DbType.String;

            var entityParam = _dataProvider.GetParameter();
            entityParam.ParameterName = "Entity";
            entityParam.Value = entity;
            entityParam.DbType = DbType.String;

            var entityIdParam = _dataProvider.GetParameter();
            entityIdParam.ParameterName = "EntityId";
            entityIdParam.Value = entityId;
            entityIdParam.DbType = DbType.Int64;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = StartAt;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = Count;
            countParam.DbType = DbType.Int32;

           
           var result = _HistoryRepository.GetDbClientContext().SqlQuery<History>("EXECUTE [dbo].[GetHistory] @dateFrom, @dateTo, @Action, @UserId, @UserIds, @Entity, @EntityId, @start, @count",
                            dateFromParam, dateToParam, actionParam, userParam, usersParam, entityParam, entityIdParam, startParam, countParam).ToList();
           return result;           
        }

        /// <summary>
        /// Gets the history by users.
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="action">The action.</param>
        /// <param name="UserIds">The user ids.</param>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>IList&lt;History&gt;.</returns>
        public IList<History> GetHistoryByUsers(DateTime dateFrom, DateTime dateTo, int action, string UserIds, int StartAt, int Count)
        {            

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = dateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var actionParam = _dataProvider.GetParameter();
            actionParam.ParameterName = "Action";
            actionParam.Value = action;
            actionParam.DbType = DbType.Int32;

            var userParam = _dataProvider.GetParameter();
            userParam.ParameterName = "UserIds";
            userParam.Value = UserIds;
            userParam.DbType = DbType.String;

            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "start";
            startParam.Value = StartAt;
            startParam.DbType = DbType.Int32;

            var countParam = _dataProvider.GetParameter();
            countParam.ParameterName = "count";
            countParam.Value = Count;
            countParam.DbType = DbType.Int32;

            var result = _HistoryRepository.GetDbClientContext().SqlQuery<History>("EXECUTE [dbo].[GetHistoryByUsers] @dateFrom, @dateTo, @Action, @UserIds, @start, @count",
                            dateFromParam, dateToParam, actionParam, userParam, startParam, countParam).ToList();
            return result;
            
        }

        /// <summary>
        /// GetHistoryById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>History Item</returns>
        public virtual History GetHistoryById(long Id)
        {
            string key = string.Format(CACHE_HISTORY_BY_ID_KEY, Id);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _HistoryRepository.Table
                            where x.Id == Id
                            select x;

                var result = query.FirstOrDefault();

                return result;
            });
        }

        /// <summary>
        /// AddHistory
        /// </summary>
        /// <param name="history">The history.</param>
        /// <returns>long Item Id</returns>
        public virtual long AddHistory(History history)
        {
            _HistoryRepository.Insert(history);

            return history.Id;
        }

        /// <summary>
        /// AddHistory
        /// </summary>
        /// <param name="Module">The module.</param>
        /// <param name="ActionEnum">The action enum.</param>
        /// <param name="Entity">The entity.</param>
        /// <param name="EntityID">The entity identifier.</param>
        /// <param name="Data1">The data1.</param>
        /// <param name="Data2">The data2.</param>
        /// <param name="Note">The note.</param>
        /// <param name="UserID">The user identifier.</param>
        /// <returns>long Item Id</returns>
        public virtual long AddHistory(string Module, HistoryAction ActionEnum, string Entity, long EntityID, string Data1, string Data2, string Note, long UserID)
        {
            if (Data1.Length > 0 && Data2.Length > 0 && Data1 == Data2)
                return 0;

            History h = new History
            {
                Module = Module,
                Action = (int)ActionEnum,
                Entity = Entity,
                EntityID = EntityID,
                Data1 = Data1,
                Data2 = Data2,
                Note = Note,
                UserID = UserID,
                Date = DateTime.UtcNow
            };

            _HistoryRepository.Insert(h);

            return h.Id;
        }

        #endregion Methods
    }
}