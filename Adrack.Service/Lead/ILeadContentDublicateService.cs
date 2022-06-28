// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadContentDublicateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadContentDublicateService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadContentDublicateId">The lead content dublicate identifier.</param>
        /// <returns>Profile Item</returns>
        LeadContentDublicate GetLeadContentDublicateById(long leadContentDublicateId);

        /// <summary>
        /// GetLeadContentDublicateByLeadId
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContentDublicate</returns>
        IList<LeadContentDublicate> GetLeadContentDublicateByLeadId(long leadId);

        /// <summary>
        /// Gets the lead content dublicate by SSN.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="ssn">The SSN.</param>
        /// <returns>IList&lt;LeadContentDublicate&gt;.</returns>
        IList<LeadContentDublicate> GetLeadContentDublicateBySSN(long leadId, string ssn);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<LeadContentDublicate> GetAllLeads();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadContentDublicate(LeadContentDublicate leadContentDublicate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        void UpdateLeadContentDublicate(LeadContentDublicate leadContentDublicate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        void DeleteLeadContentDublicate(LeadContentDublicate leadContentDublicate);

        #endregion Methods
    }
}