// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ForgotPasswordConfirmationValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Validators;
using Adrack.Web.Models.Membership;
using FluentValidation;

namespace Adrack.Web.Validators.Membership
{
    /// <summary>
    ///     Represents a Forgot Password Confirmation Validator
    ///     Implements the <see cref="Framework.Validators.BaseAppValidator{ForgotPasswordConfirmationModel}" />
    /// </summary>
    /// <seealso cref="Framework.Validators.BaseAppValidator{ForgotPasswordConfirmationModel}" />
    public class ForgotPasswordConfirmationValidator : BaseAppValidator<ForgotPasswordConfirmationModel>
    {
        #region Constructor

        /// <summary>
        ///     Forgot Password Confirmation Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="userSetting">User Setting</param>
        public ForgotPasswordConfirmationValidator(ILocalizedStringService localizedStringService,
            UserSetting userSetting)
        {
            RuleFor(x => x.NewPassword).NotEmpty()
                .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ForgotPassword.NewPassword"));

            RuleFor(x => x.NewPassword).Length(userSetting.MinimumRequiredPasswordLength, 999).WithMessage(
                string.Format(
                    localizedStringService.GetLocalizedString(
                        "Membership.Field.ForgotPassword.NewPassword.LengthValidation"),
                    userSetting.MinimumRequiredPasswordLength));
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(
                localizedStringService.GetLocalizedString(
                    "Membership.Field.ForgotPassword.ConfirmNewPassword.Required"));
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(
                localizedStringService.GetLocalizedString(
                    "Membership.Field.ForgotPassword.NewPassword.EnteredPasswordsDoNotMatch"));
        }

        #endregion Constructor
    }
}