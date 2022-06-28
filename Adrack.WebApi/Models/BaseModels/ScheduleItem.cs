using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    /// <summary>
    /// Class ScheduleItem.
    /// </summary>
    public class ScheduleItem
    {
        /// <summary>
        /// The day
        /// </summary>
        public string Day = string.Empty;

        /// <summary>
        /// From hour
        /// </summary>
        public string FromHour = string.Empty;

        /// <summary> 
        /// From minute
        /// </summary>
        public string FromMinute = string.Empty;

        /// <summary>
        /// Converts to hour.
        /// </summary>
        public string ToHour = string.Empty;

        /// <summary>
        /// Converts to minute.
        /// </summary>
        public string ToMinute = string.Empty;

        /// <summary>
        /// The quantity
        /// </summary>
        public string Quantity = "0";

        /// <summary>
        /// The posted wait
        /// </summary>
        public string PostedWait = "0";

        /// <summary>
        /// The sold wait
        /// </summary>
        public string SoldWait = "0";

        /// <summary>
        /// The hour maximum
        /// </summary>
        public string HourMax = "0";

        /// <summary>
        /// The price
        /// </summary>
        public string Price = "0";

        public string LeadStatus = "0";
    }
}