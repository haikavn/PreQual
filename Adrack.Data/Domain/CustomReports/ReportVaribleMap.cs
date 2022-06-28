﻿// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateInvoiceMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.CustomReports;
using Adrack.Core.Domain.Accounting;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateInvoiceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.CustomReports.ReportVariable}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.CustomReports.ReportVariable}" />
    public partial class ReportVariableMap : AppEntityTypeConfiguration<Adrack.Core.Domain.CustomReports.ReportVariable>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public ReportVariableMap()
        {
            this.ToTable("ReportVariable");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}