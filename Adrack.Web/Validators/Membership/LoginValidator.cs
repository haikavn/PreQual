// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LoginValidator.cs" company="Adrack.com">
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
    ///     Represents a Login Validator
    ///     Implements the <see cref="Framework.Validators.BaseAppValidator{LoginModel}" />
    /// </summary>
    /// <seealso cref="Framework.Validators.BaseAppValidator{LoginModel}" />
    public class LoginValidator : BaseAppValidator<LoginModel>
    {
        #region Constructor

        /// <summary>
        ///     Login Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="userSetting">User Setting</param>
        public LoginValidator(ILocalizedStringService localizedStringService, UserSetting userSetting)
        {
            if (!userSetting.UsernameEnabled)
            {
                RuleFor(x => x.Email).NotEmpty()
                    .WithMessage(localizedStringService.GetLocalizedString("Membership.Field.Email.Required"));
                RuleFor(x => x.Email).EmailAddress()
                    .WithMessage(localizedStringService.GetLocalizedString("Common.WrongEmail"));
            }
        }

        #endregion Constructor
    }
}