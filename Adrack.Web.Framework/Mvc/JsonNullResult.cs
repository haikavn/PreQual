// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="JsonNullResult.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Mvc
{
    /// <summary>
    /// Represents a Json Null Result
    /// Implements the <see cref="System.Web.Mvc.JsonResult" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.JsonResult" />
    public class JsonNullResult : JsonResult
    {
        #region Methods

        /// <summary>
        /// Execute Result
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <exception cref="ArgumentNullException">controllerContext</exception>
        public override void ExecuteResult(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            var response = controllerContext.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            this.Data = null;

            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);

            response.Write(serializedObject);
        }

        #endregion Methods
    }
}