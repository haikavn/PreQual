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
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Adrack.Web.Framework.Security
{
    /// <summary>
    /// Represents a Content Management Authorize Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PublicApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        #region Fields

        private readonly IAppContext _appContext;
        private readonly IJWTTokenService _jWTTokenService;
       
        #endregion Fields

        #region Utilities

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Content Management Authorize Attribute
        /// </summary>
        /// <param name="validate">Validate</param>
        public PublicApiAuthorizeAttribute()
        {
            _jWTTokenService = AppEngineContext.Current.Resolve<IJWTTokenService>();
            _appContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

        #endregion Constructor

        #region Methods

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Utils.SetupConnectionString(actionContext.Request);

            var processingUser = new User { Id = 1, Username = "ProcessingAdmin", UserType = UserTypes.Super };

            actionContext.RequestContext.Principal = new CustomPrincipal(processingUser);

            base.OnAuthorization(actionContext);
        }

        #endregion Methods

    }
}