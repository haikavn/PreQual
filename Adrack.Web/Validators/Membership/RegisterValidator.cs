// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="RegisterValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Directory;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Validators;
using Adrack.Web.Models.Membership;
using FluentValidation;
using FluentValidation.Results;

namespace Adrack.Web.Validators.Membership
{
    /// <summary>
    ///     Represents a Register Validator
    ///     Implements the <see cref="Framework.Validators.BaseAppValidator{RegisterModel}" />
    /// </summary>
    /// <seealso cref="Framework.Validators.BaseAppValidator{RegisterModel}" />
    public class RegisterValidator : BaseAppValidator<RegisterModel>
    {
        #region Constructor

        /// <summary>
        ///     Register Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="userSetting">User Setting</param>
        /// <param name="profileSetting">Profile Setting</param>
        /// <param name="addressSetting">Address Setting</param>
        public RegisterValidator(ILocalizedStringService localizedStringService,
            IStateProvinceService stateProvinceService, UserSetting userSetting, ProfileSetting profileSetting,
            AddressSetting addressSetting)
        {
            // Email
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.Email.Required"));
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage(localizedStringService.GetLocalizedString("Common.WrongEmail"));

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

            // Country
            if (addressSetting.CountryEnabled && addressSetting.CountryRequired)
                RuleFor(x => x.CountryId).NotEqual(0)
                    .WithMessage(localizedStringService.GetLocalizedString("Address.Field.Country.Required"));

            // State Province
            if (addressSetting.CountryEnabled && addressSetting.StateProvinceEnabled &&
                addressSetting.StateProvinceRequired)
                Custom(x =>
                {
                    var stateProvinceSelected = stateProvinceService.GetStateProvinceByCountryId(x.CountryId).Count > 0;

                    if (stateProvinceSelected)
                        if (x.StateProvinceId == 0)
                            return new ValidationFailure("StateProvinceId",
                                localizedStringService.GetLocalizedString("Address.Field.StateProvince.Required"));

                    return null;
                });
        }

        #endregion Constructor
    }
}