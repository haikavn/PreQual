// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SmtpAccountValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using Adrack.Data;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Models.Message;
using Adrack.Web.Framework.Validators;
using FluentValidation;

namespace Adrack.Web.ContentManagement.Validators.Message
{
    /// <summary>
    /// Represents a Smtp Account Validator
    /// Implements the <see cref="Adrack.Web.Framework.Validators.BaseAppValidator{Adrack.Web.ContentManagement.Models.Message.SmtpAccountModel}" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Validators.BaseAppValidator{Adrack.Web.ContentManagement.Models.Message.SmtpAccountModel}" />
    public class SmtpAccountValidator : BaseAppValidator<SmtpAccountModel>
    {
        #region Constructor

        /// <summary>
        /// Smtp Account Validator
        /// </summary>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="dbContext">Db Context</param>
        public SmtpAccountValidator(ILocalizedStringService localizedStringService, IDbClientContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizedStringService.GetLocalizedString("Common.WrongEmail"));

            RuleFor(x => x.DisplayName).NotEmpty();

            SetStringPropertiesMaxLength<SmtpAccount>(dbContext);
        }

        #endregion Constructor
    }
}