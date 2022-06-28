// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerInvoiceDetails.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class BuyerInvoiceDetails.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerInvoiceDetails : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var bInvoice = new BuyerInvoiceDetails()
            {
            };

            return bInvoice;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer leads count.
        /// </summary>
        /// <value>The buyer leads count.</value>
        public int BuyerLeadsCount { get; set; }

        /// <summary>
        /// Gets or sets the buyer sum.
        /// </summary>
        /// <value>The buyer sum.</value>
        public decimal BuyerSum { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        /// <value>The name of the campaign.</value>
        public string CampaignName { get; set; }

        #endregion Properties
    }
}