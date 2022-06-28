// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="QueryableExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Queryable Extensions
    /// </summary>
    public static class QueryableExtensions
    {
        #region Methods

        /// <summary>
        /// Include Properties
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="includeProperties">Include Properties</param>
        /// <returns>Queryable Collection Item</returns>
        /// <exception cref="ArgumentNullException">queryable</exception>
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includeProperties)
        {
            if (queryable == null)
                throw new ArgumentNullException("queryable");

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            return queryable;
        }

        #endregion Methods
    }
}