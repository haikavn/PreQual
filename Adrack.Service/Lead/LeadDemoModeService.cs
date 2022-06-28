// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 12-28-2020
//
// Last Modified By : Grigori D.
// Last Modified On : 12-28-2020
// ***********************************************************************
// <copyright file="LeadDemoModeService.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core.Domain.Accounting;
using Adrack.Service.Accounting;
using Adrack.Core;
using System.Xml.Serialization;
using Adrack.Core.Attributes;

namespace Adrack.Service.Lead
{

    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.ILeadDemoModeService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadDemoModeService" />
    public partial class LeadDemoModeService : ILeadDemoModeService
    {
        #region Constants
        private const string CACHE_AFFILIATE_CHANNEL_GetAffiliateChannels = "App.Cache.AffiliateChannel.GetAffiliateChannels-{0}";
        private const string CACHE_BUYER_CHANNEL_GetBuyerChannels = "App.Cache.BuyerChannel.GetBuyerChannels-{0}";

        #endregion Constants

        #region Fields

        /// <summary>
        /// LeadMain
        /// </summary>
        private readonly IRepository<LeadMain> _leadMainRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// IDataProvider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// The buyer balance repository
        /// </summary>
        private readonly IRepository<BuyerBalance> _buyerBalanceRepository;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;


        /// <summary>
        /// The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;


        private readonly IRepository<AffiliateChannel> _affiliateChannelRepository;
        private readonly IRepository<BuyerChannel> _buyerChannelRepository;
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IRepository<LeadContentDublicate> _leadContentDublicateRepository;

        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IRedirectUrlService _redirectUrlService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadMainRepository">The lead main repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="affiliateChannelRepository">The Affiliate Channel Repository.</param>
        /// <param name="buyerChannelRepository">The Buyer Channel Repository.</param>
        /// <param name="campaignRepository">The Campaign Repository.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="buyerBalanceRepository">The buyer balance repository.</param>
        /// <param name="leadContentDublicateRepository">Lead Content Dublicate Repository.</param>
        /// <param name="accountingService">The accounting service.</param>
        public LeadDemoModeService(
                                IRepository<LeadMain> leadMainRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher,
                                IDataProvider dataProvider,
                                IRepository<AffiliateChannel> affiliateChannelRepository,
                                IRepository<BuyerChannel> buyerChannelRepository,
                                IRepository<Campaign> campaignRepository,
                                ISettingService settingService,
                                IRepository<BuyerBalance> buyerBalanceRepository,
                                IRepository<LeadContentDublicate> leadContentDublicateRepository,
                                IAccountingService accountingService,
                                IBuyerChannelService buyerChannelService,
                                IRedirectUrlService redirectUrlService
                                )
        {
            this._leadMainRepository = leadMainRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
            this._affiliateChannelRepository = affiliateChannelRepository;
            this._buyerChannelRepository = buyerChannelRepository;
            this._campaignRepository = campaignRepository;
            this._settingService = settingService;
            this._buyerBalanceRepository = buyerBalanceRepository;
            this._leadContentDublicateRepository = leadContentDublicateRepository;
            this._accountingService = accountingService;
            this._buyerChannelService = buyerChannelService;
            this._redirectUrlService = redirectUrlService;
        }

        #endregion Constructor

        #region Export and Import XML Methods

        public virtual XElement SetCampaignXmlValue(Campaign campaign)
        {
            var element = new XElement("Campaigns"); ;
            element.Add(new XElement("CampaignId", campaign.Id));
            element.Add(new XElement("Name", campaign.Name));
            element.Add(new XElement("Deleted", campaign.IsDeleted));
            element.Add(new XElement("Description", campaign.Description));
            element.Add(new XElement("Status", campaign.Status));
            element.Add(new XElement("XmlTemplate", campaign.DataTemplate));
            element.Add(new XElement("NetworkTargetRevenue", campaign.NetworkTargetRevenue));
            element.Add(new XElement("HtmlFormId", campaign.HtmlFormId));
            element.Add(new XElement("VerticalId", campaign.VerticalId));
            element.Add(new XElement("PrioritizedEnabled", campaign.PrioritizedEnabled));
            element.Add(new XElement("CampaignKey", campaign.CampaignKey));
            element.Add(new XElement("Visibility", campaign.Visibility));
            element.Add(new XElement("IsTemplate", campaign.IsTemplate));
            element.Add(new XElement("NetworkMinimumRevenue", campaign.NetworkMinimumRevenue));
            element.Add(new XElement("CampaignType", campaign.CampaignType));
            element.Add(new XElement("PriceFormat", campaign.PriceFormat));
            element.Add(new XElement("CreatedOn", campaign.CreatedOn));
            element.Add(new XElement("Start", campaign.Start));
            element.Add(new XElement("Finish", campaign.Finish));
            element.Add(new XElement("PingTreeCycle", campaign.PingTreeCycle));

            return element;
        }

        public virtual XElement SetCampaignTemplateXmlValue(CampaignField obj)
        {
            var element = new XElement("CampaignTemplates"); ;
            element.Add(new XElement("CampaignTemplateId", obj.Id));
            element.Add(new XElement("CampaignId", obj.CampaignId));
            element.Add(new XElement("TemplateField", obj.TemplateField));
            element.Add(new XElement("DatabaseField", obj.DatabaseField));
            element.Add(new XElement("Validator", obj.Validator));
            element.Add(new XElement("SectionName", obj.SectionName));
            element.Add(new XElement("Description", obj.Description));
            element.Add(new XElement("MinLength", obj.MinLength));
            element.Add(new XElement("MaxLength", obj.MaxLength));
            element.Add(new XElement("Required", obj.Required));
            element.Add(new XElement("BlackListTypeId", obj.BlackListTypeId));
            element.Add(new XElement("PossibleValue", obj.PossibleValue));
            element.Add(new XElement("IsHash", obj.IsHash));
            element.Add(new XElement("IsHidden", obj.IsHidden));
            element.Add(new XElement("IsFilterable", obj.IsFilterable));
            element.Add(new XElement("Label", obj.Label));
            element.Add(new XElement("ColumnNumber", obj.ColumnNumber));
            element.Add(new XElement("PageNumber", obj.PageNumber));
            element.Add(new XElement("IsFormField", obj.IsFormField));
            element.Add(new XElement("OptionValues", obj.OptionValues));
            element.Add(new XElement("FieldType", obj.FieldType));
            element.Add(new XElement("FieldFilterSettings", obj.FieldFilterSettings));


            return element;
        }
        
        public virtual XElement SetBuyerXmlValue(Buyer obj)
        {
            var element = new XElement("Buyers"); ;
            element.Add(new XElement("BuyerId", obj.Id));
            element.Add(new XElement("CountryId", obj.CountryId));
            element.Add(new XElement("StateProvinceId", obj.StateProvinceId));
            element.Add(new XElement("Name", obj.Name));
            element.Add(new XElement("AddressLine1", obj.AddressLine1));
            element.Add(new XElement("AddressLine2", obj.AddressLine2));
            element.Add(new XElement("City", obj.City));
            element.Add(new XElement("ZipPostalCode", obj.ZipPostalCode));
            element.Add(new XElement("Phone", obj.Phone));
            element.Add(new XElement("Email", obj.Email));
            element.Add(new XElement("CreatedOn", obj.CreatedOn));
            element.Add(new XElement("Status", obj.Status));
            element.Add(new XElement("BillFrequency", obj.BillFrequency));
            element.Add(new XElement("FrequencyValue", obj.FrequencyValue));
            element.Add(new XElement("LastPostedSold", obj.LastPostedSold));
            element.Add(new XElement("LastPosted", obj.LastPosted));
            element.Add(new XElement("AlwaysSoldOption", obj.AlwaysSoldOption));
            element.Add(new XElement("MaxDuplicateDays", obj.MaxDuplicateDays));
            element.Add(new XElement("DailyCap", obj.DailyCap));
            element.Add(new XElement("Description", obj.Description));
            element.Add(new XElement("ExternalId", obj.ExternalId));
            element.Add(new XElement("Deleted", obj.Deleted));
            element.Add(new XElement("IsBiWeekly", obj.IsBiWeekly));
            element.Add(new XElement("CoolOffEnabled", obj.CoolOffEnabled));
            element.Add(new XElement("CoolOffStart", obj.CoolOffStart));
            element.Add(new XElement("CoolOffEnd", obj.CoolOffEnd));
            element.Add(new XElement("DoNotPresentStatus", obj.DoNotPresentStatus));
            element.Add(new XElement("DoNotPresentUrl", obj.DoNotPresentUrl));
            element.Add(new XElement("DoNotPresentResultField", obj.DoNotPresentResultField));
            element.Add(new XElement("DoNotPresentResultValue", obj.DoNotPresentResultValue));
            element.Add(new XElement("DoNotPresentRequest", obj.DoNotPresentRequest));
            element.Add(new XElement("DoNotPresentPostMethod", obj.DoNotPresentPostMethod));
            element.Add(new XElement("CanSendLeadId", obj.CanSendLeadId));
            element.Add(new XElement("AccountId", obj.AccountId));
            element.Add(new XElement("IconPath", obj.IconPath));


            return element;
        }

        public virtual XElement SetBuyerChannelXmlValue(BuyerChannel obj)
        {
            
            var element = new XElement("BuyerChannels"); ;
            element.Add(new XElement("BuyerChannelId", obj.Id));
            element.Add(new XElement("CampaignId", obj.CampaignId));
            element.Add(new XElement("BuyerId", obj.BuyerId));
            element.Add(new XElement("Name", obj.Name));
            element.Add(new XElement("Status", obj.Status));
            element.Add(new XElement("XmlTemplate", obj.XmlTemplate));
            element.Add(new XElement("AcceptedField", obj.AcceptedField));
            element.Add(new XElement("AcceptedValue", obj.AcceptedValue));
            element.Add(new XElement("AcceptedFrom", obj.AcceptedFrom));
            element.Add(new XElement("ErrorField", obj.ErrorField));
            element.Add(new XElement("ErrorValue", obj.ErrorValue));
            element.Add(new XElement("ErrorFrom", obj.ErrorFrom));
            element.Add(new XElement("RejectedField", obj.RejectedField));
            element.Add(new XElement("RejectedValue", obj.RejectedValue));
            element.Add(new XElement("RejectedFrom", obj.RejectedFrom));
            element.Add(new XElement("TestField", obj.TestField));
            element.Add(new XElement("TestValue", obj.TestValue));
            element.Add(new XElement("TestFrom", obj.TestFrom));
            element.Add(new XElement("RedirectField", obj.RedirectField));
            element.Add(new XElement("MessageField", obj.MessageField));
            element.Add(new XElement("PriceField", obj.PriceField));
            element.Add(new XElement("Delimeter", obj.Delimeter));
            element.Add(new XElement("PriceRejectField", obj.PriceRejectField));
            element.Add(new XElement("PriceRejectValue", obj.PriceRejectValue));
            element.Add(new XElement("PostingUrl", obj.PostingUrl));
            element.Add(new XElement("DeliveryMethod", obj.DeliveryMethod));
            element.Add(new XElement("Timeout", obj.Timeout));
            element.Add(new XElement("AfterTimeout", obj.AfterTimeout));
            element.Add(new XElement("NotificationEmail", obj.NotificationEmail));
            element.Add(new XElement("AffiliatePrice", obj.AffiliatePrice));
            element.Add(new XElement("BuyerPrice", obj.BuyerPrice));
            element.Add(new XElement("CapReachedNotification", obj.CapReachedNotification));
            element.Add(new XElement("TimeoutNotification", obj.TimeoutNotification));
            element.Add(new XElement("OrderNum", obj.OrderNum));
            element.Add(new XElement("GroupNum", obj.GroupNum));
            element.Add(new XElement("IsFixed", obj.IsFixed));
            element.Add(new XElement("AllowedAffiliateChannels", obj.AllowedAffiliateChannels));
            element.Add(new XElement("DataFormat", obj.DataFormat));
            element.Add(new XElement("PostingHeaders", obj.PostingHeaders));
            element.Add(new XElement("BuyerPriceOption", obj.BuyerPriceOption));
            element.Add(new XElement("AffiliatePriceOption", obj.AffiliatePriceOption));
            element.Add(new XElement("AlwaysSoldOption", obj.AlwaysSoldOption));
            element.Add(new XElement("ZipCodeTargeting", obj.ZipCodeTargeting));
            element.Add(new XElement("StateTargeting", obj.StateTargeting));
            element.Add(new XElement("MinAgeTargeting", obj.MinAgeTargeting));
            element.Add(new XElement("MaxAgeTargeting", obj.MaxAgeTargeting));
            element.Add(new XElement("EnableZipCodeTargeting", obj.EnableZipCodeTargeting));
            element.Add(new XElement("EnableStateTargeting", obj.EnableStateTargeting));
            element.Add(new XElement("EnableAgeTargeting", obj.EnableAgeTargeting));
            element.Add(new XElement("ZipCodeCondition", obj.ZipCodeCondition));
            element.Add(new XElement("StateCondition", obj.StateCondition));
            element.Add(new XElement("Deleted", obj.Deleted));
            element.Add(new XElement("Holidays", obj.Holidays));
            element.Add(new XElement("RedirectUrl", obj.RedirectUrl));
            element.Add(new XElement("MaxDuplicateDays", obj.MaxDuplicateDays));
            element.Add(new XElement("TimeZone", obj.TimeZone));
            element.Add(new XElement("TimeZoneStr", obj.TimeZoneStr));
            element.Add(new XElement("LeadAcceptRate", obj.LeadAcceptRate));
            element.Add(new XElement("SubIdWhiteListEnabled", obj.SubIdWhiteListEnabled));
            element.Add(new XElement("AccountIdField", obj.AccountIdField));
            element.Add(new XElement("EnableCustomPriceReject", obj.EnableCustomPriceReject));
            element.Add(new XElement("PriceRejectWinResponse", obj.PriceRejectWinResponse));
            element.Add(new XElement("FieldAppendEnabled", obj.FieldAppendEnabled));
            element.Add(new XElement("WinResponseUrl", obj.WinResponseUrl));
            element.Add(new XElement("WinResponsePostMethod", obj.WinResponsePostMethod));
            element.Add(new XElement("LeadIdField", obj.LeadIdField));
            element.Add(new XElement("ChildChannels", obj.ChildChannels));
            element.Add(new XElement("ResponseFormat", obj.ResponseFormat));
            element.Add(new XElement("ChannelMappingUniqueId", obj.ChannelMappingUniqueId));
            element.Add(new XElement("StatusExpireDate", obj.StatusExpireDate));
            element.Add(new XElement("StatusAutoChange", obj.StatusAutoChange));
            element.Add(new XElement("StatusChangeMinutes", obj.StatusChangeMinutes));
            element.Add(new XElement("ChangeStatusAfterCount", obj.ChangeStatusAfterCount));
            element.Add(new XElement("CurrentStatusChangeNum", obj.CurrentStatusChangeNum));
            element.Add(new XElement("DailyCap", obj.DailyCap));
            element.Add(new XElement("Note", obj.Note));
            element.Add(new XElement("CapReachEmailCount", obj.CapReachEmailCount));
            element.Add(new XElement("CountryId", obj.CountryId));
            element.Add(new XElement("HolidayYear", obj.HolidayYear));
            element.Add(new XElement("HolidayAnnualAutoRenew", obj.HolidayAnnualAutoRenew));
            element.Add(new XElement("HolidayIgnore", obj.HolidayIgnore));
            element.Add(new XElement("ManagerId", obj.ManagerId));

            return element;
        }

        public virtual XElement SetAffiliateXmlValue(Affiliate obj)
        {
            var element = new XElement("Affiliates"); ;
            element.Add(new XElement("AffiliateId", obj.Id));
            element.Add(new XElement("CountryId", obj.CountryId));
            element.Add(new XElement("StateProvinceId", obj.StateProvinceId));
            element.Add(new XElement("Name", obj.Name));
            element.Add(new XElement("AddressLine1", obj.AddressLine1));
            element.Add(new XElement("AddressLine2", obj.AddressLine2));
            element.Add(new XElement("City", obj.City));
            element.Add(new XElement("ZipPostalCode", obj.ZipPostalCode));
            element.Add(new XElement("Phone", obj.Phone));
            element.Add(new XElement("Email", obj.Email));
            element.Add(new XElement("CreatedOn", obj.CreatedOn));
            element.Add(new XElement("UserId", obj.UserId));
            element.Add(new XElement("ManagerId", obj.ManagerId));
            element.Add(new XElement("Status", obj.Status));
            element.Add(new XElement("BillFrequency", obj.BillFrequency));
            element.Add(new XElement("FrequencyValue", obj.FrequencyValue));
            element.Add(new XElement("BillWithin", obj.BillWithin));
            element.Add(new XElement("RegistrationIp", obj.RegistrationIp));
            element.Add(new XElement("Website", obj.Website));
            element.Add(new XElement("Deleted", obj.IsDeleted));
            element.Add(new XElement("IsBiWeekly", obj.IsBiWeekly));
            element.Add(new XElement("WhiteIp", obj.WhiteIp));
            element.Add(new XElement("DefaultAffiliatePriceMethod", obj.DefaultAffiliatePriceMethod));
            element.Add(new XElement("DefaultAffiliatePrice", obj.DefaultAffiliatePrice));
            element.Add(new XElement("IconPath", obj.IconPath));

            return element;
        }

        public virtual XElement SetAffiliateChannelXmlValue(AffiliateChannel obj)
        {

            var element = new XElement("AffiliateChannels"); ;
            element.Add(new XElement("AffiliateChannelId", obj.Id));
            element.Add(new XElement("CampaignId", obj.CampaignId));
            element.Add(new XElement("AffiliateId", obj.AffiliateId));
            element.Add(new XElement("Name", obj.Name));
            element.Add(new XElement("Status", obj.Status));
            element.Add(new XElement("XmlTemplate", obj.XmlTemplate));
            element.Add(new XElement("DataFormat", obj.DataFormat));
            element.Add(new XElement("MinPriceOption", obj.MinPriceOption));
            element.Add(new XElement("NetworkTargetRevenue", obj.NetworkTargetRevenue));
            element.Add(new XElement("NetworkMinimumRevenue", obj.NetworkMinimumRevenue));
            element.Add(new XElement("AffiliateChannelKey", obj.ChannelKey));
            element.Add(new XElement("Deleted", obj.IsDeleted));
            element.Add(new XElement("AffiliatePriceMethod", obj.AffiliatePriceMethod));
            element.Add(new XElement("AffiliatePrice", obj.AffiliatePrice));
            element.Add(new XElement("Timeout", obj.Timeout));
            element.Add(new XElement("Note", obj.Note));
            element.Add(new XElement("AffiliateChannelPassword", obj.ChannelPassword));

            return element;
        }
        


        private List<BuyerChannel> GetBuyerChannels(short deleted = 0)
        {

            var query = from x in _buyerChannelRepository.Table
                where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                orderby x.OrderNum
                select x;

            return query.ToList();

        }
        #endregion Export and Import XML Methods


        #region Demo Generation Methods

        /// <summary>
        /// Get Demo Leads
        /// </summary>
        /// <returns>Profile Demo Collection Item</returns>
        public virtual IList<LeadMainContent> GetDemoLeads(DateTime dateFrom, DateTime dateTo)
        {
            int leadCount = 100;
            var listLeadDemo = new List<LeadMainContent>();

            IList<AffiliateChannel> affiliateChannel = GetAffiliateChannels();
            if (affiliateChannel != null && affiliateChannel.Count > 0)
            {
                Random _random = new Random();
                int affiliateChannelNumber, buyerChannelNumber;
                long? tBuyerId;
                long? tBuyerChannelId;
                short tStatus;
                short tSuccessCount = 0;
                short tErrorCount = 0;

                for (int i = 0; i < leadCount; i++)
                {
                    affiliateChannelNumber = _random.Next(affiliateChannel.Count);
                    tBuyerId = null;
                    tBuyerChannelId = null;
                    tStatus = 3; //Reject

                    //if (_random.Next(1, 10) % 10 == 0){}

                    if (tSuccessCount < leadCount / 10)
                    {
                        IList<BuyerChannel> buyerChannel = GetBuyerChannelsByCampaignId((long)affiliateChannel[affiliateChannelNumber].CampaignId);
                        if (buyerChannel != null && buyerChannel.Count > 0)
                        {
                            buyerChannelNumber = _random.Next(buyerChannel.Count);

                            tBuyerId = buyerChannel[buyerChannelNumber].BuyerId;
                            tBuyerChannelId = buyerChannel[buyerChannelNumber].Id;

                            tStatus = 1; //Success 
                            tSuccessCount++;

                        }
                    }

                    if (tStatus != 1 && tErrorCount < leadCount / 5 && _random.Next(2) == 1)
                    {
                        tStatus = 2; //Error 
                        tErrorCount++;
                    }

                    listLeadDemo.Add(new LeadMainContent
                    {
                        Id = i,
                        Created = RandomDay(dateFrom, dateTo, _random),
                        UpdateDate = null,
                        Email = GenerateName(_random.Next(3, 10), _random).ToLower() + "@" +
                                GenerateName(_random.Next(3, 5), _random).ToLower() + ".com",
                        Firstname = GenerateName(_random.Next(4, 7), _random),
                        Lastname = GenerateName(_random.Next(6, 12), _random),
                        State = GetState(_random),
                        Zip = _random.Next(10000, 99999).ToString(),
                        AffiliateId = affiliateChannel[affiliateChannelNumber].AffiliateId,
                        AffiliateChannelId = affiliateChannel[affiliateChannelNumber].Id,
                        BuyerId = tBuyerId,
                        BuyerChannelId = tBuyerChannelId,
                        CampaignId = (long)affiliateChannel[affiliateChannelNumber].CampaignId,
                        ProcessingTime = 0.0,
                        Status = tStatus,
                        Url = "",
                        AffiliatePrice = 0,
                        BuyerPrice = 0
                    });


                }
            }

            return listLeadDemo;
        }

        private void SetXmlValue(XDocument xDocument, string elementName, string value, bool createIfNotExist = false)
        {
            string[] names = elementName.Split(new char[1] { ',' });

            foreach (var name in names)
            {
                var element = xDocument.Descendants().Where(x => x.Name.LocalName.ToLower() == name.ToLower()).FirstOrDefault();
                if (element == null)
                {
                    if (createIfNotExist)
                    {
                        element = new XElement(name);
                        xDocument.Root.Add(element);
                    }
                    else continue;
                }

                element.Value = value;
            }
        }

        private IList<BuyerChannel> ExlcudeBuyerChannels(long affiliateChannelId, IList<BuyerChannel> buyerChannels)
        {
            IList<BuyerChannel> allowedBuyerChannels = new List<BuyerChannel>();

            foreach(var buyerChannel in buyerChannels)
            {
                if (_buyerChannelService.CheckAllowedAffiliateChannel(affiliateChannelId, buyerChannel.Id))
                {
                    allowedBuyerChannels.Add(buyerChannel);
                }
            }

            return allowedBuyerChannels;
        }

        /// <summary>
        /// Lead Posting Simulation
        /// </summary>
        public virtual void LeadPostingSimulation()
        {
            //string strDefaultLeadXml = GetDefaultLeadXml();
            int leadCount = 3;

            IList<AffiliateChannel> affiliateChannel = GetAffiliateChannels();

            if (affiliateChannel != null && affiliateChannel.Count > 0)
            {
                XDocument xDocument = new XDocument(new XElement("Root"));

                Random random = new Random();

                affiliateChannel = affiliateChannel.OrderBy(x => x.Id).ToList();
                var affiliateChannelNumber = random.Next(affiliateChannel.Count);

                IList<BuyerChannel> buyerChannels = GetBuyerChannelsByCampaignId((long)affiliateChannel[affiliateChannelNumber].CampaignId);

                buyerChannels = ExlcudeBuyerChannels(affiliateChannel[affiliateChannelNumber].Id, buyerChannels);

                /*while (buyerChannels.Count == 0 && affiliateChannel.Count > 0)
                {
                    affiliateChannel.RemoveAt(affiliateChannelNumber);
                    affiliateChannelNumber = random.Next(affiliateChannel.Count);

                    if (affiliateChannel.Count == 0) continue;

                    buyerChannels = GetBuyerChannelsByCampaignId((long)affiliateChannel[affiliateChannelNumber].CampaignId);
                    buyerChannels = ExlcudeBuyerChannels(affiliateChannel[affiliateChannelNumber].Id, buyerChannels);
                }

                if (affiliateChannel.Count == 0) return;*/

                var campaign = GetCampaignById((long)affiliateChannel[affiliateChannelNumber].CampaignId);

                while ((affiliateChannelNumber < affiliateChannel.Count && affiliateChannel[affiliateChannelNumber].Status != 1) || buyerChannels.Count == 0 || buyerChannels.Where(x => (!x.Deleted.HasValue || x.Deleted.HasValue && !x.Deleted.Value) && x.Status == BuyerChannelStatuses.Active).FirstOrDefault() == null)
                {
                    affiliateChannel.RemoveAt(affiliateChannelNumber);
                    affiliateChannelNumber = random.Next(affiliateChannel.Count);
                    buyerChannels = GetBuyerChannelsByCampaignId((long)affiliateChannel[affiliateChannelNumber].CampaignId);
                    buyerChannels = ExlcudeBuyerChannels(affiliateChannel[affiliateChannelNumber].Id, buyerChannels);
                }

                var redirectUrls = new List<string>();
                var dublicateSSNs = new List<XDocument>();
                bool isCorrect = true;
                bool applyRedirect = true;

                for (int i = 0; i < leadCount; i++)
                {
                    if (!string.IsNullOrEmpty(affiliateChannel[affiliateChannelNumber].ChannelPassword))
                    {
                        if (buyerChannels != null && buyerChannels.Count > 0)
                        {
                            if (campaign != null && !string.IsNullOrEmpty(campaign.DataTemplate))
                            {
                                try
                                {
                                    xDocument = XDocument.Parse(campaign.DataTemplate);
                                }
                                catch
                                {
                                    if (leadCount < 10000)
                                    {
                                        leadCount++;
                                        continue;
                                    }
                                }

                                var buyerChannelNumber = random.Next(buyerChannels.Count);

                                BuyerBalance buyerBalance = _accountingService.GetBuyerBalanceById(buyerChannels[buyerChannelNumber].BuyerId);
                                if(buyerBalance != null)
                                    _accountingService.UpdateBuyerBalance(buyerBalance, "DemoBalance");

                                var ssn = random.Next(100000000, 999999999).ToString();
                                SetXmlValue(xDocument, "CHANNELID", affiliateChannel[affiliateChannelNumber]
                                        .ChannelKey.ToString(), true);

                                SetXmlValue(xDocument, "PASSWORD", affiliateChannel[affiliateChannelNumber].ChannelPassword, true);

                                SetXmlValue(xDocument, "leadminprice,minprice", buyerChannels[buyerChannelNumber].AffiliatePrice.ToString());

                                SetXmlValue(xDocument, "ipaddress,ip", random.Next(10, 256) + "." + random.Next(1, 256) + "." +
                                                              random.Next(10, 256) + "." + random.Next(100, 256));

                                SetXmlValue(xDocument, "REQUESTEDAMOUNT", random.Next(100, 1501).ToString());

                                SetXmlValue(xDocument, "ssn", ssn);

                                SetXmlValue(xDocument, "DATEOFBIRTH,dob", GetRandomBirthDay(new DateTime(1975, 1, 1), new DateTime(1995, 1, 1), random).ToString("MM/dd/yyyy"));

                                SetXmlValue(xDocument, "firstname", GenerateName(random.Next(4, 7), random));

                                SetXmlValue(xDocument, "lastname", GenerateName(random.Next(6, 12), random));

                                SetXmlValue(xDocument, "address", random.Next(10, 999) + " " +
                                                            GenerateName(random.Next(5, 10), random) + " St.");

                                var stateCode = GetState(random);
                                SetXmlValue(xDocument, "city", GetCityNameByStateCode(stateCode));

                                SetXmlValue(xDocument, "statecode,state", stateCode);

                                SetXmlValue(xDocument, "ZIPCODE,zip", random.Next(10000, 99999).ToString());

                                SetXmlValue(xDocument, "homephone", random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString());

                                SetXmlValue(xDocument, "cellphone", random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString());

                                SetXmlValue(xDocument, "dlstate", stateCode);

                                SetXmlValue(xDocument, "dlnumber", GenerateRandomString(4, random) + random.Next(1000, 9999).ToString());

                                SetXmlValue(xDocument, "ARMEDFORCES", random.Next(1, 3) == 2 ? "Yes" : "No");

                                SetXmlValue(xDocument, "RENTOROWN", random.Next(1, 3) == 2 ? "Rent" : "Own");

                                SetXmlValue(xDocument, "email", GenerateName(random.Next(3, 10), random).ToLower() + "@" +
                                                          GenerateName(random.Next(3, 5), random).ToLower() + ".com");

                                SetXmlValue(xDocument, "CITIZENSHIP", random.Next(1, 3) == 2 ? "Yes" : "No");

                                SetXmlValue(xDocument, "EMPPHONE", random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString());

                                SetXmlValue(xDocument, "JOBTITLE", GetRandomJobTitle(random));

                                SetXmlValue(xDocument, "NEXTPAYDATE", GetRandomBirthDay(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(-20),
                                                random).ToString("MM/dd/yyyy"));

                                SetXmlValue(xDocument, "SECONDPAYDATE", GetRandomBirthDay(DateTime.Today.AddDays(-19), DateTime.Today.AddDays(-10),
                                                random).ToString("MM/dd/yyyy"));

                                SetXmlValue(xDocument, "BANKNAME", GetRandomBankName(random));

                                SetXmlValue(xDocument, "BANKPHONE", random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString());

                                SetXmlValue(xDocument, "ROUTINGNUMBER", random.Next(100000000, 999999999).ToString());

                                SetXmlValue(xDocument, "ACCOUNTNUMBER", random.Next(100000000, 999999999).ToString());

                                SetXmlValue(xDocument, "DIRECTDEPOSIT", random.Next(1, 3) == 2 ? "Yes" : "No");


                                ///////////////////////////////////////
                                // LeadPostingSimulation
                                ///////////////////////////////////////

                                //HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://localhost:44331/import");
                                string domainName = HttpContext.Current?.Request.Url.GetLeftPart(UriPartial.Authority);

                                if (string.IsNullOrEmpty(domainName))
                                {
                                    domainName = ConfigurationManager.AppSettings["ApplicationUrl"];
                                }


                                if (!string.IsNullOrEmpty(domainName))
                                {
                                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(domainName + "/import");
                                    req.Method = "post";

                                    var postData = xDocument.ToString();

                                    var aaa = Encoding.Default.GetBytes($"\"{postData}\"");
                                    req.ContentType = "application/json";
                                    req.ContentLength = aaa.Length;
                                    req.GetRequestStream().Write(aaa, 0, aaa.Length);
                                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                                    using (System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream()))
                                    {
                                        string responseString = sr.ReadToEnd();

                                        var dublicate = _leadContentDublicateRepository.Table.FirstOrDefault(x => x.Ssn == ssn);
                                        if (dublicate != null)
                                        {
                                            dublicateSSNs.Add(xDocument);
                                        }


                                        /*
                                         if (responseString.Contains("sold"))
                                        {
                                            var xDoc = XDocument.Parse(responseString);

                                            if (xDoc.Root != null)
                                            {
                                                var redirect = xDoc.Root.Elements().Where(x => x.Name == "redirect").FirstOrDefault();
                                                if (redirect != null)
                                                {
                                                    var redirectURL = redirect.Value;
                                                    redirectUrls.Add(redirectURL);
                                                }
                                            }
                                        }*/

                                        if (responseString.Contains("sold") || responseString.Contains("success"))
                                        {
                                            var xDoc = XDocument.Parse(responseString);

                                            if (xDoc.Root != null)
                                            {
                                                var redirect = xDoc.Root.Elements().Where(x => x.Name == "redirect").FirstOrDefault();
                                                if (redirect != null && !string.IsNullOrEmpty(redirect.Value))
                                                {
                                                    var redirectURL = redirect.Value;

                                                    var id = xDoc.Root.Elements().Where(x => x.Name == "id").FirstOrDefault();
                                                    if (id != null && !string.IsNullOrEmpty(id.Value))
                                                    {
                                                        var leadId = Convert.ToInt64(id.Value);
                                                        if (leadId > 0)
                                                        {
                                                            if (applyRedirect)
                                                            {

                                                                var ru = new RedirectUrl
                                                                {
                                                                    Clicked = false,
                                                                    Created = DateTime.UtcNow
                                                                };
                                                                ru.ClickDate = ru.Created;
                                                                ru.LeadId = leadId;
                                                                ru.Url = redirectURL;
                                                                ru.NavigationKey = "";
                                                                ru.Device = "";
                                                                ru.Ip = "";
                                                                ru.Title = "";
                                                                ru.Description = "";
                                                                ru.Address = "";
                                                                ru.ZipCode = "";
                                                                _redirectUrlService.InsertRedirectUrl(ru);
                                                            }

                                                            applyRedirect = !applyRedirect;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    res.Close();
                                }
                            }
                            else
                                isCorrect = false;

                        }
                        else
                            isCorrect = false;

                    }
                    else
                        isCorrect = false;


                    if (!isCorrect && leadCount < 10000)
                    {
                        leadCount++;
                        isCorrect = true;
                    }
                }
                if (redirectUrls.Any())
                {
                    redirectUrls = redirectUrls.Take(Convert.ToInt32(Math.Round((double)(redirectUrls.Count / 100) * 90))).ToList();
                    foreach (var url in redirectUrls)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var respString = reader.ReadToEnd();
                        }
                    }
                }
                if (dublicateSSNs.Any())
                {
                    dublicateSSNs = dublicateSSNs.Take(Convert.ToInt32(Math.Round((double)(dublicateSSNs.Count / 100) * 30))).ToList();
                    foreach (var ssn in dublicateSSNs)
                    {
                        string domainName = HttpContext.Current?.Request.Url.GetLeftPart(UriPartial.Authority);

                        if (string.IsNullOrEmpty(domainName))
                        {
                            domainName = ConfigurationManager.AppSettings["ApplicationUrl"];
                        }

                        if (!string.IsNullOrEmpty(domainName))
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(domainName + "/import");
                            req.Method = "post";

                            var postData = xDocument.ToString();

                            var aaa = Encoding.Default.GetBytes($"\"{postData}\"");
                            req.ContentType = "application/json";
                            req.ContentLength = aaa.Length;
                            req.GetRequestStream().Write(aaa, 0, aaa.Length);
                            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream()))
                            {
                            }
                        }
                    }
                }
            }

        }


        #endregion Methods

        #region Private Methods
        private string GetDefaultLeadXml()
        {
            return "<REQUEST>" +
                    "<REFERRAL>" +
                        "<CHANNELID>9f0aead</CHANNELID>" +
                        "<PASSWORD>q6zvrk3c</PASSWORD>" +
                        "<SUBID>-12</SUBID>" +
                        "<SUBID2></SUBID2>" +
                        "<REFERRINGURL>adrack.com</REFERRINGURL>" +
                        "<LEADMINPRICE>2</LEADMINPRICE>" +
                    "</REFERRAL>" +
                    "<CUSTOMER>" +
                        "<PERSONALINFO>" +
                            "<IPADDRESS>75.2.92.149</IPADDRESS>" +
                            "<REQUESTEDAMOUNT>1406</REQUESTEDAMOUNT>" +
                            "<SSN>999999999</SSN>" +
                            "<DATEOFBIRTH>09-16-1980</DATEOFBIRTH>" +
                            "<FIRSTNAME>John</FIRSTNAME>" +
                            "<LASTNAME>Doe</LASTNAME>" +
                            "<ADDRESS>123 Main St.</ADDRESS>" +
                            "<CITY>Chicago</CITY>" +
                            "<STATECODE>IL</STATECODE>" +
                            "<ZIPCODE>60610</ZIPCODE>" +
                            "<HOMEPHONE>3125558901</HOMEPHONE>" +
                            "<CELLPHONE></CELLPHONE>" +
                            "<DLSTATE>IL</DLSTATE>" +
                            "<DLNUMBER>Qe509454</DLNUMBER>" +
                            "<ARMEDFORCES>Yes</ARMEDFORCES>" +
                            "<CONTACTTIME></CONTACTTIME>" +
                            "<RENTOROWN>rent</RENTOROWN>" +
                            "<EMAIL>test@nags.us</EMAIL>" +
                            "<ADDRESSLENGHT></ADDRESSLENGHT>" +
                            "<CITIZENSHIP>Yes</CITIZENSHIP>" +
                        "</PERSONALINFO>" +
                        "<EMPLOYMENTINFO>" +
                            "<INCOMETYPE>Job Income</INCOMETYPE>" +
                            "<EMPLENGHT></EMPLENGHT>" +
                            "<EMPNAME></EMPNAME>" +
                            "<EMPPHONE>3125558957</EMPPHONE>" +
                            "<JOBTITLE>programmer</JOBTITLE>" +
                            "<PAYFREQUENCY>Weekly</PAYFREQUENCY>" +
                            "<NEXTPAYDATE>03-01-2016</NEXTPAYDATE>" +
                            "<SECONDPAYDATE>03-16-2016</SECONDPAYDATE>" +
                        "</EMPLOYMENTINFO>" +
                        "<BANKINFO>" +
                            "<BANKNAME>US Bank</BANKNAME>" +
                            "<BANKPHONE></BANKPHONE>" +
                            "<ACCOUNTTYPE>Checking Account</ACCOUNTTYPE>" +
                            "<ROUTINGNUMBER>071000013</ROUTINGNUMBER>" +
                            "<ACCOUNTNUMBER>78979456</ACCOUNTNUMBER>" +
                            "<BANKLENGHT>90</BANKLENGHT>" +
                            "<NETMONTHLYINCOME>5000</NETMONTHLYINCOME>" +
                            "<DIRECTDEPOSIT>Yes</DIRECTDEPOSIT>" +
                        "</BANKINFO>" +
                    "</CUSTOMER>" +
                "</REQUEST>";
        }


        private DateTime GetRandomBirthDay(DateTime start, DateTime end, Random r)
        {
            //DateTime start = new DateTime(1975, 1, 1);
            //DateTime end = new DateTime(1995, 1, 1);

            int range = (end - start).Days;
            return start.AddDays(r.Next(range));
        }


        private string GenerateRandomString(int length, Random r)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[r.Next(s.Length)]).ToArray());
        }


        private string GetRandomJobTitle(Random r)
        {
            List<string> jobs = new List<string>();

            jobs.Add("programmer");
            jobs.Add("programmer");
            jobs.Add("chef");
            jobs.Add("construction worker");
            jobs.Add("firefighter");
            jobs.Add("doctor");
            jobs.Add("astronaut");
            jobs.Add("police");
            jobs.Add("teacher");

            return jobs[r.Next(0, jobs.Count)].ToString();
        }

        private string GetRandomBankName(Random r)
        {
            List<string> items = new List<string>();

            items.Add("US Bank");
            items.Add("US Bank");
            items.Add("Bank of America Corp.");
            items.Add("Citigroup Inc.");
            items.Add("U.S.Bancorp.");
            items.Add("Truist Financial Corporation.");
            items.Add("TD Group US Holdings LLC");

            return items[r.Next(0, items.Count)].ToString();
        }

        private string GenerateName(int len, Random r)
        {
            //Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }


        private IList<AffiliateChannel> GetAffiliateChannels(short deleted = 0)
        {
            var query = from x in _affiliateChannelRepository.Table
                        where (x.CampaignId != null && (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value)))
                        orderby x.Id descending
                        select x;

            return query.ToList();

            /*
            string key = string.Format(CACHE_AFFILIATE_CHANNEL_GetAffiliateChannels, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateChannelRepository.Table
                    where (x.CampaignId != null && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value)))
                    orderby x.Id descending
                    select x;

                return query.ToList();
            });
            */
        }


        private Campaign GetCampaignById(long campaignId)
        {
            if (campaignId == 0)
                return null;

            return _campaignRepository.GetById(campaignId);
        }


        private IList<BuyerChannel> GetBuyerChannelsByCampaignId(long campaignId, short deleted = 0)
        {

            var query = from x in _buyerChannelRepository.Table
                        where x.CampaignId == campaignId && (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                        orderby x.OrderNum
                        select x;

            return query.ToList();

        }

        private DateTime RandomDay(DateTime dateFrom, DateTime dateTo, Random r)
        {
            //DateTime start = new DateTime(2015, 1, 1);
            //int range = (DateTime.Today - start).Days;
            //return start.AddDays(r.Next(range));

            int range = (dateTo - dateFrom).Days;
            return dateFrom.AddDays(r.Next(range));
        }

        private string GetState(Random r)
        {
            List<string> states = new List<string>();

            states.Add("AL");
            states.Add("AK");
            states.Add("AZ");
            states.Add("AR");
            states.Add("CA");
            states.Add("CO");
            states.Add("CT");
            states.Add("DE");
            states.Add("DC");
            states.Add("FL");
            states.Add("GA");
            states.Add("HI");
            states.Add("ID");
            states.Add("IL");
            states.Add("IN");
            states.Add("IA");
            states.Add("KS");
            states.Add("KY");
            states.Add("LA");
            states.Add("ME");
            states.Add("MD");
            states.Add("MA");
            states.Add("MI");
            states.Add("MN");
            states.Add("MS");
            states.Add("MO");
            states.Add("MT");
            states.Add("NE");
            states.Add("NV");
            states.Add("NH");
            states.Add("NJ");
            states.Add("NM");
            states.Add("NY");
            states.Add("NC");
            states.Add("ND");
            states.Add("OH");
            states.Add("OK");
            states.Add("OR");
            states.Add("PA");
            states.Add("RI");
            states.Add("SC");
            states.Add("SD");
            states.Add("TN");
            states.Add("TX");
            states.Add("UT");
            states.Add("VT");
            states.Add("VA");
            states.Add("WA");
            states.Add("WV");
            states.Add("WI");
            states.Add("WY");

            return states[r.Next(0, states.Count)].ToString();
        }


        private string GetCityNameByStateCode(string abbr)
        {
            Dictionary<string, string> states = new Dictionary<string, string>();

            states.Add("AL", "Birmingham");
            states.Add("AK", "Anchorage");
            states.Add("AZ", "Phoenix");
            states.Add("AR", "Little Rock");
            states.Add("CA", "Los Angeles");
            states.Add("CO", "Denver");
            states.Add("CT", "Bridgeport");
            states.Add("DE", "Wilmington");
            states.Add("DC", "Washington, D.C.");
            states.Add("FL", "Jacksonville");
            states.Add("GA", "Atlanta");
            states.Add("HI", "Honolulu");
            states.Add("ID", "Boise");
            states.Add("IL", "Chicago");
            states.Add("IN", "Indianapolis");
            states.Add("IA", "Des Moines");
            states.Add("KS", "Wichita");
            states.Add("KY", "Louisville");
            states.Add("LA", "New Orleans");
            states.Add("ME", "Portland");
            states.Add("MD", "Baltimore");
            states.Add("MA", "Boston");
            states.Add("MI", "Detroit");
            states.Add("MN", "Minneapolis");
            states.Add("MS", "Jackson");
            states.Add("MO", "Kansas City");
            states.Add("MT", "Billings");
            states.Add("NE", "Omaha");
            states.Add("NV", "Las Vegas");
            states.Add("NH", "Manchester");
            states.Add("NJ", "Newark");
            states.Add("NM", "Albuquerque");
            states.Add("NY", "New York City");
            states.Add("NC", "Charlotte");
            states.Add("ND", "Fargo");
            states.Add("OH", "Columbus");
            states.Add("OK", "Oklahoma City");
            states.Add("OR", "Portland");
            states.Add("PA", "Philadelphia");
            states.Add("RI", "Providence");
            states.Add("SC", "Charleston");
            states.Add("SD", "Sioux Falls");
            states.Add("TN", "Nashville");
            states.Add("TX", "Houston");
            states.Add("UT", "Salt Lake City");
            states.Add("VT", "Burlington");
            states.Add("VA", "Virginia Beach");
            states.Add("WA", "Seattle");
            states.Add("WV", "Charleston");
            states.Add("WI", "Milwaukee");
            states.Add("WY", "Cheyenne");
            if (states.ContainsKey(abbr))
                return (states[abbr]);
            /* error handler is to return an empty string rather than throwing an exception */
            return "";
        }

        #endregion Methods
    }



}