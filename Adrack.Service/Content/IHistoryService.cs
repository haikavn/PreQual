// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IHistoryService.cs" company="Adrack.com">
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
    public partial interface IHistoryService
    {
        #region Methods

        /// <summary>
        /// GetAllHistory
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
        IList<History> GetAllHistory(DateTime dateFrom, DateTime dateTo, int action, long userId, string userIds, string entity, long entityId, int StartAt, int Count);

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
        IList<History> GetHistoryByUsers(DateTime dateFrom, DateTime dateTo, int action, string UserIds, int StartAt, int Count);

        /// <summary>
        /// GetHistoryById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>History Item</returns>
        History GetHistoryById(long Id);

        /// <summary>
        /// AddHistory
        /// </summary>
        /// <param name="history">The history.</param>
        /// <returns>long Item Id</returns>
        long AddHistory(History history);

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
        long AddHistory(string Module, HistoryAction ActionEnum, string Entity, long EntityID, string Data1, string Data2, string Note, long UserID);

        #endregion Methods
    }
}