// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="Localized.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>LocalizedStringValue.</returns>
    public delegate LocalizedStringValue Localized(string text, params object[] args);
}