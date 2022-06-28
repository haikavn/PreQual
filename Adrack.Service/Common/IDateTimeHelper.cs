// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IDateTimeHelper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Date Time Helper
    /// </summary>
    public partial interface IDateTimeHelper
    {
        #region Methods

        /// <summary>
        /// Find Time Zone By Id
        /// </summary>
        /// <param name="id">Find Time Zone Identifier</param>
        /// <returns>Time Zone Info Item</returns>
        TimeZoneInfo FindTimeZoneById(string id);

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        List<SelectListItem> GetTimeZones(string selectedValue = "");

        /// <summary>
        /// Gets the system time zones.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        List<SelectListItem> GetSystemTimeZones(string selectedValue = "");

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUserTime(DateTime dateTime);

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceDateTimeKind">Source Date Time Kind</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUserTime(DateTime dateTime, DateTimeKind sourceDateTimeKind);

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <param name="destinationTimeZone">Destination Time Zone</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone);

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUtcTime(DateTime dateTime);

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceDateTimeKind">Source Date Time Kind</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUtcTime(DateTime dateTime, DateTimeKind sourceDateTimeKind);

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <returns>Date Time Item</returns>
        DateTime ConvertToUtcTime(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// Get User Time Zone
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Date Time Item</returns>
        TimeZoneInfo GetUserTimeZone(User user);

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Application Time Zone
        /// </summary>
        /// <value>The application time zone.</value>
        TimeZoneInfo AppTimeZone { get; set; }

        /// <summary>
        /// Gets or Sets the Current Time Zone
        /// </summary>
        /// <value>The current time zone.</value>
        TimeZoneInfo CurrentTimeZone { get; set; }

        #endregion Properties
    }
}