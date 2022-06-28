// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-09-2021
//
// Last Modified By : Grigori D.
// Last Modified On : 03-09-2021
// ***********************************************************************
// <copyright file="Planservice.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Adrack.PlanManagement;
using Newtonsoft.Json;
using Adrack.Service.Lead;
using Adrack.Core;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Membership.IPlanService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IPlanService" />
    public partial class PlanService : IPlanService
    {
        #region Constants
        private const string CACHE_PLAN_GetAllPlans = "App.Cache.Plans.GetAllPlans-{0}";
        private const string CACHE_PLAN_PATTERN_KEY = "App.Cache.Plans.";

        private const string CACHE_USERPLANS_PATTERN_KEY = "App.Cache.Plans-{0}";


        private const string CACHE_FEATURE_GetAllFeatures = "App.Cache.Features.GetAllFeatures";
        private const string CACHE_FEATURE_PATTERN_KEY = "App.Cache.Features.";

        private const string CACHE_PLANFEATURES_PATTERN_KEY = "App.Cache.Features-{0}";
        #endregion Constants

        #region Fields

        /// <summary>
        /// Plan
        /// </summary>
        private readonly IRepository<PlanModel> _planRepository;

        /// <summary>
        /// Feature
        /// </summary>
        private readonly IRepository<Feature> _featureRepository;


        /// <summary>
        /// PlanFeature
        /// </summary>
        private readonly IRepository<PlanFeature> _planFeatureRepository;

        /// <summary>
        /// UserPlan
        /// </summary>
        private readonly IRepository<UserPlan> _userPlanRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// IDataProvider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// ISettingService
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// IReportService
        /// </summary>
        private readonly IReportService _reportService;


        /// <summary>
        ///     User Service
        /// </summary>
        private readonly IUserService _userService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="planRepository">The plan main repository.</param>
        /// <param name="featureRepository">The feature main repository.</param>
        /// <param name="planFeatureRepository">The planFeature main repository.</param>
        /// <param name="userPlanRepository">The user Plans repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="userService">User Service</param>
        public PlanService(
                                IRepository<PlanModel> planRepository,
                                IRepository<Feature> featureRepository,
                                IRepository<PlanFeature> planFeatureRepository,
                                IRepository<UserPlan> userPlanRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher,
                                IDataProvider dataProvider,
                                ISettingService settingService,
                                IReportService reportService,
                                IUserService userService
                                )
        {
            this._planRepository = planRepository;            
            this._featureRepository = featureRepository;            
            this._planFeatureRepository = planFeatureRepository;            
            this._userPlanRepository = userPlanRepository;            
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._settingService = settingService;
            this._reportService = reportService;
            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Plans
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Plans Collection Item</returns>
        public virtual IList<PlanModel> GetAllPlans(short deleted = 0)
        {
            string key = string.Format(CACHE_PLAN_GetAllPlans, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _planRepository.Table
                    where
                        (deleted == -1 || 
                         (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) ||
                         (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }

        /// <summary>
        /// Get Plan By Id
        /// </summary>
        /// <param name="id">Plan Id</param>
        /// <returns>Plan Item</returns>
        public virtual PlanModel GetPlanByID(long id)
        {
            var query = from x in _planRepository.Table
                where x.Id == id
                select x;

            var plan = query.FirstOrDefault();

            return plan;
        }

        /// <summary>
        /// Insert Plan
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">plan</exception>
        public virtual long InsertPlan(PlanModel plan)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            _planRepository.Insert(plan);

            _cacheManager.RemoveByPattern(CACHE_PLAN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(plan);

            return plan.Id;
        }


        /// <summary>
        /// Update Plan
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void UpdatePlan(PlanModel plan)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            _planRepository.Update(plan);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_PLAN_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(plan);
        }


        /// <summary>
        /// Delete Plan
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void DeletePlan(PlanModel plan)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            plan.Deleted = true;
            _planRepository.Update(plan);

            _cacheManager.RemoveByPattern(CACHE_PLAN_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(plan);
        }
        #endregion Methods
        
        #region UserPlan Methods

        /// <summary>
        /// GetPlans By UserId
        /// </summary>
        /// <returns>UserPlan Collection Item</returns>
        public virtual IList<UserPlan> GetPlansByUserId(long id)
        {
            var query = from x in _userPlanRepository.Table
                join a in _planRepository.Table on x.UserId equals a.Id
                where x.UserId == id && ((a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue)
                orderby x.Id descending
                select x;

            return query.ToList();
        }


        /// <summary>
        /// GetPlan By UserId
        /// </summary>
        /// <returns>UserPlan Item</returns>
        public virtual UserPlan GetPlanByUserId(long id)
        {
            var query = from x in _userPlanRepository.Table
                join a in _planRepository.Table on x.UserId equals a.Id
                where x.UserId == id && ((a.Deleted.HasValue && !a.Deleted.Value) || !a.Deleted.HasValue)
                orderby x.Id descending
                select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get UserPlan
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="planId">Plan Id</param>
        /// <returns>UserPlan Item</returns>
        public virtual UserPlan GetUserPlan(long userId, long planId)
        {
            var query = from x in _userPlanRepository.Table
                where x.UserId == userId && x.PlanId == planId
                orderby x.Id descending
                select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Insert User Plan
        /// </summary>
        /// <param name="userPlan">The user's plan.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        public virtual long InsertUserPlan(UserPlan userPlan)
        {
            if (userPlan == null)
                throw new ArgumentNullException("userPlan");

            _userPlanRepository.Insert(userPlan);

            _cacheManager.RemoveByPattern(CACHE_USERPLANS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userPlan);

            return userPlan.Id;
        }


        /// <summary>
        /// Update UserPlan
        /// </summary>
        /// <param name="userPlan">The userPlan.</param>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        public virtual void UpdateUserPlan(UserPlan userPlan)
        {
            if (userPlan == null)
                throw new ArgumentNullException("userPlan");

            _userPlanRepository.Update(userPlan);

            _cacheManager.RemoveByPattern(CACHE_USERPLANS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userPlan);
        }


        /// <summary>
        /// Delete User Plan
        /// </summary>
        /// <param name="userPlan">The user's Plan.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">userPlan</exception>
        public virtual void DeleteUserPlan(UserPlan userPlan)
        {
            if (userPlan == null)
                throw new ArgumentNullException("userPlan");

            _userPlanRepository.Delete(userPlan);

            _cacheManager.RemoveByPattern(CACHE_USERPLANS_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userPlan);
        }

        #endregion Methods



        #region Feature Methods

        /// <summary>
        /// Get All Features
        /// </summary>
        /// <returns>Features Collection Item</returns>
        public virtual IList<Feature> GetAllFeatures()
        {
            string key = string.Format(CACHE_FEATURE_GetAllFeatures);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _featureRepository.Table
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
        }

        /// <summary>
        /// Get Feature By Id
        /// </summary>
        /// <param name="id">Feature Id</param>
        /// <returns>Feature Item</returns>
        public virtual Feature GetFeatureByID(long id)
        {
            var query = from x in _featureRepository.Table
                where x.Id == id
                select x;

            var feature = query.FirstOrDefault();

            return feature;
        }

        /// <summary>
        /// Insert Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">feature</exception>
        public virtual long InsertFeature(Feature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            _featureRepository.Insert(feature);

            _cacheManager.RemoveByPattern(CACHE_FEATURE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(feature);

            return feature.Id;
        }


        /// <summary>
        /// Update Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void UpdateFeature(Feature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            _featureRepository.Update(feature);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_FEATURE_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(feature);
        }


        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <exception cref="ArgumentNullException">Feature</exception>
        public virtual void DeleteFeature(Feature feature)
        {
            if (feature == null)
                throw new ArgumentNullException("feature");

            _featureRepository.Delete(feature);

            _cacheManager.RemoveByPattern(CACHE_FEATURE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(feature);
        }


        #endregion Feature Methods

        #region PlanFeature Methods

        /// <summary>
        /// Get Features By PlanId
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <returns>Feature Collection Item</returns>
        public virtual IList<Feature> GetFeaturesByPlanId(long planId)
        {
            var query = from x in _featureRepository.Table
                        join a in _planFeatureRepository.Table on x.Id equals a.FeatureId
                        where a.PlanId == planId
                        orderby x.Id descending
                        select x;

            return query.ToList();
        }



        /// <summary>
        /// Get PlanFeature
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="featureId">Feature Id</param>
        /// <returns>UserPlan Item</returns>
        public virtual PlanFeature GetPlanFeature(long planId, long featureId)
        {
            var query = from x in _planFeatureRepository.Table
                        where x.PlanId == planId && x.FeatureId == featureId
                        orderby x.Id descending
                        select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Insert Plan Feature
        /// </summary>
        /// <param name="planFeature">The plan's feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">planFeature</exception>
        public virtual long InsertPlanFeature(PlanFeature planFeature)
        {
            if (planFeature == null)
                throw new ArgumentNullException("planFeature");

            _planFeatureRepository.Insert(planFeature);

            _cacheManager.RemoveByPattern(CACHE_PLANFEATURES_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(planFeature);

            return planFeature.Id;
        }



        /// <summary>
        /// Delete Plan Feature
        /// </summary>
        /// <param name="planFeature">The plan's feature.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">planFeature</exception>
        public virtual void DeletePlanFeature(PlanFeature planFeature)
        {
            if (planFeature == null)
                throw new ArgumentNullException("planFeature");

            _planFeatureRepository.Delete(planFeature);

            _cacheManager.RemoveByPattern(CACHE_PLANFEATURES_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(planFeature);
        }

        #endregion Methods


        #region Plan Verification function

        /// <summary>
        /// Check Plan Statuses By UserId
        /// </summary>
        /// <returns>AdrackPlanVerificationStatus Collection Item</returns>
        public IList<AdrackPlanVerificationStatus>  CheckPlanStatusesByUserId(long userId)
        {
            IList<AdrackPlanVerificationStatus> statuses =  new List<AdrackPlanVerificationStatus>();
           
            var user = _userService.GetUserById(userId);
            if (user.UserType == UserTypes.Super)
            {
                return null;
            }

            UserPlan userPlan = this.GetPlanByUserId(userId);
            if (userPlan == null)
            {
                return null;
                //throw new ArgumentException($"This User have not plan.");
            }

            PlanModel plan = GetPlanByID(userPlan.PlanId);
            if (plan == null)
            {
                return null;
                //throw new ArgumentNullException($"checkPlanVerification-Plan");
            }

            AdrackManagementPlan adrackPlan = JsonConvert.DeserializeObject<AdrackManagementPlan>(plan.Object);
            if (adrackPlan == null)
            {
                return null;
                //throw new ArgumentNullException($"checkPlanVerification-AdrackPlan");
            }

            var remainingEntity = this._reportService.GetRemainingEntities();
            var remainingPeriod = this._reportService.GetRemainingsByPeriod(adrackPlan.GetPeriodStart(), adrackPlan.GetPeriodEnd());


            if (adrackPlan.UserLimits > remainingEntity.TotalUsers) 
                statuses.Add(AdrackPlanVerificationStatus.UsersLimitReached);

            if (adrackPlan.AffiliateChannelLimit > remainingEntity.TotalAffiliateChannels) 
                statuses.Add(AdrackPlanVerificationStatus.AffiliateChannelLimitReached);

            if (adrackPlan.CampaignLimit > remainingEntity.TotalCampaigns) 
                statuses.Add(AdrackPlanVerificationStatus.CampaignLimitReached);

            if (adrackPlan.BuyerChannelLimit > remainingEntity.TotalBuyerChannels) 
                statuses.Add(AdrackPlanVerificationStatus.BuyerChannelLimitReached);

            if (adrackPlan.PingLimits > remainingPeriod.TotalPings) 
                statuses.Add(AdrackPlanVerificationStatus.PingLimitsReached);

            if (adrackPlan.LeadCount > remainingPeriod.TotalLeads) 
                statuses.Add(AdrackPlanVerificationStatus.LeadCountReached);
            

            return statuses;
        }



        #endregion
    }



}