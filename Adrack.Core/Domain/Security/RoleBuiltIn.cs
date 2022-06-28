// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RoleBuiltIn.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Represents a Role Built In
    /// </summary>
    public static partial class RoleBuiltIn
    {
        #region Properties

        /// <summary>
        /// Built-in global administrators have complete and unrestricted access to the full website
        /// </summary>
        /// <value>The global administrators.</value>
        public static string GlobalAdministrators
        {
            get
            {
                return "GlobalAdministrators";
            }
        }

        /// <summary>
        /// Built-in content managers have restricted access to the website
        /// </summary>
        /// <value>The content managers.</value>
        public static string ContentManagers
        {
            get
            {
                return "ContentManagers";
            }
        }

        /// <summary>
        /// Built-in network user are registered users
        /// </summary>
        /// <value>The network users.</value>
        public static string NetworkUsers
        {
            get
            {
                return "NetworkUsers";
            }
        }

        #endregion Properties
    }
}