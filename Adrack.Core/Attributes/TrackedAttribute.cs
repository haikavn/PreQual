// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Campaign.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class TrackedAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; set; }

        public string TableFieldName { get; set; }


        public string EntityName { get; set; }

        public string EntityIdField { get; set; }

        public Type DisplayType { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="DisplayName"></param>
        ///// <param name="TableName"></param>
        //public MyTrackedAttribute(string DisplayName, string TableName)
        //{
        //    this.DisplayName = DisplayName;
        //    this.TableName = TableName;
        //}
    }
}