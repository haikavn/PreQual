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
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    public class ContentManagementAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region Fields

        /// <summary>
        /// Validate
        /// </summary>
        private readonly bool _validate;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Handle Unauthorized Request
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        private void HandleUnauthorizedRequest(AuthorizationContext authorizationContext)
        {
            authorizationContext.Result = new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Handles the not found request.
        /// </summary>
        /// <param name="authorizationContext">The authorization context.</param>
        private void HandleNotFoundRequest(AuthorizationContext authorizationContext)
        {
            authorizationContext.Result = new HttpNotFoundResult();
        }

        /// <summary>
        /// Get Content Management Authorize Attributes
        /// </summary>
        /// <param name="descriptor">Descriptor</param>
        /// <returns>Content Management Authorize Attribute Collection Item</returns>
        private IEnumerable<ContentManagementAuthorizeAttribute> GetContentManagementAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(ContentManagementAuthorizeAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(ContentManagementAuthorizeAttribute), true))
                .OfType<ContentManagementAuthorizeAttribute>();
        }

        /// <summary>
        /// Content Management Page Requested
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <returns>Boolean Item</returns>
        private bool ContentManagementPageRequested(AuthorizationContext authorizationContext)
        {
            var contentManagementAuthorizeAttributes = GetContentManagementAuthorizeAttributes(authorizationContext.ActionDescriptor);

            if (contentManagementAuthorizeAttributes != null && contentManagementAuthorizeAttributes.Any())
                return true;

            return false;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Content Management Authorize Attribute
        /// </summary>
        public ContentManagementAuthorizeAttribute()
            : this(false)
        {
        }

        /// <summary>
        /// Content Management Authorize Attribute
        /// </summary>
        /// <param name="validate">Validate</param>
        public ContentManagementAuthorizeAttribute(bool validate)
        {
            this._validate = validate;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <exception cref="ArgumentNullException">authorizationContext</exception>
        /// <exception cref="InvalidOperationException">You cannot use [ContentManagementAuthorize] attribute when a child action cache is active</exception>
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
                throw new ArgumentNullException("authorizationContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(authorizationContext))
                throw new InvalidOperationException("You cannot use [ContentManagementAuthorize] attribute when a child action cache is active");

            if (_validate)
                return;

            var routeData = authorizationContext.RequestContext.RouteData;
            var controller = routeData.GetRequiredString("controller").ToLower();
            var action = routeData.GetRequiredString("action").ToLower();

            var id = "";

            string accessToken = authorizationContext.RequestContext.HttpContext.Request.Headers["Authorization"];

            /*try
            {
                //arman to check
                id = routeData.GetRequiredString("id");
                id = id;
            }
            catch
            {
            }*/

            // Call this outside of login
            if (controller.ToLower() == "accounting" && action.ToLower() == "generateinvoices")
                return;

            if (controller.ToLower() == "affiliatechannel" && action.ToLower() == "postspecification")
                return;

            if (action.ToLower() == "buyerreportapi")
                return;

            if (ContentManagementPageRequested(authorizationContext))
            {
                var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
                User user = permissionService.GetAppContext().AppUser;

                var appContext = AppEngineContext.Current.Resolve<IAppContext>();
                HttpCookie userCookie = appContext.GetUserCookie();

                if (user != null && appContext.CheckUserExpire(user.Id) && action.ToLower() != "gettimezonenow" && !authorizationContext.IsChildAction)
                {
                    authorizationContext.Result = new RedirectToRouteResult("Logout", new System.Web.Routing.RouteValueDictionary());
                    return;
                }

                var navigationService = AppEngineContext.Current.Resolve<INavigationService>();

                bool result = permissionService.Authorize(PermissionProvider.AccessContentManagementPageApplication);

                if (user != null && user.ChangePassOnLogin.HasValue && user.ChangePassOnLogin.Value)
                {
                    authorizationContext.Result = new RedirectToRouteResult("ChangePassword", new System.Web.Routing.RouteValueDictionary());
                }
                else
                {
                    if (!result)
                    {
                        HandleUnauthorizedRequest(authorizationContext);
                    }
                    else
                    {
                        if (action.ToLower() != "gettimezonenow")
                        {
                            try
                            {
                                appContext.SetUserCookie(Guid.Parse(user.GuId));
                                appContext.SetUserExpire(user.Id);
                            }
                            catch { }
                        }

                        if (user.UserType != UserTypes.Super)
                            result = navigationService.CheckPermission("ContentManagement", controller, action, false);

                        if (user != null && user.UserType != UserTypes.Super && user.UserType != UserTypes.Network)
                        {
                            if ((user.UserType != UserTypes.Buyer && controller == "buyer" && action == "item" && id != user.ParentId.ToString()) ||
                                (user.UserType != UserTypes.Affiliate && controller == "affiliate" && action == "item" && id != user.ParentId.ToString())
                                )
                            {
                                result = false;
                            }

                            if (controller == "affiliatechannel" && action == "item")
                            {
                                long longId = 0;
                                if (long.TryParse(id, out longId))
                                {
                                    if (user.UserType == UserTypes.Buyer)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        var aChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
                                        AffiliateChannel channel = aChannelService.GetAffiliateChannelById(longId);
                                        if (channel != null && user.ParentId != channel.AffiliateId) result = false;
                                    }
                                }
                            }

                            if (controller == "buyerchannel" && action == "item")
                            {
                                long longId = 0;
                                if (long.TryParse(id, out longId))
                                {
                                    if (user.UserType == UserTypes.Affiliate)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        var userService = AppEngineContext.Current.Resolve<IUserService>();

                                        var bChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>();
                                        BuyerChannel channel = bChannelService.GetBuyerChannelById(longId);

                                        if (channel != null)
                                        {
                                            if (userService.GetUserBuyerChannels(user.Id).Where(x => x.BuyerChannelId == channel.Id).FirstOrDefault() == null)
                                            {
                                                if (user.ParentId != channel.BuyerId) result = false;
                                            }
                                            else
                                                result = true;
                                        }
                                    }
                                }
                            }
                        }

                        /*try
                        {
                            switch (user.UserTypeId)
                            {
                                case 3:
                                    if ((controller.ToLower() != "affiliate" && controller.ToLower() != "affiliatechannel") || (controller.ToLower() == "affiliate" && user.ParentId.ToString() != id)) result = false;
                                    else
                                        if (controller.ToLower() == "affiliatechannel")
                                        {
                                            var affiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
                                            AffiliateChannel channel = affiliateChannelService.GetAffiliateChannelById(long.Parse(id));

                                            if (channel == null || (channel != null && channel.AffiliateId != user.ParentId)) result = false;
                                        }
                                    break;

                                case 4:
                                    if ((controller.ToLower() != "buyer" && controller.ToLower() != "buyerchannel") || (controller.ToLower() == "buyer" && user.ParentId.ToString() != id)) result = false;
                                    else
                                        if (controller.ToLower() == "affiliatechannel")
                                        {
                                            var affiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
                                            AffiliateChannel channel = affiliateChannelService.GetAffiliateChannelById(long.Parse(id));

                                            if (channel == null || (channel != null && channel.AffiliateId != user.ParentId)) result = false;
                                        }
                                    break;
                            }
                        }
                        catch
                        {
                            result = false;
                        }*/

                        if (!result)
                        {
                            HandleNotFoundRequest(authorizationContext);
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}