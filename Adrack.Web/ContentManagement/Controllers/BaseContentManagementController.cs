// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BaseContentManagementController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Controllers;
using Adrack.Web.Framework.Security;
using Adrack.Web.Framework.Seo;
using System.Text;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Represents a Base Content Management Controller
    /// Implements the <see cref="Adrack.Web.Framework.Controllers.BaseController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Controllers.BaseController" />
    [AppHttpsRequirement(SslRequirement.Yes)]
    [WwwRequirement]
    [UserTimeout]
    [ContentManagementAuthorize]
    [ContentManagementAntiForgery]
    public abstract partial class BaseContentManagementController : BaseController
    {
        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            AppEngineContext.Current.Resolve<IAppContext>().IsGlobalAdministrator = true;

            base.Initialize(requestContext);
        }

        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="exceptionContext">Exception Context</param>
        protected override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception != null)
                LogException(exceptionContext.Exception);

            base.OnException(exceptionContext);
        }

        /// <summary>
        /// Page Unauthorized
        /// </summary>
        /// <returns>Action Result</returns>
        protected ActionResult PageUnauthorized()
        {
            return RedirectToAction("PageUnauthorized", "Common", new { pageUrl = Request.RawUrl });
        }

        /// <summary>
        /// Json
        /// </summary>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <param name="behavior">The JSON request behavior</param>
        /// <returns>Json Result</returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion Methods
    }
}