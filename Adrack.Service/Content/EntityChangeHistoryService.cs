using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;

namespace Adrack.Service.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityChangeHistoryService : IEntityChangeHistoryService
    {
        #region Constants

        private const string CACHE_ENTITY_CHANGE_HISTORY_ALL_KEY = "App.Cache.EntityChangeHistory";
        private const string CACHE_ENTITY_CHANGE_GetHistory = "App.Cache.EntityChangeHistory.GetHistory{0}{1}{2}{3}";
        private const string CACHE_ENTITY_CHANGE_GetEntityHistory = "App.Cache.EntityChangeHistory.GetEntityHistory{0}{1}{2}";

        #endregion

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<EntityChangeHistory> _entityChangeHistoryRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        private readonly IDataProvider _dataProvider;

        private readonly IRepository<User> _userRepository;
        #endregion

        /// <summary>
        /// History Service
        /// </summary>
        /// <param name="entityChangeHistoryRepository"></param>
        /// <param name="dbContext"></param>
        /// <param name="cacheManager"></param>
        public EntityChangeHistoryService(IRepository<EntityChangeHistory> entityChangeHistoryRepository,
                                          ICacheManager cacheManager,
                                          IDataProvider dataProvider,
                                          IAppEventPublisher appEventPublisher,
                                          IRepository<User> userRepository)
        {
            _entityChangeHistoryRepository = entityChangeHistoryRepository;
            _cacheManager = cacheManager;
            _dataProvider = dataProvider;
            _appEventPublisher = appEventPublisher;
            _userRepository = userRepository;
        }


        /// <summary>
        /// Update History
        /// </summary>
        /// <param name="history">EntityChangeHistory</param>
        public virtual void UpdateHistory(EntityChangeHistory history)
        {
            if (history == null)
                throw new ArgumentNullException("history");

            _entityChangeHistoryRepository.Update(history);
            _cacheManager.RemoveByPattern(CACHE_ENTITY_CHANGE_HISTORY_ALL_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(history);
        }

        /// <summary>
        /// Get history row
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityChangeHistory GetHistoryById(long id)
        {
            if (id == 0)
                return null;

            return _entityChangeHistoryRepository.GetById(id);
        }

        /// <summary>
        /// Get Entity created history row
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual EntityChangeHistory GetEntityHistory(long entityId, string entity, string state)
        {
            string key = string.Format(CACHE_ENTITY_CHANGE_GetEntityHistory, entityId, entity, state);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _entityChangeHistoryRepository.Table
                    where x.EntityId == entityId && x.Entity == entity && x.State == state
                    orderby x.Id descending
                    select x;

                return query.FirstOrDefault();

            });

            /*
            var query = from x in _entityChangeHistoryRepository.Table
                        where x.EntityId == entityId && x.Entity == entity && x.State == state
                        orderby x.Id descending
                        select x;

            return query.FirstOrDefault();
            */
        }

        public virtual List<GetHistory> GetHistory(long entityId, string entityName, DateTime startDate, DateTime endDate)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = startDate;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = endDate;
            dateToParam.DbType = DbType.DateTime;

            var entityNameParam = _dataProvider.GetParameter();
            entityNameParam.ParameterName = "EntityName";
            entityNameParam.Value = entityName;
            entityNameParam.DbType = DbType.String;

            var entityIdParam = _dataProvider.GetParameter();
            entityIdParam.ParameterName = "EntityId";
            entityIdParam.Value = entityId;
            entityIdParam.DbType = DbType.Int64;

            string key = string.Format(CACHE_ENTITY_CHANGE_GetHistory, entityIdParam.Value, dateFromParam.Value, dateToParam.Value
                ,entityNameParam.Value);

            return _cacheManager.Get(key, () =>
            {
                var result = _entityChangeHistoryRepository.GetDbClientContext().SqlQuery<GetHistory>("EXECUTE [dbo].[GetHistoryFinal] @EntityId, @dateFrom, @dateTo, @EntityName",
                           entityIdParam, dateFromParam, dateToParam, entityNameParam);
                return result.ToList();

            });
        }

        /// <summary>
        /// Get history
        /// </summary>
        /// <returns></returns>
        public virtual List<GetHistory> GetHistory(long entityId, string entityName, DateTime startDate, DateTime endDate, long skip, long take, out long count)
        {
            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            dateFromParam.Value = startDate;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = endDate;
            dateToParam.DbType = DbType.DateTime;

            var entityNameParam = _dataProvider.GetParameter();
            entityNameParam.ParameterName = "EntityName";
            entityNameParam.Value = entityName;
            entityNameParam.DbType = DbType.String;

            var entityIdParam = _dataProvider.GetParameter();
            entityIdParam.ParameterName = "EntityId";
            entityIdParam.Value = entityId;
            entityIdParam.DbType = DbType.Int64;

            var rowsCount = _dataProvider.GetParameter();
            rowsCount.ParameterName = "RowsCount";
            rowsCount.Direction = System.Data.ParameterDirection.Output;
            rowsCount.DbType = DbType.Int64;

            var offset = _dataProvider.GetParameter();
            offset.ParameterName = "Offset";
            offset.DbType = DbType.Int64;
            offset.Value = skip;

            var fetch = _dataProvider.GetParameter();
            fetch.ParameterName = "Fetch";
            fetch.DbType = DbType.Int64;
            fetch.Value = take;

            var result = _entityChangeHistoryRepository.GetDbClientContext().SqlQuery<GetHistory>("EXECUTE [dbo].[GetHistoryWithPagingFinal] @EntityId, @dateFrom, @dateTo, @EntityName, @Offset, @Fetch, @RowsCount output",
                        entityIdParam, dateFromParam, dateToParam, entityNameParam, offset, fetch, rowsCount);

            var res = result.ToList();
            count = (long)rowsCount.Value;
            return res;
        }
    }
}
