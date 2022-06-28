// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateChannelFilterConditionMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateChannelFilterConditionMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelFilterCondition}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannelFilterCondition}" />
    public partial class AffiliateChannelFilterConditionMap : AppEntityTypeConfiguration<AffiliateChannelFilterCondition>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateChannelFilterConditionMap() // elite group
        {
            this.ToTable("AffiliateChannelFilterCondition");
        }

        #endregion Constructor
    }
}