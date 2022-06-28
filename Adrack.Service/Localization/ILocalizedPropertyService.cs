// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILocalizedPropertyService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Localized Property Service
    /// </summary>
    public partial interface ILocalizedPropertyService
    {
        #region Methods

        /// <summary>
        /// Get Localized Property By Id
        /// </summary>
        /// <param name="localizedPropertyId">Localized Property Identifier</param>
        /// <returns>Localized Property Item</returns>
        LocalizedProperty GetLocalizedPropertyById(long localizedPropertyId);

        /// <summary>
        /// Get Localized Value
        /// </summary>
        /// <param name="entityId">Entity Identifier</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="keyGroup">Key Group</param>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        string GetLocalizedPropertyValue(long entityId, long languageId, string keyGroup, string key);

        /// <summary>
        /// Get All Localized Properties
        /// </summary>
        /// <returns>Localized Property Collection Item</returns>
        IList<LocalizedProperty> GetAllLocalizedProperties();

        /// <summary>
        /// Insert Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        void InsertLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// Update Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        void UpdateLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// Save Localized Property Value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void SaveLocalizedPropertyValue<T>(T entity, long languageId, Expression<Func<T, string>> key, string value) where T : BaseEntity, ILocalizedEntity;

        /// <summary>
        /// Save Localized Property Value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Type Property</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void SaveLocalizedPropertyValue<T, TPropType>(T entity, long languageId, Expression<Func<T, TPropType>> key, TPropType value) where T : BaseEntity, ILocalizedEntity;

        /// <summary>
        /// Delete Localized Property
        /// </summary>
        /// <param name="localizedProperty">Localized Property</param>
        void DeleteLocalizedProperty(LocalizedProperty localizedProperty);

        #endregion Methods
    }
}