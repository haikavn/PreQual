// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IPagination.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Pagination
    /// Implements the <see cref="System.Collections.Generic.IList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    public interface IPagination<T> : IList<T>
    {
        #region Properties

        /// <summary>
        /// Page Index
        /// </summary>
        /// <value>The index of the page.</value>
        int PageIndex { get; }

        /// <summary>
        /// Page Size
        /// </summary>
        /// <value>The size of the page.</value>
        int PageSize { get; }

        /// <summary>
        /// Total Page Count
        /// </summary>
        /// <value>The total page count.</value>
        int TotalPageCount { get; }

        /// <summary>
        /// Total Pages
        /// </summary>
        /// <value>The total pages.</value>
        int TotalPages { get; }

        /// <summary>
        /// Has Previous Page
        /// </summary>
        /// <value><c>true</c> if this instance has previous page; otherwise, <c>false</c>.</value>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Has Next Page
        /// </summary>
        /// <value><c>true</c> if this instance has next page; otherwise, <c>false</c>.</value>
        bool HasNextPage { get; }

        #endregion Properties
    }
}