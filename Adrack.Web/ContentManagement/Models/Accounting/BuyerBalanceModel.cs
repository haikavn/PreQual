// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BuyerBalanceModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Accounting;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Accounting
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class BuyerBalanceModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyerBalanceModel"/> class.
        /// </summary>
        public BuyerBalanceModel()
        {
            this.TotalSoldSum = 0;
            this.TotalPaymentSum = 0;
            this.TotalBalance = 0;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the buyerbalance list.
        /// </summary>
        /// <value>The buyerbalance list.</value>
        public IList<BuyerBalanceView> buyerbalanceList { get; set; }

        /// <summary>
        /// Gets or sets the total sold sum.
        /// </summary>
        /// <value>The total sold sum.</value>
        public decimal TotalSoldSum { get; set; }

        /// <summary>
        /// Gets or sets the total payment sum.
        /// </summary>
        /// <value>The total payment sum.</value>
        public decimal TotalPaymentSum { get; set; }

        /// <summary>
        /// Gets or sets the total balance.
        /// </summary>
        /// <value>The total balance.</value>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// Gets or sets the total invoiced sum.
        /// </summary>
        /// <value>The total invoiced sum.</value>
        public decimal TotalInvoicedSum { get; set; }

        #endregion Properties
    }
}