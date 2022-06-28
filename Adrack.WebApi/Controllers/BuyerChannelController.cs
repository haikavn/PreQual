using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Xml;
using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.PlanManagement;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework.Cache;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BuyerChannels;
using Adrack.WebApi.Models.Lead;
using Nager.Date;
using Nager.Date.Model;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/buyerchannel")]
    public class BuyerChannelController : BaseApiController
    {
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IBuyerChannelTemplateService _buyerChannelTemplateService;
        private readonly IBuyerChannelTemplateMatchingService _buyerChannelTemplateMatchingService;
        private readonly IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;
        private readonly IBuyerChannelScheduleService _buyerChannelScheduleService;
        private readonly ICampaignService _campaignService;
        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ISearchService _searchService;
        private readonly IBuyerService _buyerService;
        private readonly IAppContext _appContext;
        private readonly IRepository<Campaign> _campaignRepo;
        private readonly IRepository<Buyer> _buyerRepo;
        private readonly ICountryService _countryService;
        private readonly IPermissionService _permissionService;
        private readonly IPlanService _planService;
        private readonly ISettingService _settingService;
        private readonly IEmailService _emailService;
        private readonly ISmtpAccountService _smtpAccountService;
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;

        private static string _editBuyerChannelPermissionKey {get;set;} = "edit-general-information-buyerchannel";
        private static string _viewBuyerChannelPermissionKey { get; set; } = "view-general-information-buyerchannel";
        private static string _editBuyerChannelAdvancedResponserKey { get; set; } = "edit-advanced-response-settings-buyerchannel";
        private static string _viewBuyerChannelAdvancedResponserKey { get; set; } = "view-advanced-response-settings-buyerchannel";
        private static string _viewBuyerChannelResponseSettingKey { get; set; } = "view-response-settings-buyerchannel";
        private static string _editBuyerChannelResponseSettingKey { get; set; } = "edit-response-settings-buyerchannel";
        private static string _viewBuyerChannelFilterKey { get; set; } = "view-filters-buyerchannel";
        private static string _editBuyerChannelFilterKey { get; set; } = "edit-filters-buyerchannel";
        private static string _viewBuyerChannelGeneralInfoKey { get; set; } = "view-general-information-buyerchannel";
        private static string _editBuyerChannelGeneralInfoKey { get; set; } = "edit-general-information-buyerchannel";
        private static string _viewBuyerChannelScheduleKey { get; set; } = "view-schedule-buyerchannel";
        private static string _editBuyerChannelScheduleKey { get; set; } = "edit-schedule-buyerchannel";
        private static string _viewBuyerChannelIntegrationKey { get; set; } = "view-integration-buyerchannel";
        private static string _editBuyerChannelIntegrationKey { get; set; } = "edit-integration-buyerchannel";

        private string _uploadBuyerIconFolderUrl => $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/Icons/Buyer/";



        public BuyerChannelController(IBuyerChannelService buyerChannelService,
                                      IBuyerChannelTemplateService buyerChannelTemplateService,
                                      IBuyerChannelTemplateMatchingService buyerChannelTemplateMatchingService,
                                      IBuyerChannelFilterConditionService buyerChannelFilterConditionService,
                                      IBuyerChannelScheduleService buyerChannelScheduleService,
                                      ICampaignService campaignService,
                                      ICampaignTemplateService campaignTemplateService,
                                      ISearchService searchService,
                                      IBuyerService buyerService,
                                      IRepository<Campaign> campaignRepo,
                                      IRepository<Buyer> buyerRepo,
                                      IUserService userService,
                                      IAppContext appContext,
                                      IRoleService roleService,
                                      ICountryService countryService,
                                      IPermissionService permissionService,
                                      IPlanService planService,
                                      IAffiliateService affiliateService,
                                      IAffiliateChannelService affiliateChannelService,
                                      ISettingService settingService,
                                      IEmailService emailService,
                                      ISmtpAccountService smtpAccountService,
                                      IEntityChangeHistoryService entityChangeHistoryService) :base()
        {
            _buyerChannelService = buyerChannelService;
            _buyerChannelTemplateService = buyerChannelTemplateService;
            _buyerChannelTemplateMatchingService = buyerChannelTemplateMatchingService;
            _buyerChannelFilterConditionService = buyerChannelFilterConditionService;
            _buyerChannelScheduleService = buyerChannelScheduleService;
            _campaignService = campaignService;
            _campaignTemplateService = campaignTemplateService;
            _searchService = searchService;
            _buyerService = buyerService;
            _campaignRepo = campaignRepo;
            _buyerRepo = buyerRepo;
            _userService = userService;
            _appContext = appContext;
            _roleService = roleService;
            _countryService = countryService;
            _permissionService = permissionService;
            _planService = planService;
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _settingService = settingService;
            _emailService = emailService;
            _smtpAccountService = smtpAccountService;
            _entityChangeHistoryService = entityChangeHistoryService;
        }


        [HttpPost]
        [Route("cloneBuyerChannel")]
        public IHttpActionResult CloneBuyerChannel(WebApi.Models.BuyerChannels.CloneBuyerChannelModel cloneBuyerChannelModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            var buyerChannel = _buyerChannelService.GetBuyerChannelById(cloneBuyerChannelModel.Id);

            if (buyerChannel == null)
            {
                return HttpBadRequest($"buyer channel for given {cloneBuyerChannelModel.Id} id not found");
            }

            var buyerChannelModel = GetBuyerChannelModel(buyerChannel);

            buyerChannelModel.Name = cloneBuyerChannelModel.Name;

            return CreateBuyerChannel(buyerChannelModel);
        }


        [HttpPost]
        [Route("addBuyerChannel")]
        public IHttpActionResult CreateBuyerChannel(WebApi.Models.BuyerChannels.BuyerChannelModel buyerChannelModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());


            try
            {
                var planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if (planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.BuyerChannelLimitReached))
                {
                    return HttpBadRequest($"Buyer channel limit reached.");
                }

                if (buyerChannelModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(buyerChannelModel.BuyerId);

                if (buyer == null)
                {
                    return HttpBadRequest($"buyer is required");
                }

                var campaign = _campaignService.GetCampaignById(buyerChannelModel.CampaignId);
                if (campaign == null)
                {
                    return HttpBadRequest($"campaign is required");
                }

                foreach (var scheduleDayModel in buyerChannelModel.BuyerChannelSchedules)
                {
                    if (!scheduleDayModel.ValidateDailyLimit())
                    {
                        return HttpBadRequest($"Invalid daily limit");
                    }
                }


                if (buyerChannelModel.BuyerChannelFilters != null)
                {
                    if (!ValidationListFilters(buyerChannelModel.BuyerChannelFilters))
                    {
                        return HttpBadRequest($"Invalid filter conditions");
                    }
                }

                buyerChannelModel.CountryId = (buyerChannelModel.CountryId.HasValue && buyerChannelModel.CountryId != 0)? buyerChannelModel.CountryId: 80;
                var buyerChannel = (BuyerChannel)buyerChannelModel;

                if (string.IsNullOrEmpty(buyerChannel.XmlTemplate))
                {
                    buyerChannel.XmlTemplate = campaign.DataTemplate;
                }
                var buyerChannelId = _buyerChannelService.InsertBuyerChannel(buyerChannel);

                var access = new EntityOwnership
                {
                    Id = 0,
                    UserId = _appContext.AppUser.Id,
                    EntityId = buyerChannelId,
                    EntityName = EntityType.BuyerChannel.ToString()
                };
                _userService.InsertEntityOwnership(access);

                var pingTrees =_campaignService.GetPingTrees(buyerChannel.CampaignId);
                if (pingTrees.Count == 0)
                {
                    var pingTree = new PingTree()
                    {
                        CampaignId = buyerChannel.CampaignId,
                        Name = "Main",
                        Quantity = int.MaxValue
                    };

                    _campaignService.InsertPingTree(pingTree);

                    var pingTreeItem = new PingTreeItem()
                    {
                        BuyerChannelId = buyerChannel.Id,
                        GroupNum = 0,
                        OrderNum = 1,
                        IsLocked = false,
                        Percent = 100,
                        PingTreeId = pingTree.Id,
                        Status = EntityStatus.Active
                    };

                    _campaignService.InsertPingTreeItem(pingTreeItem);
                }
                else
                {
                    foreach(var pingTree in pingTrees)
                    {
                        var pingTreeItems = _campaignService.GetPingTreeItems(pingTree.Id);
                        int orderNum = 0;

                        if (pingTreeItems.Count > 0)
                            orderNum = pingTreeItems.OrderByDescending(x => x.OrderNum).Select(x => x.OrderNum).FirstOrDefault();

                        var pingTreeItem = new PingTreeItem()
                        {
                            BuyerChannelId = buyerChannel.Id,
                            GroupNum = 0,
                            OrderNum = orderNum + 1,
                            IsLocked = false,
                            Percent = 100,
                            PingTreeId = pingTree.Id,
                            Status = EntityStatus.Active
                        };

                        _campaignService.InsertPingTreeItem(pingTreeItem);
                    }
                }

                if (buyerChannelModel.XmlTemplate == "")
                {
                    buyerChannelModel.XmlTemplate = campaign.DataTemplate;
                }

                XmlDocument xmlDocument = new XmlDocument();
                if (buyerChannelModel.XmlTemplate == null)
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                else
                {
                    xmlDocument.LoadXml(buyerChannelModel.XmlTemplate);
                }

                Utils.ClearXmlElements(xmlDocument, buyerChannelModel.BuyerChannelFields.Select(x => x.Name + "," + x.SectionName).ToList());

                foreach (var field in buyerChannelModel.BuyerChannelFields)
                {
                    var buyerChannelTemplate = (BuyerChannelTemplate)field;
                    buyerChannelTemplate.BuyerChannelId = buyerChannelId;
                    var fieldId = _buyerChannelTemplateService.InsertBuyerChannelTemplate(buyerChannelTemplate);

                    foreach (var match in field.BuyerChannelFieldMatches)
                    {
                        var fieldMatching = (BuyerChannelTemplateMatching)match;
                        fieldMatching.BuyerChannelTemplateId = fieldId;
                        _buyerChannelTemplateMatchingService.InsertBuyerChannelTemplateMatching(fieldMatching);
                    }

                    if (buyerChannelTemplate.SectionName != null && buyerChannelTemplate.TemplateField != null)
                    {
                        Utils.AddXmlElement(xmlDocument, buyerChannelTemplate.SectionName, buyerChannelTemplate.TemplateField);
                    }
                }

                foreach (var filter in buyerChannelModel.BuyerChannelFilters)
                {
                    var buyerChannelFilter = (BuyerChannelFilterCondition)filter;
                    buyerChannelFilter.BuyerChannelId = buyerChannelId;
                    _buyerChannelFilterConditionService.InsertFilterCondition(buyerChannelFilter);
                }

                foreach (var scheduleDayModel in buyerChannelModel.BuyerChannelSchedules)
                {
                    var buyerChannelScheduleDay = new BuyerChannelScheduleDay()
                    {
                        BuyerChannelId = buyerChannelId,
                        DayValue = scheduleDayModel.DayValue,
                        DailyLimit = scheduleDayModel.DailyLimit,
                        NoLimit = scheduleDayModel.NoLimit,
                        IsEnabled = scheduleDayModel.IsEnabled
                    };

                    _buyerChannelService.InsertBuyerChannelScheduleDay(buyerChannelScheduleDay);

                    foreach (var scheduleTimePeriodModel in scheduleDayModel.ScheduleTimePeriods)
                    {
                        var scheduleTimePeriod = (BuyerChannelScheduleTimePeriod)scheduleTimePeriodModel;
                        scheduleTimePeriod.ScheduleDayId = buyerChannelScheduleDay.Id;
                        _buyerChannelService.InsertBuyerChannelScheduleTimePeriod(scheduleTimePeriod);
                    }
                }

                var country = _countryService.GetCountryById(buyerChannel.CountryId.Value);

                if (country != null && buyerChannelModel.Holidays!=null)
                {
                    foreach (var holidayModel in buyerChannelModel.Holidays)
                    {
                        var buyerChannelHoliday = new BuyerChannelHoliday();

                        buyerChannelModel.HolidayYear = (buyerChannelModel.HolidayYear == 0) ? DateTime.UtcNow.Year : buyerChannelModel.HolidayYear;
                        var holiday = DateSystem.GetPublicHoliday(buyerChannelModel.HolidayYear, country.TwoLetteroCode.ToUpper()).Where(x => x.Date.Year == holidayModel.Year && x.Date.Month == holidayModel.Month && x.Date.Day == holidayModel.Day).FirstOrDefault();

                        buyerChannelHoliday.Name = holiday == null ? holidayModel.Name : holiday.Name;
                        buyerChannelHoliday.BuyerChannelId = buyerChannel.Id;

                        try
                        {
                            buyerChannelHoliday.HolidayDate =
                                new DateTime(holidayModel.Year, holidayModel.Month, holidayModel.Day);
                        }
                        catch
                        {
                            buyerChannelHoliday.HolidayDate = DateTime.UtcNow;
                        }

                        if (holiday == null && buyerChannelHoliday.HolidayDate < DateTime.UtcNow)
                        {
                            //continue;
                        }

                        _buyerChannelService.InsertBuyerChannelHoliday(buyerChannelHoliday);
                    }
                }


                if (buyerChannelModel.BuyerChannelFilters != null)
                {
                    UpdateListFilters(buyerChannelId, buyerChannelModel.BuyerChannelFilters);
                }

                buyerChannel.XmlTemplate = xmlDocument.OuterXml;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok(buyerChannelModel);
            
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateBuyerChannelIntegrationRow/{id}")]
        public IHttpActionResult UpdateBuyerChannelIntegrationRow(long id, [FromBody]BuyerChannelIntegrationModel buyerChannelIntegrationModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannelField = _buyerChannelTemplateService.GetBuyerChannelTemplateById(id);

                if (buyerChannelField == null)
                {
                    return HttpBadRequest($"no buyer channel field was found for given id {id}");
                }

                if (buyerChannelIntegrationModel == null)
                {
                    return HttpBadRequest($"buyer channel integration model is null for given id {id}");
                }

                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelField.BuyerChannelId);

                buyerChannelField.CampaignTemplateId = buyerChannelIntegrationModel.CampaignFieldId;
                buyerChannelField.TemplateField = buyerChannelIntegrationModel.Name;
                buyerChannelField.DefaultValue = buyerChannelIntegrationModel.DefaultValue;
                buyerChannelField.DataFormat = buyerChannelIntegrationModel.GetDataFormat();

                _buyerChannelTemplateMatchingService.DeleteBuyerChannelTemplateMatchingsByTemplateId(id);

                foreach (var match in buyerChannelIntegrationModel.BuyerChannelFieldMatches)
                {
                    var fieldMatching = (BuyerChannelTemplateMatching)match;
                    fieldMatching.BuyerChannelTemplateId = id;
                    _buyerChannelTemplateMatchingService.InsertBuyerChannelTemplateMatching(fieldMatching);
                }

                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(buyerChannel.XmlTemplate);
                }
                catch
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                Utils.AddXmlElement(xmlDocument, buyerChannelField.SectionName, buyerChannelField.TemplateField);
                buyerChannel.XmlTemplate = xmlDocument.OuterXml;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok(buyerChannelField);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateBuyerChannelFields/{id}")]
        public IHttpActionResult UpdateBuyerChannelFields(long id, List<BuyerChannelIntegrationModel> buyerChannelFieldModels)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

            if (buyerChannel == null)
            {
                return HttpBadRequest($"buyer channel for given {id} id not found");
            }

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(buyerChannel.XmlTemplate);
            }
            catch
            {
                xmlDocument.LoadXml("<request></request>");
            }

            Utils.ClearXmlElements(xmlDocument, buyerChannelFieldModels.Select(x => x.Name + "," + x.SectionName).ToList());

            _buyerChannelTemplateService.DeleteBuyerChannelTemplatesByBuyerChannelId(id);
            foreach (var field in buyerChannelFieldModels)
            {
                var buyerChannelTemplate = (BuyerChannelTemplate)field;
                buyerChannelTemplate.BuyerChannelId = id;
                var fieldId = _buyerChannelTemplateService.InsertBuyerChannelTemplate(buyerChannelTemplate);

                foreach (var match in field.BuyerChannelFieldMatches)
                {
                    var fieldMatching = (BuyerChannelTemplateMatching)match;
                    fieldMatching.BuyerChannelTemplateId = fieldId;
                    _buyerChannelTemplateMatchingService.InsertBuyerChannelTemplateMatching(fieldMatching);
                }

                Utils.AddXmlElement(xmlDocument, buyerChannelTemplate.SectionName, buyerChannelTemplate.TemplateField);
            }

            buyerChannel.XmlTemplate = xmlDocument.OuterXml;
            _buyerChannelService.UpdateBuyerChannel(buyerChannel);

            var buyerChannelModel = (Models.BuyerChannels.BuyerChannelModel)buyerChannel;

            var buyerChannelFields = _buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id);
            List<BuyerChannelTemplateMatching> matchings = new List<BuyerChannelTemplateMatching>();
            foreach (var field in buyerChannelFields)
            {
                matchings.AddRange(_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(field.Id));
            }
            buyerChannelModel.FillFields((List<BuyerChannelTemplate>)buyerChannelFields, matchings);

            return Ok(buyerChannelModel);
        }

        [HttpPost]
        [Route("updateBuyerChannelAdvancedResponse/{id}")]
        public IHttpActionResult UpdateBuyerChannelAdvancedResponse(long id, [FromBody]BuyerChannelAdvancedResponseModel buyerChannelAdvancedResponseModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelAdvancedResponserKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!buyerChannelAdvancedResponseModel.ChildChannels.Any())
            {
                ModelState.AddModelError("EmptyChildChannels", "Child channels can not be empty");
            }
            var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);
            if (buyerChannel != null)
            {
                buyerChannel.ChildChannels = string.Join(",", buyerChannelAdvancedResponseModel.ChildChannels.Select(item => item.Id));
                buyerChannel.LeadIdField = buyerChannelAdvancedResponseModel.LeadId.ToString();
                buyerChannel.WinResponseUrl = buyerChannelAdvancedResponseModel.WinResponseUrl;
                buyerChannel.WinResponsePostMethod = buyerChannelAdvancedResponseModel.WinResponsePostMethod;
                buyerChannel.PriceRejectWinResponse = buyerChannelAdvancedResponseModel.PriceReject;
                buyerChannel.EnableCustomPriceReject = buyerChannelAdvancedResponseModel.CustomPriceReject;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);
            }
            return Ok(buyerChannel);
        }

        [HttpGet]
        [Route("getBuyerChannelAdvancedResponse/{id}")]
        public IHttpActionResult GetBuyerChannelAdvancedResponse(long id)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelAdvancedResponserKey))
            {
                return HttpBadRequest("access-denied");
            }
            var result = new BuyerChannelAdvancedResponseModel();

            var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);
            if (buyerChannel != null)
            {
                var childrenIds = buyerChannel.ChildChannels.Split(',').Select(long.Parse).ToArray();
                var childChannels = _buyerChannelService.GetBuyerChannels(childrenIds);
                if (childChannels.Any())
                {
                    result.ChildChannels = childChannels.Select(item => new ChildChannel() { Name = item.Name, Id = item.Id }).ToList();
                }
                result.LeadId = Convert.ToDouble(buyerChannel.LeadIdField);
                result.WinResponseUrl = buyerChannel.WinResponseUrl;
                result.WinResponsePostMethod = buyerChannel.WinResponsePostMethod;
                result.PriceReject = buyerChannel.PriceRejectWinResponse;
                result.CustomPriceReject = buyerChannel.EnableCustomPriceReject ?? false;
                result.TypeId = buyerChannel.AlwaysSoldOption;

                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("updateBuyerChannelResponseSetting/{id}")]
        public IHttpActionResult UpdateBuyerChannelResponseSetting(long id, [FromBody]BuyerChannelResponseSettingModel buyerChannelResponseSettingModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelResponseSettingKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {id}");
                }

                if (buyerChannelResponseSettingModel == null)
                {
                    return HttpBadRequest($"buyer channel response setting model is null for given id {id}");
                }

                buyerChannel.RedirectField = buyerChannelResponseSettingModel.RedirectField;
                buyerChannel.MessageField = buyerChannelResponseSettingModel.MessageField;
                buyerChannel.PriceField = buyerChannelResponseSettingModel.PriceField;
                buyerChannel.AccountIdField = buyerChannelResponseSettingModel.AccountIdField;
                buyerChannel.AcceptedField = buyerChannelResponseSettingModel.SoldFieldName;
                buyerChannel.AcceptedValue = buyerChannelResponseSettingModel.SoldValue;
                buyerChannel.AcceptedFrom = buyerChannelResponseSettingModel.SoldFrom;
                buyerChannel.ErrorField = buyerChannelResponseSettingModel.ErrorFieldName;
                buyerChannel.ErrorValue = buyerChannelResponseSettingModel.ErrorValue;
                buyerChannel.ErrorFrom = buyerChannelResponseSettingModel.ErrorFrom;
                buyerChannel.RejectedField = buyerChannelResponseSettingModel.RedirectField;
                buyerChannel.RejectedValue = buyerChannelResponseSettingModel.RejectedValue;
                buyerChannel.RejectedFrom = buyerChannelResponseSettingModel.RejectedFrom;
                buyerChannel.TestField = buyerChannelResponseSettingModel.TestFieldName;
                buyerChannel.TestValue = buyerChannelResponseSettingModel.TestValue;
                buyerChannel.TestFrom = buyerChannelResponseSettingModel.TestFrom;
                buyerChannel.PriceRejectField = buyerChannelResponseSettingModel.PriceRejectFieldName;
                buyerChannel.PriceRejectValue = buyerChannelResponseSettingModel.PriceRejectValue;
                buyerChannel.Delimeter = buyerChannelResponseSettingModel.Delimeter;
                buyerChannel.LeadIdField = buyerChannelResponseSettingModel.LeadId;
                buyerChannel.WinResponseUrl = buyerChannelResponseSettingModel.WinResponseUrl;
                buyerChannel.WinResponsePostMethod = buyerChannelResponseSettingModel.WinResponsePostMethod;
                buyerChannel.PriceRejectWinResponse = buyerChannelResponseSettingModel.WinResponsePriceReject;
                buyerChannel.AlwaysSoldOption = buyerChannelResponseSettingModel.TypeId;
                buyerChannel.AlwaysBuyerPrice = buyerChannelResponseSettingModel.AlwaysBuyerPrice;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannelById/{id}")]
        public IHttpActionResult GetBuyerChannelById(long id)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
               return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                    return NotFound();

                var buyerChannelModel = GetBuyerChannelModel(buyerChannel);

                return Ok(buyerChannelModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannelByName/{exceptedId}/{name}")]
        public IHttpActionResult GetBuyerChannelByName(string name, long exceptedId)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannels = _buyerChannelService.GetBuyerChannelByContainingName(name, exceptedId);

                if (buyerChannels == null || !buyerChannels.Any())
                    return NotFound();
                var resultValue = buyerChannels.Select(item => new ChildChannel { Id = item.Id, Name = item.Name });
                return Ok(resultValue);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannelsByCampaignId/{campaignId}")]
        [ContentManagementCache("App.Cache.BC.BuyerChannel.")]
        public IHttpActionResult GetBuyerChannelsByCampaignId(long campaignId)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(campaignId);

                if (buyerChannels == null)
                    return NotFound();

                List<Models.BuyerChannels.BuyerChannelModel> buyerChannelModels = new List<Models.BuyerChannels.BuyerChannelModel>();

                foreach (var buyerChannel in buyerChannels)
                {
                    buyerChannelModels.Add((Models.BuyerChannels.BuyerChannelModel)buyerChannel);
                }

                return Ok(buyerChannelModels);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpDelete]
        [Route("deleteBuyerChannel/{id}")]
        public IHttpActionResult DeleteBuyerChannel(long id)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                    return NotFound();

                buyerChannel.Deleted = true;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updatePingTreeItem")]
        public IHttpActionResult UpdatePingTreeItem([FromBody]PingTreeUpdateModel pingTreeUpdateModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(pingTreeUpdateModel.Id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {pingTreeUpdateModel.Id}");
                }

                if (pingTreeUpdateModel == null)
                {
                    return HttpBadRequest($"ping tree update model is null for given id {pingTreeUpdateModel.Id}");
                }

                bool checkPercentageSplits = _buyerChannelService.CheckSumLeadAcceptRateByGroupNum(pingTreeUpdateModel.Group, pingTreeUpdateModel.Id,
                    pingTreeUpdateModel.PercentageSplits);
                if (!checkPercentageSplits)
                {
                    return HttpBadRequest($"percentage splits more than 100% for given group {pingTreeUpdateModel.Group}");
                }


                buyerChannel.OrderNum = pingTreeUpdateModel.Order;
                buyerChannel.GroupNum = pingTreeUpdateModel.Group;
                buyerChannel.LeadAcceptRate = pingTreeUpdateModel.PercentageSplits;
                buyerChannel.Status = pingTreeUpdateModel.Status ? BuyerChannelStatuses.Active : BuyerChannelStatuses.Inactive;
                buyerChannel.IsFixed = pingTreeUpdateModel.IsFixed;
                buyerChannel.AlwaysBuyerPrice = pingTreeUpdateModel.AlwaysBuyerPrice;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updatePingTreeItems")]
        public IHttpActionResult UpdatePingTreeItems([FromBody] List<PingTreeUpdateModel> pingTreeUpdateModels)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                foreach (var pingTreeUpdateModel in pingTreeUpdateModels)
                {
                    var buyerChannel = _buyerChannelService.GetBuyerChannelById(pingTreeUpdateModel.Id);

                    if (buyerChannel == null)
                    {
                        return HttpBadRequest($"no buyer channel was found for given id {pingTreeUpdateModel.Id}");
                    }

                    bool checkPercentageSplits = _buyerChannelService.CheckSumLeadAcceptRateByGroupNum(pingTreeUpdateModel.Group, pingTreeUpdateModel.Id,
                        pingTreeUpdateModel.PercentageSplits);
                    if (!checkPercentageSplits)
                    {
                        return HttpBadRequest($"percentage splits more than 100% for given group {pingTreeUpdateModel.Group}");
                    }

                    //if (pingTreeUpdateModel == null)
                    //{
                    //    return HttpBadRequest($"ping tree update model is null for given id {pingTreeUpdateModel.Id}");
                    //}

                    buyerChannel.OrderNum = pingTreeUpdateModel.Order;
                    buyerChannel.GroupNum = pingTreeUpdateModel.Group;
                    buyerChannel.LeadAcceptRate = pingTreeUpdateModel.PercentageSplits;
                    buyerChannel.Status = (BuyerChannelStatuses)Convert.ToInt16(pingTreeUpdateModel.Status);
                    _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("activateBuyerChannel/{id}/{status}")]
        public IHttpActionResult ActivateBuyerChannel(long id, BuyerChannelStatuses status)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");
                }

                buyerChannel.Status = status;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannelBySearchPattern")]
        public IHttpActionResult GetBuyerChannelBySearchPattern(string inputValue)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            var buyerChannelModels = new List<Models.BuyerChannels.BuyerChannelModel>();
            var buyerChannels = _buyerChannelService.GetAllBuyerChannels();
            foreach (var buyerChannel in buyerChannels)
            {
                var buyerChannelModel = (Models.BuyerChannels.BuyerChannelModel)buyerChannel;
                if (_searchService.CheckPropValue(buyerChannelModel, inputValue))
                {
                    buyerChannelModels.Add(buyerChannelModel);
                }
            }
            return Ok(buyerChannelModels);
        }

        [HttpGet]
        [Route("getBuyerChannelsByBuyerId/{BuyerId}")]
        [ContentManagementCache("App.Cache.BC.BuyerChannel.")]

        public IHttpActionResult GetBuyerChannelsByBuyerId(long buyerId)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByBuyerId(buyerId);

                if (buyerChannels == null)
                    return NotFound();

                var result = (from bc in buyerChannels
                              join c in _campaignRepo.Table on bc.CampaignId equals c.Id
                              join b in _buyerRepo.Table on bc.BuyerId equals b.Id
                              select new
                              {
                                  Name = bc.Name,
                                  Id = bc.Id,
                                  BuyerId = bc.BuyerId,
                                  AlwaysSoldOption = bc.AlwaysSoldOption,
                                  BuyerIcon = b.IconPath,
                                  BuyerName = b.Name,
                                  CampaignName = c.Name,
                                  CampaignId = c.Id,
                                  Status = bc.Status
                              }).ToList();

                BuyerChannelListViewModel buyerChannelViewModel = new BuyerChannelListViewModel();

                foreach (var buyerChannel in result)
                {
                    string iconPath = "";

                    if (!string.IsNullOrEmpty(buyerChannel.BuyerIcon))
                    {
                        string filename = Path.GetFileName(buyerChannel.BuyerIcon);
                        iconPath = $"{_uploadBuyerIconFolderUrl}{filename}";
                    }

                    buyerChannelViewModel.BuyerChannels.Add(new BuyerChannelListItem()
                    {
                        Name = buyerChannel.Name,
                        Id = buyerChannel.Id,
                        BuyerId = buyerChannel.BuyerId,
                        BuyerIcon = iconPath,
                        CampaignName = buyerChannel.CampaignName,
                        Status = (EntityStatus)buyerChannel.Status,
                        BuyerName = buyerChannel.BuyerName,
                        CampaignId = buyerChannel.CampaignId,
                        TypeId = buyerChannel.AlwaysSoldOption
                    });
                }

                return Ok(buyerChannelViewModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannels/{status}")]
        [ContentManagementCache("App.Cache.BC.BuyerChannel.")]

        public IHttpActionResult GetBuyerChannels(BuyerChannelStatuses status = BuyerChannelStatuses.All, [FromUri] List<long> buyerIds = null, [FromUri] List<long> campaignIds = null)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (buyerIds != null && buyerIds.Count == 0)
                    buyerIds = null;

                if (campaignIds != null && campaignIds.Count == 0)
                    campaignIds = null;

                var buyerChannels = _buyerChannelService.GetAllBuyerChannels();

                if (status != BuyerChannelStatuses.All)
                    buyerChannels = buyerChannels.Where(x => x.Status == status).ToList();

                if (buyerChannels == null)
                    return NotFound();

                var result = (from bc in buyerChannels
                              join c in _campaignRepo.Table on bc.CampaignId equals c.Id
                              join b in _buyerRepo.Table on bc.BuyerId equals b.Id
                              where (buyerIds == null || buyerIds.Contains(bc.BuyerId)) &&
                                    (campaignIds == null || campaignIds.Contains(bc.CampaignId))
                              select new
                              {
                                  Name = bc.Name,
                                  Id = bc.Id,
                                  BuyerId = bc.BuyerId,
                                  AlwaysSoldOption = bc.AlwaysSoldOption,
                                  BuyerIcon = b.IconPath,
                                  BuyerName = b.Name,
                                  CampaignName = c.Name,
                                  CampaignId = c.Id,
                                  Status = bc.Status
                              }).ToList();

                BuyerChannelListViewModel buyerChannelViewModel = new BuyerChannelListViewModel();

                foreach (var buyerChannel in result)
                {
                    string iconPath = "";

                    if (!string.IsNullOrEmpty(buyerChannel.BuyerIcon))
                    {
                        string filename = Path.GetFileName(buyerChannel.BuyerIcon);
                        iconPath = $"{_uploadBuyerIconFolderUrl}{filename}";
                    }

                    var createdBy = string.Empty;
                    var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(buyerChannel.Id, "BuyerChannel", "Added");
                    if (createdHistoryObj != null)
                        createdBy = _userService.GetUserById(createdHistoryObj.UserId)?.Username;

                    var updatedBy = string.Empty;
                    var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(buyerChannel.Id, "BuyerChannel", "Modified");
                    if (updatedHistoryObj != null)
                        updatedBy = _userService.GetUserById(updatedHistoryObj.UserId)?.Username;

                    DateTime? createDate = null;
                    if (createdHistoryObj != null)
                        createDate = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);

                    DateTime? updateDate = null;
                    if (updatedHistoryObj != null)
                        updateDate = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);

                    buyerChannelViewModel.BuyerChannels.Add(new BuyerChannelListItem()
                    {
                        Name = buyerChannel.Name,
                        Id = buyerChannel.Id,
                        BuyerId = buyerChannel.BuyerId,
                        BuyerIcon = iconPath,
                        CampaignName = buyerChannel.CampaignName,
                        Status = (EntityStatus)buyerChannel.Status,
                        BuyerName = buyerChannel.BuyerName,
                        CampaignId = buyerChannel.CampaignId,
                        TypeId = buyerChannel.AlwaysSoldOption,
                        CreatedDate = createDate,
                        CreatedBy = createdBy,
                        UpdatedDate = updateDate,
                        UpdatedBy = updatedBy
                    });
                }

                return Ok(buyerChannelViewModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updateBuyerChannelName")]
        public IHttpActionResult UpdateName([FromBody]BuyerChannelNameUpdateModel buyerChannelNameUpdateModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (string.IsNullOrWhiteSpace(buyerChannelNameUpdateModel.Name))
            {
                return HttpBadRequest("Buyer channel name is required");
            }
            if (buyerChannelNameUpdateModel.Id == 0)
            {
                return HttpBadRequest("Buyer channel id is required");
            }

            if (_buyerChannelService.GetBuyerChannelByName(buyerChannelNameUpdateModel.Name, buyerChannelNameUpdateModel.Id) != null)
            {
                return HttpBadRequest("Buyer channel with the specified name already exists");
            }

            else
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelNameUpdateModel.Id, false);
                if (buyerChannel == null)
                {
                    return HttpBadRequest("Buyer channel with the specified id doesn't exist");
                }
                else
                {
                    buyerChannel.Name = buyerChannelNameUpdateModel.Name;
                    _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                    return Ok(buyerChannelNameUpdateModel);
                }
            }
        }

        [HttpPut]
        [Route("updateBuyerChannelStatus")]
        public IHttpActionResult UpdateStatus([FromBody]BuyerChannelStatusUpdateModel buyerChannelStatusUpdateModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (buyerChannelStatusUpdateModel.Id == 0)
            {
                return HttpBadRequest("Buyer id is required");
            }
            else
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelStatusUpdateModel.Id, false);
                if (buyerChannel == null)
                {
                    return HttpBadRequest("Buyer channel with the specified id doesn't exist");
                }
                else 
                {
                    buyerChannel.Status = buyerChannelStatusUpdateModel.Status;
                    _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                    return Ok(buyerChannelStatusUpdateModel);
                }

            }
        }

        [HttpPost]
        [Route("updateBuyerChannelFilters/{buyerChannelId}")]
        public IHttpActionResult UpdateBuyerChannelFilters(long buyerChannelId, [FromBody] List<BuyerChannelFilterCreateModel> buyerChannelFilterCreateModels)
        {
            if (!_permissionService.Authorize(_editBuyerChannelFilterKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerChannelFilterCreateModels != null)
                {
                    if (!ValidationListFilters(buyerChannelFilterCreateModels))
                    {
                        return HttpBadRequest($"Invalid filter conditions");
                    }
                }

                var affiliateChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId);


                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {buyerChannelId}");
                }

                UpdateListFilters(buyerChannelId, buyerChannelFilterCreateModels);

                return Ok();
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getBuyerChannelNotes/{buyerChannelId}")]
        public IHttpActionResult GetBuyerChannelNotes(long buyerChannelId)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            var result = new List<BuyerChannelNoteModel>();
            var buyerChannelNotes = (List<BuyerChannelNote>)this._buyerChannelService.GetAllBuyerChannelNotesByBuyerChannelId(buyerChannelId);

            foreach (var item in buyerChannelNotes)
            {
                result.Add(new BuyerChannelNoteModel()
                {
                    NoteId = item.Id,
                    BuyerChannelId = item.BuyerChannelId,
                    Created = item.Created,
                    UpdatedDate = _settingService.GetTimeZoneDate(item.Updated.HasValue ? item.Updated.Value : DateTime.UtcNow),
                    Note = item.Note,
                    Title = item.Title,
                });
            }
            result = result.OrderByDescending(item => item.UpdatedDate).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("updateNote")]
        public IHttpActionResult UpdateBuyerChannelNote(BuyerChannelNoteModel model)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (model == null)
            {
                return HttpBadRequest($"Given buyer channel note argument is null");
            }
            model.UpdatedDate = DateTime.UtcNow;
            if (model.NoteId != 0)
            {
                var note = this._buyerChannelService.GetBuyerChannelNoteById(model.NoteId);
                if (note == null)
                {
                    return HttpBadRequest($"Given buyer channel note does not exist");
                }
                note.Note = model.Note;
                note.Updated = model.UpdatedDate;
                note.Title = model.Title;
                this._buyerChannelService.UpdateBuyerChannelNote(note);
                return Ok(note);
            }
            else
            {
                var newNote = new BuyerChannelNote()
                {
                    Created = DateTime.UtcNow,
                    BuyerChannelId = model.BuyerChannelId,
                    Note = model.Note,
                    Updated = null,
                    Title = model.Title,
                };
                newNote.Id = this._buyerChannelService.InsertBuyerChannelNote(newNote);
                if (newNote.Id != 0)
                {
                    var buyerChannel = _buyerChannelService.GetBuyerChannelById(model.BuyerChannelId, false);
                    if (buyerChannel != null)
                    {
                        var buyer = _buyerService.GetBuyerById(buyerChannel.BuyerId, false);
                        if (buyer != null && buyer.ManagerId.HasValue)
                        {
                            var manager = _userService.GetUserById(buyer.ManagerId.Value);
                            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
                            var user = _appContext.AppUser;
                            var body = new StringBuilder();

                            string emailContent = model.NoteId == 0 ? " has a new note " : " note has been updated ";

                            body.AppendLine($"Buyer channel {buyerChannel.Name} {emailContent}: {newNote.Note}<br />");
                            _emailService.SendEmail(smtpSetting, "no-reply@adrack.com", "Adrack", manager.Email, manager.BuiltInName, "New note", body.ToString());
                        }
                    }
                }
                return Ok(newNote);

                return Ok(newNote);
            }
        }

        [HttpDelete]
        [Route("deleteNote/{buyerChannelNoteId}")]
        public IHttpActionResult DeleteBuyerChannelNote(long buyerChannelNoteId)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (buyerChannelNoteId == 0)
            {
                return HttpBadRequest($"Given buyer channel note {buyerChannelNoteId} Id is null");
            }
            else
            {
                var note = this._buyerChannelService.GetBuyerChannelNoteById(buyerChannelNoteId);
                if (note == null)
                {
                    return HttpBadRequest($"Given buyer channel note does not exist");
                }
                this._buyerChannelService.DeleteBuyerChannelNote(note);
            }

            return Ok();
        }

        [HttpPut]
        [Route("updateBuyerChannelGeneralInfo/{id}")]
        public IHttpActionResult UpdateBuyerChannelGeneralInfo(long id, [FromBody] BuyerChannelGeneralInfoUpdateModel buyerChannelGeneralInfoUpdateModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {id}");
                }

                if (buyerChannelGeneralInfoUpdateModel == null)
                {
                    return HttpBadRequest($"buyer channel general information model is null for given id {id}");
                }

                if (buyerChannelGeneralInfoUpdateModel.BuyerId == 0)
                {
                    return HttpBadRequest($"buyer field is required");
                }

                if (string.IsNullOrWhiteSpace(buyerChannelGeneralInfoUpdateModel.Name))
                {
                    return HttpBadRequest("Buyer channel name is required");
                }

                if (_buyerChannelService.GetBuyerChannelByName(buyerChannelGeneralInfoUpdateModel.Name, id, false) != null)
                {
                    return HttpBadRequest("Buyer channel with the specified name already exists");
                }

                var user = _userService.GetUserById(buyerChannelGeneralInfoUpdateModel.NewManagerId);

                if (user == null)
                {
                    return HttpBadRequest($"user not found");
                }

                buyerChannel.Name = buyerChannelGeneralInfoUpdateModel.Name;
                buyerChannel.BuyerId = buyerChannelGeneralInfoUpdateModel.BuyerId;
                buyerChannel.CampaignId = buyerChannelGeneralInfoUpdateModel.CampaignId;
                buyerChannel.Status = buyerChannelGeneralInfoUpdateModel.Status;
                buyerChannel.TimeZone = buyerChannelGeneralInfoUpdateModel.TimeZone;
                buyerChannel.ResponseFormat = buyerChannelGeneralInfoUpdateModel.ResponseFormat;
                buyerChannel.DataFormat = buyerChannelGeneralInfoUpdateModel.DataFormat;
                buyerChannel.RedirectUrl = buyerChannelGeneralInfoUpdateModel.RedirectUrl;
                buyerChannel.Timeout = buyerChannelGeneralInfoUpdateModel.Timeout;
                buyerChannel.NotificationEmail = buyerChannelGeneralInfoUpdateModel.NotificationEmail;
                buyerChannel.PostingUrl = buyerChannelGeneralInfoUpdateModel.PostingUrl;
                buyerChannel.PostingHeaders = buyerChannelGeneralInfoUpdateModel.PostingHeaders;
                buyerChannel.ChangeStatusAfterCount = buyerChannelGeneralInfoUpdateModel.PauseAfterTimeout;
                buyerChannel.StatusChangeMinutes = buyerChannelGeneralInfoUpdateModel.PauseFor;
                buyerChannel.AffiliatePriceOption = buyerChannelGeneralInfoUpdateModel.AffiliatePriceOption;
                buyerChannel.AffiliatePrice = buyerChannelGeneralInfoUpdateModel.AffiliatePrice;
                buyerChannel.BuyerPriceOption = buyerChannelGeneralInfoUpdateModel.BuyerPriceOption;
                buyerChannel.BuyerPrice = buyerChannelGeneralInfoUpdateModel.BuyerPrice;
                buyerChannel.CapReachedNotification = buyerChannelGeneralInfoUpdateModel.CapReachedNotification;
                buyerChannel.StatusAutoChange = buyerChannelGeneralInfoUpdateModel.IsPauseChannel;
                buyerChannel.TimeoutNotification = buyerChannelGeneralInfoUpdateModel.TimeoutNotification;
                buyerChannel.MaxDuplicateDays = buyerChannelGeneralInfoUpdateModel.MaxDuplicateDays;
                buyerChannel.AlwaysSoldOption = buyerChannelGeneralInfoUpdateModel.TypeId;

                buyerChannel.ManagerId = buyerChannelGeneralInfoUpdateModel.NewManagerId;

                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("pauseTheChannel/{id}/{isEnable}")]
        public IHttpActionResult PauseTheChannel(long id, bool isEnable)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {id}");
                }

                buyerChannel.StatusAutoChange = isEnable;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("allowCapReachedNotification/{id}/{isEnable}")]
        public IHttpActionResult AllowCapReachedNotification(long id, bool isEnable)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {id}");
                }

                buyerChannel.CapReachedNotification = isEnable;

                _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("allowTimeOutNotification/{id}/{isEnable}")]
        public IHttpActionResult AllowTimeOutNotification(long id, bool isEnable)
        {
            if (!_permissionService.Authorize(_editBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(id);

                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel was found for given id {id}");
                }

                buyerChannel.TimeoutNotification = isEnable;

                _buyerChannelService.UpdateBuyerChannel(buyerChannel);
                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateBuyerChannelHolidaySettings")]
        public IHttpActionResult UpdateBuyerChannelHolidaySettings(BuyerChannelHolidaySettingsModel model)
        {
            if (!_permissionService.Authorize(_editBuyerChannelIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (model == null)
            {
                return HttpBadRequest($"Given buyer channel note argument is null");
            }

            var buyerChannel = _buyerChannelService.GetBuyerChannelById(model.BuyerChannelId);
            if (buyerChannel == null)
            {
                return HttpBadRequest($"Given buyer channel does not exist");
            }

            var country = _countryService.GetCountryById(model.CountryId);
            if (country == null)
            {
                return HttpBadRequest($"Country does not exist");
            }

            buyerChannel.CountryId = model.CountryId;
            buyerChannel.HolidayAnnualAutoRenew = model.HolidayAnnualAutoRenew;
            buyerChannel.HolidayIgnore = model.HolidayIgnore;
            buyerChannel.HolidayYear = model.HolidayYear;
            _buyerChannelService.UpdateBuyerChannel(buyerChannel);

            if (model.Holidays != null)
            {
                _buyerChannelService.ResetBuyerChannelHolidays(buyerChannel.Id, model.HolidayYear);

                foreach (var h in model.Holidays)
                {
                    var buyerChannelHoliday = new BuyerChannelHoliday();

                    var holiday = DateSystem.GetPublicHoliday(h.Year, country.TwoLetteroCode.ToUpper()).Where(x => x.Date.Year == h.Year && x.Date.Month == h.Month && x.Date.Day == h.Day).FirstOrDefault();

                    buyerChannelHoliday.Name = holiday == null ? h.Name : holiday.Name;
                    buyerChannelHoliday.BuyerChannelId = model.BuyerChannelId;
                    buyerChannelHoliday.HolidayDate = new DateTime(h.Year, h.Month, h.Day, 0, 0, 0);

                    if (holiday == null && buyerChannelHoliday.HolidayDate < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0))
                    {
                        continue;
                    }

                    _buyerChannelService.InsertBuyerChannelHoliday(buyerChannelHoliday);
                }
            }

            return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);
        }

        [HttpPost]
        [Route("createOrUpdateHoliday")]
        public IHttpActionResult CreateOrUpdateHoliday(BuyerChannelHolidayUpdateModel model)
        {
            if (!_permissionService.Authorize(_editBuyerChannelIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (model == null)
            {
                return HttpBadRequest($"Given buyer channel note argument is null");
            }

            var buyerChannel = _buyerChannelService.GetBuyerChannelById(model.BuyerChannelId);
            if (buyerChannel == null)
            {
                return HttpBadRequest($"Buyer channel does not exist");
            }

            var country = _countryService.GetCountryById(buyerChannel.CountryId.HasValue ? buyerChannel.CountryId.Value : 80);
            if (country == null)
            {
                return HttpBadRequest($"Country does not exist");
            }

            var buyerChannelHoliday = _buyerChannelService.GetBuyerChannelHolidayById(model.Id);
            if (buyerChannelHoliday == null)
            {
                buyerChannelHoliday = new BuyerChannelHoliday();
            }

            var holiday = DateSystem.GetPublicHoliday(model.Year, country.TwoLetteroCode.ToUpper()).Where(x => x.Date.Year == model.Year && x.Date.Month == model.Month && x.Date.Day == model.Day).FirstOrDefault();

            buyerChannelHoliday.Name = holiday == null ? model.Name : holiday.Name;
            buyerChannelHoliday.BuyerChannelId = model.BuyerChannelId;
            buyerChannelHoliday.HolidayDate = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);

            if (holiday == null && buyerChannelHoliday.HolidayDate < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0))
            {
                return HttpBadRequest($"Invalid date");
            }

            if (model.Id == 0)
            {
                _buyerChannelService.InsertBuyerChannelHoliday(buyerChannelHoliday);
            }
            else
            {
                _buyerChannelService.UpdateBuyerChannelHoliday(buyerChannelHoliday);

            }

            return Ok(buyerChannelHoliday);
        }

        [HttpPost]
        [Route("deleteHoliday")]
        public IHttpActionResult DeleteHoliday(long id)
        {
            if (!_permissionService.Authorize(_editBuyerChannelIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            var buyerChannelHoliday = _buyerChannelService.GetBuyerChannelHolidayById(id);
            if (buyerChannelHoliday == null)
            {
                return HttpBadRequest($"Holiday not found");
            }

            _buyerChannelService.DeleteBuyerChannelHoliday(buyerChannelHoliday);

            return Ok();
        }

        [HttpGet]
        [Route("getBuyerChannelHolidays")]
        public IHttpActionResult GetBuyerChannelHolidays(long id, long countryId, int year)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            return Ok(GetBuyerChannelHolidayList(id, countryId, year));
        }

        private List<BuyerChannelHolidayModel> GetBuyerChannelHolidayList(long buyerChannelId, long countryId, int year)
        {
            List<BuyerChannelHolidayModel> buyerChannelHolidayModels = new List<BuyerChannelHolidayModel>();

            var country = _countryService.GetCountryById(countryId);
            if (country == null)
            {
                return buyerChannelHolidayModels;
            }

            var holidays = DateSystem.GetPublicHoliday(year, country.TwoLetteroCode.ToUpper());

            List<BuyerChannelHoliday> buyerChannelHolidays = (List<BuyerChannelHoliday>)_buyerChannelService.GetBuyerChannelHolidays(buyerChannelId);

            foreach (var holiday in holidays)
            {
                BuyerChannelHolidayModel buyerChannelHolidayModel = new BuyerChannelHolidayModel();
                buyerChannelHolidayModel.Name = holiday.Name;
                buyerChannelHolidayModel.BuyerChannelId = buyerChannelId;
                buyerChannelHolidayModel.Year = holiday.Date.Year;
                buyerChannelHolidayModel.Month = holiday.Date.Month;
                buyerChannelHolidayModel.Day = holiday.Date.Day;
                buyerChannelHolidayModel.IsCustomHoliday = false;
                var bcHoliday = buyerChannelHolidays.Where(x => x.HolidayDate == holiday.Date).FirstOrDefault();
                buyerChannelHolidayModel.Id = bcHoliday == null ? 0 : bcHoliday.Id;
                buyerChannelHolidayModels.Add(buyerChannelHolidayModel);
                if (bcHoliday != null)
                {
                    buyerChannelHolidays.Remove(bcHoliday);
                }
            }

            foreach(var holiday in buyerChannelHolidays)
            {
                BuyerChannelHolidayModel buyerChannelHolidayModel = new BuyerChannelHolidayModel();
                buyerChannelHolidayModel.Name = holiday.Name;
                buyerChannelHolidayModel.IsCustomHoliday = true;
                buyerChannelHolidayModel.BuyerChannelId = buyerChannelId;
                buyerChannelHolidayModel.Year = holiday.HolidayDate.Year;
                buyerChannelHolidayModel.Month = holiday.HolidayDate.Month;
                buyerChannelHolidayModel.Day = holiday.HolidayDate.Day;
                buyerChannelHolidayModel.Id = holiday.Id;
                buyerChannelHolidayModels.Add(buyerChannelHolidayModel);
            }

            return buyerChannelHolidayModels.OrderBy(x => x.Year).OrderBy(x => x.Month).OrderBy(x => x.Day).ToList();
        }


        [HttpGet]
        [Route("getBuyerChannelScheduleDay/buyerChannelId/day")]
        public IHttpActionResult GetBuyerChannelScheduleDay(long buyerChannelId, short day)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }
            List<BuyerChannelScheduleDayModel> modelOut = new List<BuyerChannelScheduleDayModel>();
            
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannelScheduleDay = _buyerChannelService.GetBuyerChannelScheduleDays(buyerChannelId, day);

                if (buyerChannelScheduleDay == null)
                {
                    return HttpBadRequest($"no buyer channel schedule was found");
                }
                else
                {
                    foreach (BuyerChannelScheduleDay dayitem in buyerChannelScheduleDay)
                    {
                        var schedule = new BuyerChannelScheduleDayModel()
                        {
                            Id = dayitem.Id,
                            DayValue = dayitem.DayValue,
                            DailyLimit = dayitem.DailyLimit,
                            NoLimit = dayitem.NoLimit,
                            IsEnabled = dayitem.IsEnabled
                        };

                        var buyerChannelScheduleTimePeriod = _buyerChannelService.GetBuyerChannelScheduleTimePeriod(schedule.Id);
                        if (buyerChannelScheduleTimePeriod != null)
                        {
                            foreach (BuyerChannelScheduleTimePeriod timePerioditem in buyerChannelScheduleTimePeriod)
                            {
                                schedule.ScheduleTimePeriods.Add(new BuyerChannelScheduleTimePeriodInpModel() {
                                     FromTime = timePerioditem.FromTime,
                                     HourMax = timePerioditem.HourMax,
                                     LeadStatus = timePerioditem.LeadStatus,
                                     PostedWait = timePerioditem.PostedWait,
                                     Price = timePerioditem.Price,
                                     Quantity = timePerioditem.Quantity,
                                     SoldWait = timePerioditem.SoldWait,
                                     ToTime = timePerioditem.ToTime,
                                     Id = timePerioditem.Id
                                });
                            }
                        }

                        modelOut.Add(schedule);
                    }
                }
                
                return Ok(modelOut);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updateBuyerChannelScheduleDay")]
        public IHttpActionResult UpdateBuyerChannelScheduleDay([FromBody]BuyerChannelScheduleDayInpModel inpModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var buyerChannelScheduleDay = _buyerChannelService.GetBuyerChannelScheduleDay(inpModel.Id);
                bool isNew = false;

                if (buyerChannelScheduleDay == null)
                {
                    isNew = true;
                    buyerChannelScheduleDay = new BuyerChannelScheduleDay();
                }

                if (buyerChannelScheduleDay.DayValue < 1 || buyerChannelScheduleDay.DayValue > 7)
                {
                    return HttpBadRequest($"invalid day value");
                }

                buyerChannelScheduleDay.BuyerChannelId = inpModel.BuyerChannelId;
                buyerChannelScheduleDay.DayValue = inpModel.DayValue;
                buyerChannelScheduleDay.DailyLimit = inpModel.DailyLimit;
                buyerChannelScheduleDay.NoLimit = inpModel.NoLimit;
                buyerChannelScheduleDay.IsEnabled = inpModel.IsEnabled;

                if (!isNew)
                    _buyerChannelService.UpdateBuyerChannelScheduleDay(buyerChannelScheduleDay);
                else
                    _buyerChannelService.InsertBuyerChannelScheduleDay(buyerChannelScheduleDay);

                return Ok(buyerChannelScheduleDay);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPut]
        [Route("addBuyerChannelScheduleTimePeriod")]
        public IHttpActionResult InsertBuyerChannelScheduleTimePeriod(long ScheduleDayId, [FromBody]BuyerChannelScheduleTimePeriodInpModel inpModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                BuyerChannelScheduleTimePeriod obj = _buyerChannelService.InsertBuyerChannelScheduleTimePeriod(ScheduleDayId, inpModel.FromTime, inpModel.ToTime, inpModel.Quantity, inpModel.PostedWait, inpModel.SoldWait, inpModel.HourMax, (decimal)inpModel.Price, (short)inpModel.LeadStatus);

                return Ok(obj);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPut]
        [Route("updateBuyerChannelScheduleTimePeriod")]
        public IHttpActionResult UpdateBuyerChannelScheduleTimePeriod([FromBody]BuyerChannelScheduleTimePeriodInpModel inpModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                BuyerChannelScheduleTimePeriod obj = _buyerChannelService.UpdateBuyerChannelScheduleTimePeriod(inpModel.Id, inpModel.FromTime, inpModel.ToTime, inpModel.Quantity, inpModel.PostedWait, inpModel.SoldWait, inpModel.HourMax, (decimal)inpModel.Price, (short)inpModel.LeadStatus);

                return Ok(obj);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpDelete]
        [Route("deleteBuyerChannelScheduleTimePeriod")]
        public IHttpActionResult DeleteBuyerChannelScheduleTimePeriod([FromBody]BuyerChannelScheduleTimePeriodInpModel inpModel)
        {
            if (!_permissionService.Authorize(_editBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                _buyerChannelService.DeleteBuyerChannelScheduleTimePeriod(inpModel.Id);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updateBuyerChannelSchedule/{buyerChannelId}")]
        public IHttpActionResult UpdateBuyerChannelSchedule(long buyerChannelId, [FromBody] IList<BuyerChannelScheduleDayModel> buyerChannelSchedules)
        {
            if (!_permissionService.Authorize(_editBuyerChannelScheduleKey))
            {
                return HttpBadRequest("access-denied");
            }

            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId);
            if (buyerChannel == null)
            {
                return HttpBadRequest($"Buyer channel does not exist");
            }

            foreach (var scheduleDayModel in buyerChannelSchedules)
            {
                if (!scheduleDayModel.ValidateDailyLimit())
                {
                    return HttpBadRequest($"Invalid daily limit");
                }
            }

            try
            {
                _buyerChannelService.ResetBuyerChannelSchedule(buyerChannelId);

                foreach (var scheduleDayModel in buyerChannelSchedules)
                {
                    var buyerChannelScheduleDay = new BuyerChannelScheduleDay()
                    {
                        BuyerChannelId = buyerChannelId,
                        DayValue = scheduleDayModel.DayValue,
                        DailyLimit = scheduleDayModel.DailyLimit,
                        NoLimit = scheduleDayModel.NoLimit,
                        IsEnabled = scheduleDayModel.IsEnabled
                    };

                    _buyerChannelService.InsertBuyerChannelScheduleDay(buyerChannelScheduleDay);

                    foreach (var scheduleTimePeriodModel in scheduleDayModel.ScheduleTimePeriods)
                    {
                        var scheduleTimePeriod = (BuyerChannelScheduleTimePeriod)scheduleTimePeriodModel;
                        scheduleTimePeriod.ScheduleDayId = buyerChannelScheduleDay.Id;
                        _buyerChannelService.InsertBuyerChannelScheduleTimePeriod(scheduleTimePeriod);
                    }
                }

                return Ok((Models.BuyerChannels.BuyerChannelModel)buyerChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }



        [HttpGet]
        [Route("getAttachedAffiliateChannelListById/{id}")]
        public IHttpActionResult GetAttachedAffiliateChannelListById(long id)
        {
            if (!_permissionService.Authorize(_viewBuyerChannelPermissionKey))
            {
                return HttpBadRequest("access-denied");
            }
            var buyerChannel = _buyerChannelService.GetBuyerChannelById(id, false);
            if (buyerChannel == null)
            {
                return HttpBadRequest($"no buyer channel field was found for given id {id}");
            }


            var attachedAffiliateChannels = new List<AttachedAffiliateChannelModel>();
            var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByCampaignId(buyerChannel.CampaignId);

            var atAffiliateChannels = _buyerChannelService.GetAttachedAffiliateChannels(buyerChannel.Id);

            foreach (var channel in affiliateChannels)
            {
                var includedChannel = atAffiliateChannels.FirstOrDefault(x => x.Id == channel.Id);
                attachedAffiliateChannels.Add(new AttachedAffiliateChannelModel
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Included = includedChannel != null ? true : false,
                    Status = channel.Status
                });
            }

            return Ok(attachedAffiliateChannels);
        }

        #region PrivateMethods

        private Models.BuyerChannels.BuyerChannelModel GetBuyerChannelModel(BuyerChannel buyerChannel)
        {
            var buyerChannelModel = (Models.BuyerChannels.BuyerChannelModel)buyerChannel;
            buyerChannelModel.FillFilters((List<BuyerChannelFilterCondition>)_buyerChannelFilterConditionService.GetFilterConditionsByBuyerChannelId(buyerChannel.Id, -1), (List<CampaignField>)_campaignTemplateService.GetCampaignTemplatesByCampaignId(buyerChannel.CampaignId));

            var buyerChannelFields = _buyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id);
            List<BuyerChannelTemplateMatching> matchings = new List<BuyerChannelTemplateMatching>();
            foreach (var field in buyerChannelFields)
            {
                matchings.AddRange(_buyerChannelTemplateMatchingService.GetBuyerChannelTemplateMatchingsByTemplateId(field.Id));
            }
            buyerChannelModel.FillFields((List<BuyerChannelTemplate>)buyerChannelFields, matchings);

            var days = _buyerChannelService.GetBuyerChannelScheduleDays(buyerChannel.Id);

            foreach (var day in days)
            {
                var dayModel = new BuyerChannelScheduleDayModel()
                {
                    DailyLimit = day.DailyLimit,
                    DayValue = day.DayValue,
                    IsEnabled = day.IsEnabled,
                    NoLimit = day.NoLimit,
                    Id = day.Id,
                    ScheduleTimePeriods = new List<BuyerChannelScheduleTimePeriodInpModel>()
                };

                var timePeriods = _buyerChannelService.GetBuyerChannelScheduleTimePeriod(day.Id);

                foreach (var timePeriod in timePeriods)
                {
                    dayModel.ScheduleTimePeriods.Add(new BuyerChannelScheduleTimePeriodInpModel()
                    {
                        FromTime = timePeriod.FromTime,
                        HourMax = timePeriod.HourMax,
                        LeadStatus = timePeriod.LeadStatus,
                        PostedWait = timePeriod.PostedWait,
                        Price = timePeriod.Price,
                        Quantity = timePeriod.Quantity,
                        SoldWait = timePeriod.SoldWait,
                        ToTime = timePeriod.ToTime,
                        Id = timePeriod.Id
                    });
                }

                buyerChannelModel.BuyerChannelSchedules.Add(dayModel);
            }

            buyerChannelModel.ManagerId = buyerChannel.ManagerId.HasValue ? buyerChannel.ManagerId.Value : 0;
            var user = _userService.GetUserById(buyerChannelModel.ManagerId);

            if (user != null && user.Roles != null && user.Roles.Count > 0)
            {
                buyerChannelModel.ManagerRole = user.Roles.ElementAt(0).Name;
            }

            return buyerChannelModel;
        }

        private BuyersChannelViewModel CreateBuyerChannelViewModel(BuyerChannel buyerChannel)
        {
            var buyerName = string.Empty;
            var campaignName = string.Empty;
            var buyer = _buyerService.GetBuyerById(buyerChannel.BuyerId);
            var campaign = _campaignService.GetCampaignById(buyerChannel.CampaignId);
            if (buyer != null)
            {
                buyerName = buyer.Name;
            }
            if (campaign != null)
            {
                campaignName = campaign.Name;
            }

            return new BuyersChannelViewModel
            {
                BuyerId = buyerChannel.BuyerId,
                BuyerName = buyerName,
                BuyersChannelId = buyerChannel.Id,
                CampaignId = buyerChannel.CampaignId,
                Campaign = campaignName,
                BuyersChannelName = buyerChannel.Name,
                BuyersChannelType = (BuyerChannelType)buyerChannel.AlwaysSoldOption,
                Status = (ActivityStatuses)buyerChannel.Status,
            };
        }

        private string UpdateListFilters(long buyerChannelId, List<BuyerChannelFilterCreateModel> filterModels)
        {
            try
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId);

                if (buyerChannel == null)
                {
                    return $"buyer channel not found";
                }

                bool isNew = false;

                Campaign campaign = _campaignService.GetCampaignById(buyerChannel.CampaignId);

                if (campaign == null)
                {
                    return $"campaign channel not found";
                }

                _buyerChannelFilterConditionService.DeleteFilterConditions(buyerChannel.Id);
                var conditions = new List<BuyerChannelFilterCondition>();
                Dictionary<string, List<BuyerChannelFilterCondition>> filterConditions = new Dictionary<string, List<BuyerChannelFilterCondition>>();

                foreach (var conditionModel in filterModels)
                {
                    var filterCondition = (BuyerChannelFilterCondition)conditionModel;

                    filterCondition.BuyerChannelId = buyerChannelId;

                    if (conditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(conditionModel.CampaignFieldName))
                    {
                        var campaignField = campaign.CampaignFields.FirstOrDefault(x => x.TemplateField.ToLower() == conditionModel.CampaignFieldName.ToLower());
                        if (campaignField != null)
                        {
                            filterCondition.CampaignTemplateId = campaignField.Id;
                        }
                    }

                    conditions.Add(filterCondition);

                    string filterConditionKey = $"{filterCondition.Value}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.BuyerChannelId}";

                    var subConditions = new List<BuyerChannelFilterCondition>();

                    if (conditionModel.Children != null)
                    {
                        foreach (var subConditionModel in conditionModel.Children)
                        {
                            var filterSubCondition = (BuyerChannelFilterCondition)subConditionModel;

                            filterSubCondition.BuyerChannelId = buyerChannelId;

                            if (subConditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(subConditionModel.CampaignFieldName))
                            {
                                var campaignField = campaign.CampaignFields.FirstOrDefault(x => x.TemplateField.ToLower() == subConditionModel.CampaignFieldName.ToLower());
                                if (campaignField != null)
                                {
                                    filterSubCondition.CampaignTemplateId = campaignField.Id;
                                }
                            }

                            subConditions.Add(filterSubCondition);
                        }
                    }

                    filterConditions[filterConditionKey] = subConditions;
                }

                buyerChannel.BuyerChannelFilterConditions = conditions;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                foreach (var filterCondition in buyerChannel.BuyerChannelFilterConditions.ToList())
                {
                    string filterConditionKey = $"{filterCondition.Value}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.BuyerChannelId}";

                    if (filterConditions.ContainsKey(filterConditionKey))
                    {
                        foreach (var subFilterCondition in filterConditions[filterConditionKey])
                        {
                            subFilterCondition.ParentId = filterCondition.Id;
                            conditions.Add(subFilterCondition);
                        }
                    }
                }

                buyerChannel.BuyerChannelFilterConditions = conditions;
                _buyerChannelService.UpdateBuyerChannel(buyerChannel);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private bool ValidationListFilters(List<BuyerChannelFilterCreateModel> filterModels)
        {
            try
            {
                var groups = filterModels.GroupBy(item => item.CampaignFieldId);
                foreach (var group in groups)
                {
                    //Console.WriteLine("List with FieldId == {0}", group.Key);
                    var g2 = group;
                    foreach (var item in group)
                    {
                        //Console.WriteLine("    Condition: {0}", item.Condition);
                        foreach (var item2 in g2)
                        {
                            if (item.Condition == (short)Conditions.Contains && item2.Condition == (short)Conditions.NotContains)
                            {
                                if (item.GetValue() == item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals && item2.Condition == (short)Conditions.NotEquals)
                            {
                                if (item.GetValue() == item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.StringByLength && item2.Condition == (short)Conditions.StringByLength)
                            {
                                if (item.GetValue() != item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals && item2.Condition == (short)Conditions.Equals)
                            {
                                if (item.GetValue() != item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals && 
                                     (item2.Condition == (short)Conditions.NumberGreater || 
                                      item2.Condition == (short)Conditions.NumberLess || 
                                      item2.Condition == (short)Conditions.NumberGreaterOrEqual ||
                                      item2.Condition == (short)Conditions.NumberLessOrEqual ||
                                      item2.Condition == (short)Conditions.NumberRange
                                      )
                                     )
                            {
                                return false;
                            }
                            else if (item.Condition == (short)Conditions.NumberGreater && item2.Condition == (short)Conditions.NumberLess)
                            {
                                if (Convert.ToDecimal(item.GetValue()) >= Convert.ToDecimal(item2.GetValue()))
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.NumberGreaterOrEqual && item2.Condition == (short)Conditions.NumberLessOrEqual)
                            {
                                if (Convert.ToDecimal(item.GetValue()) > Convert.ToDecimal(item2.GetValue()))
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.NumberRange && item2.Condition == (short)Conditions.NumberRange)
                            {
                                /*if ((Convert.ToDecimal(item.Values[0].Value1) > Convert.ToDecimal(item2.Values[0].Value2)) || 
                                    (Convert.ToDecimal(item.Values[0].Value2) < Convert.ToDecimal(item2.Values[0].Value1))
                                    )
                                {
                                    return false;
                                }*/
                            }

                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }


}