using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Message;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Models.New.Lead;


namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/emailProcessing")]
    public class EmailProcessingController : BaseApiController
    {
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IAppContext _appContext;
        private readonly ILeadMainService _leadMainService;
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly ICampaignService _campaignService;
        private readonly IAffiliateResponseService _affiliateResponseService;
        private readonly ILeadMainResponseService _leadMainResponseService;
        private readonly ILeadContentDublicateService _leadContentDuplicateService;


        public EmailProcessingController(
            IEmailService emailService,
            ISettingService settingService,
            IAppContext appContext,
            ILeadMainService leadMainService,
            IAffiliateService affiliateService,
            IAffiliateChannelService affiliateChannelService,
            IBuyerService buyerService,
            IBuyerChannelService buyerChannelService,
            ICampaignService campaignService,
            IAffiliateResponseService affiliateResponseService,
            ILeadMainResponseService leadMainResponseService,
            ILeadContentDublicateService leadContentDuplicateService)
        {
            _emailService = emailService;
            _settingService = settingService;
            _appContext = appContext;
            _leadMainService = leadMainService;
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _campaignService = campaignService;
            _affiliateResponseService = affiliateResponseService;
            _leadMainResponseService = leadMainResponseService;
            _leadContentDuplicateService = leadContentDuplicateService;
        }



        /// <summary>
        /// Send Queue Emails
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("sendQueueEmails")]
        public IHttpActionResult SendQueueEmails(string password)
        {
            if (password != "asd528_dje8dGh")
            {
                return HttpBadRequest("Incorrect password");
            }

            try
            {
                _emailService.SendQueueEmails();
                return Ok();
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}