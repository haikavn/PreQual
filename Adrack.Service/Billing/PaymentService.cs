// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 05-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-11-2020
// ***********************************************************************
// <copyright file="BillingService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Billing;
using Adrack.Core.Infrastructure.Data;

using Adrack.Data;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Payment;
using System.Web.Helpers;
using Newtonsoft.Json;
using Adrack.PlanManagement;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Accounting.IAccountingService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Accounting.IAccountingService" />
    public partial class PaymentService : IPaymentService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.Accounting.By.Id-{0}";

        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.Accounting.";

        #endregion Constants

        #region Fields

        //Arman Potential Bug
        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IRepository<Payment> _paymentRepository;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// The history repository
        /// </summary>
        private readonly IHistoryService _historyRepository;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        private readonly IRepository<User> _userRepository;


        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly ILogService _logService;

        private readonly IRepository<UserPlan> _userPlanRepository;

        private readonly IRepository<PlanModel> _planRepository;
        private readonly IRepository<Coupon> _couponRepository;
        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="historyRepository">The history repository.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="logService">Error logging service.</param>
        public PaymentService(
                                    IAppContext appContext,
                                    ICacheManager cacheManager,
                                    IAppEventPublisher appEventPublisher,
                                    IDataProvider dataProvider,
                                    IRepository<Payment> paymentRepository,
                                    IHistoryService historyRepository,
                                    ISettingService settingService,
                                    IRepository<User> userRepository,
                                    ILogService logService,
                                    IRepository<UserPlan> userPlanRepository,
                                    IRepository<PlanModel> planRepository,
                                    IRepository<Coupon> couponRepository)
        {
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._appContext = appContext;
            this._paymentRepository = paymentRepository;
            this._dataProvider = dataProvider;
            this._historyRepository = historyRepository;
            this._userRepository = userRepository;
            this._settingService = settingService;
            this._logService = logService;
            this._userPlanRepository = userPlanRepository;
            this._planRepository = planRepository;
            this._couponRepository = couponRepository;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Invoices of Buyers
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Payment> GetAllPayments(DateTime? dateFrom, DateTime? dateTo)
        {

            var query = from x in _paymentRepository.Table
                        orderby x.Id descending
                        select x;

            if (dateFrom != null)
            {
                query.Where("PaymentDate >= dateFrom && PaymentDate <= dateTo");
            }

            var bPayment = query.ToList();

            return bPayment;
        }

        public virtual IList<Payment> GetPaymentsByUser(long userId)
        {

            var query = from x in _paymentRepository.Table
                        where x.UserId == userId
                        orderby x.Id descending
                        select x;

            var bPayment = query.ToList();

            return bPayment;
        }


        public virtual Payment GetPaymentById(long id)
        {
            var query = from x in _paymentRepository.Table
                        where x.Id == id
                        select x;

            var bPayment = query.FirstOrDefault();

            return bPayment;
        }

        public virtual string InitPayment()
        {
            string client_token = "";


            return client_token;
        }

        

        public virtual bool DoPayment(Payment payment)
        {
            _paymentRepository.Insert(payment);

            return true;
        }

        public string GetPayPalSubscribtionId(long userId)
        {
            var query = from x in _paymentRepository.Table
                        where x.UserId == userId
                        orderby x.Id descending
                        select x.PayPalSubscriptionId;

            var payPalSubscriptionId = query.FirstOrDefault();

            return payPalSubscriptionId;
        }

        public string GetPayPalCustomerId(long userId)
        {
            var query = from x in _paymentRepository.Table
                        where x.UserId == userId
                        orderby x.Id descending
                        select x.PayPalCustomerId;

            return query.FirstOrDefault();
        }

        public void CancelSubscription(string payPalSubscribtionId)
        {
            var query = from x in _paymentRepository.Table
                        where x.PayPalSubscriptionId == payPalSubscribtionId
                        orderby x.Id descending
                        select x;

            Payment payment = query.FirstOrDefault();
            payment.Status = -1;

            _paymentRepository.Update(payment);
        }

        public IList<PaymentPastDays> GetPastDuePayments()
        {
            var result = new List<PaymentPastDays>();
            var query = from x in _paymentRepository.Table
                        join u in _userRepository.Table on x.UserId equals u.Id
                        where DateTime.UtcNow > x.ExpireDate && string.IsNullOrEmpty(x.TransactionId)
                        select new { x, u };

            foreach (var payment in query)
            {
                var days = DateTime.Now - payment.x.ExpireDate;
                if (days.Days == 2 || days.Days == 1 || days.Days == 5)
                    result.Add(new PaymentPastDays() {
                        PastDaysCount = days.Days,
                        Payment = payment.x,
                        User = payment.u
                    });
            }

            return result;

        }

        public double CalculatePayment(long userId, string coupon)
        {
            double result = 0;
            var userPlan = (from up in _userPlanRepository.Table
                            where up.UserId == userId
                            select up).FirstOrDefault();
            if (userPlan != null)
            {
                var plans = _planRepository.Table.ToList();
                if (plans != null && plans.Any())
                {
                    foreach (var item in plans)
                    {
                        var itemDeserialized = JsonConvert.DeserializeObject<AdrackManagementPlan>(item.Object);
                        if (userPlan.Id == item.Id && itemDeserialized.IsActive)
                        {
                            result = itemDeserialized.PricePlan.Price;

                            if (itemDeserialized.PricePlan.DiscountInPercent.HasValue)
                            {
                                result = result - ((result / 100) * (double)(itemDeserialized.PricePlan.DiscountInPercent));
                            }
                            else if (itemDeserialized.PricePlan.DiscountInPrice.HasValue)
                            {
                                result = result - (double)itemDeserialized.PricePlan.DiscountInPrice;
                            }

                            if (itemDeserialized.IndependentDiscountInPercent.HasValue)
                            {
                                result = result - ((result / 100) * (double)(itemDeserialized.IndependentDiscountInPercent));
                            }
                            else if (itemDeserialized.IndependentDiscountInPrice.HasValue)
                            {
                                result = result - (double)itemDeserialized.IndependentDiscountInPrice;
                            }



                            if (!string.IsNullOrWhiteSpace(coupon))
                            {
                                var existingCoupon = _couponRepository.Table
                                                        .Where(c => c.CouponExpression == coupon
                                                                   && !c.UsedBySystemUtc.HasValue).FirstOrDefault();
                                if (existingCoupon != null)
                                {
                                    if (existingCoupon.IsPercent)
                                        result = result - ((result / 100) * (double)existingCoupon.DiscountAmount);
                                    else
                                    if (!existingCoupon.IsPercent)
                                        result = result - (double)existingCoupon.DiscountAmount;
                                }
                               
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void DisposeCoupon(string coupon)
        {

            var existingCoupon = _couponRepository.Table
                                    .Where(c => c.CouponExpression == coupon
                                               && !c.UsedBySystemUtc.HasValue).FirstOrDefault();

            existingCoupon.AppliedByUserUtc = DateTime.UtcNow;

            _couponRepository.Update(existingCoupon);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(existingCoupon);
        }

        #endregion Methods
    }
}