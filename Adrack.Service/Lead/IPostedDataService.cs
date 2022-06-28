// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IPostedDataService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IPostedDataService
    /// </summary>
    public partial interface IPostedDataService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="postedDataId">The posted data identifier.</param>
        /// <returns>Profile Item</returns>
        PostedData GetPostedDataById(long postedDataId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<PostedData> GetAllPostedDatas();

        /// <summary>
        /// Gets the posted datas by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;PostedData&gt;.</returns>
        IList<PostedData> GetPostedDatasByBuyerChannelId(long buyerChannelId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="postedData">The posted data.</param>
        /// <returns>System.Int64.</returns>
        long InsertPostedData(PostedData postedData);

        long InsertPostedDataList(IEnumerable<PostedData> list);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="postedData">The posted data.</param>
        void UpdatePostedData(PostedData postedData);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="postedData">The posted data.</param>
        void DeletePostedData(PostedData postedData);

        #endregion Methods
    }
}