// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Validator.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Adrack.Web.Helpers
{
    /// <summary>
    ///     Class Validator.
    /// </summary>
    public class Validator
    {
        /// <summary>
        ///     Matches the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="regexstr">The regexstr.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MatchString(string str, string regexstr)
        {
            str = str.Trim();
            var pattern = new Regex(regexstr);
            return pattern.IsMatch(str);
        }

        /// <summary>
        ///     Determines whether [is valid number] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns><c>true</c> if [is valid number] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsValidNumber(string val)
        {
            int d;
            return int.TryParse(val, out d);
        }

        /// <summary>
        ///     Determines whether [is valid decimal] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="before">The before.</param>
        /// <param name="after">The after.</param>
        /// <returns><c>true</c> if [is valid decimal] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsValidDecimal(string val, int before, int after)
        {
            decimal d;
            if (!decimal.TryParse(val, out d)) return false;

            if (d == 0) return true;

            if (before == after && before == 0) return true;

            var precision = 0;

            while (d * (decimal)Math.Pow(10, precision) !=
                   Math.Round(d * (decimal)Math.Pow(10, precision)))
            {
                precision++;
                if (precision > after) return false;
            }

            var dd = decimal.Floor(d < 0 ? decimal.Negate(d) : d);
            var cnt = 1;
            while ((dd = decimal.Floor(dd / 10m)) != 0m)
            {
                cnt++;
                if (cnt > before) return false;
            }

            return true;
        }

        /// <summary>
        ///     Determines whether [is valid date time] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="format">The format.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if [is valid date time] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsValidDateTime(string val, string format, out DateTime result)
        {
            try
            {
                if (string.IsNullOrEmpty(format)) format = "MM/dd/yyyy";
                result = DateTime.ParseExact(val, format, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                var logService = AppEngineContext.Current.Resolve<ILogService>();
                if (logService != null)
                    logService.Error(ex.Message, ex);
            }

            result = DateTime.Now;
            return false;
        }

        /// <summary>
        ///     Determines whether [is valid user name] [the specified string username].
        /// </summary>
        /// <param name="strUsername">The string username.</param>
        /// <returns><c>true</c> if [is valid user name] [the specified string username]; otherwise, <c>false</c>.</returns>
        public static bool IsValidUserName(string strUsername)
        {
            // Allows word characters [A-Za-z0-9_], single quote, dash and period
            // must be at least two characters long and less then 128
            var regExPattern = @"^[\w-‘\.]{2,128}$";
            // We also permit email address characters in user name. Set to false
            // if you don’t permit email addresses as usernames.
            var allowEmailUsernames = true;
            if (allowEmailUsernames)
                return MatchString(strUsername, regExPattern) || IsValidEmailAddress(strUsername);
            return MatchString(strUsername, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid password] [the specified string password].
        /// </summary>
        /// <param name="strPassword">The string password.</param>
        /// <returns><c>true</c> if [is valid password] [the specified string password]; otherwise, <c>false</c>.</returns>
        public static bool IsValidPassword(string strPassword)
        {
            // Allows any type of character
            // If complexity is enabled, the password must be longer
            // and contain at least one uppercase, one lowercase,
            // one numeric and one symbolic character. Set to false
            // if your requirements differ.
            var passwordComplexity = true;
            // These are some proposed minimum password lengths. If
            // complexity is enabled (above), the stronger (longer)
            // minimum password rule applies.
            var minPasswordLen = 6;
            var strongPasswordLen = 8;
            if (passwordComplexity)
            {
                var regExPattern =
                    @"^.*(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[`~!@#\$%\^\&\*\(\)-_\=\+\[\{\]\}\\\|;:’,/?]).*$";
                return strPassword.Length >= strongPasswordLen &&
                       MatchString(strPassword, regExPattern);
            }

            return strPassword.Length >= minPasswordLen;
        }

        /// <summary>
        ///     Determines whether [is valid name] [the specified string name].
        /// </summary>
        /// <param name="strName">Name of the string.</param>
        /// <returns><c>true</c> if [is valid name] [the specified string name]; otherwise, <c>false</c>.</returns>
        public static bool IsValidName(string strName)
        {
            // Allows alphabetical chars, single quote, dash and space
            // must be at least two characters long and caps out at 128 (database size)
            var regExPattern = @"^[a-zA-Z-‘\.\s]{2,128}$";
            return MatchString(strName, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid street address] [the specified string address].
        /// </summary>
        /// <param name="strAddress">The string address.</param>
        /// <returns><c>true</c> if [is valid street address] [the specified string address]; otherwise, <c>false</c>.</returns>
        public static bool IsValidStreetAddress(string strAddress)
        {
            // Since so many different types of address formats we’re just going to swing the bat at
            // this one for now and do a match against a series of digits (potentially containing
            // punctuation), followed by a series of characters representing the street name and then
            // potentially a type of street and unit number
            var regExPattern = @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}(\s[a-zA-Z]{2,15})?([#\.0-9a-zA-Z]*)?";
            return MatchString(strAddress, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid city] [the specified string city].
        /// </summary>
        /// <param name="strCity">The string city.</param>
        /// <returns><c>true</c> if [is valid city] [the specified string city]; otherwise, <c>false</c>.</returns>
        public static bool IsValidCity(string strCity)
        {
            // Here we simply treat city names like people names and defer to our name validation function.
            return IsValidName(strCity);
        }

        /// <summary>
        ///     Determines whether [is valid us state] [the specified string state].
        /// </summary>
        /// <param name="strState">State of the string.</param>
        /// <returns><c>true</c> if [is valid us state] [the specified string state]; otherwise, <c>false</c>.</returns>
        public static bool IsValidUSState(string strState)
        {
            // Names of 50 US States
            string[] stateNames =
            {
                "ALABAMA", "ALASKA", "ARIZONA", "ARKANSAS", "CALIFORNIA", "COLORADO", "CONNECTICUT", "DELAWARE",
                "FLORIDA", "GEORGIA", "HAWAII", "IDAHO", "ILLINOIS", "INDIANA", "IOWA", "KANSAS", "KENTUCKY",
                "LOUISIANA", "MAINE", "MARYLAND", "MASSACHUSETTS", "MICHIGAN", "MINNESOTA", "MISSISSIPPI", "MISSOURI",
                "MONTANA", "NEBRASKA", "NEVADA", "NEW HAMPSHIRE", "NEW JERSEY", "NEW MEXICO", "NEW YORK",
                "NORTH CAROLINA", "NORTH DAKOTA", "OHIO", "OKLAHOMA", "OREGON", "PENNSYLVANIA", "RHODE ISLAND",
                "SOUTH CAROLINA", "SOUTHDAKOTA", "TENNESSEE", "TEXAS", "UTAH", "VERMONT", "VIRGINIA", "WASHINGTON",
                "WEST VIRGINIA", "WISCONSIN", "WYOMING"
            };
            // Postal codes of 50 US States
            string[] stateCodes =
            {
                "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS",
                "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC",
                "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
            };
            // This one is somewhat easier because we have a finite set of values to check against.
            // We simply uppercase our value anc check against our list.
            strState = strState.ToUpper();
            var stateCodesArray = new ArrayList(stateCodes);
            var stateNamesArray = new ArrayList(stateNames);
            return stateCodesArray.Contains(strState) || stateNamesArray.Contains(strState);
        }

        /// <summary>
        ///     Determines whether [is valid zip code] [the specified string zip].
        /// </summary>
        /// <param name="strZIP">The string zip.</param>
        /// <returns><c>true</c> if [is valid zip code] [the specified string zip]; otherwise, <c>false</c>.</returns>
        public static bool IsValidZIPCode(string strZIP, string country)
        {
            if (string.IsNullOrEmpty(country))
                country = "";
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            // must be at least two characters long and caps out at 128 (database size)
            var regExPattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            switch(country.ToLower())
            {
                case "us": regExPattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$"; break;
                case "ca": regExPattern = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$"; break;
            }
            return MatchString(strZIP, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid us phone number] [the specified string phone].
        /// </summary>
        /// <param name="strPhone">The string phone.</param>
        /// <returns><c>true</c> if [is valid us phone number] [the specified string phone]; otherwise, <c>false</c>.</returns>
        public static bool IsValidUSPhoneNumber(string strPhone)
        {
            // Allows phone number of the format: NPA = [2-9][0-8][0-9] Nxx = [2-9][0-9][0-9] Station = [0-9][0-9][0-9][0-9]
            var regExPattern = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
            return MatchString(strPhone, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid cc number] [the specified string cc number].
        /// </summary>
        /// <param name="strCCNumber">The string cc number.</param>
        /// <returns><c>true</c> if [is valid cc number] [the specified string cc number]; otherwise, <c>false</c>.</returns>
        public static bool IsValidCCNumber(string strCCNumber)
        {
            // This expression is basically looking for series of numbers confirming to the standards
            // for Visa, MC, Discover and American Express with optional dashes between groups of numbers
            var regExPattern = @"^((4\d{3})|(5[1-5]\d{2})|(6011))-?\d{4}-?\d{4}-?\d{4}|3[4,7][\d\s-]{15}$";
            return MatchString(strCCNumber, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid SSN] [the specified string SSN].
        /// </summary>
        /// <param name="strSSN">The string SSN.</param>
        /// <returns><c>true</c> if [is valid SSN] [the specified string SSN]; otherwise, <c>false</c>.</returns>
        public static bool IsValidSSN(string strSSN)
        {
            strSSN = strSSN.Replace("-", "");
            // Allows SSN’s of the format 123-456-7890. Accepts hyphen delimited SSN’s or plain numeric values.
            var regExPattern = @"^\d{3}[-]?\d{2}[-]?\d{4}$";
            return MatchString(strSSN, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid email address2] [the specified string email].
        /// </summary>
        /// <param name="strEmail">The string email.</param>
        /// <returns><c>true</c> if [is valid email address2] [the specified string email]; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmailAddress2(string strEmail)
        {
            try
            {
                var addr = new MailAddress(strEmail);
                return addr.Address == strEmail;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether [is valid email address] [the specified string email].
        /// </summary>
        /// <param name="strEmail">The string email.</param>
        /// <returns><c>true</c> if [is valid email address] [the specified string email]; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmailAddress(string strEmail)
        {
            // Allows common email address that can start with a alphanumeric char and contain word, dash and period characters
            // followed by a domain name meeting the same criteria followed by a alpha suffix between 2 and 9 character lone
            var regExPattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            return MatchString(strEmail, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid URL] [the specified string URL].
        /// </summary>
        /// <param name="strURL">The string URL.</param>
        /// <returns><c>true</c> if [is valid URL] [the specified string URL]; otherwise, <c>false</c>.</returns>
        public static bool IsValidURL(string strURL)
        {
            // Allows HTTP and FTP URL’s, domain name must start with alphanumeric and can contain a port number
            // followed by a path containing a standard path character and ending in common file suffixies found in URL’s
            // and accounting for potential CGI GET data
            var regExPattern =
                @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\’\/\\\+&%\$#_=]*)?$";
            return MatchString(strURL, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid ip address] [the specified string ip].
        /// </summary>
        /// <param name="strIP">The string ip.</param>
        /// <returns><c>true</c> if [is valid ip address] [the specified string ip]; otherwise, <c>false</c>.</returns>
        public static bool IsValidIPAddress(string strIP)
        {
            // Allows four octets of numbers that contain values between 4 numbers in the IP address to 0-255 and are separated by periods
            var regExPattern =
                @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            return MatchString(strIP, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid alpha text] [the specified string alpha].
        /// </summary>
        /// <param name="strAlpha">The string alpha.</param>
        /// <returns><c>true</c> if [is valid alpha text] [the specified string alpha]; otherwise, <c>false</c>.</returns>
        public static bool IsValidAlphaText(string strAlpha)
        {
            // Allows one or more alphabetical characters. This is a more generic validation function.
            var regExPattern = @"^[A-Za-z]+$";
            return MatchString(strAlpha, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid alpha numeric text] [the specified string alpha number].
        /// </summary>
        /// <param name="strAlphaNum">The string alpha number.</param>
        /// <returns><c>true</c> if [is valid alpha numeric text] [the specified string alpha number]; otherwise, <c>false</c>.</returns>
        public static bool IsValidAlphaNumericText(string strAlphaNum)
        {
            // Allows one or more alphabetical and/or numeric characters. This is a more generic validation function.
            var regExPattern = @"^[A-Za-z0-9]+$";
            return MatchString(strAlphaNum, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid numeric text] [the specified string numeric].
        /// </summary>
        /// <param name="strNumeric">The string numeric.</param>
        /// <returns><c>true</c> if [is valid numeric text] [the specified string numeric]; otherwise, <c>false</c>.</returns>
        public static bool IsValidNumericText(string strNumeric)
        {
            // Allows one or more positive or negative, integer or decimal numbers. This is a more generic validation function.
            var regExPattern = @"/[+-]?\d+(\.\d+)?$";
            return MatchString(strNumeric, regExPattern);
        }

        /// <summary>
        ///     Determines whether [is valid routing number] [the specified routing number].
        /// </summary>
        /// <param name="routingNumber">The routing number.</param>
        /// <returns><c>true</c> if [is valid routing number] [the specified routing number]; otherwise, <c>false</c>.</returns>
        public static bool IsValidRoutingNumber(string routingNumber)
        {
            if (routingNumber.Length < 9) return false;

            //    Electronic Funds Transfer Routing Number Check
            long Sum = 0;
            Sum = 3 * Convert.ToInt32(routingNumber.Substring(0, 1)) +
                  7 * Convert.ToInt32(routingNumber.Substring(1, 1)) +
                  Convert.ToInt32(routingNumber.Substring(2, 1)) +
                  3 * Convert.ToInt32(routingNumber.Substring(3, 1)) +
                  7 * Convert.ToInt32(routingNumber.Substring(4, 1)) +
                  Convert.ToInt32(routingNumber.Substring(5, 1)) +
                  3 * Convert.ToInt32(routingNumber.Substring(6, 1)) +
                  7 * Convert.ToInt32(routingNumber.Substring(7, 1)) +
                  Convert.ToInt32(routingNumber.Substring(8, 1));

            return Sum % 10 == 0;
        }

        /// <summary>
        ///     Determines whether [is valid json] [the specified string input].
        /// </summary>
        /// <param name="strInput">The string input.</param>
        /// <returns><c>true</c> if [is valid json] [the specified string input]; otherwise, <c>false</c>.</returns>
        private static bool IsValidJson(string strInput)
        {
            var logService = AppEngineContext.Current.Resolve<ILogService>();
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException ex)
                {
                    //Arman Handle Exception
                    if (logService != null)
                        logService.Error(ex.Message, ex);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    //Arman Handle Exception
                    if (logService != null)
                        logService.Error(ex.Message, ex);
                    return false;
                }

            return false;
        }
    }
}