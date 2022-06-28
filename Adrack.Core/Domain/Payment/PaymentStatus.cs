// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PaymentStatus.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Payment
{
    /// <summary>
    /// Represents a Payment Status Enumeration
    /// </summary>
    public enum PaymentStatus
    {
        #region Enumeration

        /// <summary>
        /// Pending
        /// </summary>
        Pending = 100,

        /// <summary>
        /// Authorized
        /// </summary>
        Authorized = 200,

        /// <summary>
        /// Paid
        /// </summary>
        Paid = 300,

        /// <summary>
        /// Partially Refunded
        /// </summary>
        PartiallyRefunded = 400,

        /// <summary>
        /// Refunded
        /// </summary>
        Refunded = 500,

        /// <summary>
        /// Voided
        /// </summary>
        Voided = 600

        #endregion Enumeration
    }
}