﻿// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class PingTreeItemMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PingTreeItem}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.PingTreeItem}" />
    public partial class PingTreeItemMap : AppEntityTypeConfiguration<PingTreeItem>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PingTreeItemMap()
        {
            this.ToTable("PingTreeItem");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}