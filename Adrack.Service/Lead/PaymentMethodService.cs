// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PaymentMethodService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Lead.IPaymentMethodService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IPaymentMethodService" />
    public partial class PaymentMethodService : IPaymentMethodService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PAYMENT_METHOD_BY_ID_KEY = "App.Cache.PaymentMethod.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PAYMENT_METHOD_ALL_KEY = "App.Cache.PaymentMethod.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PAYMENT_METHOD_PATTERN_KEY = "App.Cache.PaymentMethod.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="paymentMethodRepository">The payment method repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public PaymentMethodService(IRepository<PaymentMethod> paymentMethodRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._paymentMethodRepository = paymentMethodRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual PaymentMethod GetPaymentMethodById(long paymentMethodId)
        {            

            return _paymentMethodRepository.GetById(paymentMethodId);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<PaymentMethod> GetAllPaymentMethods()
        {
            
                var query = from x in _paymentMethodRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
          
        }

        /// <summary>
        /// Gets all payment methods by affiliate identifier.
        /// </summary>
        /// <param name="affiliateid">The affiliateid.</param>
        /// <returns>IList&lt;PaymentMethod&gt;.</returns>
        public virtual IList<PaymentMethod> GetAllPaymentMethodsByAffiliateId(long affiliateid)
        {
                var query = from x in _paymentMethodRepository.Table
                            where x.AffiliateId == affiliateid
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;            
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="paymentMethod">The payment method.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">paymentMethod</exception>
        public virtual long InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new ArgumentNullException("paymentMethod");

            _paymentMethodRepository.Insert(paymentMethod);

            _cacheManager.RemoveByPattern(CACHE_PAYMENT_METHOD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(paymentMethod);

            return paymentMethod.Id;
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <returns>System.Int64.</returns>
        public virtual long SetDefaultPaymentMethod(long Id, long AffiliateId)
        {
            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "Id";
            IdParam.Value = Id;
            IdParam.DbType = DbType.Int64;

            var affiliateIdParam = _dataProvider.GetParameter();
            affiliateIdParam.ParameterName = "@AffiliateId";
            affiliateIdParam.Value = AffiliateId;
            affiliateIdParam.DbType = DbType.Int64;

            _paymentMethodRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[SetDefaultPaymentMethod] @Id, @AffiliateId", IdParam, affiliateIdParam).FirstOrDefault();

            return 0;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="paymentMethod">The payment method.</param>
        /// <exception cref="ArgumentNullException">paymentMethod</exception>
        public virtual void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new ArgumentNullException("paymentMethod");

            PaymentMethod pm = _paymentMethodRepository.GetById(paymentMethod.Id);
            pm.AccountNumber = paymentMethod.AccountNumber;
            pm.AccountOwnerAddress = paymentMethod.AccountOwnerAddress;
            pm.AccountOwnerPhone = paymentMethod.AccountOwnerPhone;
            pm.AffiliateId = paymentMethod.AffiliateId;
            pm.BankAddress = paymentMethod.BankAddress;
            pm.BankName = paymentMethod.BankName;
            pm.BankPhone = paymentMethod.BankPhone;
            pm.IsPrimary = paymentMethod.IsPrimary;
            pm.NameOnAccount = paymentMethod.NameOnAccount;
            pm.PaymentType = paymentMethod.PaymentType;
            pm.SpecialInstructions = paymentMethod.SpecialInstructions;
            pm.SwiftRoutingNumber = paymentMethod.SwiftRoutingNumber;

            _paymentMethodRepository.Update(pm);

            _cacheManager.RemoveByPattern(CACHE_PAYMENT_METHOD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(paymentMethod);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <exception cref="ArgumentNullException">paymentMethod</exception>
        public virtual void DeletePaymentMethod(long Id)
        {
            if (Id == 0)
                throw new ArgumentNullException("paymentMethod");

            PaymentMethod paymentMethod = _paymentMethodRepository.GetById(Id);
            _paymentMethodRepository.Delete(paymentMethod);

            _cacheManager.RemoveByPattern(CACHE_PAYMENT_METHOD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(paymentMethod);
        }

        #endregion Methods
    }
}