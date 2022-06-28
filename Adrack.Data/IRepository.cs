// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IRepository.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IRepository<T> where T : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Gets a Entity By Identifier
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Type</returns>
        T GetById(object id);

        void InsertNoUpdate(T entity);

        void Reset();

        void Truncate();

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        void SaveChanges();
        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(IEnumerable<T> entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        void Update(IEnumerable<T> entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Gets a Entity Table
        /// </summary>
        /// <value>The table.</value>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a Entity Table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        /// <value>The table no tracking.</value>
        IQueryable<T> TableNoTracking { get; }

        bool GetCanTrackChanges();

        void SetCanTrackChanges(bool canTrackChanges);

        string GetConnectionString();

        List<string> GetAllKeys();

        IDbClientContext GetDbClientContext();

        #endregion Methods
    }
}