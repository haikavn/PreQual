// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CampaignTemplateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class CampaignTemplateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CampaignField}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.CampaignField}" />
    public partial class CampaignTemplateMap : AppEntityTypeConfiguration<CampaignField>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public CampaignTemplateMap() // elite group
        {
            this.ToTable("CampaignTemplate");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}