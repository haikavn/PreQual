// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SiteMaintenanceAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Site Maintenance Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class SiteMaintenanceAttribute : ActionFilterAttribute
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

            string actionName = actionExecutingContext.ActionDescriptor.ActionName;

            if (String.IsNullOrEmpty(actionName))
                return;

            string controllerName = actionExecutingContext.Controller.ToString();

            if (String.IsNullOrEmpty(controllerName))
                return;

            if (actionExecutingContext.IsChildAction)
                return;

            var appSetting = AppEngineContext.Current.Resolve<AppSetting>();

            if (!appSetting.SiteMaintenance)
                return;

            if (!(controllerName.Equals("Adrack.Web.Controllers.MembershipController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Login", StringComparison.InvariantCultureIgnoreCase)) &&
                !(controllerName.Equals("Adrack.Web.Controllers.MembershipController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Logout", StringComparison.InvariantCultureIgnoreCase)) &&
                !(controllerName.Equals("Adrack.Web.Controllers.CommonController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("SiteMaintenance", StringComparison.InvariantCultureIgnoreCase)))
            {
                var appContext = AppEngineContext.Current.Resolve<IAppContext>().AppUser;

                if (appContext.IsGlobalAdministrator())
                {
                    // Content Management Page
                }
                else
                {
                    var siteMaintenance = new UrlHelper(actionExecutingContext.RequestContext).RouteUrl("SiteMaintenance");

                    actionExecutingContext.Result = new RedirectResult(siteMaintenance);
                }
            }
        }

        #endregion Methods
    }
}