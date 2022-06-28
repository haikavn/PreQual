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
using Adrack.Service.Agent;
using Adrack.Service.Audit;
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
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
    public class ContentManagementApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public class ErrorViewModel
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<ErrorViewModel> Errors { get; set; }

            public ErrorViewModel()
            {
                Errors = new List<ErrorViewModel>();
            }
        }

        #region Fields

        /// <summary>
        /// Validate
        /// </summary>
        private readonly bool _validate;

        private readonly IAppContext _appContext;
        private readonly IJWTTokenService _jWTTokenService;
       
        #endregion Fields

        #region Utilities

        /// <summary>
        /// Handle Unauthorized Request
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext, JWTVerficationResults jWTVerficationResult)
        {
            var errorViewModel = new ErrorViewModel()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = jWTVerficationResult != JWTVerficationResults.Expired ? "access-denied" : "session-expired"
            };

            var responseMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            responseMessage.Content = new ObjectContent<ErrorViewModel>(errorViewModel, new JsonMediaTypeFormatter());

            actionContext.Response = responseMessage;
            //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Handles the not found request.
        /// </summary>
        /// <param name="authorizationContext">The authorization context.</param>
        private void HandleNotFoundRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Get Content Management Authorize Attributes
        /// </summary>
        /// <param name="descriptor">Descriptor</param>
        /// <returns>Content Management Authorize Attribute Collection Item</returns>
        private IEnumerable<ContentManagementApiAuthorizeAttribute> GetContentManagementAuthorizeAttributes(HttpActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes<ContentManagementApiAuthorizeAttribute>().
                Concat<ContentManagementApiAuthorizeAttribute>(descriptor.ControllerDescriptor.GetCustomAttributes<ContentManagementApiAuthorizeAttribute>())
                .OfType<ContentManagementApiAuthorizeAttribute>();
        }

        /// <summary>
        /// Content Management Page Requested
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <returns>Boolean Item</returns>
        private bool ContentManagementPageRequested(HttpActionContext actionContext)
        {
            var contentManagementAuthorizeAttributes = GetContentManagementAuthorizeAttributes(actionContext.ActionDescriptor);

            if (contentManagementAuthorizeAttributes != null && contentManagementAuthorizeAttributes.Any())
                return true;

            return false;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Content Management Authorize Attribute
        /// </summary>
        /// <param name="validate">Validate</param>
        public ContentManagementApiAuthorizeAttribute(bool validate)
        {
            this._validate = validate;
            _jWTTokenService = AppEngineContext.Current.Resolve<IJWTTokenService>();
            _appContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

        #endregion Constructor

        #region Methods

        public override void OnAuthorization(HttpActionContext actionContext)
        {        
            if (_validate)
                return;

            var userService = AppEngineContext.Current.Resolve<IUserService>();

            //------Hard code authentication ------------------------------------
            #if DEBUG
            var fakeUser = userService.GetAllUsers().OrderBy(x => x.Id).Where(x => x.UserType == UserTypes.Super).FirstOrDefault();
            if (fakeUser == null)
            {
                fakeUser = new User { Id = 1, Username = "admin", UserType = UserTypes.Super };
            }

            HttpContext.Current.Request.Headers["CurrentUserId"] = fakeUser.Id.ToString();

            var fakeToken = _jWTTokenService.GenerateAccessToken(fakeUser.Id, fakeUser.Username);
    
            if (!string.IsNullOrEmpty(fakeToken) && !actionContext.Request.Headers.Contains("Authorization"))
            {
                actionContext.Request.Headers.Add("Authorization", fakeToken);
            }
            var refreshTokens = _jWTTokenService.GetAllRefreshTokens() ?? new Dictionary<long, string>();
            var fakeRefreshToken = _jWTTokenService.GenerateRefreshToken(fakeUser.Id, fakeUser.Username);
            refreshTokens[fakeUser.Id] = fakeRefreshToken;
            _jWTTokenService.InsertRefreshToken(refreshTokens);
            #endif
            //-------------------------------------------------------------     

            IEnumerable<string> headerValues;
            actionContext.Request.Headers.TryGetValues("Authorization", out headerValues);

            string token = headerValues != null ? headerValues.First() : "";
            token = token.Replace("Bearer ", "").Replace("bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                HandleUnauthorizedRequest(actionContext, JWTVerficationResults.Invalid);
                return;
            }

            //Utils.SetupConnectionString(actionContext.Request); 
            JWTVerficationResults jWTVerficationResult = JWTVerficationResults.None;
            long userId = _jWTTokenService.VerifyUser(token, out jWTVerficationResult);
            HttpContext.Current.Request.Headers["CurrentUserId"] = userId.ToString();

            bool result = _jWTTokenService.Verify(token, userId);
            var user = userService.GetUserById(userId);

            if (user == null || jWTVerficationResult != JWTVerficationResults.Valid || !result)
            {
                HandleUnauthorizedRequest(actionContext, jWTVerficationResult);
                return;
            }

            if (ContentManagementPageRequested(actionContext))
            {
                actionContext.RequestContext.Principal = new CustomPrincipal(user);
            }
            
            base.OnAuthorization(actionContext);
        }

        #endregion Methods


        #region Private Methods



        #endregion
    }
}