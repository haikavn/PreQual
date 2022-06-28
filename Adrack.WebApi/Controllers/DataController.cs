using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Data;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Employee;
using Adrack.WebApi.Models.Localization;
using Adrack.WebApi.PdfBuilder.PdfReportCreators;
using Braintree;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : BaseApiPublicController
    {
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IDepartmentService _departmentService;
        private readonly ILocalizedStringService _localizedStringService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IAppContext _appContext;
        private readonly ICacheManager _cacheManager;
        private readonly IVerticalService _verticalService;
        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly ILeadDemoModeService _leadDemoModeService;
        private readonly IReportService _reportService;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IAddonService _addonService;
        private readonly IPermissionService _permissionService;
        private readonly IPlanService _planService;
        private readonly IProfileService _profileService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailService _emailService;
        private readonly IDbContextService _dbContextService;
        private readonly ICampaignService _campaignService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IAffiliateService _affiliateService;

        protected PermoformanceCountersManager PermoformanceCountersManager = new PermoformanceCountersManager();
        public DataController(ICountryService countryService,
                              IStateProvinceService stateProvinceService,
                              IDepartmentService departmentService,
                              ILocalizedStringService localizedStringService,
                              IRoleService roleService,
                              IUserService userService,
                              ISettingService settingService,
                              IAppContext appContext,
                              ICacheManager cacheManager,
                              IVerticalService verticalService,
                              ICampaignTemplateService campaignTemplateService,
                              ILeadDemoModeService leadDemoModeService,
                              IReportService reportService,
                              IBuyerService buyerService,
                              IBuyerChannelService buyerChannelService,
                              IAddonService addonService,
                              IPermissionService permissionService,
                              IPlanService planService,
                              IProfileService profileService,
                              IEncryptionService encryptionService,
                              IEmailService emailService,
                              IDbContextService dbContextService,
                              ICampaignService campaignService,
                              IAffiliateChannelService affiliateChannelService,
                              IAffiliateService affiliateService
            )
        {
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _departmentService = departmentService;
            _localizedStringService = localizedStringService;
            _roleService = roleService;
            _userService = userService;
            _settingService = settingService;
            _appContext = appContext;
            _cacheManager = cacheManager;
            _verticalService = verticalService;
            _campaignTemplateService = campaignTemplateService;
            _leadDemoModeService = leadDemoModeService;
            _reportService = reportService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _addonService = addonService;
            _permissionService = permissionService;
            _planService = planService;
            _profileService = profileService;
            _encryptionService = encryptionService;
            _emailService = emailService;
            _dbContextService = dbContextService;
            _campaignService = campaignService;
            PermoformanceCountersManager.SetUpPerformanceCounters();
            _affiliateChannelService = affiliateChannelService;
            _affiliateService = affiliateService;
        }

        #region HTTP methods


        [HttpPost]
        [Route("importSystem")]
        public IHttpActionResult ImportSystem(string password, [FromBody] string strImportXml)
        {
            if (password != "Spo_kd58_Kht5_udgos")
                return HttpBadRequest("Invalid key");

            try
            {
                Dictionary<string, string> transformCampaignIds = new Dictionary<string, string>();

                var xDoc = XDocument.Parse(strImportXml.Replace("\\r\\n    ", "").Replace("\\r\\n  ", ""));

                if (xDoc.Root != null)
                {
                    var campaignXmlList = xDoc.Root.Elements().Where(x => x.Name == "Campaigns").ToList();

                    foreach (var campaignXml in campaignXmlList)
                    {
                        var prevCampaignId = campaignXml.Elements().FirstOrDefault(x => x.Name == "CampaignId")?.Value;

                        Campaign campaign = new Campaign();

                        campaign.Id = 0;
                        campaign.Name = campaignXml.Elements().FirstOrDefault(x => x.Name == "Name")?.Value;
                        campaign.IsDeleted = Convert.ToBoolean(campaignXml.Elements().FirstOrDefault(x => x.Name == "Deleted")?.Value);
                        campaign.Description = campaignXml.Elements().FirstOrDefault(x => x.Name == "Description")?.Value;
                        
                        Enum.TryParse<ActivityStatuses>(campaignXml.Elements().FirstOrDefault(x => x.Name == "Status")?.Value, out var resultEnumStatus);
                        campaign.Status = resultEnumStatus;
                        
                        campaign.DataTemplate = campaignXml.Elements().FirstOrDefault(x => x.Name == "XmlTemplate")?.Value;
                        campaign.NetworkTargetRevenue = Convert.ToDecimal(campaignXml.Elements().FirstOrDefault(x => x.Name == "NetworkTargetRevenue")?.Value);

                        //var htmlFormId = campaignXml.Elements().FirstOrDefault(x => x.Name == "HtmlFormId")?.Value;
                        //if (!string.IsNullOrEmpty(htmlFormId))
                        //    campaign.HtmlFormId = (Guid)htmlFormId1;
                        
                        campaign.VerticalId = Convert.ToInt64(campaignXml.Elements().FirstOrDefault(x => x.Name == "VerticalId")?.Value);

                        var prioritizedEnabled = campaignXml.Elements().FirstOrDefault(x => x.Name == "PrioritizedEnabled")?.Value;
                        if(!string.IsNullOrEmpty(prioritizedEnabled))
                            campaign.PrioritizedEnabled = Convert.ToBoolean(prioritizedEnabled);

                        campaign.CampaignKey = campaignXml.Elements().FirstOrDefault(x => x.Name == "CampaignKey")?.Value;
                        campaign.Visibility = Convert.ToInt16(campaignXml.Elements().FirstOrDefault(x => x.Name == "Visibility")?.Value);
                        campaign.IsTemplate = Convert.ToBoolean(campaignXml.Elements().FirstOrDefault(x => x.Name == "IsTemplate")?.Value);
                        campaign.NetworkMinimumRevenue = Convert.ToDecimal(campaignXml.Elements().FirstOrDefault(x => x.Name == "NetworkMinimumRevenue")?.Value);
                        
                        Enum.TryParse<CampaignTypes>(campaignXml.Elements().FirstOrDefault(x => x.Name == "CampaignType")?.Value, out var resultEnumCampaignType);
                        campaign.CampaignType = resultEnumCampaignType;
                        
                        campaign.PriceFormat = Convert.ToInt16(campaignXml.Elements().FirstOrDefault(x => x.Name == "PriceFormat")?.Value);
                        campaign.CreatedOn = Convert.ToDateTime(campaignXml.Elements().FirstOrDefault(x => x.Name == "CreatedOn")?.Value);
                        campaign.Start = Convert.ToDateTime(campaignXml.Elements().FirstOrDefault(x => x.Name == "Start")?.Value);

                        var pingTreeCycle = campaignXml.Elements().FirstOrDefault(x => x.Name == "PingTreeCycle")?.Value;
                        if (!string.IsNullOrEmpty(pingTreeCycle))
                            campaign.PingTreeCycle = Convert.ToInt16(pingTreeCycle);

                        campaign.Finish = Convert.ToDateTime(campaignXml.Elements().FirstOrDefault(x => x.Name == "Finish")?.Value);

                        //insert campaign
                        var CampaignId = _campaignService.InsertCampaign(campaign);

                        transformCampaignIds.Add(prevCampaignId, CampaignId.ToString());

                        var campaignTemplateXmlList = campaignXml.Elements().Where(x => x.Name == "CampaignTemplates").ToList();
                        foreach (var campaignTemplateXml in campaignTemplateXmlList)
                        {
                            CampaignField campaignTemplate = new CampaignField();

                            campaignTemplate.Id = 0;
                            campaignTemplate.CampaignId = CampaignId;
                            campaignTemplate.TemplateField = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "TemplateField")?.Value;
                            campaignTemplate.DatabaseField = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "DatabaseField")?.Value;
                            campaignTemplate.Validator = Convert.ToInt16(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "Validator")?.Value);
                            campaignTemplate.SectionName = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "SectionName")?.Value;
                            campaignTemplate.Description = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "Description")?.Value;
                            campaignTemplate.MinLength = Convert.ToInt32(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "MinLength")?.Value);
                            campaignTemplate.MaxLength = Convert.ToInt32(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "MaxLength")?.Value);
                            campaignTemplate.Required = Convert.ToBoolean(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "Required")?.Value);

                            var blackListTypeId = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "BlackListTypeId")?.Value;
                            if (!string.IsNullOrEmpty(blackListTypeId))
                                campaignTemplate.BlackListTypeId = Convert.ToInt32(blackListTypeId);
                            
                            campaignTemplate.IsHash = Convert.ToBoolean(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "IsHash")?.Value);

                            var isHidden = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "IsHidden")?.Value;
                            if (!string.IsNullOrEmpty(isHidden))
                                campaignTemplate.IsHidden = Convert.ToBoolean(isHidden);
                            
                            campaignTemplate.IsFilterable = Convert.ToBoolean(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "IsFilterable")?.Value);
                            campaignTemplate.Label = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "Label")?.Value;
                            campaignTemplate.ColumnNumber = Convert.ToInt32(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "ColumnNumber")?.Value);
                            campaignTemplate.PageNumber = Convert.ToInt32(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "PageNumber")?.Value);
                            campaignTemplate.IsFormField = Convert.ToBoolean(campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "IsFormField")?.Value);
                            campaignTemplate.OptionValues = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "OptionValues")?.Value;

                            var fieldType = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "FieldType")?.Value;
                            if (!string.IsNullOrEmpty(fieldType))
                                campaignTemplate.FieldType = Convert.ToInt16(fieldType);
                            
                            campaignTemplate.FieldFilterSettings = campaignTemplateXml.Elements().FirstOrDefault(x => x.Name == "FieldFilterSettings")?.Value;

                            
                            //insert campaignTemplate
                            _campaignTemplateService.InsertCampaignTemplate(campaignTemplate);
                        }
                    }


                    /////////////////////////////////////////
                    /// Buyers
                    /// /////////////////////////////////////
                    var buyerXmlList = xDoc.Root.Elements().Where(x => x.Name == "Buyers").ToList();

                    foreach (var buyerXml in buyerXmlList)
                    {
                        var buyer = new Buyer();

                        buyer.Id = 0;
                        buyer.CountryId = Convert.ToInt32(buyerXml.Elements().FirstOrDefault(x => x.Name == "CountryId")?.Value);

                        var stateProvinceId = buyerXml.Elements().FirstOrDefault(x => x.Name == "StateProvinceId")?.Value;
                        if (!string.IsNullOrEmpty(stateProvinceId))
                            buyer.StateProvinceId = Convert.ToInt32(stateProvinceId);
                        
                        buyer.Name = buyerXml.Elements().FirstOrDefault(x => x.Name == "Name")?.Value;
                        buyer.AddressLine1 = buyerXml.Elements().FirstOrDefault(x => x.Name == "AddressLine1")?.Value;
                        buyer.AddressLine2 = buyerXml.Elements().FirstOrDefault(x => x.Name == "AddressLine2")?.Value;
                        buyer.City = buyerXml.Elements().FirstOrDefault(x => x.Name == "City")?.Value;
                        buyer.ZipPostalCode = buyerXml.Elements().FirstOrDefault(x => x.Name == "ZipPostalCode")?.Value;
                        buyer.Phone = buyerXml.Elements().FirstOrDefault(x => x.Name == "Phone")?.Value;
                        buyer.Email = buyerXml.Elements().FirstOrDefault(x => x.Name == "Email")?.Value;
                        buyer.CreatedOn = Convert.ToDateTime(buyerXml.Elements().FirstOrDefault(x => x.Name == "CreatedOn")?.Value);
                        buyer.Status = Convert.ToInt16(buyerXml.Elements().FirstOrDefault(x => x.Name == "Status")?.Value);

                        var billFrequency = buyerXml.Elements().FirstOrDefault(x => x.Name == "BillFrequency")?.Value;
                        if (!string.IsNullOrEmpty(billFrequency))
                            buyer.BillFrequency = billFrequency;
                        
                        buyer.FrequencyValue = Convert.ToInt32(buyerXml.Elements().FirstOrDefault(x => x.Name == "FrequencyValue")?.Value);

                        var lastPostedSold = buyerXml.Elements().FirstOrDefault(x => x.Name == "LastPostedSold")?.Value;
                        if (!string.IsNullOrEmpty(lastPostedSold))
                            buyer.LastPostedSold = Convert.ToDateTime(lastPostedSold);

                        var lastPosted = buyerXml.Elements().FirstOrDefault(x => x.Name == "LastPosted")?.Value;
                        if (!string.IsNullOrEmpty(lastPosted))
                            buyer.LastPosted = Convert.ToDateTime(lastPosted);

                        buyer.AlwaysSoldOption = Convert.ToInt16(buyerXml.Elements().FirstOrDefault(x => x.Name == "AlwaysSoldOption")?.Value);
                        buyer.MaxDuplicateDays = Convert.ToInt16(buyerXml.Elements().FirstOrDefault(x => x.Name == "MaxDuplicateDays")?.Value);
                        buyer.DailyCap = Convert.ToInt32(buyerXml.Elements().FirstOrDefault(x => x.Name == "DailyCap")?.Value);
                        buyer.Description = buyerXml.Elements().FirstOrDefault(x => x.Name == "Description")?.Value;

                        var externalId = buyerXml.Elements().FirstOrDefault(x => x.Name == "ExternalId")?.Value;
                        if (!string.IsNullOrEmpty(externalId))
                            buyer.ExternalId = Convert.ToInt32(externalId);

                        var deleted = buyerXml.Elements().FirstOrDefault(x => x.Name == "Deleted")?.Value;
                        if (!string.IsNullOrEmpty(deleted))
                            buyer.Deleted = Convert.ToBoolean(deleted);

                        var isBiWeekly = buyerXml.Elements().FirstOrDefault(x => x.Name == "IsBiWeekly")?.Value;
                        if (!string.IsNullOrEmpty(isBiWeekly))
                            buyer.IsBiWeekly = Convert.ToBoolean(isBiWeekly);

                        var coolOffEnabled = buyerXml.Elements().FirstOrDefault(x => x.Name == "CoolOffEnabled")?.Value;
                        if (!string.IsNullOrEmpty(coolOffEnabled))
                            buyer.CoolOffEnabled = Convert.ToBoolean(coolOffEnabled);

                        var coolOffStart = buyerXml.Elements().FirstOrDefault(x => x.Name == "CoolOffStart")?.Value;
                        if (!string.IsNullOrEmpty(coolOffStart))
                            buyer.CoolOffStart = Convert.ToDateTime(coolOffStart);
                        
                        var coolOffEnd = buyerXml.Elements().FirstOrDefault(x => x.Name == "CoolOffEnd")?.Value;
                        if (!string.IsNullOrEmpty(coolOffEnd))
                            buyer.CoolOffEnd = Convert.ToDateTime(coolOffEnd);

                        var doNotPresentStatus = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentStatus")?.Value;
                        if (!string.IsNullOrEmpty(doNotPresentStatus))
                            buyer.DoNotPresentStatus = Convert.ToInt16(doNotPresentStatus);

                        buyer.DoNotPresentUrl = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentUrl")?.Value;
                        buyer.DoNotPresentResultField = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentResultField")?.Value;
                        buyer.DoNotPresentResultValue = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentResultValue")?.Value;
                        buyer.DoNotPresentRequest = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentRequest")?.Value;
                        buyer.DoNotPresentPostMethod = buyerXml.Elements().FirstOrDefault(x => x.Name == "DoNotPresentPostMethod")?.Value;
                        buyer.CanSendLeadId = Convert.ToBoolean(buyerXml.Elements().FirstOrDefault(x => x.Name == "CanSendLeadId")?.Value);
                        buyer.AccountId = Convert.ToInt32(buyerXml.Elements().FirstOrDefault(x => x.Name == "AccountId")?.Value);
                        buyer.IconPath = buyerXml.Elements().FirstOrDefault(x => x.Name == "IconPath")?.Value;



                        //insert campaign
                        var BuyerId = _buyerService.InsertBuyer(buyer);


                        var buyerChannelXmlList = buyerXml.Elements().Where(x => x.Name == "BuyerChannels").ToList();
                        foreach (var buyerChannelXml in buyerChannelXmlList)
                        {
                            var buyerChannel = new BuyerChannel();

                            buyerChannel.Id = 0;
                            buyerChannel.BuyerId = BuyerId;

                            var prevCampaignId = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "CampaignId")?.Value;
                            if (transformCampaignIds.ContainsKey(prevCampaignId))
                                buyerChannel.CampaignId = Convert.ToInt32(transformCampaignIds[prevCampaignId]);

                            buyerChannel.Name = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Name")?.Value;

                            Enum.TryParse<BuyerChannelStatuses>(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Status")?.Value, out var resultEnumStatus);
                            buyerChannel.Status = resultEnumStatus;
                            
                            buyerChannel.XmlTemplate = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "XmlTemplate")?.Value;
                            buyerChannel.AcceptedField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AcceptedField")?.Value;
                            buyerChannel.AcceptedValue = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AcceptedValue")?.Value;
                            buyerChannel.AcceptedFrom = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AcceptedFrom")?.Value);
                            buyerChannel.ErrorField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ErrorField")?.Value;
                            buyerChannel.ErrorValue = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ErrorValue")?.Value;
                            buyerChannel.ErrorFrom = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ErrorFrom")?.Value);
                            buyerChannel.RejectedField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "RejectedField")?.Value;
                            buyerChannel.RejectedValue = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "RejectedValue")?.Value;
                            buyerChannel.RejectedFrom = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "RejectedFrom")?.Value);
                            buyerChannel.TestField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TestField")?.Value;
                            buyerChannel.TestValue = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TestValue")?.Value;
                            buyerChannel.TestFrom = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TestFrom")?.Value);
                            buyerChannel.RedirectField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "RedirectField")?.Value;
                            buyerChannel.MessageField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "MessageField")?.Value;
                            buyerChannel.PriceField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PriceField")?.Value;
                            buyerChannel.Delimeter = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Delimeter")?.Value;
                            buyerChannel.PriceRejectField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PriceRejectField")?.Value;
                            buyerChannel.PriceRejectValue = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PriceRejectValue")?.Value;
                            buyerChannel.PostingUrl = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PostingUrl")?.Value;
                            buyerChannel.DeliveryMethod = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "DeliveryMethod")?.Value);
                            buyerChannel.Timeout = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Timeout")?.Value);
                            buyerChannel.AfterTimeout = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AfterTimeout")?.Value);
                            buyerChannel.NotificationEmail = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "NotificationEmail")?.Value;

                            var affiliatePrice = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AffiliatePrice")?.Value;
                            if (!string.IsNullOrEmpty(affiliatePrice))
                                buyerChannel.AffiliatePrice = Convert.ToDecimal(affiliatePrice);

                            var buyerPrice = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "BuyerPrice")?.Value;
                            if (!string.IsNullOrEmpty(buyerPrice))
                                buyerChannel.BuyerPrice = Convert.ToDecimal(buyerPrice);
                            
                            buyerChannel.CapReachedNotification = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "CapReachedNotification")?.Value);
                            buyerChannel.TimeoutNotification = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TimeoutNotification")?.Value);
                            buyerChannel.OrderNum = Convert.ToInt32(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "OrderNum")?.Value);
                            buyerChannel.GroupNum = Convert.ToInt32(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "GroupNum")?.Value);
                            buyerChannel.IsFixed = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "IsFixed")?.Value);
                            buyerChannel.AllowedAffiliateChannels = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AllowedAffiliateChannels")?.Value;
                            buyerChannel.DataFormat = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "DataFormat")?.Value);
                            buyerChannel.PostingHeaders = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PostingHeaders")?.Value;

                            Enum.TryParse<BuyerPriceOptions>(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "BuyerPriceOption")?.Value, out var resultEnumBuyerPriceOptions);
                            buyerChannel.BuyerPriceOption = resultEnumBuyerPriceOptions;
                            
                            buyerChannel.AffiliatePriceOption = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AffiliatePriceOption")?.Value);
                            buyerChannel.AlwaysSoldOption = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AlwaysSoldOption")?.Value);
                            buyerChannel.ZipCodeTargeting = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ZipCodeTargeting")?.Value;
                            buyerChannel.StateTargeting = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "StateTargeting")?.Value;
                            buyerChannel.MinAgeTargeting = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "MinAgeTargeting")?.Value);
                            buyerChannel.MaxAgeTargeting = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "MaxAgeTargeting")?.Value);
                            buyerChannel.EnableZipCodeTargeting = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "EnableZipCodeTargeting")?.Value);
                            buyerChannel.EnableStateTargeting = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "EnableStateTargeting")?.Value);
                            buyerChannel.EnableAgeTargeting= Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "EnableAgeTargeting")?.Value);
                            buyerChannel.ZipCodeCondition = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ZipCodeCondition")?.Value);
                            buyerChannel.StateCondition = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "StateCondition")?.Value);
                            buyerChannel.Deleted = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Deleted")?.Value);
                            buyerChannel.Holidays = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "Holidays")?.Value;
                            buyerChannel.RedirectUrl = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "RedirectUrl")?.Value;
                            buyerChannel.MaxDuplicateDays = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "MaxDuplicateDays")?.Value);
                            buyerChannel.TimeZone = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TimeZone")?.Value;
                            buyerChannel.TimeZoneStr = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "TimeZoneStr")?.Value;

                            var leadAcceptRate = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "LeadAcceptRate")?.Value;
                            if (!string.IsNullOrEmpty(leadAcceptRate))
                                buyerChannel.LeadAcceptRate = Convert.ToInt16(leadAcceptRate);

                            var subIdWhiteListEnabled = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "SubIdWhiteListEnabled")?.Value;
                            if (!string.IsNullOrEmpty(subIdWhiteListEnabled))
                                buyerChannel.SubIdWhiteListEnabled = Convert.ToBoolean(subIdWhiteListEnabled);

                            buyerChannel.AccountIdField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "AccountIdField")?.Value;

                            var enableCustomPriceReject = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "EnableCustomPriceReject")?.Value;
                            if (!string.IsNullOrEmpty(enableCustomPriceReject))
                                buyerChannel.EnableCustomPriceReject = Convert.ToBoolean(enableCustomPriceReject);
                            
                            buyerChannel.PriceRejectWinResponse = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "PriceRejectWinResponse")?.Value;

                            var fieldAppendEnabled = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "FieldAppendEnabled")?.Value;
                            if (!string.IsNullOrEmpty(fieldAppendEnabled))
                                buyerChannel.FieldAppendEnabled = Convert.ToBoolean(fieldAppendEnabled);
                            
                            buyerChannel.WinResponseUrl = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "WinResponseUrl")?.Value;
                            buyerChannel.WinResponsePostMethod = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "WinResponsePostMethod")?.Value;
                            buyerChannel.LeadIdField = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "LeadIdField")?.Value;
                            buyerChannel.ChildChannels = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ChildChannels")?.Value;
                            buyerChannel.ResponseFormat = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ResponseFormat")?.Value);
                            buyerChannel.ChannelMappingUniqueId = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ChannelMappingUniqueId")?.Value;

                            var statusExpireDate = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "StatusExpireDate")?.Value;
                            if (!string.IsNullOrEmpty(statusExpireDate))
                                buyerChannel.StatusExpireDate = Convert.ToDateTime(statusExpireDate);

                            buyerChannel.StatusAutoChange = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "StatusAutoChange")?.Value);
                            buyerChannel.StatusChangeMinutes = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "StatusChangeMinutes")?.Value);
                            buyerChannel.ChangeStatusAfterCount = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ChangeStatusAfterCount")?.Value);

                            var currentStatusChangeNum = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "CurrentStatusChangeNum")?.Value;
                            if (!string.IsNullOrEmpty(currentStatusChangeNum))
                                buyerChannel.CurrentStatusChangeNum = Convert.ToInt16(currentStatusChangeNum);

                            var dailyCap = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "DailyCap")?.Value;
                            if (!string.IsNullOrEmpty(dailyCap))
                                buyerChannel.DailyCap = Convert.ToInt16(dailyCap);

                            var capReachEmailCount = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "CapReachEmailCount")?.Value;
                            if (!string.IsNullOrEmpty(capReachEmailCount))
                                buyerChannel.CapReachEmailCount = Convert.ToInt16(capReachEmailCount);

                            var countryId = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "CountryId")?.Value;
                            if (!string.IsNullOrEmpty(countryId))
                                buyerChannel.CountryId = Convert.ToInt16(countryId);

                            var holidayYear = buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "HolidayYear")?.Value;
                            if (!string.IsNullOrEmpty(holidayYear))
                                buyerChannel.HolidayYear = Convert.ToInt16(holidayYear);

                            buyerChannel.HolidayAnnualAutoRenew = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "HolidayAnnualAutoRenew")?.Value);
                            buyerChannel.HolidayIgnore = Convert.ToBoolean(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "HolidayIgnore")?.Value);
                            buyerChannel.ManagerId = Convert.ToInt16(buyerChannelXml.Elements().FirstOrDefault(x => x.Name == "ManagerId")?.Value);


                            //insert campaignTemplate
                            _buyerChannelService.InsertBuyerChannel(buyerChannel);
                        }
                    }




                    /////////////////////////////////////////
                    /// Affiliates
                    /// /////////////////////////////////////
                    var affiliateXmlList = xDoc.Root.Elements().Where(x => x.Name == "Affiliates").ToList();

                    foreach (var affiliateXml in affiliateXmlList)
                    {
                        var affiliate = new Affiliate();

                        affiliate.Id = 0;
                        affiliate.CountryId = Convert.ToInt32(affiliateXml.Elements().FirstOrDefault(x => x.Name == "CountryId")?.Value);

                        var stateProvinceId = affiliateXml.Elements().FirstOrDefault(x => x.Name == "StateProvinceId")?.Value;
                        if (!string.IsNullOrEmpty(stateProvinceId))
                            affiliate.StateProvinceId = Convert.ToInt32(stateProvinceId);

                        affiliate.Name = affiliateXml.Elements().FirstOrDefault(x => x.Name == "Name")?.Value;
                        affiliate.AddressLine1 = affiliateXml.Elements().FirstOrDefault(x => x.Name == "AddressLine1")?.Value;
                        affiliate.AddressLine2 = affiliateXml.Elements().FirstOrDefault(x => x.Name == "AddressLine2")?.Value;
                        affiliate.City = affiliateXml.Elements().FirstOrDefault(x => x.Name == "City")?.Value;
                        affiliate.ZipPostalCode = affiliateXml.Elements().FirstOrDefault(x => x.Name == "ZipPostalCode")?.Value;
                        affiliate.Phone = affiliateXml.Elements().FirstOrDefault(x => x.Name == "Phone")?.Value;
                        affiliate.Email = affiliateXml.Elements().FirstOrDefault(x => x.Name == "Email")?.Value;
                        affiliate.CreatedOn = Convert.ToDateTime(affiliateXml.Elements().FirstOrDefault(x => x.Name == "CreatedOn")?.Value);
                        affiliate.UserId = Convert.ToInt32(affiliateXml.Elements().FirstOrDefault(x => x.Name == "UserId")?.Value);
                        affiliate.ManagerId = Convert.ToInt32(affiliateXml.Elements().FirstOrDefault(x => x.Name == "ManagerId")?.Value);
                        affiliate.Status = Convert.ToInt16(affiliateXml.Elements().FirstOrDefault(x => x.Name == "Status")?.Value);


                        var billFrequency = affiliateXml.Elements().FirstOrDefault(x => x.Name == "BillFrequency")?.Value;
                        if (!string.IsNullOrEmpty(billFrequency))
                            affiliate.BillFrequency = billFrequency;

                        affiliate.FrequencyValue = Convert.ToInt32(affiliateXml.Elements().FirstOrDefault(x => x.Name == "FrequencyValue")?.Value);

                        affiliate.BillWithin = Convert.ToInt32(affiliateXml.Elements().FirstOrDefault(x => x.Name == "BillWithin")?.Value);
                        affiliate.RegistrationIp = affiliateXml.Elements().FirstOrDefault(x => x.Name == "RegistrationIp")?.Value;
                        affiliate.Website = affiliateXml.Elements().FirstOrDefault(x => x.Name == "Website")?.Value;
                        
                        var deleted = affiliateXml.Elements().FirstOrDefault(x => x.Name == "Deleted")?.Value;
                        if (!string.IsNullOrEmpty(deleted))
                            affiliate.IsDeleted = Convert.ToBoolean(deleted);

                        var isBiWeekly = affiliateXml.Elements().FirstOrDefault(x => x.Name == "IsBiWeekly")?.Value;
                        if (!string.IsNullOrEmpty(isBiWeekly))
                            affiliate.IsBiWeekly = Convert.ToBoolean(isBiWeekly);

                        affiliate.WhiteIp =affiliateXml.Elements().FirstOrDefault(x => x.Name == "WhiteIp")?.Value;
                        affiliate.DefaultAffiliatePriceMethod = Convert.ToInt16(affiliateXml.Elements().FirstOrDefault(x => x.Name == "DefaultAffiliatePriceMethod")?.Value);
                        affiliate.DefaultAffiliatePrice = Convert.ToDecimal(affiliateXml.Elements().FirstOrDefault(x => x.Name == "DefaultAffiliatePrice")?.Value);
                        affiliate.IconPath = affiliateXml.Elements().FirstOrDefault(x => x.Name == "IconPath")?.Value;


                        //insert campaign
                        var affiliateId = _affiliateService.InsertAffiliate(affiliate);


                        var affiliateChannelXmlList = affiliateXml.Elements().Where(x => x.Name == "AffiliateChannels").ToList();
                        foreach (var affiliateChannelXml in affiliateChannelXmlList)
                        {
                            var affiliateChannel = new AffiliateChannel();

                            affiliateChannel.Id = 0;
                            affiliateChannel.AffiliateId = affiliateId;

                            var prevCampaignId = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "CampaignId")?.Value;
                            if (transformCampaignIds.ContainsKey(prevCampaignId))
                                affiliateChannel.CampaignId= Convert.ToInt32(transformCampaignIds[prevCampaignId]);

                            affiliateChannel.Name = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "Name")?.Value;
                            affiliateChannel.Status = Convert.ToInt16(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "Status")?.Value);
                            affiliateChannel.XmlTemplate = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "XmlTemplate")?.Value;
                            affiliateChannel.DataFormat = Convert.ToInt16(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "DataFormat")?.Value);
                            affiliateChannel.MinPriceOption = Convert.ToInt16(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "MinPriceOption")?.Value);
                            affiliateChannel.NetworkTargetRevenue = Convert.ToDecimal(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "NetworkTargetRevenue")?.Value);
                            affiliateChannel.NetworkMinimumRevenue = Convert.ToDecimal(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "NetworkMinimumRevenue")?.Value);
                            affiliateChannel.ChannelKey = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "AffiliateChannelKey")?.Value;
                            affiliateChannel.IsDeleted = Convert.ToBoolean(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "Deleted")?.Value);
                            affiliateChannel.AffiliatePriceMethod = Convert.ToInt16(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "AffiliatePriceMethod")?.Value);

                            var affiliatePrice = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "affiliatePrice")?.Value;
                            if (!string.IsNullOrEmpty(affiliatePrice))
                                affiliateChannel.AffiliatePrice = Convert.ToDecimal(affiliatePrice);

                            affiliateChannel.Timeout = Convert.ToInt16(affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "Timeout")?.Value);
                            affiliateChannel.Note = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "Note")?.Value;
                            affiliateChannel.ChannelPassword = affiliateChannelXml.Elements().FirstOrDefault(x => x.Name == "AffiliateChannelPassword")?.Value;
 

                            //insert campaignTemplate
                            _affiliateChannelService.InsertAffiliateChannel(affiliateChannel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("exportSystem")]
        public IHttpActionResult ExportSystem(string password, string filterEntityName = "all", bool filterWithItems = true, long filterEntityId = -1)
        {
            XDocument xDocument = new XDocument(new XElement("System"));

            if (password != "Spo_kd58_Kht5_udgos")
                return HttpBadRequest("Invalid key");

            try
            {
                if (filterEntityName.ToLower() == "all" || filterEntityName.ToLower() == "campaigns")
                {
                    var campaigns = _campaignService.GetAllCampaigns();
                    if (campaigns != null)
                    {
                        foreach (var campaign in campaigns)
                        {
                            if (filterEntityName.ToLower() == "campaigns" && filterEntityId > 0)
                            {
                                if (filterEntityId != campaign.Id)
                                    continue;
                            }


                            var elementCampaign = _leadDemoModeService.SetCampaignXmlValue(campaign);

                            if (filterWithItems)
                            {
                                var campaignTemplates =
                                    _campaignTemplateService.GetCampaignTemplatesByCampaignId(campaign.Id);
                                if (campaignTemplates != null)
                                {
                                    foreach (var campaignTemplate in campaignTemplates)
                                    {
                                        var elementCampaignTemplate =
                                            _leadDemoModeService.SetCampaignTemplateXmlValue(campaignTemplate);
                                        elementCampaign?.Add(elementCampaignTemplate);
                                    }
                                }
                            }

                            xDocument.Root?.Add(elementCampaign);
                        }
                    }
                }


                if (filterEntityName.ToLower() == "all" || filterEntityName.ToLower() == "buyers")
                {
                    var buyers = _buyerService.GetAllBuyersByStatus(EntityFilterByStatus.All);
                    if (buyers != null)
                    {
                        foreach (var buyer in buyers)
                        {
                            if (filterEntityName.ToLower() == "buyers" && filterEntityId > 0)
                            {
                                if (filterEntityId != buyer.Id)
                                    continue;
                            }

                            var elementBuyer = _leadDemoModeService.SetBuyerXmlValue(buyer);

                            if (filterWithItems)
                            {
                                var buyerChannels = _buyerChannelService.GetAllBuyerChannels(buyer.Id);
                                if (buyerChannels != null)
                                {
                                    foreach (var buyerChannel in buyerChannels)
                                    {
                                        var elementBuyerChannel =
                                            _leadDemoModeService.SetBuyerChannelXmlValue(buyerChannel);
                                        elementBuyer?.Add(elementBuyerChannel);
                                    }
                                }
                            }

                            xDocument.Root?.Add(elementBuyer);
                        }
                    }
                }


                if (filterEntityName.ToLower() == "all" || filterEntityName.ToLower() == "affiliates")
                {
                    var affiliates = _affiliateService.GetAllAffiliates();
                    ;
                    if (affiliates != null)
                    {
                        foreach (var affiliate in affiliates)
                        {
                            if (filterEntityName.ToLower() == "affiliates" && filterEntityId > 0)
                            {
                                if (filterEntityId != affiliate.Id)
                                    continue;
                            }

                            var elementAffiliate = _leadDemoModeService.SetAffiliateXmlValue(affiliate);

                            if (filterWithItems)
                            {
                                var affiliateChannels =
                                    _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliate.Id);
                                if (affiliateChannels != null)
                                {
                                    foreach (var affiliateChannel in affiliateChannels)
                                    {
                                        var elementAffiliateChannel =
                                            _leadDemoModeService.SetAffiliateChannelXmlValue(affiliateChannel);
                                        elementAffiliate?.Add(elementAffiliateChannel);
                                    }
                                }
                            }

                            xDocument.Root?.Add(elementAffiliate);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }


            return Ok( xDocument.ToString()); 
        }


        [HttpGet]
        [Route("getSystemDateTime")]
        public IHttpActionResult GetSystemDateTime()
        {
            var dt = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            string timeZoneStr = "";
            Setting tzStr = _settingService.GetSetting("TimeZoneStr");
            if (tzStr != null && !string.IsNullOrEmpty(tzStr.Value))
                timeZoneStr = tzStr.Value;


            return Ok(new { dateTime = dt, timeZone = timeZoneStr });
        }

        [HttpPost]
        [Route("removeEntities")]
        public IHttpActionResult RemoveEntity(RemoveEntitiesModel removeEntitiesModel)
        {
            try
            {
                _settingService.RemoveEntity(new Entity()
                {
                    EntityIds = string.Join(",",removeEntitiesModel.EntityIds),
                    EntityType = removeEntitiesModel.EntityType,
                    Passport = "yes"
                });
                return Ok();
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("countries")]
        public IHttpActionResult GetAllCountries()
        {
            try
            {
                var countries = GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        
        [HttpGet]
        [Route("stateProvinces")]
        public IHttpActionResult GetAllStateProvinces(int countryId)
        {
            try
            {
                var stateProvinces = GetStateProvinces(countryId);
                return Ok(stateProvinces);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getZipCodeLength/{countryId}")]
        public IHttpActionResult GetZipCodeLength(int countryId)
        {
            try
            {

                var country = GetCountries().FirstOrDefault(x=>x.Id==countryId);
                if (country == null)
                {
                    return HttpBadRequest("Wrong country Id is provided");
                }

                return Ok(country.ZipCodeLength);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }




        [HttpGet]
        [Route("departments")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllDepartments()
        {
            try
            {
                var departments = GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("localizedStrings")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllLocalizedStrings()
        {
            try
            {
                var localizedStrings = _localizedStringService.GetAllLocalizedStrings().ToList();
                var localizedStringList = new List<LocalizedStringModel>();
                foreach (var item in localizedStrings)
                {
                    localizedStringList.Add((LocalizedStringModel)item);
                }
                return Ok(localizedStringList);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("roles")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                var roles = GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("userTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllUserTypes()
        {
            try
            {
                var userTypes = GetUserTypes();
                return Ok(userTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("dataFormats")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllDataFormats()
        {
            try
            {
                var dataFormats = GetDataFormats();
                return Ok(dataFormats);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getValidators")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllValidators()
        {
            try
            {
                var validators = GetValidators();
                return Ok(validators);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getConditions")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllConditions([FromUri]byte? validator = null)
        {
            try
            {
                var conditions = GetConditions(validator);
                return Ok(conditions);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAffiliatePriceMethods")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliatePriceMethods()
        {
            try
            {
                var validators = GetAffiliatePriceMethodList();
                return Ok(validators);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAffiliateChannelPriceMethods")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliateChannelPriceMethods()
        {
            try
            {
                var priceMethods = GetAffiliateChannelPriceMethodList();
                return Ok(priceMethods);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("listsByKeys")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetListsByKeys([FromUri]DataKey[] keys, [FromUri] object param = null)
        {
            try
            {
                var list = new List<KeyValuePair<string, object>>();
                foreach (var key in keys)
                {
                    switch (key)
                    {
                        case DataKey.Country:
                            var countries = GetCountries();
                            list.Add(new KeyValuePair<string,object>(key.ToString(), countries));
                            break;
                        case DataKey.StateProvince:
                            int countryId = 0;
                            if (param != null)
                            {
                                int.TryParse(param.ToString(), out countryId);
                            }
                            var stateProvinces = GetStateProvinces(countryId);
                            list.Add(new KeyValuePair<string, object>(key.ToString(), stateProvinces));
                            break;
                        case DataKey.Role:
                            var roles = GetRoles();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), roles));
                            break;
                        case DataKey.UserType:
                            var userTypes = GetUserTypes();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), userTypes));
                            break;
                        case DataKey.Department:
                            var departments = GetDepartments();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), departments));
                            break;
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getSystemFieldsByVerticalId/{verticalId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetSystemFieldsByVerticalId(long verticalId)
        {
            try
            {
                var list = new List<string>();

                /*var fields = _verticalService.GetAllVerticalFields(verticalId);

                foreach(var field in fields)
                {
                    list.Add(field.Name);
                }*/

                PropertyInfo[] properties = typeof(LeadContent).GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    if (pi.Name.ToLower() == "id" ||
                        pi.Name.ToLower() == "leadid" ||
                        pi.Name.ToLower() == "affiliateid" ||
                        pi.Name.ToLower() == "campaigntype" ||
                        pi.Name.ToLower() == "minpricestr" ||
                        pi.Name.ToLower() == "created") continue;

                    list.Add(pi.Name);
                }

                list = list.OrderBy(x => x).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getColumnsVisibility/{page}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetColumnsVisibility(string page)
        {
            var setting = _settingService.GetSetting($"ColumnsVisibility.{page}.{_appContext.AppUser.Id}");
            if (setting == null)
            {
                setting = _settingService.GetSetting($"ColumnsVisibility.{page}");
            }

            if (setting == null)
            {
                return Ok(new List<ColumnsVisibilityModel>());//HttpBadRequest($"no setting was found for given page {page}");
            }

            var columns =  JsonConvert.DeserializeObject<List<ColumnsVisibilityModel>>(setting.Value);
            return Ok(columns);
        }


        [HttpPut]
        [Route("setColumnsVisibility")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult SetColumnsVisibility([FromBody]ColumnVisibilityUpdateModel columnsVisibilityModel)
        {
            var key = $"ColumnsVisibility.{columnsVisibilityModel.Page}.{_appContext.AppUser.Id}";
            var setting = _settingService.GetSetting(key);
            var columns = JsonConvert.SerializeObject(columnsVisibilityModel.ColumnsVisibilities);
            if (setting == null)
            {
                setting = new Setting
                {
                    Id = 0,
                    Key = key,
                    Value = columns,
                    Description = null
                };
                _settingService.InsertSetting(setting);
            }
            else
            {
                setting.Value = columns;
                _settingService.UpdateSetting(setting);
            }

            return Ok();
        }

        [HttpGet]
        [Route("getEntityTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllEntityTypeResults()
        {
            try
            {
                var entityTypes = GetEntityTypes();
                return Ok(entityTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getLeadStatusList")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetLeadStatusResults()
        {
            try
            {
                var status = GetLeadStatusList();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getBillFrequencyList")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult BillFrequencyResults()
        {
            try
            {
                var billFrList = GetBillFrequencyList();
                return Ok(billFrList);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getDateTimeFormats")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetDateTimeFormats()
        {
            //string[] dateFormats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d');

            List<string> list = new List<string>();

            list.Add("MM-dd-yyyy");
            list.Add("MM-dd-yy");
            list.Add("dd-MM-yyyy");
            list.Add("dd-MM-yy");
            list.Add("MM-dd-yyyy");
            list.Add("yyyy-MM-dd");
            list.Add("MM/dd/yyyy");
            list.Add("dd/MM/yyyy");
            list.Add("dd/MM/yy");
            list.Add("dd.MM.yyyy");
            list.Add("dd.MM.yy");
       
            return Ok(list.ToArray());
        }


        [HttpPost]
        [Route("clearCache")]
        public IHttpActionResult ClearCache()
        {
            _cacheManager.Clear();
            return Ok();
        }

        [HttpGet]
        [Route("generateDemoLeads")]
        public IHttpActionResult GenerateDemoLeads(string password)
        {
            if (password.ToLower() != "pogos")
                return HttpBadRequest("Invalid key");
            try
            {
                _leadDemoModeService.LeadPostingSimulation();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }

            return Ok();
        }



        [HttpGet]
        [Route("getBuyerTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllBuyerTypeResults()
        {
            try
            {
                var buyerTypes = GetBuyerTypes();
                return Ok(buyerTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCompanyIndustries")]
        public IHttpActionResult GetAllCompanyIndustry()
        {
            try
            {
                var industries = GetCompanyIndustries();
                return Ok(industries);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getDataFormat")]
        public IHttpActionResult GetDataFormat(Validators validator)
        {
            try
            {
                List<DataFormatTypeModel> dataFormatTypeModels = new List<DataFormatTypeModel>();
                var dataFormatTypeModel = new DataFormatTypeModel();

                switch (validator)
                {
                    case Validators.String:
                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.EditBox;
                        dataFormatTypeModel.Name = "Min length";
                        dataFormatTypeModel.Values.Add("1");
                        dataFormatTypeModels.Add(dataFormatTypeModel);

                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.EditBox;
                        dataFormatTypeModel.Name = "Max length";
                        dataFormatTypeModel.Values.Add("150");
                        dataFormatTypeModels.Add(dataFormatTypeModel);
                        break;
                    case Validators.DateOfBirth:
                    case Validators.DateTime:
                        string[] formats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d');

                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.DropDown;
                        dataFormatTypeModel.Name = "Date formats";
                        dataFormatTypeModel.Values.AddRange(formats);
                        dataFormatTypeModels.Add(dataFormatTypeModel);
                        break;
                }

                return Ok(dataFormatTypeModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getTimeZones")]
        public IHttpActionResult GetTimeZones()
        {
            List<object> timeZones = new List<object>();

            try { 
                foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                {
                    timeZones.Add(new { id = z.Id, name = z.DisplayName });
                }
            
                return Ok(timeZones);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("fillMainReport")]
        public IHttpActionResult FillMainReport(string password)
        {
            var cache = false;
            var emails = false;

            if (password != "d7DVEkPMRWrnYbp227PawkH")
                return HttpBadRequest("Invalid password");

            MemoryCacheManager.EnableRemoteCacheCleaner = false;
            try
            {
                GlobalDataManager.LockFillReport = true;

                _reportService.FillMainReport();

                GlobalDataManager.LockFillReport = false;

                cache = true;
            }
            finally
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
            }

            return Ok();
        }


        [HttpGet]
        [Route("pdf/generateBuyerMontlyStatements")]
        public async Task<IHttpActionResult> GenerateBuyerMontlyStatements(string password)
        {
            if (password != "d7DVEkPMRWrnYbp227PawkH")
                return HttpBadRequest("Invalid password");

            var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();
            var reportService = AppEngineContext.Current.Resolve<IReportService>();

            var buyers = buyerService.GetAllBuyers();
            Task.Run(() =>
            {
                foreach (Buyer b in buyers)
                {
                    if (b.SendStatementReport.HasValue && b.SendStatementReport.Value)
                    {
                        var today = DateTime.UtcNow;
                        PdfReportCreator creator = new PdfReportBuyerMontlyStatement(b.Id, reportService, buyerService);
                        string fileName = $"{b.Id}_MontlyStatement_{today.Year}-{today.Month}-{today.Day}.pdf";
                        creator.GenerateReport(fileName).Wait();
                    }
                }
            }).Wait(20000);

            return Ok();
        }

        [HttpPost]
        [Route("generate")]
        public IHttpActionResult Generate(GenerateDataModel model)
        {
            try
            {
                switch (model.DataTypes)
                {
                    case DataGenerationTypes.Addons:
                        {
                            var newAddon = (Core.Domain.Membership.Addon)model.Data;
                            _addonService.InsertAddon(newAddon);
                            break;
                        }
                    case DataGenerationTypes.Permissions:
                        {
                            var newPermission = (Permission)model.Data;
                            _permissionService.InsertPermission(newPermission);
                            break;
                        }
                    case DataGenerationTypes.Plans:
                        {
                            var newPlan = (PlanModel)model.Data;
                            _planService.InsertPlan(newPlan);
                            break;
                        }
                    case DataGenerationTypes.UserPlan:
                        {
                            var newUserPlan = (UserPlan)model.Data;
                            var plan = _planService.GetPlanByID(newUserPlan.PlanId);
                            var user = _userService.GetUserById(newUserPlan.UserId);
                            if(plan!=null && user!=null)
                            _planService.InsertUserPlan(newUserPlan);
                            break;
                        }
                    case DataGenerationTypes.PermissionAddon:
                        {
                            var newPermissionAddon = (PermissionAddon)model.Data;
                            var addon = _addonService.GetAddonById(newPermissionAddon.AddonId);
                            var permission = _permissionService.GetPermissionById(newPermissionAddon.PermissionId);
                            if (addon != null && permission != null)
                                _addonService.InsertPermissionAddon(newPermissionAddon);
                            break;
                        }
                    case DataGenerationTypes.UserAddon:
                        {
                            var newUserAddons = (UserAddons)model.Data;
                            var addon = _addonService.GetAddonById(newUserAddons.AddonId);
                            var user = _userService.GetUserById(newUserAddons.UserId);
                            if (addon != null && user != null)
                                _addonService.InsertUserAddon(newUserAddons);
                            break;
                        }
                    default:
                        break;
                }
                return Ok();
            }
            catch (Exception )
            {
                throw;
            }
        }


        [HttpPost]
        [Route("generateUser")]
        public IHttpActionResult GenerateUser([FromBody] GenerateEmployeeModel employeeModel, string apiPassword)
        {
            try
            {
                if (!_userService.CheckGlobalAttribute("FirstUserRegistration", apiPassword))
                {
                    return HttpBadRequest("Incorrect password");
                }

                if (employeeModel == null)
                {
                    return HttpBadRequest(null);
                }


                var roleId = 1;
                var role = _roleService.GetRoleById(roleId);

                if (role == null || role.Deleted || !role.Active)
                {
                    return HttpBadRequest($"no role was found for given id {roleId}");
                }

                User employee = null;

                employee = _userService.GetUserByEmail(employeeModel.Email);
                if (employee != null)
                {
                    return HttpBadRequest($"User with '{employeeModel.Email}' email already exist");
                }

                string generatedPassword = employeeModel.Password;// GeneratePassword(8, 1); //"x0$Mm@0PgM3M";

                string saltKey = _encryptionService.CreateSaltKey(20);
                string password = _encryptionService.CreatePasswordHash(generatedPassword, saltKey);

                employee = new User
                {
                    Id = 0,
                    ParentId = 0,
                    UserType = UserTypes.Super,
                    GuId = Guid.NewGuid().ToString(),
                    Username = employeeModel.Email,
                    Email = employeeModel.Email,
                    ContactEmail = employeeModel.Email,
                    Password = password,
                    SaltKey = saltKey,
                    Active = true,
                    LockedOut = false,
                    Deleted = false,
                    BuiltIn = false,
                    BuiltInName = null,
                    RegistrationDate = DateTime.Now,
                    LoginDate = DateTime.Now,
                    ActivityDate = DateTime.Now,
                    PasswordChangedDate = null,
                    LockoutDate = null,
                    IpAddress = null,
                    FailedPasswordAttemptCount = null,
                    Comment = null,
                    DepartmentId = 1,
                    MenuType = null,
                    MaskEmail = false,
                    ValidateOnLogin = false,
                    ChangePassOnLogin = false,
                    TimeZone = null,
                    RemoteLoginGuid = null
                };
                employee.Roles.Add(role);
                _userService.InsertUser(employee);

                var profile = new Profile
                {
                    Id = 0,
                    UserId = employee.Id,
                    FirstName = employeeModel.FirstName,
                    LastName = employeeModel.LastName,
                    MiddleName = employeeModel.MiddleName,
                    JobTitle = employeeModel.JobTitle,
                    Phone = "",
                    CellPhone = "",
                    Summary = "",
                };
                _profileService.InsertProfile(profile);


                var userModel = new EmployeeAdvancedModel()
                {
                    UserId = employee.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    JobTitle = profile.JobTitle,
                    RoleId = roleId,
                    IsActive = employee.Active,
                    LogoPath = string.IsNullOrWhiteSpace(employee.ProfilePicturePath) ? null : employee.ProfilePicturePath
                };

                return Ok(userModel);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("sendTestEmail")]
        public IHttpActionResult SendTestEmail(string password)
        {
            if (password != "2233pogos")
            {
                return BadRequest("Invalid password");
            }

            var user = _userService.GetUserByEmail("admin@adrack.com");
            (_emailService as EmailService).CanThrowException = true;
            user.Email = "haik.a@adrack.com";

            _emailService.SendUserWelcomeMessage(user, 1, EmailOperatorEnums.LeadNative);

            return Ok();
        }


        [HttpGet]
        [Route("getClientContextCount")]
        public IHttpActionResult GetClientContextCount()
        {
            return Ok(new { count = (_dbContextService as DbContextService).ClientContextCount });
        }

        #endregion


        #region Private methods
        private List<IdNameModel> GetStateProvinces(int countryId)
        {
            var stateProvinces = _stateProvinceService.GetStateProvinceByCountryId(countryId);

            return stateProvinces.Select(x => new IdNameModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        private List<IdNameModel> GetRoles()
        {
            var roles = _roleService.GetAllRoles();
            return roles.Select(item => new IdNameModel()
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }
        private List<IdNameModel> GetDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            return departments.Select(item => new IdNameModel()
            {
                Id = item.Id,
                Name = item.Name

            }).ToList();
        }

        private List<CountryViewModel> GetCountries()
        {
            var countries = _countryService.GetAllCountries();

            return countries.Select(x => new CountryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.TwoLetteroCode,
                ZipCodeLength = x.ZipLength
            }).ToList();
        }

        private List<ValidatorModel> GetValidators()
        {
            var validatorModels = new List<ValidatorModel>();

            var validators = Enum.GetValues(typeof(Validators)).Cast<Validators>().ToList();
            foreach (var validator in validators)
            {
                validatorModels.Add(new ValidatorModel
                {
                    Id = (byte)validator,
                    Name = validator.ToString()
                });
            }

            return validatorModels;
        }

        private List<ConditionModel> GetConditions(byte? validator)
        {
            var conditionModels = new List<ConditionModel>();

            var conditions = Enum.GetValues(typeof(Conditions)).Cast<Conditions>().ToList();
            if (validator.HasValue)
            {
                switch (validator)
                {
                    case (byte) Validators.Email:
                    case (byte) Validators.String:
                    case (byte) Validators.Zip:
                    case (byte) Validators.Phone:
                    case (byte) Validators.Ssn:
                    case (byte) Validators.AccountNumber:
                    case (byte) Validators.State:
                        conditions = conditions.Where(x => x == Conditions.Contains ||
                                                           x == Conditions.NotContains ||
                                                           x == Conditions.StartsWith ||
                                                           x == Conditions.EndsWith ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.StringByLength).ToList();
                        break;
                    case (byte) Validators.Number:
                    case (byte) Validators.RoutingNumber:
                    case (byte) Validators.Decimal:
                    case (byte) Validators.SubId:
                        conditions = conditions.Where(x => x == Conditions.Equals ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.NumberGreater ||
                                                           x == Conditions.NumberGreaterOrEqual ||
                                                           x == Conditions.NumberLess ||
                                                           x == Conditions.NumberLessOrEqual ||
                                                           x == Conditions.NumberRange ||
                                                           x == Conditions.StringByLength).ToList();
                        break;
                    case (byte) Validators.DateTime:
                    case (byte) Validators.DateOfBirth:
                        conditions = conditions.Where(x => x == Conditions.Equals ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.NumberGreater ||
                                                           x == Conditions.NumberGreaterOrEqual ||
                                                           x == Conditions.NumberLess ||
                                                           x == Conditions.NumberLessOrEqual ||
                                                           x == Conditions.StringByLength).ToList();
                        break;

                }
            }

            foreach (var condition in conditions)
            {
                conditionModels.Add(new ConditionModel
                {
                    Id = (byte)condition,
                    Name = condition.ToString()
                });
            }

            return conditionModels;
        }

        [HttpGet]
        [Route("setUserTypes")]
        private List<IdNameModel> GetUserTypes()
        {
            var userTypesModel = new List<IdNameModel>();
            
            var affiliateUsersCount = _userService.GetAffiliateUsers().Count();
            var buyerUsersCount = _userService.GetBuyerUsers().Count();
            var networkUsersCount = _userService.GetNetworkUsers().Count();

            var userTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().ToList();
            foreach (var userType in userTypes)
            {
                if (userType == UserTypes.Super)
                    continue;

                int userCount = 0;
                if (userType == UserTypes.Affiliate)
                    userCount = affiliateUsersCount;
                else if (userType == UserTypes.Buyer)
                    userCount = buyerUsersCount;
                else if (userType == UserTypes.Network)
                    userCount = networkUsersCount;

                userTypesModel.Add(new IdNameModel
                {
                    Id = (int)userType,
                    Name = userType.ToString(),
                    Count = userCount
                });
            }

            return userTypesModel;
        }

        private List<IdNameModel> GetAffiliatePriceMethodList()
        {
            var affiliatePriceMethodList = new List<IdNameModel>();

            var affiliatePriceMethods = Enum.GetValues(typeof(AffilatePriceMethods)).Cast<AffilatePriceMethods>().ToList();
            foreach (var affiliatePriceMethod in affiliatePriceMethods)
            {
                affiliatePriceMethodList.Add(new IdNameModel
                {
                    Id = (int)affiliatePriceMethod,
                    Name = affiliatePriceMethod.ToString()
                });
            }

            return affiliatePriceMethodList;
        }

        private List<IdNameModel> GetAffiliateChannelPriceMethodList()
        {
            var affiliatePriceMethodList = new List<IdNameModel>();

            var affiliatePriceMethods = Enum.GetValues(typeof(AffiliateChannelPriceMethods)).Cast<AffiliateChannelPriceMethods>().ToList();
            foreach (var affiliatePriceMethod in affiliatePriceMethods)
            {
                affiliatePriceMethodList.Add(new IdNameModel
                {
                    Id = (int)affiliatePriceMethod,
                    Name = affiliatePriceMethod.ToString()
                });
            }

            return affiliatePriceMethodList;
        }

        private List<IdNameModel> GetDataFormats()
        {
            var formatModels = new List<IdNameModel>();

            var formats = Enum.GetValues(typeof(DataFormat)).Cast<DataFormat>().ToList();
            foreach (var format in formats)
            {
                formatModels.Add(new IdNameModel()
                {
                    Id =Convert.ToInt64(format),
                    Name = format.ToString()
                });
            }

            return formatModels;
        }

        private List<IdNameModel> GetBuyerTypes()
        {
            var buyerTypesModel = new List<IdNameModel>();

            var buyerTypes = Enum.GetValues(typeof(BuyerType)).Cast<BuyerType>().ToList();
            foreach (var type in buyerTypes)
            {
                buyerTypesModel.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return buyerTypesModel;
        }

        private List<IdNameModel> GetCompanyIndustries()
        {
            var industryModels = new List<IdNameModel>();

            var industries =_verticalService.GetAllVerticals();
            foreach (var industry in industries)
            {
                industryModels.Add(new IdNameModel
                {
                    Id = industry.Id,
                    Name =industry.Name
                });
            }

            return industryModels;
        }

        private List<IdNameModel> GetEntityTypes()
        {
            var entityTypesModel = new List<IdNameModel>();

            var entityTypes = Enum.GetValues(typeof(EntityType)).Cast<EntityType>().ToList();
            foreach (var type in entityTypes)
            {
                entityTypesModel.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return entityTypesModel;
        }

        private List<IdNameModel> GetLeadStatusList()
        {
            var StatusModelList = new List<IdNameModel>();

            var statusList = Enum.GetValues(typeof(LeadResponseStatus)).Cast<LeadResponseStatus>().ToList();
            foreach (var type in statusList)
            {
                StatusModelList.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return StatusModelList;
        }

        private List<IdNameModel> GetBillFrequencyList()
        {
            var BillFrequencyList = new List<IdNameModel>();

            var billFrequencyList = Enum.GetValues(typeof(BillFrequency)).Cast<BillFrequency>().ToList();
            foreach (var type in billFrequencyList)
            {
                BillFrequencyList.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.GetDescription()
                });
            }

            return BillFrequencyList;
        }

        #endregion
    }
}