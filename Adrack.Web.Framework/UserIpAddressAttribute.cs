// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="UserIpAddressAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Service.Membership;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a User IpAddress Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class UserIpAddressAttribute : ActionFilterAttribute
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

            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            // Update IP Address
            string ipAddress = commonHelper.GetIpAddress();

            if (!String.IsNullOrEmpty(ipAddress))
            {
                var appContext = AppEngineContext.Current.Resolve<IAppContext>();

                var appUser = appContext.AppUser;
                if (appUser != null)
                {
                    if (!ipAddress.Equals(appUser.IpAddress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var userService = AppEngineContext.Current.Resolve<IUserService>();

                        appUser.IpAddress = ipAddress;
                        userService.UpdateUser(appUser);
                    }
                }
            }
        }

        #endregion Methods
    }
}