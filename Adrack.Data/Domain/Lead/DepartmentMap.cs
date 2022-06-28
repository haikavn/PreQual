// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DepartmentMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class DepartmentMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Department}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.Department}" />
    public partial class DepartmentMap : AppEntityTypeConfiguration<Department>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public DepartmentMap()
        {
            this.ToTable("Department");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}