// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BaseAppValidator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Data;
using FluentValidation;
using System.Linq;

namespace Adrack.Web.Framework.Validators
{
    /// <summary>
    /// Represents a Base Application Validator
    /// Implements the <see cref="FluentValidation.AbstractValidator{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FluentValidation.AbstractValidator{T}" />
    public abstract class BaseAppValidator<T> : AbstractValidator<T> where T : class
    {
        #region Constructor

        /// <summary>
        /// Base Application Validator
        /// </summary>
        protected BaseAppValidator()
        {
            PostInitialize();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Post Initialize
        /// </summary>
        protected virtual void PostInitialize()
        {
        }

        /// <summary>
        /// Set String Properties Max Length
        /// </summary>
        /// <typeparam name="TObject">Type Of OBject</typeparam>
        /// <param name="dbContext">Db Context</param>
        /// <param name="filterPropertyNames">Filter Property Names</param>
        protected virtual void SetStringPropertiesMaxLength<TObject>(IDbClientContext dbContext, params string[] filterPropertyNames)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !filterPropertyNames.Contains(p.Name))
                .Select(p => p.Name).ToArray();

            var maxLength = dbContext.GetColumnsMaxLength(dbObjectType.Name, names);
            var expression = maxLength.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, string>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Length(0, maxLength[expr.Key]);
            }
        }

        #endregion Methods
    }
}