// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ForgotPasswordValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Service.Localization;
using Adrack.Web.Framework.Validators;
using Adrack.Web.Models.Membership;

namespace Adrack.Web.Validators.Membership
{
    /// <summary>
    ///     Represents a Forgot Password Validator
    ///     Implements the <see cref="Framework.Validators.BaseAppValidator{ForgotPasswordModel}" />
    /// </summary>
    /// <seealso cref="Framework.Validators.BaseAppValidator{ForgotPasswordModel}" />
    public class ForgotPasswordValidator : BaseAppValidator<ForgotPasswordModel>
    {
        #region Constructor

        /// <summary>
        ///     Forgot Password Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        public ForgotPasswordValidator(ILocalizedStringService localizedStringService)
        {
        }

        #endregion Constructor
    }
}