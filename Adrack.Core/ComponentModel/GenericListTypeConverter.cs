// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="GenericListTypeConverter.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Adrack.Core.ComponentModel
{
    /// <summary>
    /// Represents a Generic List Type Converter
    /// Implements the <see cref="System.ComponentModel.TypeConverter" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.ComponentModel.TypeConverter" />
    public class GenericListTypeConverter<T> : TypeConverter
    {
        #region Fields

        /// <summary>
        /// Type Converter
        /// </summary>
        protected readonly TypeConverter typeConverter;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Get String Array
        /// </summary>
        /// <param name="stringValue">String Value</param>
        /// <returns>String Item Collection</returns>
        protected virtual string[] GetStringArray(string stringValue)
        {
            if (!String.IsNullOrEmpty(stringValue))
            {
                string[] result = stringValue.Split(',');

                Array.ForEach(result, x => x.Trim());

                return result;
            }
            else
            {
                return new string[0];
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Generic List Type Converter
        /// </summary>
        /// <exception cref="InvalidOperationException">No type converter exists for type " + typeof(T).FullName</exception>
        public GenericListTypeConverter()
        {
            typeConverter = TypeDescriptor.GetConverter(typeof(T));

            if (typeConverter == null)
            {
                throw new InvalidOperationException("No type converter exists for type " + typeof(T).FullName);
            }
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Can Convert From
        /// </summary>
        /// <param name="typeDescriptorContext">Type Descriptor Context</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Bool Item</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                string[] items = GetStringArray(sourceType.ToString());

                return items.Any();
            }

            return base.CanConvertFrom(typeDescriptorContext, sourceType);
        }

        /// <summary>
        /// Can Convert From
        /// </summary>
        /// <param name="typeDescriptorContext">Type Descriptor Context</param>
        /// <param name="cultureInfo">Culture Info</param>
        /// <param name="objectValue">Object Value</param>
        /// <returns>Object Item</returns>
        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object objectValue)
        {
            if (objectValue is string)
            {
                string[] items = GetStringArray((string)objectValue);

                var result = new List<T>();

                Array.ForEach(items, x =>
                {
                    object objectItem = typeConverter.ConvertFromInvariantString(x);

                    if (objectItem != null)
                    {
                        result.Add((T)objectItem);
                    }
                });

                return result;
            }

            return base.ConvertFrom(typeDescriptorContext, cultureInfo, objectValue);
        }

        /// <summary>
        /// Convert To
        /// </summary>
        /// <param name="typeDescriptorContext">Type Descriptor Context</param>
        /// <param name="cultureInfo">Culture Info</param>
        /// <param name="objectValue">Object Value</param>
        /// <param name="destinationType">Destination Type</param>
        /// <returns>Object Item</returns>
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object objectValue, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string result = string.Empty;

                if (objectValue != null)
                {
                    for (int x = 0; x < ((IList<T>)objectValue).Count; x++)
                    {
                        var string_0 = Convert.ToString(((IList<T>)objectValue)[x], CultureInfo.InvariantCulture);

                        result += string_0;

                        if (x != ((IList<T>)objectValue).Count - 1)
                            result += ",";
                    }
                }

                return result;
            }

            return base.ConvertTo(typeDescriptorContext, cultureInfo, objectValue, destinationType);
        }

        #endregion Methods
    }
}