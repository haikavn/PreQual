using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace Adrack.Core.Helpers
{
    public static class WebHelper
    {
        #region properties and constants

        private const string HttpContextPropertyName = "MS_HttpContext";

        #endregion properties and constants

        #region extended methods for HttpRequestMessage class

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        public static string GetBaseUrl(this HttpRequestMessage request)
        {
            if (request?.RequestUri == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") { appUrl = $"{appUrl}/"; }

            var baseUrl = $"{request.RequestUri.Scheme}://{request.RequestUri.Authority}{appUrl}";

            return baseUrl;
        }

        /// <summary>
        /// Get host address
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpRequestMessage request)
        {
            request = request ?? new HttpRequestMessage();
            if (request.Properties.ContainsKey(HttpContextPropertyName))
            {
                return ((HttpContextWrapper)request.Properties[HttpContextPropertyName]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        public static string GetSubdomain(HttpRequestBase httpRequestBase)
        {
            string hostName = httpRequestBase.Url?.Host;

            if (string.IsNullOrEmpty(hostName))
                hostName = httpRequestBase.Headers["referring-url"];

            if (string.IsNullOrEmpty(hostName))
                hostName = httpRequestBase.UrlReferrer?.Host;

            if (hostName.Contains("adrack.live"))
            {
                string[] domains = hostName.Split(new char[] { '.' });

                if (domains.Length < 3/* || hostName.Contains("adrack.xyz")*/) return "";

                return domains[0].Replace("-api", "").Replace("qa-", "dev-");
            }
            else
            {
                return GetDefaultDatabase();
            }
        }

        public static string GetSubdomain(HttpRequestMessage httpRequestMessage)
        {
            string hostName = httpRequestMessage.RequestUri?.Host;

            if (string.IsNullOrEmpty(hostName))
                hostName = HttpContext.Current.Request.UrlReferrer?.Host;

            if (hostName.Contains("adrack.live"))
            {
                string[] domains = hostName.Split(new char[] { '.' });

                if (domains.Length < 3/* || hostName.Contains("adrack.xyz")*/) return "";

                return domains[0].Replace("-api", "").Replace("qa-", "dev-");
            }
            else
            {
                return GetDefaultDatabase();
            }
        }

        public static string GetSubdomain()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null)
                    return "";
            }
            catch
            {
                return "";
            }

            string hostName = HttpContext.Current.Request.Url?.Host;

            if (string.IsNullOrEmpty(hostName))
                hostName = HttpContext.Current.Request.Headers["referring-url"];

            if (string.IsNullOrEmpty(hostName))
                hostName = HttpContext.Current.Request.UrlReferrer?.Host;

            if (hostName.Contains("adrack.live"))
            {
                string[] domains = hostName.Split(new char[] { '.' });

                if (domains.Length < 3/* || hostName.Contains("adrack.xyz")*/) return "";

                return domains[0].Replace("-api", "").Replace("qa-", "dev-");
            }
            else
            {
                return GetDefaultDatabase();
            }
        }

        public static string GetCurrentUserId()
        {
            try
            {
                string userId = HttpContext.Current?.Request?.Headers["CurrentUserId"];
                if (userId == null)
                    userId = "";

                return userId;
            }
            catch
            {
                return "";
            }
        }

        public static string GetDefaultDatabase()
        {
            string dbConnectionString = Environment.GetEnvironmentVariable("DataConnectionString");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                var dbConnectionStringSetting = ConfigurationManager.ConnectionStrings["DataConnectionString"];
                if (dbConnectionStringSetting != null)
                    dbConnectionString = dbConnectionStringSetting.ConnectionString;
            }

            if (string.IsNullOrEmpty(dbConnectionString))
                dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];

            if (string.IsNullOrEmpty(dbConnectionString))
                dbConnectionString = "";

            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(dbConnectionString);

            if (!string.IsNullOrEmpty(sqlConnectionStringBuilder.InitialCatalog) && sqlConnectionStringBuilder.InitialCatalog != "{0}")
            {
                return sqlConnectionStringBuilder.InitialCatalog;
            }
            else
            {
                return ConfigurationManager.AppSettings["DataConnectionDefaultDatabase"];
            }
        }

        #endregion extended methods for HttpRequestMessage class
    }
}