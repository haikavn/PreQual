// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Utils.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Text.RegularExpressions;
using System.Web;
using System;

namespace Adrack.Web.Helpers
{
    /// <summary>
    ///     Class Utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     Gets the ip address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetIPAddress()
        {
            var context = HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0) return addresses[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        ///     Gets the base URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        public static string GetBaseUrl(HttpRequestBase request)
        {
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        /// <summary>
        ///     Determines whether [has consecutive chars] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <returns><c>true</c> if [has consecutive chars] [the specified source]; otherwise, <c>false</c>.</returns>
        public static bool HasConsecutiveChars(string source, int sequenceLength)
        {
            // should check sequence length

            // just repeating letters
            return Regex.IsMatch(source, "([a-zA-Z])\\1{" + (sequenceLength - 1) + "}");
        }

        public static bool IsHoliday(DateTime date)
        {
            if (date.Day == 1 && date.Month == 1 ||
                date.Day == 21 && date.Month == 1 ||
                date.Day == 18 && date.Month == 2 ||
                date.Day == 28 && date.Month == 5 ||
                date.Day == 4 && date.Month == 7 ||
                date.Day == 1 && date.Month == 5 ||
                date.Day == 8 && date.Month == 10 ||
                date.Day == 11 && date.Month == 11 ||
                date.Day == 25 && date.Month == 12)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if this date is a federal holiday.
        /// </summary>
        /// <param name="date">This date</param>
        /// <returns>True if this date is a federal holiday</returns>
        public static bool IsFederalHoliday(DateTime date)
        {
            // to ease typing
            int nthWeekDay = (int)(Math.Ceiling((double)date.Day / 7.0d));
            DayOfWeek dayName = date.DayOfWeek;
            bool isThursday = dayName == DayOfWeek.Thursday;
            bool isFriday = dayName == DayOfWeek.Friday;
            bool isMonday = dayName == DayOfWeek.Monday;
            bool isWeekend = dayName == DayOfWeek.Saturday || dayName == DayOfWeek.Sunday;

            // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
            if ((date.Month == 12 && date.Day == 31 && isFriday) ||
                (date.Month == 1 && date.Day == 1 && !isWeekend) ||
                (date.Month == 1 && date.Day == 2 && isMonday)) return true;

            // MLK day (3rd monday in January)
            if (date.Month == 1 && isMonday && nthWeekDay == 3) return true;

            // President’s Day (3rd Monday in February)
            if (date.Month == 2 && isMonday && nthWeekDay == 3) return true;

            // Memorial Day (Last Monday in May)
            if (date.Month == 5 && isMonday && date.AddDays(7).Month == 6) return true;

            // Independence Day (July 4, or preceding Friday/following Monday if weekend)
            if ((date.Month == 7 && date.Day == 3 && isFriday) ||
                (date.Month == 7 && date.Day == 4 && !isWeekend) ||
                (date.Month == 7 && date.Day == 5 && isMonday)) return true;

            // Labor Day (1st Monday in September)
            if (date.Month == 9 && isMonday && nthWeekDay == 1) return true;

            // Columbus Day (2nd Monday in October)
            if (date.Month == 10 && isMonday && nthWeekDay == 2) return true;

            // Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
            if ((date.Month == 11 && date.Day == 10 && isFriday) ||
                (date.Month == 11 && date.Day == 11 && !isWeekend) ||
                (date.Month == 11 && date.Day == 12 && isMonday)) return true;

            // Thanksgiving Day (4th Thursday in November)
            if (date.Month == 11 && isThursday && nthWeekDay == 4) return true;

            // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
            if ((date.Month == 12 && date.Day == 24 && isFriday) ||
                (date.Month == 12 && date.Day == 25 && !isWeekend) ||
                (date.Month == 12 && date.Day == 26 && isMonday)) return true;

            return false;
        }
    }
}