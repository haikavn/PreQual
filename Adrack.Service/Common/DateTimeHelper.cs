// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="DateTimeHelper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web.Mvc;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Date Time Helper
    /// Implements the <see cref="Adrack.Service.Common.IDateTimeHelper" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Common.IDateTimeHelper" />
    public partial class DateTimeHelper : IDateTimeHelper
    {
        #region Fields

        /// <summary>
        /// Date Time Setting
        /// </summary>
        private readonly DateTimeSetting _dateTimeSetting;

        /// <summary>
        /// Global Attribute Service
        /// </summary>
        private readonly IGlobalAttributeService _globalAttributeService;

        /// <summary>
        /// Setting Service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Date Time Helper
        /// </summary>
        /// <param name="dateTimeSetting">Date Time Setting</param>
        /// <param name="globalAttributeService">Global Attribute Service</param>
        /// <param name="settingService">Setting Service</param>
        /// <param name="appContext">Application Context</param>
        public DateTimeHelper(DateTimeSetting dateTimeSetting, IGlobalAttributeService globalAttributeService, ISettingService settingService, IAppContext appContext)
        {
            this._dateTimeSetting = dateTimeSetting;
            this._globalAttributeService = globalAttributeService;
            this._settingService = settingService;
            this._appContext = appContext;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Find Time Zone By Id
        /// </summary>
        /// <param name="id">Find Time Zone Identifier</param>
        /// <returns>Time Zone Info Item</returns>
        public virtual TimeZoneInfo FindTimeZoneById(string id)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        public List<SelectListItem> GetTimeZones(string selectedValue = "")
        {
            List<SelectListItem> timeZones = new List<SelectListItem>();
            timeZones.Add(new SelectListItem() { Value = "-12", Text = "(GMT -12:00) Eniwetok, Kwajalein", Selected = (selectedValue == "-12" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-11", Text = "(GMT -11:00) Midway Island, Samoa", Selected = (selectedValue == "-11" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-10", Text = "(GMT -10:00) Hawaii", Selected = (selectedValue == "-10" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-9", Text = "(GMT -9:00) Alaska", Selected = (selectedValue == "-9" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-8", Text = "(GMT -8:00) Pacific Time (US & Canada)", Selected = (selectedValue == "-8" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-7", Text = "(GMT -7:00) Mountain Time (US & Canada)", Selected = (selectedValue == "-7" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-6", Text = "(GMT -6:00) Central Time (US & Canada), Mexico City", Selected = (selectedValue == "-6" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-5", Text = "(GMT -5:00) Eastern Time (US & Canada), Bogota, Lima", Selected = (selectedValue == "-5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-4", Text = "(GMT -4:00) Atlantic Time (Canada), Caracas, La Paz", Selected = (selectedValue == "-4" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-3.5", Text = "(GMT -3:30) Newfoundland", Selected = (selectedValue == "-3.5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-3", Text = "(GMT -3:00) Brazil, Buenos Aires, Georgetown", Selected = (selectedValue == "-3" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-2", Text = "(GMT -2:00) Mid-Atlantic", Selected = (selectedValue == "-2" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "-1", Text = "(GMT -1:00 hour) Azores, Cape Verde Islands", Selected = (selectedValue == "-1" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "0", Text = "(GMT) Western Europe Time, London, Lisbon, Casablanca", Selected = (selectedValue == "0" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "1", Text = "(GMT +1:00 hour) Brussels, Copenhagen, Madrid, Paris", Selected = (selectedValue == "1" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "2", Text = "(GMT +2:00) Kaliningrad, South Africa", Selected = (selectedValue == "2" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "3", Text = "(GMT +3:00) Baghdad, Riyadh, Moscow, St. Petersburg", Selected = (selectedValue == "3" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "3.5", Text = "(GMT +3:30) Tehran", Selected = (selectedValue == "3.5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "4", Text = "(GMT +4:00) Abu Dhabi, Muscat, Baku, Tbilisi", Selected = (selectedValue == "4" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "4.5", Text = "(GMT +4:30) Kabul", Selected = (selectedValue == "4.5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "5", Text = "(GMT +5:00) Ekaterinburg, Islamabad, Karachi, Tashkent", Selected = (selectedValue == "5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "5.5", Text = "(GMT +5:30) Bombay, Calcutta, Madras, New Delhi", Selected = (selectedValue == "5.5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "5.75", Text = "(GMT +5:45) Kathmandu", Selected = (selectedValue == "5.75" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "6", Text = "(GMT +6:00) Almaty, Dhaka, Colombo", Selected = (selectedValue == "6" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "7", Text = "GMT +7:00) Bangkok, Hanoi, Jakarta", Selected = (selectedValue == "7" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "8", Text = "(GMT + 8:00) Beijing, Perth, Singapore, Hong Kong", Selected = (selectedValue == "8" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "9", Text = "(GMT +9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk", Selected = (selectedValue == "9" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "9.5", Text = "(GMT +9:30) Adelaide, Darwin", Selected = (selectedValue == "9.5" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "10", Text = "(GMT +10:00) Eastern Australia, Guam, Vladivostok", Selected = (selectedValue == "10" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "11", Text = "(GMT +11:00) Magadan, Solomon Islands, New Caledonia", Selected = (selectedValue == "11" ? true : false) });
            timeZones.Add(new SelectListItem() { Value = "12", Text = "(GMT +12:00) Auckland, Wellington, Fiji, Kamchatka", Selected = (selectedValue == "12" ? true : false) });
            return timeZones;
        }

        /// <summary>
        /// Gets the system time zones.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        public List<SelectListItem> GetSystemTimeZones(string selectedValue = "")
        {
            List<SelectListItem> timeZones = new List<SelectListItem>();

            ReadOnlyCollection<TimeZoneInfo> systemTimeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo tzi in systemTimeZones)
            {
                timeZones.Add(new SelectListItem() { Value = tzi.StandardName, Text = tzi.DisplayName, Selected = (selectedValue == tzi.StandardName ? true : false) });
            }

            return timeZones;
        }

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUserTime(DateTime dateTime)
        {
            return ConvertToUserTime(dateTime, dateTime.Kind);
        }

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceDateTimeKind">Source Date Time Kind</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUserTime(DateTime dateTime, DateTimeKind sourceDateTimeKind)
        {
            dateTime = DateTime.SpecifyKind(dateTime, sourceDateTimeKind);

            var currentUserTimeZoneInfo = this.CurrentTimeZone;

            return TimeZoneInfo.ConvertTime(dateTime, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            var currentUserTimeZoneInfo = this.CurrentTimeZone;

            return ConvertToUserTime(dateTime, sourceTimeZone, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// Convert To User Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <param name="destinationTimeZone">Destination Time Zone</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUtcTime(DateTime dateTime)
        {
            return ConvertToUtcTime(dateTime, dateTime.Kind);
        }

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceDateTimeKind">Source Date Time Kind</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUtcTime(DateTime dateTime, DateTimeKind sourceDateTimeKind)
        {
            dateTime = DateTime.SpecifyKind(dateTime, sourceDateTimeKind);

            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        /// <summary>
        /// Convert To Utc Time
        /// </summary>
        /// <param name="dateTime">Date Time</param>
        /// <param name="sourceTimeZone">Source Time Zone</param>
        /// <returns>Date Time Item</returns>
        public virtual DateTime ConvertToUtcTime(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            if (sourceTimeZone.IsInvalidTime(dateTime))
            {
                // Could Not Convert
                return dateTime;
            }

            return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
        }

        /// <summary>
        /// Get User Time Zone
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Date Time Item</returns>
        public virtual TimeZoneInfo GetUserTimeZone(User user)
        {
            // Registered User
            TimeZoneInfo timeZoneInfo = null;

            if (_dateTimeSetting.AllowUserToSetTimeZone)
            {
                string timeZoneId = string.Empty;

                if (user != null)
                    timeZoneId = user.GetGlobalAttribute<string>(GlobalAttributeBuiltIn.TimeZoneId, _globalAttributeService);

                try
                {
                    if (!String.IsNullOrEmpty(timeZoneId))
                        timeZoneInfo = FindTimeZoneById(timeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }
            }

            // Default Timezone
            if (timeZoneInfo == null)
                timeZoneInfo = this.AppTimeZone;

            return timeZoneInfo;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Applilcation Time Zone
        /// </summary>
        /// <value>The application time zone.</value>
        public virtual TimeZoneInfo AppTimeZone
        {
            get
            {
                TimeZoneInfo timeZoneInfo = null;

                try
                {
                    if (!String.IsNullOrEmpty(_dateTimeSetting.AppTimeZoneId))
                        timeZoneInfo = FindTimeZoneById(_dateTimeSetting.AppTimeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }

                if (timeZoneInfo == null)
                    timeZoneInfo = TimeZoneInfo.Local;

                return timeZoneInfo;
            }
            set
            {
                string defaultTimeZoneId = string.Empty;

                if (value != null)
                {
                    defaultTimeZoneId = value.Id;
                }

                _dateTimeSetting.AppTimeZoneId = defaultTimeZoneId;

                _settingService.SaveSetting(_dateTimeSetting);
            }
        }

        /// <summary>
        /// Gets or Sets the Current Time Zone
        /// </summary>
        /// <value>The current time zone.</value>
        public virtual TimeZoneInfo CurrentTimeZone
        {
            get
            {
                return GetUserTimeZone(_appContext.AppUser);
            }
            set
            {
                if (!_dateTimeSetting.AllowUserToSetTimeZone)
                    return;

                string timeZoneId = string.Empty;

                if (value != null)
                {
                    timeZoneId = value.Id;
                }

                _globalAttributeService.SaveGlobalAttribute(_appContext.AppUser, GlobalAttributeBuiltIn.TimeZoneId, timeZoneId);
            }
        }

        #endregion Properties
    }
}