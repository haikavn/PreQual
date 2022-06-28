// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BaseSitemapGenerator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Adrack.Service.Seo
{
    /// <summary>
    /// Represents a Base Sitemap Generator
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract partial class BaseSitemapGenerator : IDisposable
    {
        #region Fields

        /// <summary>
        /// Date Format
        /// </summary>
        private const string DateFormat = @"yyyy-MM-dd";

        /// <summary>
        /// Xml Text Writer
        /// </summary>
        private XmlTextWriter _xmlTextWriter;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Method that is overridden, that handles creation of child urls.
        /// Use the method WriteUrlLocation() within this method.
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        protected abstract void GenerateUrlNodes(UrlHelper urlHelper);

        /// <summary>
        /// Writes the url location to the writer.
        /// </summary>
        /// <param name="url">Url of indexed location (don't put root url information in).</param>
        /// <param name="updateFrequency">Update frequency - always, hourly, daily, weekly, yearly, never.</param>
        /// <param name="lastUpdated">Date last updated.</param>
        protected virtual void WriteUrlLocation(string url, UpdateFrequency updateFrequency, DateTime lastUpdated)
        {
            _xmlTextWriter.WriteStartElement("url");

            string loc = CommonHelper.XmlEncode(url);

            _xmlTextWriter.WriteElementString("loc", loc);
            _xmlTextWriter.WriteElementString("changefreq", updateFrequency.ToString().ToLowerInvariant());
            _xmlTextWriter.WriteElementString("lastmod", lastUpdated.ToString(DateFormat));
            _xmlTextWriter.WriteEndElement();
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Generate
        /// </summary>
        /// <param name="urlHelper">Url helper</param>
        /// <returns>Sitemap.xml as string</returns>
        public virtual string Generate(UrlHelper urlHelper)
        {
            using (var memoryStream = new MemoryStream())
            {
                Generate(urlHelper, memoryStream);

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Generate
        /// </summary>
        /// <param name="urlHelper">Url Helper</param>
        /// <param name="stream">Stream</param>
        public virtual void Generate(UrlHelper urlHelper, Stream stream)
        {
            _xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };
            _xmlTextWriter.WriteStartDocument();
            _xmlTextWriter.WriteStartElement("urlset");
            _xmlTextWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            _xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            _xmlTextWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

            GenerateUrlNodes(urlHelper);

            _xmlTextWriter.WriteEndElement();
            _xmlTextWriter.Close();
        }

        #endregion Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _xmlTextWriter.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}