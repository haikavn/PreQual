// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DoNotPresentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class SubIdWhiteListMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.SubIdWhiteList}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.SubIdWhiteList}" />
    public partial class SubIdWhiteListMap : AppEntityTypeConfiguration<SubIdWhiteList>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public SubIdWhiteListMap() // elite group
        {
            this.ToTable("SubIdWhiteList");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}