// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AuthenticationSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Authentication Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class AuthenticationSetting : ISetting
    {
        #region Constructor

        /// <summary>
        /// Authentication Setting
        /// </summary>
        public AuthenticationSetting()
        {
            ActiveAuthenticationMethodKey = new List<string>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Auto Register Enabled
        /// </summary>
        /// <value><c>true</c> if [automatic register enabled]; otherwise, <c>false</c>.</value>
        public bool AutoRegisterEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Active Authentication Method Key
        /// </summary>
        /// <value>The active authentication method key.</value>
        public List<string> ActiveAuthenticationMethodKey { get; set; }

        #endregion Properties
    }
}