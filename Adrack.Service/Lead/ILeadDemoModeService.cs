// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 12-28-2020
//
// Last Modified By : Grigori D.
// Last Modified On : 12-28-2020
// ***********************************************************************
// <copyright file="ILeadDemoModeService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadDemoModeService
    {

        #region Export and Import XML Methods

        XElement SetCampaignXmlValue(Campaign campaign);
        XElement SetCampaignTemplateXmlValue(CampaignField obj);
        XElement SetBuyerXmlValue(Buyer buyer);
        XElement SetBuyerChannelXmlValue(BuyerChannel obj);
        XElement SetAffiliateXmlValue(Affiliate obj);
        XElement SetAffiliateChannelXmlValue(AffiliateChannel obj);


        #endregion Export and Import XML Methods


        #region Demo Generation Methods
        IList<LeadMainContent> GetDemoLeads(DateTime dateFrom, DateTime dateTo);
        void LeadPostingSimulation();

        #endregion Methods
    }
}