// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterService.cs" company="Adrack.com">
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
    /// Class DoNotPresentService.
    /// Implements the <see cref="Adrack.Service.Lead.IDoNotPresentService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IDoNotPresentService" />
    public partial class DoNotPresentService : IDoNotPresentService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_DONOTPRESENT_BY_ID_KEY = "App.Cache.DoNotPresent.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_DONOTPRESENT_ALL_KEY = "App.Cache.DoNotPresent.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_DONOTPRESENT_PATTERN_KEY = "App.Cache.DoNotPresent.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<DoNotPresent> _doNotPresentRepository;

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
        /// <param name="doNotPresentRepository">The do not present repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public DoNotPresentService(IRepository<DoNotPresent> doNotPresentRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._doNotPresentRepository = doNotPresentRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="filter">The DoNotPresent.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">DoNotPresent</exception>
        public virtual long InsertDoNotPresent(DoNotPresent doNotPresent)
        {
            if (doNotPresent == null)
                throw new ArgumentNullException("doNotPresent");

            _doNotPresentRepository.Insert(doNotPresent);

            _cacheManager.RemoveByPattern(CACHE_DONOTPRESENT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(doNotPresent);

            return doNotPresent.Id;
        }

        public virtual int CheckDoNotPresent(string email, string ssn, long buyerId)
        {
            var ssnParam = _dataProvider.GetParameter();
            ssnParam.ParameterName = "ssn";
            ssnParam.Value = ssn;
            ssnParam.DbType = DbType.String;

            var emailParam = _dataProvider.GetParameter();
            emailParam.ParameterName = "email";
            emailParam.Value = email;
            emailParam.DbType = DbType.String;

            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "buyerid";
            buyerIdParam.Value = buyerId;
            buyerIdParam.DbType = DbType.Int64;

            return _doNotPresentRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[CheckDoNotPresent] @email,@ssn,@buyerid", emailParam, ssnParam, buyerIdParam).FirstOrDefault();
        }

        #endregion Methods
    }
}