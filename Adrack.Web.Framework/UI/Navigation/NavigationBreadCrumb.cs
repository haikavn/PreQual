// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="NavigationBreadCrumb.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Linq;
using System.Text;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Navigation Bread Crumb
    /// </summary>
    public class NavigationBreadCrumb
    {
        #region Methods

        /// <summary>
        /// Set Label
        /// </summary>
        /// <param name="label">Label</param>
        public static void SetLabel(string label)
        {
            var state = SessionStateManager.GetSessionState(NavigationSession.SessionId);

            state.Current.ActionLabel = label;
        }

        /// <summary>
        /// Display
        /// </summary>
        /// <returns>String Item</returns>
        public static string Display()
        {
            var sessionState = SessionStateManager.GetSessionState(NavigationSession.SessionId);

            if (sessionState.Crumbs != null && !sessionState.Crumbs.Any())
                return "<!-- BreadCrumbs stack is empty -->";

            var stringBuilder = new StringBuilder();

            //stringBuilder.Append("<ul class=\"breadcrumb\">");

            sessionState.Crumbs.ForEach(x =>
            {
                if (sessionState.Current.ActionLabel == x.ActionLabel)
                {
                    stringBuilder.Append("<li class=\"active\">" + x.ActionLabel + "</li>");
                }
                else
                {
                    stringBuilder.Append("<li><a href=\"" + x.Url + "\">" + x.ActionLabel + "</a></li>");
                }
            });

            // stringBuilder.Append("</ul>");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Display Raw
        /// </summary>
        /// <param name="textOnly">Text Only</param>
        /// <returns>String Item</returns>
        public static string DisplayRaw(bool textOnly = true)
        {
            var sessionState = SessionStateManager.GetSessionState(NavigationSession.SessionId);

            if (sessionState.Crumbs != null && !sessionState.Crumbs.Any())
                return "<!-- BreadCrumbs stack is empty -->";

            if (textOnly)
                return string.Join(" > ", sessionState.Crumbs.Select(x => x.ActionLabel).ToArray());

            return string.Join(" > ", sessionState.Crumbs.Select(x => "<a href=\"" + x.Url + "\">" + x.ActionLabel + "</a>").ToArray());
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

        #endregion Properties
    }
}