// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ProfileValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Models.Membership;
using Adrack.Web.Framework.Validators;
using FluentValidation;

namespace Adrack.Web.ContentManagement.Validators.Membership
{
    /// <summary>
    /// Represents a Profile Validator
    /// Implements the <see cref="Adrack.Web.Framework.Validators.BaseAppValidator{Adrack.Web.ContentManagement.Models.Membership.ProfileModel}" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Validators.BaseAppValidator{Adrack.Web.ContentManagement.Models.Membership.ProfileModel}" />
    public class ProfileValidator : BaseAppValidator<ProfileModel>
    {
        #region Constructor

        /// <summary>
        /// Profile Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="userSetting">User Setting</param>
        public ProfileValidator(ILocalizedStringService localizedStringService, UserSetting userSetting)
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ChangePassword.OldPassword.Required"));
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ChangePassword.NewPassword.Required"));

            RuleFor(x => x.NewPassword).NotEqual(x => x.OldPassword).WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ForgotPassword.OldPassword.NewPasswordMatchWithOldPassword"));

            //RuleFor(x => x.NewPassword).Matches(@"^[a-zA-Z-']*$").WithMessage("Matches Worked");

            RuleFor(x => x.NewPassword).Length(userSetting.MinimumRequiredPasswordLength, 999).WithMessage(string.Format(localizedStringService.GetLocalizedString("Membership.Field.ForgotPassword.NewPassword.LengthValidation"), userSetting.MinimumRequiredPasswordLength));
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ForgotPassword.ConfirmNewPassword.Required"));
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ForgotPassword.NewPassword.EnteredPasswordsDoNotMatch"));
        }

        #endregion Constructor
    }
}