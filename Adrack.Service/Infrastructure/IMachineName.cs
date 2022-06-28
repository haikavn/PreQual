// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IMachineName.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Infrastructure
{
    /// <summary>
    /// Represents a Machine Name
    /// </summary>
    public interface IMachineName
    {
        #region Methods

        /// <summary>
        /// Machine Name
        /// </summary>
        /// <returns>System.String.</returns>
        string GetMachineName();

        #endregion Methods
    }
}