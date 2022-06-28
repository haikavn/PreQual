// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="RefundedLeadModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System;

namespace Adrack.Web.ContentManagement.Models.Accounting
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class RefundedLeadModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// RefundedLeads
        /// </summary>
        /// <value>The identifier.</value>

        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets a price.
        /// </summary>
        /// <value>a price.</value>
        public float APrice { get; set; }

        /// <summary>
        /// Gets or sets the b price.
        /// </summary>
        /// <value>The b price.</value>
        public float BPrice { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets a invoice identifier.
        /// </summary>
        /// <value>a invoice identifier.</value>
        public long AInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the b invoice identifier.
        /// </summary>
        /// <value>The b invoice identifier.</value>
        public long BInvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the review note.
        /// </summary>
        /// <value>The review note.</value>
        public string ReviewNote { get; set; }

        /// <summary>
        /// Gets or sets the approved.
        /// </summary>
        /// <value>The approved.</value>
        public byte Approved { get; set; }

        #endregion Properties
    }
}