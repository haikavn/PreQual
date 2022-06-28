// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="HoneypotValidatorAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Security.Honeypot
{
    /// <summary>
    /// Represents a Honeypot Validator Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HoneypotValidatorAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region Methods

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <exception cref="ArgumentNullException">authorizationContext</exception>
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
                throw new ArgumentNullException("authorizationContext");

            var securitySetting = AppEngineContext.Current.Resolve<SecuritySetting>();

            if (securitySetting.HoneypotEnabled)
            {
                string inputValue = authorizationContext.HttpContext.Request.Form[securitySetting.HoneypotInputName];

                var isBot = !String.IsNullOrWhiteSpace(inputValue);

                if (isBot)
                {
                    var logService = AppEngineContext.Current.Resolve<ILogService>();

                    logService.Warning("A bot detected. Honeypot.");

                    var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

                    string url = commonHelper.GetPageUrl(true);

                    authorizationContext.Result = new RedirectResult(url);
                }
            }
        }

        #endregion Methods
    }
}