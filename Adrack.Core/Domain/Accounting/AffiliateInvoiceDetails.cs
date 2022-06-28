// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateInvoiceDetails.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Accounting
{
    /// <summary>
    /// Class AffiliateInvoiceDetails.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class AffiliateInvoiceDetails : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aInvoice = new AffiliateInvoiceDetails()
            {
            };

            return aInvoice;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The affiliate price.</value>
        public decimal AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the affiliate leads count.
        /// </summary>
        /// <value>The affiliate leads count.</value>
        public int AffiliateLeadsCount { get; set; }

        /// <summary>
        /// Gets or sets the affiliate sum.
        /// </summary>
        /// <value>The affiliate sum.</value>
        public decimal AffiliateSum { get; set; }

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