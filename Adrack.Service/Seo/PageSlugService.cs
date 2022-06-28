// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PageSlugService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Seo;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Seo
{
    /// <summary>
    /// Represents a Page Slug Service
    /// Implements the <see cref="Adrack.Service.Seo.IPageSlugService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Seo.IPageSlugService" />
    public partial class PageSlugService : IPageSlugService
    {
        #region Nested Class

        /// <summary>
        /// Represents a Cache Page Slug
        /// </summary>
        [Serializable]
        public class CachePageSlug
        {
            #region Properties

            /// <summary>
            /// Gets or Sets the Page Slug Identifier
            /// </summary>
            /// <value>The identifier.</value>
            public long Id { get; set; }

            /// <summary>
            /// Gets or Sets the Language Identifier
            /// </summary>
            /// <value>The language identifier.</value>
            public long LanguageId { get; set; }

            /// <summary>
            /// Gets or Sets the Entity Identifier
            /// </summary>
            /// <value>The entity identifier.</value>
            public long EntityId { get; set; }

            /// <summary>
            /// Gets or Sets the Entity Name
            /// </summary>
            /// <value>The name of the entity.</value>
            public string EntityName { get; set; }

            /// <summary>
            /// Gets or Sets the Name
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or Sets the Active
            /// </summary>
            /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
            public bool Active { get; set; }

            #endregion Properties
        }

        #endregion Nested Class

        #region Constants

        /// <summary>
        /// Cache Page Slug By Id Key
        /// </summary>
        private const string CACHE_PAGESLUG_BY_ID_KEY = "App.Cache.PageSlug.By.Id-{0}";

        /// <summary>
        /// Cache Page Slug By Name Key
        /// </summary>
        private const string CACHE_PAGESLUG_BY_NAME_KEY = "App.Cache.PageSlug.By.Name-{0}";

        /// <summary>
        /// Cache Page Slug By Language Id, Entity Id,  Entity Name Key
        /// </summary>
        private const string CACHE_PAGESLUG_BY_LANGUAGE_ID_ENTITY_ID_ENTITY_NAME_KEY = "App.Cache.PageSlug.By.Language.Id-Entity.Id-Entity.Name-{0}-{1}-{2}";

        /// <summary>
        /// Cache Page Slug All Key
        /// </summary>
        private const string CACHE_PAGESLUG_ALL_KEY = "App.Cache.PageSlug.All";

        /// <summary>
        /// Cache Page Slug Pattern Key
        /// </summary>
        private const string CACHE_PAGESLUG_PATTERN_KEY = "App.Cache.PageSlug.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Localization Setting
        /// </summary>
        private readonly LocalizationSetting _localizationSetting;

        /// <summary>
        /// Page Slug
        /// </summary>
        private readonly IRepository<PageSlug> _pageSlugRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get All Cache Page Slug
        /// </summary>
        /// <returns>Cache Page Slug Item</returns>
        protected virtual IList<CachePageSlug> GetAllCachePageSlug()
        {
            string key = string.Format(CACHE_PAGESLUG_ALL_KEY);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _pageSlugRepository.Table
                            select x;

                var pageSlug = query.ToList();

                var _cachePageSlug = new List<CachePageSlug>();

                foreach (var value in pageSlug)
                {
                    var cachePageSlug = new CachePageSlug()
                    {
                        Id = value.Id,
                        LanguageId = value.LanguageId,
                        EntityId = value.EntityId,
                        EntityName = value.EntityName,
                        Name = value.Name,
                        Active = value.Active
                    };

                    _cachePageSlug.Add(cachePageSlug);
                }

                return _cachePageSlug;
            });
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Page Slug Service
        /// </summary>
        /// <param name="localizationSetting">Localization Setting</param>
        /// <param name="pageSlugRepository">PageSlug Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        public PageSlugService(LocalizationSetting localizationSetting, IRepository<PageSlug> pageSlugRepository, ICacheManager cacheManager)
        {
            this._localizationSetting = localizationSetting;
            this._pageSlugRepository = pageSlugRepository;
            this._cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Page Slug By Id
        /// </summary>
        /// <param name="pageSlugId">Page Slug Identifier</param>
        /// <returns>Page Slug Item</returns>
        public virtual PageSlug GetPageSlugById(long pageSlugId)
        {
            if (pageSlugId == 0)
                return null;

            string key = string.Format(CACHE_PAGESLUG_BY_ID_KEY, pageSlugId);

            return _cacheManager.Get(key, () => { return _pageSlugRepository.GetById(pageSlugId); });
        }

        /// <summary>
        /// Get Page Slug By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Page Slug Item</returns>
        public virtual PageSlug GetPageSlugByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            string key = string.Format(CACHE_PAGESLUG_BY_NAME_KEY, name);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _pageSlugRepository.Table
                            where x.Name == name
                            select x;

                var pageSlugs = query.FirstOrDefault();

                return pageSlugs;
            });
        }

        /// <summary>
        /// Get Cache Page Slug By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Cache Page Slug Item</returns>
        public virtual CachePageSlug GetCachePageSlugByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            if (_localizationSetting.LoadAllPageSlugsOnStartup)
            {
                var query = from x in GetAllCachePageSlug()
                            where x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                            select x;

                var pageSlugs = query.FirstOrDefault();

                return pageSlugs;
            }
            else
            {
                string key = string.Format(CACHE_PAGESLUG_BY_NAME_KEY, name);

                return _cacheManager.Get(key, () =>
                {
                    var pageSlug = GetPageSlugByName(name);

                    if (pageSlug == null)
                        return null;

                    var cachePageSlug = new CachePageSlug()
                    {
                        Id = pageSlug.Id,
                        LanguageId = pageSlug.LanguageId,
                        EntityId = pageSlug.EntityId,
                        EntityName = pageSlug.EntityName,
                        Name = pageSlug.Name,
                        Active = pageSlug.Active
                    };

                    return cachePageSlug;
                });
            }
        }

        /// <summary>
        /// Get Active Page Slug
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="entityName">Entity Name</param>
        /// <returns>System.String.</returns>
        public virtual string GetActivePageSlug(long languageId, long entityId, string entityName)
        {
            if (_localizationSetting.LoadAllPageSlugsOnStartup)
            {
                string key = string.Format(CACHE_PAGESLUG_BY_LANGUAGE_ID_ENTITY_ID_ENTITY_NAME_KEY, languageId, entityId, entityName);

                return _cacheManager.Get(key, () =>
                {
                    var query = from x in GetAllCachePageSlug()
                                where x.LanguageId == languageId &&
                                      x.EntityId == entityId &&
                                      x.EntityName == entityName &&
                                      x.Active
                                orderby x.Id descending
                                select x.Name;

                    var pageSlug = query.FirstOrDefault();

                    if (pageSlug == null)
                        pageSlug = "";

                    return pageSlug;
                });
            }
            else
            {
                string key = string.Format(CACHE_PAGESLUG_BY_LANGUAGE_ID_ENTITY_ID_ENTITY_NAME_KEY, languageId, entityId, entityName);

                return _cacheManager.Get(key, () =>
               {
                   var query = from x in _pageSlugRepository.Table
                               where x.LanguageId == languageId &&
                                     x.EntityId == entityId &&
                                     x.EntityName == entityName &&
                                     x.Active
                               orderby x.Id descending
                               select x.Name;

                   var pageSlug = query.FirstOrDefault();

                   if (pageSlug == null)
                       pageSlug = "";

                   return pageSlug;
               });
            }
        }

        /// <summary>
        /// Get All Page Slugs
        /// </summary>
        /// <returns>Page Slug Collection Item</returns>
        public virtual IList<PageSlug> GetAllPageSlugs()
        {
            string key = CACHE_PAGESLUG_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _pageSlugRepository.Table
                            orderby x.EntityName, x.Id
                            select x;

                var pageSlugs = query.ToList();

                return pageSlugs;
            });
        }

        /// <summary>
        /// Get All Page Slugs
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Page Slug Collection Item</returns>
        public virtual IPagination<PageSlug> GetAllPageSlugs(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pageSlugRepository.Table;

            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            query = query.OrderBy(x => x.Name);

            var pageSlugs = new Pagination<PageSlug>(query, pageIndex, pageSize);

            return pageSlugs;
        }

        /// <summary>
        /// Insert Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        /// <exception cref="ArgumentNullException">pageSlug</exception>
        public virtual void InsertPageSlug(PageSlug pageSlug)
        {
            if (pageSlug == null)
                throw new ArgumentNullException("pageSlug");

            _pageSlugRepository.Insert(pageSlug);

            _cacheManager.RemoveByPattern(CACHE_PAGESLUG_PATTERN_KEY);
        }

        /// <summary>
        /// Save Page Slug
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="name">Name</param>
        /// <param name="languageId">Language Identifier</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        public virtual void SavePageSlug<T>(T entity, string name, long languageId) where T : BaseEntity, IPageSlugSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            long entityId = entity.Id;
            string entityName = typeof(T).Name;

            var query = from x in _pageSlugRepository.Table
                        where x.LanguageId == languageId &&
                              x.EntityId == entityId &&
                              x.EntityName == entityName &&
                              x.Active
                        orderby x.Id descending
                        select x;

            var allPageSlugs = query.ToList();
            var activePageSlug = allPageSlugs.FirstOrDefault(x => x.Active);

            if (activePageSlug == null && !string.IsNullOrWhiteSpace(name))
            {
                var nonActivePageSlugWithSpecifiedName = allPageSlugs.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !x.Active);

                if (nonActivePageSlugWithSpecifiedName != null)
                {
                    nonActivePageSlugWithSpecifiedName.Active = true;
                    UpdatePageSlug(nonActivePageSlugWithSpecifiedName);
                }
                else
                {
                    var pageSlug = new PageSlug()
                    {
                        LanguageId = languageId,
                        EntityId = entityId,
                        EntityName = entityName,
                        Name = name,
                        Active = true,
                    };

                    InsertPageSlug(pageSlug);
                }
            }

            if (activePageSlug != null && string.IsNullOrWhiteSpace(name))
            {
                activePageSlug.Active = false;
                UpdatePageSlug(activePageSlug);
            }

            if (activePageSlug != null && !string.IsNullOrWhiteSpace(name))
            {
                if (activePageSlug.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    //
                }
                else
                {
                    var nonActivePageSlugWithSpecifiedName = allPageSlugs.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !x.Active);

                    if (nonActivePageSlugWithSpecifiedName != null)
                    {
                        nonActivePageSlugWithSpecifiedName.Active = true;
                        UpdatePageSlug(nonActivePageSlugWithSpecifiedName);
                    }
                    else
                    {
                        var pageSlug = new PageSlug()
                        {
                            LanguageId = languageId,
                            EntityId = entityId,
                            EntityName = entityName,
                            Name = name,
                            Active = true,
                        };

                        nonActivePageSlugWithSpecifiedName.Active = false;
                        UpdatePageSlug(pageSlug);
                    }
                }
            }
        }

        /// <summary>
        /// Update Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        /// <exception cref="ArgumentNullException">pageSlug</exception>
        public virtual void UpdatePageSlug(PageSlug pageSlug)
        {
            if (pageSlug == null)
                throw new ArgumentNullException("pageSlug");

            _pageSlugRepository.Update(pageSlug);

            _cacheManager.RemoveByPattern(CACHE_PAGESLUG_PATTERN_KEY);
        }

        /// <summary>
        /// Delete Page Slug
        /// </summary>
        /// <param name="pageSlug">Page Slug</param>
        /// <exception cref="ArgumentNullException">pageSlug</exception>
        public virtual void DeletePageSlug(PageSlug pageSlug)
        {
            if (pageSlug == null)
                throw new ArgumentNullException("pageSlug");

            _pageSlugRepository.Delete(pageSlug);

            _cacheManager.RemoveByPattern(CACHE_PAGESLUG_PATTERN_KEY);
        }

        #endregion Methods
    }
}