// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SessionState.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Session State
    /// </summary>
    public class SessionState
    {
        #region Constructor

        /// <summary>
        /// Session State
        /// </summary>
        /// <param name="cookie">Cookie</param>
        public SessionState(string cookie)
        {
            SessionCookie = cookie;
            Crumbs = new List<SessionStateEntry>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Push
        /// </summary>
        /// <param name="actionExecutingContext">Action Executing Context</param>
        /// <param name="label">Label</param>
        public void Push(ActionExecutingContext actionExecutingContext, string label)
        {
            var key =
                actionExecutingContext.HttpContext.Request.Url.ToString()
                .ToLower()
                .GetHashCode();

            if (Crumbs.Any(x => x.Key == key))
            {
                var newCrumbs = new List<SessionStateEntry>();
                var remove = false;
                // We've seen this route before, maybe user clicked on a breadcrumb
                foreach (var crumb in Crumbs)
                {
                    if (crumb.Key == key)
                    {
                        remove = true;
                    }
                    if (!remove)
                    {
                        newCrumbs.Add(crumb);
                    }
                }
                Crumbs = newCrumbs;
            }

            Current = new SessionStateEntry().WithKey(key)
                .SetContext(actionExecutingContext)
                .WithUrl(actionExecutingContext.HttpContext.Request.Url.ToString())
                .WithLabel(label);

            Crumbs.Add(Current);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Session Cookie
        /// </summary>
        /// <value>The session cookie.</value>
        public string SessionCookie { get; set; }

        /// <summary>
        /// Gets or Sets the Crumbs
        /// </summary>
        /// <value>The crumbs.</value>
        public List<SessionStateEntry> Crumbs { get; set; }

        /// <summary>
        /// Gets or Sets the Current
        /// </summary>
        /// <value>The current.</value>
        public SessionStateEntry Current { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Session State Entry
    /// </summary>
    public class SessionStateEntry
    {
        #region Constructor

        /// <summary>
        /// Session State Entry
        /// </summary>
        /// <param name="key">Kye</param>
        /// <returns>Session State Entry Item</returns>
        public SessionStateEntry WithKey(int key)
        {
            Key = key;
            return this;
        }

        /// <summary>
        /// Session State Entry
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Session State Entry Item</returns>
        public SessionStateEntry WithUrl(string url)
        {
            Url = url;
            return this;
        }

        /// <summary>
        /// Session State Entry
        /// </summary>
        /// <param name="label">Label</param>
        /// <returns>Session State Entry Item</returns>
        public SessionStateEntry WithLabel(string label)
        {
            ActionLabel = label ?? ActionLabel;

            return this;
        }

        /// <summary>
        /// Session State Entry
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Session State Entry Item</returns>
        public SessionStateEntry SetContext(ActionExecutingContext context)
        {
            Context = context;

            ControllerLabel = ControllerLabel ?? Controller;
            ActionLabel = ActionLabel ?? Action;

            return this;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or Sets the Context
        /// </summary>
        /// <value>The context.</value>
        public ActionExecutingContext Context { get; private set; }

        /// <summary>
        /// Gets or Sets the Controller Label
        /// </summary>
        /// <value>The controller label.</value>
        public string ControllerLabel { get; set; }

        /// <summary>
        /// Gets or Sets the Action Label
        /// </summary>
        /// <value>The action label.</value>
        public string ActionLabel { get; set; }

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public int Key { get; set; }

        /// <summary>
        /// Gets or Sets the Url
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets the Controller
        /// </summary>
        /// <value>The controller.</value>
        private string Controller
        {
            get
            {
                return (string)Context.RouteData.Values["controller"];
            }
        }

        /// <summary>
        /// Gets or Sets the Action
        /// </summary>
        /// <value>The action.</value>
        private string Action
        {
            get
            {
                return (string)Context.RouteData.Values["action"];
            }
        }

        #endregion Properties
    }
}