// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BasePublicController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Controllers;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.Seo;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Base Public Controller
    ///     Implements the <see cref="BaseController" />
    /// </summary>
    /// <seealso cref="BaseController" />
    [SiteMaintenance]
    [LanguageSeoCode]
    [AppHttpsRequirement(SslRequirement.Yes)]
    [WwwRequirement]
    public abstract class BasePublicController : BaseController
    {
        #region Methods

        /// <summary>
        ///     Invoke Http 404
        /// </summary>
        /// <returns>Action Result Item</returns>
        protected virtual ActionResult InvokeHttp404()
        {
            IController errorController = AppEngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();

            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(HttpContext, routeData));

            return new EmptyResult();
        }

        #endregion Methods
    }
}