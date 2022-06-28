// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadMainDublicateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadMainDublicateMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadContentDublicate}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadContentDublicate}" />
    public partial class LeadMainDublicateMap : AppEntityTypeConfiguration<LeadContentDublicate>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadMainDublicateMap()
        {
            this.ToTable("LeadContentDublicate");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}