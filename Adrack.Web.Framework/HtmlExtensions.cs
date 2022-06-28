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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Html Extensions
    /// </summary>
    public static class HtmlExtensions
    {
        #region Methods

        #region Content Management

        /// <summary>
        /// Is Active
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="value">Value</param>
        /// <param name="actionName">Action Name</param>
        /// <param name="controllerName">Controller Name</param>
        /// <returns>String Value</returns>
        public static MvcHtmlString IsActive(this HtmlHelper htmlHelper, string value, string actionName, string controllerName)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            if (String.IsNullOrEmpty(actionName))
                return null;

            if (String.IsNullOrEmpty(controllerName))
                return null;

            var returnActive = actionName == ParentActionName(htmlHelper) && controllerName == ParentControllerName(htmlHelper);

            return returnActive ? new MvcHtmlString(value) : new MvcHtmlString("");
        }

        #endregion Content Management

        /// <summary>
        /// Required Hint
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="additionalText">Additional Text</param>
        /// <returns>String</returns>
        public static MvcHtmlString RequiredHint(this HtmlHelper htmlHelper, string additionalText = null)
        {
            var tagBuilder = new TagBuilder("span");

            tagBuilder.AddCssClass("required");

            var innerText = "*";

            if (!String.IsNullOrEmpty(additionalText))
                innerText += " " + additionalText;

            tagBuilder.SetInnerText(innerText);

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Field Name For
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TResult">Type Result</typeparam>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="expression">Expression</param>
        /// <returns>String</returns>
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        /// <summary>
        /// Field Id For
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TResult">Type Result</typeparam>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="expression">Expression</param>
        /// <returns>String</returns>
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            var id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            return id.Replace('[', '_').Replace(']', '_');
        }

        /// <summary>
        /// Label For
        /// </summary>
        /// <typeparam name="TModel">Type Model</typeparam>
        /// <typeparam name="TValue">Type Value</typeparam>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="suffix">Suffix</param>
        /// <returns>String</returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string suffix)
        {
            string getExpressionText = ExpressionHelper.GetExpressionText(expression);

            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string resolvedLabelText = modelMetadata.DisplayName ?? (modelMetadata.PropertyName ?? getExpressionText.Split(new[] { '.' }).Last());

            if (string.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            var tagBuilder = new TagBuilder("label");

            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(getExpressionText)));

            if (!String.IsNullOrEmpty(suffix))
            {
                resolvedLabelText = String.Concat(resolvedLabelText, suffix);
            }

            tagBuilder.SetInnerText(resolvedLabelText);

            var dictionary = ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tagBuilder.MergeAttributes(dictionary, true);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Widget
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <param name="widgetZone">Wedget Zone</param>
        /// <param name="additionalData">Additional Data</param>
        /// <returns>String</returns>
        public static MvcHtmlString Widget(this HtmlHelper htmlHelper, string widgetZone, object additionalData = null)
        {
            return htmlHelper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone, additionalData = additionalData });
        }

        /// <summary>
        /// Parent Action Name
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>String</returns>
        public static string ParentActionName(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.ParentActionViewContext.RouteData.GetRequiredString("action");
        }

        /// <summary>
        /// Action Name
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>String</returns>
        public static string ActionName(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.RouteData.GetRequiredString("action");
        }

        /// <summary>
        /// Parent Controller Name
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>String</returns>
        public static string ParentControllerName(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.ParentActionViewContext.RouteData.GetRequiredString("controller");
        }

        /// <summary>
        /// Controller Name
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>String</returns>
        public static string ControllerName(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
        }

        #endregion Methods
    }
}