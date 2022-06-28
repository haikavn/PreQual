// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ScheduleTaskMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Agent;

namespace Adrack.Data.Domain.Agent
{
    /// <summary>
    /// Represents a Schedule Task Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Agent.ScheduleTask}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Agent.ScheduleTask}" />
    public partial class ScheduleTaskMap : AppEntityTypeConfiguration<ScheduleTask>
    {
        #region Constructor

        /// <summary>
        /// Schedule Task Map
        /// </summary>
        public ScheduleTaskMap()
        {
            this.ToTable("ScheduleTask");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(500);
            this.Property(x => x.ServiceType).IsRequired().HasMaxLength(1000);
            this.Property(x => x.Seconds).IsRequired();
        }

        #endregion Constructor
    }
}