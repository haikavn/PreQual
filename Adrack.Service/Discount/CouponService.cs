// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="CountryService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Click
{
   
    public partial class CouponService : ICouponService
    {
        #region Constants

        private const string CACHE_CLICK_CHANNEL_BY_ACCESS_KEY = "App.Cache.Click.Channel.By.AccessKey-{0}";

        private const string CACHE_CLICK_PATTERN_KEY = "App.Cache.Click.";

        #endregion Constants

        #region Fields

        private readonly IRepository<Coupon> _couponRepository;


        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Country Service
        /// </summary>
        /// <param name="countryRepository">Country Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public CouponService(IRepository<Coupon> couponRepository, IRepository<User> userRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._userRepository = userRepository;
            this._couponRepository = couponRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        public void DeleteCoupon(Coupon Coupon)
        {
            
        }

        public IList<Coupon> GetAllCoupons()
        {
            throw new NotImplementedException();
        }

        #endregion Constructor

        #region Methods

       

        public Coupon GetCouponById(long Id)
        {
            if (Id == 0)
                return null;

            return _couponRepository.GetById(Id);
        }

        public IList<Coupon> GetCouponsByUserId(long userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
                return _couponRepository.Table.Where(c => c.UserId == userId).ToList();
            else return null;
        }

        

        public long InsertCoupon(Coupon Coupon)
        {
            if (Coupon == null)
                throw new ArgumentNullException("Coupon");


            _couponRepository.SetCanTrackChanges(true);

            _couponRepository.Insert(Coupon);

            _appEventPublisher.EntityInserted(Coupon);
            _cacheManager.ClearRemoteServersCache();
            return Coupon.Id;
        }

        public void UpdateCoupon(Coupon Coupon)
        {
            
            if (Coupon == null)
                throw new ArgumentNullException("Coupon");

            _couponRepository.SetCanTrackChanges(true);

            _couponRepository.Update(Coupon);

           
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(Coupon);
        }


        #endregion Methods
    }
}