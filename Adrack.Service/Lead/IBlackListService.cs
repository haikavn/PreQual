// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBlackListService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBlackListService
    /// </summary>
    public partial interface IBlackListService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        BlackListType GetBlackListTypeById(long Id);

        /// <summary>
        /// Gets the type of the black list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>BlackListType.</returns>
        BlackListType GetBlackListType(string name);

        /// <summary>
        /// Gets the black list value by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BlackListValue.</returns>
        BlackListValue GetBlackListValueById(long Id);

        /// <summary>
        /// Gets the custom black list value by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>CustomBlackListValue.</returns>
        CustomBlackListValue GetCustomBlackListValueById(long Id);

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        IList<BlackListValue> GetAllBlackListValues(long Id);

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        IList<BlackListValue> GetAllBlackListValues(string value);

        /// <summary>
        /// Gets all black list values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="blackListTypeId">The black list type identifier.</param>
        /// <returns>IList&lt;BlackListValue&gt;.</returns>
        IList<BlackListValue> GetAllBlackListValues(string value, long blackListTypeId);

        /// <summary>
        /// Checks the black list value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>BlackListValue.</returns>
        bool CheckBlackListValue(string name, string value);

        /// <summary>
        /// Gets the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        /// <returns>IList&lt;CustomBlackListValue&gt;.</returns>
        IList<CustomBlackListValue> GetCustomBlackListValues(long channelId, short channelType);
        
        /// <summary>
        /// Gets the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>        
        /// <returns>Count of values</returns>
        /// 
        int GetCustomBlackListItemsCount(long channelId, short channelType);

        /// <summary>
        /// Gets the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        /// <param name="fieldId"></param>
        /// <returns>IList&lt;CustomBlackListValue&gt;.</returns>
        IList<CustomBlackListValue> GetCustomBlackListValues(long channelId, short channelType, long fieldId);

        bool CheckCustomBlackListValues(long channelId, short channelType, long fieldId, string value);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<BlackListType> GetAllBlackListTypes();

        /// <summary>
        /// Gets all black list types by parent identifier.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;BlackListType&gt;.</returns>
        IList<BlackListType> GetAllBlackListTypesByParentId(short type, long parentid);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        /// <returns>System.Int64.</returns>
        long InsertBlackListType(BlackListType blacklisttype);

        /// <summary>
        /// Inserts the black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        /// <returns>System.Int64.</returns>
        long InsertBlackListValue(BlackListValue blacklistvalue);

        /// <summary>
        /// Inserts the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        void InsertCustomBlackListValue(CustomBlackListValue blacklistvalue);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        void UpdateBlackListType(BlackListType blacklisttype);

        /// <summary>
        /// Updates the black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        void UpdateBlackListValue(BlackListValue blacklistvalue);

        /// <summary>
        /// Updates the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        void UpdateCustomBlackListValue(CustomBlackListValue blacklistvalue);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="blacklisttype">The blacklisttype.</param>
        void DeleteBlackListType(BlackListType blacklisttype);

        /// <summary>
        /// Deletes the black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        void DeleteBlackListValue(BlackListValue blacklistvalue);

        /// <summary>
        /// Deletes the custom black list value.
        /// </summary>
        /// <param name="blacklistvalue">The blacklistvalue.</param>
        void DeleteCustomBlackListValue(CustomBlackListValue blacklistvalue);

        /// <summary>
        /// Deletes the custom black list values.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="channelType">Type of the channel.</param>
        void DeleteCustomBlackListValues(long channelId, short channelType);

        #endregion Methods
    }
}