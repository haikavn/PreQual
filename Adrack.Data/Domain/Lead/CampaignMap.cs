// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CampaignMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class CampaignMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Campaign}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Campaign}" />
    public partial class CampaignMap : AppEntityTypeConfiguration<Campaign>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public CampaignMap() // elite group
        {
            this.ToTable("Campaign");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}