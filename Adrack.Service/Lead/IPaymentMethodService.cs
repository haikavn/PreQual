// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IPaymentMethodService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IPaymentMethodService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier.</param>
        /// <returns>Profile Item</returns>
        PaymentMethod GetPaymentMethodById(long paymentMethodId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<PaymentMethod> GetAllPaymentMethods();

        /// <summary>
        /// Gets all payment methods by affiliate identifier.
        /// </summary>
        /// <param name="affiliateid">The affiliateid.</param>
        /// <returns>IList&lt;PaymentMethod&gt;.</returns>
        IList<PaymentMethod> GetAllPaymentMethodsByAffiliateId(long affiliateid);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="paymentMethod">The payment method.</param>
        /// <returns>System.Int64.</returns>
        long InsertPaymentMethod(PaymentMethod paymentMethod);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <returns>System.Int64.</returns>
        long SetDefaultPaymentMethod(long Id, long AffiliateId);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="paymentMethod">The payment method.</param>
        void UpdatePaymentMethod(PaymentMethod paymentMethod);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="Id">The identifier.</param>
        void DeletePaymentMethod(long Id);

        #endregion Methods
    }
}