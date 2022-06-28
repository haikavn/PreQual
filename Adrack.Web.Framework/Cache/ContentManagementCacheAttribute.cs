// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ContentManagementAuthorizeAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Common;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Adrack.Web.Framework.Cache
{
    /// <summary>
    /// Represents a Content Management Authorize Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ContentManagementCacheAttribute : ActionFilterAttribute
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private string _cachePattern = "";
       
        #endregion Fields

        #region Constructor

        /// <summary>
        /// Content Management Authorize Attribute
        /// </summary>
        /// <param name="validate">Validate</param>
        public ContentManagementCacheAttribute(string cachePattern)
        {
            _cacheManager = AppEngineContext.Current.Resolve<ICacheManager>();
            _cachePattern = cachePattern;
        }

        #endregion Constructor

        #region Methods

        protected string GetActionId(HttpActionContext actionContext)
        {
            string actionId = actionContext.ActionDescriptor.ActionName;

            foreach (string key in actionContext.ActionArguments.Keys)
            {
                if (actionContext.ActionArguments[key] != null)
                {
                    object obj = actionContext.ActionArguments[key];

                    if (obj != null)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        using (var ms = new MemoryStream())
                        {
                            bf.Serialize(ms, obj);
                            actionId += "-" + key + "=" + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }

            return actionId;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string actionId = GetActionId(actionContext);

            if (!string.IsNullOrEmpty(actionContext.ActionDescriptor.ActionName))
            {
                if (!string.IsNullOrEmpty(actionId))
                {
                    object result = _cacheManager.Get(_cachePattern + actionId);
                    if (result != null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.OK,
                            result,
                            actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );
                        return;
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            string actionId = GetActionId(actionExecutedContext.ActionContext);

            _cacheManager.Set(_cachePattern + actionId, (actionExecutedContext.ActionContext.Response.Content as ObjectContent).Value, 2280);

            base.OnActionExecuted(actionExecutedContext);
        }

        #endregion Methods

    }
}