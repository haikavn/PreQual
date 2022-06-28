// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="PageLayoutPosition.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Web.Framework.UI
{
    /// <summary>
    /// Represents a Page Layout Position Enumeration
    /// </summary>
    public enum PageLayoutPosition
    {
        #region Enumeration

        /// <summary>
        /// Head
        /// </summary>
        Head,

        /// <summary>
        /// Body
        /// </summary>
        Body,

        /// <summary>
        /// Html Attribute
        /// </summary>
        HtmlAttribute,

        /// <summary>
        /// Body Attribute
        /// </summary>
        BodyAttribute,

        /// <summary>
        /// Container Attribute
        /// </summary>
        ContainerAttribute,

        /// <summary>
        /// Container Content Attribute
        /// </summary>
        ContainerContentAttribute,

        /// <summary>
        /// Container Content Wrapper Attribute
        /// </summary>
        ContainerContentWrapperAttribute,

        /// <summary>
        /// Content Attribute
        /// </summary>
        ContentAttribute

        #endregion Enumeration
    }
}