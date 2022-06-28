using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.PlanManagement;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/plans")]
    [AllowAnonymous]
    public class PlanController : BaseApiController
    {

        
        private readonly IPlanService _planService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;

        private readonly IPermissionService _permissionService;
        

        public PlanController(IPlanService planService,
            IReportService reportService,
            IUserService userService,
            ICacheManager cacheManager,
            IPermissionService permissionService)
        {
            _planService = planService;
            _reportService = reportService;            
            _userService = userService;
            _cacheManager = cacheManager;
            _permissionService = permissionService;
        }


        #region Methods

        /// <summary>
        /// Get Plan List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getPlanList")]
        public IHttpActionResult GetPlanList()
        {
            
            var plans = _planService.GetAllPlans();
            return Ok(plans);
        }


        /*
        [HttpGet]
        [Route("addPlanGetTest")]
        public IHttpActionResult AddPlanGetTest()
        {
            AdrackManagementPlan plan = new AdrackManagementPlan();
            plan.InitDefault();
            return AddPlan(plan);
        }
        */



        /*
        [HttpPost]
        [Route("addPlan")]
        public IHttpActionResult AddPlan(AdrackManagementPlan plan)
        {
            if (plan == null)
            {
                return HttpBadRequest("Plan model is empty");
            }
            if (plan.IndependentDiscountInPercent.HasValue && plan.IndependentDiscountInPrice.HasValue)
            {
                return HttpBadRequest("Only one additional discount can be provided");
            }

            try
            {                
                PlanModel model = new PlanModel();
                model.Name = plan.Name;
                model.Object = JsonConvert.SerializeObject(plan);
                _planService.InsertPlan(model);
                
                return Ok(0);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("updatePlan")]
        public IHttpActionResult UpdatePlan(AdrackManagementPlan plan)
        {
            
            if (plan == null)
                return HttpBadRequest("plan model is empty");

            if (plan.IndependentDiscountInPercent.HasValue && plan.IndependentDiscountInPrice.HasValue)
            {
                return HttpBadRequest("Only one additional discount can be provided");
            }
            if (plan.PricePlan != null && plan.PricePlan.DiscountInPercent.HasValue && plan.PricePlan.DiscountInPrice.HasValue)
            {
                return HttpBadRequest("Only one subscribtion plan discount can be provided");
            }

            var currentPlan = _planService.GetPlanByID(plan.Id);
            if (currentPlan == null)
                return HttpBadRequest("plan not found");

            try
            {

                currentPlan.Name = plan.Name;
                currentPlan.Object = JsonConvert.SerializeObject(plan);
                _planService.UpdatePlan(currentPlan);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        */

        [HttpPost]
        [Route("addPlan")]
        public IHttpActionResult AddPlan(PlanModel plan)
        {
            if (plan == null)
            {
                return HttpBadRequest("Plan model is empty");
            }

            try
            {
                PlanModel model = new PlanModel();
                model.Name = plan.Name;
                model.Amount = plan.Amount;
                _planService.InsertPlan(model);

                return Ok(0);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("updatePlan")]
        public IHttpActionResult UpdatePlan(PlanModel plan)
        {

            if (plan == null)
                return HttpBadRequest("plan model is empty");

            var currentPlan = _planService.GetPlanByID(plan.Id);
            if (currentPlan == null)
                return HttpBadRequest("plan not found");

            try
            {

                currentPlan.Name = plan.Name;
                currentPlan.Amount = plan.Amount;
                currentPlan.Deleted = plan.Deleted;

                _planService.UpdatePlan(currentPlan);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deletePlan")]
        public IHttpActionResult DeletePlan(long planId)
        {
            var currentPlan = _planService.GetPlanByID(planId);
            if (currentPlan == null)
                return HttpBadRequest("Plan not found");

            try
            {
                _planService.DeletePlan(currentPlan);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        #endregion

        #region UserPlan Methods

        [HttpPost]
        [Route("addUserPlan")]
        public IHttpActionResult AddUserPlan([FromBody] UserPlan userPlan)
        {
            //if (!_permissionService.Authorize(_viewPlanGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (userPlan == null)
            {
                return HttpBadRequest("UserPlan model is empty");
            }

            var plan = _planService.GetPlanByID(userPlan.PlanId);
            if (plan == null)
            {
                return HttpBadRequest("This Plan not found.");
            }

            
            var currentUserPlan = _planService.GetUserPlan(userPlan.UserId, userPlan.PlanId);
            if (currentUserPlan != null)
            {
                return HttpBadRequest("This User's Plan already exists");
            }
            

            try
            {
                var id = _planService.InsertUserPlan(userPlan);
                return Ok(id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deleteUserPlan")]
        public IHttpActionResult DeleteUserPlan([FromBody] UserPlan userPlan)
        {
            //if (!_permissionService.Authorize(_viewPlanGeneralInfoKey))
            //   return HttpBadRequest("access-denied");

            if (userPlan == null)
            {
                return HttpBadRequest("UserPlan model is empty");
            }

            
            var currentUserPlan = _planService.GetUserPlan(userPlan.UserId, userPlan.PlanId);
            if (currentUserPlan == null)
            {
                return HttpBadRequest("This User's Plan not found.");
            }
            

            try
            {
                _planService.DeleteUserPlan(currentUserPlan);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        #endregion



        #region Feature Methods

        /// <summary>
        /// Get Feature List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getFeatureList")]
        public IHttpActionResult GetFeatureList()
        {

            var features = _planService.GetAllFeatures();
            return Ok(features);
        }


        [HttpGet]
        [Route("getFeaturesByPlanId")]
        public IHttpActionResult GetFeaturesByPlanI(long planId)
        {

            var features = _planService.GetFeaturesByPlanId(planId);
            return Ok(features);
        }

        [HttpGet]
        [Route("getFeatureById")]
        public IHttpActionResult GetFeatureById(long id)
        {
            var feature = _planService.GetFeatureByID(id);
            return Ok(feature);
        }


        [HttpPost]
        [Route("addFeature")]
        public IHttpActionResult AddFeature(Feature feature)
        {
            if (feature == null)
            {
                return HttpBadRequest("Feature model is empty");
            }

            try
            {
                var featureId = _planService.InsertFeature(feature);

                return Ok(featureId);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("updateFeature")]
        public IHttpActionResult UpdateFeature(Feature feature)
        {

            if (feature == null)
                return HttpBadRequest("Feature model is empty");

            var currentFeature = _planService.GetFeatureByID(feature.Id);
            if (currentFeature == null)
                return HttpBadRequest("Feature not found");

            try
            {

                currentFeature.Name = feature.Name;
                currentFeature.Value = feature.Value;
                currentFeature.TypeValue = feature.TypeValue;

                _planService.UpdateFeature(currentFeature);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deleteFeature")]
        public IHttpActionResult DeleteFeature(long featureId)
        {
            var currentFeature = _planService.GetFeatureByID(featureId);
            if (currentFeature == null)
                return HttpBadRequest("Feature not found");

            try
            {
                _planService.DeleteFeature(currentFeature);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        #endregion


        #region PlanFeature Methods

        [HttpPost]
        [Route("addPlanFeature")]
        public IHttpActionResult AddPlanFeature([FromBody] PlanFeature planFeature)
        {
            //if (!_permissionService.Authorize(_viewPlanGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (planFeature == null)
            {
                return HttpBadRequest("PlanFeature model is empty");
            }

            var plan = _planService.GetPlanByID(planFeature.PlanId);
            if (plan == null)
            {
                return HttpBadRequest("This Plan not found.");
            }

            var feature = _planService.GetFeatureByID(planFeature.FeatureId);
            if (feature == null)
            {
                return HttpBadRequest("This Feature not found.");
            }


            var currentPlanFeature = _planService.GetPlanFeature(planFeature.PlanId, planFeature.FeatureId);
            if (currentPlanFeature != null)
            {
                return HttpBadRequest("This Plan's Feature already exists");
            }


            try
            {
                var id = _planService.InsertPlanFeature(planFeature);
                return Ok(id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deletePlanFeaturen")]
        public IHttpActionResult DeletePlanFeature([FromBody] PlanFeature planFeature)
        {
            //if (!_permissionService.Authorize(_viewPlanGeneralInfoKey))
            //   return HttpBadRequest("access-denied");

            if (planFeature == null)
            {
                return HttpBadRequest("PlanFeature model is empty");
            }


            var currentPlanFeature = _planService.GetPlanFeature(planFeature.PlanId, planFeature.FeatureId);
            if (currentPlanFeature == null)
            {
                return HttpBadRequest("This Plan's Feature not found.");
            }


            try
            {
                _planService.DeletePlanFeature(currentPlanFeature);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        #endregion


        #region Plan Verification function

        [HttpGet]
        [Route("CheckPlanStatusesByUserId")]
        public IHttpActionResult CheckPlanStatusesByUserId(long userId)
        {
            IList<AdrackPlanVerificationStatus> statuses = _planService.CheckPlanStatusesByUserId(userId); ;
            return Ok(statuses);
        }
        

        /*
        /// <summary>
        /// Get User Plan List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getAllPlans")]
        public IHttpActionResult GetAllPlans()
        {
            
            var plans = _planService.GetAllUserPlans();
            List<AdrackManagementPlan> adrackPlans=new List<AdrackManagementPlan>();
            var remaingingEntity=this._reportService.GetRemainingEntities();
            
            foreach (var plan in plans)
            {
                AdrackManagementPlan adrackPlan= JsonConvert.DeserializeObject<AdrackManagementPlan>(plan.Object);
                var remaingingPeriod = this._reportService.GetRemainingsByPeriod(adrackPlan.GetPeriodStart(), adrackPlan.GetPeriodEnd());
                adrackPlan.GetCheckStatus(remaingingEntity, remaingingPeriod);
                adrackPlans.Add(adrackPlan);
            }

            return Ok(adrackPlans);
        }
        */

        #endregion

    }
}