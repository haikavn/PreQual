// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CommonHelper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.ComponentModel;
using Adrack.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Common Helper
    /// Implements the <see cref="Adrack.Core.ICommonHelper" />
    /// </summary>
    /// <seealso cref="Adrack.Core.ICommonHelper" />
    public partial class CommonHelper : ICommonHelper
    {
        #region Fields

        /// <summary>
        /// Http Context Base
        /// </summary>
        private readonly HttpContextBase _httpContextBase;

        /// <summary>
        /// Asp Net Hosting Permission Level
        /// </summary>
        private static AspNetHostingPermissionLevel? _aspNetHostingPermissionLevel = null;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Is Request Available
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Boolean Item</returns>
        protected virtual Boolean IsRequestAvailable(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            try
            {
                if (httpContext.Request == null)
                    return false;
            }
            catch (HttpException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Try Write Web Config
        /// </summary>
        /// <returns>Boolean Item</returns>
        protected virtual bool TryWriteWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(GetRootPath("~/Web.config"), DateTime.UtcNow);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try Write Global Asax
        /// </summary>
        /// <returns>Boolean Item</returns>
        protected virtual bool TryWriteGlobalAsax()
        {
            try
            {
                File.SetLastWriteTimeUtc(GetRootPath("~/Global.asax"), DateTime.UtcNow);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Common Helper
        /// </summary>
        /// <param name="httpContextBase">Http Context Base</param>
        public CommonHelper(HttpContextBase httpContextBase)
        {
            this._httpContextBase = httpContextBase;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Ip Address
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GetIpAddress()
        {
            if (!IsRequestAvailable(_httpContextBase))
                return string.Empty;

            var result = "";

            if (_httpContextBase.Request.Headers != null)
            {
                var xForwardedForHttpHeader = "X-FORWARDED-FOR";

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["xForwardedForHTTPheader"]))
                {
                    xForwardedForHttpHeader = ConfigurationManager.AppSettings["xForwardedForHTTPheader"];
                }

                string xForwardedFor = _httpContextBase.Request.Headers.AllKeys
                    .Where(x => xForwardedForHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContextBase.Request.Headers[k])
                    .FirstOrDefault();

                if (!String.IsNullOrEmpty(xForwardedFor))
                {
                    string ipAddress = xForwardedFor.Split(new char[] { ',' }).FirstOrDefault();

                    result = ipAddress;
                }
            }

            if (String.IsNullOrEmpty(result) && _httpContextBase.Request.UserHostAddress != null)
            {
                result = _httpContextBase.Request.UserHostAddress;
            }

            if (result == "::1")
            {
                result = "127.0.0.1";
            }

            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);

                if (index > 0)
                    result = result.Substring(0, index);
            }

            return result;
        }

        /// <summary>
        /// Get Url Referrer
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GetUrlReferrer()
        {
            var referrerUrl = string.Empty;

            if (IsRequestAvailable(_httpContextBase) && _httpContextBase.Request.UrlReferrer != null)
                referrerUrl = _httpContextBase.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        /// <summary>
        /// Is Connection Secured
        /// </summary>
        /// <returns>Bool Item</returns>
        public virtual bool IsConnectionSecured()
        {
            var useSsl = false;

            if (IsRequestAvailable(_httpContextBase))
            {
                useSsl = _httpContextBase.Request.IsSecureConnection;

                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        /// <summary>
        /// Get Page Url
        /// </summary>
        /// <param name="includeQueryString">Include Query String</param>
        /// <returns>String Item</returns>
        public virtual string GetPageUrl(bool includeQueryString)
        {
            var useSsl = IsConnectionSecured();

            return GetPageUrl(includeQueryString, useSsl);
        }

        /// <summary>
        /// Get Page Url
        /// </summary>
        /// <param name="includeQueryString">Include Query String</param>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        public virtual string GetPageUrl(bool includeQueryString, bool useSsl)
        {
            var pageUrl = string.Empty;

            if (!IsRequestAvailable(_httpContextBase))
                return pageUrl;

            if (includeQueryString)
            {
                var appHost = GetAppHost(useSsl);

                if (appHost.EndsWith("/"))
                    appHost = appHost.Substring(0, appHost.Length - 1);

                pageUrl = appHost + _httpContextBase.Request.RawUrl;
            }
            else
            {
                if (_httpContextBase.Request.Url != null)
                {
                    pageUrl = _httpContextBase.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }

            pageUrl = pageUrl.ToLowerInvariant();

            return pageUrl;
        }

        /// <summary>
        /// Get Server Variable
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>String Item</returns>
        public virtual string GetServerVariable(string name)
        {
            var result = string.Empty;

            try
            {
                if (!IsRequestAvailable(_httpContextBase))
                    return result;

                if (_httpContextBase.Request.ServerVariables[name] != null)
                {
                    result = _httpContextBase.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Get Application Host
        /// </summary>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        /// <exception cref="Exception">Application site cannot be loaded</exception>
        public virtual string GetAppHost(bool useSsl)
        {
            var result = "";

            var httpHost = GetServerVariable("HTTP_HOST");

            if (!String.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;

                if (!result.EndsWith("/"))
                    result += "/";
            }

            var appSetting = AppEngineContext.Current.Resolve<AppSetting>();

            if (appSetting == null)
                throw new Exception("Application site cannot be loaded");

            if (!String.IsNullOrWhiteSpace(httpHost))
            {
                result = appSetting.Url;

                if (string.IsNullOrEmpty(result))
                    result = "http://" + httpHost;

                if (!result.EndsWith("/"))
                    result += "/";
            }

            if (useSsl)
            {
                if (!String.IsNullOrWhiteSpace(appSetting.SecureUrl))
                {
                    result = appSetting.SecureUrl;
                }
                else
                {
                    result = result.Replace("http:/", "https:/");
                }
            }
            else
            {
                if (appSetting.SslEnabled && !String.IsNullOrWhiteSpace(appSetting.SecureUrl))
                {
                    result = appSetting.Url;
                }
            }

            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Get Application Location
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string GetAppLocation()
        {
            var useSsl = IsConnectionSecured();

            return GetAppLocation(useSsl);
        }

        /// <summary>
        /// Get Application Location
        /// </summary>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        public virtual string GetAppLocation(bool useSsl)
        {
            var result = GetAppHost(useSsl);

            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);

            if (IsRequestAvailable(_httpContextBase))
                result = result + _httpContextBase.Request.ApplicationPath;

            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Is Static Resource
        /// </summary>
        /// <param name="httpRequest">Http Request</param>
        /// <returns><c>true</c> if [is static resource] [the specified HTTP request]; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">request</exception>
        public virtual bool IsStaticResource(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException("request");

            string rootPath = httpRequest.Path;

            string extension = VirtualPathUtility.GetExtension(rootPath);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get Root Path
        /// </summary>
        /// <param name="rootPath">Root Path</param>
        /// <returns>String Item</returns>
        public virtual string GetRootPath(string rootPath)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(rootPath);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                rootPath = rootPath.Replace("~/", "").TrimStart('/').Replace('/', '\\');

                return Path.Combine(baseDirectory, rootPath);
            }
        }

        /// <summary>
        /// Modify Query String
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="queryStringModification">Query String Modification</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>String</returns>
        public virtual string ModifyQueryString(string url, string queryStringModification, string anchor)
        {
            if (url == null)
                url = string.Empty;

            url = url.ToLowerInvariant();

            if (queryStringModification == null)
                queryStringModification = string.Empty;

            queryStringModification = queryStringModification.ToLowerInvariant();

            if (anchor == null)
                anchor = string.Empty;

            anchor = anchor.ToLowerInvariant();

            string stringA = string.Empty;
            string stringB = string.Empty;

            if (url.Contains("#"))
            {
                stringB = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }

            if (url.Contains("?"))
            {
                stringA = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(stringA))
                {
                    var dictionary = new Dictionary<string, string>();

                    foreach (string str3 in stringA.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new[] { '=' });

                            if (strArray.Length == 2)
                            {
                                if (!dictionary.ContainsKey(strArray[0]))
                                {
                                    dictionary[strArray[0]] = strArray[1];
                                }
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new[] { '=' });

                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }

                    var stringBuilder = new StringBuilder();

                    foreach (string str5 in dictionary.Keys)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }
                        stringBuilder.Append(str5);

                        if (dictionary[str5] != null)
                        {
                            stringBuilder.Append("=");
                            stringBuilder.Append(dictionary[str5]);
                        }
                    }

                    stringA = stringBuilder.ToString();
                }
                else
                {
                    stringA = queryStringModification;
                }
            }

            if (!string.IsNullOrEmpty(anchor))
            {
                stringB = anchor;
            }

            return (url + (string.IsNullOrEmpty(stringA) ? "" : ("?" + stringA)) + (string.IsNullOrEmpty(stringB) ? "" : ("#" + stringB))).ToLowerInvariant();
        }

        /// <summary>
        /// Remove Query String
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="queryString">Query String</param>
        /// <returns>String</returns>
        public virtual string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
                url = string.Empty;

            url = url.ToLowerInvariant();

            if (queryString == null)
                queryString = string.Empty;

            queryString = queryString.ToLowerInvariant();

            string str = string.Empty;

            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }

                    dictionary.Remove(queryString);

                    var stringBuilder = new StringBuilder();

                    foreach (string str5 in dictionary.Keys)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append("&");
                        }

                        stringBuilder.Append(str5);

                        if (dictionary[str5] != null)
                        {
                            stringBuilder.Append("=");
                            stringBuilder.Append(dictionary[str5]);
                        }
                    }
                    str = stringBuilder.ToString();
                }
            }

            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        /// <summary>
        /// Get Query String By Name
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="name">Name</param>
        /// <returns>String</returns>
        public virtual T QueryString<T>(string name)
        {
            string queryParam = null;

            if (IsRequestAvailable(_httpContextBase) && _httpContextBase.Request.QueryString[name] != null)
                queryParam = _httpContextBase.Request.QueryString[name];

            if (!String.IsNullOrEmpty(queryParam))
                return CommonHelper.To<T>(queryParam);

            return default(T);
        }

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }


        #region Static Helper

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Bool Item</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();

            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);

            return result;
        }

        /// <summary>
        /// Generate Random Digit Code
        /// </summary>
        /// <param name="lenght">Lenght</param>
        /// <returns>String Item</returns>
        public static string GenerateRandomDigitCode(int lenght)
        {
            var random = new Random();

            var valueString = string.Empty;

            for (int x = 0; x < lenght; x++)
            {
                valueString = String.Concat(valueString, random.Next(10).ToString());
            }

            return valueString;
        }

        /// <summary>
        /// Generate Random Integer
        /// </summary>
        /// <param name="min">Minimum Number</param>
        /// <param name="max">Maximum Number</param>
        /// <returns>Integer Item</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];

            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);

            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure Email is vaild
        /// </summary>
        /// <param name="value">Input Value</param>
        /// <returns>String Item</returns>
        /// <exception cref="Adrack.Core.AppException">Email is not valid.</exception>
        public static string EnsureEmail(string value)
        {
            var result = EnsureNotNull(value);

            result = result.Trim();
            result = EnsureMaximumLength(result, 100);

            if (!IsValidEmail(value))
            {
                throw new AppException("Email is not valid.");
            }

            return result;
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="value">Input Value</param>
        /// <param name="maxLength">Maximum Length</param>
        /// <param name="valueAdd">A Value to add to the end if the original string was shorten</param>
        /// <returns>String Item</returns>
        public static string EnsureMaximumLength(string value, int maxLength, string valueAdd = null)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length > maxLength)
            {
                var result = value.Substring(0, maxLength);

                if (!String.IsNullOrEmpty(valueAdd))
                {
                    result += valueAdd;
                }

                return result;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>String Item</returns>
        public static string EnsureNumericOnly(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            foreach (char x in value)
            {
                if (Char.IsDigit(x))
                {
                    stringBuilder.Append(x);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>String Item</returns>
        public static string EnsureNotNull(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Bool Item</returns>
        public static bool IsStringNullOrEmpty(params string[] value)
        {
            bool result = false;

            Array.ForEach(value, x => { if (string.IsNullOrEmpty(x)) result = true; });

            return result;
        }

        /// <summary>
        /// Asp Net Hosting Permission Level
        /// </summary>
        /// <returns>Asp Net Hosting Permission Level Item</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_aspNetHostingPermissionLevel.HasValue)
            {
                _aspNetHostingPermissionLevel = AspNetHostingPermissionLevel.None;

                foreach (AspNetHostingPermissionLevel aspNetHostingPermissionLevel in
                    new AspNetHostingPermissionLevel[]{
                        AspNetHostingPermissionLevel.Unrestricted,
                        AspNetHostingPermissionLevel.High,
                        AspNetHostingPermissionLevel.Medium,
                        AspNetHostingPermissionLevel.Low,
                        AspNetHostingPermissionLevel.Minimal
                })
                {
                    try
                    {
                        new AspNetHostingPermission(aspNetHostingPermissionLevel).Demand();
                        _aspNetHostingPermissionLevel = aspNetHostingPermissionLevel;

                        break;
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }

            return _aspNetHostingPermissionLevel.Value;
        }

        /// <summary>
        /// Set Property
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <param name="propertyName">Property Name</param>
        /// <param name="objectValue">Object Value</param>
        /// <exception cref="ArgumentNullException">
        /// instance
        /// or
        /// propertyName
        /// </exception>
        /// <exception cref="Adrack.Core.AppException">
        /// No property '{0}' found on the instance of type '{1}'.
        /// or
        /// The property '{0}' on the instance of type '{1}' does not have a setter.
        /// </exception>
        public static void SetProperty(object instance, string propertyName, object objectValue)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            Type instanceType = instance.GetType();

            PropertyInfo propertyInfo = instanceType.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new AppException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            }

            if (!propertyInfo.CanWrite)
            {
                throw new AppException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            }

            if (objectValue != null && !objectValue.GetType().IsAssignableFrom(propertyInfo.PropertyType))
            {
                objectValue = To(objectValue, propertyInfo.PropertyType);
            }

            propertyInfo.SetValue(instance, objectValue, new object[0]);
        }

        /// <summary>
        /// Get App Custom Type Converter
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Type Converter Item</returns>
        public static TypeConverter GetAppCustomTypeConverter(Type type)
        {
            if (type == typeof(List<int>))
            {
                return new GenericListTypeConverter<int>();
            }

            if (type == typeof(List<decimal>))
            {
                return new GenericListTypeConverter<decimal>();
            }

            if (type == typeof(List<string>))
            {
                return new GenericListTypeConverter<string>();
            }

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type
        /// </summary>
        /// <param name="objectValue">Object Value</param>
        /// <param name="destinationType">Destination Type</param>
        /// <returns>Object Item</returns>
        public static object To(object objectValue, Type destinationType)
        {
            return To(objectValue, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type
        /// </summary>
        /// <param name="objectValue">Object Value</param>
        /// <param name="destinationType">Destination Type</param>
        /// <param name="cultureInfo">Culture Info</param>
        /// <returns>Object Item</returns>
        public static object To(object objectValue, Type destinationType, CultureInfo cultureInfo)
        {
            if (objectValue != null)
            {
                var sourceType = objectValue.GetType();

                TypeConverter destinationConverter = GetAppCustomTypeConverter(destinationType);

                TypeConverter sourceConverter = GetAppCustomTypeConverter(sourceType);

                if (destinationConverter != null && destinationConverter.CanConvertFrom(objectValue.GetType()))
                {
                    return destinationConverter.ConvertFrom(null, cultureInfo, objectValue);
                }

                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                {
                    return sourceConverter.ConvertTo(null, cultureInfo, objectValue, destinationType);
                }

                if (destinationType.IsEnum && objectValue is int)
                {
                    return Enum.ToObject(destinationType, (int)objectValue);
                }

                if (!destinationType.IsAssignableFrom(objectValue.GetType()))
                {
                    return Convert.ChangeType(objectValue, destinationType, cultureInfo);
                }
            }
            return objectValue;
        }

        /// <summary>
        /// Converts a value to a destination type
        /// </summary>
        /// <typeparam name="T">The type to convert the value to</typeparam>
        /// <param name="objectValue">Object Value</param>
        /// <returns>T Item</returns>
        public static T To<T>(object objectValue)
        {
            //return (T)Convert.ChangeType(objectValue, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(objectValue, typeof(T));
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="stringValue">String Value</param>
        /// <returns>System.String.</returns>
        public static string ConvertEnum(string stringValue)
        {
            string result = string.Empty;

            char[] letters = stringValue.ToCharArray();

            foreach (char x in letters)
            {
                if (x.ToString() != x.ToString().ToLower())
                {
                    result += " " + x.ToString();
                }
                else
                {
                    result += x.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Set Culture
        /// </summary>
        /// <param name="culture">Culture</param>
        public static void SetCulture(string culture = "en-US")
        {
            if (string.IsNullOrEmpty(culture))
            {
                culture = "en-US";
            }

            var cultureInfo = CultureInfo.CreateSpecificCulture(culture);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        #region Xml Helper

        /// <summary>
        /// Xml Encode
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Encoded String</returns>
        public static string XmlEncode(string value)
        {
            if (value == null)
                return null;

            value = Regex.Replace(value, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);

            return XmlEncodeAsIs(value);
        }

        /// <summary>
        /// Xml Encode As Is
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Encoded String</returns>
        public static string XmlEncodeAsIs(string value)
        {
            if (value == null)
                return null;

            var stringWriter = new StringWriter();

            using (var xmlTextWriter = new XmlTextWriter(stringWriter))
            {
                xmlTextWriter.WriteString(value);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Encodes An Attribute
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Encoded String</returns>
        public static string XmlEncodeAttribute(string value)
        {
            if (value == null)
                return null;

            value = Regex.Replace(value, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);

            return XmlEncodeAttributeAsIs(value);
        }

        /// <summary>
        /// Encodes An Attribute As Is
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Encoded String</returns>
        public static string XmlEncodeAttributeAsIs(string value)
        {
            return XmlEncodeAsIs(value).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes An Attribute
        /// </summary>
        /// <param name="value">Attribute</param>
        /// <returns>Decoded String</returns>
        public static string XmlDecode(string value)
        {
            var stringBuilder = new StringBuilder(value);

            return stringBuilder.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes A Datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized Datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            var xmlSerializer = new XmlSerializer(typeof(DateTime));

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            {
                xmlSerializer.Serialize(stringWriter, dateTime);

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Deserializes A Datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized String</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            var xmlSerializer = new XmlSerializer(typeof(DateTime));

            using (var sr = new StringReader(dateTime))
            {
                object deserialize = xmlSerializer.Deserialize(sr);

                return (DateTime)deserialize;
            }
        }

        #endregion Xml Helper

        #endregion Static Helper

        #endregion Methods
    }
}