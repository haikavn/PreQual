// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 01-27-2021
//
// Last Modified By : Grigori
// Last Modified On : 01-27-2021
// ***********************************************************************
// <copyright file="ReportsViewedMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.CustomReports;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead.Reports;

namespace Adrack.Data.Domain.Lead.Reports
{
    /// <summary>
    /// Class AffiliateInvoiceMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Reports.ReportsViewed}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Reports.ReportsViewed}" />
    public partial class ReportsViewedMap : AppEntityTypeConfiguration<ReportsViewed>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public ReportsViewedMap()
        {
            this.ToTable("ReportsViewed");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}