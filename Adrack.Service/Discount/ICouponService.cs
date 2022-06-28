// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ICountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Click
{
    /// <summary>
    /// Represents a Country Service
    /// </summary>
    public partial interface ICouponService
    {
        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        Coupon GetCouponById(long Id);


        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<Coupon> GetAllCoupons();

        /// <summary>
        /// Gets the Coupons by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>IList&lt;Coupon&gt;.</returns>
        IList<Coupon> GetCouponsByUserId(long userId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="Coupon">The Coupon.</param>
        /// <returns>System.Int64.</returns>
        long InsertCoupon(Coupon Coupon);



        /// <summary>
        /// Update Coupon set
        /// </summary>
        /// <param name="Coupon">The Coupon.</param>
        void UpdateCoupon(Coupon Coupon);



        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="Coupon">The Coupon.</param>
        void DeleteCoupon(Coupon Coupon);


    }
}
