// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadSensitiveDataService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadSensitiveDataService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadSensitiveDataId">The lead sensitive data identifier.</param>
        /// <returns>Profile Item</returns>
        LeadSensitiveData GetLeadSensitiveDataById(long leadSensitiveDataId);

        /// <summary>
        /// Gets the lead sensitive data by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadSensitiveData.</returns>
        LeadSensitiveData GetLeadSensitiveDataByLeadId(long leadId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadSensitiveData(LeadSensitiveData leadMain);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void UpdateLeadSensitiveData(LeadSensitiveData leadMain);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void DeleteLeadSensitiveData(LeadSensitiveData leadMain);

        #endregion Methods
    }
}