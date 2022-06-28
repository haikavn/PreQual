// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RequestManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service;
using Adrack.Service.Helpers;
using Adrack.Web.Managers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

//using Microsoft.Web.Administration;

namespace Adrack.Managers
{ 
    /// <summary>
    ///     Class RequestManager.
    ///     Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RequestManager : IDisposable
    {
        public class FilterData
        {
            public CampaignField CampaignField { get; set; }
            public object ChannelField { get; set; } 

            public object Filter { get; set; }

            public object Value { get; set; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                export.Dispose();
        }

        #region Private properties

        /// <summary>
        ///     The export
        /// </summary>
        private readonly ExportManager export = new ExportManager();

        //private LeadsContext dbContext = new LeadsContext();
        /// <summary>
        ///     The import
        /// </summary>
        private readonly ImportManager import = new ImportManager();

        /// <summary>
        ///     The response
        /// </summary>
        private ResponseManager response = new ResponseManager();

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets the import manager.
        /// </summary>
        /// <value>The import.</value>
        public ImportManager Import => import;

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public ResponseManager Response
        {
            get => response;
            set => response = value;
        }

        /// <summary>
        ///     Gets or sets the test manager.
        /// </summary>
        /// <value>The test manager.</value>
        public TestingManager TestManager { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value><c>true</c> if [test mode]; otherwise, <c>false</c>.</value>
        public bool TestMode { get; set; }

        #endregion Public properties

        #region Public methods

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestManager" /> class.
        /// </summary>
        public RequestManager()
        {
            TestManager = new TestingManager();
            TestMode = false;
        }

        /// <summary>
        ///     Generates the error affiliate response.
        /// </summary>
        /// <param name="context">The context.</param>
        public void GenerateErrorAffiliateResponse(RequestContext context, string message)
        {
            try
            {
                var ar = new AffiliateResponse
                {
                    AffiliateChannelId = (context.Extra["informat"] as AffiliateChannel).Id,
                    AffiliateId = (context.Extra["informat"] as AffiliateChannel).AffiliateId,
                    Created = Helpers.UtcNow(),
                    LeadId = context.Extra.ContainsKey("lead") && (context.Extra["lead"] as LeadMain).Id > 0
                        ? (context.Extra["lead"] as LeadMain).Id
                        : (long?)null,
                    Response = "Error:"+message,
                    MinPrice = 0,
                    ProcessStartedAt = context.StartDateUtc,
                    Message = "Error:"+ message,
                    Status = (short)RequestResult.ResultTypes
                        .Error, //(res ? (short)export.Result.ResultType : (short)import.Result.ResultType);
                    ErrorType = (short)RequestResult.ErrorTypes
                        .None, // (res ? (short)export.Result.ErrorType : (short)import.Result.ErrorType);
                    Validator = null,
                    State = context.Extra.ContainsKey("leadContent") ? (context.Extra["leadContent"] as LeadContent).State : ""
                };
                context.AffiliateResponseService.InsertAffiliateResponse(ar);

                if (context.Extra.ContainsKey("lead") && context.Extra.ContainsKey("leadContent"))
                {
                    (context.Extra["lead"] as LeadMain).Status = (short)RequestResult.ResultTypes.Error;
                    (context.Extra["lead"] as LeadMain).ErrorType = (short)RequestResult.ErrorTypes.None;

                    //context.LeadContentService.UpdateLeadContent(context.Extra["leadContent"] as LeadContent);
                    //context.LeadMainService.UpdateLeadMain(context.Extra["lead"] as LeadMain);
                }

                import.Result.ResultType = RequestResult.ResultTypes.Error;
                import.Result.ErrorType = RequestResult.ErrorTypes.None;
                import.Result.Message = message;
                if (context.Extra.ContainsKey("informat"))
                    response.PrepareResponse(context, (AffiliateChannel)context.Extra["informat"], import.Result,
                        false);
                else
                    response.PrepareResponse(context, null, import.Result, false);
            }
            catch
            {
                //handle exception here
            }
        }

        /// <summary>
        ///     Processes the data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="context">The context.</param>
        /// <param name="isachannel">if set to <c>true</c> [isachannel].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ProcessData(HttpRequestBase httpRequest, HttpResponseBase httpResponse, RequestContext context,
            bool isachannel)
        {

            var affMessage = "";

            string baseUrl = Helpers.GetServerIPAddress();

            if (!context.CachedUrlService.CheckCachedUrl(baseUrl))
            {
                var cacheManager = AppEngineContext.Current.ContainerManager.Resolve<ICacheManager>("Application.Cache.Manager_Static");
                cacheManager.Clear();                
                context.CachedUrlService.InsertCachedUrl(new CachedUrl() { Url = baseUrl });
                SharedData.ClearBuyerChannelLeadsCount();
            }

            var xmlHelper = new XmlHelper();

            import.XmlHelper = xmlHelper;
            export.XmlHelper = xmlHelper;
            InitSettings(context);
            // WaitForQueueRelease();

            if (!string.IsNullOrEmpty(httpRequest["mode"]))
                context.Extra["mode"] = httpRequest["mode"];
            else
                context.Extra["mode"] = "";

            if (!string.IsNullOrEmpty(httpRequest["BuyerChannelId"]))
            {
                long bChannelId;
                long.TryParse(httpRequest["BuyerChannelId"], out bChannelId);
                context.Extra["BuyerChannelId"] = bChannelId;
            }
            else
            {
                context.Extra["BuyerChannelId"] = 0;
            }

            context.TimeZoneSetting = context.SettingService.GetSetting("TimeZone");
            context.StartDateUtc = Helpers.UtcNow();

            context.HttpRequest = httpRequest;
            context.HttpResponse = httpResponse;
            context.Manager = this;

            var res = import.ProcessData(context, isachannel);
            bool importRes = res;

            if (context.Extra.ContainsKey("informat"))
            {
                var affiliateChannel = (AffiliateChannel)context.Extra["informat"];

                if (res && affiliateChannel.Status !=
                    (short)RequestResult.StatusTypes.Test)
                {
                    res = export.Process(context, (LeadMain)context.Extra["lead"]);

                    if (affiliateChannel.Timeout.HasValue && affiliateChannel.Timeout.Value > 0 && export.PastTotalSeconds >= affiliateChannel.Timeout.Value)
                    {
                        import.Result.ResultType = RequestResult.ResultTypes.Reject;
                        import.Result.Message = "Affiliate timeout";

                        if (context.Extra.ContainsKey("lead"))
                        {
                            (context.Extra["lead"] as LeadMain).Status = (short)import.Result.ResultType;
                            (context.Extra["lead"] as LeadMain).ErrorType = (short)import.Result.ErrorType;
                        }

                        response.PrepareResponse(context, affiliateChannel, import.Result, false);
                    }
                    else
                    {
                        affMessage = context.Extra.ContainsKey("AffiliateResponseMessage")
                               ? context.Extra["AffiliateResponseMessage"].ToString()
                               : "";

                        if (export.Result.Message == null)
                            export.Result.Message = "";

                        if (affMessage != "")
                            export.Result.Message += affMessage;

                        response.PrepareResponse(context, (AffiliateChannel)context.Extra["informat"], export.Result,
                            true);
                        if (!context.Extra.ContainsKey("succeded"))
                        {
                            (context.Extra["lead"] as LeadMain).Status = (short)export.Result.ResultType;
                            (context.Extra["lead"] as LeadMain).ErrorType = (short)export.Result.ErrorType;
                        }
                        else
                        {
                            (context.Extra["lead"] as LeadMain).Status = (short)RequestResult.ResultTypes.Success;
                            (context.Extra["lead"] as LeadMain).ErrorType = (short)RequestResult.ErrorTypes.None;
                        }
                    }
                }
                else
                {
                    if (((AffiliateChannel)context.Extra["informat"]).Status == (short)RequestResult.StatusTypes.Test)
                    {
                        import.Result.ResultType = RequestResult.ResultTypes.Test;
                        import.Result.ErrorType = RequestResult.ErrorTypes.None;
                        import.Result.Message = "Test succeded";
                    }

                    if (context.Extra.ContainsKey("informat"))
                        response.PrepareResponse(context, (AffiliateChannel)context.Extra["informat"], import.Result,
                            false);
                    else
                        response.PrepareResponse(context, null, import.Result, false);

                    if (context.Extra.ContainsKey("lead"))
                    {
                        (context.Extra["lead"] as LeadMain).Status = (short)import.Result.ResultType;
                        (context.Extra["lead"] as LeadMain).ErrorType = (short)import.Result.ErrorType;
                    }
                }
            }
            else
            {
                response.PrepareResponse(context, null, import.Result, false);

                if (context.Extra.ContainsKey("lead"))
                {
                    (context.Extra["lead"] as LeadMain).Status = (short)import.Result.ResultType;
                    (context.Extra["lead"] as LeadMain).ErrorType = (short)import.Result.ErrorType;
                }
            }

            if (context.Extra.ContainsKey("dublTask"))
                (context.Extra["dublTask"] as Task).Wait();

            //context.LeadContentService.UpdateLeadContent(context.Extra["leadContent"] as LeadContent);
            //context.LeadMainService.UpdateLeadMain(context.Extra["lead"] as LeadMain);

            try
            {
                LeadMain leadMain = (context.Extra["lead"] as LeadMain);

                if (leadMain.Id > 0 || (leadMain.Id == 0 && context.DebugMode))
                {
                    var ar = new AffiliateResponse
                    {
                        AffiliateChannelId = (context.Extra["informat"] as AffiliateChannel).Id,
                        AffiliateId = (context.Extra["informat"] as AffiliateChannel).AffiliateId,
                        Created = Helpers.UtcNow(),
                        LeadId = leadMain.Id > 0
                            ? leadMain.Id
                            : (long?)null,
                        Response = Response.Response,
                        MinPrice = (decimal)(context.Extra["leadContent"] as LeadContent).Minprice,
                        ProcessStartedAt = context.StartDateUtc,
                        Message = affMessage,
                        Status = (short)import.Result.ResultType,
                        ErrorType = (short)import.Result.ErrorType,
                        Validator = import.Result.Validator,
                        ReceivedData = (context.Extra.ContainsKey("ReceivedData") && !importRes && context.DebugMode ? context.Extra["ReceivedData"].ToString() : "")
                    };
                    context.AffiliateResponseService.InsertAffiliateResponse(ar);
                }
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                context.ProcessingLogService.InsertProcessingLog(new ProcessingLog
                {
                    Created = Helpers.UtcNow(),
                    LeadId = (context.Extra["lead"] as LeadMain).Id,
                    Message = ex.Message,
                    Name = "AffiliateResponse"
                });
            }

            return res;
        }

        /// <summary>
        ///     Initializes the settings.
        /// </summary>
        /// <param name="context">The context.</param>
        protected void InitSettings(RequestContext context)
        {
            var set = context.SettingService.GetSetting("System.DublicateMonitor");
            short dm;
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.DublicateMonitor = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.AllowAffiliateRedirect");
            context.AllowAffiliateRedirect = false;
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.AllowAffiliateRedirect = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.AffiliateRedirectUrl");
            context.AffiliateRedirectUrl = set == null ? "" : set.Value;

            context.MaxDuplicateDays = true;
            set = context.SettingService.GetSetting("System.MaxDuplicateDays");
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.MaxDuplicateDays = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.LeadEmail");
            context.LeadEmail = false;
            dm = 0;
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.LeadEmail = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.MinProcessingMode");
            context.MinProcessingMode = false;
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.MinProcessingMode = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.SystemOnHold");
            context.SystemOnHold = false;
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.SystemOnHold = dm == 1 ? true : false;
            }

            set = context.SettingService.GetSetting("System.LeadEmailTo");
            context.LeadEmailTo = set == null ? "" : set.Value;

            set = context.SettingService.GetSetting("System.LeadEmailFields");
            context.LeadEmailFields = set == null ? "" : set.Value;

            set = context.SettingService.GetSetting("System.WhiteIp");
            context.WhiteIp = set == null ? "" : set.Value;

            var maxProcessingLeadsSetting = context.SettingService.GetSetting("System.MaxProcessingLeads");

            var maxProcessingLeads = 10;
            if (maxProcessingLeadsSetting != null)
                int.TryParse(maxProcessingLeadsSetting.Value, out maxProcessingLeads);
            GlobalDataManager.MaxProcessingLeads = maxProcessingLeads;

            var processingDelaySetting = context.SettingService.GetSetting("System.ProcessingDelay");

            var processingDelay = 500;
            if (processingDelaySetting != null)
                int.TryParse(processingDelaySetting.Value, out processingDelay);
            GlobalDataManager.ProcessingDelay = processingDelay;

            set = context.SettingService.GetSetting("System.DebugMode");
            if (set != null)
            {
                short.TryParse(set.Value, out dm);
                context.DebugMode = dm == 1 ? true : false;
            }
        }

        /// <summary>
        ///     Waits for queue release.
        /// </summary>
        protected void WaitForQueueRelease()
        {
            GlobalDataManager.WaitingLeads++;

            while (GlobalDataManager.CurrentLeadCount > GlobalDataManager.MaxProcessingLeads)
                Thread.Sleep(GlobalDataManager.ProcessingDelay);
            GlobalDataManager.WaitingLeads--;
            GlobalDataManager.CurrentLeadCount++;
        }

        #endregion Public methods
    }
}