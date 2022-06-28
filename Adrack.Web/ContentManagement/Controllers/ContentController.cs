// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ContentController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Represents a Content Controller
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class ContentController : BaseContentManagementController
    {
        #region Methods

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Content")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Artist
        /// </summary>
        /// <returns>Action Result Item</returns>
        [NavigationBreadCrumb]
        public ActionResult Artist()
        {
            return View();
        }

        #endregion Methods
    }
}