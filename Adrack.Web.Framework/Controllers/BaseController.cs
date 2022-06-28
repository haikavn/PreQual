// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BaseController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Audit;
using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Controllers
{
    /// <summary>
    /// Represents a Base Controller
    /// Implements the <see cref="System.Web.Mvc.Controller" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [UserIpAddress]
    [UserActivityDate]
    [UserLastVisitedPage]
    public abstract class BaseController : Controller
    {
        #region Utilities

        /// <summary>
        /// Log Exception
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void LogException(Exception exception)
        {
            var appContext = AppEngineContext.Current.Resolve<IAppContext>();

            var logService = AppEngineContext.Current.Resolve<ILogService>();

            var appUser = appContext.AppUser;

            logService.Error(exception.Message, exception, appUser);
        }

        /// <summary>
        /// Success Notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">Persist For The Next Request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(LogLevel.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Error Notification
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="persistForTheNextRequest">if set to <c>true</c> [persist for the next request].</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(LogLevel.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Error Notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">Persist For The Next Request</param>
        /// <param name="logException">Log Exception</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);

            AddNotification(LogLevel.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// Add Notification
        /// </summary>
        /// <param name="logLevel">Log Level</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">Persist For The Next Request</param>
        protected virtual void AddNotification(LogLevel logLevel, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("Application.Base.Controller.Add.Notifications.{0}", logLevel);

            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();

                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();

                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        #endregion Utilities



        #region Methods

        /// <summary>
        /// Render Partial View To String
        /// </summary>
        /// <returns>String Item</returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        /// <summary>
        /// Render Partial View To String
        /// </summary>
        /// <param name="viewName">View Name</param>
        /// <returns>String Item</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// Render Partial View To String
        /// </summary>
        /// <param name="modelName">Model Name</param>
        /// <returns>String Item</returns>
        public virtual string RenderPartialViewToString(object modelName)
        {
            return RenderPartialViewToString(null, modelName);
        }

        /// <summary>
        /// Render Partial View To String
        /// </summary>
        /// <param name="viewName">View Name</param>
        /// <param name="objectModel">Object Model</param>
        /// <returns>String Item</returns>
        public virtual string RenderPartialViewToString(string viewName, object objectModel)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = objectModel;

            using (var stringWriter = new StringWriter())
            {
                ViewEngineResult viewEngineResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);

                var viewContext = new ViewContext(this.ControllerContext, viewEngineResult.View, this.ViewData, this.TempData, stringWriter);

                viewEngineResult.View.Render(viewContext, stringWriter);

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        #endregion Methods
    }
}