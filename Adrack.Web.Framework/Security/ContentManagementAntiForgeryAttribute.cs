// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ContentManagementAntiForgeryAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Security
{
    /// <summary>
    /// Represents a Content Management Anti Forgery Attribute
    /// Implements the <see cref="System.Web.Mvc.FilterAttribute" />
    /// Implements the <see cref="System.Web.Mvc.IAuthorizationFilter" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ContentManagementAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region Fields

        /// <summary>
        /// Validate
        /// </summary>
        private readonly bool _validate;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Content Management Anti Forgery Attribute
        /// </summary>
        /// <param name="validate">Validate</param>
        public ContentManagementAntiForgeryAttribute(bool validate = false)
        {
            this._validate = validate;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="authorizationContext">Authorization Context</param>
        /// <exception cref="ArgumentNullException">authorizationContext</exception>
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            if (authorizationContext == null)
                throw new ArgumentNullException("authorizationContext");

            if (_validate)
                return;

            if (authorizationContext.IsChildAction)
                return;

            if (!String.Equals(authorizationContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                return;

            var securitySetting = AppEngineContext.Current.Resolve<SecuritySetting>();

            if (!securitySetting.ContentManagementXsrfProtection)
                return;

            var validateAntiForgeryTokenAttribute = new ValidateAntiForgeryTokenAttribute();

            validateAntiForgeryTokenAttribute.OnAuthorization(authorizationContext);
        }

        #endregion Methods
    }
}