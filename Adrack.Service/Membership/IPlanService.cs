// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-09-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-09-2021
// ***********************************************************************
// <copyright file="IAddonService.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.PlanManagement;
using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface IPlanService
    {
        #region Methods
        
        IList<PlanModel> GetAllPlans(short deleted = 0);

        PlanModel GetPlanByID(long id);


        long InsertPlan(PlanModel plan);

   
        void UpdatePlan(PlanModel plan);

        
        void DeletePlan(PlanModel plan);

        #endregion


        #region UserPlan Methods

        /// <summary>
        /// GetPlans By UserId
        /// </summary>
        /// <returns>UserPlan Collection Item</returns>
        IList<UserPlan> GetPlansByUserId(long id);


        /// <summary>
        /// GetPlan By UserId
        /// </summary>
        /// <returns>UserPlan Item</returns>
        UserPlan GetPlanByUserId(long id);

        /// <summary>
        /// Get UserPlan
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="planId">Plan Id</param>
        /// <returns>UserPlan Item</returns>
        UserPlan GetUserPlan(long userId, long planId);

        /// <summary>
        /// Insert User Plan
        /// </summary>
        /// <param name="userPlan">The user's plan.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        long InsertUserPlan(UserPlan userPlan);


        /// <summary>
        /// Update UserPlan
        /// </summary>
        /// <param name="userPlan">The userPlan.</param>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        void UpdateUserPlan(UserPlan userPlan);

        /// <summary>
        /// Delete User Plan
        /// </summary>
        /// <param name="userPlan">The user's Plan.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        void DeleteUserPlan(UserPlan userPlan);


        #endregion

        #region Feature Methods

        /// <summary>
        /// Get All Features
        /// </summary>
        /// <returns>Features Collection Item</returns>
        IList<Feature> GetAllFeatures();

        /// <summary>
        /// Get Feature By Id
        /// </summary>
        /// <param name="id">Feature Id</param>
        /// <returns>Feature Item</returns>
        Feature GetFeatureByID(long id);

        /// <summary>
        /// Insert Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">feature</exception>
        long InsertFeature(Feature feature);

        /// <summary>
        /// Update Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        void UpdateFeature(Feature feature);

        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <exception cref="ArgumentNullException">Feature</exception>
        void DeleteFeature(Feature feature);


        #endregion

        #region PlanFeature Methods

        /// <summary>
        /// Get Features By PlanId
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <returns>Feature Collection Item</returns>
        IList<Feature> GetFeaturesByPlanId(long planId);

        /// <summary>
        /// Get PlanFeature
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="featureId">Feature Id</param>
        /// <returns>UserPlan Item</returns>
        PlanFeature GetPlanFeature(long planId, long featureId);

        /// <summary>
        /// Insert Plan Feature
        /// </summary>
        /// <param name="planFeature">The plan's feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">planFeature</exception>
        long InsertPlanFeature(PlanFeature planFeature);


        /// <summary>
        /// Delete Plan Feature
        /// </summary>
        /// <param name="planFeature">The plan's feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">planFeature</exception>
        void DeletePlanFeature(PlanFeature planFeature);

        #endregion


        #region Plan Verification function

        /// <summary>
        /// Check Plan Statuses By UserId
        /// </summary>
        /// <returns>AdrackPlanVerificationStatus Collection Item</returns>
        IList<AdrackPlanVerificationStatus> CheckPlanStatusesByUserId(long userId);

        #endregion
    }
}