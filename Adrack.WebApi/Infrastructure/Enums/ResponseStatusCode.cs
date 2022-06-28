using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Infrastructure.Enums
{

    public enum ResponseStatusCode
    {
        Unauthorized = 401,
        Forbidden = 403,
        PageNotFound = 404,
        InternalServerError = 500
    }
}