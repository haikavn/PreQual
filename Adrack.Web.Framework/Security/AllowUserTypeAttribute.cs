using Adrack.Core;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Adrack.Web.Framework.Security
{
    public class AllowUserTypeAttribute : ActionFilterAttribute
    {
        private readonly UserTypes[] _allowedUserTypes = null;
        private IAppContext _appContext = null;

        public AllowUserTypeAttribute(params UserTypes[] allowedUserTypes)
        {
            _allowedUserTypes = allowedUserTypes;
        }

        public AllowUserTypeAttribute(IAppContext appContext, params UserTypes[] allowedUserTypes)
        {
            _allowedUserTypes = allowedUserTypes;
            _appContext = appContext;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_appContext == null)
                _appContext = AppEngineContext.Current.Resolve<IAppContext>();

            if (_appContext.AppUser != null &&
                _appContext.AppUser.UserType != UserTypes.Super &&
                !_allowedUserTypes.Contains((UserTypes)_appContext.AppUser.UserType))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
