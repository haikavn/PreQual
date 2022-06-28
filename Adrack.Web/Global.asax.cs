// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Infrastructure;
using Adrack.Service.Agent;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Web.Controllers;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using Adrack.Web.Framework.Mvc.Route;
using Adrack.Web.Framework.ViewEngines.Razor;
using FluentValidation.Mvc;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web
{
    /// <summary>
    ///     Represents a Global Application
    ///     Implements the <see cref="HttpApplication" />
    /// </summary>
    /// <seealso cref="HttpApplication" />
    public class MvcApplication : HttpApplication
    {
        #region Application Register Routes

        /// <summary>
        ///     Application Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        public static void Application_RegisterRoutes(RouteCollection routeCollection)
        {
            routeCollection.IgnoreRoute("favicon.ico");
            routeCollection.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // When Debug
            routeCollection.IgnoreRoute("{*browserlink}", new { browserlink = @".*__browserLink.*" });

            var routePublisher = AppEngineContext.Current.Resolve<IRoutePublisher>();

            routePublisher.RegisterRoutes(routeCollection);

            routeCollection.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });
        }

        #endregion Application Register Routes

        /// <summary>
        ///     Application Start
        /// </summary>
        protected void Application_Start()
        {
            // Disable IIS Information Request
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider
            ModelMetadataProviders.Current = new AppMetadataProvider();

            // Registering Rebular MVC
            AreaRegistration.RegisterAllAreas();
            Application_RegisterRoutes(RouteTable.Routes);

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(
                new FluentValidationModelValidatorProvider(new AppValidatorFactory()));

            // Start Schedule Tasks Agent
            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();

            var settingService = AppEngineContext.Current.Resolve<ISettingService>();

            var settting = settingService.GetSetting("System.AutoCacheMode");
            MemoryCacheManager.EnableAutoCacheMode = false;
            if (settting != null)
            {
                short dm;
                short.TryParse(settting.Value, out dm);
                MemoryCacheManager.EnableAutoCacheMode = dm == 1 ? true : false;
            }

            settting = settingService.GetSetting("System.AutoCacheUrls");
            MemoryCacheManager.AutoCacheUrls = (settting == null ? "" : settting.Value);

            /*            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                        if (appUrl != "/") appUrl += "/";
                        var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, appUrl);*/

            settting = settingService.GetSetting("AppSetting.Url");
            if (settting == null)
            {
                settting = new Setting();
                settting.Key = "AppSetting.Url";
                settting.Value = "/";
                settingService.InsertSetting(settting);
            }
            else if (string.IsNullOrEmpty(settting.Value))
            {
                settting.Value = "/";
                settingService.UpdateSetting(settting);
            }

            try
            {
                // Log Service
                var logService = AppEngineContext.Current.Resolve<ILogService>();

                logService.Information("Application Started", null, null);
            }
            catch (Exception)
            {
                // Don't throw new exception if occurs
            }
        }

        /// <summary>
        ///     Session Start
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Session_Start(object sender, EventArgs e)
        {
            //
        }

        /// <summary>
        ///     Application BeginRequest
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Ignore Static Resources
            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            if (commonHelper.IsStaticResource(Request))
                return;

            // Ping Keep Alive Page Requested (we ignore it to prevent creating a guest user records)
            var keepAliveUrl = string.Format("{0}ping/index", commonHelper.GetAppLocation());

            if (commonHelper.GetPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;
        }

        /// <summary>
        ///     Application EndRequest
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //
        }

        /// <summary>
        ///     Application Authenticate Request
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Application_Culture();
        }

        /// <summary>
        ///     Application Error
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            // Log Error
            Application_LogException(exception);

            // Error Routes
            //Application_ErrorRoutes(exception);
        }

        /// <summary>
        ///     Session End
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Session_End(object sender, EventArgs e)
        {
            //
        }

        /// <summary>
        ///     Application End
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_End(object sender, EventArgs e)
        {
            //
        }

        #region Custom Application

        /// <summary>
        ///     Application Error Routes
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void Application_ErrorRoutes(Exception exception)
        {
            // Get HTTP Errors
            var httpException = exception as HttpException;

            // Common Helper
            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            if (commonHelper.IsStaticResource(Request))
                return;

            if (httpException == null)
                httpException = new HttpException(500, "Internal Server Error", exception);

            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");

            switch (httpException.GetHttpCode())
            {
                case 401:
                    routeData.Values.Add("action", "PageUnauthorized");
                    break;

                case 403:
                    routeData.Values.Add("action", "PageForbidden");
                    break;

                case 404:
                    routeData.Values.Add("action", "PageNotFound");
                    break;

                case 500:
                    routeData.Values.Add("action", "PageInternalServerError");
                    break;

                default:
                    routeData.Values.Add("action", "PageError");
                    routeData.Values.Add("statusCode", httpException.GetHttpCode());
                    break;
            }

            IController errorCommonController = AppEngineContext.Current.Resolve<CommonController>();

            errorCommonController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        /// <summary>
        ///     Application Culture
        /// </summary>
        protected void Application_Culture()
        {
            // Ignore Static Resources
            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            if (commonHelper.IsStaticResource(Request))
                return;

            // Ping Keep Alive Page Requested
            var keepAliveUrl = string.Format("{0}ping/index", commonHelper.GetAppLocation());

            if (commonHelper.GetPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;

            CommonHelper.SetCulture();
        }

        /// <summary>
        ///     Application Log Exception
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void Application_LogException(Exception exception)
        {
            if (exception == null)
                return;

            try
            {
                // Log
                var log = AppEngineContext.Current.Resolve<ILogService>();

                var appContext = AppEngineContext.Current.Resolve<IAppContext>();

                log.Error(exception.Message, exception, appContext.AppUser);
            }
            catch (Exception)
            {
                // Don't Throw New Exception If Occurs
            }
        }

        #endregion Custom Application
    }
}