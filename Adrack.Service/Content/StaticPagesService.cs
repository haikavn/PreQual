// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-18-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-18-2021
// ***********************************************************************
// <copyright file="StaticPagesService.cs" company="Adrack.com">
//     Copyright © 2021
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
using System.Linq;


namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Content.IStaticPagesService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.IStaticPagesService" />
    public partial class StaticPagesService : IStaticPagesService
    {
        #region Constants
        private const string CACHE_STATICPAGES_GetAllStaticPages = "App.Cache.StaticPages.GetAllStaticPages";
        private const string CACHE_STATICPAGES_PATTERN_KEY = "App.Cache.StaticPages.";

        private const string CACHE_STATICPAGECATEGORY_GetAllStaticPageCategories = "App.Cache.StaticPageCategory.GetAllStaticPageCategories";
        private const string CACHE_STATICPAGECATEGORY_PATTERN_KEY = "App.Cache.StaticPageCategory.";

        private const string CACHE_STATICPAGECATEGORYRELATION_GetAllStaticPageCategoryRelations = "App.Cache.StaticPageCategoryRelation.GetAllStaticPageCategoryRelations";
        private const string CACHE_STATICPAGECATEGORYRELATION_GetStaticPageCategoryRelation = "App.Cache.StaticPageCategoryRelation.GetStaticPageCategoryRelation-{0}-{1}";
        private const string CACHE_STATICPAGECATEGORYRELATION_PATTERN_KEY = "App.Cache.StaticPageCategoryRelation.";


        #endregion Constants

        #region Fields

        /// <summary>
        /// StaticPages
        /// </summary>
        private readonly IRepository<StaticPages> _staticPagesRepository;

        /// <summary>
        /// StaticPageCategory
        /// </summary>
        private readonly IRepository<StaticPageCategory> _staticPageCategoryRepository;

        /// <summary>
        /// StaticPageCategory
        /// </summary>
        private readonly IRepository<StaticPageCategoryRelation> _staticPageCategoryRelationRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// IDataProvider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        private readonly ISettingService _settingService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="staticPagesRepository">The static pages repository.</param>
        /// <param name="staticPageCategoryRepository">The static page category repository.</param>
        /// <param name="staticPageCategoryRelationRepository">The static page category relation repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        public StaticPagesService(
                                IRepository<StaticPages> staticPagesRepository,
                                IRepository<StaticPageCategory> staticPageCategoryRepository,
                                IRepository<StaticPageCategoryRelation> staticPageCategoryRelationRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher,
                                IDataProvider dataProvider,
                                ISettingService settingService
                                )
        {
            this._staticPagesRepository = staticPagesRepository;
            this._staticPageCategoryRepository = staticPageCategoryRepository;
            this._staticPageCategoryRelationRepository = staticPageCategoryRelationRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._settingService = settingService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Static Pages
        /// </summary>
        /// <returns>StaticPages Collection Item</returns>
        public virtual IList<StaticPages> GetAllStaticPages()
        {
            string key = string.Format(CACHE_STATICPAGES_GetAllStaticPages);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _staticPagesRepository.Table
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }


        /// <summary>
        /// Get Static Page By Id
        /// </summary>
        /// <param name="id">StaticPage Id</param>
        /// <returns>Static Page Item</returns>
        public virtual StaticPages GetStaticPageById(long id)
        {
            var query = from x in _staticPagesRepository.Table
                where x.Id == id
                select x;

            var page = query.FirstOrDefault();

            return page;
        }

        /// <summary>
        /// Insert Static Page
        /// </summary>
        /// <param name="staticPage">The StaticPage.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        public virtual long InsertStaticPage(StaticPages staticPage)
        {
            if (staticPage == null)
                throw new ArgumentNullException("staticPage");

            _staticPagesRepository.Insert(staticPage);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGES_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(staticPage);

            return staticPage.Id;
        }


        /// <summary>
        /// Update Static Page
        /// </summary>
        /// <param name="staticPage">The Static Page.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        public virtual void UpdateStaticPage(StaticPages staticPage)
        {
            if (staticPage == null)
                throw new ArgumentNullException("staticPage");

            _staticPagesRepository.Update(staticPage);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_STATICPAGES_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(staticPage);
        }



        /// <summary>
        /// Delete StaticPage
        /// </summary>
        /// <param name="staticPage">The Static Page.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        public virtual void DeleteStaticPage(StaticPages staticPage)
        {
            if (staticPage == null)
                throw new ArgumentNullException("staticPage");

            _staticPagesRepository.Delete(staticPage);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGES_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(staticPage);
        }


        #endregion Methods

        #region StaticPageCategory Methods

        /// <summary>
        /// Get All Static Page Categories
        /// </summary>
        /// <returns>StaticPageCategory Collection Item</returns>
        public virtual IList<StaticPageCategory> GetAllStaticPageCategories()
        {
            string key = string.Format(CACHE_STATICPAGECATEGORY_GetAllStaticPageCategories);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _staticPageCategoryRepository.Table
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }


        /// <summary>
        /// Get Static Page Category By Category Id
        /// </summary>
        /// <param name="categoryId">StaticPageCategory Id</param>
        /// <returns>Static Page Item</returns>
        public virtual StaticPageCategory GetStaticPageCategoryById(long categoryId)
        {
            var query = from x in _staticPageCategoryRepository.Table
                where x.Id == categoryId
                        select x;

            var page = query.FirstOrDefault();

            return page;
        }


        /// <summary>
        /// Get Static Page Child Categories
        /// </summary>
        /// <param name="categoryId">StaticPageCategory Id</param>
        /// <returns>Static Page Item</returns>
        public virtual IList<StaticPageCategory> GetStaticPageChildCategories(long categoryId)
        {
            var query = from x in _staticPageCategoryRepository.Table
                where x.ParentId == categoryId
                select x;

            return query.ToList();
        }

        /// <summary>
        /// Insert Static Page Category
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">staticPageCategory</exception>
        public virtual long InsertStaticPageCategory(StaticPageCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("staticPageCategory");

            _staticPageCategoryRepository.Insert(category);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGECATEGORY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(category);

            return category.Id;
        }


        /// <summary>
        /// Update Static Page Category
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <exception cref="ArgumentNullException">staticPageCategory</exception>
        public virtual void UpdateStaticPageCategory(StaticPageCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("staticPageCategory");

            _staticPageCategoryRepository.Update(category);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_STATICPAGECATEGORY_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(category);
        }



        /// <summary>
        /// Delete StaticPageCategory
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <exception cref="ArgumentNullException">staticPage</exception>
        public virtual void DeleteStaticPageCategory(StaticPageCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("staticPageCategory");

            _staticPageCategoryRepository.Delete(category);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGECATEGORY_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(category);
        }

        #endregion StaticPageCategory


        #region StaticPageCategoryRelation Methods

        /// <summary>
        /// Get All Static Page Category Relations
        /// </summary>
        /// <param name="pageId">Page Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <returns>StaticPageCategoryRelation Collection Item</returns>
        public virtual IList<StaticPageCategoryRelation> GetAllStaticPageCategoryRelations(long pageId = 0, long categoryId = 0)
        {
            string key = string.Format(CACHE_STATICPAGECATEGORYRELATION_GetAllStaticPageCategoryRelations);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _staticPageCategoryRelationRepository.Table
                    where (pageId == 0 || x.PageId == pageId) && (categoryId == 0 || x.CategoryId == categoryId)
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }

        /// <summary>
        /// Get StaticPageCategoryRelation
        /// </summary>
        /// <param name="pageId">Page Id</param>
        /// <param name="categoryId">Category Id</param>
        /// <returns>StaticPageCategoryRelation Item</returns>
        public virtual StaticPageCategoryRelation GetStaticPageCategoryRelation(long pageId, long categoryId)
        {
            string key = string.Format(CACHE_STATICPAGECATEGORYRELATION_GetStaticPageCategoryRelation, pageId, categoryId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _staticPageCategoryRelationRepository.Table
                    where x.PageId == pageId && x.CategoryId == categoryId
                    orderby x.Id descending
                    select x;

                return query.FirstOrDefault();
            });
        }

        /// <summary>
        /// Insert StaticPage Category Relation
        /// </summary>
        /// <param name="pageCategoryRelation">The page category relation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">pageCategoryRelation</exception>
        public virtual long InsertStaticPageCategoryRelation(StaticPageCategoryRelation pageCategoryRelation)
        {
            if (pageCategoryRelation == null)
                throw new ArgumentNullException("pageCategoryRelation");

            _staticPageCategoryRelationRepository.Insert(pageCategoryRelation);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGECATEGORYRELATION_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(pageCategoryRelation);

            return pageCategoryRelation.Id;
        }

        /// <summary>
        /// Delete StaticPageCategory Relation
        /// </summary>
        /// <param name="pageCategoryRelation">The page category relation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">pageCategoryRelation</exception>
        public virtual void DeleteStaticPageCategoryRelation(StaticPageCategoryRelation pageCategoryRelation)
        {
            if (pageCategoryRelation == null)
                throw new ArgumentNullException("pageCategoryRelation");

            _staticPageCategoryRelationRepository.Delete(pageCategoryRelation);

            _cacheManager.RemoveByPattern(CACHE_STATICPAGECATEGORYRELATION_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(pageCategoryRelation);
        }

        #endregion StaticPageCategoryRelation 
    }



}