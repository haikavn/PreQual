// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ContentController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Content Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class ContentController : BasePublicController
    {
        #region Methods

        /// <summary>
        ///     Artist
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult Artist()
        {
            return View();
        }

        /// <summary>
        ///     Album
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult Album()
        {
            return View();
        }

        #endregion Methods
    }
}