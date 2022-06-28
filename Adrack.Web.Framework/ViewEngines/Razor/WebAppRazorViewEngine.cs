// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="WebAppRazorViewEngine.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Represents a Web Application Razor View Engine
    /// Implements the <see cref="Adrack.Web.Framework.ViewEngines.Razor.WebAppVirtualPathProviderViewEngine" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.ViewEngines.Razor.WebAppVirtualPathProviderViewEngine" />
    public class WebAppRazorViewEngine : WebAppVirtualPathProviderViewEngine
    {
        #region Utilities

        /// <summary>
        /// Create Partial View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="partialPath">Partial Path</param>
        /// <returns>View Item</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;

            var runViewStartPages = false;

            IEnumerable<string> fileExtensions = base.FileExtensions;

            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions);

            //return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="viewPath">View Path</param>
        /// <param name="masterPath">Master Path</param>
        /// <returns>View Item</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;

            var runViewStartPages = true;

            IEnumerable<string> fileExtensions = base.FileExtensions;

            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions);
        }

        #endregion Utilities



        #region Methods

        /// <summary>
        /// Web Application Razor View Engine
        /// </summary>
        public WebAppRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
                                          {
                                              // Default
                                              "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                              "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                          };

            AreaMasterLocationFormats = new[]
                                            {
                                                // Default
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                            };

            AreaPartialViewLocationFormats = new[]
                                                 {
                                                    // Default
                                                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                                                 };

            ViewLocationFormats = new[]
                                      {
                                            // Default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml",

                                            // Member
                                            "~/Member/Views/{1}/{0}.cshtml",
                                            "~/Member/Views/Shared/{0}.cshtml",

                                            // Global Administrator
                                            "~/ContentManagement/Views/{1}/{0}.cshtml",
                                            "~/ContentManagement/Views/Shared/{0}.cshtml",
                                      };

            MasterLocationFormats = new[]
                                        {
                                            // Default
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml"
                                        };

            PartialViewLocationFormats = new[]
                                             {
                                                // Default
                                                "~/Views/{1}/{0}.cshtml",
                                                "~/Views/Shared/{0}.cshtml",

                                                // Member
                                                "~/Member/Views/{1}/{0}.cshtml",
                                                "~/Member/Views/Shared/{0}.cshtml",

                                                // Global Administrator
                                                "~/ContentManagement/Views/{1}/{0}.cshtml",
                                                "~/ContentManagement/Views/Shared/{0}.cshtml",
                                             };

            FileExtensions = new[] { "cshtml" };
        }

        #endregion Methods
    }
}