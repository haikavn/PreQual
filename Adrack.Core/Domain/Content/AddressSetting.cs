// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AddressSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Represents a Address Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class AddressSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the First Name Enabled
        /// </summary>
        /// <value><c>true</c> if [first name enabled]; otherwise, <c>false</c>.</value>
        public bool FirstNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the First Name Required
        /// </summary>
        /// <value><c>true</c> if [first name required]; otherwise, <c>false</c>.</value>
        public bool FirstNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name Enabled
        /// </summary>
        /// <value><c>true</c> if [last name enabled]; otherwise, <c>false</c>.</value>
        public bool LastNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name Required
        /// </summary>
        /// <value><c>true</c> if [last name required]; otherwise, <c>false</c>.</value>
        public bool LastNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 1 Enabled
        /// </summary>
        /// <value><c>true</c> if [address line1 enabled]; otherwise, <c>false</c>.</value>
        public bool AddressLine1Enabled { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 1 Required
        /// </summary>
        /// <value><c>true</c> if [address line1 required]; otherwise, <c>false</c>.</value>
        public bool AddressLine1Required { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 2 Enabled
        /// </summary>
        /// <value><c>true</c> if [address line2 enabled]; otherwise, <c>false</c>.</value>
        public bool AddressLine2Enabled { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 2 Required
        /// </summary>
        /// <value><c>true</c> if [address line2 required]; otherwise, <c>false</c>.</value>
        public bool AddressLine2Required { get; set; }

        /// <summary>
        /// Gets or Sets the Country Enabled
        /// </summary>
        /// <value><c>true</c> if [country enabled]; otherwise, <c>false</c>.</value>
        public bool CountryEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Country Required
        /// </summary>
        /// <value><c>true</c> if [country required]; otherwise, <c>false</c>.</value>
        public bool CountryRequired { get; set; }

        /// <summary>
        /// Gets or Sets the City Enabled
        /// </summary>
        /// <value><c>true</c> if [city enabled]; otherwise, <c>false</c>.</value>
        public bool CityEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the City Required
        /// </summary>
        /// <value><c>true</c> if [city required]; otherwise, <c>false</c>.</value>
        public bool CityRequired { get; set; }

        /// <summary>
        /// Gets or Sets the State Province Enabled
        /// </summary>
        /// <value><c>true</c> if [state province enabled]; otherwise, <c>false</c>.</value>
        public bool StateProvinceEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the State Province Required
        /// </summary>
        /// <value><c>true</c> if [state province required]; otherwise, <c>false</c>.</value>
        public bool StateProvinceRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Zip Postal Code Enabled
        /// </summary>
        /// <value><c>true</c> if [zip postal code enabled]; otherwise, <c>false</c>.</value>
        public bool ZipPostalCodeEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Zip Postal Code Required
        /// </summary>
        /// <value><c>true</c> if [zip postal code required]; otherwise, <c>false</c>.</value>
        public bool ZipPostalCodeRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Telephone Enabled
        /// </summary>
        /// <value><c>true</c> if [telephone enabled]; otherwise, <c>false</c>.</value>
        public bool TelephoneEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Telephone Required
        /// </summary>
        /// <value><c>true</c> if [telephone required]; otherwise, <c>false</c>.</value>
        public bool TelephoneRequired { get; set; }

        #endregion Properties
    }
}