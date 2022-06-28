// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerInvitation}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerInvitation}" />
    public partial class BuyerInvitationMap : AppEntityTypeConfiguration<BuyerInvitation>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerInvitationMap()
        {
            this.ToTable("BuyerInvitation");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}