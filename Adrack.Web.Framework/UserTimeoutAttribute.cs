// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="UserTimeoutAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a User Timeout Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserTimeoutAttribute : ActionFilterAttribute
    {
        #region Constants

        /// <summary>
        /// Cache User Cookie Key
        /// </summary>
        private const string CACHE_USER_COOKIE_KEY = "App.Cache.User.Cookie";

        #endregion Constants



        #region Utilities

        /// <summary>
        /// Get User Cookie
        /// </summary>
        /// <returns>Http Cookie Item</returns>
        protected virtual HttpCookie GetUserCookie()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
                return null;

            return HttpContext.Current.Request.Cookies[CACHE_USER_COOKIE_KEY];
        }

        #endregion Utilities



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

            //// Only GET Requests
            //if (!String.Equals(actionExecutingContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            //    return;

            //var userCookie = GetUserCookie();

            //if (userCookie == null)
            //    return;

            //if (String.IsNullOrEmpty(userCookie.Value))
            //    return;

            //if (userCookie.Expires > DateTime.Now)
            //{
            //    var authenticationService = AppEngineContext.Current.Resolve<IAuthenticationService>();

            //    authenticationService.SignOut();

            //    string redirectTo = "~/login";

            //    if (!string.IsNullOrEmpty(actionExecutingContext.HttpContext.Request.RawUrl))
            //    {
            //        redirectTo = string.Format("~/login?returnUrl={0}", HttpUtility.UrlEncode(actionExecutingContext.HttpContext.Request.RawUrl));
            //        actionExecutingContext.Result = new RedirectResult(redirectTo);
            //        return;
            //    }
            //}
        }

        #endregion Methods
    }
}