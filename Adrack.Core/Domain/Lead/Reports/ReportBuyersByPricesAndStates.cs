// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 24-02-2021
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 24-02-2021
// ***********************************************************************
// <copyright file="ReportBuyersByPricesAndStates.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportBuyersByPricesAndStates.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportBuyersByPricesAndStates : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the Leads.
        /// </summary>
        /// <value>Leads.</value>
        public int Leads { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        /// <value>State.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the BuyerPrice.
        /// </summary>
        /// <value>State.</value>
        public decimal BuyerPrice { get; set; }


        #endregion Properties
    }
}