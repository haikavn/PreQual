// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PostTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Service.Agent;
using Adrack.Service.Audit;
using System;
using System.Net;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Ping Task
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class PostTask : ITask
    {
        #region Fields

        /// <summary>
        /// Site Setting
        /// </summary>
        private readonly AppSetting _appSetting;

        /// <summary>
        /// Log service
        /// </summary>
        private readonly ILogService _logService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Ping Task
        /// </summary>
        /// <param name="appSetting">Application Setting</param>
        /// <param name="logService">Log Service</param>
        public PostTask(AppSetting appSetting, ILogService logService)
        {
            this._appSetting = appSetting;
            this._logService = logService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            using (var wb = new WebClient())
            {
                try
                {
                    wb.Headers.Add("Content-Type", "text/xml; encoding='utf-8'");

                    string response = wb.UploadString(Url, "POST", Data);
                }
                catch (Exception ex)
                {
                    //Arman Handle Exception
                    this._logService.Error(ex.Message, ex);
                }
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// The URL
        /// </summary>
        public string Url = "";

        /// <summary>
        /// The data
        /// </summary>
        public string Data = "";

        /// <summary>
        /// The buyer identifier
        /// </summary>
        public long BuyerId = 0;

        /// <summary>
        /// The response error
        /// </summary>
        public string ResponseError = "";

        /// <summary>
        /// The lead identifier
        /// </summary>
        public long LeadId = 0;

        /// <summary>
        /// The buyer price
        /// </summary>
        public decimal BuyerPrice = 0;

        /// <summary>
        /// The affiliate price
        /// </summary>
        public decimal AffiliatePrice = 0;

        /// <summary>
        /// The buyer channel identifier
        /// </summary>
        public long BuyerChannelId = 0;

        /// <summary>
        /// The affiliate channel identifier
        /// </summary>
        public long AffiliateChannelId = 0;

        /// <summary>
        /// The state
        /// </summary>
        public string State = "";

        /// <summary>
        /// The status
        /// </summary>
        public short Status = 0;

        /// <summary>
        /// The affiliate identifier
        /// </summary>
        public long AffiliateId = 0;

        /// <summary>
        /// The campaign identifier
        /// </summary>
        public long CampaignId = 0;

        /// <summary>
        /// The created
        /// </summary>
        public DateTime Created = DateTime.Now;

        /// <summary>
        /// The campaign type
        /// </summary>
        public short CampaignType = 0;

        #endregion Properties
    }
}