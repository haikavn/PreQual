// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="CaptchaSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Web.Framework.Security.Captcha
{
    /// <summary>
    /// Represents a Captcha Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class CaptchaSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Enabled
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or Sets the ReCaptcha Public Key
        /// </summary>
        /// <value>The re captcha public key.</value>
        public string ReCaptchaPublicKey { get; set; }

        /// <summary>
        /// Gets or Sets the ReCaptcha Private Key
        /// </summary>
        /// <value>The re captcha private key.</value>
        public string ReCaptchaPrivateKey { get; set; }

        /// <summary>
        /// Gets or Sets the ReCaptcha Theme
        /// </summary>
        /// <value>The re captcha theme.</value>
        public string ReCaptchaTheme { get; set; }

        /// <summary>
        /// Gets or Sets the On Login Page
        /// </summary>
        /// <value><c>true</c> if [on login page]; otherwise, <c>false</c>.</value>
        public bool OnLoginPage { get; set; }

        /// <summary>
        /// Gets or Sets the On Register Page
        /// </summary>
        /// <value><c>true</c> if [on register page]; otherwise, <c>false</c>.</value>
        public bool OnRegisterPage { get; set; }

        #endregion Properties
    }
}