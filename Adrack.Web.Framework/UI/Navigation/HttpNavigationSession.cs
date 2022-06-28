// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="HttpNavigationSession.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web;

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Http Navigation Session
    /// Implements the <see cref="Adrack.Web.Framework.UI.Navigation.INavigationSession" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.UI.Navigation.INavigationSession" />
    public class HttpNavigationSession : INavigationSession
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Session Identifier
        /// </summary>
        /// <value>The session identifier.</value>
        public string SessionId
        {
            get
            {
                var sessionID = HttpContext.Current.Session.SessionID;

                HttpContext.Current.Session["SessionId"] = sessionID;

                return sessionID;
            }
        }

        #endregion Properties
    }
}