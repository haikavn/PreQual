using System;
using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Adrack.Core;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelModel
    {
        public string Name { get; set; }
        public string TimeZone { get; set; }
        public string RedirectUrl { get; set; }
        public long BuyerId { get; set; }
        public short? MaxDuplicateDays { get; set; }
        public short Timeout { get; set; }
        public BuyerChannelStatuses Status { get; set; }
        public short? ResponseFormat { get; set; }
        public bool IsCapReachedNotification { get; set; }
        public bool IsTimeoutNotification { get; set; }
        public bool IsPauseChannel { get; set; }
        public short PauseAfterTimeout { get; set; }
        public short PauseFor { get; set; }
        public long ManagerId { get; set; }
        public string ManagerRole { get; set; }
        public short AffiliatePriceOption { get; set; }
        public decimal AffiliatePrice { get; set; }
        public BuyerPriceOptions BuyerPriceOption { get; set; }
        public decimal BuyerPrice { get; set; }
        public string XmlTemplate { get; set; }
        public List<BuyerChannelIntegrationModel> BuyerChannelFields { get; set; } = new List<BuyerChannelIntegrationModel>();
        public long CampaignId { get; set; }
        public List<BuyerChannelFilterCreateModel> BuyerChannelFilters { get; set; } = new List<BuyerChannelFilterCreateModel>();
        [MaxLength(50)]
        public string RedirectField { get; set; }
        [MaxLength(50)]
        public string MessageField { get; set; }
        [MaxLength(50)]
        public string PriceField { get; set; }
        [MaxLength(50)]
        public string AccountIdField { get; set; }

        [MaxLength(50)]
        public string SoldFieldName { get; set; }

        [MaxLength(50)]
        public string SoldValue { get; set; }
        public short SoldFrom { get; set; }

        [MaxLength(50)]
        public string ErrorFieldName { get; set; }

        [MaxLength(50)]
        public string ErrorValue { get; set; }
        public short ErrorFrom { get; set; }

        [MaxLength(50)]
        public string RejectedFieldName { get; set; }

        [MaxLength(50)]
        public string RejectedValue { get; set; }
        public short RejectedFrom { get; set; }
        public string TestFieldName { get; set; }
        public string TestValue { get; set; }
        public short TestFrom { get; set; }
        public string PriceRejectFieldName { get; set; }
        public string PriceRejectValue { get; set; }
        public string Delimeter { get; set; }
        public string ChildChannels { get; set; }
        public bool? EnableCustomPriceReject { get; set; }
        public bool? FieldAppendEnabled { get; set; }
        [MaxLength(500)]

        public string WinResponseUrl { get; set; }
        [MaxLength(50)]
        public string WinResponsePostMethod { get; set; }
        public string PriceRejectWinResponse { get; set; }
        public string LeadId { get; set; }

        public IList<BuyerChannelScheduleDayModel> BuyerChannelSchedules { get; set; } = new List<BuyerChannelScheduleDayModel>();

        public IList<BuyerChannelHolidayUpdateSimpleModel> Holidays { get; set; } = new List<BuyerChannelHolidayUpdateSimpleModel>();

        //------Not included in UI------
        public string PostingUrl { get; set; }
        public short DeliveryMethod { get; set; }
        public short AfterTimeout { get; set; }
        public string NotificationEmail { get; set; }
        public int OrderNum { get; set; }

        public int GroupNum { get; set; }

        public bool IsFixed { get; set; }
        public string AllowedAffiliateChannels { get; set; }
        public short DataFormat { get; set; }
        public string PostingHeaders { get; set; }
        public short TypeId { get; set; }
        public string ZipCodeTargeting { get; set; }
        public string StateTargeting { get; set; }
        public short MinAgeTargeting { get; set; }
        public short MaxAgeTargeting { get; set; }
        public bool EnableZipCodeTargeting { get; set; }
        public bool EnableStateTargeting { get; set; }
        public bool EnableAgeTargeting { get; set; }
        public short ZipCodeCondition { get; set; }
        public short StateCondition { get; set; }
        public bool? Deleted { get; set; }
        public string TimeZoneStr { get; set; }
        public double? LeadAcceptRate { get; set; }
        public bool? SubIdWhiteListEnabled { get; set; }
        public string ChannelMappingUniqueId { get; set; }
        public string StatusStr { get; set; }
        public DateTime? StatusExpireDate { get; set; }
        public int? DailyCap { get; set; }
        public string Note { get; set; }
        public long Id { get; private set; }

        public long? CountryId { get; set; }

        public int HolidayYear { get; set; }

        public bool HolidayAnnualAutoRenew { get; set; }

        public bool HolidayIgnore { get; set; }

        public bool AlwaysBuyerPrice { get; set; }
        //------Not included in UI------

        public static explicit operator BuyerChannel(BuyerChannelModel buyerChannelModel)
        {
            return new BuyerChannel
            {
                Id = 0,
                CampaignId = buyerChannelModel.CampaignId,
                BuyerId = buyerChannelModel.BuyerId,
                Name = buyerChannelModel.Name,
                Status = buyerChannelModel.Status,
                XmlTemplate = buyerChannelModel.XmlTemplate,
                AcceptedField = buyerChannelModel.SoldFieldName,
                AcceptedValue = buyerChannelModel.SoldValue,
                AcceptedFrom = buyerChannelModel.SoldFrom,
                ErrorField = buyerChannelModel.ErrorFieldName,
                ErrorValue = buyerChannelModel.ErrorValue,
                ErrorFrom = buyerChannelModel.ErrorFrom,
                RejectedField = buyerChannelModel.RejectedFieldName,
                RejectedValue = buyerChannelModel.RejectedValue,
                RejectedFrom = buyerChannelModel.RejectedFrom,
                TestField = buyerChannelModel.TestFieldName,
                TestValue = buyerChannelModel.TestValue,
                TestFrom = buyerChannelModel.TestFrom,
                RedirectField = buyerChannelModel.RedirectField,
                MessageField = buyerChannelModel.MessageField,
                PriceField = buyerChannelModel.PriceField,
                Delimeter = buyerChannelModel.Delimeter,
                PriceRejectField = buyerChannelModel.PriceRejectFieldName,
                PriceRejectValue = buyerChannelModel.PriceRejectValue,
                PostingUrl = buyerChannelModel.PostingUrl,
                DeliveryMethod = buyerChannelModel.DeliveryMethod,
                Timeout = buyerChannelModel.Timeout,
                AfterTimeout = buyerChannelModel.AfterTimeout,
                NotificationEmail = buyerChannelModel.NotificationEmail,
                AffiliatePrice = buyerChannelModel.AffiliatePrice,
                BuyerPrice = buyerChannelModel.BuyerPrice,
                CapReachedNotification = buyerChannelModel.IsCapReachedNotification,
                TimeoutNotification = buyerChannelModel.IsTimeoutNotification,
                OrderNum = buyerChannelModel.OrderNum,
                GroupNum = buyerChannelModel.GroupNum,
                IsFixed = buyerChannelModel.IsFixed,
                AllowedAffiliateChannels = buyerChannelModel.AllowedAffiliateChannels,
                DataFormat = buyerChannelModel.DataFormat,
                PostingHeaders = buyerChannelModel.PostingHeaders,
                BuyerPriceOption = buyerChannelModel.BuyerPriceOption,
                AffiliatePriceOption = buyerChannelModel.AffiliatePriceOption,
                AlwaysSoldOption = buyerChannelModel.TypeId,
                ZipCodeTargeting = buyerChannelModel.ZipCodeTargeting,
                StateTargeting = buyerChannelModel.StateTargeting,
                MinAgeTargeting = buyerChannelModel.MinAgeTargeting,
                MaxAgeTargeting = buyerChannelModel.MaxAgeTargeting,
                EnableZipCodeTargeting = buyerChannelModel.EnableZipCodeTargeting,
                EnableStateTargeting = buyerChannelModel.EnableStateTargeting,
                EnableAgeTargeting = buyerChannelModel.EnableAgeTargeting,
                ZipCodeCondition = buyerChannelModel.ZipCodeCondition,
                StateCondition = buyerChannelModel.StateCondition,
                Deleted = false,
                RedirectUrl = buyerChannelModel.RedirectUrl,
                MaxDuplicateDays = buyerChannelModel.MaxDuplicateDays,
                TimeZone = buyerChannelModel.TimeZone,
                TimeZoneStr = buyerChannelModel.TimeZoneStr,
                LeadAcceptRate = buyerChannelModel.LeadAcceptRate,
                SubIdWhiteListEnabled = buyerChannelModel.SubIdWhiteListEnabled,
                AccountIdField = buyerChannelModel.AccountIdField,
                EnableCustomPriceReject = buyerChannelModel.EnableCustomPriceReject,
                PriceRejectWinResponse = buyerChannelModel.PriceRejectWinResponse,
                FieldAppendEnabled = buyerChannelModel.FieldAppendEnabled,
                WinResponseUrl = buyerChannelModel.WinResponseUrl,
                WinResponsePostMethod = buyerChannelModel.WinResponsePostMethod,
                LeadIdField = buyerChannelModel.LeadId,
                ChildChannels = buyerChannelModel.ChildChannels,
                ResponseFormat = buyerChannelModel.ResponseFormat,
                ChannelMappingUniqueId = buyerChannelModel.ChannelMappingUniqueId,
                StatusStr = buyerChannelModel.StatusStr,
                StatusExpireDate = buyerChannelModel.StatusExpireDate,
                StatusAutoChange = buyerChannelModel.IsPauseChannel,
                StatusChangeMinutes = buyerChannelModel.PauseFor,
                ChangeStatusAfterCount = buyerChannelModel.PauseAfterTimeout,
                CurrentStatusChangeNum = null,
                DailyCap = buyerChannelModel.DailyCap,
                Note = buyerChannelModel.Note,
                CapReachEmailCount = null,
                CountryId = buyerChannelModel.CountryId,
                HolidayYear = buyerChannelModel.HolidayYear,
                HolidayAnnualAutoRenew = buyerChannelModel.HolidayAnnualAutoRenew,
                HolidayIgnore = buyerChannelModel.HolidayIgnore,
                ManagerId = buyerChannelModel.ManagerId,
                AlwaysBuyerPrice = buyerChannelModel.AlwaysBuyerPrice
            };
        }

        public void FillFilters(List<BuyerChannelFilterCondition> buyerChannelFilterConditions, List<CampaignField> campaignFields)
        {
            List<BuyerChannelFilterCreateModel> buyerChannelFilters = new List<BuyerChannelFilterCreateModel>();

            if (buyerChannelFilterConditions != null)
            {
                BuyerChannelFilterCreateModel buyerChannelFilterCreateModel = new BuyerChannelFilterCreateModel();

                Dictionary<long, BuyerChannelFilterCreateModel> keyValuePairs = new Dictionary<long, BuyerChannelFilterCreateModel>();

                foreach (var filter in buyerChannelFilterConditions.Where(x => !x.ParentId.HasValue || x.ParentId.Value == 0).ToList())
                {
                    buyerChannelFilterCreateModel = new BuyerChannelFilterCreateModel();
                    buyerChannelFilterCreateModel.CampaignFieldId = filter.CampaignTemplateId;
                    buyerChannelFilterCreateModel.Children = new List<BuyerChannelSubFilterModel>();
                    buyerChannelFilterCreateModel.Condition = filter.Condition;
                    buyerChannelFilterCreateModel.ParentId = 0;
                    buyerChannelFilterCreateModel.Values = buyerChannelFilterCreateModel.GetValues(filter.Value);

                    var campaignField = campaignFields.Where(x => x.Id == filter.CampaignTemplateId).FirstOrDefault();
                    if (campaignField != null)
                        buyerChannelFilterCreateModel.CampaignFieldName = campaignField.TemplateField;

                    buyerChannelFilters.Add(buyerChannelFilterCreateModel);
                    keyValuePairs[filter.Id] = buyerChannelFilterCreateModel;
                }

                foreach (long parentId in keyValuePairs.Keys)
                {
                    var buyerChannelFilterCreateParentModel = keyValuePairs[parentId];

                    foreach (var filter in buyerChannelFilterConditions.Where(x => x.ParentId.HasValue && x.ParentId.Value == parentId).ToList())
                    {
                        BuyerChannelSubFilterModel buyerChannelSubFilterModel = new BuyerChannelSubFilterModel();
                        buyerChannelSubFilterModel.CampaignFieldId = filter.CampaignTemplateId;
                        buyerChannelSubFilterModel.Condition = filter.Condition;
                        buyerChannelSubFilterModel.ParentId = parentId;
                        buyerChannelSubFilterModel.Values = buyerChannelFilterCreateModel.GetValues(filter.Value);

                        var campaignField = campaignFields.Where(x => x.Id == filter.CampaignTemplateId).FirstOrDefault();
                        if (campaignField != null)
                            buyerChannelSubFilterModel.CampaignFieldName = campaignField.TemplateField;

                        buyerChannelFilterCreateParentModel.Children.Add(buyerChannelSubFilterModel);
                    }
                }
            }

            this.BuyerChannelFilters = buyerChannelFilters;
        }

        public void FillFields(List<BuyerChannelTemplate> buyerChannelFields, List<BuyerChannelTemplateMatching> matchings)
        {
            this.BuyerChannelFields = new List<BuyerChannelIntegrationModel>();

            foreach(var field in buyerChannelFields)
            {
                List<BuyerChannelFieldMatchingModel> matchingModels = new List<BuyerChannelFieldMatchingModel>();

                var fieldMathings = matchings.Where(x => x.BuyerChannelTemplateId == field.Id).ToList();

                foreach(var matching in fieldMathings)
                {
                    matchingModels.Add(new BuyerChannelFieldMatchingModel()
                    {
                        BuyerChannelFieldId = field.Id,
                        InputValue = matching.InputValue,
                        OutputValue = matching.OutputValue
                    });
                }

                this.BuyerChannelFields.Add(new BuyerChannelIntegrationModel() { 
                     BuyerChannelFieldMatches = matchingModels,
                     CampaignFieldId = field.CampaignTemplateId,
                     DataFormatValues = field.DataFormatValues,
                     DefaultValue = field.DefaultValue,
                     Name = field.TemplateField,
                     SectionName = field.SectionName
                });
            }
        }

        public static explicit operator BuyerChannelModel(BuyerChannel buyerChannel)
        {
            return new BuyerChannelModel
            {
                Id = buyerChannel.Id,
                CampaignId = buyerChannel.CampaignId,
                BuyerId = buyerChannel.BuyerId,
                Name = buyerChannel.Name,
                Status = buyerChannel.Status,
                XmlTemplate = buyerChannel.XmlTemplate,
                SoldFieldName = buyerChannel.AcceptedField,
                SoldValue = buyerChannel.AcceptedValue,
                SoldFrom = buyerChannel.AcceptedFrom,
                ErrorFieldName = buyerChannel.ErrorField,
                ErrorValue = buyerChannel.ErrorValue,
                ErrorFrom = buyerChannel.ErrorFrom,
                RejectedFieldName = buyerChannel.RejectedField,
                RejectedValue = buyerChannel.RejectedValue,
                RejectedFrom = buyerChannel.RejectedFrom,
                TestFieldName = buyerChannel.TestField,
                TestValue = buyerChannel.TestValue,
                TestFrom = buyerChannel.TestFrom,
                RedirectField = buyerChannel.RedirectField,
                MessageField = buyerChannel.MessageField,
                PriceField = buyerChannel.PriceField,
                Delimeter = buyerChannel.Delimeter,
                PriceRejectFieldName = buyerChannel.PriceRejectField,
                PriceRejectValue = buyerChannel.PriceRejectValue,
                PostingUrl = buyerChannel.PostingUrl,
                DeliveryMethod = buyerChannel.DeliveryMethod,
                Timeout = buyerChannel.Timeout,
                AfterTimeout = buyerChannel.AfterTimeout,
                NotificationEmail = buyerChannel.NotificationEmail,
                AffiliatePrice = buyerChannel.AffiliatePrice,
                BuyerPrice = buyerChannel.BuyerPrice,
                IsCapReachedNotification = buyerChannel.CapReachedNotification,
                IsTimeoutNotification = buyerChannel.TimeoutNotification,
                OrderNum = buyerChannel.OrderNum,
                GroupNum = buyerChannel.GroupNum.HasValue ? buyerChannel.GroupNum.Value : 0,
                IsFixed = buyerChannel.IsFixed,
                AllowedAffiliateChannels = buyerChannel.AllowedAffiliateChannels,
                DataFormat = buyerChannel.DataFormat,
                PostingHeaders = buyerChannel.PostingHeaders,
                BuyerPriceOption = buyerChannel.BuyerPriceOption,
                AffiliatePriceOption = buyerChannel.AffiliatePriceOption,
                TypeId = buyerChannel.AlwaysSoldOption,
                ZipCodeTargeting = buyerChannel.ZipCodeTargeting,
                StateTargeting = buyerChannel.StateTargeting,
                MinAgeTargeting = buyerChannel.MinAgeTargeting,
                MaxAgeTargeting = buyerChannel.MaxAgeTargeting,
                EnableZipCodeTargeting = buyerChannel.EnableZipCodeTargeting,
                EnableStateTargeting = buyerChannel.EnableStateTargeting,
                EnableAgeTargeting = buyerChannel.EnableAgeTargeting,
                ZipCodeCondition = buyerChannel.ZipCodeCondition,
                StateCondition = buyerChannel.StateCondition,
                Deleted = buyerChannel.Deleted,
                RedirectUrl = buyerChannel.RedirectUrl,
                MaxDuplicateDays = buyerChannel.MaxDuplicateDays,
                TimeZone = buyerChannel.TimeZone,
                TimeZoneStr = buyerChannel.TimeZoneStr,
                LeadAcceptRate = buyerChannel.LeadAcceptRate,
                SubIdWhiteListEnabled = buyerChannel.SubIdWhiteListEnabled,
                AccountIdField = buyerChannel.AccountIdField,
                EnableCustomPriceReject = buyerChannel.EnableCustomPriceReject,
                PriceRejectWinResponse = buyerChannel.PriceRejectWinResponse,
                FieldAppendEnabled = buyerChannel.FieldAppendEnabled,
                WinResponseUrl = buyerChannel.WinResponseUrl,
                WinResponsePostMethod = buyerChannel.WinResponsePostMethod,
                LeadId = buyerChannel.LeadIdField,
                ChildChannels = buyerChannel.ChildChannels,
                ResponseFormat = buyerChannel.ResponseFormat,
                ChannelMappingUniqueId = buyerChannel.ChannelMappingUniqueId,
                StatusStr = buyerChannel.StatusStr,
                StatusExpireDate = buyerChannel.StatusExpireDate,
                IsPauseChannel = buyerChannel.StatusAutoChange ?? false,
                PauseFor = buyerChannel.StatusChangeMinutes ?? 0,
                PauseAfterTimeout = buyerChannel.ChangeStatusAfterCount ?? 0,
                DailyCap = buyerChannel.DailyCap,
                Note = buyerChannel.Note,
                BuyerChannelFilters = new List<BuyerChannelFilterCreateModel>(),
                CountryId = buyerChannel.CountryId.HasValue ? buyerChannel.CountryId.Value : 0,
                HolidayYear = buyerChannel.HolidayYear,
                HolidayAnnualAutoRenew = buyerChannel.HolidayAnnualAutoRenew,
                HolidayIgnore = buyerChannel.HolidayIgnore,
                AlwaysBuyerPrice = buyerChannel.AlwaysBuyerPrice.HasValue ? buyerChannel.AlwaysBuyerPrice.Value : false
            };
        }

        public BuyerChannelModel()
        {
            BuyerChannelSchedules = new List<BuyerChannelScheduleDayModel>();
        }
    }
}