// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadFieldsContentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadFieldsContentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadFieldsContent}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadFieldsContent}" />
    public partial class LeadFieldsContentMap : AppEntityTypeConfiguration<LeadFieldsContent>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadFieldsContentMap()
        {
            this.ToTable("LeadFieldsContent");

            this.HasKey(x => x.Id);

            /*this.HasOptional(x => x.LeadMain)
                .WithMany()
                .HasForeignKey(x => x.LeadId)
                .WillCascadeOnDelete(true);*/
        }

        #endregion Constructor
    }
}