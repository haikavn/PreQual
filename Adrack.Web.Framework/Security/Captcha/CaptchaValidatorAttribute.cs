// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="CaptchaValidatorAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Security.Captcha
{
    /// <summary>
    /// Represents a Captcha Validator Attribute
    /// Implements the <see cref="System.Web.Mvc.ActionFilterAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        #region Constants

        /// <summary>
        /// Captcha Challenge Field Key
        /// </summary>
        private const string CAPTCHA_CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";

        /// <summary>
        /// Captcha Response Field Key
        /// </summary>
        private const string CAPTCHA_RESPONSE_FIELD_KEY = "recaptcha_response_field";

        #endregion Constants



        #region Methods

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="actionExecutingContext">Action Executing Context</param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            bool isValid = false;

            var reCaptchaChallengeFieldKey = actionExecutingContext.HttpContext.Request.Form[CAPTCHA_CHALLENGE_FIELD_KEY];
            var reCaptchaResponseFieldKey = actionExecutingContext.HttpContext.Request.Form[CAPTCHA_RESPONSE_FIELD_KEY];

            if (!string.IsNullOrEmpty(reCaptchaChallengeFieldKey) && !string.IsNullOrEmpty(reCaptchaResponseFieldKey))
            {
                var captchaSetting = AppEngineContext.Current.Resolve<CaptchaSetting>();

                if (captchaSetting.Enabled)
                {
                    var recaptchaValidator = new Recaptcha.RecaptchaValidator
                    {
                        Challenge = reCaptchaChallengeFieldKey,
                        PrivateKey = captchaSetting.ReCaptchaPrivateKey,
                        RemoteIP = actionExecutingContext.HttpContext.Request.UserHostAddress,
                        Response = reCaptchaResponseFieldKey
                    };

                    var reCaptchaResponse = recaptchaValidator.Validate();

                    isValid = reCaptchaResponse.IsValid;
                }
            }

            actionExecutingContext.ActionParameters["captchaValid"] = isValid;

            base.OnActionExecuting(actionExecutingContext);
        }

        #endregion Methods
    }
}