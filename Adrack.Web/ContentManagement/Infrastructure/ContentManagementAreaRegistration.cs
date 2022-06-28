// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ContentManagementAreaRegistration.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Infrastructure
{
    /// <summary>
    /// Represents a Content Management Area Registration
    /// Implements the <see cref="System.Web.Mvc.AreaRegistration" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public class ContentManagementAreaRegistration : AreaRegistration
    {
        #region Methods

        /// <summary>
        /// Register Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Management_Default", "Management/{controller}/{action}/{id}", new { controller = "Home", action = "Index", area = "Management", id = "" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Area Name
        /// </summary>
        /// <value>The name of the area.</value>
        public override string AreaName
        {
            get
            {
                return "Management";
            }
        }

        #endregion Properties
    }
}