// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SettingExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Adrack.Service.Configuration
{
    /// <summary>
    /// Represents a Setting Extensions
    /// </summary>
    public static class SettingExtensions
    {
        #region Methods

        /// <summary>
        /// Get Setting Key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string GetSettingKey<T, TPropType>(this T entity, Expression<Func<T, TPropType>> key) where T : ISetting, new()
        {
            var member = key.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", key));
            }

            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", key));
            }

            var keyResult = typeof(T).Name + "." + propertyInfo.Name;

            return keyResult;
        }

        #endregion Methods
    }
}