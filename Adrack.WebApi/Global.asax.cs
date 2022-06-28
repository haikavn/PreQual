using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Helpers;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Agent;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using Adrack.Web.Framework.Mvc.Route;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Infrastructure.Constants;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Infrastructure.Web.Attributes;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentValidation.Mvc;

namespace Adrack.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected bool IsInitialized { get; set; } = false;

        /// <summary>
        ///    Application Start method
        /// </summary>
        protected void Application_Start()
        {
            // Initialize Engine Context
            AppEngineContext.Initialize(false);
            
            // Add Functionality On Top Of The Default Model Metadata Provider
            ModelMetadataProviders.Current = new AppMetadataProvider();

            // GlobalConfiguration. Set the WebApi DependencyResolver
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = 
                new AutofacWebApiDependencyResolver(AppEngineContext.Current.ContainerManager.Container);
           GlobalConfiguration.Configuration.Filters.Add(new ValidateModelAttribute());

            // GlobalConfiguration.Configuration.Formatters.Add(GlobalConfiguration.Configuration.Formatters.JsonFormatter);
            var jsonContentTypeEnabled = ConfigurationManager.AppSettings["JsonContentTypeEnabled"];
            if (!string.IsNullOrEmpty(jsonContentTypeEnabled) && jsonContentTypeEnabled.ToLower() == "true")
            {
                GlobalConfiguration.Configuration.Formatters.Remove(
                    GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            }

            GlobalConfiguration.Configuration.Formatters.Insert(0, new TextMediaTypeFormatter());

            AreaRegistration.RegisterAllAreas();
            Application_RegisterRoutes(RouteTable.Routes);

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(
                new FluentValidationModelValidatorProvider(new AppValidatorFactory()));

            //Initialize();

            // FilterConfig, BundleConfig
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbContextService.Instance = new DbContextService();
            //WebApiApplication.DbContextService = AppEngineContext.Current.Resolve<IDbContextService>();

            string dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];
            string subDomain = "master";

           // WebApiApplication.DbContextService.AddClientContext(subDomain, subDomain, string.Format(dbConnectionString, subDomain));


            //this.EndRequest += WebApiApplication_EndRequest;
        }

        private void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            // Start Schedule Tasks Agent
            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();

            var settingService = AppEngineContext.Current.Resolve<ISettingService>();

            var setting = settingService.GetSetting(SettingConstants.AutoCacheMode);
            MemoryCacheManager.EnableAutoCacheMode = false;
            if (setting != null)
            {
                short.TryParse(setting.Value, out var dm);
                MemoryCacheManager.EnableAutoCacheMode = dm == SettingConstants.IsAutoCacheModeEnabled;
            }

            setting = settingService.GetSetting(SettingConstants.AutoCacheUrls);
            MemoryCacheManager.AutoCacheUrls = (setting?.Value ?? string.Empty);

            setting = settingService.GetSetting(SettingConstants.AppSettingUrl);
            if (setting == null)
            {
                setting = new Setting
                {
                    Key = SettingConstants.AppSettingUrl,
                    Value = SettingConstants.AppSettingUrlValue
                };
                settingService.InsertSetting(setting);
            }
            else if (string.IsNullOrEmpty(setting.Value))
            {
                setting.Value = SettingConstants.AppSettingUrlValue;
                settingService.UpdateSetting(setting);
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
        ///     Application BeginRequest
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var uriObject = app.Context.Request.Url;

            var context = new HttpContextWrapper(app.Context);
            app.Context.Request.Headers["UniqueRequestId"] = Guid.NewGuid().ToString();
           
            RouteData routeData = RouteTable.Routes.GetRouteData(context);
            string action = routeData.Values["action"] as string;
            string controller = routeData.Values["controller"] as string;

            if (uriObject.AbsoluteUri.Contains("/api/") || 
                uriObject.AbsoluteUri.ToLower().Contains("/import") ||
                uriObject.AbsoluteUri.ToLower().Contains("/navigate"))
            {
                var appContext = AppEngineContext.Current.Resolve<IAppContext>();

                if (appContext.AppUser != null)
                {
                    app.Context.Request.Headers["CurrentUserId"] = appContext.AppUser.Id.ToString();
                }
                if (!SetupConnectionString(context.Request))
                    Response.Redirect("https://adrack.com/lead-distribution/");
            }

            //WebApiApplication.DbContextService
            //Initialize();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var uriObject = app.Context.Request.Url;

            var context = new HttpContextWrapper(app.Context);
            RouteData routeData = RouteTable.Routes.GetRouteData(context);
            string action = routeData.Values["action"] as string;
            string controller = routeData.Values["controller"] as string;

            if (uriObject.AbsoluteUri.Contains("/api/") || 
                uriObject.AbsoluteUri.ToLower().Contains("/import") ||
                uriObject.AbsoluteUri.ToLower().Contains("/navigate"))
                SetupConnectionString(context.Request, false);
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
            Application_ErrorRoutes(exception);
        }

        #region Custom Application
        /// <summary>
        ///     Check request is for static resource
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static bool IsStaticResource(HttpRequest request)
        {
            // Common Helper
            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            return commonHelper.IsStaticResource(request);
        }

        /// <summary>
        ///     Application Error Routes
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void Application_ErrorRoutes(Exception exception)
        {
            if (IsStaticResource(Request))
                return;

            // Get HTTP Errors
            if (!(exception is HttpException httpException))
                httpException = new HttpException((int)ResponseStatusCode.InternalServerError, "Internal Server Error", exception);

            Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");

            switch ((ResponseStatusCode)httpException.GetHttpCode())
            {
                case ResponseStatusCode.Unauthorized:
                    routeData.Values.Add("action", "PageUnauthorized");
                    break;

                case ResponseStatusCode.Forbidden:
                    routeData.Values.Add("action", "PageForbidden");
                    break;

                case ResponseStatusCode.PageNotFound:
                    routeData.Values.Add("action", "PageNotFound");
                    break;

                case ResponseStatusCode.InternalServerError:
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
        ///     Ignore Request if it is static resource or ping for keep alive
        /// </summary>
        /// <returns></returns>
        private bool Application_IgnoreRequest()
        {
            // Ignore Static Resources
            var commonHelper = AppEngineContext.Current.Resolve<ICommonHelper>();

            if (commonHelper.IsStaticResource(Request))
                return true;

            // Ping Keep Alive Page Requested (ignore it to prevent creating a guest user records)
            var keepAliveUrl = $"{commonHelper.GetAppLocation()}ping/index";

            return commonHelper.GetPageUrl(false)
                .StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Application Culture
        /// </summary>
        protected void Application_Culture()
        {
            if (Application_IgnoreRequest())
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

                //log.Error(exception.Message, exception, appContext.AppUser);
            }
            catch (Exception)
            {
                // Don't Throw New Exception If Occurs
            }
        }

        public bool SetupConnectionString(HttpRequestBase request, bool addContext = true)
        {
            //IDbContext _dbContext = AppEngineContext.Current.Resolve<IDbContext>();
            //ICacheManager cacheManager = AppEngineContext.Current.Resolve<ICacheManager>();
            string dataConnectionDefaultDatabase = ConfigurationManager.AppSettings["DataConnectionDefaultDatabase"];
            string subDomain = WebHelper.GetSubdomain(request);

            /*var masterContext = WebApiApplication.DbContextService.GetClientContext("master");
            if (masterContext != null)
            {
                var dbExistsResults = masterContext.SqlQuery<List<string>>($"select [name] as database_name from sys.databases where [name] = {subDomain} order by name");
                if (dbExistsResults.Count() > 0)
                {
                    var dbExistsResult = dbExistsResults.ElementAt(0);
                    return false;
                }
            }*/

            if (string.IsNullOrEmpty(subDomain))
            {
                subDomain = dataConnectionDefaultDatabase;
            }

            string dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];

            /*string dbConnectionString = Environment.GetEnvironmentVariable("DataConnectionString");

            if (string.IsNullOrEmpty(dbConnectionString))
            {
                var dbConnectionStringSetting = ConfigurationManager.ConnectionStrings["DataConnectionString"];
                if (dbConnectionStringSetting != null)
                    dbConnectionString = dbConnectionStringSetting.ConnectionString;
            }

            if (string.IsNullOrEmpty(dbConnectionString))
                dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];*/

            if (!string.IsNullOrEmpty(dbConnectionString))
            {
                //cacheManager.Set("System.DataConnectionDefaultDatabase", dataConnectionDefaultDatabase, 60);
                //_dbContext.ConnectionString = string.Format(_dbContext.ConnectionString, subDomain);
                if (addContext)
                    DbContextService.Instance.AddClientContext(subDomain, $"{subDomain}-{request.Headers["UniqueRequestId"]}", string.Format(dbConnectionString, subDomain));
                    //DbContextService.Instance.AddClientContext(subDomain, subDomain, string.Format(dbConnectionString, subDomain));
                else
                    DbContextService.Instance.RemoveClientContext($"{subDomain}-{request.Headers["UniqueRequestId"]}");
                    //DbContextService.Instance.RemoveClientContext(subDomain);
            }

            request.Headers["AdrackSubDomain"] = subDomain;

            return true;
        }

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
                new[] { "Adrack.WebApi.Controllers" });
        }

        #endregion Application Register Routes

        #endregion Custom Application
    }
}
