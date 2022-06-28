// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="WebAppVirtualPathProviderViewEngine.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Represents a Web Application Virtual Path Provider View Engine
    /// Implements the <see cref="System.Web.Mvc.VirtualPathProviderViewEngine" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.VirtualPathProviderViewEngine" />
    public abstract class WebAppVirtualPathProviderViewEngine : System.Web.Mvc.VirtualPathProviderViewEngine
    {
        #region Fields

        /// <summary>
        /// Empty Location Array
        /// </summary>
        private readonly string[] _emptyLocations = null;

        /// <summary>
        /// Get Extension Thunk
        /// </summary>
        internal Func<string, string> GetExtensionThunk;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get Path
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="locations">Locations</param>
        /// <param name="areaLocations">Area Locations</param>
        /// <param name="locationsPropertyName">Locations Property Name</param>
        /// <param name="name">Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <param name="cacheKeyPrefix">Cache Key Prefix</param>
        /// <param name="useCache">Use Cache</param>
        /// <param name="searchedLocations">Searched Locations</param>
        /// <returns>String Item</returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            string areaName = GetAreaName(controllerContext.RouteData);

            // Little hack to get admin area to be in /User/ instead of /App/Member/ or Areas/Member/
            if (!string.IsNullOrEmpty(areaName) && areaName.Equals("user", StringComparison.InvariantCultureIgnoreCase))
            {
                var newLocations = areaLocations.ToList();

                newLocations.Insert(0, "~/Member/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/Member/Views/Shared/{0}.cshtml");

                areaLocations = newLocations.ToArray();
            }

            // Little hack to get admin area to be in /Management/ instead of /App/Admin/ or Areas/Admin/
            if (!string.IsNullOrEmpty(areaName) && areaName.Equals("management", StringComparison.InvariantCultureIgnoreCase))
            {
                var newLocations = areaLocations.ToList();

                newLocations.Insert(0, "~/ContentManagement/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/ContentManagement/Views/Shared/{0}.cshtml");

                areaLocations = newLocations.ToArray();
            }

            bool flag = !string.IsNullOrEmpty(areaName);

            List<ViewLocation> viewLocations = GetViewLocations(locations, flag ? areaLocations : null);

            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { locationsPropertyName }));
            }

            bool flag2 = IsSpecificPath(name);

            string key = this.CreateCacheKey(cacheKeyPrefix, name, flag2 ? string.Empty : controllerName, areaName);

            if (useCache)
            {
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);

                if (cached != null)
                {
                    return cached;
                }
            }

            if (!flag2)
            {
                return this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, key, ref searchedLocations);
            }

            return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        /// <summary>
        /// File Path Is Supported
        /// </summary>
        /// <param name="virtualPath">Virtual Path</param>
        /// <returns>Boolean Item</returns>
        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            if (this.FileExtensions == null)
            {
                return true;
            }

            string stringValue = this.GetExtensionThunk(virtualPath).TrimStart(new char[] { '.' });

            return this.FileExtensions.Contains<string>(stringValue, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get Path From Specific Name
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="name">Name</param>
        /// <param name="cacheKey">Cache Key</param>
        /// <param name="searchedLocations">Searched Locations</param>
        /// <returns>String Item</returns>
        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;

            if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;

                searchedLocations = new string[] { name };
            }

            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);

            return virtualPath;
        }

        /// <summary>
        /// Get Path From General Name
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="locations">Locations</param>
        /// <param name="name">Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <param name="areaName">Area Name</param>
        /// <param name="cacheKey">Cache Key</param>
        /// <param name="searchedLocations">Searched Locations</param>
        /// <returns>String Item</returns>
        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;

            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                string stringValue = locations[i].Format(name, controllerName, areaName);

                if (this.FileExists(controllerContext, stringValue))
                {
                    searchedLocations = _emptyLocations;

                    virtualPath = stringValue;

                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);

                    return virtualPath;
                }

                searchedLocations[i] = stringValue;
            }

            return virtualPath;
        }

        /// <summary>
        /// Create Cache Key
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="name">Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <param name="areaName">Area Name</param>
        /// <returns>String Item</returns>
        protected virtual string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}", new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName });
        }

        /// <summary>
        /// Get View Locations
        /// </summary>
        /// <param name="viewLocationFormats">View Location Formats</param>
        /// <param name="areaViewLocationFormats">Area View Location Formats</param>
        /// <returns>View Location Collection Item</returns>
        protected virtual List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            var list = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                list.AddRange(areaViewLocationFormats.Select(x => new AreaAwareViewLocation(x)).Cast<ViewLocation>());
            }

            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(x => new ViewLocation(x)));
            }

            return list;
        }

        /// <summary>
        /// Is Specific Path
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Boolean Item</returns>
        protected virtual bool IsSpecificPath(string name)
        {
            char charValue = name[0];

            if (charValue != '~')
            {
                return (charValue == '/');
            }

            return true;
        }

        /// <summary>
        /// Get Area Name
        /// </summary>
        /// <param name="routeData">Route Data</param>
        /// <returns>String Item</returns>
        protected virtual string GetAreaName(RouteData routeData)
        {
            object objectValue;

            if (routeData.DataTokens.TryGetValue("area", out objectValue))
            {
                return (objectValue as string);
            }

            return GetAreaName(routeData.Route);
        }

        /// <summary>
        /// Get Area Name
        /// </summary>
        /// <param name="route">Route</param>
        /// <returns>String Item</returns>
        protected virtual string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;

            if (area != null)
            {
                return area.Area;
            }

            var route2 = route as Route;

            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }

            return null;
        }

        /// <summary>
        /// Find Web Application View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="viewName">View Name</param>
        /// <param name="masterName">Master Name</param>
        /// <param name="useCache">Use Cache</param>
        /// <returns>View Engine Result Item</returns>
        /// <exception cref="ArgumentNullException">controllerContext</exception>
        /// <exception cref="ArgumentException">View name cannot be null or empty. - viewName</exception>
        protected virtual ViewEngineResult FindWebAppView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string[] stringArray1;
            string[] stringArray2;

            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", "viewName");
            }

            string requiredString = controllerContext.RouteData.GetRequiredString("controller");

            string stringValue1 = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out stringArray1);

            string stringValue2 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out stringArray2);

            if (!string.IsNullOrEmpty(stringValue1) && (!string.IsNullOrEmpty(stringValue2) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(this.CreateView(controllerContext, stringValue1, stringValue2), this);
            }

            if (stringArray2 == null)
            {
                stringArray2 = new string[0];
            }

            return new ViewEngineResult(stringArray1.Union<string>(stringArray2));
        }

        /// <summary>
        /// Find Web Application Partial View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="partialViewName">Partial View Name</param>
        /// <param name="useCache">Use Cache</param>
        /// <returns>View Engine Result Item</returns>
        /// <exception cref="ArgumentNullException">controllerContext</exception>
        /// <exception cref="ArgumentException">Partial view name cannot be null or empty. - partialViewName</exception>
        protected virtual ViewEngineResult FindWebAppPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] strArray;

            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            }

            string requiredString = controllerContext.RouteData.GetRequiredString("controller");

            string stringValue = this.GetPath(controllerContext, this.PartialViewLocationFormats, this.AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out strArray);

            if (string.IsNullOrEmpty(stringValue))
            {
                return new ViewEngineResult(strArray);
            }

            return new ViewEngineResult(this.CreatePartialView(controllerContext, stringValue), this);
        }

        //protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        //{
        //    return BuildManager.GetObjectFactory(virtualPath, false) != null;
        //}

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Web Application Virtual Path Provider View Engine
        /// </summary>
        protected WebAppVirtualPathProviderViewEngine()
        {
            GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Find View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="viewName">View Name</param>
        /// <param name="masterName">Master Name</param>
        /// <param name="useCache">Use Cache</param>
        /// <returns>View Engine Result Item</returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = FindWebAppView(controllerContext, viewName, masterName, useCache);

            return result;
        }

        /// <summary>
        /// Find Partial View
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="partialViewName">Partial View Name</param>
        /// <param name="useCache">Use Cache</param>
        /// <returns>View Engine Result Item</returns>
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewEngineResult result = FindWebAppPartialView(controllerContext, partialViewName, useCache);

            return result;
        }

        #endregion Methods
    }

    /// <summary>
    /// Represents a Area Aware View Location
    /// Implements the <see cref="Adrack.Web.Framework.ViewEngines.Razor.ViewLocation" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.ViewEngines.Razor.ViewLocation" />
    public class AreaAwareViewLocation : ViewLocation
    {
        #region Constructor

        /// <summary>
        /// Area Aware View Location
        /// </summary>
        /// <param name="virtualPathFormatString">Virtual Path FormatString</param>
        public AreaAwareViewLocation(string virtualPathFormatString)
            : base(virtualPathFormatString)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="viewName">View Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <param name="areaName">Area Name</param>
        /// <returns>String Item</returns>
        public override string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
        }

        #endregion Methods
    }

    /// <summary>
    /// Represents a View Location
    /// </summary>
    public class ViewLocation
    {
        #region Fields

        /// <summary>
        /// Virtual Path Format String
        /// </summary>
        protected readonly string _virtualPathFormatString;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// View Location
        /// </summary>
        /// <param name="virtualPathFormatString">Virtual Path Format String</param>
        public ViewLocation(string virtualPathFormatString)
        {
            _virtualPathFormatString = virtualPathFormatString;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="viewName">View Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <param name="areaName">Area Name</param>
        /// <returns>String Item</returns>
        public virtual string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName);
        }

        #endregion Methods
    }
}