// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadMainResponseMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadMainResponseMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadMainResponse}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadMainResponse}" />
    public partial class LeadMainResponseMap : AppEntityTypeConfiguration<LeadMainResponse>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadMainResponseMap()
        {
            this.ToTable("LeadMainResponse");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}