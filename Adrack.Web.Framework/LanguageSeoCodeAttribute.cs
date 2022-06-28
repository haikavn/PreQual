// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LanguageSeoCodeAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Infrastructure;
using Adrack.Web.Framework.Localization;
using System;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Language Seo Code Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public partial class LanguageSeoCodeAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="actionExecutingContext">Action Executing Context</param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext == null || actionExecutingContext.HttpContext == null)
                return;

            HttpRequestBase httpRequestBaseRequest = actionExecutingContext.HttpContext.Request;

            if (httpRequestBaseRequest == null)
                return;

            if (actionExecutingContext.IsChildAction)
                return;

            if (!String.Equals(actionExecutingContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var localizationSetting = AppEngineContext.Current.Resolve<LocalizationSetting>();

            if (!localizationSetting.SeoFriendlyUrlsForLanguagesEnabled)
                return;

            if (actionExecutingContext.RouteData == null || actionExecutingContext.RouteData.Route == null || !(actionExecutingContext.RouteData.Route is LocalizedRoute))
                return;

            var pageUrl = actionExecutingContext.HttpContext.Request.RawUrl;

            string applicationPath = actionExecutingContext.HttpContext.Request.ApplicationPath;

            if (pageUrl.IsLocalizedUrl(applicationPath, true))
                return;

            var appContext = AppEngineContext.Current.Resolve<IAppContext>();

            pageUrl = pageUrl.AddLanguageSeoCultureToRawUrl(applicationPath, appContext.AppLanguage);

            actionExecutingContext.Result = new RedirectResult(pageUrl, true);
        }

        #endregion Methods
    }
}