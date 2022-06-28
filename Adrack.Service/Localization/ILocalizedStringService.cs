// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILocalizedStringService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using System.Collections.Generic;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Localized String Service
    /// </summary>
    public partial interface ILocalizedStringService
    {
        #region Methods

        /// <summary>
        /// Get Localized String By Id
        /// </summary>
        /// <param name="localizedStringId">Localized String Identifier</param>
        /// <returns>Localized String Item</returns>
        LocalizedString GetLocalizedStringById(long localizedStringId);

        /// <summary>
        /// Get Localized String By Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Localized String Item</returns>
        LocalizedString GetLocalizedStringByKey(string key);

        /// <summary>
        /// Get Localized String By Key
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <returns>Localized String Item</returns>
        LocalizedString GetLocalizedStringByKey(long languageId, string key);

        /// <summary>
        /// Get All Localized Strings
        /// </summary>
        /// <returns>Localized String Collection Item</returns>
        IList<LocalizedString> GetAllLocalizedStrings();

        /// <summary>
        /// Get All Localized Strings
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Localized String Collection Item</returns>
        IList<LocalizedString> GetAllLocalizedStrings(long languageId);

        /// <summary>
        /// Get All Localized String Values
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Localized String Collection Item</returns>
        Dictionary<string, KeyValuePair<long, string>> GetAllLocalizedStringValues(long languageId);

        /// <summary>
        /// Get Localized String
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        string GetLocalizedString(string key);

        /// <summary>
        /// Get Localized String
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="returnEmptyIfNotFound">Return Empty If Not Found</param>
        /// <returns>String Item</returns>
        string GetLocalizedString(long languageId, string key, string defaultValue = "", bool returnEmptyIfNotFound = false);

        /// <summary>
        /// Insert Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        void InsertLocalizedString(LocalizedString localizedString);

        /// <summary>
        /// Update Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        void UpdateLocalizedString(LocalizedString localizedString);

        /// <summary>
        /// Delete Localized String
        /// </summary>
        /// <param name="localizedString">Localized String</param>
        void DeleteLocalizedString(LocalizedString localizedString);

        #endregion Methods
    }
}