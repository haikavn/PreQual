// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadContentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadContentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadContent}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadContent}" />
    public partial class LeadContentMap : AppEntityTypeConfiguration<LeadContent>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadContentMap()
        {
            this.ToTable("LeadContent");

            this.HasKey(x => x.Id);

            /*this.HasOptional(x => x.LeadMain)
                .WithMany()
                .HasForeignKey(x => x.LeadId)
                .WillCascadeOnDelete(true);*/
        }

        #endregion Constructor
    }
}