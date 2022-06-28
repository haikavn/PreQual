// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IDoNotPresentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IDoNotPresentService
    /// </summary>
    public partial interface ISubIdWhiteListService
    {
        #region Methods

        /// <summary>
        /// Insert DoNotPresent
        /// </summary>
        /// <param name="filter">The DoNotPresent.</param>
        /// <returns>System.Int64.</returns>
        long InsertSubIdWhiteList(SubIdWhiteList subIdWhiteList);

        int CheckSubIdWhiteList(string subId, long buyerChannelId);

        IList<SubIdWhiteList> GetAllSubIdWhiteList();

        IList<SubIdWhiteList> GetAllSubIdWhiteList(long buyerChannelId);
        void DeleteSubIdWhiteList(SubIdWhiteList subIdWhiteList);
        void DeleteAllSubIdWhiteList(long buyerChannelId);

        #endregion Methods
    }
}