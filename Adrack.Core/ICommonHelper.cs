// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ICommonHelper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Common Helper
    /// </summary>
    public partial interface ICommonHelper
    {
        #region Methods

        /// <summary>
        /// Get Ip Address
        /// </summary>
        /// <returns>String Item</returns>
        string GetIpAddress();

        /// <summary>
        /// Get Url Referrer
        /// </summary>
        /// <returns>String Item</returns>
        string GetUrlReferrer();

        /// <summary>
        /// Is Connection Secured
        /// </summary>
        /// <returns>Bool Item</returns>
        bool IsConnectionSecured();

        /// <summary>
        /// Get Page Url
        /// </summary>
        /// <param name="includeQueryString">Include Query String</param>
        /// <returns>String Item</returns>
        string GetPageUrl(bool includeQueryString);

        /// <summary>
        /// Get Page Url
        /// </summary>
        /// <param name="includeQueryString">Include Query String</param>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        string GetPageUrl(bool includeQueryString, bool useSsl);

        /// <summary>
        /// Get Server Variable
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>String Item</returns>
        string GetServerVariable(string name);

        /// <summary>
        /// Get Application Host
        /// </summary>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        string GetAppHost(bool useSsl);

        /// <summary>
        /// Get Application Location
        /// </summary>
        /// <returns>String Item</returns>
        string GetAppLocation();

        /// <summary>
        /// Get Application Location
        /// </summary>
        /// <param name="useSsl">Use Secure Sockets Layer</param>
        /// <returns>String Item</returns>
        string GetAppLocation(bool useSsl);

        /// <summary>
        /// Is Static Resource
        /// </summary>
        /// <param name="httpRequest">Http Request</param>
        /// <returns><c>true</c> if [is static resource] [the specified HTTP request]; otherwise, <c>false</c>.</returns>
        bool IsStaticResource(HttpRequest httpRequest);

        /// <summary>
        /// Get Root Path
        /// </summary>
        /// <param name="rootPath">Root Path</param>
        /// <returns>String Item</returns>
        string GetRootPath(string rootPath);

        /// <summary>
        /// Modify Query String
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="queryStringModification">Query String Modification</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>String</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// Remove Query String
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="queryString">Query String</param>
        /// <returns>String</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// Get Query String By Name
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="name">Name</param>
        /// <returns>String</returns>
        T QueryString<T>(string name);

        #endregion Methods
    }
}