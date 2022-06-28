// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ILocalizedModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized Model
    /// </summary>
    public interface ILocalizedModel
    {
    }

    /// <summary>
    /// Represents a Localized Model
    /// Implements the <see cref="Adrack.Web.Framework.Localization.ILocalizedModel" />
    /// </summary>
    /// <typeparam name="TLocalizedModel">The type of the t localized model.</typeparam>
    /// <seealso cref="Adrack.Web.Framework.Localization.ILocalizedModel" />
    public interface ILocalizedModel<TLocalizedModel> : ILocalizedModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Locales
        /// </summary>
        /// <value>The locales.</value>
        IList<TLocalizedModel> Locales { get; set; }

        #endregion Properties
    }
}