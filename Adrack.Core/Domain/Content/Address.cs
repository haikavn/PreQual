// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Address.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Membership;
using System;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Represents a Address
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class Address : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var address = new Address()
            {
                AddressTypeId = this.AddressTypeId,
                CountryId = this.CountryId,
                StateProvinceId = this.StateProvinceId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                AddressLine1 = this.AddressLine1,
                AddressLine2 = this.AddressLine2,
                City = this.City,
                ZipPostalCode = this.ZipPostalCode,
                Telephone = this.Telephone,
                Default = this.Default,
            };

            return address;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Address Type Identifier
        /// </summary>
        /// <value>The address type identifier.</value>
        public long AddressTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The country identifier.</value>
        public long? CountryId { get; set; }

        /// <summary>
        /// Gets or Sets the State Province Identifier
        /// </summary>
        /// <value>The state province identifier.</value>
        public long? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or Sets the First Name
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 1
        /// </summary>
        /// <value>The address line1.</value>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or Sets the Address Line 2
        /// </summary>
        /// <value>The address line2.</value>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or Sets the City
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or Sets the Zip Postal Code
        /// </summary>
        /// <value>The zip postal code.</value>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or Sets the Telephone
        /// </summary>
        /// <value>The telephone.</value>
        public long? Telephone { get; set; }

        /// <summary>
        /// Gets or Sets the Default
        /// </summary>
        /// <value><c>true</c> if default; otherwise, <c>false</c>.</value>
        public bool Default { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Address Type
        /// </summary>
        /// <value>The type of the address.</value>
        public virtual AddressType AddressType { get; set; }

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

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

        #endregion Navigation Properties
    }
}