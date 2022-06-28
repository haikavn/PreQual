// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-18-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-18-2021
// ***********************************************************************
// <copyright file="IStaticPagesService.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Content;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IStaticPagesService
    {
        #region Methods

        /// <summary>
        /// Get All Static Pages
        /// </summary>
        /// <returns>StaticPages Collection Item</returns>
        IList<StaticPages> GetAllStaticPages();


        /// <summary>
        /// Get Static Page By Id
        /// </summary>
        /// <param name="id">StaticPage Id</param>
        /// <returns>Static Page Item</returns>
        StaticPages GetStaticPageById(long id);

        /// <summary>
        /// Insert Static Page
        /// </summary>
        /// <param name="staticPage">The StaticPage.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        long InsertStaticPage(StaticPages staticPage);

        /// <summary>
        /// Update Static Page
        /// </summary>
        /// <param name="staticPage">The Static Page.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        void UpdateStaticPage(StaticPages staticPage);

        /// <summary>
        /// Delete StaticPage
        /// </summary>
        /// <param name="staticPage">The Static Page.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        void DeleteStaticPage(StaticPages staticPage);

        #endregion Methods

        #region StaticPageCategory Methods

        /// <summary>
        /// Get All Static Page Categories
        /// </summary>
        /// <returns>StaticPageCategory Collection Item</returns>
        IList<StaticPageCategory> GetAllStaticPageCategories();


        /// <summary>
        /// Get Static Page Category By Category Id
        /// </summary>
        /// <param name="categoryId">StaticPageCategory Id</param>
        /// <returns>Static Page Item</returns>
        StaticPageCategory GetStaticPageCategoryById(long categoryId);


        /// <summary>
        /// Get Static Page Child Categories
        /// </summary>
        /// <param name="categoryId">StaticPageCategory Id</param>
        /// <returns>Static Page Item</returns>
        IList<StaticPageCategory> GetStaticPageChildCategories(long categoryId);

        /// <summary>
        /// Insert Static Page Category
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">staticPageCategory</exception>
        long InsertStaticPageCategory(StaticPageCategory category);


        /// <summary>
        /// Update Static Page Category
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <exception cref="ArgumentNullException">staticPageCategory</exception>
        void UpdateStaticPageCategory(StaticPageCategory category);


        /// <summary>
        /// Delete StaticPageCategory
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        void DeleteStaticPageCategory(StaticPageCategory category);

        #endregion StaticPageCategory

        #region StaticPageCategoryRelation Methods

        /// <summary>
        /// Get All Static Page Category Relations
        /// </summary>
        /// <param name="pageId">Page Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <returns>StaticPageCategoryRelation Collection Item</returns>
        IList<StaticPageCategoryRelation> GetAllStaticPageCategoryRelations(long pageId = 0, long categoryId = 0);


        /// <summary>
        /// Get StaticPageCategoryRelation
        /// </summary>
        /// <param name="pageId">Page Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <returns>StaticPageCategoryRelation Item</returns>
        StaticPageCategoryRelation GetStaticPageCategoryRelation(long pageId, long categoryId);


        /// <summary>
        /// Insert StaticPage Category Relation
        /// </summary>
        /// <param name="pageCategoryRelation">The page category relation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">pageCategoryRelation</exception>
        long InsertStaticPageCategoryRelation(StaticPageCategoryRelation pageCategoryRelation);


        /// <summary>
        /// Delete StaticPageCategory Relation
        /// </summary>
        /// <param name="pageCategoryRelation">The page category relation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">pageCategoryRelation</exception>
        void DeleteStaticPageCategoryRelation(StaticPageCategoryRelation pageCategoryRelation);

        #endregion StaticPageCategoryRelation Methods
    }
}