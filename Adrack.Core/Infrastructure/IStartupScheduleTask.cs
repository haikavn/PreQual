// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="IStartupScheduleTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Startup Schedule Task
    /// </summary>
    public interface IStartupScheduleTask
    {
        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        void Execute();

        #endregion Methods

        #region Properties

        /// <summary>
        /// Order
        /// </summary>
        /// <value>The order.</value>
        int Order { get; }

        #endregion Properties
    }
}