// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AutoMapperStartupTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure;

namespace Adrack.Web.ContentManagement.Infrastructure
{
    /// <summary>
    /// Represents a Auto Mapper Startup Task
    /// Implements the <see cref="Adrack.Core.Infrastructure.IAppStartupTask" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.IAppStartupTask" />
    public partial class AutoMapperStartupTask : IAppStartupTask
    {
        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            AutoMapperConfiguration.Init();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Order
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return 0; }
        }

        #endregion Properties
    }
}