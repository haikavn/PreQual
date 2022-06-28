// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LocalizedUrlExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using System;

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized Url Extensions
    /// </summary>
    public static class LocalizedUrlExtensions
    {
        #region Fields

        /// <summary>
        /// SEO Code Length
        /// </summary>
        private static int _seoCultureLength = 2;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Is Virtual Directory
        /// </summary>
        /// <param name="applicationPath">Application Path</param>
        /// <returns>Boolean Item</returns>
        /// <exception cref="ArgumentException">Application path is not specified</exception>
        private static bool IsVirtualDirectory(this string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            return applicationPath != "/";
        }

        #endregion Utilities



        #region Methods

        /// <summary>
        /// Remove Application Path From Raw Url
        /// </summary>
        /// <param name="rawUrl">Raw Url</param>
        /// <param name="applicationPath">Application Path</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentException">Application path is not specified</exception>
        public static string RemoveApplicationPathFromRawUrl(this string rawUrl, string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            if (rawUrl.Length == applicationPath.Length)
                return "/";

            var result = rawUrl.Substring(applicationPath.Length);

            if (!result.StartsWith("/"))
                result = "/" + result;

            return result;
        }

        /// <summary>
        /// Get Language Seo Culture From Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="applicationPath">Application Path</param>
        /// <param name="isRawPath">Is Raw Path</param>
        /// <returns>String Item</returns>
        public static string GetLanguageSeoCultureFromUrl(this string url, string applicationPath, bool isRawPath)
        {
            if (isRawPath)
            {
                if (applicationPath.IsVirtualDirectory())
                {
                    url = url.RemoveApplicationPathFromRawUrl(applicationPath);
                }

                return url.Substring(1, _seoCultureLength);
            }
            else
            {
                return url.Substring(2, _seoCultureLength);
            }
        }

        /// <summary>
        /// Is Localized Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="applicationPath">Application Path</param>
        /// <param name="isRawPath">Is Raw Path</param>
        /// <returns>String Item</returns>
        public static bool IsLocalizedUrl(this string url, string applicationPath, bool isRawPath)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if (isRawPath)
            {
                if (applicationPath.IsVirtualDirectory())
                {
                    url = url.RemoveApplicationPathFromRawUrl(applicationPath);
                }

                int length = url.Length;

                if (length < 1 + _seoCultureLength)
                    return false;

                if (length == 1 + _seoCultureLength)
                    return true;

                return (length > 1 + _seoCultureLength) && (url[1 + _seoCultureLength] == '/');
            }
            else
            {
                int length = url.Length;

                if (length < 2 + _seoCultureLength)
                    return false;

                if (length == 2 + _seoCultureLength)
                    return true;

                return (length > 2 + _seoCultureLength) && (url[2 + _seoCultureLength] == '/');
            }
        }

        /// <summary>
        /// Remove Language Seo Culture From Raw Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="applicationPath">Application Path</param>
        /// <returns>String Item</returns>
        public static string RemoveLanguageSeoCultureFromRawUrl(this string url, string applicationPath)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            string result = null;

            if (applicationPath.IsVirtualDirectory())
            {
                url = url.RemoveApplicationPathFromRawUrl(applicationPath);
            }

            int length = url.Length;

            if (length < _seoCultureLength + 1)
                result = url;
            else if (length == 1 + _seoCultureLength)
                result = url.Substring(0, 1);
            else
                result = url.Substring(_seoCultureLength + 1);

            if (applicationPath.IsVirtualDirectory())
                result = applicationPath + result;

            return result;
        }

        /// <summary>
        /// Add Language Seo Culture To Raw Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="applicationPath">Application Path</param>
        /// <param name="language">Language</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentNullException">language</exception>
        public static string AddLanguageSeoCultureToRawUrl(this string url, string applicationPath, Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            //null validation is not required
            //if (string.IsNullOrEmpty(url))
            //    return url;

            int startIndex = 0;

            if (applicationPath.IsVirtualDirectory())
            {
                startIndex = applicationPath.Length;
            }

            url = url.Insert(startIndex, language.Culture.Substring(0, 2));
            url = url.Insert(startIndex, "/");

            return url;
        }

        #endregion Methods
    }
}