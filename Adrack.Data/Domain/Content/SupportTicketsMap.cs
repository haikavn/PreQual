// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SupportTicketsMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class SupportTicketsMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.SupportTickets}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.SupportTickets}" />
    public partial class SupportTicketsMap : AppEntityTypeConfiguration<SupportTickets>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public SupportTicketsMap()
        {
            this.ToTable("SupportTickets");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}