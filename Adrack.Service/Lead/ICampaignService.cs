// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICampaignService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface ICampaignService
    /// </summary>
    public partial interface ICampaignService
    {
        #region Methods

        /// <summary>
        /// Get Campaign By Id
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>Profile Item</returns>
        Campaign GetCampaignById(long campaignId, bool cached = false);

        /// <summary>
        /// Get Campaign parse XML  By Id
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>Profile Item</returns>
        /// 
        XmlDocument GetCampaignByIdXml(long campaignId);

        /// <summary>
        /// Gets the name of the campaign by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Campaign.</returns>
        Campaign GetCampaignByName(string name, long exceptId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        IList<Campaign> GetAllCampaigns(short deleted = 0);

        /// <summary>
        /// Gets all campaigns.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        IList<Campaign> GetAllCampaigns(short type, short deleted = 0);

        /// <summary>
        /// Gets campaigns list with filter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Campaign</returns>
        IList<Campaign> GetAllCampaignsByStatus(short deleted = 0, short status = -1);

        /// <summary>
        /// Gets the template campaigns.
        /// </summary>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        IList<Campaign> GetTemplateCampaigns();

        /// <summary>
        /// Gets the campaigns by vertical identifier.
        /// </summary>
        /// <param name="verticalId">The vertical identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        IList<Campaign> GetCampaignsByVerticalId(long verticalId, short deleted = 0);

        /// <summary>
        /// Gets the campaign templates by vertical identifier.
        /// </summary>
        /// <param name="verticalId">The vertical identifier.</param>
        /// <returns>IList&lt;Campaign&gt;.</returns>
        IList<Campaign> GetCampaignTemplatesByVerticalId(long verticalId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns>System.Int64.</returns>
        long InsertCampaign(Campaign campaign);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        void UpdateCampaign(Campaign campaign);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        void DeleteCampaign(Campaign campaign);

        /// <summary>
        /// Deletes the campaign templates.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        void DeleteCampaignTemplates(long campaignId);

        long InsertPingTree(PingTree pingTree);

        void UpdatePingTree(PingTree pingTree);

        void DeletePingTree(PingTree pingTree);

        long InsertPingTreeItem(PingTreeItem pingTreeItem);

        void UpdatePingTreeItem(PingTreeItem pingTreeItem);

        void DeletePingTreeItem(PingTreeItem pingTreeItem);

        void DeletePingTreeItems(long pingTreeId);


        IList<PingTree> GetPingTrees(long campaignId);

        IList<PingTreeItem> GetPingTreeItems(long pingTreeId, bool cached = false);

        PingTree GetPingTreeById(long Id, bool cached = false);

        PingTreeItem GetPingTreeItemById(long Id, bool cached = false);


        List<CampaignList> GetCampaignListWithVerticals(ActivityStatuses status);

        #endregion Methods

        string GetConnectionString();

        List<string> GetAllKeys();

    }
}