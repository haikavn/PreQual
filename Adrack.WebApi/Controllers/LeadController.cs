using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Xml;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.New.Lead;


namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/leads")]
    public class LeadController : BaseApiController
    {
        private readonly ISettingService _settingService;
        private readonly IAppContext _appContext;
        private readonly ILeadMainService _leadMainService;
        private readonly ILeadDemoModeService _leadDemoModeService;
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly ICampaignService _campaignService;
        private readonly IAffiliateResponseService _affiliateResponseService;
        private readonly ILeadMainResponseService _leadMainResponseService;
        private readonly ILeadContentDublicateService _leadContentDuplicateService;
        private readonly IRedirectUrlService _redirectUrlService;
        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly IPermissionService _permissionService;
        private readonly IReportService _reportService;

        private static string _viewLeadListKey { get; set; } = "view-list-lead";
        private static string _viewLeadInfoKey { get; set; } = "view-info-lead";

        public LeadController(
            ISettingService settingService,
            IAppContext appContext,
            ILeadMainService leadMainService,
            ILeadDemoModeService leadDemoModeService,
            IAffiliateService affiliateService,
            IAffiliateChannelService affiliateChannelService,
            IBuyerService buyerService,
            IBuyerChannelService buyerChannelService,
            ICampaignService campaignService,
            IAffiliateResponseService affiliateResponseService,
            ILeadMainResponseService leadMainResponseService,
            ILeadContentDublicateService leadContentDuplicateService,
            IRedirectUrlService redirectUrlService,
            ICampaignTemplateService campaignTemplateService,
            IPermissionService permissionService,
            IReportService reportService)
        {
            _settingService = settingService;
            _appContext = appContext;
            _leadMainService = leadMainService;
            _leadDemoModeService = leadDemoModeService;
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _campaignService = campaignService;
            _affiliateResponseService = affiliateResponseService;
            _leadMainResponseService = leadMainResponseService;
            _leadContentDuplicateService = leadContentDuplicateService;
            _redirectUrlService = redirectUrlService;
            _campaignTemplateService = campaignTemplateService;
            _permissionService = permissionService;
            _reportService = reportService;
        }

        [HttpPost]
        [Route("getLeads")]
        public IHttpActionResult GetLeads([FromBody]LeadRequestModel leadFilter)
        {
            if (!_permissionService.Authorize(_viewLeadListKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (leadFilter == null)
                {
                    return HttpBadRequest("Lead filter model is empty");
                }

                //........................DemoMode ..........................
                /*
                var demoMode = ConfigurationManager.AppSettings["DemoMode"];
                if (!string.IsNullOrEmpty(demoMode) && demoMode == "true")
                {
                    var leadsDemo = _leadDemoModeService.GetDemoLeads(leadFilter.DateFrom, leadFilter.DateTo);

                    var leadsDemoViewModelList = GetLeadDemoContentViewModels(leadsDemo, leadFilter);
                    return Ok(leadsDemoViewModelList);
                }
                */
                //...........................................................


                _reportService.ValidateFilters(FilterCampaignIds: leadFilter.FilterCampaignIds,
                    FilterAffiliateIds: leadFilter.FilterAffiliateIds,
                    FilterAffiliateChannelIds: leadFilter.FilterAffiliateChannelIds,
                    FilterBuyerIds: leadFilter.FilterBuyerIds,
                    FilterBuyerChannelIds: leadFilter.FilterBuyerChannelIds, fillBuyers: false);

                if (leadFilter.PageSize == 0)
                    leadFilter.PageSize = 100;

                if (leadFilter.Page == 0)
                    leadFilter.Page = 1;

                leadFilter.DateFrom = new DateTime(leadFilter.DateFrom.Year, leadFilter.DateFrom.Month, leadFilter.DateFrom.Day, 0, 0, 0);
                leadFilter.DateTo = new DateTime(leadFilter.DateTo.Year, leadFilter.DateTo.Month, leadFilter.DateTo.Day, 23, 59, 59);

                leadFilter.DateFrom = _settingService.GetUTCDate(leadFilter.DateFrom);
                leadFilter.DateTo = _settingService.GetUTCDate(leadFilter.DateTo);

                
                var leads = _leadMainService.GetLeadsAll(leadFilter.DateFrom,
                                                                          leadFilter.DateTo,
                                                                          0,
                                                                          "",
                                                                          string.Join(",", leadFilter.FilterAffiliateIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterAffiliateChannelIds.Where(x => x != 0)),
                                                                          leadFilter.SubId,
                                                                          string.Join(",", leadFilter.FilterBuyerIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterBuyerChannelIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterCampaignIds.Where(x => x != 0)),
                                                                          leadFilter.Status != 0 ? leadFilter.Status : (short)-1,
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          0,
                                                                          "",
                                                                          "",
                                                                          ((leadFilter.Page - 1) * leadFilter.PageSize),
                                                                          leadFilter.PageSize
                                                                          );

                var leadsViewModelList = GetLeadMainContentViewModels(leads);

                return Ok(leadsViewModelList);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("getLeadsCount")]
        public IHttpActionResult GetLeadsCount([FromBody] LeadRequestModel leadFilter)
        {
            if (!_permissionService.Authorize(_viewLeadListKey))
            {
                //return HttpBadRequest("access-denied");
            }
            try
            {
                if (leadFilter == null)
                {
                    return HttpBadRequest("Lead filter model is empty");
                }

                _reportService.ValidateFilters(FilterCampaignIds: leadFilter.FilterCampaignIds,
                    FilterAffiliateIds: leadFilter.FilterAffiliateIds,
                    FilterAffiliateChannelIds: leadFilter.FilterAffiliateChannelIds,
                    FilterBuyerIds: leadFilter.FilterBuyerIds,
                    FilterBuyerChannelIds: leadFilter.FilterBuyerChannelIds, fillBuyers: false);

                if (leadFilter.PageSize == 0)
                    leadFilter.PageSize = 100;

                if (leadFilter.Page == 0)
                    leadFilter.Page = 1;

                leadFilter.DateFrom = new DateTime(leadFilter.DateFrom.Year, leadFilter.DateFrom.Month, leadFilter.DateFrom.Day, 0, 0, 0);
                leadFilter.DateTo = new DateTime(leadFilter.DateTo.Year, leadFilter.DateTo.Month, leadFilter.DateTo.Day, 23, 59, 59);

                leadFilter.DateFrom = _settingService.GetUTCDate(leadFilter.DateFrom);
                leadFilter.DateTo = _settingService.GetUTCDate(leadFilter.DateTo);

                var leadCount = _leadMainService.GetLeadsCount(leadFilter.DateFrom,
                                                                          leadFilter.DateTo,
                                                                          "",
                                                                          string.Join(",", leadFilter.FilterAffiliateIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterAffiliateChannelIds.Where(x => x != 0)),
                                                                          "",
                                                                          string.Join(",", leadFilter.FilterBuyerIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterBuyerChannelIds.Where(x => x != 0)),
                                                                          string.Join(",", leadFilter.FilterCampaignIds.Where(x => x != 0)),
                                                                          leadFilter.Status != 0 ? leadFilter.Status : (short)-1,
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          0,
                                                                          "",
                                                                          ""
                                                                          );

                return Ok(new { count = leadCount });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpGet]
        [Route("getLeadsCsv")]
        public IHttpActionResult GetLeadsCsv([FromUri] DateTime dateFrom,
                                             [FromUri] DateTime dateTo,
                                             [FromUri] short status = -1,
                                             [FromUri] List<long> filterAffiliateIds = null,
                                             [FromUri] List<long> filterAffiliateChannelIds = null,
                                             [FromUri] List<long> filterBuyerIds = null,
                                             [FromUri] List<long> filterBuyerChannelIds = null,
                                             [FromUri] List<long> filterCampaignIds = null)
        {
            if (!_permissionService.Authorize(_viewLeadInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var leads = _leadMainService.GetLeadsAll(dateFrom,
                                                                          dateTo,
                                                                          0,
                                                                          "",
                                                                          string.Join(",", filterAffiliateIds),
                                                                          string.Join(",", filterAffiliateChannelIds),
                                                                          "",
                                                                          string.Join(",", filterBuyerIds),
                                                                          string.Join(",", filterBuyerChannelIds),
                                                                          string.Join(",", filterCampaignIds),
                                                                          status,
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          "",
                                                                          0,
                                                                          "",
                                                                          "",
                                                                          0,
                                                                          int.MaxValue
                                                                        );

                var leadsViewModelList = GetLeadMainContentViewModels(leads);

                var csvOutputString = string.Empty;
                csvOutputString += $"Id,";
                csvOutputString += $"Campaign Name,";
                csvOutputString += $"Affiliate Name,";
                csvOutputString += $"Status,";
                csvOutputString += $"Affiliate Channel Name,";
                csvOutputString += $"Processing Time,";
                csvOutputString += $"Firstname,";
                csvOutputString += $"Lastname,";
                csvOutputString += $"State,";
                csvOutputString += $"Zip,";
                csvOutputString += $"Email,";
                csvOutputString += $"Buyer Name,";
                csvOutputString += $"Buyer ChannelId,";
                csvOutputString += $"Buyer ChannelName,";
                csvOutputString += $"Redirect,";
                csvOutputString += $"Affiliate Price,";
                csvOutputString += $"Buyer Price,";
                csvOutputString += $"Risk Score,";

                foreach (var leadMainContentViewModel in leadsViewModelList)
                {
                    csvOutputString += "\n";
                    csvOutputString += $"{leadMainContentViewModel.Id},";
                    csvOutputString += $"{leadMainContentViewModel.CampaignName},";
                    csvOutputString += $"{leadMainContentViewModel.AffiliateName},";
                    csvOutputString += $"{leadMainContentViewModel.Status},";
                    csvOutputString += $"{leadMainContentViewModel.AffiliateChannelName},";
                    csvOutputString += $"{leadMainContentViewModel.ProcessingTime},";
                    csvOutputString += $"{leadMainContentViewModel.Firstname},";
                    csvOutputString += $"{leadMainContentViewModel.Lastname},";
                    csvOutputString += $"{leadMainContentViewModel.State},";
                    csvOutputString += $"{leadMainContentViewModel.Zip},";
                    csvOutputString += $"{leadMainContentViewModel.Email},";
                    csvOutputString += $"{leadMainContentViewModel.BuyerName},";
                    csvOutputString += $"{leadMainContentViewModel.BuyerChannelName},";
                    csvOutputString += $"{leadMainContentViewModel.Url}";
                    csvOutputString += $"{leadMainContentViewModel.AffiliateProfit},";
                    csvOutputString += $"{leadMainContentViewModel.SoldAmount},";
                    csvOutputString += $"{leadMainContentViewModel.Profit}";
                }

                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(Encoding.UTF8.GetBytes(csvOutputString))
                };
                responseMessage.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"Leads-{dateFrom:dd.MM.YYYY}-{dateTo:dd.MM.YYYY}.csv"
                    };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                return ResponseMessage(responseMessage);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpGet]
        [Route("getLeadById/{id}")]
        public IHttpActionResult GetLeadById(long id)
        {
            if (!_permissionService.Authorize(_viewLeadInfoKey))
            {
                //return HttpBadRequest("access-denied");
            }

            try
            {
                var lm = _leadMainService.GetLeadMainById(id);
                if (lm != null)
                {
                    lm.ViewDate = DateTime.UtcNow;
                    _leadMainService.UpdateLeadMain(lm);
                }

                var leadItem = new LeadItemModel();

                var lead = _leadMainService.GetLeadsAllById(id);
                if (lead == null)
                {
                    return HttpBadRequest($"lead not found by given id {id}");
                }

                short leadStatus = lead.Status;
                if (leadStatus > 4) leadStatus = 2;

                leadItem.LeadId = lead.LeadId;
                leadItem.CreatedAt = lead.Created.HasValue ? lead.Created.Value : DateTime.UtcNow;
                leadItem.AffiliatePrice = lead.AffiliatePrice.HasValue ? lead.AffiliatePrice.Value : 0;
                leadItem.BuyerPrice = lead.BuyerPrice.HasValue ? lead.BuyerPrice.Value : 0;

                if (!string.IsNullOrEmpty(lead.MinpriceStr))
                {
                    leadItem.MinPrices = string.Empty;
                    List<string> minPriceList = lead.MinpriceStr.Split(',').ToList<string>();
                    foreach (string minPriceItem in minPriceList)
                    {
                        if (double.TryParse(minPriceItem, out var dc))
                        {
                            leadItem.MinPrices += dc.ToString("C", CultureInfo.CurrentCulture) + ",";
                        }
                    }

                    if (!string.IsNullOrEmpty(leadItem.MinPrices))
                        leadItem.MinPrices = leadItem.MinPrices.Remove(leadItem.MinPrices.Length - 1);
                }

                // leadItem.MinPrices = !string.IsNullOrEmpty(lead.MinpriceStr) ? lead.MinpriceStr : "";
                
                
                leadItem.LeadStatus = (LeadResponseStatus)leadStatus;
                leadItem.LeadStatusName = Enum.GetName(typeof(LeadResponseStatus), (LeadResponseStatus)leadStatus);
                leadItem.IpAddress = !string.IsNullOrEmpty(leadItem.IpAddress) ? leadItem.IpAddress : "";

                var campaign = _campaignService.GetCampaignById(lead.CampaignId);
                leadItem.CampaignId = campaign == null ? 0 : campaign.Id;
                leadItem.CampaignName = campaign == null ? "" : campaign.Name;

                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(lead.AffiliateChannelId);
                leadItem.AffiliateChannelId = affiliateChannel == null ? 0 : affiliateChannel.Id;
                leadItem.AffiliateChannelName = affiliateChannel == null ? "" : affiliateChannel.Name;

                var affiliate = _affiliateService.GetAffiliateById(lead.AffiliateId, true);
                leadItem.AffiliateId = affiliate == null ? 0 : affiliate.Id;
                leadItem.AffiliateName = affiliate == null ? "" : affiliate.Name;

                var affiliateResponses = _affiliateResponseService.GetAffiliateResponsesByLeadId(id);

                foreach (var response in affiliateResponses)
                {
                    leadItem.AffiliateResponses.Add(new LeadAffiliateResponse
                    {
                        MinPrice = response.MinPrice,
                        ProcessStarted = response.ProcessStartedAt,
                        ResponseSent = response.Created,
                        ProcessingTime = (response.Created - response.ProcessStartedAt).Milliseconds,
                        Response = response.Response
                    });
                }

                /*if (affiliateResponses.Count == 0 && lead.LeadId == 1)
                {
                    leadItem.AffiliateResponses.Add(new LeadAffiliateResponse
                    {
                        MinPrice = 10,
                        ProcessStarted = DateTime.UtcNow,
                        ResponseSent = DateTime.UtcNow,
                        ProcessingTime = 100,
                        Response = "response"
                    });
                }*/


                var list = new List<XmlNode>();
                var leadCommonInfoItemModels = new List<LeadCommonInfoItemModel>();
                try
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(lead.ReceivedData);
                    GetNodes(xmlDoc.DocumentElement, list, leadCommonInfoItemModels, lead.CampaignId);
                }
                catch (Exception e)
                {
                    throw e;
                }

                leadItem.Fields = leadCommonInfoItemModels;

                /*var allowedNodes = _campaignTemplateService.CampaignTemplateAllowedNames(lead.CampaignId);
                foreach (var node in list.Where(x => allowedNodes.Contains(x.Name)))
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.Element)
                        {
                            var prop = leadItem.LeadCommonInf.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == childNode.Name.ToLower());
                            prop?.SetValue(leadItem.LeadCommonInf, childNode.InnerText, null);
                        }
                    }
                }*/

                var leadResponseList = _leadMainResponseService.GetLeadMainResponseByLeadId(id);
                var distinct = new Hashtable();
                var postedFilter = new Hashtable();

                foreach (var response in leadResponseList)
                {
                    if (distinct[response.Id] != null)
                    {
                        if (postedFilter[response.Posted] != null)
                            response.Posted = (distinct[response.Id] as LeadResponse)?.Posted;
                        postedFilter[response.Posted] = 1;
                    }

                    distinct[response.Id] = response;
                }

                foreach (var key in distinct.Values.Cast<LeadResponse>().OrderBy(x => x.Id))
                {
                    leadItem.LeadLogs.Add(new LeadLogModel
                    {
                        BuyerName = key.BuyerName,
                        BuyerId = key.BuyerId,
                        BuyerChannelName = key.BuyerChanelName,
                        BuyerChannelId = key.BuyerChannelId,
                        PostedData = key.Posted,
                        PostedDate = key.Created,
                        Request = key.Posted,
                        ResponseDate = key.ResponseCreated,
                        Response = key.Response,
                        ResponseTime = key.ResponseTime,
                        MinPrice = key.MinPrice,
                        Status = key.Status > 4 ? (short)2 : key.Status,
                        BuyerIconPath = key.IconPath
                    });
                }

                /*if (distinct.Count == 0 && lead.LeadId == 1)
                {
                    leadItem.LeadLogs.Add(new LeadLogModel()
                    {
                        BuyerChannelId = 1,
                        BuyerChannelName = "BuyerChannel 1",
                        BuyerId = 1,
                        BuyerName = "Buyer 1",
                        MinPrice = 10,
                        PostedDate = DateTime.UtcNow,
                        Request = "Request data",
                        Response = "Response data",
                        ResponseDate = DateTime.UtcNow,
                        ResponseTime = 3,
                        Status = 1,
                        BuyerIconPath = null
                    });
                    leadItem.LeadLogs.Add(new LeadLogModel()
                    {
                        BuyerChannelId = 2,
                        BuyerChannelName = "BuyerChannel 2",
                        BuyerId = 2,
                        BuyerName = "Buyer 2",
                        MinPrice = 10,
                        PostedDate = DateTime.UtcNow,
                        Request = "Request data",
                        Response = "Response data",
                        ResponseDate = DateTime.UtcNow,
                        ResponseTime = 3,
                        Status = 1,
                        BuyerIconPath = null
                    });
                }*/

                var leadDuplicateList =
                    _leadContentDuplicateService.GetLeadContentDublicateBySSN(id, lead.Ssn ?? "");

                if (leadDuplicateList.Any())
                {
                    if (leadDuplicateList.Any(x => x.Id != id))
                    {
                        leadItem.LeadDuplicateMonitors.Add(new LeadDuplicateMonitorModel()
                        {
                            LeadId = lead.LeadId,
                            Created = lead.Created,
                            AffiliateId = lead.AffiliateId,
                            AffiliateName = affiliate.Name,
                            RequestedAmount = lead.RequestedAmount,
                            NetMonthlyIncome = lead.NetMonthlyIncome,
                            PayFrequency = lead.PayFrequency,
                            DirectDeposit = lead.Directdeposit,
                            Email = lead.Email,
                            HomePhone = lead.HomePhone,
                            Ip = lead.Ip
                        });
                    }

                    foreach (var duplicate in leadDuplicateList)
                    {
                        leadItem.LeadDuplicateMonitors.Add(new LeadDuplicateMonitorModel()
                        {
                            LeadId = duplicate.LeadId,
                            Created = duplicate.Created,
                            AffiliateId = duplicate.AffiliateId,
                            AffiliateName = duplicate.AffiliateName,
                            RequestedAmount = duplicate.RequestedAmount,
                            NetMonthlyIncome = duplicate.NetMonthlyIncome,
                            PayFrequency = duplicate.PayFrequency,
                            DirectDeposit = duplicate.Directdeposit,
                            Email = duplicate.Email,
                            HomePhone = duplicate.HomePhone,
                            Ip = duplicate.Ip
                        });
                    }
                }
                
                
                /*if (leadItem.LeadDuplicateMonitors.Count == 0 && lead.LeadId == 1)
                {
                    leadItem.LeadDuplicateMonitors.Add(new LeadDuplicateMonitorModel()
                    {
                        AffiliateId = lead.AffiliateId,
                        AffiliateName = affiliate.Name,
                        Created = lead.Created.HasValue ? lead.Created.Value : DateTime.UtcNow,
                        HomePhone = lead.HomePhone,
                        Ip = lead.Ip,
                        LeadId = lead.LeadId,
                        NetMonthlyIncome = lead.NetMonthlyIncome,
                        PayFrequency = lead.PayFrequency,
                        RequestedAmount = lead.RequestedAmount,
                    });
                }*/

                leadItem.LeadJourneys.Add(new LeadJourneyModel
                {
                    Name = affiliate.Name,
                    ChannelName = affiliateChannel.Name,
                    DateTime = lead.Created ?? DateTime.UtcNow,
                    Action = LeadActionType.Received,
                    Type = (short)UserTypes.Affiliate,
                    Data = lead.Minprice.ToString()
                });

                foreach (var lr in leadResponseList)
                {
                    var buyer = _buyerService.GetBuyerById(lr.BuyerId);
                    var buyerChannel = _buyerChannelService.GetBuyerChannelById(lr.BuyerChannelId);

                    leadItem.LeadJourneys.Add(new LeadJourneyModel
                    {
                        Name = buyer.Name,
                        ChannelName = buyerChannel.Name,
                        Type = (short)UserTypes.Buyer,
                        Action = LeadActionType.Posted,
                        DateTime = lr.Created,
                        Data = ((LeadResponseStatus)lr.Status).ToString()
                    });
                }

                foreach (var ar in affiliateResponses)
                {
                    leadItem.LeadJourneys.Add(new LeadJourneyModel
                    {
                        Name = affiliate.Name,
                        ChannelName = affiliateChannel.Name,
                        DateTime = ar.Created,
                        Action = LeadActionType.Responsed,
                        Type = (short)UserTypes.Affiliate,
                        Data = ar.Message
                    });
                }

                leadItem.RedirectUrl = _redirectUrlService.GetRedirectUrlByLeadId(id);

                var NextAndPrev = _leadMainService.GetNextPrevLeadId(id);
                leadItem.nextLeadId = NextAndPrev[0];
                leadItem.prevLeadId = NextAndPrev[1];

                return Ok(leadItem);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        private LeadMainContentDetailsViewModel GetLeadMainContentDetailsViewModel(LeadMainContent leadMainContent)
        {
            var leadMainContentModel = leadMainContent.GetDetailsViewModel();

            if (leadMainContent.BuyerId != null)
            {
                var buyer = _buyerService.GetBuyerById((long)leadMainContent.BuyerId, true);
                if (buyer != null)
                {
                    leadMainContentModel.Buyer = buyer.GetViewModel();
                }
            }

            if (leadMainContent.BuyerChannelId != null)
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById((long)leadMainContent.BuyerChannelId);
                if (buyerChannel != null)
                {
                    leadMainContentModel.BuyerChannel = buyerChannel.GetViewModel();
                }
            }

            var campaign = _campaignService.GetCampaignById(leadMainContent.CampaignId);
            if (campaign != null && !string.IsNullOrWhiteSpace(campaign.Name))
            {
                leadMainContentModel.CampaignName = campaign.Name;
            }

            var affiliate = _affiliateService.GetAffiliateById(leadMainContent.AffiliateId, true);
            if (affiliate != null)
            {
                leadMainContentModel.Affiliate = affiliate.GetViewModel();
            }

            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(leadMainContent.AffiliateChannelId);
            if (affiliateChannel != null)
            {
                leadMainContentModel.AffiliateChannel = affiliateChannel.GetViewModel();
            }

            var affiliateResponses = _affiliateResponseService.GetAffiliateResponsesByLeadId(leadMainContent.Id);
            if (affiliateResponses != null)
            {
                foreach (var affiliateResponse in affiliateResponses)
                {
                    leadMainContentModel.AffiliateResponses.Add(affiliateResponse.GetViewModel());
                }
            }

            var leadResponseList = _leadMainResponseService.GetLeadMainResponseByLeadId(leadMainContent.Id);
            if (leadResponseList != null)
            {
                foreach (var leadResponse in leadResponseList)
                {
                    leadMainContentModel.LeadResponses.Add(leadResponse.GetViewModel());
                }
            }

            var leadDuplicateList = _leadContentDuplicateService.GetLeadContentDublicateBySSN(leadMainContent.Id,
                !string.IsNullOrEmpty(leadMainContent.Ssn) ? leadMainContent.Ssn : "");
            if (leadDuplicateList != null)
            {
                foreach (var leadContentDuplicate in leadDuplicateList)
                {
                    leadMainContentModel.LeadContentDuplicates.Add(leadContentDuplicate.GetViewModel());
                }
            }


            return leadMainContentModel;
        }

        private IList<LeadAdvancedModel> GetLeadMainContentViewModels(IEnumerable<LeadMainContent> leads)
        {
            var leadMainContentList = new List<LeadAdvancedModel>();

            var setting = _settingService.GetSetting("TimeZoneStr");
            if (setting == null)
            {
                setting = _settingService.GetSetting("TimeZone");
            }

            foreach (var lead in leads)
            {
                var affiliateName = _affiliateService.GetAffiliateById(lead.AffiliateId, true).Name;
                var affiliateChannelName = _affiliateChannelService.GetAffiliateChannelById(lead.AffiliateChannelId, true).Name;
                var buyerName = lead.BuyerId != null && lead.BuyerChannelId != null ? _buyerService.GetBuyerById((long)lead.BuyerId, true).Name : "";
                var buyerChannelName = lead.BuyerChannelId != null ? _buyerChannelService.GetBuyerChannelById((long)lead.BuyerChannelId, true).Name : "";
                var campaignName = _campaignService.GetCampaignById(lead.CampaignId, true).Name;

                decimal affiliatePrice = lead.AffiliatePrice.HasValue ? lead.AffiliatePrice.Value : 0;
                decimal buyerPrice = lead.BuyerPrice.HasValue ? lead.BuyerPrice.Value : 0;

                bool? isRedirected = null;

                if (lead.Status == 1)
                {
                    if (lead.Clicked.HasValue && lead.Clicked.Value)
                        isRedirected = true;
                    else
                        isRedirected = false;
                }

                leadMainContentList.Add(item: new LeadAdvancedModel
                {
                    Id = lead.Id,
                    Created = lead.Created != null ? _settingService.GetTimeZoneDate((DateTime)lead.Created, tzSettings: setting) : (DateTime?) null,
                    UpdateDate = lead.UpdateDate != null ? _settingService.GetTimeZoneDate((DateTime)lead.UpdateDate, tzSettings: setting) : (DateTime?)null,
                    Email = lead.Email,
                    Firstname = lead.Firstname,
                    Lastname = lead.Lastname,
                    State = lead.State,
                    Zip = lead.Zip,
                    AffiliateName = affiliateName,
                    AffiliateChannelName = affiliateChannelName,
                    BuyerName = buyerName,
                    BuyerChannelName = buyerChannelName,
                    CampaignName = campaignName,
                    ProcessingTime = lead.ProcessingTime,
                    Status = lead.Status,
                    Url = lead.Clicked.HasValue && lead.Clicked.Value && lead.Status == (short)LeadResponseStatus.Sold ? lead.Url : "",
                    AffiliateProfit = affiliatePrice,
                    SoldAmount = buyerPrice,
                    Profit = buyerPrice - affiliatePrice,
                    DuplicateIndicator = lead.Warning,
                    IsRedirected = isRedirected
                });
            }

            return leadMainContentList;
        }


        private IList<LeadAdvancedModel> GetLeadDemoContentViewModels(IEnumerable<LeadMainContent> leads, LeadRequestModel leadFilter)
        {
            var leadMainContentList = new List<LeadAdvancedModel>();

            foreach (var lead in leads)
            {
                if (
                    (
                        leadFilter.FilterAffiliateIds.Count == 0 ||
                        (leadFilter.FilterAffiliateIds.Count > 0 &&
                         leadFilter.FilterAffiliateIds.Contains(lead.AffiliateId)) ||
                        (leadFilter.FilterAffiliateIds.Count == 1 && leadFilter.FilterAffiliateIds[0] == 0)
                    ) &&
                    (
                        leadFilter.FilterAffiliateChannelIds.Count == 0 ||
                        (leadFilter.FilterAffiliateChannelIds.Count > 0 &&
                         leadFilter.FilterAffiliateChannelIds.Contains(lead.AffiliateChannelId)) ||
                        (leadFilter.FilterAffiliateChannelIds.Count == 1 && leadFilter.FilterAffiliateChannelIds[0] == 0)
                    ) &&
                    (
                        leadFilter.FilterBuyerIds.Count == 0 ||
                        (leadFilter.FilterBuyerIds.Count > 0 && lead.BuyerId != null &&
                         leadFilter.FilterBuyerIds.Contains((long)lead.BuyerId)) ||
                        (leadFilter.FilterBuyerIds.Count == 1 && leadFilter.FilterBuyerIds[0] == 0)
                    ) &&
                    (
                        leadFilter.FilterBuyerChannelIds.Count == 0 ||
                        (leadFilter.FilterBuyerChannelIds.Count > 0 && lead.BuyerChannelId != null &&
                         leadFilter.FilterBuyerChannelIds.Contains((long)lead.BuyerChannelId)) ||
                        (leadFilter.FilterBuyerChannelIds.Count == 1 && leadFilter.FilterBuyerChannelIds[0] == 0)
                    ) &&
                    (
                        leadFilter.FilterCampaignIds.Count == 0 ||
                        (leadFilter.FilterCampaignIds.Count > 0 &&
                         leadFilter.FilterCampaignIds.Contains(lead.CampaignId)) ||
                        (leadFilter.FilterCampaignIds.Count == 1 && leadFilter.FilterCampaignIds[0] == 0)
                    ) && 
                    (
                        leadFilter.Status == 0 || leadFilter.Status == lead.Status
                    )
                )
                {
                    var affiliateName = _affiliateService.GetAffiliateById(lead.AffiliateId, true)?.Name ?? "";
                    var affiliateChannelName =
                        _affiliateChannelService.GetAffiliateChannelById(lead.AffiliateChannelId, true)?.Name ?? "";
                    var buyerName = lead.BuyerId != null
                        ? _buyerService.GetBuyerById((long) lead.BuyerId, true)?.Name
                        : "" ?? "";
                    var buyerChannelName = lead.BuyerChannelId != null
                        ? _buyerChannelService.GetBuyerChannelById((long) lead.BuyerChannelId, true)?.Name
                        : "" ?? "";
                    var campaignName = _campaignService.GetCampaignById(lead.CampaignId, true)?.Name ?? "";

                    leadMainContentList.Add(new LeadAdvancedModel
                    {
                        Id = lead.Id,
                        Created = lead.Created,
                        UpdateDate = lead.UpdateDate,
                        Email = lead.Email,
                        Firstname = lead.Firstname,
                        Lastname = lead.Lastname,
                        State = lead.State,
                        Zip = lead.Zip,
                        AffiliateName = affiliateName,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerName = buyerName,
                        BuyerChannelName = buyerChannelName,
                        CampaignName = campaignName,
                        ProcessingTime = lead.ProcessingTime,
                        Status = lead.Status,
                        Url = lead.Clicked.HasValue && lead.Clicked.Value &&
                              lead.Status == (short) LeadResponseStatus.Sold
                            ? lead.Url
                            : "",
                        AffiliateProfit = lead.AffiliatePrice,
                        SoldAmount = lead.BuyerPrice,
                        Profit = lead.BuyerPrice - lead.AffiliatePrice
                    });
                }
            }

            return leadMainContentList;
        }

        public void ParseDates(string dateFromStr, string dateToStr, out DateTime dateFrom, out DateTime dateTo)
        {
            var userNow = _settingService.GetTimeZoneDate(DateTime.Now, _appContext.AppUser);

            dateFrom = dateTo = userNow;

            if (!string.IsNullOrWhiteSpace(dateFromStr))
            {
                dateFrom = DateTime.ParseExact(dateFromStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrWhiteSpace(dateToStr))
            {
                dateTo = DateTime.ParseExact(dateToStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);
        }

        private void GetNodes(XmlNode parent, List<XmlNode> xmlNodes, List<LeadCommonInfoItemModel> leadCommonInfoItemModels, long campaignId, List<CampaignField> campaignFields = null)
        {
            if (campaignFields == null)
                campaignFields = (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(campaignId);

            var b = true;
            XmlAttribute attr = null;

            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    b = false;
                    GetNodes(node, xmlNodes, leadCommonInfoItemModels, campaignId, campaignFields);

                    var ct = campaignFields.Where(x => x.TemplateField == node.Name).FirstOrDefault(); //_campaignTemplateService.GetCampaignTemplateBySectionAndName("", node.Name, campaignId);

                    if (node.Attributes != null && node.Attributes["decrypted"] == null)
                    {
                        if (parent.OwnerDocument != null)
                            attr = parent.OwnerDocument.CreateAttribute("decrypted");

                        if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value &&
                            ct.Validator != 5 && ct.Validator != 6 && ct.Validator != 12 &&
                            ct.TemplateField.ToLower() != "ssn" && ct.TemplateField.ToLower() != "dlnumber" &&
                            ct.TemplateField.ToLower() != "accountnumber")
                        {
                            if (attr != null)
                            {
                                attr.Value = Helper.Decrypt(node.InnerText);
                            }
                        }


                        if (attr != null)
                            node.Attributes.Append(attr);
                    }

                }
            }
            if (b)
            {
                attr = parent.OwnerDocument?.CreateAttribute("decrypted");

                var leadCommonInfoItemModel = new LeadCommonInfoItemModel();
                leadCommonInfoItemModel.Name = parent.Name;
                leadCommonInfoItemModel.Value = parent.InnerText;
                leadCommonInfoItemModel.EncryptedValue = "";

                if (!xmlNodes.Contains(parent))
                {
                    var ct = campaignFields.Where(x => x.TemplateField == parent.Name).FirstOrDefault();//_campaignTemplateService.GetCampaignTemplateBySectionAndName("", parent.Name, campaignId);

                    if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value && ct.Validator != 5 &&
                        ct.Validator != 6 && ct.Validator != 12 && ct.TemplateField.ToLower() != "ssn" &&
                        ct.TemplateField.ToLower() != "dlnumber" && ct.TemplateField.ToLower() != "accountnumber")
                    {
                        attr.Value = Helper.Decrypt(parent.InnerText);
                        leadCommonInfoItemModel.EncryptedValue = attr.Value;
                        leadCommonInfoItemModel.IsEncrypted = true;
                    }

                    if (ct != null && (ct.Validator == 2 || ct.Validator == 16))
                    {
                        if (attr != null && double.TryParse(parent.InnerText, out var dc))
                        {
                            leadCommonInfoItemModel.Value = dc.ToString("C", CultureInfo.CurrentCulture); 
                        }
                    }


                    xmlNodes.Add(parent);
                    parent.Attributes.Append(attr);

                    leadCommonInfoItemModel.SectionName = parent.ParentNode == null ? "" : parent.ParentNode.Name;
                    leadCommonInfoItemModels.Add(leadCommonInfoItemModel);
                }
            }
        }
    }
}