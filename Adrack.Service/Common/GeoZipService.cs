// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 05-05-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-05-2020
// ***********************************************************************
// <copyright file="GeoZipService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a GeoZip Service
    /// Implements the <see cref="Adrack.Service.Common.IGeoZipService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Common.IGeoZipService" />
    public partial class GeoZipService : IGeoZipService
    {
        #region Constants

        /// <summary>
        /// Cache GeoZip By Id Key
        /// </summary>
        private const string CACHE_GEOZIP_ALL_KEY = "App.Cache.GeoZip.By.Id-{0}";

        #endregion Constants

        #region Fields

        /// <summary>
        /// GeoZip
        /// </summary>
        private readonly IRepository<GeoZip> _geoZipRepository;

        /// <summary>
        /// AbaNumber
        /// </summary>
        private readonly IRepository<AbaNumber> _abaNumberRepository;


        #endregion Fields

        #region Constructor

        /// <summary>
        /// GeoZip Service
        /// </summary>
        /// <param name="geoZipRepository">GeoZip Repository</param>
        public GeoZipService(IRepository<GeoZip> geoZipRepository, IRepository<AbaNumber> abaNumberRepository)
        {
            this._geoZipRepository = geoZipRepository;
            this._abaNumberRepository = abaNumberRepository;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// GetGeoDataByZip
        /// </summary>
        /// <returns>Navigation Collection Item</returns>
        public virtual GeoZip GetGeoDataByZip(int zip)
        {
            var query = from x in _geoZipRepository.Table
                        where x.zip == zip
                        select x;

            var geodata = query.FirstOrDefault();

            return geodata;
        }

        /// <summary>
        /// GetBankByAbaNumber
        /// </summary>
        /// <returns>Navigation Collection Item</returns>
        public virtual AbaNumber GetBankByAbaNumber(long abanumber)
        {
            var query = from x in _abaNumberRepository.Table
                        where x.abanumber == abanumber
                        select x;

            var bankdata = query.FirstOrDefault();

            return bankdata;
        }

        #endregion Methods
    }
}