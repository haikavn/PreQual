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

using Adrack.Core.Infrastructure;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;

namespace Adrack.Web.Framework.Security.Captcha
{
    /// <summary>
    /// Represents a Html Extensions
    /// </summary>
    public static class HtmlExtensions
    {
        private static readonly bool CaptchaEnabled=false;

        #region Methods

        /// <summary>
        /// Generate Captcha
        /// </summary>
        /// <param name="htmlHelper">Html Helper</param>
        /// <returns>String Item</returns>
        public static string GenerateCaptcha(this HtmlHelper htmlHelper)
        {
            if (!CaptchaEnabled) return "";

            var captchaSetting = AppEngineContext.Current.Resolve<CaptchaSetting>();

            var reCaptchaTheme = !string.IsNullOrEmpty(captchaSetting.ReCaptchaTheme) ? captchaSetting.ReCaptchaTheme : "white";

            var captchaControl = new Recaptcha.RecaptchaControl
            {
                ID = "recaptcha",
                Theme = reCaptchaTheme,
                PublicKey = captchaSetting.ReCaptchaPublicKey,
                PrivateKey = captchaSetting.ReCaptchaPrivateKey
            };

            var htmlWriter = new HtmlTextWriter(new StringWriter());

            captchaControl.RenderControl(htmlWriter);

            return htmlWriter.InnerWriter.ToString();
        }

        #endregion Methods
    }
}