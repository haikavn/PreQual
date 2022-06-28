// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PingController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Mvc;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Ping Controller
    ///     Implements the <see cref="Controller" />
    /// </summary>
    /// <seealso cref="Controller" />
    public class PingController : Controller
    {
        #region Methods

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        public ActionResult Index()
        {
            return Content("I am alive!");
        }

        #endregion Methods
    }
}