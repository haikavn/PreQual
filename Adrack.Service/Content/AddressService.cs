// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AddressService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Directory;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Address Service
    /// Implements the <see cref="Adrack.Service.Content.IAddressService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.IAddressService" />
    public partial class AddressService : IAddressService
    {
        #region Constants

        /// <summary>
        /// Cache Address By Id Key
        /// </summary>
        private const string CACHE_ADDRESS_BY_ID_KEY = "App.Cache.Address.By.Id-{0}";

        /// <summary>
        /// Cache Address By User Id Key
        /// </summary>
        private const string CACHE_ADDRESS_BY_USER_ID_KEY = "App.Cache.Address.By.User.Id-{0}";

        /// <summary>
        /// Cache Address By User Id Key, Address Type Id Key
        /// </summary>
        private const string CACHE_ADDRESS_BY_USER_ID_KEY_ADDRESS_TYPE_ID_KEY = "App.Cache.Address.By.User.Id-{0}.Address.Type.Id-{1}";

        /// <summary>
        /// Cache Address All Key
        /// </summary>
        private const string CACHE_ADDRESS_ALL_KEY = "App.Cache.Address.All";

        /// <summary>
        /// Cache Address Pattern Key
        /// </summary>
        private const string CACHE_ADDRESS_PATTERN_KEY = "App.Cache.Address.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Address Setting
        /// </summary>
        private readonly AddressSetting _addressSetting;

        /// <summary>
        /// Address
        /// </summary>
        private readonly IRepository<Address> _addressRepository;

        /// <summary>
        /// Address Type
        /// </summary>
        private readonly IRepository<AddressType> _addressTypeRepository;

        /// <summary>
        /// Country Service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// StateProvince Service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Address Service
        /// </summary>
        /// <param name="addressSetting">Address Setting</param>
        /// <param name="addressRepository">Address Repository</param>
        /// <param name="addressTypeRepository">Address Type Repository</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public AddressService(AddressSetting addressSetting, IRepository<Address> addressRepository, IRepository<AddressType> addressTypeRepository, ICountryService countryService, IStateProvinceService stateProvinceService, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._addressSetting = addressSetting;
            this._addressRepository = addressRepository;
            this._addressTypeRepository = addressTypeRepository;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Address By Id
        /// </summary>
        /// <param name="addressId">Address Identifier</param>
        /// <returns>Address Item</returns>
        public virtual Address GetAddressById(long addressId)
        {
            if (addressId == 0)
                return null;

            string key = string.Format(CACHE_ADDRESS_BY_ID_KEY, addressId);

            return _cacheManager.Get(key, () => { return _addressRepository.GetById(addressId); });
        }

        /// <summary>
        /// Get Address By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Address Collection Item</returns>
        public virtual IList<Address> GetAddressByUserId(long userId)
        {
            string key = string.Format(CACHE_ADDRESS_BY_USER_ID_KEY, userId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _addressRepository.Table
                            where x.UserId == userId
                            select x;

                var address = query.ToList();

                return address;
            });
        }

        /// <summary>
        /// Get Address By User Id And Address Type Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <param name="addressTypeId">Address Type Identifier</param>
        /// <returns>Address Collection Item</returns>
        public virtual Address GetAddressByUserIdAndAddressTypeId(long userId, long addressTypeId)
        {
            string key = string.Format(CACHE_ADDRESS_BY_USER_ID_KEY_ADDRESS_TYPE_ID_KEY, userId, addressTypeId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _addressRepository.Table
                            where x.UserId == userId &&
                                  x.AddressTypeId == addressTypeId
                            select x;

                var address = query.FirstOrDefault();

                return address;
            });
        }

        /// <summary>
        /// Get All Addresses
        /// </summary>
        /// <returns>Address Collection Item</returns>
        public virtual IList<Address> GetAllAddresses()
        {
            string key = CACHE_ADDRESS_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _addressRepository.Table
                            orderby x.FirstName, x.Id
                            select x;

                var address = query.ToList();

                return address;
            });
        }

        /// <summary>
        /// Insert Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <exception cref="ArgumentNullException">address</exception>
        public virtual void InsertAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            //if (address.CountryId == 0)
            //    address.CountryId = null;

            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;

            _addressRepository.Insert(address);

            _cacheManager.RemoveByPattern(CACHE_ADDRESS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(address);
        }

        /// <summary>
        /// Update Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <exception cref="ArgumentNullException">address</exception>
        public virtual void UpdateAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            //if (address.CountryId == 0)
            //    address.CountryId = null;

            //if (address.StateProvinceId == 0)
            //    address.StateProvinceId = null;

            _addressRepository.Update(address);

            _cacheManager.RemoveByPattern(CACHE_ADDRESS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(address);
        }

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <exception cref="ArgumentNullException">address</exception>
        public virtual void DeleteAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            _addressRepository.Delete(address);

            _cacheManager.RemoveByPattern(CACHE_ADDRESS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(address);
        }

        /// <summary>
        /// Validate Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Bool Item</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public virtual bool IsAddressValid(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (_addressSetting.FirstNameEnabled && _addressSetting.FirstNameRequired && String.IsNullOrWhiteSpace(address.FirstName))
                return false;

            if (_addressSetting.LastNameEnabled && _addressSetting.LastNameRequired && String.IsNullOrWhiteSpace(address.LastName))
                return false;

            if (_addressSetting.AddressLine1Enabled && _addressSetting.AddressLine1Required && String.IsNullOrWhiteSpace(address.AddressLine1))
                return false;

            if (_addressSetting.AddressLine2Enabled && _addressSetting.AddressLine2Required && String.IsNullOrWhiteSpace(address.AddressLine2))
                return false;

            if (_addressSetting.CountryEnabled && _addressSetting.CountryRequired)
                return false;

            if (_addressSetting.CityEnabled && _addressSetting.CityRequired && String.IsNullOrWhiteSpace(address.City))
                return false;

            if (_addressSetting.StateProvinceEnabled && _addressSetting.StateProvinceRequired)
                return false;

            if (_addressSetting.ZipPostalCodeEnabled && _addressSetting.ZipPostalCodeRequired && String.IsNullOrWhiteSpace(address.ZipPostalCode))
                return false;

            if (_addressSetting.TelephoneEnabled && _addressSetting.TelephoneRequired && String.IsNullOrWhiteSpace(address.Telephone.ToString()))
                return false;

            if (_addressSetting.CountryEnabled)
            {
                if (address.CountryId == null || address.CountryId == 0)
                    return false;

                var country = _countryService.GetCountryById(address.CountryId.Value);

                if (country == null)
                    return false;

                if (_addressSetting.StateProvinceEnabled)
                {
                    var stateProvince_0 = _stateProvinceService.GetStateProvinceByCountryId(country.Id);

                    if (stateProvince_0.Count > 0)
                    {
                        if (address.StateProvinceId == null || address.StateProvinceId == 0)
                            return false;

                        var stateProvince_1 = _stateProvinceService.GetStateProvinceById(address.StateProvinceId.Value);

                        if (stateProvince_1 == null)
                            return false;
                    }
                }
            }

            return true;
        }

        #region Address Type

        /// <summary>
        /// Get Address Type By Id
        /// </summary>
        /// <param name="addressTypeId">Address Type Identifier</param>
        /// <returns>Address Type Item</returns>
        public virtual AddressType GetAddressTypeById(long addressTypeId)
        {
            if (addressTypeId == 0)
                return null;

            return _addressTypeRepository.GetById(addressTypeId);
        }

        /// <summary>
        /// Get Address Type By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Address Type Item</returns>
        public virtual AddressType GetAddressTypeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from x in _addressTypeRepository.Table
                        orderby x.Name
                        where x.Name == name
                        select x;

            var address = query.FirstOrDefault();

            return address;
        }

        /// <summary>
        /// Get All Address Types
        /// </summary>
        /// <returns>Address Type Collection Item</returns>
        public virtual IList<AddressType> GetAllAddressTypes()
        {
            var query = from x in _addressTypeRepository.Table
                        orderby x.DisplayOrder, x.Id
                        select x;

            var addressTypes = query.ToList();

            return addressTypes;
        }

        /// <summary>
        /// Insert Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        /// <exception cref="ArgumentNullException">addressType</exception>
        public virtual void InsertAddressType(AddressType addressType)
        {
            if (addressType == null)
                throw new ArgumentNullException("addressType");

            _addressTypeRepository.Insert(addressType);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(addressType);
        }

        /// <summary>
        /// Update Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        /// <exception cref="ArgumentNullException">addressType</exception>
        public virtual void UpdateAddressType(AddressType addressType)
        {
            if (addressType == null)
                throw new ArgumentNullException("addressType");

            _addressTypeRepository.Update(addressType);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(addressType);
        }

        /// <summary>
        /// Delete Address Type
        /// </summary>
        /// <param name="addressType">Address Type</param>
        /// <param name="isPermanently">Is Permanently</param>
        /// <exception cref="ArgumentNullException">addressType</exception>
        public virtual void DeleteAddressType(AddressType addressType, bool isPermanently = false)
        {
            if (addressType == null)
                throw new ArgumentNullException("addressType");

            if (!isPermanently)
            {
                addressType.Published = false;
                addressType.Deleted = true;

                if (!String.IsNullOrEmpty(addressType.Name))
                    addressType.Name += "-DELETED";

                UpdateAddressType(addressType);
            }
            else
            {
                _addressTypeRepository.Delete(addressType);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityDeleted(addressType);
            }
        }

        #endregion Address Type

        #endregion Methods
    }
}