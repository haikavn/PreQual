// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="GenerateInvoiceModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Accounting
{
    /// <summary>
    /// Class GenerateInvoiceModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class GenerateInvoiceModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The affiliate list.</value>
        public IList<Affiliate> affiliateList { get; set; }

        /// <summary>
        /// Gets or sets the buyer list.
        /// </summary>
        /// <value>The buyer list.</value>
        public IList<Buyer> buyerList { get; set; }

        /// <summary>
        /// Affiliates Invoices
        /// </summary>
        /// <value>The invoices.</value>
        public List<AffiliateInvoice> invoices { get; set; }

        #endregion Properties

    }
}