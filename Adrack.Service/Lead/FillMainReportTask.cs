// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FillMainReportTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Agent;
using System.Configuration;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class FillMainReportTask.
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class FillMainReportTask : ITask
    {
        #region Fields

        /// <summary>
        /// Site Setting
        /// </summary>
        private readonly AppSetting _appSetting;

        private readonly IRepository<LeadMain> _leadMainRepository;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Ping Task
        /// </summary>
        /// <param name="appSetting">Application Setting</param>
        /// <param name="dbContext">The database context.</param>
        public FillMainReportTask(AppSetting appSetting, IRepository<LeadMain> leadMainRepository)
        {
            this._appSetting = appSetting;
            this._leadMainRepository = leadMainRepository;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            var demoMode = ConfigurationManager.AppSettings["DemoMode"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode == "true")
                _leadMainRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[FillMainReport]").FirstOrDefault();
        }

        #endregion Methods
    }
}