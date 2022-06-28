// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="INavigationSession.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Web.Framework.UI.Navigation
{
    /// <summary>
    /// Represents a Navigation Session
    /// </summary>
    public interface INavigationSession
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Session Identifier
        /// </summary>
        /// <value>The session identifier.</value>
        string SessionId { get; }

        #endregion Properties
    }
}