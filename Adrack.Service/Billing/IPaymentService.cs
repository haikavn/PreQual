// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 05-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-11-2020
// ***********************************************************************
// <copyright file="IPaymentService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Billing;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Payment;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Payment Service
    /// </summary>
    public partial interface IPaymentService
    {
        #region Methods

        /// <summary>
        /// GetAllPayments
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        IList<Payment> GetAllPayments(DateTime? dateFrom, DateTime? dateTo);

        IList<Payment> GetPaymentsByUser(long userId);
        
        Payment GetPaymentById(long id);

        string InitPayment();
        
        string GetPayPalSubscribtionId(long userId);

        string GetPayPalCustomerId(long userId);

        bool DoPayment(Payment payment);
        
        void CancelSubscription(string payPalSubscribtionId);
        IList<PaymentPastDays> GetPastDuePayments();

        double CalculatePayment(long userId, string coupon);

        void DisposeCoupon(string coupon);
        #endregion Methods
    }
}