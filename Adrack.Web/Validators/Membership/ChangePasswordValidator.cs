// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="ChangePasswordValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Localization;
using Adrack.Service.Security;
using Adrack.Web.Framework.Validators;
using Adrack.Web.Models.Membership;
using FluentValidation;
using FluentValidation.Results;

namespace Adrack.Web.Validators.Membership
{
    /// <summary>
    ///     Represents a Register Validator
    ///     Implements the <see cref="Framework.Validators.BaseAppValidator{ChangePasswordModel}" />
    /// </summary>
    /// <seealso cref="Framework.Validators.BaseAppValidator{ChangePasswordModel}" />
    public class ChangePasswordValidator : BaseAppValidator<ChangePasswordModel>
    {
        #region Constructor

        /// <summary>
        ///     Register Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="userSetting">User Setting</param>
        /// <param name="profileSetting">Profile Setting</param>
        /// <param name="addressSetting">Address Setting</param>
        /// <param name="_appContext">The application context.</param>
        /// <param name="_encryptionService">The encryption service.</param>
        public ChangePasswordValidator(ILocalizedStringService localizedStringService, UserSetting userSetting,
            ProfileSetting profileSetting, AddressSetting addressSetting, IAppContext _appContext,
            IEncryptionService _encryptionService)
        {
            Custom(x =>
            {
                if (_appContext.AppUser == null)
                    return new ValidationFailure("OldPassword", "Current password is not valid");

                if (_appContext.AppUser.Password !=
                    _encryptionService.CreatePasswordHash(x.OldPassword, _appContext.AppUser.SaltKey))
                    return new ValidationFailure("OldPassword", "Current password is not valid");

                return null;
            });

            // Password
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.Password.Required"));
            RuleFor(x => x.Password).Length(userSetting.MinimumRequiredPasswordLength, 999).WithMessage(
                string.Format(localizedStringService.GetLocalizedString("Membership.Field.Password.LengthValidation"),
                    userSetting.MinimumRequiredPasswordLength));

            // Confirm Password
            RuleFor(x => x.ConfirmPassword).NotEmpty()
                .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.ConfirmPassword.Required"));
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.Password.DoNotMatch"));
        }

        #endregion Constructor
    }
}