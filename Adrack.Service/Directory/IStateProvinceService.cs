// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IStateProvinceService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using System.Collections.Generic;

namespace Adrack.Service.Directory
{
    /// <summary>
    /// Represents a State Province Service
    /// </summary>
    public partial interface IStateProvinceService
    {
        #region Methods

        /// <summary>
        /// Get State Province By Id
        /// </summary>
        /// <param name="stateProvinceId">State Province Identifier</param>
        /// <returns>State Province Item</returns>
        StateProvince GetStateProvinceById(long stateProvinceId);

        /// <summary>
        /// Get State Province By Country Id
        /// </summary>
        /// <param name="countryId">Country Identifier</param>
        /// <returns>State Province Collection Item</returns>
        IList<StateProvince> GetStateProvinceByCountryId(long countryId);

        /// <summary>
        /// Get All State Provinces
        /// </summary>
        /// <returns>State Province Collection Item</returns>
        IList<StateProvince> GetAllStateProvinces();

        /// <summary>
        /// Insert State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        void InsertStateProvince(StateProvince stateProvince);

        /// <summary>
        /// Update State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        void UpdateStateProvince(StateProvince stateProvince);

        /// <summary>
        /// Delete State Province
        /// </summary>
        /// <param name="stateProvince">State Province</param>
        void DeleteStateProvince(StateProvince stateProvince);

        #endregion Methods
    }
}