using Adrack.Core;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Adrack.Web.Framework.Security
{
    public class AllowUserPermissionAttribute : ActionFilterAttribute
    {
        private readonly string[] _permissions = null;
        private IPermissionService _permissionService = null;

        public AllowUserPermissionAttribute(IPermissionService permissionService, params string[] permissions)
        {
            _permissions = permissions;
            _permissionService = permissionService;
        }

        public AllowUserPermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_permissionService == null)
                _permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            bool hasAccess = true;

            foreach(var permission in _permissions)
            {
                hasAccess = _permissionService.Authorize(permission);
                if (!hasAccess)
                    break;
            }

            if (!hasAccess)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
