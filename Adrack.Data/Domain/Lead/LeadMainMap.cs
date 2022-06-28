// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadMainMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadMainMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadMain}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadMain}" />
    public partial class LeadMainMap : AppEntityTypeConfiguration<LeadMain>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadMainMap()
        {
            this.ToTable("LeadMain");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}