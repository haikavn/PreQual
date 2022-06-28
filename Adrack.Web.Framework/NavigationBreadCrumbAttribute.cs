// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="NavigationBreadCrumbAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.UI.Navigation;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Navigation Bread Crumb Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NavigationBreadCrumbAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="actionExecutingContext">Action Executing Context</param>
        /// <exception cref="ArgumentNullException">actionExecutingContext</exception>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext == null)
                throw new ArgumentNullException("actionExecutingContext");

            if (actionExecutingContext.IsChildAction)
                return;

            if (!String.Equals(actionExecutingContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (Clear)
                SessionStateManager.RemoveState(NavigationSession.SessionId);

            var sessionState = SessionStateManager.GetSessionState(NavigationSession.SessionId);

            sessionState.Push(actionExecutingContext, Label);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Navigation Session
        /// </summary>
        /// <value>The navigation session.</value>
        private static INavigationSession _navigationSession { get; set; }

        /// <summary>
        /// Gets or Sets the Navigation Session
        /// </summary>
        /// <value>The navigation session.</value>
        private static INavigationSession NavigationSession
        {
            get
            {
                if (_navigationSession != null)
                {
                    return _navigationSession;
                }

                return new HttpNavigationSession();
            }
        }

        /// <summary>
        /// Gets or Sets the Clear
        /// </summary>
        /// <value><c>true</c> if clear; otherwise, <c>false</c>.</value>
        public bool Clear { get; set; }

        /// <summary>
        /// Gets or Sets the Label
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        #endregion Properties
    }
}