// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICampaignTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface ICampaignTemplateService
    /// </summary>
    public partial interface ICampaignTemplateService
    {
        #region Methods

        /// <summary>
        /// Gets the campaign template by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>CampaignTemplate.</returns>
        CampaignField GetCampaignTemplateById(long Id, bool cached = false);

        /// <summary>
        /// Determines whether [is campaign template hidden] [the specified campaign identifier].
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns><c>true</c> if [is campaign template hidden] [the specified campaign identifier]; otherwise, <c>false</c>.</returns>
        bool IsCampaignTemplateHidden(long campaignId, string fieldName);

        /// <summary>
        /// Campaigns the template allowed names.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> CampaignTemplateAllowedNames(long campaignId);

        /// <summary>
        /// Gets the name of the campaign template by section and.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>CampaignTemplate.</returns>
        CampaignField GetCampaignTemplateBySectionAndName(string sectionName, string fieldName, long ID);

        /// <summary>
        /// Gets the campaign templates by section.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>IList&lt;CampaignTemplate&gt;.</returns>
        IList<CampaignField> GetCampaignTemplatesBySection(long campaignId, string sectionName);

        /// <summary>
        /// Gets the campaign template by validator.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>CampaignTemplate.</returns>
        CampaignField GetCampaignTemplateByValidator(long campaignId, short validator);

        /// <summary>
        /// Gets the campaign templates by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="order">The order.</param>
        /// <param name="isfilterable">The isfilterable.</param>
        /// <returns>IList&lt;CampaignTemplate&gt;.</returns>
        IList<CampaignField> GetCampaignTemplatesByCampaignId(long campaignId, string order = "asc", short isfilterable = 0);

        /// <summary>
        /// Inserts the campaign template.
        /// </summary>
        /// <param name="campaignTemplate">The campaign template.</param>
        /// <returns>System.Int64.</returns>
        long InsertCampaignTemplate(CampaignField campaignTemplate);

        /// <summary>
        /// Updates the campaign template.
        /// </summary>
        /// <param name="campaignTemplate">The campaign template.</param>
        void UpdateCampaignTemplate(CampaignField campaignTemplate);

        /// <summary>
        /// Deletes the campaign template.
        /// </summary>
        /// <param name="tpl">The TPL.</param>
        void DeleteCampaignTemplate(CampaignField tpl);

        #endregion Methods
    }
}