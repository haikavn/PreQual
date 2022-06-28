using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrack.WebApi.Infrastructure.Enums;

namespace Adrack.WebApi.Controllers
{
    public class CommonController : Controller
    {
        #region Methods

        /// <summary>
        ///     Page Unauthorized
        /// </summary>
        /// <returns>Action Result</returns>
        protected ActionResult PageUnauthorized()
        {
            return Json(new { StatusCode = ResponseStatusCode.Unauthorized }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Page Error
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>Action Result</returns>
        public ActionResult PageError(int statusCode)
        {
            return Json(new { StatusCode = statusCode }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Page Forbidden
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageForbidden()
        {
            return Json(new { StatusCode = ResponseStatusCode.Forbidden }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Page Not Found
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageNotFound()
        {
            return Json(new { StatusCode = ResponseStatusCode.PageNotFound }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Page Internal Server Error
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult PageInternalServerError()
        {
            return Json(new { StatusCode = ResponseStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}
