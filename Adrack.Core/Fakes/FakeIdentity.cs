// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakeIdentity.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Security.Principal;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Identity
    /// Implements the <see cref="System.Security.Principal.IIdentity" />
    /// </summary>
    /// <seealso cref="System.Security.Principal.IIdentity" />
    public class FakeIdentity : IIdentity
    {
        #region Fields

        /// <summary>
        /// Name
        /// </summary>
        private readonly string _name;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Identity
        /// </summary>
        /// <param name="userName">User Name</param>
        public FakeIdentity(string userName)
        {
            _name = userName;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Authentication Type
        /// </summary>
        /// <value>The type of the authentication.</value>
        /// <exception cref="NotImplementedException"></exception>
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or Sets the Is Authenticated
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated
        {
            get { return !String.IsNullOrEmpty(_name); }
        }

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
        }

        #endregion Properties
    }
}