// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAddressService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Directory;
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Address Service
    /// </summary>
    public partial interface IAddressService
    {
        #region Methods

        /// <summary>
        /// Get Address By Id
        /// </summary>
        /// <param name="addressId">Address Identifier</param>
        /// <returns>Address Item</returns>
        Address GetAddressById(long addressId);

        /// <summary>
        /// Get Address By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Address Collection Item</returns>
        IList<Address> GetAddressByUserId(long userId);

        /// <summary>
        /// Get Address By User Id And Address Type Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <param name="addressTypeId">Address Type Identifier</param>
        /// <returns>Address Collection Item</returns>
        Address GetAddressByUserIdAndAddressTypeId(long userId, long addressTypeId);

        /// <summary>
        /// Get All Addresses
        /// </summary>
        /// <returns>Address Collection Item</returns>
        IList<Address> GetAllAddresses();

        /// <summary>
        /// Insert Address
        /// </summary>
        /// <param name="address">Address</param>
        void InsertAddress(Address address);

        /// <summary>
        /// Update Address
        /// </summary>
        /// <param name="address">Address</param>
        void UpdateAddress(Address address);

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="address">Address</param>
        void DeleteAddress(Address address);

        /// <summary>
        /// Validate Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Bool Item</returns>
        bool IsAddressValid(Address address);

        #region Address Type

        /// <summary>
        /// Gets the address type by identifier.
        /// </summary>
        /// <param name="addressTypeId">Address Type Identifier</param>
        /// <returns>Address Type Item</returns>
        AddressType GetAddressTypeById(long addressTypeId);

        /// <summary>
        /// Get Address Type By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Address Type Item</returns>
        AddressType GetAddressTypeByName(string name);

        /// <summary>
        /// Get All Address Types
        /// </summary>
        /// <returns>Address Type Collection Item</returns>
        IList<AddressType> GetAllAddressTypes();

        /// <summary>
        /// Insert Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        void InsertAddressType(AddressType addressType);

        /// <summary>
        /// Update Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        void UpdateAddressType(AddressType addressType);

        /// <summary>
        /// Delete Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        /// <param name="isPermanently">Is Permanently</param>
        void DeleteAddressType(AddressType addressType, bool isPermanently = false);

        #endregion Address Type

        #endregion Methods
    }
}