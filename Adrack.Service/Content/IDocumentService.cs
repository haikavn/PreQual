// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IDocumentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Content;

//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Arman Zakaryan
// Description:	Document Service
//------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Interface IDocumentService
    /// </summary>
    public partial interface IDocumentService
    {
        #region Methods

        /// <summary>
        /// Gets the documents by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>IList&lt;Document&gt;.</returns>
        IList<Document> GetDocumentsByAffiliateId(long affiliateId);

        /// <summary>
        /// AddDocument
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>long</returns>
        long AddDocument(Document doc);

        /// <summary>
        /// DeleteDocument
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>long</returns>
        long DeleteDocument(long Id);

        #endregion Methods
    }
}