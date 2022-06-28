// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="Pager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web;

namespace Adrack.Web.Framework.UI.Pagination
{
    /// <summary>
    /// Represents a Pager
    /// Implements the <see cref="System.Web.IHtmlString" />
    /// </summary>
    /// <seealso cref="System.Web.IHtmlString" />
    public partial class Pager : IHtmlString
    {
        #region Methods

        /// <summary>
        /// To Html String
        /// </summary>
        /// <returns>String Item</returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ToHtmlString()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}