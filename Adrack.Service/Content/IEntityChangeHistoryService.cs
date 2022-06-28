using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core.Domain.Content;

namespace Adrack.Service.Content
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityChangeHistoryService
    {
        /// <summary>
        /// Update History
        /// </summary>
        /// <param name="history">EntityChangeHistory</param>
        void UpdateHistory(EntityChangeHistory history);

        /// <summary>
        /// Get history row
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EntityChangeHistory GetHistoryById(long id);

        /// <summary>
        /// Get Entity created history row
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        EntityChangeHistory GetEntityHistory(long entityId, string entity, string state);


        /// <summary>
        /// Get History
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<GetHistory> GetHistory(long entityId, string entityName, DateTime startDate, DateTime endDate, long skip, long take, out long count);

        List<GetHistory> GetHistory(long entityId, string entityName, DateTime startDate, DateTime endDate);
    }
}
