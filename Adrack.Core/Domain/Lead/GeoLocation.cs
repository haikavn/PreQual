// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="GeoLocation.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a LeadContent
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class GeoLocation : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var leadGetData = new GeoLocation()
            {
            };

            return leadGetData;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Lead Geo Data
        /// </summary>
        /// <value>The country code.</value>

        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>The name of the country.</value>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the name of the region.
        /// </summary>
        /// <value>The name of the region.</value>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the area code.
        /// </summary>
        /// <value>The area code.</value>
        public string AreaCode { get; set; }

        #endregion Properties
    }
}