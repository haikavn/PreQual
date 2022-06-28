// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Buyer.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Attributes;
using Adrack.Core.Domain.Directory;
using Newtonsoft.Json;
using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Buyer.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    [Tracked]
    public partial class Buyer : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var buyer = new Buyer()
            {
            };

            return buyer;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The country identifier.</value>
        public long CountryId { get; set; }

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The state province identifier.</value>
        public long? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public long? ManagerId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        [Tracked]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [Tracked]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [Tracked]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        [Tracked]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the zip postal code.
        /// </summary>
        /// <value>The zip postal code.</value>
        [Tracked]
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [Tracked]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Tracked]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Tracked]
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the bill frequency.
        /// </summary>
        /// <value>The bill frequency.</value>
        public string BillFrequency { get; set; }

        /// <summary>
        /// Gets or sets the frequency value.
        /// </summary>
        /// <value>The frequency value.</value>
        public int? FrequencyValue { get; set; }

        /// <summary>
        /// Gets or sets the last posted sold.
        /// </summary>
        /// <value>The last posted sold.</value>
        public DateTime? LastPostedSold { get; set; }

        /// <summary>
        /// Gets or sets the last posted.
        /// </summary>
        /// <value>The last posted.</value>
        public DateTime? LastPosted { get; set; }

        /// <summary>
        /// Gets or sets the always sold option.
        /// </summary>
        /// <value>The always sold option.</value>
        public short AlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the maximum duplicate days.
        /// </summary>
        /// <value>The maximum duplicate days.</value>
        public short MaxDuplicateDays { get; set; }

        /// <summary>
        /// Gets or sets the daily cap.
        /// </summary>
        /// <value>The daily cap.</value>
        public int DailyCap { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>The external identifier.</value>
        public long? ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Buyer"/> is deleted.
        /// </summary>
        /// <value><c>null</c> if [deleted] contains no value, <c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        public bool? Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is bi weekly.
        /// </summary>
        /// <value><c>null</c> if [is bi weekly] contains no value, <c>true</c> if [is bi weekly]; otherwise, <c>false</c>.</value>
        public bool? IsBiWeekly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cool off enabled].
        /// </summary>
        /// <value><c>null</c> if [cool off enabled] contains no value, <c>true</c> if [cool off enabled]; otherwise, <c>false</c>.</value>
        public bool? CoolOffEnabled { get; set; }

        /// <summary>
        /// Gets or sets the cool off start.
        /// </summary>
        /// <value>The cool off start.</value>

        public DateTime? CoolOffStart { get; set; }

        /// <summary>
        /// Gets or sets the cool off end.
        /// </summary>
        /// <value>The cool off end.</value>

        public DateTime? CoolOffEnd { get; set; }

        /// <summary>
        /// Gets or sets do not present status.
        /// </summary>
        public short? DoNotPresentStatus { get; set; }

        /// <summary>
        /// Gets or sets do not present url.
        /// </summary>
        public string DoNotPresentUrl { get; set; }

        /// <summary>
        /// Gets or sets do not present response field.
        /// </summary>
        public string DoNotPresentResultField { get; set; }

        /// <summary>
        /// Gets or sets do not present response value.
        /// </summary>
        public string DoNotPresentResultValue { get; set; }

        public string DoNotPresentRequest { get; set; }

        public string DoNotPresentPostMethod { get; set; }

        public bool? CanSendLeadId { get; set; }

        public int? AccountId { get; set; }

        public string IconPath { get; set; }


        public bool? SendStatementReport { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Country
        /// </summary>
        /// <value>The country.</value>
        public virtual Country Country { get; set; }

        /// <summary>
        /// Gets or Sets the State Province
        /// </summary>
        /// <value>The state province.</value>
        public virtual StateProvince StateProvince { get; set; }


        /// <summary>
        /// Gets or Sets the AutosendInvoice
        /// </summary>
        /// <value>The AutosendInvoice.</value>
        public bool? AutosendInvoice { get; set; }

        #endregion Navigation Properties
    }
}