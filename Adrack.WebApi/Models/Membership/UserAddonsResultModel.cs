using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Membership
{
    public class UserAddonsResultModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the Addon identifier.
        /// </summary>
        /// <value>The Addon identifier.</value>
        public long AddonId { get; set; }

        /// <summary>
        /// Gets or sets from Date.
        /// </summary>
        /// <value>Date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>The Status.</value>
        public short? Status { get; set; }


        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        /// <value>The Amount.</value>
        public double? UserAddonAmount { get; set; }


        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>The Description.</value>
        public string Description { get; set; }


        #endregion Properties
    }
}