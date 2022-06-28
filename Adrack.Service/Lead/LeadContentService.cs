// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadContentService.cs" company="Adrack.com">
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
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.ILeadContentService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadContentService" />
    public partial class LeadContentService : ILeadContentService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEADCONETENT_BY_ID_KEY = "App.Cache.LeadContent.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_ALL_KEY = "App.Cache.LeadContent.All";

       

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_PATTERN_KEY = "App.Cache.LeadContent.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<LeadContent> _leadMainRepository;

        /// <summary>
        /// The lead geo data repository
        /// </summary>
        private readonly IRepository<LeadGeoData> _leadGeoDataRepository;

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
        /// <param name="leadMainRepository">The lead main repository.</param>
        /// <param name="leadGeoDataRepository">The lead geo data repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public LeadContentService(IRepository<LeadContent> leadMainRepository, IRepository<LeadGeoData> leadGeoDataRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._leadMainRepository = leadMainRepository;
            this._leadGeoDataRepository = leadGeoDataRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead content by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContent.</returns>
        public virtual LeadContent GetLeadContentById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEADCONETENT_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _leadMainRepository.GetById(leadId); });
        }

        /// <summary>
        /// Gets the lead content by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContent.</returns>
        public virtual LeadContent GetLeadContentByLeadId(long leadId)
        {
            if (leadId == 0)
                return null;

            var query = from x in _leadMainRepository.Table
                        where x.LeadId == leadId
                        select x;

            var profiles = query.FirstOrDefault();

            return profiles;
        }

        /// <summary>
        /// Gets the dublicate lead.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="created">The created.</param>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>LeadContent.</returns>
        public virtual LeadContent GetDublicateLead(string ssn, DateTime created, long affiliateId)
        {
            var ssnParam = _dataProvider.GetParameter();
            ssnParam.ParameterName = "ssn";
            ssnParam.Value = (ssn == null ? "" : ssn);
            ssnParam.DbType = DbType.String;

            var createdParam = _dataProvider.GetParameter();
            createdParam.ParameterName = "created";
            createdParam.Value = created;
            createdParam.DbType = DbType.DateTime;

            var affiliateIdParam = _dataProvider.GetParameter();
            affiliateIdParam.ParameterName = "affiliateId";
            affiliateIdParam.Value = affiliateId;
            affiliateIdParam.DbType = DbType.Int64;

            return _leadMainRepository.GetDbClientContext().SqlQuery<LeadContent>("EXECUTE [dbo].[GetDublicateLead] @ssn, @created, @affiliateid", ssnParam, createdParam, affiliateIdParam).FirstOrDefault();
        }

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<LeadContent> GetAllLeads()
        {
            string key = CACHE_LEAD_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadMainRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Checks for dublicate.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="leadid">The leadid.</param>
        /// <returns>LeadContent.</returns>
        public virtual List<LeadContent> CheckForDublicate(string ssn, long leadid)
        {
            var ssnParam = _dataProvider.GetParameter();
            ssnParam.ParameterName = "ssn";
            ssnParam.Value = (ssn == null ? "" : ssn);
            ssnParam.DbType = DbType.String;

            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadid;
            leadIdParam.DbType = DbType.Int64;

            return _leadMainRepository.GetDbClientContext().SqlQuery<LeadContent>("EXECUTE [dbo].[CheckForDublicate] @ssn, @leadId", ssnParam, leadIdParam).ToList();
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual long InsertLeadContent(LeadContent leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Insert(leadMain);

            var IpAddressParam = _dataProvider.GetParameter();
            IpAddressParam.ParameterName = "IpAddress";
            IpAddressParam.Value = leadMain.Ip;
            IpAddressParam.DbType = DbType.String;

            /*GeoLocation geoLocation = _dbContext.SqlQuery<GeoLocation>("EXECUTE [dbo].[usp_geo_location] @IpAddress", IpAddressParam).FirstOrDefault();

            if (geoLocation != null)
            {
                LeadGeoData leadGeoData = new LeadGeoData();

                leadGeoData.LeadId = leadMain.Id;
                leadGeoData.AreaCode = leadGeoData.AreaCode;

                leadGeoData.CityName = geoLocation.CityName;
                leadGeoData.CountryCode = geoLocation.CountryCode;
                leadGeoData.CountryName = geoLocation.CountryName;
                leadGeoData.Latitude = geoLocation.Latitude.ToString();
                leadGeoData.Longitude = geoLocation.Longitude.ToString();
                leadGeoData.RegionName = geoLocation.RegionName;
                leadGeoData.TimeZone = geoLocation.TimeZone;
                leadGeoData.ZipCode = geoLocation.ZipCode;

                this._leadGeoDataRepository.Insert(leadGeoData);
            }*/

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadMain);

            return leadMain.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void UpdateLeadContent(LeadContent leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Update(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadMain);
        }

        /// <summary>
        /// Delete LeadContent
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <exception cref="ArgumentNullException">leadMain</exception>
        public virtual void DeleteLeadContent(LeadContent leadMain)
        {
            if (leadMain == null)
                throw new ArgumentNullException("leadMain");

            _leadMainRepository.Delete(leadMain);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(leadMain);
        }

        #endregion Methods
    }
}