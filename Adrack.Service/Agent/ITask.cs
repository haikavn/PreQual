// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ITask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Represents a Task
    /// </summary>
    public partial interface ITask
    {
        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        void Execute();

        #endregion Methods
    }
}