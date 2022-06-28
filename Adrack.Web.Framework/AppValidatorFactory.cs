// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AppValidatorFactory.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Application Validator Factory
    /// Implements the <see cref="FluentValidation.Attributes.AttributedValidatorFactory" />
    /// </summary>
    /// <seealso cref="FluentValidation.Attributes.AttributedValidatorFactory" />
    public class AppValidatorFactory : AttributedValidatorFactory
    {
        #region Fields

        //private readonly InstanceCache _cache = new InstanceCache();

        #endregion Fields



        #region Methods

        /// <summary>
        /// Get Validator
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Validator Interface</returns>
        public override IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));

                if ((attribute != null) && (attribute.ValidatorType != null))
                {
                    //validators can depend on some customer specific settings (such as working language)
                    //that's why we do not cache validators
                    //var instance = _cache.GetOrCreateInstance(attribute.ValidatorType,
                    //                           x => EngineContext.Current.ContainerManager.ResolveUnregistered(x));
                    var instance = AppEngineContext.Current.ContainerManager.ResolveUnregistered(attribute.ValidatorType);

                    return instance as IValidator;
                }
            }

            return null;
        }

        #endregion Methods
    }
}