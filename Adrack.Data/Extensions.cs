// ***********************************************************************
// Assembly         : Adrack.Data
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

using Adrack.Core;
using System;
using System.Data.Entity.Core.Objects;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Extensions
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Get Unproxied Entity Type (If your Entity Framework context is proxy-enabled, the runtime will create a proxy instance of your entities, i.e. a dynamically generated class which inherits from your entity class and overrides its virtual properties by inserting specific code useful for example for tracking changes and lazy loading)
        /// </summary>
        /// <param name="baseEntity">Base Entity</param>
        /// <returns>Type</returns>
        public static Type GetUnproxiedEntityType(this BaseEntity baseEntity)
        {
            var objectType = ObjectContext.GetObjectType(baseEntity.GetType());

            return objectType;
        }

        #endregion Methods
    }
}