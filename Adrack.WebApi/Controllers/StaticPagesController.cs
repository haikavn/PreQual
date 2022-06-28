using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/staticpages")]
    public class StaticPagesController : BaseApiController
    {
        private readonly IStaticPagesService _staticPagesService;
        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;

        private readonly IPermissionService _permissionService;
        //private static string _viewStaticPagesGeneralInfoKey { get; set; } = "view-general-info-staticpages";
        //private static string _viewStaticPageCategoryGeneralInfoKey { get; set; } = "view-general-info-staticpagecategory";


        public StaticPagesController(IStaticPagesService staticPagesService,
            IUserService userService,
            ICacheManager cacheManager,
            IPermissionService permissionService)
        {
            _staticPagesService = staticPagesService;
            _userService = userService;
            _cacheManager = cacheManager;
            _permissionService = permissionService;
        }


        #region Methods

        /// <summary>
        /// Get StaticPages List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getStaticPagesList")]
        public IHttpActionResult GetStaticPagesList()
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var pages = _staticPagesService.GetAllStaticPages();
            return Ok(pages);
        }

        [HttpPost]
        [Route("getStaticPageById")]
        public IHttpActionResult GetStaticPageById(long id)
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var currentStaticPage = _staticPagesService.GetStaticPageById(id);
            if (currentStaticPage == null)
                return HttpBadRequest("Static Page not found");

            return Ok(currentStaticPage);
        }

        [HttpPost]
        [Route("addStaticPage")]
        public IHttpActionResult AddStaticPage([FromBody] StaticPages staticPage)
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (staticPage == null)
            {
                return HttpBadRequest("StaticPages model is empty");
            }

            try
            {
                var pageId = _staticPagesService.InsertStaticPage(staticPage);
                return Ok(pageId);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        
        [HttpPost]
        [Route("updateStaticPage")]
        public IHttpActionResult UpdateStaticPage([FromBody] StaticPages staticPage)
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (staticPage == null)
                return HttpBadRequest("StaticPages model is empty");

            var currentStaticPage = _staticPagesService.GetStaticPageById(staticPage.Id);
            if (currentStaticPage == null)
                return HttpBadRequest("Static Page not found");

            try
            {
                currentStaticPage.Title = staticPage.Title ?? currentStaticPage.Title;
                currentStaticPage.Body = staticPage.Body ?? currentStaticPage.Body;
                currentStaticPage.CreatedDate = staticPage.CreatedDate ?? currentStaticPage.CreatedDate; 

                _staticPagesService.UpdateStaticPage(currentStaticPage);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        
        [HttpPost]
        [Route("deleteStaticPage")]
        public IHttpActionResult DeleteStaticPage(long id)
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var currentStaticPage = _staticPagesService.GetStaticPageById(id);
            if (currentStaticPage == null)
                return HttpBadRequest("Static Page not found");

            var relationPageCategory = _staticPagesService.GetAllStaticPageCategoryRelations(id, 0);
            if (relationPageCategory != null && relationPageCategory.Count > 0)
                return HttpBadRequest("This Static Page is linked to the Category");

            try
            {
                _staticPagesService.DeleteStaticPage(currentStaticPage);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        #endregion


        #region StaticPageCategory Methods

        /// <summary>
        /// Get StaticPageCategory List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getStaticPageCategoryList")]
        public IHttpActionResult GetStaticPageCategoryList()
        {
            //if (!_permissionService.Authorize(_viewStaticPageCategoryGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var pages = _staticPagesService.GetAllStaticPageCategories();
            return Ok(pages);
        }


        [HttpPost]
        [Route("getStaticPageCategoryById")]
        public IHttpActionResult GetStaticPageCategoryById(long id)
        {
            //if (!_permissionService.Authorize(_viewStaticPagesGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var currentStaticPageCategory = _staticPagesService.GetStaticPageCategoryById(id);
            if (currentStaticPageCategory == null)
                return HttpBadRequest("Static Page's Category not found");

            return Ok(currentStaticPageCategory);
        }

        [HttpPost]
        [Route("addStaticPageCategory")]
        public IHttpActionResult AddStaticPageCategory([FromBody] StaticPageCategory category)
        {
            //if (!_permissionService.Authorize(_viewStaticPageCategoryGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (category == null)
                return HttpBadRequest("StaticPageCategory model is empty");

            if (category.ParentId != 0)
            {
                var parentCategory = _staticPagesService.GetStaticPageCategoryById(category.ParentId);
                if (parentCategory == null)
                    return HttpBadRequest("Parent Category not found");
            }

            try
            {
                var pageId = _staticPagesService.InsertStaticPageCategory(category);
                return Ok(pageId);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateStaticPageCategory")]
        public IHttpActionResult UpdateStaticPageCategory([FromBody] StaticPageCategory category)
        {
            //if (!_permissionService.Authorize(_viewStaticPageCategoryGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (category == null)
                return HttpBadRequest("StaticPageCategory model is empty");

            var currentCategory = _staticPagesService.GetStaticPageCategoryById(category.Id);
            if (currentCategory == null)
                return HttpBadRequest("Static Page's Category not found");

            if (category.Id == category.ParentId)
                return HttpBadRequest("Parent Category is incorrect");

            if (category.ParentId != 0)
            {
                var parentCategory = _staticPagesService.GetStaticPageCategoryById(category.ParentId);
                if (parentCategory == null)
                    return HttpBadRequest("Parent Category not found");
            }
            
            try
            {
                currentCategory.Name = category.Name ?? currentCategory.Name;
                currentCategory.ParentId = category.ParentId;

                _staticPagesService.UpdateStaticPageCategory(currentCategory);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("deleteStaticPageCategory")]
        public IHttpActionResult DeleteStaticPageCategory(long id)
        {
            //if (!_permissionService.Authorize(_viewStaticPageCategoryGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var currentCategory = _staticPagesService.GetStaticPageCategoryById(id);
            if (currentCategory == null)
                return HttpBadRequest("Static Page's Category not found");

            var relationPageCategory = _staticPagesService.GetAllStaticPageCategoryRelations(0, id);
            if (relationPageCategory != null && relationPageCategory.Count > 0)
                return HttpBadRequest("This Category is linked to the Static Page");

            var childCategories = _staticPagesService.GetStaticPageChildCategories(currentCategory.Id);
            if (childCategories.Count > 0)
                return HttpBadRequest("This Category has child Item(s)");

            try
            {
                _staticPagesService.DeleteStaticPageCategory(currentCategory);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        #endregion StaticPageCategory


        #region StaticPageCategory Relation Methods

        [HttpPost]
        [Route("getStaticPageCategoryRelations")]
        public IHttpActionResult GetStaticPageCategoryRelations(long pageId = 0, long categoryId = 0)
        {
            var currentStaticPageCategoryRelations = _staticPagesService.GetAllStaticPageCategoryRelations(pageId, categoryId);

            return Ok(currentStaticPageCategoryRelations);
        }


        [HttpPost]
        [Route("addStaticPageCategoryRelation")]
        public IHttpActionResult AddStaticPageCategoryRelation([FromBody] StaticPageCategoryRelation pageCategoryRelation)
        {
            if (pageCategoryRelation == null)
                return HttpBadRequest("StaticPageCategoryRelation model is empty");

            var page = _staticPagesService.GetStaticPageById(pageCategoryRelation.PageId);
            if (page == null)
                return HttpBadRequest("This Page not found.");

            var category = _staticPagesService.GetStaticPageCategoryById(pageCategoryRelation.CategoryId);
            if (category == null)
                return HttpBadRequest("This Category not found.");

            var currentPageCategory = _staticPagesService.GetStaticPageCategoryRelation(pageCategoryRelation.PageId, pageCategoryRelation.CategoryId);
            if (currentPageCategory != null)
                return HttpBadRequest("This Category's Page already exists");

            try
            {
                var id = _staticPagesService.InsertStaticPageCategoryRelation(pageCategoryRelation);
                return Ok(id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deleteStaticPageCategoryRelation")]
        public IHttpActionResult DeleteStaticPageCategoryRelation([FromBody] StaticPageCategoryRelation pageCategoryRelation)
        {
            if (pageCategoryRelation == null)
                return HttpBadRequest("StaticPageCategoryRelation model is empty");

            var currentPageCategory = _staticPagesService.GetStaticPageCategoryRelation(pageCategoryRelation.PageId, pageCategoryRelation.CategoryId);
            if (currentPageCategory == null)
                return HttpBadRequest("This Category's Page not found");

            try
            {
                _staticPagesService.DeleteStaticPageCategoryRelation(currentPageCategory);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        #endregion StaticPageCategory Relation Methods

    }
}