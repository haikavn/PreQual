// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BuyerPaymentModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Accounting
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class BuyerPaymentModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// AffiliatePayments
        /// </summary>
        /// <value>The identifier.</value>
//        public List<AffiliatePayment> AffiliatePayments { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>The payment date.</value>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the buyers list.
        /// </summary>
        /// <value>The buyers list.</value>
        public IList<Buyer> BuyersList { get; set; }

        #endregion Properties
    }
}