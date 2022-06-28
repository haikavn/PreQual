// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 04-08-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="JsonData.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Adrack.Data
{
    /// <summary>
    /// Class JsonData.
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// The draw
        /// </summary>
        public int draw;

        /// <summary>
        /// The records total
        /// </summary>
        public int recordsTotal;

        /// <summary>
        /// The records filtered
        /// </summary>
        public int recordsFiltered;

        /// <summary>
        /// The data
        /// </summary>
        public List<string[]> data = new List<string[]>();

        /// <summary>
        /// The records sum
        /// </summary>
        public double recordsSum;

        /// <summary>
        /// The totals sum string
        /// </summary>
        public string[] totalsSumStr = new string[10] { "", "", "", "", "", "", "", "", "", "" };

        /// <summary>
        /// The basic data
        /// </summary>
        public List<string[]> basicData = new List<string[]>();

        /// <summary>
        /// The time zone now string
        /// </summary>
        public string TimeZoneNowStr = "";
    }
}