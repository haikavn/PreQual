// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AppHttpsRequirementAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Security
{
    /// <summary>
    /// Represents a Application Https Requirement Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AppHttpsRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region Constructor

        /// <summary>
        /// Application Https Requirement Attribute
        /// </summary>
        /// <param name="sslRequirement">Secure Sockets Layer Requirement</param>
        public AppHttpsRequirementAttribute(SslRequirement sslRequirement)
        {
            this.SslRequirement = sslRequirement;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="authorizationContext">AuthorizationContext</param>
        /// <exception cref="ArgumentNullException">authorizationContext</exception>
        /// <exception cref="AppException">Not supported SslProtected parameter</exception>
        public virtual void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
                throw new ArgumentNullException("authorizationContext");

            if (authorizationContext.IsChildAction)
                return;

            if (!String.Equals(authorizationContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            
            var securitySetting = AppEngineContext.Current.Resolve<SecuritySetting>();

            var appSetting = AppEngineContext.Current.Resolve<AppSetting>();
        
            

            if (securitySetting.SslRequiredForAllPages)
                this.SslRequirement = SslRequirement.Yes;

            
            appSetting.Url = "https://"+authorizationContext.HttpContext.Request.Url.Host;
            appSetting.Host = authorizationContext.HttpContext.Request.Url.Host;

            if (appSetting.Host=="localhost")
                appSetting.SslEnabled = false;

            //In case if SSL is required please uncomment this line
            else
                appSetting.SslEnabled = true;

            this.SslRequirement = SslRequirement.Yes;

            switch (this.SslRequirement)
            {
                case SslRequirement.Yes:
                    {
                        var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

                        var connectionSecured = commonHelper.IsConnectionSecured();

                        if (!connectionSecured)
                        {
                            //appSetting = AppEngineContext.Current.Resolve<AppSetting>();

                            if (appSetting.SslEnabled)
                            {
                                string pageUrl = appSetting.Url;

                                authorizationContext.Result = new RedirectResult(pageUrl, true);
                            }
                        }
                    }
                    break;

                case SslRequirement.No:
                    {
                        var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

                        var connectionSecured = commonHelper.IsConnectionSecured();

                        if (connectionSecured)
                        {
                            string pageUrl = commonHelper.GetPageUrl(true, false);

                            authorizationContext.Result = new RedirectResult(pageUrl, true);
                        }
                    }
                    break;

                case SslRequirement.NoMatter:
                    {
                        //do nothing
                    }
                    break;

                default:
                    throw new AppException("Not supported SslProtected parameter");
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Secure Sockets Layer Requirement
        /// </summary>
        /// <value>The SSL requirement.</value>
        public SslRequirement SslRequirement { get; set; }

        #endregion Properties
    }
}