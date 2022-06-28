// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerChannelScheduleService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerChannelScheduleService
    /// </summary>
    public partial interface IBuyerChannelScheduleService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        BuyerChannelSchedule GetBuyerChannelScheduleById(long Id);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerChannelSchedule> GetAllBuyerChannelSchedules();

        /// <summary>
        /// Gets the lead schedules by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="cache">Caching parameter, yes/no</param>
        /// <returns>IList&lt;BuyerChannelSchedule&gt;.</returns>
        IList<BuyerChannelSchedule> GetBuyerChannelsByBuyerChannelId(long buyerChannelId, bool cache=true, bool allQuantities = false);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The lead schedule.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The lead schedule.</param>
        void UpdateBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannelSchedule">The lead schedule.</param>
        void DeleteBuyerChannelSchedule(BuyerChannelSchedule buyerChannelSchedule);

        /// <summary>
        /// Deletes the lead schedules by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        void DeleteBuyerChannelSchedulesByBuyerChannelId(long buyerChannelId);

        #endregion Methods
    }
}