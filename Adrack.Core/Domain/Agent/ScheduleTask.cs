// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ScheduleTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Agent
{
    /// <summary>
    /// Represents a Schedule Task
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ScheduleTask : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        /// <value>The type of the service.</value>
        public string ServiceType { get; set; }

        /// <summary>
        /// Gets or Sets the Seconds
        /// </summary>
        /// <value>The seconds.</value>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or Sets the Enabled
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or Sets the Stop On Error
        /// </summary>
        /// <value><c>true</c> if [stop on error]; otherwise, <c>false</c>.</value>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Gets or Sets the Machine Name
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or Sets the Machine Duration
        /// </summary>
        /// <value>The duration of the machine.</value>
        public DateTime? MachineDuration { get; set; }

        /// <summary>
        /// Gets or Sets the Last Start
        /// </summary>
        /// <value>The last start.</value>
        public DateTime? LastStart { get; set; }

        /// <summary>
        /// Gets or Sets the Last End
        /// </summary>
        /// <value>The last end.</value>
        public DateTime? LastEnd { get; set; }

        /// <summary>
        /// Gets or Sets the Last Success
        /// </summary>
        /// <value>The last success.</value>
        public DateTime? LastSuccess { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or Sets the First Start Time
        /// </summary>
        /// <value>The First Start Time.</value>
        public DateTime? FirstStartTime { get; set; }

        #endregion Properties
    }
}