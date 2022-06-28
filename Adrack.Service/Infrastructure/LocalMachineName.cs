// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LocalMachineName.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Service.Infrastructure
{
    /// <summary>
    /// Represents a Local Machine Name
    /// Implements the <see cref="Adrack.Service.Infrastructure.IMachineName" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Infrastructure.IMachineName" />
    public class LocalMachineName : IMachineName
    {
        #region Methods

        /// <summary>
        /// Get Machine Name
        /// </summary>
        /// <returns>String Item</returns>
        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        #endregion Methods
    }
}