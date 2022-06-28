// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadSensitiveDataMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class LeadSensitiveDataMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadSensitiveData}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.LeadSensitiveData}" />
    public partial class LeadSensitiveDataMap : AppEntityTypeConfiguration<LeadSensitiveData>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public LeadSensitiveDataMap()
        {
            this.ToTable("LeadSensitiveData");

            this.HasKey(x => x.Id);

            /*this.HasOptional(x => x.LeadMain)
                .WithMany()
                .HasForeignKey(x => x.LeadId)
                .WillCascadeOnDelete(true);*/
        }

        #endregion Constructor
    }
}