// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Affiliate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Attributes;
using Adrack.Core.Domain.Directory;
using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Affiliate
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// 
    [Tracked]
    public partial class Affiliate : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var affiliate = new Affiliate()
            {
            };

            return affiliate;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The country identifier.</value>
        /// 
        [Tracked(DisplayName = "Country", TableName = "Country")]
        public long CountryId { get; set; }

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The state province identifier.</value>

        [Tracked(DisplayName = "State", TableName = "StateProvince")]

        public long? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        /// 
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
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Tracked]
        public ActivityStatuses Status { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        [Tracked]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Affiliate"/> is deleted.
        /// </summary>
        /// <value><c>null</c> if [deleted] contains no value, <c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion Properties

    }
}