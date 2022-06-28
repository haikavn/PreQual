// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="VerticalService.cs" company="Adrack.com">
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
using System.IO;
using System.Linq;
using System.Xml;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class VerticalService.
    /// Implements the <see cref="Adrack.Service.Lead.IVerticalService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IVerticalService" />
    public partial class VerticalService : IVerticalService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_VERTICAL_BY_ID_KEY = "App.Cache.Vertical.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_VERTICAL_ALL_KEY = "App.Cache.Vertical.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_VERTICAL_PATTERN_KEY = "App.Cache.Vertical.";
        /// <summary>
        /// Cache Profile All Fields Key
        /// </summary>
        private const string CACHE_VERTICAL_Field_ALL_KEY = "App.Cache.VerticalField.All";

        private const string CACHE_VERTICAL_GetFieldsByVerticalId = "App.Cache.VerticalField.GetFieldsByVerticalId";


        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_VERTICAL_Field_PATTERN_KEY = "App.Cache.VerticalField.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Vertical> _verticalRepository;

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<VerticalField> _verticalFieldRepository;

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
        /// Profile Service
        /// </summary>
        /// <param name="verticalRepository">The vertical repository.</param>
        /// <param name="verticalFieldRepository"> The vertical field repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public VerticalService(IRepository<Vertical> verticalRepository,
                               IRepository<VerticalField> verticalFieldRepository,
                               ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._verticalRepository = verticalRepository;
            this._verticalFieldRepository = verticalFieldRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Vertical GetVerticalById(long Id)
        {
            if (Id == 0)
                return null;

            return _verticalRepository.GetById(Id);
        }

        /// <summary>
        /// Get Vertical By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Vertical GetVerticalByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return (from x in _verticalRepository.Table where x.Name == name select x).FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Vertical> GetAllVerticals()
        {
            string key = CACHE_VERTICAL_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _verticalRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">vertical</exception>
        public virtual long InsertVertical(Vertical vertical)
        {
            if (vertical == null)
                throw new ArgumentNullException("vertical");

            _verticalRepository.Insert(vertical);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(vertical);

            return vertical.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        /// <exception cref="ArgumentNullException">vertical</exception>
        public virtual void UpdateVertical(Vertical vertical)
        {
            if (vertical == null)
                throw new ArgumentNullException("vertical");

            _verticalRepository.Update(vertical);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(vertical);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        /// <exception cref="ArgumentNullException">vertical</exception>
        public virtual void DeleteVertical(Vertical vertical)
        {
            if (vertical == null)
                throw new ArgumentNullException("vertical");

            _verticalRepository.Delete(vertical);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(vertical);
        }


        /// <summary>
        ///  Get VerticalField By Id
        /// </summary>
        /// <param name="Id">long</param>
        /// <returns>VerticalField</returns>
        public virtual VerticalField GetVerticalFieldById(long Id)
        {
            if (Id == 0)
                return null;

            return _verticalFieldRepository.GetById(Id);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<VerticalField> GetAllVerticalFields()
        {
            string key = CACHE_VERTICAL_Field_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _verticalFieldRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        public virtual IList<VerticalField> GetAllVerticalFields(long verticalId)
        {
            string key = string.Format(CACHE_VERTICAL_GetFieldsByVerticalId, verticalId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _verticalFieldRepository.Table
                            where x.VerticalId == verticalId
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">verticalField</exception>
        public virtual long InsertVerticalField(VerticalField verticalField)
        {
            if (verticalField == null)
                throw new ArgumentNullException("verticalField");

            _verticalFieldRepository.Insert(verticalField);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_Field_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(verticalField);

            return verticalField.Id;
        }
        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        /// <exception cref="ArgumentNullException">verticalField</exception>
        public virtual void UpdateVerticalField(VerticalField verticalField)
        {
            if (verticalField == null)
                throw new ArgumentNullException("verticalField");

            _verticalFieldRepository.Update(verticalField);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_Field_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(verticalField);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        /// <exception cref="ArgumentNullException">verticalField</exception>
        public virtual void DeleteVerticalField(VerticalField verticalField)
        {
            if (verticalField == null)
                throw new ArgumentNullException("verticalField");

            _verticalFieldRepository.Delete(verticalField);

            _cacheManager.RemoveByPattern(CACHE_VERTICAL_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(verticalField);
        }

        /// <summary>
        /// Load fields names from xml
        /// </summary>
        /// <param name="xml">string</param>
        /// <returns> List of field names </returns>
        public List<string> LoadFieldNamesFromXml(string xml)
        {
            var values = new List<string>();
            var xmlReader = XmlReader.Create(new StringReader(xml));
            xmlReader.Read();
            var subReader = xmlReader.ReadSubtree();
            if (!subReader.Read())
                return new List<string>();

            while (subReader.Read())
            {
                if (subReader.NodeType == XmlNodeType.Element)
                {
                    values.Add(subReader.Name);
                }
            }
            return values;
        }

        #endregion Methods
    }
}