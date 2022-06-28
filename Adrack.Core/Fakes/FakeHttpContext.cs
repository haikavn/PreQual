// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakeHttpContext.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Http Context
    /// Implements the <see cref="System.Web.HttpContextBase" />
    /// </summary>
    /// <seealso cref="System.Web.HttpContextBase" />
    public class FakeHttpContext : HttpContextBase
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
        /// Principal
        /// </summary>
        private IPrincipal _principal;

        /// <summary>
        /// Query String Parameter Collection
        /// </summary>
        private readonly NameValueCollection _queryStringParameterCollection;

        /// <summary>
        /// Relative Url
        /// </summary>
        private readonly string _relativeUrl;

        /// <summary>
        /// Method
        /// </summary>
        private readonly string _method;

        /// <summary>
        /// Session State Item Collection
        /// </summary>
        private readonly SessionStateItemCollection _sessionStateItemCollection;

        /// <summary>
        /// Server Variable Collection
        /// </summary>
        private readonly NameValueCollection _serverVariableCollection;

        /// <summary>
        /// Http Response Base
        /// </summary>
        private HttpResponseBase _httpResponseBase;

        /// <summary>
        /// Http Request Base
        /// </summary>
        private HttpRequestBase _httpRequestBase;

        /// <summary>
        /// Dictionary Items
        /// </summary>
        private readonly IDictionary _dictionaryItems;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Http Context
        /// </summary>
        /// <returns>Fake Http Context</returns>
        public static FakeHttpContext Root()
        {
            return new FakeHttpContext("~/");
        }

        /// <summary>
        /// Fake Http Context
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="method">Method</param>
        public FakeHttpContext(string relativeUrl, string method) : this(relativeUrl, method, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Fake Http Context
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        public FakeHttpContext(string relativeUrl) : this(relativeUrl, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Fake Http Context
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="principal">Principal</param>
        /// <param name="formParameterCollection">Form Parameter Collection</param>
        /// <param name="queryStringParameterCollection">Query String Parameter Collection</param>
        /// <param name="httpCookieCollection">Http Cookie Collection</param>
        /// <param name="sessionStateItemCollection">Session State Item Collection</param>
        /// <param name="serverVariableCollection">Server Variable Collection</param>
        public FakeHttpContext(string relativeUrl, IPrincipal principal, NameValueCollection formParameterCollection, NameValueCollection queryStringParameterCollection, HttpCookieCollection httpCookieCollection, SessionStateItemCollection sessionStateItemCollection, NameValueCollection serverVariableCollection) : this(relativeUrl, null, principal, formParameterCollection, queryStringParameterCollection, httpCookieCollection, sessionStateItemCollection, serverVariableCollection)
        {
        }

        /// <summary>
        /// Fake Http Context
        /// </summary>
        /// <param name="relativeUrl">Relative Url</param>
        /// <param name="method">Method</param>
        /// <param name="principal">Principal</param>
        /// <param name="formParameterCollection">Form Parameter Collection</param>
        /// <param name="queryStringParameterCollection">Query String Parameter Collection</param>
        /// <param name="httpCookieCollection">Http Cookie Collection</param>
        /// <param name="sessionStateItemCollection">Session State Item Collection</param>
        /// <param name="serverVariableCollection">Server Variable Collection</param>
        public FakeHttpContext(string relativeUrl, string method, IPrincipal principal, NameValueCollection formParameterCollection, NameValueCollection queryStringParameterCollection, HttpCookieCollection httpCookieCollection, SessionStateItemCollection sessionStateItemCollection, NameValueCollection serverVariableCollection)
        {
            _relativeUrl = relativeUrl;
            _method = method;
            _principal = principal;
            _formParameterCollection = formParameterCollection;
            _queryStringParameterCollection = queryStringParameterCollection;
            _httpCookieCollection = httpCookieCollection;
            _sessionStateItemCollection = sessionStateItemCollection;
            _serverVariableCollection = serverVariableCollection;

            _dictionaryItems = new Dictionary<object, object>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Set Request
        /// </summary>
        /// <param name="httpRequestBase">Http Request Base</param>
        public void SetRequest(HttpRequestBase httpRequestBase)
        {
            _httpRequestBase = httpRequestBase;
        }

        /// <summary>
        /// Set Response
        /// </summary>
        /// <param name="httpResponseBase">Http Response Base</param>
        public void SetResponse(HttpResponseBase httpResponseBase)
        {
            _httpResponseBase = httpResponseBase;
        }

        /// <summary>
        /// Get Service
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Object Item</returns>
        public override object GetService(Type serviceType)
        {
            return null;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Request
        /// </summary>
        /// <value>The request.</value>
        public override HttpRequestBase Request
        {
            get { return _httpRequestBase ?? new FakeHttpRequest(_relativeUrl, _method, _formParameterCollection, _queryStringParameterCollection, _httpCookieCollection, _serverVariableCollection); }
        }

        /// <summary>
        /// Gets or Sets the Response
        /// </summary>
        /// <value>The response.</value>
        public override HttpResponseBase Response
        {
            get { return _httpResponseBase ?? new FakeHttpResponse(); }
        }

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public override IPrincipal User
        {
            get { return _principal; }
            set { _principal = value; }
        }

        /// <summary>
        /// Gets or Sets the Session
        /// </summary>
        /// <value>The session.</value>
        public override HttpSessionStateBase Session
        {
            get { return new FakeHttpSessionState(_sessionStateItemCollection); }
        }

        /// <summary>
        /// Gets or Sets the Items
        /// </summary>
        /// <value>The items.</value>
        public override IDictionary Items
        {
            get { return _dictionaryItems; }
        }

        /// <summary>
        /// Gets or Sets the Skip Authorization
        /// </summary>
        /// <value><c>true</c> if [skip authorization]; otherwise, <c>false</c>.</value>
        public override bool SkipAuthorization { get; set; }

        #endregion Properties
    }
}