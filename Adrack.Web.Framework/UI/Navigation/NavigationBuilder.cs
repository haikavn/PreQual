// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="NavigationBuilder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;
using Adrack.Service.Common;
using Adrack.Service.Helpers;
using Adrack.Service.Localization;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Navigation Builder
    /// Implements the <see cref="Adrack.Web.Framework.UI.Navigation.NavigationBase" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.UI.Navigation.NavigationBase" />
    public class NavigationBuilder : NavigationBase
    {
        #region Utilities

        /// <summary>
        /// Authorizes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>List&lt;NavigationItem&gt;.</returns>
        private List<NavigationItem> Authorize(List<NavigationItem> items)
        {
            var appContext = AppEngineContext.Current.Resolve<IAppContext>();
            var logService = AppEngineContext.Current.Resolve<ILogService>();
            var localizedStringService = AppEngineContext.Current.Resolve<ILocalizedStringService>();
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            var navigationItem = new List<NavigationItem>();

            foreach (var item in items)
            {
                if (appContext.AppUser != null && (appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || appContext.AppUser.UserType == SharedData.NetowrkUserTypeId))
                {
                    item.Key = localizedStringService.GetLocalizedString(item.Key);
                    navigationItem.Add(item);
                    continue;
                }

                try
                {
                    if (item.Permission.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Any(x => permissionService.Authorize(permissionService.GetPermissionById(Int64.Parse(x.Trim())).Key)))
                    {
                        item.Key = localizedStringService.GetLocalizedString(item.Key);

                        navigationItem.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    logService.Error(ex.Message, ex);
                }
            }

            return navigationItem;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Navigation Builder
        /// </summary>
        /// <param name="layout">Layout</param>
        public NavigationBuilder(string layout) : base(layout)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Load
        /// </summary>
        /// <returns>Navigation Item Collection</returns>
        public override List<NavigationItem> Load()
        {
            var navigationService = AppEngineContext.Current.Resolve<INavigationService>();

            var navigationRoot = navigationService.GetNavigationByLayout(Layout);

            List<NavigationItem> root;
            List<NavigationItem> child;

            root = BuildRoot(navigationRoot.ToList());

            child = BuildChild(root, 0);

            return child;
        }

        /// <summary>
        /// Build Root
        /// </summary>
        /// <param name="navigation">Navigation</param>
        /// <returns>Navigation Item Collection</returns>
        public virtual List<NavigationItem> BuildRoot(List<Core.Domain.Common.Navigation> navigation)
        {
            var navigationItem = new List<NavigationItem>();

            var query = (from item in navigation
                         select new NavigationItem
                         {
                             Id = item.Id,
                             ParentId = item.ParentId,
                             Layout = item.Layout,
                             Key = item.Key,
                             Controller = item.Controller,
                             Action = item.Action,
                             Permission = item.Permission,
                             HtmlClass = item.HtmlClass,
                             Url = item.Url,
                             ImageUrl = item.ImageUrl,
                             Published = item.Published,
                             Deleted = item.Deleted,
                             DisplayOrder = item.DisplayOrder,
                             Color = item.Color
                         });

            if (query != null)
            {
                navigationItem = Authorize(query.ToList());
            }

            return navigationItem;
        }

        /// <summary>
        /// Build Child
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="parentId">Parent Identifier</param>
        /// <returns>Navigation Item Collection</returns>
        private List<NavigationItem> BuildChild(List<NavigationItem> child, long parentId)
        {
            var navigationItem = new List<NavigationItem>();

            navigationItem = (from item in child
                              where item.ParentId == parentId
                              orderby item.DisplayOrder
                              select new NavigationItem
                              {
                                  Id = item.Id,
                                  ParentId = item.ParentId,
                                  Layout = item.Layout,
                                  Key = item.Key,
                                  Controller = item.Controller,
                                  Action = item.Action,
                                  Permission = item.Permission,
                                  HtmlClass = item.HtmlClass,
                                  Url = item.Url,
                                  ImageUrl = item.ImageUrl,
                                  Published = item.Published,
                                  Deleted = item.Deleted,
                                  DisplayOrder = item.DisplayOrder,
                                  Color = item.Color,
                                  Child = (parentId != item.Id ? BuildChild(child, item.Id) : new List<NavigationItem>())
                              }).ToList();

            return navigationItem;
        }

        #endregion Methods
    }
}