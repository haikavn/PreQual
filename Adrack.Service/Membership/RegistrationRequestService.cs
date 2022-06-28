// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RegistrationRequestService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a RegistrationRequest Service
    /// Implements the <see cref="Adrack.Service.Membership.IRegistrationRequestService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IRegistrationRequestService" />
    public partial class RegistrationRequestService : IRegistrationRequestService
    {
        #region Constants

        /// <summary>
        /// Cache RegistrationRequest By Id Key
        /// </summary>
        private const string CACHE_REGISTRATION_REQUEST_BY_ID_KEY = "App.Cache.RegistrationRequest.By.Id-{0}";

        /// <summary>
        /// The cache registration request by userid key
        /// </summary>
        private const string CACHE_REGISTRATION_REQUEST_BY_USERID_KEY = "App.Cache.RegistrationRequest.By.UserId-{0}";

        /// <summary>
        /// Cache RegistrationRequest All Key
        /// </summary>
        private const string CACHE_REGISTRATION_REQUEST_ALL_KEY = "App.Cache.RegistrationRequest.All";

        /// <summary>
        /// Cache RegistrationRequest Pattern Key
        /// </summary>
        private const string CACHE_REGISTRATION_REQUEST_PATTERN_KEY = "App.Cache.RegistrationRequest.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// RegistrationRequest
        /// </summary>
        private readonly IRepository<RegistrationRequest> _registrationRequestRepository;

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
        /// RegistrationRequest Service
        /// </summary>
        /// <param name="registrationRequestRepository">The registration request repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public RegistrationRequestService(IRepository<RegistrationRequest> registrationRequestRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._registrationRequestRepository = registrationRequestRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>IList&lt;RegistrationRequest&gt;.</returns>
        public virtual IList<RegistrationRequest> GetRegistrationRequest(string email)
        {
                var query = from x in _registrationRequestRepository.Table
                            where x.Email == email
                            orderby x.Id
                            select x;

                return query.ToList();            
        }

        /// <summary>
        /// Gets the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="code">The code.</param>
        /// <returns>RegistrationRequest.</returns>
        public virtual RegistrationRequest GetRegistrationRequest(string email, string code)
        {
          
                var query = from x in _registrationRequestRepository.Table
                            where x.Code == code && x.Email == email
                            orderby x.Id
                            select x;

                return query.FirstOrDefault();
          
        }

        /// <summary>
        /// Insert RegistrationRequest
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        /// <exception cref="ArgumentNullException">registrationRequest</exception>
        public virtual void InsertRegistrationRequest(RegistrationRequest registrationRequest)
        {
            if (registrationRequest == null)
                throw new ArgumentNullException("registrationRequest");

            _registrationRequestRepository.Insert(registrationRequest);

            _cacheManager.RemoveByPattern(CACHE_REGISTRATION_REQUEST_PATTERN_KEY);

            _appEventPublisher.EntityInserted(registrationRequest);
        }

        /// <summary>
        /// Delete RegistrationRequest
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        /// <exception cref="ArgumentNullException">registrationRequest</exception>
        public virtual void DeleteRegistrationRequest(RegistrationRequest registrationRequest)
        {
            if (registrationRequest == null)
                throw new ArgumentNullException("registrationRequest");

            _registrationRequestRepository.Delete(registrationRequest);

            _cacheManager.RemoveByPattern(CACHE_REGISTRATION_REQUEST_PATTERN_KEY);
            
            _appEventPublisher.EntityDeleted(registrationRequest);
        }

        /// <summary>
        /// Deletes the registration request.
        /// </summary>
        /// <param name="email">The email.</param>
        public virtual void DeleteRegistrationRequest(string email)
        {
            List<RegistrationRequest> list = (List<RegistrationRequest>)this.GetRegistrationRequest(email);

            foreach (RegistrationRequest item in list)
            {
                DeleteRegistrationRequest(item);
            }
        }

        #endregion Methods
    }
}