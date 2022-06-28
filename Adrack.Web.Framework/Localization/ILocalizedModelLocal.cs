// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ILocalizedModelLocal.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized Model Local
    /// </summary>
    public interface ILocalizedModelLocal
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Language Identifier
        /// </summary>
        /// <value>The language identifier.</value>
        long LanguageId { get; set; }

        #endregion Properties
    }
}