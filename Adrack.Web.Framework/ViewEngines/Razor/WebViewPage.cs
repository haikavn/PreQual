// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="WebViewPage.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Localization;
using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Adrack.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Represents a Web View Page
    /// Implements the <see cref="System.Web.Mvc.WebViewPage{TModel}" />
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <seealso cref="System.Web.Mvc.WebViewPage{TModel}" />
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        #region Fields

        /// <summary>
        /// Localized
        /// </summary>
        private Localized _localized;

        /// <summary>
        /// Localized String Service
        /// </summary>
        private ILocalizedStringService _localizedStringService;

        /// <summary>
        /// Application Context
        /// </summary>
        private IAppContext _appContext;

        #endregion Fields



        #region Methods

        /// <summary>
        /// Initialize Helpers
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();

            _localizedStringService = AppEngineContext.Current.Resolve<ILocalizedStringService>();

            _appContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

        /// <summary>
        /// Render Wrapped Section
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="wrapperHtmlAttributes">Wrapper Html Attributes</param>
        /// <returns>Helper Result Item</returns>
        public HelperResult RenderWrappedSection(string name, object wrapperHtmlAttributes)
        {
            Action<TextWriter> action = delegate (TextWriter textWriter)
            {
                var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(wrapperHtmlAttributes);

                var tagBuilder = new TagBuilder("div");

                tagBuilder.MergeAttributes(htmlAttributes);

                var renderSection = RenderSection(name, false);

                if (renderSection != null)
                {
                    textWriter.Write(tagBuilder.ToString(TagRenderMode.StartTag));

                    renderSection.WriteTo(textWriter);

                    textWriter.Write(tagBuilder.ToString(TagRenderMode.EndTag));
                }
            };

            return new HelperResult(action);
        }

        /// <summary>
        /// Render Section
        /// </summary>
        /// <param name="sectionName">Section Name</param>
        /// <param name="defaultContent">Default Content</param>
        /// <returns>Helper Result Item</returns>
        public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
        {
            return IsSectionDefined(sectionName) ? RenderSection(sectionName) : defaultContent(new object());
        }

        /// <summary>
        /// Language Culture
        /// </summary>
        /// <returns>System.String.</returns>
        public string LanguageCulture()
        {
            var languageCulture = _appContext.AppLanguage.Culture;

            if (!String.IsNullOrEmpty(languageCulture))
            {
                var culture = new CultureInfo(languageCulture);

                return culture.TwoLetterISOLanguageName;
            }

            return "en";
        }

        /// <summary>
        /// Language Rtl
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool LanguageRtl()
        {
            var languageRtl = _appContext.AppLanguage.Rtl;

            return languageRtl;
        }

        /// <summary>
        /// Get Selected TabIndex
        /// </summary>
        /// <returns>Integer Item</returns>
        public int GetSelectedTabIndex()
        {
            int index = 0;

            string dataKey = "Application.Selected.Tab.Index";

            if (ViewData[dataKey] is int)
            {
                index = (int)ViewData[dataKey];
            }

            if (TempData[dataKey] is int)
            {
                index = (int)TempData[dataKey];
            }

            if (index < 0)
                index = 0;

            return index;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Localized
        /// </summary>
        /// <value>The t.</value>
        public Localized T
        {
            get
            {
                if (_localized == null)
                {
                    _localized = (stringValue, args) =>
                    {
                        var localizedString = _localizedStringService.GetLocalizedString(stringValue);

                        if (string.IsNullOrEmpty(localizedString))
                        {
                            return new LocalizedStringValue(stringValue);
                        }

                        return new LocalizedStringValue((args == null || args.Length == 0) ? localizedString : string.Format(localizedString, args));
                    };
                }

                return _localized;
            }
        }

        /// <summary>
        /// Gets or Sets the Layout
        /// </summary>
        /// <value>The layout.</value>
        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);

                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Application Context
        /// </summary>
        /// <value>The application context.</value>
        public IAppContext AppContext
        {
            get
            {
                return _appContext;
            }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Web View Page
    /// Implements the <see cref="System.Web.Mvc.WebViewPage{TModel}" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.WebViewPage{TModel}" />
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}