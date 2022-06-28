// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="GlobalAttributeExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using System;
using System.Linq;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Global Attribute Extensions
    /// </summary>
    public static class GlobalAttributeExtensions
    {
        #region Methods

        /// <summary>
        /// Get Global Attribute
        /// </summary>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="baseEntity">Base Entity</param>
        /// <param name="key">Key</param>
        /// <returns>Global Attribute Item</returns>
        public static TPropType GetGlobalAttribute<TPropType>(this BaseEntity baseEntity, string key)
        {
            var globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();

            return GetGlobalAttribute<TPropType>(baseEntity, key, globalAttributeService);
        }

        /// <summary>
        /// Get Global Attribute
        /// </summary>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="baseEntity">Base Entity</param>
        /// <param name="key">Key</param>
        /// <param name="globalAttributeService">Global Attribute Service</param>
        /// <returns>Global Attribute Item</returns>
        /// <exception cref="ArgumentNullException">baseEntity</exception>
        public static TPropType GetGlobalAttribute<TPropType>(this BaseEntity baseEntity, string key, IGlobalAttributeService globalAttributeService)
        {
            if (baseEntity == null)
                throw new ArgumentNullException("baseEntity");

            string keyGroup = baseEntity.GetUnproxiedEntityType().Name;

            var globalAttributeList = globalAttributeService.GetGlobalAttributeByEntityIdAndKeyGroup(baseEntity.Id, keyGroup);

            if (globalAttributeList == null)
                return default(TPropType);

            if (globalAttributeList.Count == 0)
                return default(TPropType);

            var globalAttribute = globalAttributeList.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (globalAttribute == null || string.IsNullOrEmpty(globalAttribute.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(globalAttribute.Value);
        }

        #endregion Methods
    }
}