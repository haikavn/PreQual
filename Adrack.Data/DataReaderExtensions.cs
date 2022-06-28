// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DataReaderExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Data Reader Extensions
    /// </summary>
    public static class DataReaderExtensions
    {
        #region Methods

        /// <summary>
        /// Data Reader To Object List
        /// </summary>
        /// <typeparam name="TType">Entity Type</typeparam>
        /// <param name="dataReader">Data Reader</param>
        /// <param name="fieldsToSkip">Fields To Skip</param>
        /// <param name="propertyInfoList">Property Info List</param>
        /// <returns>List Collection Item</returns>
        public static List<TType> DataReaderToObjectList<TType>(this IDataReader dataReader, string fieldsToSkip = null, Dictionary<string, PropertyInfo> propertyInfoList = null) where TType : new()
        {
            if (dataReader == null)
                return null;

            var items = new List<TType>();

            if (propertyInfoList == null)
            {
                propertyInfoList = new Dictionary<string, PropertyInfo>();

                var props = typeof(TType).GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var prop in props)
                    propertyInfoList.Add(prop.Name.ToLower(), prop);
            }

            while (dataReader.Read())
            {
                var inst = new TType();

                DataReaderToObject(dataReader, inst, fieldsToSkip, propertyInfoList);

                items.Add(inst);
            }

            return items;
        }

        /// <summary>
        /// Data Reader To Object
        /// </summary>
        /// <param name="dataReader">Data Reader</param>
        /// <param name="instance">Instance</param>
        /// <param name="fieldsToSkip">Fields To Skip</param>
        /// <param name="propertyInfoList">Property Info List</param>
        /// <exception cref="InvalidOperationException">Data reader cannot be used because it is already closed</exception>
        public static void DataReaderToObject(this IDataReader dataReader, object instance, string fieldsToSkip = null, Dictionary<string, PropertyInfo> propertyInfoList = null)
        {
            if (dataReader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it is already closed");

            if (string.IsNullOrEmpty(fieldsToSkip))
                fieldsToSkip = string.Empty;
            else
                fieldsToSkip = "," + fieldsToSkip + ",";

            fieldsToSkip = fieldsToSkip.ToLower();

            if (propertyInfoList == null)
            {
                propertyInfoList = new Dictionary<string, PropertyInfo>();

                var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var prop in props)
                    propertyInfoList.Add(prop.Name.ToLower(), prop);
            }

            for (int index = 0; index < dataReader.FieldCount; index++)
            {
                string name = dataReader.GetName(index).ToLower();

                if (propertyInfoList.ContainsKey(name))
                {
                    var prop = propertyInfoList[name];

                    if (fieldsToSkip.Contains("," + name + ","))
                        continue;

                    if ((prop != null) && prop.CanWrite)
                    {
                        var xValue = dataReader.GetValue(index);

                        prop.SetValue(instance, (xValue == DBNull.Value) ? null : xValue, null);
                    }
                }
            }
        }

        #endregion Methods
    }
}