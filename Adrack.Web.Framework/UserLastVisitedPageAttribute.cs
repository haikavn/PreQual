// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="UserLastVisitedPageAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a User Last Visited Page Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class UserLastVisitedPageAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="actionExecutingContext">Action Executing Context</param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext == null || actionExecutingContext.HttpContext == null || actionExecutingContext.HttpContext.Request == null)
                return;

            // Don't Apply Filter To Child Methods
            if (actionExecutingContext.IsChildAction)
                return;

            // Only GET Requests
            if (!String.Equals(actionExecutingContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var userSetting = AppEngineContext.Current.Resolve<UserSetting>();

            if (!userSetting.LastVisitedPage)
                return;

            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();
            var pageUrl = commonHelper.GetPageUrl(true);

            if (!String.IsNullOrEmpty(pageUrl))
            {
                var appContext = AppEngineContext.Current.Resolve<IAppContext>();
                var globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();

                if (appContext.AppUser != null)
                {
                    var previousPageUrl = appContext.AppUser.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.LastVisitedPage);

                    if (!pageUrl.Equals(previousPageUrl))
                    {
                        globalAttributeService.SaveGlobalAttribute(appContext.AppUser, GlobalAttributeBuiltIn.LastVisitedPage, pageUrl);
                    }
                }
            }
        }

        #endregion Methods
    }
}