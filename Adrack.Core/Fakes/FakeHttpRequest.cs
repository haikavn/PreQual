// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakeHttpRequest.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Http Request
    /// Implements the <see cref="System.Web.HttpRequestBase" />
    /// </summary>
    /// <seealso cref="System.Web.HttpRequestBase" />
    public class FakeHttpRequest : HttpRequestBase
    {
        #region Fields

        /// <summary>
        /// Http Cookie Collection
        /// </summary>
        private readonly HttpCookieCollection _httpCookieCollection;

        /// <summary>
        /// From Parameter Collection
        /// </summary>
        private readonly NameValueCollection _formParameterCollection;

        /// <summary>
        /// Query String Parameter Collection
        /// </summary>
        private readonly NameValueCollection _queryStringParameterCollection;

        /// <summary>
        /// Header Collection
        /// </summary>
        private readonly NameValueCollection _headerCollection;

        /// <summary>
        /// Server Variable Collection
        /// </summary>
        private readonly NameValueCollection _serverVariableCollection;

        /// <summary>
        /// Relative Url
        /// </summary>
        private readonly string _relativeUrl;

        /// <summary>
        /// Url
        /// </summary>
        private readonly Uri _url;

        /// <summary>
        /// Url Referrer
        /// </summary>
        private readonly Uri _urlReferrer;

        /// <summary>
        /// Http Method
        /// </summary>
        private readonly string _httpMethod;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Http Request
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="method">Method</param>
        /// <param name="formParameterCollection">Form Parameter Collection</param>
        /// <param name="queryStringParameterCollection">Query String Parameter Collection</param>
        /// <param name="httpCookieCollection">Http Cookie Collection</param>
        /// <param name="serverVariableCollection">Server Variable Collection</param>
        public FakeHttpRequest(string relativeUrl, string method, NameValueCollection formParameterCollection, NameValueCollection queryStringParameterCollection, HttpCookieCollection httpCookieCollection, NameValueCollection serverVariableCollection)
        {
            _httpMethod = method;
            _relativeUrl = relativeUrl;
            _formParameterCollection = formParameterCollection;
            _queryStringParameterCollection = queryStringParameterCollection;
            _httpCookieCollection = httpCookieCollection;
            _serverVariableCollection = serverVariableCollection;

            if (_formParameterCollection == null)
                _formParameterCollection = new NameValueCollection();
            if (_queryStringParameterCollection == null)
                _queryStringParameterCollection = new NameValueCollection();
            if (_httpCookieCollection == null)
                _httpCookieCollection = new HttpCookieCollection();
            if (_serverVariableCollection == null)
                _serverVariableCollection = new NameValueCollection();
            if (_headerCollection == null)
                _headerCollection = new NameValueCollection();
        }

        /// <summary>
        /// Fake Http Request
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="method">Method</param>
        /// <param name="url">Url</param>
        /// <param name="urlReferrer">Url Referrer</param>
        /// <param name="formParameterCollection">Form Parameter Collection</param>
        /// <param name="queryStringParameterCollection">Query String Parameter Collection</param>
        /// <param name="httpCookieCollection">Http Cookie Collection</param>
        /// <param name="serverVariableCollection">Server Variable Collection</param>
        public FakeHttpRequest(string relativeUrl, string method, Uri url, Uri urlReferrer, NameValueCollection formParameterCollection, NameValueCollection queryStringParameterCollection, HttpCookieCollection httpCookieCollection, NameValueCollection serverVariableCollection) : this(relativeUrl, method, formParameterCollection, queryStringParameterCollection, httpCookieCollection, serverVariableCollection)
        {
            _url = url;
            _urlReferrer = urlReferrer;
        }

        /// <summary>
        /// Fake Http Request
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="url">Url</param>
        /// <param name="urlReferrer">Url Referrer</param>
        public FakeHttpRequest(string relativeUrl, Uri url, Uri urlReferrer) : this(relativeUrl, HttpVerbs.Get.ToString("g"), url, urlReferrer, null, null, null, null)
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Server Variables
        /// </summary>
        /// <value>The server variables.</value>
        public override NameValueCollection ServerVariables
        {
            get { return _serverVariableCollection; }
        }

        /// <summary>
        /// Gets or Sets the Form
        /// </summary>
        /// <value>The form.</value>
        public override NameValueCollection Form
        {
            get { return _formParameterCollection; }
        }

        /// <summary>
        /// Gets or Sets the Query String
        /// </summary>
        /// <value>The query string.</value>
        public override NameValueCollection QueryString
        {
            get { return _queryStringParameterCollection; }
        }

        /// <summary>
        /// Gets or Sets the Headers
        /// </summary>
        /// <value>The headers.</value>
        public override NameValueCollection Headers
        {
            get { return _headerCollection; }
        }

        /// <summary>
        /// Gets or Sets the Cookies
        /// </summary>
        /// <value>The cookies.</value>
        public override HttpCookieCollection Cookies
        {
            get { return _httpCookieCollection; }
        }

        /// <summary>
        /// Gets or Sets the Application Relative Current Execution File Path
        /// </summary>
        /// <value>The application relative current execution file path.</value>
        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _relativeUrl; }
        }

        /// <summary>
        /// Gets or Sets the Url
        /// </summary>
        /// <value>The URL.</value>
        public override Uri Url
        {
            get { return _url; }
        }

        /// <summary>
        /// Gets or Sets the Url Referrer
        /// </summary>
        /// <value>The URL referrer.</value>
        public override Uri UrlReferrer
        {
            get { return _urlReferrer; }
        }

        /// <summary>
        /// Gets or Sets the Path Info
        /// </summary>
        /// <value>The path information.</value>
        public override string PathInfo
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets or Sets the Application Path
        /// </summary>
        /// <value>The application path.</value>
        public override string ApplicationPath
        {
            get
            {
                if (_relativeUrl != null && _relativeUrl.StartsWith("~/"))
                    return _relativeUrl.Remove(0, 1);
                return null;
            }
        }

        /// <summary>
        /// Gets or Sets the Http Method
        /// </summary>
        /// <value>The HTTP method.</value>
        public override string HttpMethod
        {
            get { return _httpMethod; }
        }

        /// <summary>
        /// Gets or Sets the User Host Address
        /// </summary>
        /// <value>The user host address.</value>
        public override string UserHostAddress
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or Sets the Raw Url
        /// </summary>
        /// <value>The raw URL.</value>
        public override string RawUrl
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or Sets the Is SecureConnection
        /// </summary>
        /// <value><c>true</c> if this instance is secure connection; otherwise, <c>false</c>.</value>
        public override bool IsSecureConnection
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or Sets the Is Authenticated
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public override bool IsAuthenticated
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or Sets the User Languages
        /// </summary>
        /// <value>The user languages.</value>
        public override string[] UserLanguages
        {
            get { return null; }
        }

        #endregion Properties
    }
}