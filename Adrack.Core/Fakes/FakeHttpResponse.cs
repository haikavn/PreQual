// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakeHttpResponse.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Text;
using System.Web;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Http Response
    /// Implements the <see cref="System.Web.HttpResponseBase" />
    /// </summary>
    /// <seealso cref="System.Web.HttpResponseBase" />
    public class FakeHttpResponse : HttpResponseBase
    {
        #region Fields

        /// <summary>
        /// Http Cookie Collection
        /// </summary>
        private readonly HttpCookieCollection _httpCookieCollection;

        /// <summary>
        /// String Builder
        /// </summary>
        private readonly StringBuilder _outputString = new StringBuilder();

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Http Response
        /// </summary>
        public FakeHttpResponse()
        {
            this._httpCookieCollection = new HttpCookieCollection();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="s">String</param>
        public override void Write(string s)
        {
            _outputString.Append(s);
        }

        /// <summary>
        /// Apply Application Path Modifier
        /// </summary>
        /// <param name="virtualPath">Virtual Path</param>
        /// <returns>The virtual path, with the session ID inserted.</returns>
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Response Output
        /// </summary>
        /// <value>The response output.</value>
        public string ResponseOutput
        {
            get { return _outputString.ToString(); }
        }

        /// <summary>
        /// Gets or Sets the Status Code
        /// </summary>
        /// <value>The status code.</value>
        public override int StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets the Redirect Location
        /// </summary>
        /// <value>The redirect location.</value>
        public override string RedirectLocation { get; set; }

        /// <summary>
        /// Gets or Sets the Cookies
        /// </summary>
        /// <value>The cookies.</value>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return _httpCookieCollection;
            }
        }

        #endregion Properties
    }
}