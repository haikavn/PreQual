// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserTypeBuiltIn.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a User Type Built In
    /// </summary>
    public static partial class UserTypeBuiltIn
    {
        #region Properties

        /// <summary>
        /// Built In User Type
        /// </summary>
        /// <value>The built in.</value>
        public static string BuiltIn
        {
            get
            {
                return "BuiltIn";
            }
        }

        /// <summary>
        /// Affiliate User Type
        /// </summary>
        /// <value>The affiliate.</value>
        public static string Affiliate
        {
            get
            {
                return "Affiliate";
            }
        }

        /// <summary>
        /// Buyer User Type
        /// </summary>
        /// <value>The buyer.</value>
        public static string Buyer
        {
            get
            {
                return "Buyer";
            }
        }

        /// <summary>
        /// Network User User Type
        /// </summary>
        /// <value>The network user.</value>
        public static string NetworkUser
        {
            get
            {
                return "NetworkUser";
            }
        }

        #endregion Properties
    }
}