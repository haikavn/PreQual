// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="HtmlExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Adrack.Web.Framework.Security.Honeypot
{
    /// <summary>
    /// Represents a Html Extensions
    /// </summary>
    public static class HtmlExtensions
    {
        #region Methods

        /// <summary>
        /// Generate Honeypot Input
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString GenerateHoneypotInput(this HtmlHelper htmlHelper)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("<div style=\"display:none;\">");
            stringBuilder.Append(Environment.NewLine);

            var securitySetting = AppEngineContext.Current.Resolve<SecuritySetting>();
            var hpInput = htmlHelper.TextBox(securitySetting.HoneypotInputName);

            stringBuilder.Append(hpInput.ToString());

            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("</div>");

            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        #endregion Methods
    }
}