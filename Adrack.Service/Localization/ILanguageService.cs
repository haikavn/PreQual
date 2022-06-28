// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILanguageService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using System.Collections.Generic;

namespace Adrack.Service.Localization
{
    /// <summary>
    /// Represents a Language Service
    /// </summary>
    public partial interface ILanguageService
    {
        #region Methods

        /// <summary>
        /// Get Language By Id
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Language Item</returns>
        Language GetLanguageById(long languageId);

        /// <summary>
        /// Get All Languages
        /// </summary>
        /// <returns>Language Collection Item</returns>
        IList<Language> GetAllLanguages();

        /// <summary>
        /// Insert Language
        /// </summary>
        /// <param name="language">Language</param>
        void InsertLanguage(Language language);

        /// <summary>
        /// Update Language
        /// </summary>
        /// <param name="language">Language</param>
        void UpdateLanguage(Language language);

        /// <summary>
        /// Delete Language
        /// </summary>
        /// <param name="language">Language</param>
        void DeleteLanguage(Language language);

        #endregion Methods
    }
}