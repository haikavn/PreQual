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
using Adrack.Core.Domain.Click;

namespace Adrack.Data.Domain.Click
{
    /// <summary>
    /// Class ClickChannelMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Click.ClickChannel}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Click.ClickChannel}" />
    public partial class ClickPostBackUrlLogMap : AppEntityTypeConfiguration<ClickPostbackUrlLog>
    {
        #region Constructor


        public ClickPostBackUrlLogMap() // elite group
        {
            this.ToTable("ClickPostbackUrlLog");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}