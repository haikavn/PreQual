// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="INavigationService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;
using Adrack.Data;
using System.Collections.Generic;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Navigation Service
    /// </summary>
    public partial interface IDbContextService
    {
        #region Methods

        void AddClientContext(string domain, string key, string connectionString);

        void RemoveClientContext(string key);

        IDbClientContext GetClientContext(string key);

        #endregion Methods
    }
}