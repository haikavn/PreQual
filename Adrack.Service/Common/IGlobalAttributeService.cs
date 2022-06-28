// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IGlobalAttributeService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Adrack.Core;
using Adrack.Core.Domain.Common;
using System.Collections.Generic;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Global Attribute Service
    /// </summary>
    public partial interface IGlobalAttributeService
    {
        #region Methods

        /// <summary>
        /// Get Global Attribute By Id
        /// </summary>
        /// <param name="globalAttributeId">Global Attribute Identifier</param>
        /// <returns>Global Attribute Item</returns>
        GlobalAttribute GetGlobalAttributeById(long globalAttributeId);

        /// <summary>
        /// Get Global Attribute By Entity Id And Key Group
        /// </summary>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="keyGroup">Key Group</param>
        /// <returns>Global Attribute Collection Item</returns>
        IList<GlobalAttribute> GetGlobalAttributeByEntityIdAndKeyGroup(long entityId, string keyGroup);

        /// <summary>
        /// Get Global Attribute By Key And Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Global Attribute Item</returns>
        GlobalAttribute GetGlobalAttributeByKeyAndValue(string pKey, string pValue);


        /// <summary>
        /// Save Global Attribute Only Key And Value
        /// </summary>
        /// <typeparam name="pKey">Key</typeparam>
        /// <param name="pValue">Value</param>
        /// <param name="pExtraData">ExtraData</param>
        /// <exception cref="ArgumentNullException">key or value</exception>
        void SaveGlobalAttributeOnlyKeyAndValue(string pKey, string pValue, string pExtraData = "");

        /// <summary>
        /// Delete Global Attribute By Key And Value
        /// </summary>
        /// <typeparam name="pKey">Key</typeparam>
        /// <param name="pValue">Value</param>
        /// <exception cref="ArgumentNullException">key or value</exception>
        void DeleteGlobalAttributeByKeyAndValue(string pKey, string pValue);

        /// <summary>
        /// Get All Global Attributes
        /// </summary>
        /// <returns>Global Attribute Collection Item</returns>
        IList<GlobalAttribute> GetAllGlobalAttributes();

        /// <summary>
        /// Insert Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        void InsertGlobalAttribute(GlobalAttribute globalAttribute);

        /// <summary>
        /// Save Global Attribute
        /// </summary>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="baseEntity">Base Entity</param>
        /// <param name="key">Key</param>
        /// <param name="propTypeValue">Property Type Value</param>
        void SaveGlobalAttribute<TPropType>(BaseEntity baseEntity, string key, TPropType propTypeValue);

        /// <summary>
        /// Update Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        void UpdateGlobalAttribute(GlobalAttribute globalAttribute);

        /// <summary>
        /// Delete Global Attribute
        /// </summary>
        /// <param name="globalAttribute">Global Attribute</param>
        void DeleteGlobalAttribute(GlobalAttribute globalAttribute);

        #endregion Methods
    }
}