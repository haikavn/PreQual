// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AppConfiguration.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Configuration;
using System.Xml;

namespace Adrack.Core.Infrastructure.Configuration
{
    /// <summary>
    /// Represents a Application Configuration
    /// Implements the <see cref="System.Configuration.IConfigurationSectionHandler" />
    /// </summary>
    /// <seealso cref="System.Configuration.IConfigurationSectionHandler" />
    public partial class AppConfiguration : IConfigurationSectionHandler
    {
        #region Utilities

        /// <summary>
        /// Set By X Element
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <param name="converter">Converter</param>
        /// <returns>Return Type</returns>
        private T SetByXElement<T>(XmlNode xmlNode, string attributeName, Func<string, T> converter)
        {
            if (xmlNode == null || xmlNode.Attributes == null)
                return default(T);

            var attr = xmlNode.Attributes[attributeName];

            if (attr == null)
                return default(T);

            var attrVal = attr.Value;

            return converter(attrVal);
        }

        /// <summary>
        /// Get String
        /// </summary>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns>System.String.</returns>
        private string GetString(XmlNode xmlNode, string attributeName)
        {
            return SetByXElement<string>(xmlNode, attributeName, Convert.ToString);
        }

        /// <summary>
        /// Get Bool
        /// </summary>
        /// <param name="xmlNode">Xml Node</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool GetBool(XmlNode xmlNode, string attributeName)
        {
            return SetByXElement<bool>(xmlNode, attributeName, Convert.ToBoolean);
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="configContext">Config Context</param>
        /// <param name="xmlNodeSection">Xml Node Section</param>
        /// <returns>The created xml node section handler object</returns>
        public object Create(object parent, object configContext, XmlNode xmlNodeSection)
        {
            var appConfiguration = new AppConfiguration();

            var applicationCachesNode = xmlNodeSection.SelectSingleNode("Application");
            appConfiguration.ApplicationCacheEnabled = GetBool(applicationCachesNode, "CacheEnabled");
            appConfiguration.ApplicationStartupTasksEnabled = GetBool(applicationCachesNode, "StartupTasksEnabled");

            var webFarmNode = xmlNodeSection.SelectSingleNode("WebFarm");
            appConfiguration.WebFarmMultipleInstanceEnabled = GetBool(webFarmNode, "MultipleInstanceEnabled");
            appConfiguration.WebFarmRunOnAzureWebsites = GetBool(webFarmNode, "RunOnAzureWebsites");

            var azureBlobStorageNode = xmlNodeSection.SelectSingleNode("AzureBlobStorage");
            appConfiguration.AzureBlobStorageConnectionString = GetString(azureBlobStorageNode, "ConnectionString");
            appConfiguration.AzureBlobStorageContainerName = GetString(azureBlobStorageNode, "ContainerName");
            appConfiguration.AzureBlobStorageEndPoint = GetString(azureBlobStorageNode, "EndPoint");

            var redisCachingNode = xmlNodeSection.SelectSingleNode("RedisCaching");
            appConfiguration.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            appConfiguration.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");

            return appConfiguration;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Application Cache Enabled
        /// </summary>
        /// <value><c>true</c> if [application cache enabled]; otherwise, <c>false</c>.</value>
        public bool ApplicationCacheEnabled { get; private set; }

        /// <summary>
        /// Application Cache Enabled
        /// </summary>
        /// <value><c>true</c> if [application startup tasks enabled]; otherwise, <c>false</c>.</value>
        public bool ApplicationStartupTasksEnabled { get; private set; }

        /// <summary>
        /// Web Farm Multiple Instance Enabled
        /// </summary>
        /// <value><c>true</c> if [web farm multiple instance enabled]; otherwise, <c>false</c>.</value>
        public bool WebFarmMultipleInstanceEnabled { get; private set; }

        /// <summary>
        /// Web Farm Run On Azure Websites
        /// </summary>
        /// <value><c>true</c> if [web farm run on azure websites]; otherwise, <c>false</c>.</value>
        public bool WebFarmRunOnAzureWebsites { get; private set; }

        /// <summary>
        /// Azure Blob Storage Connection String
        /// </summary>
        /// <value>The azure BLOB storage connection string.</value>
        public string AzureBlobStorageConnectionString { get; private set; }

        /// <summary>
        /// Azure Blob Storage Container Name
        /// </summary>
        /// <value>The name of the azure BLOB storage container.</value>
        public string AzureBlobStorageContainerName { get; private set; }

        /// <summary>
        /// Azure Blob Storage End Point
        /// </summary>
        /// <value>The azure BLOB storage end point.</value>
        public string AzureBlobStorageEndPoint { get; private set; }

        /// <summary>
        /// Redis Caching Enabled
        /// </summary>
        /// <value><c>true</c> if [redis caching enabled]; otherwise, <c>false</c>.</value>
        public bool RedisCachingEnabled { get; private set; }

        /// <summary>
        /// Redis Caching Connection String
        /// </summary>
        /// <value>The redis caching connection string.</value>
        public string RedisCachingConnectionString { get; private set; }

        #endregion Properties
    }
}