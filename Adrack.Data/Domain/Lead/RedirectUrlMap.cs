// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RedirectUrlMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class RedirectUrlMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.RedirectUrl}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.RedirectUrl}" />
    public partial class RedirectUrlMap : AppEntityTypeConfiguration<RedirectUrl>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public RedirectUrlMap() // elite group
        {
            this.ToTable("RedirectUrl");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}