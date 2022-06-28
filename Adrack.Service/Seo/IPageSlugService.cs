// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IPageSlugService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Seo;
using System.Collections.Generic;

namespace Adrack.Service.Seo
{
    /// <summary>
    /// Represents a Page Slug Service
    /// </summary>
    public partial interface IPageSlugService
    {
        #region Methods

        /// <summary>
        /// Get Page Slug By Id
        /// </summary>
        /// <param name="pageSlugId">Page Slug Identifier</param>
        /// <returns>Page Slug Item</returns>
        PageSlug GetPageSlugById(long pageSlugId);

        /// <summary>
        /// Get Page Slug By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Page Slug Item</returns>
        PageSlug GetPageSlugByName(string name);

        /// <summary>
        /// Get Cache Page Slug By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Cache Page Slug Item</returns>
        PageSlugService.CachePageSlug GetCachePageSlugByName(string name);

        /// <summary>
        /// Get Active Page Slug
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="entityName">Entity Name</param>
        /// <returns>System.String.</returns>
        string GetActivePageSlug(long languageId, long entityId, string entityName);

        /// <summary>
        /// Get All Page Slugs
        /// </summary>
        /// <returns>Page Slug Collection Item</returns>
        IList<PageSlug> GetAllPageSlugs();

        /// <summary>
        /// Get All Page Slugs
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Page Slug Collection Item</returns>
        IPagination<PageSlug> GetAllPageSlugs(string name, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Insert Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        void InsertPageSlug(PageSlug pageSlug);

        /// <summary>
        /// Save Page Slug
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="name">Name</param>
        /// <param name="languageId">Language Identifier</param>
        void SavePageSlug<T>(T entity, string name, long languageId) where T : BaseEntity, IPageSlugSupported;

        /// <summary>
        /// Update Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        void UpdatePageSlug(PageSlug pageSlug);

        /// <summary>
        /// Delete Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        void DeletePageSlug(PageSlug pageSlug);

        #endregion Methods
    }
}