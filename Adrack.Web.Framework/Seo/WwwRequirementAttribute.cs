// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="WwwRequirementAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Seo;
using Adrack.Core.Infrastructure;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Seo
{
    /// <summary>
    /// Represents a World Wide Web Requirement Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WwwRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region Methods

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <exception cref="ArgumentNullException">authorizationContext</exception>
        /// <exception cref="AppException">Not supported WwwRequirement parameter</exception>
        public virtual void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
                throw new ArgumentNullException("authorizationContext");

            if (authorizationContext.IsChildAction)
                return;

            if (!String.Equals(authorizationContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (authorizationContext.HttpContext.Request.IsLocal)
                return;

            var seoSetting = AppEngineContext.Current.Resolve<SeoSetting>();

            switch (seoSetting.WwwRequirement)
            {
                case WwwRequirement.WithWww:
                    {
                        var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();
                        string pageUrl = commonHelper.GetPageUrl(true);
                        var connectionSecured = commonHelper.IsConnectionSecured();

                        if (connectionSecured)
                        {
                            bool startsWith3W = pageUrl.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);

                            if (!startsWith3W)
                            {
                                pageUrl = pageUrl.Replace("https://", "https://www.");

                                authorizationContext.Result = new RedirectResult(pageUrl, true);
                            }
                        }
                        else
                        {
                            bool startsWith3W = pageUrl.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);

                            if (!startsWith3W)
                            {
                                pageUrl = pageUrl.Replace("http://", "http://www.");

                                authorizationContext.Result = new RedirectResult(pageUrl, true);
                            }
                        }
                    }
                    break;

                case WwwRequirement.WithoutWww:
                    {
                        var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();
                        string pageUrl = commonHelper.GetPageUrl(true);
                        var connectionSecured = commonHelper.IsConnectionSecured();

                        if (connectionSecured)
                        {
                            bool startsWith3W = pageUrl.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);

                            if (startsWith3W)
                            {
                                pageUrl = pageUrl.Replace("https://www.", "https://");

                                authorizationContext.Result = new RedirectResult(pageUrl, true);
                            }
                        }
                        else
                        {
                            bool startsWith3W = pageUrl.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);

                            if (startsWith3W)
                            {
                                pageUrl = pageUrl.Replace("http://www.", "http://");

                                authorizationContext.Result = new RedirectResult(pageUrl, true);
                            }
                        }
                    }
                    break;

                case WwwRequirement.NoMatter:
                    {
                        // Do Nothing
                    }
                    break;

                default:
                    throw new AppException("Not supported WwwRequirement parameter");
            }
        }

        #endregion Methods
    }
}