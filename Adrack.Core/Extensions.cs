// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Extensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Extensions
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Check String If Null
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Value</param>
        /// <returns>Boolean</returns>
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        #endregion Methods
    }
}