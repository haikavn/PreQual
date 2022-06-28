// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Pagination.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Pagination
    /// Implements the <see cref="System.Collections.Generic.List{T}" />
    /// Implements the <see cref="Adrack.Core.IPagination{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    /// <seealso cref="Adrack.Core.IPagination{T}" />
    [Serializable]
    public class Pagination<T> : List<T>, IPagination<T>
    {
        #region Constructor

        /// <summary>
        /// Pagination
        /// </summary>
        /// <param name="source">Sourcce</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        public Pagination(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Init(source, pageIndex, pageSize);
        }

        /// <summary>
        /// Pagination
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="totalCount">Total Count</param>
        public Pagination(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Init(source, pageIndex, pageSize, totalCount);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Pagination
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="totalPageCount">Total Page Count</param>
        /// <exception cref="ArgumentNullException">source</exception>
        /// <exception cref="ArgumentException">pageSize must be greater than zero</exception>
        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int? totalPageCount = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalPageCount = totalPageCount ?? source.Count();
            TotalPages = TotalPageCount / pageSize;

            if (TotalPageCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            source = totalPageCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize) : source;
            AddRange(source);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Page Index
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; private set; }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; private set; }

        /// <summary>
        /// Total Page Count
        /// </summary>
        /// <value>The total page count.</value>
        public int TotalPageCount { get; private set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Has Previous Page
        /// </summary>
        /// <value><c>true</c> if this instance has previous page; otherwise, <c>false</c>.</value>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        /// <summary>
        /// Has Next Page
        /// </summary>
        /// <value><c>true</c> if this instance has next page; otherwise, <c>false</c>.</value>
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        #endregion Properties
    }
}