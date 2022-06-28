// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SupportTicketsMessagesMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class SupportTicketsMessagesMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.SupportTicketsMessages}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.SupportTicketsMessages}" />
    public partial class SupportTicketsMessagesMap : AppEntityTypeConfiguration<SupportTicketsMessages>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public SupportTicketsMessagesMap()
        {
            this.ToTable("SupportTicketsMessages");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}