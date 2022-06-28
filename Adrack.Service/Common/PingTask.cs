// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PingTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Service.Agent;
using System.Net;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Ping Task
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class PingTask : ITask
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
        public PingTask(AppSetting appSetting)
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
            /*string url = this._appSetting.Url + "ping/index";

            using (var webClient = new WebClient())
            {
                webClient.DownloadString(url);
            }*/
        }

        #endregion Methods
    }
}