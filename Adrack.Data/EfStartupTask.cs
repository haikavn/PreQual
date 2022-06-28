// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EfStartupTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Entity Framework Startup Task
    /// Implements the <see cref="Adrack.Core.Infrastructure.IAppStartupTask" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.IAppStartupTask" />
    public partial class EfStartupTask : IAppStartupTask
    {
        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        /// <exception cref="AppException">No IDataProvider found</exception>
        public void Execute()
        {
            var dataSetting = AppEngineContext.Current.Resolve<DataSetting>();

            if (dataSetting != null && dataSetting.IsValid())
            {
                var dataProvider = AppEngineContext.Current.Resolve<IDataProvider>();

                if (dataProvider == null)
                    throw new AppException("No IDataProvider found");

                dataProvider.SetDatabaseInitializer();
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Order
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return -1000; }
        }

        #endregion Properties
    }
}