// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserSubscribtionMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class UserSubscribtionMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.UserSubscribtion}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.UserSubscribtion}" />
    public partial class UserSubscribtionMap : AppEntityTypeConfiguration<UserSubscribtion>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public UserSubscribtionMap() // elite group
        {
            this.ToTable("UserSubscribtion");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}