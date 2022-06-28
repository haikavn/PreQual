using Adrack.Web.Framework.Security;
using Adrack.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Adrack.WebApi.Controllers
{
    [PublicApiAuthorize]
    public class BaseApiPublicController : ApiController
    {
        [NonAction]
        public IHttpActionResult HttpBadRequest(string message)
        {
            return Content(HttpStatusCode.OK, new ErrorViewModel()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Message = message
            });
        }
    }
}
