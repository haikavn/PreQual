// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 05-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-11-2020
// ***********************************************************************
// <copyright file="IPaymentService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Billing;
using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;
using System.IO;

namespace Adrack.Service.Content
{
    /// <summary>
    /// Represents a Payment Service
    /// </summary>
    public partial interface IStorageService
    {
        #region Methods

        Uri Upload(string blobContainerName, Stream content, string contentType, string fileName);
        void DeleteFile(string blobContainerName, string uniqueFileIdentifier);

        string DownloadFile(string blobContainerName, string fileName, Stream stream);
        #endregion Methods
    }
}