// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead.Reports;
using System.Collections.Generic;

namespace Adrack.Service.Click
{
    /// <summary>
    /// Represents a Country Service
    /// </summary>
    public partial interface IClickService
    {
        #region Methods


        ClickChannel GetClickChannelByAccessKey(string accessKey);

        ClickChannel GetClickChannelByAffiliateChannelId(long affiliateChannelId, bool fromCache = true);

        void InsertClickChannel(ClickChannel clickChannel);

        void UpdateClickChannel(ClickChannel clickChannel);



        void InsertClickMain(ClickMain clickMain);

        void InsertClickContent(ClickContent clickMain);

        void InsertClickPostbackUrlLog(ClickPostbackUrlLog clickPostbackUrlLog);


        ClickContent GetClickContent(long clickChannelId, string paramName, string paramValue);

        List<ClickPostBackUrl> GetClickPostBackUrls(long clickChannelId);

        ReportClickCount GetClicksCount(long affiliateChannelId);

        #endregion Methods
    }
}