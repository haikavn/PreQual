// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SessionStateManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Session State Manager
    /// </summary>
    public class SessionStateManager
    {
        #region Fields

        /// <summary>
        /// Session State
        /// </summary>
        public static readonly List<SessionState> SessionStates = new List<SessionState>();

        #endregion Fields



        #region Methods

        /// <summary>
        /// Get Session State By Id
        /// </summary>
        /// <param name="id">Session Identifier</param>
        /// <returns>SessionState.</returns>
        public static SessionState GetSessionState(string id)
        {
            if (SessionStates.FirstOrDefault(x => x.SessionCookie == id) == null)
            {
                CreateSessionState(id);
            }

            return SessionStates.First(x => x.SessionCookie == id);
        }

        /// <summary>
        /// Create Session State
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        /// <returns>Session State Item</returns>
        public static SessionState CreateSessionState(string cookie)
        {
            var newSessionState = new SessionState(cookie);

            SessionStates.Add(newSessionState);

            return newSessionState;
        }

        /// <summary>
        /// Remove State
        /// </summary>
        /// <param name="id">Session Identifier</param>
        public static void RemoveState(string id)
        {
            var sessionState = GetSessionState(id);

            if (sessionState != null)
            {
                SessionStates.Remove(sessionState);
            }
        }

        #endregion Methods
    }
}