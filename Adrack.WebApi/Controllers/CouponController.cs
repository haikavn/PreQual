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
using Adrack.Service.Click;
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
    [RoutePrefix("api/coupons")]
    [AllowAnonymous]
    public class CouponController : BaseApiController
    {


        private readonly IPlanService _planService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;
        private readonly ICouponService _couponService;
        private readonly IPermissionService _permissionService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAppContext _appContext;

        public CouponController(ICouponService couponService,
            IPlanService planService,
            IReportService reportService,
            IUserService userService,
            ICacheManager cacheManager,
            IPermissionService permissionService,
            IEncryptionService encryptionService,
            IAppContext appContext)
        {
            _planService = planService;
            _reportService = reportService;
            _userService = userService;
            _cacheManager = cacheManager;
            _permissionService = permissionService;
            _encryptionService = encryptionService;
            _appContext = appContext;
            _couponService = couponService;
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

        [HttpPost]
        [Route("addPlan")]
        public IHttpActionResult AddPlan(AdrackManagementPlan plan)
        {
            if (plan == null)
            {
                return HttpBadRequest("Plan model is empty");
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
        [Route("addCoupon")]
        public IHttpActionResult AddCoupon([FromBody] Coupon coupon)
        {
            try
            {
                if (coupon == null)
                {
                    return HttpBadRequest("Coupon model is empty");
                }
                coupon.CreatedUtc = DateTime.UtcNow;
                coupon.CouponExpression = _encryptionService.CreatePasswordHash(DateTime.Now.ToString(), DateTime.Now.ToString()).Substring(0, 8);
                var newCoupon = _couponService.InsertCoupon(coupon);
                return Ok(newCoupon);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateCoupon")]
        public IHttpActionResult UpdateCoupon([FromBody] Coupon coupon)
        {
            try
            {
                if (coupon == null)
                {
                    return HttpBadRequest("Coupon model is empty");
                }
                var existingCoupon = _couponService.GetCouponById(coupon.Id);
                if (existingCoupon == null)
                {
                    return HttpBadRequest("This coupon doesn't exist");
                }

                var existingUser = _userService.GetUserById(coupon.UserId.Value);
                if (existingUser == null)
                {
                    return HttpBadRequest("Provided user doesn't exist");
                }
                existingCoupon.UserId = coupon.UserId;

                _couponService.UpdateCoupon(existingCoupon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("useCoupon")]
        public IHttpActionResult UseCoupon([FromBody] string couponExpression)
        {
            try
            {
                var userCoupons = _couponService.GetCouponsByUserId(_appContext.AppUser.Id);
                var providedCoupon = userCoupons.FirstOrDefault(c => c.CouponExpression == couponExpression && !c.UsedBySystemUtc.HasValue
                && !c.AppliedByUserUtc.HasValue);

                if (string.IsNullOrWhiteSpace(couponExpression) || providedCoupon==null)
                {
                    return HttpBadRequest("Invalid coupon expression");
                }

                providedCoupon.AppliedByUserUtc = DateTime.UtcNow;

                _couponService.UpdateCoupon(providedCoupon);
                return Ok();
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