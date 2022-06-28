// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 01-20-2021
//
// Last Modified By : Grigori
// Last Modified On : 01-20-2021
// ***********************************************************************
// <copyright file="DemoLead.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Configuration;
using Adrack.Core;
using Adrack.Service.Agent;
using System.Net;
using Adrack.Core.Infrastructure;
using Adrack.Service.Lead;
using Adrack.Service.Security;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Ping Task
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class DemoLeadTask : ITask
    {
        #region Fields

        /// <summary>
        /// Site Setting
        /// </summary>
        private readonly AppSetting _appSetting;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Ping Task
        /// </summary>
        /// <param name="appSetting">Application Setting</param>
        public DemoLeadTask(AppSetting appSetting)
        {
            this._appSetting = appSetting;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
#if DEBUG
            return;
#endif
            var demoMode = ConfigurationManager.AppSettings["DemoMode"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode == "true")
            {
                var leadDemoModeService = AppEngineContext.Current.Resolve<ILeadDemoModeService>();

                leadDemoModeService.LeadPostingSimulation();
            }
        }

        #endregion Methods
    }
}