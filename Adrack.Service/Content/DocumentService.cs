// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="DocumentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Content;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Class DocumentService.
    /// Implements the <see cref="Adrack.Service.Content.IDocumentService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Content.IDocumentService" />
    public partial class DocumentService : IDocumentService
    {
        #region Constants

        /// <summary>
        /// Cache Document By Id Key
        /// </summary>
        private const string CACHE_DOCUMENT_BY_ID_KEY = "App.Cache.Document.By.Id-{0}";

        #endregion Constants

        #region Fields

        /// <summary>
        /// The document repository
        /// </summary>
        private readonly IRepository<Document> _DocumentRepository;

        /// <summary>
        /// The cache manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// The application event publisher
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
        /// <param name="documentRepository">The document repository.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        public DocumentService(IRepository<Document> documentRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._DocumentRepository = documentRepository;

            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the documents by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>IList&lt;Document&gt;.</returns>

        public virtual IList<Document> GetDocumentsByAffiliateId(long affiliateId)
        {
                var query = from x in _DocumentRepository.Table
                            where x.AffiliateId == affiliateId
                            orderby x.Id
                            select x;

                var result = query.ToList();

                return result;            
        }

        /// <summary>
        /// AddDocument
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>long</returns>
        public virtual long AddDocument(Document doc)
        {
            _DocumentRepository.Insert(doc);

            return doc.Id;
        }

        /// <summary>
        /// DeleteDocument
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>long</returns>
        public virtual long DeleteDocument(long Id)
        {
            Document d = _DocumentRepository.GetById(Id);
            _DocumentRepository.Delete(d);
            return 1;
        }
    }
}