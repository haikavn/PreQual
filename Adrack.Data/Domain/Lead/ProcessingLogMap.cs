// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ProcessingLogMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class ProcessingLogMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.ProcessingLog}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.ProcessingLog}" />
    public partial class ProcessingLogMap : AppEntityTypeConfiguration<ProcessingLog>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public ProcessingLogMap()
        {
            this.ToTable("ProcessingLog");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}